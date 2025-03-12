using System;
using System.IO;

namespace MyOptionPricer
{
    class Program
    {
        static void Main()
        {
            

            // Obtenir les paramètres de l'option via l'interface console
            var (SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity, Accuracy, DividendeRate) = ConsoleInterface.GetOptionParametersFromConsole();

            // Création de l'objet Binomial pour l'option américaine
            Binomial AmericanOption = new (
                SpotPrice,
                StrikePrice,
                TimeToMaturity,
                Volatility,
                RiskFreeRate,
                Accuracy,
                DividendeRate
            //120,100,0.20,0.5,1,10,0.3


            );

            // Calcul du prix de l'option call
            double cPriceAmer = AmericanOption.CallPrice();
            Console.WriteLine($"Prix de l'option d'achat (call) américaine : {cPriceAmer:F2}");

            // Calcul du prix de l'option put
            double pPriceAmer = AmericanOption.PutPrice();
            Console.WriteLine($"Prix de l'option de vente (put) américaine : {pPriceAmer:F2}");

            // Création de l'objet BlackSholes pour l'option européenne
            BlackSholes EuroOpt = new (
                SpotPrice,
                StrikePrice,
                RiskFreeRate,
                Volatility,
                TimeToMaturity
            );

            // Calcul du prix de l'option call européenne
            double cPriceEur = EuroOpt.CallPrice();
            Console.WriteLine($"Prix de l'option d'achat (call) européenne : {cPriceEur:F2}");

            // Calcul du prix de l'option put européenne
            double pPriceEur = EuroOpt.PutPrice();
            Console.WriteLine($"Prix de l'option de vente (put) européenne : {pPriceEur:F2}");
        }


    
    }
}