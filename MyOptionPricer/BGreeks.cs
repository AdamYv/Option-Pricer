using System;

namespace MyOptionPricer
{
    /// <summary>
    /// Grecs par différences finies centrées (« bump and reprice »).
    /// Utilisé pour les options sans formule fermée (américaine : arbre binomial,
    /// asiatique : Monte-Carlo avec nombres aléatoires communs).
    /// Conventions identiques aux formules fermées de l'option européenne :
    /// Theta par année, Vega pour une variation de 1 (100 %) de la volatilité,
    /// Rho pour une variation de 1 (100 %) du taux.
    /// </summary>
    public static class FiniteDifferenceGreeks
    {
        /// <param name="price">Prix en fonction de (S, sigma, r, T).</param>
        public static (double Delta, double Gamma, double Theta, double Vega, double Rho) Compute(
            Func<double, double, double, double, double> price,
            double S, double sigma, double r, double T)
        {
            double hS = 0.01 * S;           // choc de 1 % sur le sous-jacent
            double hSigma = 0.01;           // choc de 1 point de volatilité
            double hR = 0.0001;             // choc de 1 point de base
            double hT = Math.Min(1.0 / 365.0, T / 2.0); // un jour

            double v0 = price(S, sigma, r, T);
            double vUp = price(S + hS, sigma, r, T);
            double vDown = price(S - hS, sigma, r, T);

            double delta = (vUp - vDown) / (2 * hS);
            double gamma = (vUp - 2 * v0 + vDown) / (hS * hS);
            double vega = (price(S, sigma + hSigma, r, T) - price(S, sigma - hSigma, r, T)) / (2 * hSigma);
            double rho = (price(S, sigma, r + hR, T) - price(S, sigma, r - hR, T)) / (2 * hR);
            // Theta = dV/dt = -dV/dT : valeur de l'option un jour plus tard, à paramètres constants
            double theta = (price(S, sigma, r, T - hT) - v0) / hT;

            return (delta, gamma, theta, vega, rho);
        }
    }
}
