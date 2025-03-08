using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MyOptionPricer
{
    class AlphaVanAPI
    {
        public static readonly string apiKey = File.ReadAllText("api_key.env");

        public static async Task<double> GetStockPrice(string symbol )
        {
            Console.WriteLine(symbol);
            // Appel à une API pour obtenir le prix de l'action
            using(HttpClient client = new HttpClient())
            {
                string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={apiKey}";
                string result = await client.GetStringAsync(url);
                JObject data = JObject.Parse(result);
                JToken? priceToken = data["Global Quote"]?["05. price"];
                if (priceToken == null)
                {
                    throw new Exception("Price data not found.");
                }
                return (double)priceToken;

            }
        }

      
    
    }
}