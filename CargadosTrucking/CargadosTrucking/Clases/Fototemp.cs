using CargadosTrucking.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargadosTrucking.Clases
{
    public class Fototemp:BaseViewModel
    {
        public string FotoNombre { get; set; }
        private byte[] fot;
        public byte[] Foto { get { return fot; } set { fot = value;OnPropertyChanged(); } }
        public string Fecha { get; set; }
        private string comm;
        public string Comentario { get { return comm; } set { comm = value;OnPropertyChanged(); } }

        public string lat { get; set; }
        public string @long { get; set; }
        public string CityZip { get; set; }
    }
}
