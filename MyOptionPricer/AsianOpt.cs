using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MyOptionPricer
{


    public class MonteCarloAsianPricer
    {
        public async Task<double> PriceAsianOption(
            string symbol,
            double strike,
            double riskFreeRate,
            double maturityYears,
            int simulations = 10000,
            int timeSteps = 252)
        {
            var historicalData = await AlphaVanAPI.GetHistoricalData(symbol);
            var volatility = AlphaVanAPI.CalculateVolatility(historicalData);
            var spotPrice = historicalData.Last().Close;

            var rand = new Random();
            double totalPayoff = 0;
            double dt = maturityYears / timeSteps;
            double drift = (riskFreeRate - 0.5 * Math.Pow(volatility, 2)) * dt;

            for (int i = 0; i < simulations; i++)
            {
                var path = GeneratePath(spotPrice, drift, volatility, dt, timeSteps, rand);
                totalPayoff += Math.Max(path.Average() - strike, 0);
            }

            return Math.Exp(-riskFreeRate * maturityYears) * (totalPayoff / simulations);
        }

        private double[] GeneratePath(double S0, double drift, double vol, double dt, int steps, Random rand)
        {
            var path = new double[steps];
            double current = S0;

            for (int i = 0; i < steps; i++)
            {
                double dW = Math.Sqrt(dt) * NormInv(rand.NextDouble());
                current *= Math.Exp(drift + vol * dW);
                path[i] = current;
            }
            return path;
        }

        private double NormInv(double p)
        {
            // Algorithme de Moro pour transformation inverse normale
            double[] a = { 2.50662823884, -18.61500062529, 41.39119773534, -25.44106049637 };
            double[] b = { -8.47351093090, 23.08336743743, -21.06224101826, 3.13082909833 };
            double[] c = { 0.3374754822726147, 0.9761690190917186, 0.1607979714918209, 0.0276438810333863,
                          0.0038405729373609, 0.0003951896511919, 0.0000321767881768, 0.0000002888167364,
                          0.0000003960315187 };

            if (p <= 0 || p >= 1) throw new ArgumentException("p doit être entre 0 et 1");

            double x = p - 0.5;
            if (Math.Abs(x) < 0.42)
            {
                double y = x * x;
                return x * (((a[3] * y + a[2]) * y + a[1]) * y + a[0]) /
                        ((((b[3] * y + b[2]) * y + b[1]) * y + b[0]) * y + 1.0);
            }
            else
            {
                double y = Math.Log(-Math.Log(p < 0.5 ? p : 1 - p));
                double num = c[0];
                for (int i = 1; i < c.Length; i++) num += c[i] * Math.Pow(y, i);
                return (p < 0.5 ? -num : num);
            }
        }
    }


}
