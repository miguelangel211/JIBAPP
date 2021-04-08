using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CargadosTrucking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Configuracion : ContentPage
    {
        ApiCalls repo;
        public Configuracion()
        {
            InitializeComponent();
            repo = new ApiCalls();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var urlapibase = await SecureStorage.GetAsync("rutaapiJIB");
            if (urlapibase == "" || urlapibase == null)
                urlapibase = "http://datserver.ddns.net:8089/JIBUnitedapi/";
            rutaapi.Text = urlapibase;

        }



        async void clickonchangerutaapi(object senser, EventArgs args)
        {
            if (passw.Text == "jibunited123456")
            {
                await SecureStorage.SetAsync("rutaapiJIB", rutaapi.Text.Trim());
                await DisplayAlert("Api", "Ruta api guardada: ", "OK");
            }
            else {
                await DisplayAlert("Api", "Password incorrecto", "OK");

            }

        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            probando.IsRunning = true;
            // await SecureStorage.SetAsync("rutaapi", rutaapi.Text.Trim());
            var rep = await repo.Pruebadeconexion2(rutaapi.Text.Trim());
            probando.IsRunning = false;

            await DisplayAlert ("Resultado",rep.Errores, "OK");
        }

        void cerrarclicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }

    }
}