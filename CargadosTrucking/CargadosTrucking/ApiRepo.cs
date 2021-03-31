using CargadosTrucking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CargadosTrucking
{
    public class ApiCalls
    {
        HttpClient client;
        UriBuilder builder;
        string urlerror = "";
        HttpResponseMessage response = null;
        string url;
        string url2;
        public ApiCalls()
        {
            client = new HttpClient();

           // url2 = "http://datserver.ddns.net:8089/ApiGeoloc/api/";
        }

        public async Task<genericresult> checkin(DataCarga parametro)
        {
            string Errores = null;

            try
            {
                url2 = await SecureStorage.GetAsync("rutaapi");
                string json = JsonConvert.SerializeObject(parametro);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PostAsync(string.Concat(url2, "api/FotoUpload/Guardardata"), content);
                string r = await response.Content.ReadAsStringAsync();
                var respuestageneric = JsonConvert.DeserializeObject<genericresult>(r);
                Errores = respuestageneric.Errores;
                if (respuestageneric.realizado)
                {

                    var reso = JsonConvert.DeserializeObject<genericresult>(r);

                    return await Task.FromResult(reso);
                }
                else
                {
                    respuestageneric.realizado = false;
                    return respuestageneric;
                    
                }

            }
            catch (Exception ex)
            {

                var r = new genericresult() { realizado = false};
                if (!string.IsNullOrEmpty(Errores))
                {
                  r.Errores   = Errores;
                }
                else
                {
                    r.Errores = ex.Message;
                }
                return await Task.FromResult(r);
            }
        }

        internal async Task<genericresult> Pruebadeconexion2(string uuurlapi)
        {
    
                string Errores = null;
                try
                {
                    builder = new UriBuilder(string.Concat(uuurlapi, "api/Values/"));
                    //builder.Port = -1;
                    string urlbuild = builder.ToString();
                    response = await client.GetAsync(urlbuild);
                    if (response.IsSuccessStatusCode)
                    {
                        return await Task.FromResult(new genericresult { realizado = true, Errores = "Conexion Realizada !!!!!" });
                    }
                    else
                    {
                        return await Task.FromResult(new genericresult { realizado = false, Errores = "No se pudo establecer una conexion" });

                    }

                }
                catch (Exception ex)
                {
                    var r = new genericresult() { realizado = false };
                    r.realizado = false;
                    if (!string.IsNullOrEmpty(Errores))
                    {
                        r.Errores = Errores;
                    }
                    else
                    {
                        r.Errores = "No se pudo establecer una conexion";
                    }
                    return await Task.FromResult(r);
                }
            
        }
    }
}
