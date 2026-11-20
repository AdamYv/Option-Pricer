using System;
using MathNet.Numerics.Distributions;

namespace MyOptionPricer
{
    public class AmericanOption : OptionPricer
    {
        private readonly int _n;   // Nombre d'etapes de l'arbre
        private readonly double _q; // Taux de dividende

        public AmericanOption(double S, double K, double T, double sigma, double r, int n, double q)
            : base(S, K, r, sigma, T)
        {
            _n = n;
            _q = q;
        }

        public override double CallPrice() => ComputeOption(isCall: true);
        public override double PutPrice() => ComputeOption(isCall: false);

        /// <summary>
        /// Grecs par différences finies sur l'arbre binomial (même nombre d'étapes).
        /// Theta par année, Vega et Rho pour une variation de 1 (100 %).
        /// </summary>
        public (double Delta, double Gamma, double Theta, double Vega, double Rho) ComputeGreeks(bool isCall)
        {
            double Price(double s, double vol, double rate, double t)
            {
                var bumped = new AmericanOption(s, K, t, vol, rate, _n, _q);
                return isCall ? bumped.CallPrice() : bumped.PutPrice();
            }

            return FiniteDifferenceGreeks.Compute(Price, S, sigma, r, T);
        }

        private double ComputeOption(bool isCall)
        {
            double deltaT = T / _n;
            double up = ComputeUp();
            double p0 = ComputeRiskNeutralProbability(up, deltaT);
            double p1 = Math.Exp(-r * deltaT) - p0;

            double[] prices = new double[_n + 1];

            // Initialisation � l'expiration
            for (int i = 0; i <= _n; i++)
            {
                double stockPrice = S * Math.Pow(up, 2 * i - _n);
                prices[i] = isCall
                    ? Math.Max(stockPrice - K, 0.0)
                    : Math.Max(K - stockPrice, 0.0);
            }

            // Induction � rebours
            for (int j = _n - 1; j >= 0; j--)
            {
                for (int i = 0; i <= j; i++)
                {
                    prices[i] = p0 * prices[i + 1] + p1 * prices[i];
                    double stockPrice = S * Math.Pow(up, 2 * i - j);
                    double exercise = isCall ? stockPrice - K : K - stockPrice;
                    prices[i] = Math.Max(prices[i], exercise);
                }
            }

            return prices[0];
        }

        private double ComputeUp()
        {
            double deltaT = T / _n;
            return Math.Exp(sigma * Math.Sqrt(deltaT));
        }

        private double ComputeRiskNeutralProbability(double up, double deltaT)
        {
            double expMinusQDeltaT = Math.Exp(-_q * deltaT);
            double expMinusRDeltaT = Math.Exp(-r * deltaT);
            return (up * expMinusQDeltaT - expMinusRDeltaT) / (up * up - 1);
        }
    }
}