using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Projet1.Windows;

namespace Projet1.Panels
{
    public partial class Navbar : UserControl
    {
        public Navbar()
        {
            InitializeComponent();

            BtnAcceuil.Click += BtnAcceuil_Click;
            BtnGestion.Click += BtnGestion_Click;
        }

        public void ShowNavbarConnected()
        {
            NavbarConnected.IsVisible = true;
            NavbarConnexion.IsVisible = false;
        }

        public void ShowNavbarConnexion()
        {
            NavbarConnected.IsVisible = false;
            NavbarConnexion.IsVisible = true;
        }

        private void BtnAcceuil_Click(object? sender, RoutedEventArgs e)
        {
            // Logique du bouton "Accueil"
            Console.WriteLine("Accueil button clicked");
        }

        private void BtnGestion_Click(object? sender, RoutedEventArgs e){
            if(MainWindow.Instance != null)
                MainWindow.Instance.ShowPanGestion();
        }
    }
}