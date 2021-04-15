using System;
using System.Collections.Generic;
using System.Text;

namespace CargadosTrucking.Models
{
    public class GPSdata
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string lookupSource { get; set; }
        public string countryName { get; set; }
        public string countryCode { get; set; }
        public string continentCode { get; set; }
        public string principalSubdivision { get; set; }
        public string principalSubdivisionCode { get; set; }
        public string city { get; set; }
        public string locality { get; set; }
        public string postcode { get; set; }

    }
}
