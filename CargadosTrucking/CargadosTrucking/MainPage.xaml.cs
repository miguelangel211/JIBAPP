using CargadosTrucking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CargadosTrucking
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        MainPageViewModel Context;
        public MainPage()
        {
            InitializeComponent();
          this.BindingContext=  Context = new MainPageViewModel(Navigation);
        }


        private void tappedservice(object sender, ItemTappedEventArgs e)
        {

        }

        private void finalizarviaje(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Confirmacion(Context.Viajenumero));
        }
    }
}
