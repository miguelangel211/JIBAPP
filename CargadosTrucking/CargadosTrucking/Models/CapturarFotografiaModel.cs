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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace CargadosTrucking.Models
{
    public class CapturarFotografiaModel : BaseViewModel
    {
        public Fototemp fotolocal { get; set; }
        public byte[] image { get; set; }
        public byte[] CurrentImage { get { return image; } set { image = value; OnPropertyChanged(); } }
        private ObservableCollection<Fototemp> es;
        public ObservableCollection<Fototemp> ImagesList { get { return es; } set { es = value; OnPropertyChanged(); } }

        public Command tomarfoto;
        public Command TomarFoto { get { return tomarfoto; } set { tomarfoto = value; OnPropertyChanged(); } }

        private PgetWorkordersJibapp_Result trip;
        public PgetWorkordersJibapp_Result Trip { get { return trip; } set { trip = value; OnPropertyChanged(); } }

        #region trip values
        public string drivername;
        public string DriverName { get { return drivername; } set { drivername = value; OnPropertyChanged(); } }
        public string tripid;
        public string TripId { get { return tripid; } set { tripid = value; OnPropertyChanged(); } }
        private string wor;
        public string WrokOrder { get { return wor; } set { wor = value; OnPropertyChanged(); } }
        private string model;
        public string Modelo { get { return model; } set { model = value; OnPropertyChanged(); } }
        private bool savingfields;
        public bool SavingFieldsVisible { get { return savingfields; } set { savingfields = value; OnPropertyChanged(); } }        
        private bool filled;
        public bool FilledOrder { get { return filled; } set { filled = value; OnPropertyChanged(); } }

        private string vim;

        public string VIM { get { return vim; } set { vim = value; OnPropertyChanged(); } }
        private string ordeni;


        #endregion

        #region checks
        public bool checkba;
        public bool Baterycheck { get { return checkba; } set { checkba = value;OnPropertyChanged(); } }        
        public bool checkcata;
        public bool Catalizercheck { get { return checkcata; } set { checkcata = value;OnPropertyChanged(); } }        
        public bool checkkey;
        public bool Keyrcheck { get { return checkkey; } set { checkkey = value;OnPropertyChanged(); } }      
        public bool checkrad;
        public bool Radiocheck { get { return checkrad; } set { checkrad = value;OnPropertyChanged(); } }
        #endregion

        private bool eventose;
        public bool Eventosexisten { get { return eventose; } set { eventose = value; OnPropertyChanged(); } }
        INavigation Navigationl;
        public CapturarFotografiaModel(PgetWorkordersJibapp_Result trip, INavigation nav) {
            Navigationl = nav;
            Trip = trip;
            DriverName = trip.DriverName;
            TripId = trip.TripID.ToString();
            Modelo = trip.WO_Make + "," + trip.WO_Model + "," + trip.WO_Year;
            WrokOrder = trip.OrderID.ToString();
            TomarFoto = new Command(async () => await tomarfotot());
            VIM = trip.WO_VIN;
            ImagesList = new ObservableCollection<Fototemp>();
            checkphotoquantity();
            FilledOrder = SavingFieldsVisible;
        }

        public async Task<bool> cargardatatrip()
        {
            genericdatasingle<string> llamada = new genericdatasingle<string>();
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Adding images to Work order " + WrokOrder, new XF.Material.Forms.UI.Dialogs.Configurations.MaterialLoadingDialogConfiguration { BackgroundColor = (Color)Application.Current.Resources["azul"], MessageTextColor = (Color)Application.Current.Resources["blanco"], TintColor = (Color)Application.Current.Resources["blanco"] }))


            {
                List<Photo> imagens = new List<Photo>();
                var imgdef = ImagesList.FirstOrDefault();
                string cityzip = "";
                var resultlocation = await repoapi.Getlocation(imgdef.lat, imgdef.@long);
                if (resultlocation.realizado)
                    cityzip = resultlocation.Result.locality + "," + resultlocation.Result.principalSubdivision + "," + resultlocation.Result.postcode;
                foreach (var f in ImagesList) {
                    imagens.Add(new Photo { Foto = Convert.ToBase64String(f.Foto), Name = f.FotoNombre, Comentario = f.Comentario, lat = f.lat, @long = f.@long, CityZip = cityzip });
                }
                llamada = await repoapi.checkin(new Parametrosimages { OrderID = Trip.OrderID, Tripid = Trip.TripID, Imagenes = imagens, DriverName = DriverName,Batery=Baterycheck,Catalizer=Catalizercheck,Key=Keyrcheck,Radio=Radiocheck });
            }

            if (llamada.realizado)
            {
                await mensajetoast(llamada.Result);


                Eventosexisten = true;
                return true;
            }
            else
            {
                await mensajetoast(llamada.Errores);
            }
            return false;
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
                        CurrentImage = DependencyService.Get<IMediaService>().phototakenoriginal(ReadFully(stream));
                    await openeditorfoto(new Fototemp { Foto = CurrentImage, FotoNombre = photo.FileName });


                }
                catch {

                }
            }

            IsBusy = false;

        }
        internal async Task openeditorfoto(Fototemp FOTO, bool isediting = false) {
            var newpage = new Editarfotografia(FOTO, isediting);
            newpage.EventPass += setimagen;
            await Navigationl.PushModalAsync(newpage); ;
        }

        public void getcurrentimagestorage(string location) {
            fotolocal.Foto = File.ReadAllBytes(location);
        }



        public void removerfoto(Fototemp foto)
        {
            ImagesList.Remove(foto);
            checkphotoquantity();
        }

        public void setimagen(Fototemp data)
        {
            bool finded = false;
            for (int i = 0; i < ImagesList.Count; i++) {

                if (ImagesList[i].FotoNombre == data.FotoNombre) {
                    ImagesList[i].Foto = data.Foto;
                    ImagesList[i].Comentario = data.Comentario;
                    finded = true;
                    break;
                }
            }
            if (!finded)
                ImagesList.Add(data);
            Eventosexisten = true;
            checkphotoquantity();
        }
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();

            }

        }
        private void checkphotoquantity(){
            if ((ImagesList.Count + (Trip.fotos??0)) >= 5)
                SavingFieldsVisible = true;
            else
                SavingFieldsVisible= false;
        }

}
}
