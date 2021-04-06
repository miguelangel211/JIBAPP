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
        public CapturarFotografia(Fototemp fotoriginal)
        {
            InitializeComponent();
            this.BindingContext = context = new CapturarFotografiaModel(fotoriginal);
            Editorfoto.SetToolbarItemVisibility("effects,shape,text,path,save", false);


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Editorfoto.ImageSaved += editor_ImageSaved;
            Editorfoto.ImageLoaded += CropEditor_ImageLoaded;
            Editorfoto.ImageSaving += editor_ImageSaving;
        }

        private async void editor_ImageSaved(object sender, ImageSavedEventArgs args)
        {
          //  string savedLocation = args.Location;
            //context.getcurrentimagestorage(savedLocation);
          //  EventPass(context.fotolocal);
            

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
           //var size= Editorfoto.ActualImageRenderedBounds;
          // var foto= (Editorfoto.Source as BitmapSource);
            //if()
            Editorfoto.Rotate();
            Editorfoto.ToggleCropping(true, 0);

        }

        private async void cerrarforma(object sender, EventArgs e)
        {
           await Navigation.PopAsync();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            context.CurrentImage = null;
            Editorfoto.ImageSaved -= editor_ImageSaved;
            Editorfoto.ImageLoaded -= CropEditor_ImageLoaded;
            Editorfoto.ImageSaving -= editor_ImageSaving;

        }

        private void AceptarFoto(object sender, EventArgs e)
        {
            Editorfoto.Crop();

            Editorfoto.Save();
       
        }
    }
}