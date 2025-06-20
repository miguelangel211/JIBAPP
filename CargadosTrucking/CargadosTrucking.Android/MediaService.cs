﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CargadosTrucking.Droid;
using CargadosTrucking.Helpers;
using Java.IO;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(MediaService))]
namespace CargadosTrucking.Droid
{
    public class MediaService : IMediaService
    {
        Context CurrentContext => CrossCurrentActivity.Current.Activity;
        public string SaveImageFromByte(byte[] imageByte, string fileName)
        {
            string path = null;
            try
            {

                var source = bytesToBitmap(imageByte);

               string  url =  Android.Provider.MediaStore.Images.Media.InsertImage(MainActivity.Context.ContentResolver, source, fileName, "cargados pic");

                /*
                                ContentValues valuesvideos;
                                valuesvideos = new ContentValues();
                               valuesvideos.Put(MediaStore.MediaColumns.RelativePath, Android.OS.Environment.DirectoryDcim + "/" + "Cargados");
                                valuesvideos.Put(MediaStore.MediaColumns.Title, "cargadospic");
                                valuesvideos.Put(MediaStore.MediaColumns.DisplayName, fileName);
                                valuesvideos.Put(MediaStore.MediaColumns.MimeType, "image/jpg");

                                ContentResolver resolver = MainActivity.Context.ContentResolver; //this.CurrentContext.ContentResolver;
                                var collection = Android.Provider.MediaStore.Images.Media.ExternalContentUri;
                               Android.Net.Uri uriSavedVideo = resolver.Insert(collection, valuesvideos);

                                var url = uriSavedVideo;

                                try
                                {


                                        if (source != null)
                                        {
                                            var imageOut = resolver.OpenOutputStream(url);
                                            try
                                            {
                                                source.Compress(Bitmap.CompressFormat.Jpeg, 100, imageOut);
                                            }
                                            finally
                                            {
                                            imageOut.Close();
                                            }

                                        }
                                        else
                                        {
                                        resolver.Delete(url, null, null);
                                            url = null;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        if (url != null)
                                        {
                                            resolver.Delete(url, null, null);
                                            url = null;
                                        }
                                    }

                                    if (url != null)
                                    {
                                    // path = url.ToString();
                                        path= getRealPathFromUri(url);
                                    }

                                  //  return stringUrl;


                            */
                source = null;
                if (url != null)
                {
                    // path = url.ToString();
                    path = getRealPathFromUri(Android.Net.Uri.Parse(url));
                }

            }

            catch (Exception ex)
            {
                return null;

            }
            
            return path;
        }


        public  byte[] getimagearray(string urloriginal) {
            byte[] data = null;
            var url = Android.Net.Uri.Parse(urloriginal);
             var   inputStream = MainActivity.Context.ContentResolver.OpenInputStream(url);
                data = ReadFully(inputStream);
                if( inputStream != null ) inputStream.Close();
            return data;
        }
        public static String getRealPathFromUri(Android.Net.Uri contentUri)
        {
            var context = MainActivity.Context;
            Android.Database.ICursor cursor = null;
            try
            {
                String[] proj = { MediaStore.MediaColumns.Data };
                cursor = context.ContentResolver.Query(contentUri, proj, null, null, null);
                int column_index = cursor.GetColumnIndexOrThrow(MediaStore.MediaColumns.Data);
                cursor.MoveToFirst();
                return cursor.GetString(column_index);
            }
            finally
            {
                if (cursor != null)
                {
                    cursor.Close();
                }
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public void CopyFile(System.IO.Stream instream, System.IO.Stream outstream)
        {
            var buffer = new byte[1024];
            int read;
            while ((read = instream.Read(buffer)) != -1)
            {

                outstream.Write(buffer, 0, read);
            }
        }

        public static Bitmap bytesToBitmap(byte[] imageBytes)
        {

            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);

            return bitmap;
        }

        public byte[] reziseImage(byte[] Image)
        {
          
            var bit = bytesToBitmap(Image);
            if (bit.Height>bit.Width) {
                var matrix = new Matrix();
                matrix.PostRotate(-90);
                matrix.PreScale(1, 1);

                var bit2 =Bitmap.CreateBitmap(bit, 0, 0, bit.Width , bit.Height , matrix, true);
                using (var stream = new MemoryStream())
                {
                    bit2.Compress(Bitmap.CompressFormat.Jpeg, 30, stream);
                    return stream.ToArray();
                }
            }
            using (var stream = new MemoryStream())
            {
                bit.Compress(Bitmap.CompressFormat.Jpeg, 30, stream);
                return stream.ToArray();
            }

           
        }

        public byte[] phototakenoriginal(byte[] Image)
        {
                Bitmap image = bytesToBitmap(Image);
            if (image.Width > image.Height)
            {

                var matrix = new Matrix();
                var scaleWidth = image.Width;
                var scaleHeight = image.Height;
                matrix.PostRotate(90);
                matrix.PreScale(1, 1);
                var bit2 = Bitmap.CreateBitmap(image, 0, 0, image.Width, image.Height, matrix, true);
                using (var stream = new MemoryStream())
                {
                    bit2.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                    return stream.ToArray();
                }
            }
            else
            {
                using (var stream = new MemoryStream()) { 
                    
                    image.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                    return stream.ToArray();
                }

            }
            
            
        }
    }
}