using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CargadosTrucking
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var d = new NavigationPage(new MainPage());
            d.Style = (Style)Xamarin.Forms.Application.Current.Resources["Mainpage"];
            MainPage = d;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
