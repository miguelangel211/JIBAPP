using CargadosTrucking.Clases;
using CargadosTrucking.Helpers;
using CargadosTrucking.Models;
using Imaging.Library;
using Imaging.Library.Entities;
using Imaging.Library.Enums;
using Imaging.Library.Filters.BasicFilters;
using Imaging.Library.Filters.ComplexFilters;
using Imaging.Library.Maths;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            checkifapialreadyset();
          this.BindingContext=  Context = new MainPageViewModel(Navigation);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        //  await  Context.actualizarviajedata();
        }
        private async void tappedservice(object sender, ItemTappedEventArgs e)
        {
            var tempage = new CapturarFotografia(((PgetWorkordersJibapp_Result)e.Item));
            tempage.EventPass += updatedata;
            await Navigation.PushModalAsync(tempage);
        }

        public async void updatedata()
        {
          await  Context.actualizarviajedata();
        }
        private async void checkifapialreadyset() {
          string apivalue =   await SecureStorage.GetAsync("rutaapiJIB");
            if (string.IsNullOrWhiteSpace(apivalue))
                await SecureStorage.SetAsync("rutaapiJIB", "http://datserver.ddns.net:8089/JIBUnitedapi/");
            

        }


        private  void finalizarviaje(object sender, EventArgs e)
        {

            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Configuracion());

        }

        private void removerfoto(object sender, EventArgs e)
        {
            Context.removerfoto((Fototemp)((Button)sender).BindingContext);

        }
     

    }
}
