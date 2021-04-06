using System;
using System.Collections.Generic;
using System.Text;

namespace CargadosTrucking.Helpers
{
    public interface IMediaService
    {
        string SaveImageFromByte(byte[] imageByte, string filename);
        byte[] getimagearray(string urloriginal);
    }
}
