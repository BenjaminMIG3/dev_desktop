using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Projet1.Entite;
using Projet1.Windows;

namespace Projet1.Panels
{
    public partial class PanelDetailArticle : UserControl
    {
        private Article? article;
        public PanelDetailArticle()
        {
            InitializeComponent();
        }

        public void SetArticleDetails(Article? article)
        {
            if(article is not null){
                this.article = article;
                NomArticle.Text = article.Nom ;

                if (article.DataImage != null)
                {
                    using var ms = new MemoryStream(article.DataImage);
                    ImageArticle.Source = new Bitmap(ms);
                }

                PrixArticle.Text = $"{article.Prix} €";

                DescriptionArticle.Text = article.Description;
                if(App.UserConnected is not null && App.MonModel.IsItemInUrCard(App.UserConnected.GetId(), article.Id)){
                    btnAddToCart.IsEnabled = false;
                    MessageConfirmation.Text = "Article déja dans le panier.";
                } else {
                    btnAddToCart.IsEnabled = true;
                    MessageConfirmation.Text = string.Empty;
                }
            }
        }

        private void OnRetourClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MainWindow.Instance?.ShowPanAcceuil();
        }

        private void OnAjouterAuPanierClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(App.UserConnected is not null && this.article is not null){
                var result = App.MonModel.InsertArticleIntoPanierArticle(App.UserConnected.GetId(), this.article.Id);

                if (result.ContainsKey(true) && result[true] == 0)
                {
                    MessageConfirmation.Text = "Article ajouté au panier avec succès.";
                    MessageConfirmation.Background = Brushes.Green;
                    btnAddToCart.IsEnabled = false;
                }
                else if (result.ContainsKey(false) && result[false] == 1)
                {
                    MessageConfirmation.Text = "Article déja dans le panier.";
                    MessageConfirmation.Background = Brushes.Red;
                    btnAddToCart.IsEnabled = true;
                }

                MessageConfirmation.IsVisible = true;
                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    Avalonia.Threading.Dispatcher.UIThread.Invoke(() =>
                    {
                        MessageConfirmation.IsVisible = false;
                    });
                });
            }
        }

    }
}
