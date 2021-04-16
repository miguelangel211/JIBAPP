using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Xamarin.Forms;
using System.IO;

namespace CargadosTrucking.Helpers
{
    public class Toolscs
    {
        public ImageSource byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                Stream ms = new MemoryStream(byteArrayIn);
 ;
                return ImageSource.FromStream(() => new MemoryStream(byteArrayIn));//Exception occurs here
            }
            catch { }
            return null;
        }

    }
}
