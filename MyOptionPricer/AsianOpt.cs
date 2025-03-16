using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MyOptionPricer
{
    public class AsianOpt
    {
    

        public AsianOpt()
        {
            int countSimulations = 0;
            double sommePayoffs = 0;
        }
        public async Task<int> GetDateNumber()
        {
            // Appel de l'API pour récupérer les données JSON
            var data = await AlphaVanAPI.GetTimeSeriesData("IBM");

            // Parser le JSON
            using JsonDocument doc = JsonDocument.Parse(data);
            JsonElement root = doc.RootElement;

            // Vérifier si la clé "Time Series (Daily)" existe
            if (root.TryGetProperty("Time Series (Daily)", out JsonElement timeSeries))
            {
                int count = 0;

                // Parcourir les entrées du JSON
                foreach (JsonProperty dataEntry in timeSeries.EnumerateObject())
                {
                    string date = dataEntry.Name; // La clé est la date (ex: "2024-10-21")

                    // Vérifier si la date commence par "2024-" ou "2025-"
                    if (date.StartsWith("2024-") || date.StartsWith("2025-"))
                    {
                        count++;
                    }
                }

                return count;
            }
            else
            {
                Console.WriteLine("Erreur : impossible de trouver 'Time Series (Daily)' dans la réponse JSON.");
                return 0;
            }
        }
    
    
    
    }
}
