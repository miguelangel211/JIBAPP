using CargadosTrucking.Clases;
using CargadosTrucking.Helpers;
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
   public  class EditarFotoModel:BaseViewModel
    {
          
        public Fototemp fotolocal { get; set; }
        public byte[] image { get; set; }
        public byte[] CurrentImage { get { return image; } set { image = value; OnPropertyChanged(); } }


        public Command tomarfoto;
        public Command TomarFoto { get { return tomarfoto; } set { tomarfoto = value; OnPropertyChanged(); } }

        public string escrito;
        public string Comments { get { return escrito; } set { escrito = value;OnPropertyChanged(); } }
        public EditarFotoModel(Fototemp data)
        {
            fotolocal = data;

            CurrentImage = fotolocal.Foto;
            Comments = fotolocal.Comentario;
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
  

            if (photo != null)
            {
                try
                {

                    using (var stream = await photo.OpenReadAsync())
                        CurrentImage = DependencyService.Get<IMediaService>().phototakenoriginal(ReadFully(stream));

                    fotolocal.Foto = CurrentImage; fotolocal.FotoNombre = photo.FileName; fotolocal.Fecha = DateTime.Now.ToShortDateString();fotolocal.Comentario = Comments;

                }
                catch { }
            }

            IsBusy = false;

        }

        public void getcurrentimagestorage(string location)
        {
            fotolocal.Foto = File.ReadAllBytes(location);
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
