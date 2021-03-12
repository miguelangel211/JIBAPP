using CargadosTrucking.Clases;
using CargadosTrucking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CargadosTrucking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CapturarFotografia : ContentPage
    {
        CapturarFotografiaModel context;
        public delegate void EventHandler(Fototemp status);
        public event EventHandler EventPass;
        public CapturarFotografia(Fototemp fotoriginal)
        {
            InitializeComponent();
            this.BindingContext = context = new CapturarFotografiaModel(fotoriginal);
        }

        private void cerrarforma(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void AceptarFoto(object sender, EventArgs e)
        {
            EventPass(context.fotolocal);
            Navigation.PopAsync();
        }
    }
}