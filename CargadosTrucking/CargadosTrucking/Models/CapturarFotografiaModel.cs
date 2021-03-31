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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CargadosTrucking.Models
{
    public class CapturarFotografiaModel:BaseViewModel
    {
        public Fototemp fotolocal { get; set; }
        public byte[] image { get; set; }
        public byte[] CurrentImage { get { return image; } set { image = value;OnPropertyChanged(); } }


        public Command tomarfoto;
        public Command TomarFoto { get { return tomarfoto; } set { tomarfoto = value; OnPropertyChanged(); } }

        public CapturarFotografiaModel(Fototemp data) {
            fotolocal = data;
            CurrentImage = fotolocal.Foto;
            TomarFoto = new Command(async () => await tomarfotot());
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
                    var FotoActual = File.ReadAllBytes(resultfile);

                    // ImagesList.Add(new Fototemp { FotoNombre = photo.FileName, Foto = FotoActual, Fecha = DateTime.Now.ToShortDateString() }); ;
                    //Eventosexisten = true;
                    CurrentImage = FotoActual;
                    fotolocal.Foto = FotoActual;fotolocal.FotoNombre = photo.FileName;fotolocal.Fecha = DateTime.Now.ToShortDateString();

                }
                catch { }
            }

            IsBusy = false;

        }

        public void getcurrentimagestorage(string location) {
            fotolocal.Foto = File.ReadAllBytes(location);
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
