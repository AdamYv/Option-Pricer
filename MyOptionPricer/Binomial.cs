using System;
using System.Linq.Expressions;
using MathNet.Numerics.Distributions;

namespace MyOptionPricer
{
    class Binomial
    {
        public double S {get; private set;} // current stock price
        double K ; // strike price
        double T ; // time to maturity
        double sigma ; // volatility
        double r ; // risk-free rate
        // [ ] We will use function that check the convergence of the binomial model to stop the step
        int n ; // number of steps
        int dT ; // time step
        
        
        //constructor
        public Binomial(double S, double K, double T, double sigma, double r, int n) {
            this.S = S;
            this.K = K;
            this.T = T;
            this.sigma = sigma;
            this.r = r;
            this.n = n; 
            this.dT = (int) Math.Round(T / n);
        }


        private double Compute_Up() {

            return Math.Exp(sigma * Math.Sqrt(T / dT));

        }

        private double Compute_Down() {
            return 1 / Compute_Up();
        }

        private double Compute_Risk_Neutral_Probability() {
            return (Math.Exp(r * T / dT) - Compute_Down()) / (Compute_Up() - Compute_Down());
        }

        private double[,] Create_Tree(){
            double u = Compute_Up();
            double d = Compute_Down();

            double[,] tree = new double [n+1, n+1];

            tree[0, 0] = S;

            for (int i =1; i<= n ; i++){
                for (int j = 1; j <= i; j++){
                    tree[i, j] = S * Math.Pow(u, j) * Math.Pow(d, i-j);
                }
            }
            return tree;
        }

        public double Compute_Option(bool isCall){
            double df = Math.Exp(-r * T / n); // update factor
            double u = Compute_Up();
            double d = Compute_Down();
            double p = Compute_Risk_Neutral_Probability();


            double[,] tree = Create_Tree();
            double[,] option = new double[n+1, n+1];




            for (int j = 0; j <= n; j++){
                //Compute the payoff at maturity like activation function
                option[n, j] = Math.Max(0, (isCall ? 1 : -1) * (tree[n, j] - K));
            }


            for (int i = n-1; i >= 0; i--){
                for (int j = 0; j <= i; j++){
                    double exerciseValue = isCall ? Math.Max(0, tree[i, j] - K) : Math.Max(0, K - tree[i, j]);
                    double holdValue = df * (p * option[i + 1, j + 1] + (1 - p) * option[i + 1, j]);
                    option[i, j] = Math.Max(exerciseValue, holdValue);  // Choix optimal
                    option[i, j] = Math.Exp(-r * T / n) * (p * option[i+1, j+1] + (1-p) * option[i+1, j]);
                }
            }

            return option[0, 0];
        }


    }

}
