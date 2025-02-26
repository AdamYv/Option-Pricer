using System;
using MathNet.Numerics.Distributions;

namespace MyOptionPricer
{
    class Program
    {
        static void Main(string[] args)
        {
        double mean = 0.0; // Moyenne (mu)
        double stdDev = 1.0; // Écart-type (sigma)
            double x = 1.0; // Valeur pour laquelle on veut la CDF

        double cdf = Normal.CDF(mean, stdDev, x);
        Console.WriteLine($"La CDF de N({mean}, {stdDev}²) pour x = {x} est : {cdf}");
        }
    }
}