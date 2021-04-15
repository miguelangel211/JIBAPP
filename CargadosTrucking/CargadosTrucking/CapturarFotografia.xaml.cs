using CargadosTrucking.Clases;
using CargadosTrucking.Helpers;
using CargadosTrucking.Models;
using Syncfusion.SfImageEditor.XForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CargadosTrucking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CapturarFotografia : ContentPage
    {
        CapturarFotografiaModel context;
        public delegate void EventHandler();
        public event EventHandler EventPass;
        public CapturarFotografia(PgetWorkordersJibapp_Result trip)
        {
            InitializeComponent();
            this.BindingContext = context = new CapturarFotografiaModel(trip,Navigation);


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
     
        }

        private async void editor_ImageSaved(object sender, ImageSavedEventArgs args)
        {

            

        }



        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();

            }

        }

        private void CropEditor_ImageLoaded(object sender, ImageLoadedEventArgs args)
        {


        }

        private async void cerrarforma(object sender, EventArgs e)
        {
           await Navigation.PopModalAsync();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            context.CurrentImage = null;


        }



        private async void finalizartrip(object sender, EventArgs e)
        {
            if (context.ImagesList.Count == 0) {
               await toasts.mensajetoast("There aren't images assigned to this order");
                return;
            }

            bool realizado =  await context.cargardatatrip();
            if (realizado)
            {
                EventPass();
                await Navigation.PopModalAsync();
            }



        }

        private void removerfoto(object sender, EventArgs e)
        {
            context.removerfoto((Fototemp)((Button)sender).BindingContext);

        }

        private async void imagentapped(object sender, ItemTappedEventArgs e)
        {
           await context.openeditorfoto((Fototemp)(e.Item), true);
        }
    }
}