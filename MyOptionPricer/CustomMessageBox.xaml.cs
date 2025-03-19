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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyOptionPricer
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string message)
        {
            InitializeComponent();

            txtMessage.Text = message;

            Title = "Erreur";
        }

        public CustomMessageBox(string message, string Title)
        {

            InitializeComponent();

            txtMessage.Text = message;

            this.Title = Title;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private System.Windows.Threading.DispatcherTimer _notificationTimer;

        private void Copier(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(txtMessage.Text);
                ShowNotification("Texte copié avec succès !");
            }
            catch (Exception ex)
            {
                ShowNotification($"Erreur de copie : {ex.Message}");
            }
        }

        private void ShowNotification(string message)
        {
            // Configure le timer
            if (_notificationTimer == null)
            {
                _notificationTimer = new System.Windows.Threading.DispatcherTimer();
                _notificationTimer.Tick += (s, args) =>
                {
                    txtNotification.Visibility = Visibility.Collapsed;
                    _notificationTimer.Stop();
                };
                _notificationTimer.Interval = TimeSpan.FromSeconds(2);
            }

            // Affiche la notification
            txtNotification.Text = message;
            txtNotification.Visibility = Visibility.Visible;

            // Démarre l'animation
            var animation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            txtNotification.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animation);

            // Démarre le timer
            _notificationTimer.Start();
        }


    }
}
