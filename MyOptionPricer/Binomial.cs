using System;

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
        double dT ; // time during each step

        double q ; // Dividend yield
        
        
        //STEP 0
        public Binomial(double S, double K, double T, double sigma, double r, int n, double q ){
            this.S = S;
            this.K = K;
            this.T = T;
            this.r = r;
            this.q = q;
            this.sigma = sigma;
            this.n = n; 
            this.dT = T/n;
        }


        private double computeUp() {
            return Math.Exp(sigma * Math.Sqrt(dT));

        }

        private double computeDown() {
            return 1 / computeUp();
        }

        private double computeRiskNeutralProbability() {
        
            double a = Math.Exp((r - q) * dT)- computeDown();
            double p = a / (computeUp() - computeDown());
            return p;
        }

        //STEP 1 : Init the tree
        private double[,] createTree()
        {
            
            double u = computeUp();   // Facteur d'augmentation
            double d = computeDown(); // Facteur de diminution
            double[,] tree = new double[n + 1, n + 1]; // Arbre de taille (n+1) x (n+1)
            // Initialisation du premier élément (prix initial)
            for (int i = 0; i <= n; i++)
            {
                tree[i, 0] = S * Math.Pow(d, i); // Remplit la première colonne (descente pure)

                for (int j = 1; j <= i; j++)
                {
                    tree[i, j] = tree[i, j - 1] * u / d; // Génère les valeurs successives
                }
            }
            
            return tree;
}




        public double Compute_Option(bool isCall){
            double df = Math.Exp(-r * T / n); // update factor
            double p = computeRiskNeutralProbability();
            double[,] tree = createTree();
            double[,] optionTree = new double[n+1, n+1];



            //ETAPE 2:Compute the option price at the end of each node
            for (int j = 0; j <= n; j++){
                optionTree[n, j] = Math.Max(0, (isCall ? 1 : -1) * (tree[n, j] - K));
            }
            
            //ETAPE 3:Compute the option price at the last node 
            for (int i = n-1; i >= 0; i--){
                
                for (int j = 0; j <= i; j++){
                    double exerciseValue = isCall ? Math.Max(0, tree[i, j] - K) : Math.Max(0, K - tree[i, j]);
                    double holdValue = df * (p * optionTree[i + 1, j + 1] + (1 - p) * optionTree[i + 1, j]);

                    optionTree[i, j] = Math.Max(exerciseValue, holdValue);  // Choix optimal
                    
                }
            }

            return optionTree[0, 0];
        }


    }

}
