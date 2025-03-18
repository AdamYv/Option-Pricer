using System;
using System.Collections.Generic;
using System.IO;
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
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public long Volume { get; set; }
    }

    static AlphaVanAPI()
    {
        try
        {
            var envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "api_key.env");
            if (!File.Exists(envPath)) throw new FileNotFoundException("Fichier API key manquant");
            apiKey = File.ReadAllText(envPath).Trim();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERREUR INIT: {ex.Message}");
            throw;
        }
    }

    public static async Task<List<HistoricalData>> GetHistoricalData(string symbol, int days = 100)
    {
        var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={apiKey}&outputsize=compact";

        try
        {
            var response = await client.GetStringAsync(url);
            var jsonDoc = JsonDocument.Parse(response);
            var timeSeries = jsonDoc.RootElement.GetProperty("Time Series (Daily)");

            var data = new List<HistoricalData>();
            foreach (var day in timeSeries.EnumerateObject())
            {
                if (data.Count >= days) break;

                var values = day.Value;
                data.Add(new HistoricalData
                {
                    Date = DateTime.Parse(day.Name),
                    Open = values.GetProperty("1. open").GetDouble(),
                    High = values.GetProperty("2. high").GetDouble(),
                    Low = values.GetProperty("3. low").GetDouble(),
                    Close = values.GetProperty("4. close").GetDouble(),
                    Volume = values.GetProperty("5. volume").GetInt64()
                });
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