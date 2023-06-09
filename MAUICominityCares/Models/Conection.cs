using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUICominityCares.Models
{
    public class Conection
    {
        static HttpClient clientSQL = new HttpClient();
        string coneccion = "https://localhost:7036/";
        public async Task<List<Campaign>> ObtenerInstitucionDesdeAPI()
        {
            try
            {
                HttpResponseMessage responseInstitucion = await clientSQL.GetAsync(coneccion+"api/User");
                responseInstitucion.EnsureSuccessStatusCode();

                string responseBodyInst = await responseInstitucion.Content.ReadAsStringAsync();

                List<Campaign> instituciones = JsonConvert.DeserializeObject<List<Campaign>>(responseBodyInst);

                return instituciones;
            }
            catch (Exception)
            {
                // Manejo de errores
                return null;
            }

        }

    }
}
