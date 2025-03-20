using System;

namespace MyOptionPricer
{
    public abstract class OptionPricer
    {
        public double S { get; }  // Prix actuel de l'action
        public double K { get; }  // Prix d'exercice
        public double r { get; }  // Taux d'intérêt sans risque
        public double sigma { get; }  // Volatilité
        public double T { get; }  // Temps jusqu'à l'expiration (en années)

        protected OptionPricer(double S, double K, double r, double sigma, double T)
        {
            this.S = S;
            this.K = K;
            this.r = r;
            this.sigma = sigma;
            this.T = T;
        }

        public abstract double CallPrice();
        public abstract double PutPrice();
    }
}