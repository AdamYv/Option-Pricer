using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;


namespace MyOptionPricer
{
    class ConsoleInterface
    {
        // Configuration pour intercepter Ctrl+Z
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr handle, out uint mode);

        //private static bool _ctrlZPressed = false;

        // Méthode pour obtenir les paramètres de l'option via la console
        public static (double SpotPrice, double StrikePrice, double RiskFreeRate, double Volatility, double TimeToMaturity, int Accuracy, double DividendeRate) GetOptionParametersFromConsole()
        {
            ConfigureConsoleForCtrlZ();

            var steps = new List<(string Key, Func<double> ReadFunction)>
            {
                ("SpotPrice", () => ReadValueWithNavigation("Prix actuel de l'actif sous-jacent (S) : ")),
                ("StrikePrice", () => ReadValueWithNavigation("Prix d'exercice (K) : ")),
                ("RiskFreeRate", () => ReadPercentageWithNavigation("Taux sans risque (r) en % (ex: 5 pour 5%) : ")),
                ("Volatility", () => ReadPercentageWithNavigation("Volatilité (sigma) en % (ex: 20 pour 20%) : ")),
                ("DividendeRate", () => ReadPercentageWithNavigation("Dividende reçu par an en % (ex: 3 pour 3%) : ")),
                ("TimeToMaturity", () => ReadTimeToMaturityWithNavigation())
            };

            var parameters = new Dictionary<string, double>();
            var history = new Stack<int>();
            int currentStep = 0;

            while (currentStep < steps.Count)
            {
                Console.Clear();
                var (key, readFunction) = steps[currentStep];

                // Afficher l'historique des valeurs
                if (parameters.ContainsKey(key))

                Console.WriteLine($"{key}: {parameters[key]} [\nAppuyez sur Ctrl+Z pour modifier]");

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "ascii_art.txt");
                string myBanner = File.ReadAllText(filePath);
                Console.WriteLine(myBanner);
                Console.WriteLine(new string('=', 100));
                Console.WriteLine("Pour revenir en arrière, appuyez sur Ctrl+Z.");

                try
                {
                    double value = readFunction();
                    parameters[key] = value;
                    history.Push(currentStep);
                    currentStep++;
                }
                catch (OperationCanceledException) // Ctrl+Z détecté
                {
                    if (history.Count > 0)
                    {
                        currentStep = history.Pop();
                        parameters.Remove(steps[currentStep].Key);
                    }
                }
            }

            // Saisie de la précision
            int accuracy = ReadPositiveInt("Précision (nombre de pas, n) : ");

            return (
                parameters["SpotPrice"],
                parameters["StrikePrice"],
                parameters["RiskFreeRate"],
                parameters["Volatility"],
                parameters["TimeToMaturity"],
                accuracy,
                parameters["DividendeRate"]
            );
        }

        // Méthode unifiée pour lire les valeurs avec gestion de Ctrl+Z
        private static double ReadValueWithNavigation(string prompt)
        {
            Console.Write(prompt);
            while (true)
            {
                string input = ReadLineWithCtrlZ();
                if (double.TryParse(input, out double result))
                    return result;
                Console.WriteLine("Entrée invalide. Réessayez : ");
            }
        }

        // Méthode pour lire les pourcentages
        private static double ReadPercentageWithNavigation(string prompt)
        {
            double value = ReadValueWithNavigation(prompt);
            return value / 100.0;
        }

        // Méthode pour lire le temps jusqu'à l'expiration
        private static double ReadTimeToMaturityWithNavigation()
        {
            Console.WriteLine("Choisissez le format du temps jusqu'à l'expiration :");
            Console.WriteLine("1. Années\n2. Mois\n3. Jours");

            int choice = ReadIntInRange("Votre choix (1-3) : ", 1, 3);
            double value = ReadValueWithNavigation($"Temps jusqu'à l'expiration ({GetTimeUnit(choice)}) : ");

            return choice switch
            {
                1 => value,
                2 => value / 12,
                3 => value / 365.25,
                _ => throw new InvalidOperationException()
            };
        }

        // Méthode pour intercepter Ctrl+Z
        private static string ReadLineWithCtrlZ()
        {
            var input = new System.Text.StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else if (key.Key == ConsoleKey.Z && (key.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    throw new OperationCanceledException();
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return input.ToString();
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    input.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
        }

        // Configuration de la console
        private static void ConfigureConsoleForCtrlZ()
        {
            var handle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            GetConsoleMode(handle, out uint mode);
            SetConsoleMode(handle, mode & ~0x0040u); // Conversion explicite en uint avec 'u'
        }

        // Méthodes utilitaires
        private static string GetTimeUnit(int choice) => choice switch { 1 => "années", 2 => "mois", 3 => "jours", _ => "" };


        // Méthode pour lire un entier dans une plage spécifiée
        static int ReadIntInRange(string prompt, int min, int max)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value) && value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine($"Veuillez entrer un entier entre {min} et {max}.");
            }
        }

        // Méthode pour lire un nombre décimal positif depuis la console
        static double ReadPositiveDouble(string prompt)
        {
            double value;
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out value) && value > 0)
                {
                    return value;
                }
                Console.WriteLine("Veuillez entrer un nombre valide et positif.");
            }
        }

        // Méthode pour lire un entier positif depuis la console
        static int ReadPositiveInt(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value) && value > 0)
                {
                    return value;
                }
                Console.WriteLine("Veuillez entrer un entier valide et positif.");
            }
        }
    }
}