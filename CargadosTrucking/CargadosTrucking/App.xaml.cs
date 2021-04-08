using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CargadosTrucking
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDE3MjYzQDMxMzgyZTM0MmUzMGdVWkZ3RElhTHprOFNrOHhRc25PN0NyZkZPcEM4M3VTSHBlaXUvNHdPL0U9");

            InitializeComponent();
            XF.Material.Forms.Material.Init(this);

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
