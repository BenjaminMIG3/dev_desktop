using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Projet1.Entite;
using Projet1.Windows;

namespace Projet1.Panels
{
    public partial class Navbar : UserControl
    {


        public Navbar()
        {
            InitializeComponent();
            

            BtnAcceuil.Click += BtnAcceuil_Click;
            
            this.DataContext = this;
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
            MainWindow.Instance?.ShowPanAcceuil();
        }

        private void BtnGestion_Click(object? sender, RoutedEventArgs e)
        {
            MainWindow.Instance?.ShowPanGestion();
        }

        private void BtnPanier_Click(object? sender, RoutedEventArgs e)
        {
            MainWindow.Instance?.ShowPanPanier();
        }

        private void OnMesCommandesClicked(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance?.ShowPanMesCommandes();
        }

        private void OnParametresClicked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Paramètres clicked");
        }

        private void OnDeconnexionClicked(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance?.panelConnexion.OnDisconnectClicked(null,null);
        }
    }
}