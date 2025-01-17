using Avalonia.Controls;
using Projet1.Entite;
using Projet1.Panels;

namespace Projet1.Windows;

public partial class MainWindow : Window
{
    public static MainWindow? Instance;
    public PanelAcceuil panelAcceuil;
    public FormPanel panelConnexion;
    public PanelGestion panelGestion;
    public PanelDetailArticle panelDetailArticle;
    public PanelPanier panelPanier;
    public PanelCommande panelCommande;

    public PanelDetailCommande panelDetailCommande;

    public MainWindow()
    {
        InitializeComponent();
        this.Closing += this.Before_Close_Main_Window;
        Instance = this;
        this.panelAcceuil = new PanelAcceuil();
        this.panelConnexion = new FormPanel();
        this.panelGestion = new PanelGestion();
        this.panelDetailArticle = new PanelDetailArticle();
        this.panelPanier = new PanelPanier();
        this.panelCommande = new PanelCommande();
        this.panelDetailCommande = new PanelDetailCommande();
        this.PanelNavbar.ShowNavbarConnexion();
        this.ShowPanConnexion();
        this.KeyDown += this.panelConnexion.HandleKeyPressed;
    }

    private void Before_Close_Main_Window(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if(App.UserConnected != null){
            App.MonModel.DeleteFromConnectedUser(App.UserConnected.GetId());
        }
    }

    public void ShowPanConnexion()
    {
        MainContent.Content = this.panelConnexion;
        this.panelAcceuil.IsVisible = false;
        this.panelConnexion.IsVisible = true;
        this.panelGestion.IsVisible = false;
        this.panelDetailArticle.IsVisible = false;
        this.panelPanier.IsVisible = false;
        this.panelCommande.IsVisible = false;
        this.panelDetailCommande.IsVisible = false;
    }

    public void ShowPanAcceuil()
    {
        this.panelConnexion.CloseErrorMessage(null,null);
        MainContent.Content = this.panelAcceuil;
        this.panelAcceuil.IsVisible = true;
        this.panelConnexion.IsVisible = false;
        this.panelGestion.IsVisible = false;
        this.panelDetailArticle.IsVisible = false;
        this.panelPanier.IsVisible = false;
        this.panelCommande.IsVisible = false;
        this.panelAcceuil.UpdateArticleList();
        this.panelDetailCommande.IsVisible = false;
    }

    public void ShowPanGestion()
    {
        MainContent.Content = this.panelGestion;
        this.panelAcceuil.IsVisible = false;
        this.panelGestion.IsVisible = true;
        this.panelConnexion.IsVisible = false;
        this.panelDetailArticle.IsVisible = false;
        this.panelPanier.IsVisible = false;
        this.panelCommande.IsVisible = false;
        this.panelDetailCommande.IsVisible = false;
    }

    public void ShowPanMesCommandes(){
        MainContent.Content = this.panelCommande;
        this.panelAcceuil.IsVisible = false;
        this.panelGestion.IsVisible = false;
        this.panelConnexion.IsVisible = false;
        this.panelDetailArticle.IsVisible = false;
        this.panelPanier.IsVisible = false;
        this.panelCommande.IsVisible = true;
        this.panelDetailCommande.IsVisible = false;
        if(App.UserConnected is not null) this.panelCommande.ChargerCommandes(App.UserConnected.GetId());
    }

    public void ShowPanDetailCommandes(int? id_facturation){
        MainContent.Content = this.panelDetailCommande;
        this.panelAcceuil.IsVisible = false;
        this.panelGestion.IsVisible = false;
        this.panelConnexion.IsVisible = false;
        this.panelDetailArticle.IsVisible = false;
        this.panelPanier.IsVisible = false;
        this.panelCommande.IsVisible = false;
        this.panelDetailCommande.IsVisible = true;
        this.panelDetailCommande.ChargerArticles(id_facturation);
    }

    public void ShowDetailArticle(Article? article)
    {
        MainContent.Content = this.panelDetailArticle;
        this.panelAcceuil.IsVisible = false;
        this.panelGestion.IsVisible = false;
        this.panelConnexion.IsVisible = false;
        this.panelDetailArticle.IsVisible = true;
        this.panelPanier.IsVisible = false;
        this.panelCommande.IsVisible = false;
        this.panelDetailCommande.IsVisible = false;
        this.panelDetailArticle.SetArticleDetails(article);
    }

    public void ShowPanPanier()
    {
        MainContent.Content = this.panelPanier;
        this.panelAcceuil.IsVisible = false;
        this.panelGestion.IsVisible = false;
        this.panelConnexion.IsVisible = false;
        this.panelDetailArticle.IsVisible = false;
        this.panelPanier.IsVisible = true;
        this.panelCommande.IsVisible = false;
        this.panelDetailCommande.IsVisible = false;
        this.panelPanier.ChargerArticles();
    }

    public void ShowNavbarConnected()
    {
        PanelNavbar.ShowNavbarConnected();
    }

    public void ShowNavbarConnexion()
    {
        PanelNavbar.ShowNavbarConnexion();
    }
}