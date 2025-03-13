using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyOptionPricer
{
    class AlphaVanAPI
    {
        public static readonly string apiKey = "demo";
        private static readonly HttpClient client = new HttpClient();

        // Ajout d'un constructeur statique pour gérer les erreurs d'initialisation
        static AlphaVanAPI()
        {
            try
            {
                // Chemin corrigé et vérification de l'existence du fichier
                var envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "api_key.env");



                if (!File.Exists(envPath))
                {
                    throw new FileNotFoundException($"Fichier API key introuvable: {envPath}");
                }

                //apiKey = File.ReadAllText(envPath).Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERREUR D'INITIALISATION: {ex.Message}");
                throw; // Propagate exception to show clear error
            }
        }

        public static async Task<string> GetTimeSeriesData(string symbol)
        {
            var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={apiKey}";

            try
            {
                return await client.GetStringAsync(url);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur réseau: {e.Message}");
                return null;
            }
        }

        public static async Task Afficher()
        {
            string symbol = "IBM";
            var data = await GetTimeSeriesData(symbol);
            Console.WriteLine(data ?? "Aucune donnée reçue");
        }
    }
}