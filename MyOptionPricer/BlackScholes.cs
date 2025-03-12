using MathNet.Numerics.Distributions;
using System;

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
	}
}