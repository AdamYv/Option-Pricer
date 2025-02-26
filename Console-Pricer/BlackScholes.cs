using System;
using MathNet.Numerics.Distributions;

public class BlackSholes {
    //[ ]: Add the necessary attributes
    public double S;
    public double K;
    public double r;
    public double sigma;
    public double T;

    public double call_BS(){
        float callP=0;

        return callP;
        
    }

    public double put_BS(){
        // Black-Scholes formula for put option
        return 0;
    }

    public double Function_Cumul(){
        // Cumulative distribution functionattributes

        return 0;
    }

    public double D_ONE(){
        double d1 = Math.Log(this.S, this.K) + (this.r + Math.Pow(this.sigma, 2) / 2) * this.T;
        d1 /= this.sigma * Math.Sqrt(this.T);
        return d1;
    }

    public double D_TWO(){
        double d2 = this.D_ONE() - this.sigma * Math.Sqrt(this.T);
        return d2;
    }


}