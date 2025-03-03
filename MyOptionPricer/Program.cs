using System;
using System.IO;

namespace MyOptionPricer
{
    class Program
    {
        static void Main(string[] args)
        {
            string myBanner = File.ReadAllText("ascii_art.txt");
         
            Console.WriteLine(myBanner);
            Console.WriteLine(new string('-', 100));

            // [ ] Objectif avoir une interfasse ou l'on entre le symbol la date d'expiration et le prix d'exercice
            // [ ] On affiche le prix de l'option call ou put
            

            


            



            // Paramètres de l'option
            double spotPrice = 100.0;       // Prix actuel de l'actif sous-jacent (S)
            double strikePrice = 95.0;      // Prix d'exercice (K)
            double riskFreeRate = 0.05;     // Taux sans risque (r)
            double volatility = 0.2;        // Volatilité (sigma)
            double timeToMaturity = 1.0;    // Temps jusqu'à l'expiration en années (T)
            int accuracy = 100;

            // Création de l'objet BlackSholes
            Binomial Bin = new Binomial(spotPrice, strikePrice, riskFreeRate, volatility, timeToMaturity, accuracy);

            // Calcul du prix de l'option call
            double callPrice = Bin.Compute_Option(true);
            Console.WriteLine($"Prix de l'option call : {callPrice:F4}");

            // Calcul du prix de l'option put
            double putPrice =  Bin.Compute_Option(false);
            Console.WriteLine($"Prix de l'option put : {putPrice:F4}");
        }
    }
}