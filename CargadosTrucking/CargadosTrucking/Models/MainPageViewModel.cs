using CargadosTrucking.Clases;
using CargadosTrucking.Helpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public class MainPageViewModel : BaseViewModel
    {
        private string nameuser;
        private bool eventose;
        public bool Eventosexisten { get { return eventose; } set { eventose = value; OnPropertyChanged(); } }
        public string NombreUsuario { get { return nameuser; } set { nameuser = value; OnPropertyChanged(); } }

        private string viajeno;
        public string Viajenumero { get { return viajeno; } set { viajeno = value; OnPropertyChanged(); } }

        private ObservableCollection<PgetWorkordersJibapp_Result> es { get; set; }
        public ObservableCollection<PgetWorkordersJibapp_Result> ImagesList { get { return es; } set { es = value; OnPropertyChanged(); } }


        public Command tomarfoto;
        public Command TomarFoto { get { return tomarfoto; } set { tomarfoto = value; OnPropertyChanged(); } }
        public Command selecfoto;
        public Command SeleccionarFoto { get { return selecfoto; } set { selecfoto = value; OnPropertyChanged(); } }        
        
        public Command cargartrip;
        public Command CargarTrip { get { return cargartrip; } set { cargartrip = value; OnPropertyChanged(); } }
        INavigation localNavigation;


        public string drivername;
        public string DriverName { get { return drivername; } set { drivername = value;OnPropertyChanged(); } }
        public MainPageViewModel(INavigation nav)
        {
            TomarFoto = new Command(async () => await tomarfotot());
            SeleccionarFoto = new Command(async () => await Seleccionarfotosgallery());
            CargarTrip = new Command(async()=>await cargardatatrip());
            Viajenumero = "";
            ImagesList = new ObservableCollection<PgetWorkordersJibapp_Result>();
            Eventosexisten = false;
            localNavigation = nav;
            DriverName = "NA";
        }

        public async Task cargardatatrip() {
            if (IsBusy)
                return;
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(Viajenumero))
            {
                await mensajetoast("Please add the trip number");
                IsBusy = false;
                return;
            }
            genericdatar<PgetWorkordersJibapp_Result> llamada = new genericdatar<PgetWorkordersJibapp_Result>();
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Searching WorkOrders for trip "+Viajenumero,new XF.Material.Forms.UI.Dialogs.Configurations.MaterialLoadingDialogConfiguration {BackgroundColor = (Color)Application.Current.Resources["azul"]   ,MessageTextColor= (Color)Application.Current.Resources["blanco"],TintColor= (Color)Application.Current.Resources["blanco"] }))
            {
                llamada =  await repoapi.GetWorkOPrder(int.Parse(Viajenumero));
            }
         
            if (llamada.realizado)
            {
                ImagesList = new ObservableCollection<PgetWorkordersJibapp_Result>(llamada.Result);
                DriverName = llamada.Result[0].DriverName;
                Eventosexisten = true;
            }
            else {
                ImagesList = new ObservableCollection<PgetWorkordersJibapp_Result>();
                Eventosexisten = false;
              await  mensajetoast(llamada.Errores);
            }
            IsBusy = false;
        }

        public async Task<genericresult> enviardatosviaje()
        {
            /* List<Photo> fotografias = new List<Photo>();
             foreach (var foto in ImagesList)
             {
                 fotografias.Add(new Photo() { Foto = Convert.ToBase64String(foto.Foto), Name = foto.FotoNombre });
             }

             return await repoapi.checkin(new DataCarga { Noviaje = Viajenumero.Trim(), Fotografias = fotografias });
            */
            return new genericresult();
        }

        public void removerfoto(Fototemp foto)
        {
            //ImagesList.Remove(foto);
        }
        public async Task Seleccionarfotosgallery()
        {
            // FileResult photo = null; ;

            var photo = await MediaPicker.PickPhotoAsync();
            //var resultfile = await LoadPhotoAsync(photo);
            if (photo != null)
            {
                try
                {
                    //var FotoActual = DependencyService.Get<IMediaService>().getimagearray(photo.FullPath);
                    var FotoActual= File.ReadAllBytes(photo.FullPath);
                    setimagen(new Fototemp { FotoNombre = photo.FileName, Foto = FotoActual, Fecha = DateTime.Now.ToShortDateString() });
                }
                catch(Exception ex) 
                { 
                
                }
            }
        }
        public async Task tomarfotot()
        {


        }

        public void setimagen(Fototemp data)
        {
            //ImagesList.Add(data);
            Eventosexisten = true;
        }

        async Task<string> LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                return null;

            }
            // save the file into local storage
            var newFile = "";
            using (var stream = await photo.OpenReadAsync())
                newFile=DependencyService.Get<IMediaService>().SaveImageFromByte(ReadFully(stream), photo.FileName);

            return newFile;
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();

            }

        }

        public async Task mensajetoast(string Error)
        {
            var color = new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration();
            color.BackgroundColor = (Color)Application.Current.Resources["azul"];
            await MaterialDialog.Instance.SnackbarAsync(message: Error,
            msDuration: 5000, color);
        }
    }
        }
