using Projet1.Entite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Projet1.Windows;

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
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int SelectedPrice { get; set; } = 500;

        public PanelAcceuil()
        {
            LesArticles = new ObservableCollection<Article>(App.MonModel.GetAllArticles());
            
            if (!(LesArticles != null && LesArticles.Count > 0))
            {
                Console.WriteLine("LesArticles est vide ou nul.");
            }

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

