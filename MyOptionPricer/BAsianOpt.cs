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

    private AsianOpt(double s0, double k, double r, double sigma, double t, int steps, int sims, AverageType avgType)
    {
        S0 = s0;
        K = k;
        R = r;
        Sigma = sigma;
        T = t;
        TimeSteps = steps;
        Simulations = sims;
        AvgType = avgType;
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

    public double CallPrice() => MonteCarlo(isCall: true);
    public double PutPrice() => MonteCarlo(isCall: false);

    private double MonteCarlo(bool isCall)
    {
        double dt = T / TimeSteps;
        double totalPayoff = 0.0;
        object lockObj = new object();

        Parallel.For(0, Simulations,
            () => new LocalState { Random = new Random(), Payoff = 0.0 },
            (sim, state, local) =>
            {
                double S = S0;
                var path = new double[TimeSteps];

                for (int j = 0; j < TimeSteps; j++)
                {
                    double Z = local.Random.NextGaussian();
                    S *= Math.Exp((R - 0.5 * Sigma * Sigma) * dt + Sigma * Math.Sqrt(dt) * Z);
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

        return Math.Round(Math.Exp(-R * T) * (totalPayoff / Simulations), 4);
    }

    private class LocalState
    {
        public Random Random { get; set; }
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