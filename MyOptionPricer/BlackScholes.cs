using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace MyOptionPricer
{
	public class BlackSholes : OptionPricer
	{
		public BlackSholes(double S, double K, double r, double sigma, double T)
			: base(S, K, r, sigma, T)
		{
		}

		public override double CallPrice()
		{
			double sqrT = Math.Sqrt(T);
			double d1 = (Math.Log(S / K) + (r + sigma * sigma / 2.0) * T) / (sigma * sqrT);
			double d2 = d1 - sigma * sqrT;

			double N_d1 = Normal.CDF(0.0, 1.0, d1);
			double N_d2 = Normal.CDF(0.0, 1.0, d2);

			return S * N_d1 - K * Math.Exp(-r * T) * N_d2;
		}

		public override double PutPrice()
		{
			double sqrT = Math.Sqrt(T);
			double d1 = (Math.Log(S / K) + (r + sigma * sigma / 2.0) * T) / (sigma * sqrT);
			double d2 = d1 - sigma * sqrT;

			double N_d1 = Normal.CDF(0.0, 1.0, -d1);
			double N_d2 = Normal.CDF(0.0, 1.0, -d2);

			return K * Math.Exp(-r * T) * N_d2 - S * N_d1;
		}

        protected double ComputeD1()
        {
            return (Math.Log(S / K) + (r + sigma * sigma / 2.0) * T) / (sigma * Math.Sqrt(T));

        }

        protected double ComputeD2()
        {
            return ComputeD1() - sigma * Math.Sqrt(T);
        }
    }

	public class Greeks : BlackSholes
	{
        private readonly double d1;
        private readonly double d2;

		public Greeks(double S, double K, double r, double sigma, double T) : base(S, K, r, sigma, T)
		{
			this.d1 = ComputeD1();
			this.d2 = ComputeD2(); 
		}

        private double ComputeDeltaCall()
		{

			return Normal.CDF(0.0, 1.0, d1 );
		}
		
		private double ComputeDeltaPut()
		{

			return ComputeDeltaCall()-1;
		}
		
		private double ComputeGamma()
		{

			return Normal.PDF(0.0,1.0,d1) / (S * sigma * Math.Sqrt(T));
		}

		private double ComputeVega()
		{
			return S*Normal.PDF(0.0,1.0,d1) * Math.Sqrt(T) ;
		}

		private double ComputeThetaCall()
		{

			double FiPart = -(S * Normal.PDF(0.0, 1.0, d1) * sigma / (2 * Math.Sqrt(T))) ;
			double SecPart = -r * K * Math.Exp(-r * T) * Normal.CDF(0.0,1.0,d2);

			return FiPart + SecPart;
		
		}

        private double ComputeThetaPut()
        {

            double FiPart = -(S * Normal.PDF(0.0, 1.0, d1) * sigma / (2 * Math.Sqrt(T)));
            double SecPart = +r * K * Math.Exp(-r * T) * Normal.CDF(0.0, 1.0, -d2);

            return FiPart + SecPart;

        }

		private double ComputeRhoCall()
		{
			return K * T * Math.Exp(-r * T)*Normal.CDF(0.0,1.0,d2);
		}
		
		private double ComputeRhoPut()
		{
            return -K * T * Math.Exp(-r * T) * Normal.CDF(0.0, 1.0, -d2);
        }

        public (double Delta, double Gamma , double Theta ,double Vega, double Rho)ComputeGreeks(bool Call)
		{
			if (Call)
			{
				return (ComputeDeltaCall() ,ComputeGamma() , ComputeVega(), ComputeThetaCall(), ComputeRhoCall());

            }
			else
			{
				return (ComputeDeltaPut() ,ComputeGamma() , ComputeVega(), ComputeThetaPut(), ComputeRhoPut());
			}
		}




    }

}