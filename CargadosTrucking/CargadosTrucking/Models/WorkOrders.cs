using System;
using System.Collections.Generic;
using System.Text;

namespace CargadosTrucking.Models
{
    public  class PgetWorkordersJibapp_Result:BaseViewModel
    {
        public int TripID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string DriverName { get; set; }
        private int? fot;
        public Nullable<int> fotos { get { return fot; } set { fot = value;OnPropertyChanged(); } }

        public int OrderID { get; set; }
        public string WO_Make { get; set; }
        public string WO_Model { get; set; }
        public string WO_Year { get; set; }
        public string WO_VIN { get; set; }
    }
}
