using System;

namespace MyOptionPricer
{
    class Program
    {
        static void Main()
        {
            var (SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity, Accuracy, DividendeRate) =
                ConsoleInterface.GetOptionParametersFromConsole();

            // Options américaines
            Binomial AmericanOption = new(
                SpotPrice,
                StrikePrice,
                TimeToMaturity,
                Volatility,
                RiskFreeRate,
                Accuracy,
                DividendeRate);

            Console.WriteLine($"\n\nRappel des entrées : " +
                $"\nPrix d'exercice : {StrikePrice} " +
                $"\nPrix actuel : {SpotPrice}" +
                $"\nTemps jusqu'à expiration : {TimeToMaturity} ans" +
                $"\nVolatilité : {Volatility * 100}%" +
                $"\nTaux sans risque : {RiskFreeRate * 100}%" +
                $"\nPrécision (n) : {Accuracy}" +
                $"\nDividende : {DividendeRate * 100}%\n");

            // Calculs américains
            double cPriceAmer = AmericanOption.CallPrice();
            Console.WriteLine($"\nPrix du call américain : {cPriceAmer:F2}");
            DisplayGreeks(new Greeks(SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity), true);

            double pPriceAmer = AmericanOption.PutPrice();
            Console.WriteLine($"\nPrix du put américain : {pPriceAmer:F2}");
            DisplayGreeks(new Greeks(SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity), false);

            // Options européennes
            BlackSholes EuroOpt = new(
                SpotPrice,
                StrikePrice,
                RiskFreeRate,
                Volatility,
                TimeToMaturity);

            // Calculs européens
            double cPriceEur = EuroOpt.CallPrice();
            Console.WriteLine($"\nPrix du call européen : {cPriceEur:F2}");
            DisplayGreeks(new Greeks(SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity), true);

            double pPriceEur = EuroOpt.PutPrice();
            Console.WriteLine($"\nPrix du put européen : {pPriceEur:F2}");
            DisplayGreeks(new Greeks(SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity), false);
        }

        static void DisplayGreeks(Greeks greeksCalculator, bool isCall)
        {
            var (delta, gamma, vega, theta, rho) = greeksCalculator.ComputeGreeks(isCall);

            Console.WriteLine("\nGrecs associés :");
            Console.WriteLine($"• Delta: {delta,8:F4}  (Sensibilité au prix du sous-jacent)");
            Console.WriteLine($"• Gamma: {gamma,8:F4}  (Sensibilité du Delta aux variations du sous-jacent)");
            Console.WriteLine($"• Vega:  {vega,8:F4}  (Sensibilité à la volatilité)");
            Console.WriteLine($"• Theta: {theta,8:F4}  (Décroissance temporelle)");
            Console.WriteLine($"• Rho:   {rho,8:F4}  (Sensibilité aux taux d'intérêt)");
        }
    }
}