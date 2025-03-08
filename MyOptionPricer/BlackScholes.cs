using System;
using MathNet.Numerics.Distributions;
namespace MyOptionPricer{

public class BlackSholes {

    //[x]: Add the necessary attributes simplify the code reading
    public double S { get; private set; }  
        public double K { get; private set; }  
        public double r { get; private set; }  
        public double sigma { get; private set; }  
        public double T { get; private set; }  


    public BlackSholes(double S, double K, double r, double sigma, double T){
        this.S = S;
        this.K = K;
        this.r = r;
        this.sigma = sigma;
        this.T = T;
    }

    public double Compute_Option(bool isCall){
        if(isCall){
            return Call_BS();
        }
        else{
            return Put_BS();
        }
    }

    private double Call_BS(){
        // Compute D1 and D2
        double sqrT = Math.Sqrt(T);
        double d1 = (Math.Log(S /K)+(r+sigma*sigma/2.0)*T) /sigma * sqrT;

        double d2 = d1 - sigma * sqrT;

        //For the normal Law we use N(0,1, x) to get the cumulative distribution function 
        double N_d1 = Normal.CDF(0.0, 1.0, d1);
        double N_d2 = Normal.CDF(00, 1.0, d2);

        return S * N_d1 - K * Math.Exp(- r * T) * N_d2;
        
    }

    private double Put_BS(){
        double sqrT = Math.Sqrt(T);
        double d1 = (Math.Log(S /K)+(r+sigma*sigma/2.0)*T)/sigma * sqrT;

        double d2 = d1 - sigma * sqrT;

        double N_d1 = Normal.CDF(0.0, 1.0, -d1);
        double N_d2 = Normal.CDF(0.0, 1.0, -d2);

        return K * Math.Exp(- r * T) * N_d2 - S * N_d1;
    }
    



}
}