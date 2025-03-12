using System;
using MathNet.Numerics.Distributions;

namespace MyOptionPricer
{
    public class Binomial(double S, double K, double T, double sigma, double r, int n, double q)
    {
        private double Compute_Up()
        {
            double deltaT = T / n;
            return Math.Exp(sigma * Math.Sqrt(deltaT));
        }

        private double Compute_Down()
        {
            return 1.0 / Compute_Up();
        }

        private double computeRiskNeutralProbability()
        {
            double deltaT = T / n;
            double up = Compute_Up();
            double expMinusQDeltaT = Math.Exp(-q * deltaT);
            double expMinusRDeltaT = Math.Exp(-r * deltaT);
            double numerator = up * expMinusQDeltaT - expMinusRDeltaT;
            double denominator = up * up - 1;
            return numerator / denominator;
        }

        private double[,] createTree()
        {
            // This method is part of the skeleton but not used in the current implementation.
            // Returning a placeholder to maintain the structure.
            return new double[0, 0];
        }

        public double Compute_Option(bool isCall)
        {
            double deltaT = T / n;
            double up = Compute_Up();
            double p0 = computeRiskNeutralProbability();
            double p1 = Math.Exp(-r * deltaT) - p0;

            double[] p = new double[n + 1];

            // Initialize option values at expiration
            for (int i = 0; i <= n; i++)
            {
                int exponent = 2 * i - n + 1;
                double stockPrice = S * Math.Pow(up, exponent);
                p[i] = isCall ? Math.Max(stockPrice - K, 0.0) : Math.Max(K - stockPrice, 0.0);
            }

            // Backward induction to compute option price
            for (int j = n - 1; j >= 0; j--)
            {
                for (int i = 0; i <= j; i++)
                {
                    // Compute binomial value
                    p[i] = p0 * p[i + 1] + p1 * p[i];

                    // Compute exercise value
                    int exponent = 2 * i - j + 1;
                    double stockPrice = S * Math.Pow(up, exponent);
                    double exercise = isCall ? (stockPrice - K) : (K - stockPrice);

                    // American option: take maximum of binomial value and exercise value
                    p[i] = Math.Max(p[i], exercise);
                }
            }

            return p[0];
        }

        public double CallPrice()
        {
            return Compute_Option(true);
        }

        public double PutPrice()
        {
            return Compute_Option(false);
        }
    }
}