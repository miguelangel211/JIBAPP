using CargadosTrucking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CargadosTrucking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Confirmacion : ContentPage
    {
        ConfirmacionModel Context;

        public Confirmacion(string viaje)
        {
            InitializeComponent();
            this.BindingContext = Context = new ConfirmacionModel(viaje);
        }

        private void nuevoviaje(object sender, EventArgs e)
        {
            var d = new NavigationPage(new MainPage());
            d.Style = (Style)Xamarin.Forms.Application.Current.Resources["Mainpage"];
            App.Current.MainPage = d;
            
        }

        private void cerrarapp(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();

        }
    }
}