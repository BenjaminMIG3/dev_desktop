using Avalonia.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using Projet1.Entite;
using System;
using System.ComponentModel;
using Avalonia.Media;
using Avalonia.Threading;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Diagnostics;

namespace Projet1.Panels
{
    public partial class PanelDetailCommande : UserControl, INotifyPropertyChanged
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

        public PanelDetailCommande()
        {
            InitializeComponent();
            DataContext = this;
            this._total = "0";

            ChargerArticles(0);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChargerArticles(int? id_facturation)
        {
            try
            {
                if(App.UserConnected is not null)
                {
                    var articles = App.MonModel.GetArticleByIdFacturation(id_facturation);

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
            BoutonGenererFacture.IsVisible = hasArticles;
            BoutonGenererFacture.IsEnabled = hasArticles;
            TotalCommande.IsVisible = hasArticles;
        }

        private void OnBoutonGenererFactureClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (App.UserConnected is not null)
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Facture - Commande";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont font = new XFont("Arial", 12);

                int yPosition = 20;

                gfx.DrawString($"Facture pour l'utilisateur {App.UserConnected.GetNom() + " " + App.UserConnected.GetPrenom()}", font, XBrushes.Black, new XPoint(20, yPosition));
                yPosition += 20;

                gfx.DrawString($"Date de la commande: {DateTime.Now.ToString("dd/MM/yyyy")}", font, XBrushes.Black, new XPoint(20, yPosition));
                yPosition += 20;

                gfx.DrawString("Détails des articles:", font, XBrushes.Black, new XPoint(20, yPosition));
                yPosition += 20;

                foreach (var article in LesArticles)
                {
                    gfx.DrawString($"- {article.Nom} : {article.Prix:0.#0} €", font, XBrushes.Black, new XPoint(20, yPosition));

                    yPosition += 20;
                }

                gfx.DrawString($"Total : {LesArticles.Sum(a => a.Prix):0.#0} €", font, XBrushes.Black, new XPoint(20, yPosition));

                yPosition += 20;

                string filePath = "facture.pdf";
                document.Save(filePath);

                MessageValidation.Text = "Facture générée avec succès.";
                MessageValidation.Foreground = Brushes.Green;

                Process.Start("open", filePath);
            }
            else
            {
                MessageValidation.Text = "Erreur génération de la facture.";
                MessageValidation.Foreground = Brushes.Red;
            }

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
