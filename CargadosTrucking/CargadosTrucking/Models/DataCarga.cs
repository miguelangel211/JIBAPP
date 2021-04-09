using System;
using System.Collections.Generic;
using System.Text;

namespace CargadosTrucking.Models
{
    public class DataCarga
    {
        public List<Photo> Fotografias { get; set; }
        public string Noviaje { get; set; }
    }
    public class Photo
    {
        public string Name { get; set; }
        public string Foto { get; set; }
        public string Comentario { get; set; }
    }
    public class Parametrosimages
    {
        public List<Photo> Imagenes { get; set; }
        public int Tripid { get; set; }
        public int OrderID { get; set; }
        public string DriverName { get; set; }
    }
}
