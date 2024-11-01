using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Projet1.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

#pragma warning disable CS0612

namespace Projet1.Panels {
    public partial class PanelGestion : UserControl
    {
        private byte[]? imgArticle; // Utiliser "byte[]" au lieu de "Byte[]" pour le type

        public PanelGestion()
        {
            InitializeComponent();

            // Gestionnaire d'événements pour le bouton de sélection d'image
            BtnSelectImage.Click += async (sender, e) => await SelectImageAsync();
            BtnCreerArticle.Click += BtnCreerArticle_Click;
            ContainError.PointerPressed += HideErrorMessage;
        }

        private void BtnCreerArticle_Click(object? sender, RoutedEventArgs? e) {
            if (imgArticle != null) {
                if (!string.IsNullOrWhiteSpace(NomInput.Text) &&
                    !string.IsNullOrWhiteSpace(DescriptionInput.Text) &&
                    !string.IsNullOrWhiteSpace(PrixInput.Text) &&
                    double.TryParse(PrixInput.Text, out double prix)) // Utilisation de TryParse
                {
                    Article unArticle = new(NomInput.Text, DescriptionInput.Text, prix, imgArticle);
                    if (App.MonModel.InsertIntoArticle(unArticle) > 0) {
                        // Réinitialise les champs après la création réussie
                        NomInput.Text = "";
                        DescriptionInput.Text = "";
                        PrixInput.Text = "";
                        SelectedImagePath.Text = "";
                        imgArticle = null;
                    } else {
                        ShowErrorMessage("Erreur lors de la création de l'article");
                    }
                } else {
                    ShowErrorMessage("Veuillez remplir tous les champs correctement.");
                }
            } else {
                ShowErrorMessage("Veuillez sélectionner une image.");
            }
        }

        private async Task SelectImageAsync()
        {
            var filePicker = new OpenFileDialog
            {
                Title = "Sélectionner une image",
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "Images", Extensions = { "jpg", "jpeg", "png" } }
                }
            };

            var result = await filePicker.ShowAsync(GetParentWindow() ?? MainWindow.Instance); // Utilise GetParentWindow

            if (result != null && result.Length > 0) {
                // Affiche le chemin de l'image sélectionnée
                SelectedImagePath.Text = Path.GetFileName(result[0]);
                imgArticle = File.ReadAllBytes(result[0]); // Charge l'image
            }
        }

        private void ShowErrorMessage(string message) {
            ErrorMessage.Text = message;
            ErrorMessage.IsVisible = true;
        }

        private void HideErrorMessage(object? sender, RoutedEventArgs? e) {
            ErrorMessage.Text = "";
            ErrorMessage.IsVisible = false;
        }

        private Window? GetParentWindow() {
            // Obtient la fenêtre parente du contrôle (facultatif)
            return this.GetLogicalParent<Window>();
        }
    }
}
