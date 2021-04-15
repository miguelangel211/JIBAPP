using System;
using System.Collections.Generic;
using System.Text;

namespace CargadosTrucking.Models
{
    public  class Driver
    {
        public int DriverID { get; set; }
        public string DriverName { get; set; }
        public string Address { get; set; }
        public string Cel { get; set; }
        public string Radio { get; set; }
        public string Notes { get; set; }
        public Nullable<int> SUC_ID { get; set; }
        public Nullable<decimal> PrefOrder { get; set; }
    }
}
