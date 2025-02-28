using System;

namespace MyOptionPricer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenue dans le calculateur de prix d'options Black-Scholes !");

            // Paramètres de l'option
            double spotPrice = 100.0;       // Prix actuel de l'actif sous-jacent (S)
            double strikePrice = 95.0;      // Prix d'exercice (K)
            double riskFreeRate = 0.05;     // Taux sans risque (r)
            double volatility = 0.2;        // Volatilité (sigma)
            double timeToMaturity = 1.0;    // Temps jusqu'à l'expiration en années (T)

            // Création de l'objet BlackSholes
            BlackSholes black = new BlackSholes(spotPrice, strikePrice, riskFreeRate, volatility, timeToMaturity);

            // Calcul du prix de l'option call
            double callPrice = black.Call_BS();
            Console.WriteLine($"Prix de l'option call : {callPrice:F4}");

            // Calcul du prix de l'option put
            double putPrice = black.Put_BS();
            Console.WriteLine($"Prix de l'option put : {putPrice:F4}");
        }
    }
}