using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyOptionPricer
{
    /// <summary>
    /// Interaction logic for AsianForm.xaml
    /// </summary>
    public partial class AsianForm : UserControl
    {
        public AsianForm()
        {
            InitializeComponent();
        }

        private async void Button_Compute(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validation des entrées
                if (!ValidateInputs(out string symbol, out double strike,
                    out double riskFreeRate, out double maturity, out int simulations))
                    return;

                var option = await AsianOpt.CreateAsync(
                    symbol,
                    daysHistorical: 100, // Fixé à 100 comme dans l'exemple
                    strike,
                    maturity,
                    riskFreeRate / 100.0, // Conversion du pourcentage
                    simulations
                );

                // Exécuter le pricing sur un thread de fond
                double price = await Task.Run(() =>
                    radioCall.Dispatcher.Invoke(() =>
                        radioCall.IsChecked == true ? option.CallPrice() : option.PutPrice()
                    )
                );

                // Mise à jour de l'UI sur le thread principal
                Dispatcher.Invoke(() =>
                {
                    txtPrice.Text = $"{price.ToString("F4")}$";
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    new CustomMessageBox(ex.Message, "Erreur").Show();
                });
            }
        }


        private bool ValidateInputs(out string symbol, out double strike,
            out double riskFreeRate, out double maturity, out int simulations)
        {
            symbol = txtSymbol.Text.Trim();
            strike = riskFreeRate = maturity = 0;
            simulations = (int)sliderSimulations.Value;

            if (string.IsNullOrEmpty(symbol))
            {
                ShowError("Veuillez entrer un symbole.");
                return false;
            }

            if (!double.TryParse(txtStrike.Text, out strike) || strike <= 0)
            {
                ShowError("Strike invalide.");
                return false;
            }

            if (!double.TryParse(txtRisk.Text, out riskFreeRate) || riskFreeRate <= 0)
            {
                ShowError("Taux sans risque invalide.");
                return false;
            }

            if (!double.TryParse(txtMaturity.Text, out maturity) || maturity <= 0)
            {
                ShowError("Maturité invalide.");
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            new CustomMessageBox(message, "Erreur").Show();
        }

        private void Button_Copie(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtPrice.Text);
        }


    }
}
