using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using Color = Xamarin.Forms.Color;

namespace CargadosTrucking.Helpers
{
    public static class toasts
    {
        public static async Task mensajetoast(string Error)
        {
            var color = new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration();
            color.BackgroundColor = (Color)Application.Current.Resources["azul"];
            await MaterialDialog.Instance.SnackbarAsync(message: Error,
            msDuration: 5000, color);
        }

        
    }
}
