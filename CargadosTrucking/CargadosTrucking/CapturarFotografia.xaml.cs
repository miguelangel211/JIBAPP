using CargadosTrucking.Clases;
using CargadosTrucking.Models;
using Syncfusion.SfImageEditor.XForms;
using System;
using System.Collections.Generic;
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
        }

        private async void editor_ImageSaved(object sender, ImageSavedEventArgs args)
        {
            string savedLocation = args.Location;
            context.getcurrentimagestorage(savedLocation);
            EventPass(context.fotolocal);
            await Navigation.PopAsync();

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
            Editorfoto.ImageSaved -= editor_ImageSaved;
            Editorfoto.ImageLoaded -= CropEditor_ImageLoaded;

        }

        private void AceptarFoto(object sender, EventArgs e)
        {
            Editorfoto.Crop();

            Editorfoto.Save();
       
        }
    }
}