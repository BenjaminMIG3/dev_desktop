using System;
using System.Linq.Expressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Projet1.Panels;

namespace Projet1.Windows;

public partial class MainWindow : Window
{
    public static MainWindow? Instance;
    public PanelAcceuil panelAcceuil;
    public FormPanel panelConnexion;
    public PanelGestion panelGestion;

    public MainWindow()
    {
        InitializeComponent();
        this.Closing += this.Before_Close_Main_Window;
        Instance = this;
        this.panelAcceuil = new PanelAcceuil();
        this.panelConnexion = new FormPanel();
        this.panelGestion = new PanelGestion();
        PanelNavbar.ShowNavbarConnexion();
        this.ShowPanConnexion();
    }

    private void Before_Close_Main_Window(object? sender, System.ComponentModel.CancelEventArgs e){
        if(App.UserConnected != null){
            App.MonModel.DeleteFromConnectedUser(App.UserConnected.GetId());
        }
    }

    public void ShowPanConnexion(){
        MainContent.Content = this.panelConnexion;
        this.panelAcceuil.IsVisible = false;
        this.panelConnexion.IsVisible = true;
    }

    public void ShowPanAcceuil(){
        this.panelConnexion.CloseErrorMessage(null,null);
        MainContent.Content = this.panelAcceuil;
        this.panelAcceuil.IsVisible = true;
        this.panelConnexion.IsVisible = false;
    }

    public void ShowPanGestion(){
        MainContent.Content = this.panelGestion;
        this.panelAcceuil.IsVisible = false;
        this.panelGestion.IsVisible = true;
    }

    public void ShowNavbarConnected(){
        PanelNavbar.ShowNavbarConnected();
    }

    public void ShowNavbarConnexion(){
        PanelNavbar.ShowNavbarConnexion();
    }
}