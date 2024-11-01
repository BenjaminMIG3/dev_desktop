using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Projet1.Panels
{
    public partial class PanelAcceuil : UserControl
    {
        public PanelAcceuil()
        {
            InitializeComponent();
            BtnAction.Click += OnActionButtonClick;
        }

        private void OnActionButtonClick(object? sender, RoutedEventArgs e)
        {
            // Logique d'action lorsque le bouton est cliqué
            StatusMessage.Text = "Action effectuée avec succès !";
        }
    }
}