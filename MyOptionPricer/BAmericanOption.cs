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

        public override double CallPrice() => ComputeOption(isCall: true, out _, out _);
        public override double PutPrice() => ComputeOption(isCall: false, out _, out _);

        /// <summary>
        /// Grecs de l'option américaine. Delta, Gamma et Theta sont lus sur les
        /// premiers noeuds de l'arbre (Hull) : un choc sur le sous-jacent déplacerait
        /// la grille de l'arbre et rendrait Gamma très bruité. Vega et Rho sont
        /// obtenus par chocs centrés, à nombre d'étapes constant.
        /// Theta par année, Vega et Rho pour une variation de 1 (100 %).
        /// </summary>
        public (double Delta, double Gamma, double Theta, double Vega, double Rho) ComputeGreeks(bool isCall)
        {
            double Price(double s, double vol, double rate, double t)
            {
                var bumped = new AmericanOption(s, K, t, vol, rate, _n, _q);
                return isCall ? bumped.CallPrice() : bumped.PutPrice();
            }

            var fd = FiniteDifferenceGreeks.Compute(Price, S, sigma, r, T);
            if (_n < 2)
                return fd; // arbre trop court pour lire les noeuds

            double price = ComputeOption(isCall, out double[] level1, out double[] level2);
            double dt = T / _n;
            double up = ComputeUp();
            double down = 1.0 / up;

            double delta = (level1[1] - level1[0]) / (S * up - S * down);
            double gammaUp = (level2[2] - level2[1]) / (S * up * up - S);
            double gammaDown = (level2[1] - level2[0]) / (S - S * down * down);
            double gamma = (gammaUp - gammaDown) / (0.5 * (S * up * up - S * down * down));
            double theta = (level2[1] - price) / (2 * dt);

            return (delta, gamma, theta, fd.Vega, fd.Rho);
        }

        private double ComputeOption(bool isCall, out double[] level1, out double[] level2)
        {
            level1 = Array.Empty<double>();
            level2 = Array.Empty<double>();

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

                // Valeurs aux étapes 1 et 2, utilisées pour les Grecs
                if (j == 2) level2 = new[] { prices[0], prices[1], prices[2] };
                if (j == 1) level1 = new[] { prices[0], prices[1] };
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