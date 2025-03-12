using System;
using System.IO;

namespace MyOptionPricer
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "ascii_art.txt");
            string myBanner = File.ReadAllText(filePath);
            Console.WriteLine(myBanner);
            Console.WriteLine(new string('=', 100));

            // Obtenir les paramètres de l'option via l'interface console
            var optionParams = ConsoleInterface.GetOptionParametersFromConsole();

            // Création de l'objet Binomial pour l'option américaine
            Binomial AmericanOption = new Binomial(
                optionParams.SpotPrice,
                optionParams.StrikePrice,
                optionParams.TimeToMaturity,
                optionParams.Volatility,
                optionParams.RiskFreeRate,
                optionParams.Accuracy,
                optionParams.DividendeRate
            //120,100,0.20,0.5,1,10,0.3


            );

            // Calcul du prix de l'option call
            double cPriceAmer = AmericanOption.callPrice();
            Console.WriteLine($"Prix de l'option d'achat (call) américaine : {cPriceAmer:F2}");

            // Calcul du prix de l'option put
            double pPriceAmer = AmericanOption.putPrice();
            Console.WriteLine($"Prix de l'option de vente (put) américaine : {pPriceAmer:F2}");

            // Création de l'objet BlackSholes pour l'option européenne
            BlackSholes EuroOpt = new BlackSholes(
                optionParams.SpotPrice,
                optionParams.StrikePrice,
                optionParams.RiskFreeRate,
                optionParams.Volatility,
                optionParams.TimeToMaturity
            );

            // Calcul du prix de l'option call européenne
            double cPriceEur = EuroOpt.callPrice();
            Console.WriteLine($"Prix de l'option d'achat (call) européenne : {cPriceEur:F2}");

            // Calcul du prix de l'option put européenne
            double pPriceEur = EuroOpt.putPrice();
            Console.WriteLine($"Prix de l'option de vente (put) européenne : {pPriceEur:F2}");
        }


    
    }
}