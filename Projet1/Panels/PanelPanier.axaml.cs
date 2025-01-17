using Avalonia.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using Projet1.Entite;
using System;
using System.ComponentModel;
using Avalonia.Media;
using Avalonia.Threading;
using System.Globalization;
using Avalonia.Interactivity;

namespace Projet1.Panels
{
    public partial class PanelPanier : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Article> _lesArticles = new ObservableCollection<Article>();

        public ObservableCollection<Article> LesArticles
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

        private string _total;

        public string Total
        {
            get => _total;
            set
            {
                if (_total != value)
                {
                    _total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public PanelPanier()
        {
            InitializeComponent();
            DataContext = this;
            this._total = "0";
            ChargerArticles();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChargerArticles()
        {
            try
            {
                if(App.UserConnected is not null){
                    var articles = App.MonModel.GetArticleByIdUser(App.UserConnected.GetId());

                    if (articles != null)
                    {
                        LesArticles.Clear();
                        foreach (var article in articles)
                        {
                            LesArticles.Add(article);
                        }
                    }

                    Total = LesArticles.Sum(a => a.Prix).ToString("C");
                }

                MettreAJourVisibilite();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des articles : {ex.Message}");
            }
        }

        private void MettreAJourVisibilite() 
        {
            bool hasArticles = LesArticles.Count > 0;
            MessagePanierVide.IsVisible = !hasArticles;
            ListeArticles.IsVisible = hasArticles;
            BoutonPayer.IsVisible = hasArticles;
            BoutonPayer.IsEnabled = hasArticles;
            TotalPanier.IsVisible = hasArticles;
        }

        private void OnRemoveArticleClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var article = button?.DataContext as Article;

            if (article != null)
            {
                RemoveArticleFromPanier(article);
            }
        }

        private void RemoveArticleFromPanier(Article article)
        {
            LesArticles.Remove(article);

            Total = LesArticles.Sum(a => a.Prix).ToString("C");

            if (App.UserConnected is not null) App.MonModel.DeleteArticleFromMyPanier(App.UserConnected.GetId(), article.Id);
            
            MettreAJourVisibilite();
        }

        private void OnBoutonPayerClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) 
        {
            if (App.UserConnected is not null && App.MonModel.DeleteArticleFromPanier(App.UserConnected.GetId()))
            {
                MessageValidation.Text = "Panier validé avec succès.";
                MessageValidation.Foreground = Brushes.Green;
                var totalClean = this.Total.Replace(" ", "").Replace("€", "").Trim();
                if (decimal.TryParse(totalClean, NumberStyles.Number, CultureInfo.CurrentCulture, out var totalDecimal))
                {
                    Facturation facturation = new Facturation(App.UserConnected.GetId(), null, totalDecimal, DateTime.Now);
                    App.MonModel.InsertIntoFacturation([.. this.LesArticles], facturation);
                    this.LesArticles.Clear();
                }
                else
                {
                    MessageValidation.Text = "Erreur : Total invalide.";
                    MessageValidation.Foreground = Brushes.Red;
                }
                
                this.LesArticles.Clear();
            }
            else
            {
                MessageValidation.Text = "Erreur validation Panier.";
                MessageValidation.Foreground = Brushes.Red;
            }
            this.MettreAJourVisibilite();

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            timer.Tick += (s, args) =>
            {
                MessageValidation.Text = string.Empty;
                timer.Stop();
            };
            timer.Start();
        }
    }
}
