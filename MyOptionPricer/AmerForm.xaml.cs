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
    /// Interaction logic for AmerForm.xaml
    /// </summary>
    public partial class AmerForm : UserControl
    {
        public AmerForm()
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
                int n = (int)sliderAccuracy.Value; 
                double q = double.Parse(txtDividend.Text) / 100;

                // Création de l'option
                var option = new AmericanOption(
                    S, K, T, sigma, r, n, q
                );

                // Calcul du prix selon le type
                double price = radioCall.IsChecked == true ?
                    option.CallPrice() :
                    option.PutPrice();

                // Affichage du résultat
                txtResult.Text = $"{price:F4} $";

                // Calcul et affichage des Greeks (différences finies sur l'arbre)
                var greeks = option.ComputeGreeks(radioCall.IsChecked == true);
                txtDelta.Text = $"{greeks.Delta:F4}";
                txtGamma.Text = $"{greeks.Gamma:F4}";
                txtTheta.Text = $"{greeks.Theta:F4}";
                txtVega.Text = $"{greeks.Vega:F4}";
                txtRho.Text = $"{greeks.Rho:F4}";
            }
            catch (FormatException)
            {
                MessageBox.Show("Veuillez entrer des nombres valides");
            }
            catch (OverflowException)
            {
                MessageBox.Show("Les valeurs saisies sont trop grandes");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur inattendue: {ex.Message}");
            }

           

        }

        private void Button_Copie(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText($"Prix : {txtResult.Text}\n" +
                              $"Δ (Delta) : {txtDelta.Text}\n" +
                              $"Γ (Gamma) : {txtGamma.Text}\n" +
                              $"Θ (Theta) : {txtTheta.Text}\n" +
                              $"ν (Vega) : {txtVega.Text}\n" +
                              $"ρ (Rho) : {txtRho.Text}");
        }

        
    }
}
