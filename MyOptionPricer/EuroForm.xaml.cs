using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for EuroForm.xaml
    /// </summary>
    public partial class EuroForm : UserControl
    {
        public EuroForm()
        {
            InitializeComponent();
        }

        private void Button_Compute(object sender, RoutedEventArgs e)
        {
            try
            {
                // Récupération des valeurs
                double S = double.Parse(txtSpot.Text);
                double K = double.Parse(txtStrike.Text);
                double r = double.Parse(txtRiskFree.Text) / 100; // Conversion %
                double sigma = double.Parse(txtVolatility.Text) / 100;
                double T = double.Parse(txtMaturity.Text);

                // Création de l'option
                var option = new EuropOpt(S, K, r, sigma, T);

                // Calcul du prix selon le type
                double price = radioCall.IsChecked == true
                    ? option.CallPrice()
                    : option.PutPrice();

                // Affichage du prix
                txtResult.Text = $"{price:F4}";

                // Calcul des Greeks
                var greeksCalculator = new Greeks(S, K, r, sigma, T);
                var greeks = greeksCalculator.ComputeGreeks(radioCall.IsChecked == true);

                // Affichage des Greeks
                txtDelta.Text = $"{greeks.Delta:F4}";
                txtGamma.Text = $"{greeks.Gamma:F4}";
                txtTheta.Text = $"{greeks.Theta:F4}";
                txtVega.Text = $"{greeks.Vega:F4}";
                txtRho.Text = $"{greeks.Rho:F4}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur: {ex.Message}");
            }
        }

        private void Button_Copie(object sender, RoutedEventArgs e)
        {
            try
            {
                // Construction du texte à copier
                string texteACopier = $"Prix : {txtResult.Text}\n" +
                                     $"Δ (Delta) : {txtDelta.Text}\n" +
                                     $"Γ (Gamma) : {txtGamma.Text}\n" +
                                     $"Θ (Theta) : {txtTheta.Text}\n" +
                                     $"ν (Vega) : {txtVega.Text}\n" +
                                     $"ρ (Rho) : {txtRho.Text}";

                // Copie dans le presse-papiers
                Clipboard.SetText(texteACopier);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la copie : {ex.Message}");
            }
        }

    }
}
