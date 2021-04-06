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


        private void tappedservice(object sender, ItemTappedEventArgs e)
        {

        }


        private async void checkifapialreadyset() {
          string apivalue =   await SecureStorage.GetAsync("rutaapi");
            if (string.IsNullOrWhiteSpace(apivalue))
                await SecureStorage.SetAsync("rutaapi", "http://cargados.ddns.net/CargadosApi/");
            

        }


        private async void finalizarviaje(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(Context.Viajenumero))
            {
                await DisplayAlert("No se pudo registrar viaje", "Ingrese un numero de viaje", "OK");

            }
            else
            {
                finalizarbutton.Text = "Guardando";
                finalizarbutton.IsEnabled = false;
                var resultado= await  Context.enviardatosviaje();
                if (resultado.realizado)
                {
                    finalizarbutton.Text = "Finalizar";
                    finalizarbutton.IsEnabled = true;
                    await Navigation.PushAsync(new Confirmacion(Context.Viajenumero));
                }
                else{
                    await DisplayAlert("No se pudo registrar viaje", resultado.Errores, "OK");
                    finalizarbutton.Text = "Finalizar";
                    finalizarbutton.IsEnabled = true;
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Configuracion());

        }

        private void removerfoto(object sender, EventArgs e)
        {
            Context.removerfoto((Fototemp)((Button)sender).BindingContext);

        }
        /*
private async void GetPixelMapFromGallery()
{
   if (!CrossMedia.Current.IsPickPhotoSupported)
   {
       await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
       return;
   }
   var file = await CrossMedia.Current.PickPhotoAsync();

   if (file == null)
       return;

   switch (Device.RuntimePlatform)
   {
       case Device.Android:
           Source = AndroidHelper.GetPixelMap(file.GetStream());
           break;



   }

   imagenfinaldeescanner.Source = LoadFromPixel(Source);
}

private void tomarfoto(object sender, EventArgs e)
{
 //  PixelMap Source = new PixelMap(160, 150);

   var imaging = new ImagingManager(Source);
   var scale = 0.4;
   //source is PixelMap, you can find samples how to convert image to PixelMap
   imaging.AddFilter(new BicubicFilter(scale));    //Downscaling
   imaging.AddFilter(new CannyEdgeDetector());     //This filter contains Grayscale and Gaussian filter in it
   imaging.Render();                               //Renders the image to use it further use

   var blobCounter = new BlobCounter()
   {
       ObjectsOrder = ObjectsOrder.Size
   };
   imaging.AddFilter(blobCounter);

   imaging.Render();

   //Following code finds largest quadratical blob
   List<Imaging.Library.Entities.Point> corners = null;
   var blobs = blobCounter.GetObjectsInformation();
   foreach (var blob in blobs)
   {
       var points = blobCounter.GetBlobsEdgePoints(blob);

       var shapeChecker = new SimpleShapeChecker();

       if (shapeChecker.IsQuadrilateral(points, out corners))
           break;
   }

   imaging.UndoAll();                              //Undo every filters applied

   var edgePoints = new EdgePoints();
   edgePoints.SetPoints(corners.ToArray());
   edgePoints = edgePoints.ZoomIn(scale);          //Corrects points that found on downscaled image to original
   imaging.AddFilter(new QuadrilateralTransformation(edgePoints, true));

   imaging.Render();

   imagenfinaldeescanner.Source = LoadFromPixel(imaging.Output);

}


private ImageSource LoadFromPixel(PixelMap pixelMap)
{
   switch (Device.RuntimePlatform)
   {
       case Device.Android:
           return AndroidHelper.LoadImageFromPixelMap(pixelMap);



       default:
           return null;
   }
}*/
    }
}
