using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MyOptionPricer
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            string myBanner = File.ReadAllText("ascii_art.txt");
            string symbol = "IBM";

            

            double S = await AlphaVanAPI.GetStockPrice(symbol);
            Console.WriteLine($"Stock price of {symbol} is {S}");



        }

        
    }
}