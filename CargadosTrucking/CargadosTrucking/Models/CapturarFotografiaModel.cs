using CargadosTrucking.Clases;
using CargadosTrucking.Helpers;
using Imaging.Library;
using Imaging.Library.Entities;
using Imaging.Library.Enums;
using Imaging.Library.Filters.BasicFilters;
using Imaging.Library.Filters.ComplexFilters;
using Imaging.Library.Maths;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace CargadosTrucking.Models
{
    public class CapturarFotografiaModel:BaseViewModel
    {
        public Fototemp fotolocal { get; set; }
        public byte[] image { get; set; }
        public byte[] CurrentImage { get { return image; } set { image = value;OnPropertyChanged(); } }
        private ObservableCollection<Fototemp> es;
        public ObservableCollection<Fototemp> ImagesList { get { return es; } set { es = value; OnPropertyChanged(); } }

        public Command tomarfoto;
        public Command TomarFoto { get { return tomarfoto; } set { tomarfoto = value; OnPropertyChanged(); } }

        private PgetWorkordersJibapp_Result trip;
        public PgetWorkordersJibapp_Result Trip { get { return trip; } set { trip = value;OnPropertyChanged(); } }

        public string drivername;
        public string DriverName { get { return drivername; } set { drivername = value; OnPropertyChanged(); } }        
        public string tripid;
        public string TripId { get { return tripid; } set { tripid = value; OnPropertyChanged(); } }          
        private string wor;
        public string WrokOrder { get { return wor; } set { wor = value; OnPropertyChanged(); } }        
        private string model;

        public string Modelo { get { return model; } set { model = value; OnPropertyChanged(); } }

        private string vim;

        public string VIM { get { return vim; } set { vim = value; OnPropertyChanged(); } }
        private bool eventose;
        public bool Eventosexisten { get { return eventose; } set { eventose = value; OnPropertyChanged(); } }
        public CapturarFotografiaModel(PgetWorkordersJibapp_Result trip) {
            Trip = trip;
            DriverName = trip.DriverName;
            TripId = trip.TripID.ToString();
           // CurrentImage = fotolocal.Foto;
            Modelo =trip.WO_Make+"," +trip.WO_Model+","+trip.WO_Year;
            WrokOrder = trip.OrderID.ToString();
            TomarFoto = new Command(async () => await tomarfotot());
            VIM = trip.WO_VIN;
            ImagesList = new ObservableCollection<Fototemp>();
        }

        public async Task cargardatatrip(INavigation nav)
        {
            genericresult llamada = new genericresult();
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Adding images to Work order " + WrokOrder, new XF.Material.Forms.UI.Dialogs.Configurations.MaterialLoadingDialogConfiguration { BackgroundColor = (Color)Application.Current.Resources["azul"], MessageTextColor = (Color)Application.Current.Resources["blanco"], TintColor = (Color)Application.Current.Resources["blanco"] }))


            {
                List<Photo> imagens = new List<Photo>();

                foreach(var f in ImagesList){ 
                imagens.Add(new Photo { Foto= Convert.ToBase64String(f.Foto),Name=f.FotoNombre });
                }
                llamada = await repoapi.checkin(new Parametrosimages {OrderID=Trip.OrderID,Tripid=Trip.TripID,Imagenes=imagens });
            }

            if (llamada.realizado)
            {
                await mensajetoast("Images saved");

                
                Eventosexisten = true;
               await nav.PopModalAsync();
            }
            else
            {
                await mensajetoast(llamada.Errores);
            }
        }
        public async Task mensajetoast(string Error)
        {
            var color = new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration();
            color.BackgroundColor = (Color)Application.Current.Resources["azul"];
            await MaterialDialog.Instance.SnackbarAsync(message: Error,
            msDuration: 5000, color);
        }

        public async Task tomarfotot()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            FileResult photo = null; ;
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                Debug.WriteLine("No Camera", ":( No camera available.", "OK");
                return;
            }


             photo = await MediaPicker.CapturePhotoAsync();
            
            if (photo != null)
            {
                try
                {

                    using (var stream = await photo.OpenReadAsync())
                        CurrentImage = DependencyService.Get<IMediaService>().reziseImage(ReadFully(stream));

                    ImagesList.Add(new Fototemp { Foto=CurrentImage, FotoNombre= photo.FileName});
                }
                catch {
                  
                }
            }
            if (ImagesList.Count > 0)
                Eventosexisten = true;

            IsBusy = false;

        }

        public void getcurrentimagestorage(string location) {
            fotolocal.Foto = File.ReadAllBytes(location);
        }


        public void removerfoto(Fototemp foto)
        {
            ImagesList.Remove(foto);
        }
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();

            }

        }
    

}
}
