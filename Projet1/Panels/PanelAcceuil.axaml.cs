using Projet1.Entite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Projet1.Windows;
using System.Linq;
using System.Collections.Generic;

namespace Projet1.Panels
{
    public partial class PanelAcceuil : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Article>? _lesArticles;
        public ObservableCollection<Article>? LesArticles
        {
            get => _lesArticles;
            set
            {
                if (_lesArticles != value)
                {
                    _lesArticles = value;
                    OnPropertyChanged(nameof(LesArticles));
                }
            }
        }

        private ObservableCollection<Article>? _filteredArticles;
        public ObservableCollection<Article>? FilteredArticles
        {
            get => _filteredArticles;
            set
            {
                if (_filteredArticles != value)
                {
                    _filteredArticles = value;
                    OnPropertyChanged(nameof(FilteredArticles));
                }
            }
        }

        public void ApplyPriceFilter()
        {
            if (LesArticles != null)
            {
                FilteredArticles = new ObservableCollection<Article>(
                    LesArticles.Where(article => article.Prix <= SelectedPrice)
                );
            }
        }

        private void UpdatePriceSliderRange()
        {
            if (SelectedPrice > MaxPrice)
                SelectedPrice = (int)MaxPrice;

            OnPropertyChanged(nameof(SelectedPrice));
        }

        private void OnMinOrMaxPriceChanged()
        {
            UpdateArticleList(); // Recharge la liste filtrée si nécessaire
        }

        private double _maxPrice = 1000; // Valeur initiale par défaut
        public double MaxPrice
        {
            get => _maxPrice;
            set
            {
                if (_maxPrice != value)
                {
                    _maxPrice = value;
                    OnPropertyChanged(nameof(MaxPrice));
                    UpdatePriceSliderRange();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _selectedPrice;
        public int SelectedPrice
        {
            get => _selectedPrice;
            set
            {
                if (_selectedPrice != value)
                {
                    _selectedPrice = value;
                    OnPropertyChanged(nameof(SelectedPrice));
                    ApplyPriceFilter(); // Applique le filtre
                }
            }
        }

        private Dictionary<int, string> _categories = new()
        {
            { 0, "Tous" },
            { 1, "Jeux Vidéo" },
            { 2, "Multimédia" }
        };

        public ObservableCollection<KeyValuePair<int, string>> Categories { get; private set; }

        private int _selectedCategoryId = 0; // Par défaut, "Tous"
        public int SelectedCategoryId
        {
            get => _selectedCategoryId;
            set
            {
                if (_selectedCategoryId != value)
                {
                    _selectedCategoryId = value;
                    OnPropertyChanged(nameof(SelectedCategoryId));
                    UpdateFilteredArticles();
                }
            }
        }

        private KeyValuePair<int, string> _selectedCategory = new KeyValuePair<int, string>(0, "Tous");

        public KeyValuePair<int, string> SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory.Key != value.Key)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    SelectedCategoryId = value.Key; // Met à jour l'ID sélectionné
                    UpdateFilteredArticles();  // Applique les filtres dès que la catégorie change
                }
            }
        }

        private void UpdateFilteredArticles()
        {
            var articles = App.MonModel.GetAllArticles();

            // Appliquer le filtre par catégorie
            if (SelectedCategoryId > 0)  // Vérifier si la catégorie sélectionnée est autre que "Tous"
            {
                articles = articles.Where(a => a.IdCategorie == SelectedCategoryId).ToList();
            }

            // Appliquer le filtre par prix
            if (SelectedPrice > 0)
            {
                articles = articles.Where(a => a.Prix <= SelectedPrice).ToList();
            }

            // Mise à jour des articles affichés
            LesArticles = new ObservableCollection<Article>(articles);
            FilteredArticles = new ObservableCollection<Article>(articles);
        }

        public PanelAcceuil()
        {
            Categories = new ObservableCollection<KeyValuePair<int, string>>(_categories); // Initialisation des catégories
            LesArticles = new ObservableCollection<Article>(App.MonModel.GetAllArticles());

            if (LesArticles is not null)
                FilteredArticles = new ObservableCollection<Article>(LesArticles); // Initialise avec tous les articles

            this.DataContext = this;

            InitializeComponent();
        }

        public void UpdateArticleList()
        {
           LesArticles = new ObservableCollection<Article>(App.MonModel.GetAllArticles());
        }

        private void OnArticleClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button?.DataContext is Article article)
            {
                MainWindow.Instance?.ShowDetailArticle(article);
            }
        }
    }
}

