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
    public partial class Editarfotografia : ContentPage
    {
        EditarFotoModel context;
        public delegate void EventHandler(Fototemp status);
        public event EventHandler EventPass;
        bool Isedit;
        public Editarfotografia(Fototemp fotoriginal,bool isediting=false)
        {
            InitializeComponent();
            Isedit = isediting;
            this.BindingContext = context = new EditarFotoModel(fotoriginal);

            Editorfoto.ToolbarSettings.ToolbarItemSelected += ToolbarSettings_ToolbarItemSelected;
            Editorfoto.ToolbarSettings.IsVisible = false;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
           
            Editorfoto.ImageLoaded += CropEditor_ImageLoaded;
            Editorfoto.ImageSaving += editor_ImageSaving;
        }

 
               
  

        private void ToolbarSettings_ToolbarItemSelected(object sender, ToolbarItemSelectedEventArgs e)
        {
            if (e.ToolbarItem.Text == "crop")
            {
                Editorfoto.ToggleCropping(true, 0);
                croppedcontrols.IsVisible = true;
            }
            if (e.ToolbarItem.Text == "rotate")
            {
                Editorfoto.Rotate();
            }            
            if (e.ToolbarItem.Text == "photo")
            {
                context.TomarFoto.Execute(null);
            }
        }

        private async void editor_ImageSaving(object sender, ImageSavingEventArgs args)
        {
            args.Cancel = true;
            var stream = args.Stream;
            var bytes = ReadFully(stream);
            try
            {
                bytes = DependencyService.Get<IMediaService>().reziseImage(bytes);
            }
            catch (Exception ex)
            {

            }
            var location = await Geolocation.GetLastKnownLocationAsync();
     
            EventPass(new Fototemp { Foto = bytes, FotoNombre =  context.fotolocal.FotoNombre,Comentario= context.Comments,lat=location.Latitude.ToString(),@long=location.Longitude.ToString() });
            await Navigation.PopModalAsync();
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
            if (Isedit) {
                Isedit = false;
                return;
            }
          //  Editorfoto.Rotate();
        }

        private async void cerrarforma(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            context.CurrentImage = null;
            Editorfoto.ImageLoaded -= CropEditor_ImageLoaded;
            Editorfoto.ImageSaving -= editor_ImageSaving;

        }

        private void AceptarFoto(object sender, EventArgs e)
        {
            Editorfoto.Crop();

            Editorfoto.Save();

        }

        private void cerrarcrop(object sender, EventArgs e)
        {
            Editorfoto.ToggleCropping(false);
            croppedcontrols.IsVisible = false;
            toolscopntrols.IsVisible = true;

        }

        private void guardarcrop(object sender, EventArgs e)
        {
            Editorfoto.Crop();
            Editorfoto.ToggleCropping(false);
            Editorfoto.Crop();
            croppedcontrols.IsVisible = false;
            toolscopntrols.IsVisible = true;

        }

        private void rotatebutton(object sender, EventArgs e)
        {
            Editorfoto.Rotate();

        }

        private void cropbutton(object sender, EventArgs e)
        {
            Editorfoto.ToggleCropping(true, 0);
            croppedcontrols.IsVisible = true;
            toolscopntrols.IsVisible = false;

        }
    }
}