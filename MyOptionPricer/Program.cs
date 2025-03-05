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
            Console.WriteLine(new string('=', 80));

            // [ ] Objectif avoir une interfasse ou l'on entre le symbol la date d'expiration et le prix d'exercice
            // [ ] On affiche le prix de l'option call ou put
            

            // Paramètres de l'option
            double spotPrice = 20.0;       //  (S)
            double strikePrice = 21.0;      //  (K)
            double timeToMaturity = 0.50;    //  (T)
            double riskFreeRate = 0.12;     //  (r)
            double DividendYield = 0.0;     //  (q)
            double volatility = 0.2;        //  (sigma)
            int accuracy = 2;              //  (n)

            // Création de l'objet BlackSholes
            var Bin = new Binomial(spotPrice, strikePrice, timeToMaturity, volatility, riskFreeRate, accuracy, DividendYield );
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