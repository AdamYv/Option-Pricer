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
            double spotPrice = 100.0;       //  (S)
            double strikePrice = 95.0;      //  (K)
            double riskFreeRate = 0.05;     //  (r)
            double volatility = 0.2;        //  (sigma)
            double timeToMaturity = 1.0;    //  (T)
            int accuracy = 20;              //  (n)
            double DividendYield = 0.0;     //  (q)

            // Création de l'objet BlackSholes
            var Bin = new Binomial(spotPrice, strikePrice, riskFreeRate, volatility, timeToMaturity, accuracy,DividendYield );
            Console.WriteLine($"Calcul du prix de l'option avec le modèle binomial à {accuracy} étapes");

            Console.WriteLine($"K: {strikePrice}, S: {spotPrice}, r: {riskFreeRate}, sigma: {volatility}, T: {timeToMaturity}");

            // Calcul du prix de l'option call
            double callPrice = Bin.Compute_Option(true);
            Console.WriteLine($"Prix de l'option call : {callPrice:F4}");

            // Calcul du prix de l'option put
            double putPrice =  Bin.Compute_Option(false);
            Console.WriteLine($"Prix de l'option put : {putPrice:F4}");
        }
    }
}