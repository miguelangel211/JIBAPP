using System;
using System.Collections.Generic;
using System.Text;

namespace CargadosTrucking.Models
{
    public class ConfirmacionModel:BaseViewModel
    {
        private string name;
        public string NombreViaje { get { return name; } set { name = value;OnPropertyChanged(); } }
        public ConfirmacionModel(string viaje) {
            NombreViaje = "Viaje " + viaje;
        }
    }
}
