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
        HttpResponseMessage response = null;
        string url2;
        public ApiCalls()
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(4);
        }

        public async Task<genericdatasingle<string>> checkin(Parametrosimages parametro)
        {
            string Errores = null;

            try
            {
                url2 = await SecureStorage.GetAsync("rutaapiJIB");
                string json = JsonConvert.SerializeObject(parametro);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PostAsync(string.Concat(url2, "api/WorkOrders/SaveImageneswororder"), content);
                string r = await response.Content.ReadAsStringAsync();
                var respuestageneric = JsonConvert.DeserializeObject<genericdatasingle<string>>(r);
                Errores = respuestageneric.Errores;
                if (respuestageneric.realizado)
                {

                    var reso = JsonConvert.DeserializeObject<genericdatasingle<string>>(r);

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

                var r = new genericdatasingle<string>() { realizado = false};
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
        
        internal async Task<genericdatar<PgetWorkordersJibapp_Result>>  GetWorkOPrder(string tripid)
        {
    
                string Errores = null;
                try
                {
                url2 = await SecureStorage.GetAsync("rutaapiJIB");

                builder = new UriBuilder(string.Concat(url2, "api/WorkOrders/GetWorkOrders?tripid=" + tripid));
                    //builder.Port = -1;
                    string urlbuild = builder.ToString();
                    response = await client.GetAsync(urlbuild);
                    if (response.IsSuccessStatusCode)
                {
                    string r = await response.Content.ReadAsStringAsync();

                    return await Task.FromResult(JsonConvert.DeserializeObject<genericdatar<PgetWorkordersJibapp_Result>>(r));
                    }
                    else
                    {
                        return await Task.FromResult(new genericdatar<PgetWorkordersJibapp_Result> { realizado = false, Errores = "Couldn't connect to api" });

                    }

                }
                catch (Exception ex)
                {
                    var r =  new genericdatar<PgetWorkordersJibapp_Result> { realizado = false };
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
        internal async Task<genericdatasingle<Driver>> CheckCellphone(string phone)
        {
    
                string Errores = null;
                try
                {
                url2 = await SecureStorage.GetAsync("rutaapiJIB");

                builder = new UriBuilder(string.Concat(url2, "api/WorkOrders/CheckCellphone?Number=" + phone));
                    string urlbuild = builder.ToString();
                    response = await client.GetAsync(urlbuild);
                    if (response.IsSuccessStatusCode)
                {
                    string r = await response.Content.ReadAsStringAsync();

                    return await Task.FromResult(JsonConvert.DeserializeObject<genericdatasingle<Driver>>(r));
                    }
                    else
                    {
                        return await Task.FromResult(new genericdatasingle<Driver> { realizado = false, Errores = "Couldn't connect to api" });

                    }

                }
                catch (Exception ex)
                {
                    var r =  new genericdatasingle<Driver> { realizado = false };
                    r.realizado = false;
                    if (!string.IsNullOrEmpty(Errores))
                    {
                        r.Errores = ex.Message;
                    }
                    else
                    {
                        r.Errores = "Couldn't connect to api";
                    }
                    return await Task.FromResult(r);
                }
            
        }        
        internal async Task<genericdatasingle<GPSdata>> Getlocation(string latitude, string longitude)
        {
    
                string Errores = null;
                try
                {
                url2 = await SecureStorage.GetAsync("rutaapiJIB");

                builder = new UriBuilder("https://api.bigdatacloud.net/data/reverse-geocode-client?latitude="+latitude+"&longitude="+longitude+"&localityLanguage=en");
                    string urlbuild = builder.ToString();
                    response = await client.GetAsync(urlbuild);
                    if (response.IsSuccessStatusCode)
                {
                    string r = await response.Content.ReadAsStringAsync();
                    var datagps = JsonConvert.DeserializeObject<GPSdata>(r);
                    return await Task.FromResult(new genericdatasingle<GPSdata> {Result=datagps, realizado = true, Errores = "Couldn't connect to api" });
                }
                else
                    {
                        return await Task.FromResult(new genericdatasingle<GPSdata> { realizado = false, Errores = "Couldn't connect to api" });

                    }

                }
                catch (Exception ex)
                {
                    var r =  new genericdatasingle<GPSdata> { realizado = false };
                    r.realizado = false;
                    if (!string.IsNullOrEmpty(Errores))
                    {
                        r.Errores = ex.Message;
                    }
                    else
                    {
                        r.Errores = "Couldn't connect to api";
                    }
                    return await Task.FromResult(r);
                }
            
        }
    }
}
