using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CargadosTrucking.Droid;
using CargadosTrucking.Helpers;
using Plugin.CurrentActivity;
[assembly: Xamarin.Forms.Dependency(typeof(MediaService))]
namespace CargadosTrucking.Droid
{
    public class MediaService : IMediaService
    {
        Context CurrentContext => CrossCurrentActivity.Current.Activity;
        public string SaveImageFromByte(byte[] imageByte, string fileName)
        {
            string path = "";
            try
            {

               string storagePath= Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
                path = System.IO.Path.Combine(storagePath, fileName);
               // Java.IO.File storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
               //  path = System.IO.Path.Combine(storagePath.ToString(), fileName);
                System.IO.File.WriteAllBytes(path, imageByte);
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
                CurrentContext.SendBroadcast(mediaScanIntent);

            }
            catch (Exception ex)
            {

            }
            return path;
        }
    }
}