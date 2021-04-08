using System;
using System.Collections.Generic;
using System.Text;

namespace CargadosTrucking.Models
{
    public class genericresult
    {
        public bool realizado { get; set; }
        public string Errores { get; set; }
    }

    public class genericdatar<T> : genericresult {
        public List<T> Result { get; set; }
    }
}
