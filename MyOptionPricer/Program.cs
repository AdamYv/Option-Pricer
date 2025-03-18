using System;
using System.Threading.Tasks;

namespace MyOptionPricer
{
    class Program
    {
        public static async Task Main()
        {

            var pricer = new MonteCarloAsianPricer();
            double price = await pricer.PriceAsianOption(
                symbol: "IBM",
                strike: 150.0,
                riskFreeRate: 0.05,
                maturityYears: 1.0);

            Console.WriteLine($"Prix de l'option asiatique : {price:N2}");
        }            



            //var (SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity, Accuracy, DividendeRate) =
               // ConsoleInterface.GetOptionParametersFromConsole();

            //DisplayInputs(SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity, Accuracy, DividendeRate);

            // Calculs et affichage des résultats pour l'option américaine
            //CalculateAndDisplayAmericanOptionPrices(SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity, Accuracy, DividendeRate);

            // Calculs et affichage des résultats pour l'option européenne
            //CalculateAndDisplayEuropeanOptionPrices(SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity);
        }

        static void DisplayInputs(double SpotPrice, double StrikePrice, double RiskFreeRate, double Volatility, double TimeToMaturity, int Accuracy, double DividendeRate)
        {
            Console.WriteLine($"\n\nRappel des entrées : " +
                $"\nPrix d'exercice : {StrikePrice} " +
                $"\nPrix actuel : {SpotPrice}" +
                $"\nTemps jusqu'à expiration : {TimeToMaturity} ans" +
                $"\nVolatilité : {Volatility * 100}%" +
                $"\nTaux sans risque : {RiskFreeRate * 100}%" +
                $"\nPrécision (n) : {Accuracy}" +
                $"\nDividende : {DividendeRate * 100}%\n");
        }

        static void CalculateAndDisplayAmericanOptionPrices(double SpotPrice, double StrikePrice, double RiskFreeRate, double Volatility, double TimeToMaturity, int Accuracy, double DividendeRate)
        {
            // Options américaines
            AmericanOption AOpt = new(SpotPrice, StrikePrice, TimeToMaturity, Volatility, RiskFreeRate, Accuracy, DividendeRate);

            // Calculs américains
            double cPriceAmer = AOpt.CallPrice();
            Console.WriteLine($"\nPrix du call américain : {cPriceAmer:F2}");
            DisplayGreeks(new Greeks(SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity), true);

            double pPriceAmer = AOpt.PutPrice();
            Console.WriteLine($"\nPrix du put américain : {pPriceAmer:F2}");
            DisplayGreeks(new Greeks(SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity), false);
        }

        static void CalculateAndDisplayEuropeanOptionPrices(double SpotPrice, double StrikePrice, double RiskFreeRate, double Volatility, double TimeToMaturity)
        {
            // Options européennes
            EuropOpt EurOpt = new(SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity);

            // Calculs européens
            double cPriceEur = EurOpt.CallPrice();
            Console.WriteLine($"\nPrix du call européen : {cPriceEur:F2}");
            DisplayGreeks(new Greeks(SpotPrice, StrikePrice, RiskFreeRate, Volatility, TimeToMaturity), true);

            double pPriceEur = EurOpt.PutPrice();
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
