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
            UpdateArticleList();
        }

        private double _maxPrice = 1000;
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
                    ApplyPriceFilter();
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

        private int _selectedCategoryId = 0;
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
                    SelectedCategoryId = value.Key;
                    UpdateFilteredArticles();
                }
            }
        }

        private void UpdateFilteredArticles()
        {
            var articles = App.MonModel.GetAllArticles();

            if (SelectedCategoryId > 0) 
            {
                articles = articles.Where(a => a.IdCategorie == SelectedCategoryId).ToList();
            }

            if (SelectedPrice > 0)
            {
                articles = articles.Where(a => a.Prix <= SelectedPrice).ToList();
            }

            LesArticles = new ObservableCollection<Article>(articles);
            FilteredArticles = new ObservableCollection<Article>(articles);
        }

        public PanelAcceuil()
        {
            Categories = new ObservableCollection<KeyValuePair<int, string>>(_categories); 
            LesArticles = new ObservableCollection<Article>(App.MonModel.GetAllArticles());

            if (LesArticles is not null)
                FilteredArticles = new ObservableCollection<Article>(LesArticles);

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

