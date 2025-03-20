using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class AlphaVanAPI
{
    public static readonly string apiKey;
    private static readonly HttpClient client = new HttpClient();
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public class HistoricalData
    {
        public DateTime Date { get; set; }
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }
        public long Volume { get; set; }
    }

    static AlphaVanAPI()
    {
        apiKey = "7IZOC5U6JFLWEQET";
    }


    public static async Task<List<HistoricalData>> GetHistoricalData(string symbol, int days = 100)
    {
        var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={apiKey}&outputsize=compact";

        try
        {
            var response = await client.GetStringAsync(url);
            //Console.WriteLine("Réponse API : " + response); // Log de la réponse
            var jsonDoc = JsonDocument.Parse(response);

            if (jsonDoc.RootElement.TryGetProperty("Error Message", out var errorMessage))
            {
                Console.WriteLine($"ERREUR API: {errorMessage.GetString()}");
                return null;
            }

            if (!jsonDoc.RootElement.TryGetProperty("Time Series (Daily)", out var timeSeries))
            {
                Console.WriteLine("ERREUR API: La clé 'Time Series (Daily)' est manquante dans la réponse.");
                return null;
            }

            var data = new List<HistoricalData>();
            foreach (var day in timeSeries.EnumerateObject())
            {
                if (data.Count >= days) break;

                var values = day.Value;
                try
                {
                    data.Add(new HistoricalData
                    {
                        Date = DateTime.Parse(day.Name),
                        Open = float.Parse(values.GetProperty("1. open").GetString(), CultureInfo.InvariantCulture),
                        High = float.Parse(values.GetProperty("2. high").GetString(), CultureInfo.InvariantCulture),
                        Low = float.Parse(values.GetProperty("3. low").GetString(), CultureInfo.InvariantCulture),
                        Close = float.Parse(values.GetProperty("4. close").GetString(), CultureInfo.InvariantCulture),
                        Volume = long.Parse(values.GetProperty("5. volume").GetString())
                    });
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"ERREUR DE CONVERSION : {ex.Message}");
                    continue; // Ignorer cette entrée et passer à la suivante
                }
            }

            if (data.Count == 0)
            {
                Console.WriteLine("AUCUNE DONNÉE DISPONIBLE : La liste des données historiques est vide.");
                return null;
            }

            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine($"ERREUR API: {e.Message}");
            return null;
        }
    }



    public static double CalculateVolatility(List<HistoricalData> data)
    {
        if (data == null || !data.Any())
        {
            throw new ArgumentException("La liste des données historiques est null ou vide.");
        }

        var closes = data.Select(d => d.Close).ToList();
        var logReturns = new List<double>();

        for (int i = 1; i < closes.Count; i++)
        {
            logReturns.Add(Math.Log(closes[i] / closes[i - 1]));
        }

        var mean = logReturns.Average();
        var variance = logReturns.Sum(r => Math.Pow(r - mean, 2)) / (logReturns.Count - 1);
        return Math.Sqrt(variance * 252); // Volatilité annualisée
    }
}