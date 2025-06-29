﻿using System;
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
        public string lat { get; set; }
        public string @long { get; set; }
        public string CityZip { get; set; }
    }
    public class Parametrosimages
    {
        public List<Photo> Imagenes { get; set; }
        public int Tripid { get; set; }
        public int OrderID { get; set; }
        public string DriverName { get; set; }
        public bool Batery { get; set; }
        public bool Catalizer { get; set; }
        public bool Key { get; set; }
        public bool Radio { get; set; }
    }
}
