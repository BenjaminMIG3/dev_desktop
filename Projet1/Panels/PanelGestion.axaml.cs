using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Projet1.Windows;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Projet1.Entite;
using System.Collections.ObjectModel;

#pragma warning disable CS0612

namespace Projet1.Panels {
    public partial class PanelGestion : UserControl
    {
        private byte[]? imgArticle; // Utiliser "byte[]" au lieu de "Byte[]" pour le type
        public ObservableCollection<CategorieArticle> Categories { get; set; }

        public PanelGestion()
        {
            InitializeComponent();

            // Assurez-vous que vous initialisez correctement la collection des catégories
            Categories = new ObservableCollection<CategorieArticle>(App.MonModel.GetAllCategorieArticle());

            // Définissez le contexte de données
            DataContext = this;

            // Gestion des événements
            this.BtnSelectImage.Click += async (sender, e) => await SelectImageAsync();
            this.BtnCreerArticle.Click += BtnCreerArticle_Click;
            this.ContainError.PointerPressed += HideErrorMessage;
        }

        private void BtnCreerArticle_Click(object? sender, RoutedEventArgs? e)
        {
            if (imgArticle != null)
            {
                if (!string.IsNullOrWhiteSpace(NomInput.Text) &&
                    !string.IsNullOrWhiteSpace(DescriptionInput.Text) &&
                    !string.IsNullOrWhiteSpace(PrixInput.Text) &&
                    double.TryParse(PrixInput.Text, out double prix) &&
                    CategorieComboBox.SelectedItem is CategorieArticle selectedCategorie)
                {
                    int id_categ = selectedCategorie.Id;

                    Article unArticle = new(NomInput.Text, DescriptionInput.Text, prix, imgArticle, id_categ);

                    if (App.MonModel.InsertIntoArticle(unArticle) > 0)
                    {
                        NomInput.Text = "";
                        DescriptionInput.Text = "";
                        PrixInput.Text = "";
                        SelectedImagePath.Text = "";
                        imgArticle = null;
                        CategorieComboBox.SelectedIndex = 0;
                        CategorieComboBox.SelectedItem = "";
                    }
                    else
                    {
                        ShowErrorMessage("Erreur lors de la création de l'article");
                    }
                }
                else
                {
                    ShowErrorMessage("Veuillez remplir tous les champs correctement.");
                }
            }
            else
            {
                ShowErrorMessage("Veuillez sélectionner une image.");
            }
        }

        [System.Obsolete]
        private async Task SelectImageAsync()
        {
            var filePicker = new OpenFileDialog
            {
                Title = "Sélectionnez une image",
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "Images", Extensions = { "jpg", "jpeg", "png" } }
                }
            };

            if(MainWindow.Instance is not null){
                var result = await filePicker.ShowAsync(GetParentWindow() ?? MainWindow.Instance);

                if (result != null && result.Length > 0) {
                    SelectedImagePath.Text = Path.GetFileName(result[0]);
                    imgArticle = File.ReadAllBytes(result[0]);
                }
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
            return this.GetLogicalParent<Window>();
        }
    }
}
