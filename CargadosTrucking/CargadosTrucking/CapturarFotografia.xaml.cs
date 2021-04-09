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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CargadosTrucking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CapturarFotografia : ContentPage
    {
        CapturarFotografiaModel context;
        public delegate void EventHandler(Fototemp status);
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

        private async void editor_ImageSaving(object sender,ImageSavingEventArgs args)
        {
            args.Cancel = true;
            var stream = args.Stream;
            var bytes = ReadFully(stream);
            string   newFile = DependencyService.Get<IMediaService>().SaveImageFromByte(bytes,"crop_"+context.fotolocal.FotoNombre);
            EventPass(new Fototemp {Foto=bytes,FotoNombre= "crop_" + context.fotolocal.FotoNombre });
            await Navigation.PopAsync();
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
           await Navigation.PopAsync();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            context.CurrentImage = null;


        }

        private void AceptarFoto(object sender, EventArgs e)
        {

       
        }

        private async void finalizartrip(object sender, EventArgs e)
        {
           await context.cargardatatrip(Navigation);
           
        }

        private void removerfoto(object sender, EventArgs e)
        {
            context.removerfoto((Fototemp)((Button)sender).BindingContext);

        }
    }
}