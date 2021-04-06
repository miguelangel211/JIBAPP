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

        private ObservableCollection<Fototemp> es { get; set; }
        public ObservableCollection<Fototemp> ImagesList { get { return es; } set { es = value; OnPropertyChanged(); } }


        public Command tomarfoto;
        public Command TomarFoto { get { return tomarfoto; } set { tomarfoto = value; OnPropertyChanged(); } }
        public Command selecfoto;
        public Command SeleccionarFoto { get { return selecfoto; } set { selecfoto = value; OnPropertyChanged(); } }
        INavigation localNavigation;

        public MainPageViewModel(INavigation nav)
        {
            TomarFoto = new Command(async () => await tomarfotot());
            SeleccionarFoto = new Command(async () => await Seleccionarfotosgallery());
            Viajenumero = "";
            ImagesList = new ObservableCollection<Fototemp>();
            Eventosexisten = false;
            localNavigation = nav;
        }

        public async Task<genericresult> enviardatosviaje()
        {
            List<Photo> fotografias = new List<Photo>();
            foreach (var foto in ImagesList)
            {
                fotografias.Add(new Photo() { Foto = Convert.ToBase64String(foto.Foto), Name = foto.FotoNombre });
            }

            return await repoapi.checkin(new DataCarga { Noviaje = Viajenumero.Trim(), Fotografias = fotografias });

        }

        public void removerfoto(Fototemp foto)
        {
            ImagesList.Remove(foto);
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
            var resultfile = await LoadPhotoAsync(photo);
            Console.WriteLine($"CapturePhotoAsync COMPLETED: {resultfile}");

            if (photo != null)
            {
                try
                {
                    byte[] FotoActual = null;
                    //var FotoActual = File.ReadAllBytes(resultfile);
                    if (resultfile != null)
                    {
                        using (var stream = await photo.OpenReadAsync())
                            FotoActual = ReadFully(stream);
                    }
                    
                    var newpage = new CapturarFotografia(new Fototemp { FotoNombre = photo.FileName, Foto = FotoActual, Fecha = DateTime.Now.ToShortDateString() });
                    newpage.EventPass += setimagen;
                    await localNavigation.PushAsync(newpage);
                    // ImagesList.Add(new Fototemp { FotoNombre = photo.FileName, Foto = FotoActual, Fecha = DateTime.Now.ToShortDateString() }); ;
                    //Eventosexisten = true;
                }
                catch(Exception ex) 
                {


                }
            }
            IsBusy = false;

        }

        public void setimagen(Fototemp data)
        {
            ImagesList.Add(data);
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
    }
        }
