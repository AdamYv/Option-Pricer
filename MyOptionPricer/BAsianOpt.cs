using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class AsianOpt
{
    public enum AverageType { Arithmetic } // J'ai retire geometric ici 

    // Paramètres financiers
    public double S0 { get; }
    public double K { get; }
    public double R { get; }
    public double Sigma { get; }
    public double T { get; }

    // Paramètres de simulation
    public int TimeSteps { get; }
    public int Simulations { get; }
    public AverageType AvgType { get; }

    // Graine des tirages : le prix et les Grecs réutilisent les mêmes trajectoires
    // (nombres aléatoires communs), ce qui stabilise les différences finies.
    public int Seed { get; }

    public AsianOpt(double s0, double k, double r, double sigma, double t, int steps, int sims,
                    AverageType avgType = AverageType.Arithmetic, int? seed = null)
    {
        S0 = s0;
        K = k;
        R = r;
        Sigma = sigma;
        T = t;
        TimeSteps = steps;
        Simulations = sims;
        AvgType = avgType;
        Seed = seed ?? Random.Shared.Next();
    }

    public static async Task<AsianOpt> CreateAsync(
        string symbol,
        int daysHistorical,
        double strike,
        double maturityYears,
        double riskFreeRate,
        int simulations,
        AverageType avgType = AverageType.Arithmetic,
        double? impliedVol = null)
    {
        var data = await AlphaVanAPI.GetHistoricalData(symbol, daysHistorical);
        if (data == null || !data.Any())
            throw new ArgumentException("Erreur API : données historiques non disponibles");

        double s0 = data[0].Close; // Dernière clôture
        double sigma = impliedVol ?? AlphaVanAPI.CalculateVolatility(data);
        int steps = (int)(maturityYears * 252); // 252 jours/an

        return new AsianOpt(s0, strike, riskFreeRate, sigma, maturityYears, steps, simulations, avgType);
    }

    public double CallPrice() => Math.Round(MonteCarlo(true, S0, Sigma, R, T), 4);
    public double PutPrice() => Math.Round(MonteCarlo(false, S0, Sigma, R, T), 4);

    /// <summary>
    /// Grecs par différences finies sur le Monte-Carlo, avec les mêmes tirages
    /// pour chaque prix choqué. Le nombre de pas de temps reste fixe.
    /// Theta par année, Vega et Rho pour une variation de 1 (100 %).
    /// </summary>
    public (double Delta, double Gamma, double Theta, double Vega, double Rho) ComputeGreeks(bool isCall)
    {
        return MyOptionPricer.FiniteDifferenceGreeks.Compute(
            (s, vol, rate, t) => MonteCarlo(isCall, s, vol, rate, t),
            S0, Sigma, R, T);
    }

    private double MonteCarlo(bool isCall, double s0, double sigma, double r, double t)
    {
        double dt = t / TimeSteps;
        double totalPayoff = 0.0;
        object lockObj = new object();

        Parallel.For(0, Simulations,
            () => new LocalState { Payoff = 0.0 },
            (sim, state, local) =>
            {
                // Un générateur par trajectoire, dérivé de la graine : résultat
                // identique quel que soit le découpage entre threads
                var random = new Random(unchecked(Seed * 1_000_003 + sim));
                double S = s0;
                var path = new double[TimeSteps];

                for (int j = 0; j < TimeSteps; j++)
                {
                    double Z = random.NextGaussian();
                    S *= Math.Exp((r - 0.5 * sigma * sigma) * dt + sigma * Math.Sqrt(dt) * Z);
                    path[j] = S;
                }

                double avg = (AvgType == AverageType.Arithmetic) ?
                    path.Average() :
                    Math.Exp(path.Sum(x => Math.Log(x)) / path.Length);

                local.Payoff += isCall ?
                    Math.Max(avg - K, 0) :
                    Math.Max(K - avg, 0);

                return local;
            },
            local =>
            {
                lock (lockObj)
                    totalPayoff += local.Payoff;
            });

        return Math.Exp(-r * t) * (totalPayoff / Simulations);
    }

    private class LocalState
    {
        public double Payoff { get; set; }
    }
}

public static class RandomExtensions
{
    public static double NextGaussian(this Random rand)
    {
        double u1 = 1.0 - rand.NextDouble();
        double u2 = 1.0 - rand.NextDouble();
        return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
    }
}