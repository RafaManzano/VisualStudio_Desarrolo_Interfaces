﻿using _14_CRUD_PersonasUWP_Entidades;
using _14_CRUD_PersonasUWP_UI.ViewModels.Utiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace _14_CRUD_PersonasUWP_UI
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public modeloVista ViewModel { get; }
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = (modeloVista)this.DataContext;
        }

        private void ListaBotones_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ListView listView = (ListView)sender;
            menuFlyout.ShowAt(listView, e.GetPosition(listView));
            clsPersona personaSeleccionada = (clsPersona)((FrameworkElement)e.OriginalSource).DataContext;
            this.listview.SelectedItem = personaSeleccionada;
        }

        
    }
}
