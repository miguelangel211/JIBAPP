using CargadosTrucking.Helpers;
using CargadosTrucking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace CargadosTrucking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        ApiCalls apirepo;
        public Login()
        {
            InitializeComponent();
            apirepo = new ApiCalls();
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await checkifapialreadyset();
            var result = await SecureStorage.GetAsync("sessiondriverJIB");
            if (string.IsNullOrWhiteSpace(result)|| result=="null")
            {
                loading.IsVisible = false; loading.IsRunning = false;
                loginpage.IsVisible = true;
            }
            else {
                var d = new NavigationPage(new MainPage());
                d.Style = (Style)Xamarin.Forms.Application.Current.Resources["Mainpage"];
                // await  Navigation.PushAsync(d);
                App.Current.MainPage = d;
            }
        }
        private async Task checkifapialreadyset()
        {
            string apivalue = await SecureStorage.GetAsync("rutaapiJIB");
            if (string.IsNullOrWhiteSpace(apivalue))
                await SecureStorage.SetAsync("rutaapiJIB", "http://datserver.ddns.net:8089/JIBUnitedapi/");


        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            genericdatasingle<Driver> result = new genericdatasingle<Driver>();
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Login for  " + cellphone.Text, new XF.Material.Forms.UI.Dialogs.Configurations.MaterialLoadingDialogConfiguration { BackgroundColor = (Color)Application.Current.Resources["azul"], MessageTextColor = (Color)Application.Current.Resources["blanco"], TintColor = (Color)Application.Current.Resources["blanco"] }))
            {
                 result = await apirepo.CheckCellphone(cellphone.Text);

            }
            if (result.realizado)
            {
                await SecureStorage.SetAsync("sessiondriverJIB", JsonConvert.SerializeObject(result.Result));

                await toasts.mensajetoast("Login successful!!");
                var d = new NavigationPage(new MainPage());
                d.Style = (Style)Xamarin.Forms.Application.Current.Resources["Mainpage"];
                // await  Navigation.PushAsync(d);
                App.Current.MainPage = d;
            }
            else
                await toasts.mensajetoast(result.Errores);

        }
    }
}