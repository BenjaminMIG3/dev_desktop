using Avalonia.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using Projet1.Entite;
using System;
using System.ComponentModel;
using Avalonia.Interactivity;
using Projet1.Windows;

namespace Projet1.Panels
{
    public partial class PanelCommande : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<Facturation> LesCommandes { get; set; } = new ObservableCollection<Facturation>();

        public PanelCommande()
        {
            InitializeComponent();
            DataContext = this;

            if (App.UserConnected != null) ChargerCommandes(App.UserConnected.GetId());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChargerCommandes(int userId)
        {
            LesCommandes.Clear();

            var facturations = App.MonModel.GetAllFacturationByIdUser(userId);

            foreach (var fact in facturations)
            {
                fact.LesArticles = App.MonModel.GetArticleByIdFacturation(fact.IdFacturation);
                LesCommandes.Add(fact);
            }
        }

        private void Commande_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Facturation commande)
            {
                MainWindow.Instance?.ShowPanDetailCommandes(commande.IdFacturation);
            }
        }
    }
}
