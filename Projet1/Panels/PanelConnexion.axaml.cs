using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Projet1.Windows;

namespace Projet1.Panels
{
    public partial class FormPanel : UserControl
    {

        public FormPanel()
        {
            InitializeComponent();
            BtnConnexion.Click += OnLoginClick;
            ContainError.PointerPressed += CloseErrorMessage;
        }

        public void HandleKeyPressed(object? sender, KeyEventArgs e){
            if(e.Key == Key.Enter){
                OnLoginClick(null, null);
            }
        }

        private void OnLoginClick(object? sender, RoutedEventArgs? e)
        {
            var login = LoginInput.Text;
            var password = PasswordInput.Text;

            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
            {
                if (App.MonModel.CheckConnexionInfo(login, password) > 0)
                {
                    App.UserConnected = App.MonModel.GetUserByLoginAndPasswod(login, password);
                    if (App.UserConnected != null)
                    {
                        App.MonModel.InsertIntoConnectedUser(App.UserConnected.GetId());
                        if (MainWindow.Instance != null)
                        {
                            LoginInput.Text = "";
                            PasswordInput.Text = "";
                            MainWindow.Instance.PanelNavbar.ShowNavbarConnected();
                            MainWindow.Instance.ShowPanAcceuil();
                        }
                    }
                }
                else
                {
                    ErrorMessage.IsVisible = true;
                    ErrorMessage.Text = "Veuillez vérifier vos informations de connexion (bien renseigner toutes les informations)";
                }
            }
            else
            {
                ErrorMessage.IsVisible = true;
                ErrorMessage.Text = "Veuillez vérifier vos informations de connexion";
            }
        }

        public void OnDisconnectClicked(object? sender, RoutedEventArgs? e){
            if(App.UserConnected != null){
                App.MonModel.DeleteFromConnectedUser(App.UserConnected.GetId());
                App.UserConnected = null;
                MainWindow.Instance?.ShowNavbarConnexion();
                MainWindow.Instance?.ShowPanConnexion();
            }
        }

        public void CloseErrorMessage(object? sender, RoutedEventArgs? e){
            ErrorMessage.IsVisible = false;
            ErrorMessage.Text = "";
        }
    }
}
