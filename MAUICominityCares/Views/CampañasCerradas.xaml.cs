using System.Net.Http.Headers;
using MAUICominityCares.Models;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Net.Http.Headers;

using Microsoft.Maui.Controls;

using Microsoft.Maui.Controls.Xaml;
using System.Collections.Generic;

using System.Net.Http;

namespace MAUICominityCares.Views;

public partial class CampañasCerradas : ContentPage
{
    List<Campaign> campaigns { get; set; }
    Conection conn;
    static HttpClient clientSQL = new HttpClient();
    string cone = "https://localhost:7036/";
    public CampañasCerradas()
	{
		InitializeComponent();
        conn = new Conection();
        clientSQL.BaseAddress = new Uri("https://localhost:7036/");
        clientSQL.DefaultRequestHeaders.Accept.Clear();
        clientSQL.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        CargarDatosDesdeAPI();
    }
    private async Task CargarDatosDesdeAPI()
    {
        List<Campaign> items = await ObtenerDatosDesdeAPI();
        List<Assylum> asilos = await ObtenerAsilosDesdeAPI();
       
        var campañas = items.Join(
            asilos,
           cam => cam.IdAssylum,
           asil => asil.Id,
           (cam, asil) => new { campaigns = cam, asilos = asil }).Where(x => x.asilos.Region == SessionClass.Region).Where(x => x.campaigns.Status == 2);
        itemCollectionView.ItemsSource = campañas;
    }
    async Task<List<Campaign>> ObtenerDatosDesdeAPI()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(cone + "api/Campaign");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Campaign>>(json);
                return items;
            }
            else
            {
                // Manejo de errores en caso de respuesta no exitosa
                // ...
                return new List<Campaign>();
            }
        }
    }
    async Task<List<Assylum>> ObtenerAsilosDesdeAPI()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(cone + "api/Assylum");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Assylum>>(json);
                return items;
            }
            else
            {
                // Manejo de errores en caso de respuesta no exitosa
                // ...
                return new List<Assylum>();
            }
        }
    }
    private void btnCrearDonar_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        int datoId = (int)button.CommandParameter;

        // Navegar a la página ViewItems y pasar el ID de donación como parámetro
        Navigation.PushModalAsync(new DonacionesCerrada(datoId));

        
    }

    private async void btnVolver_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NewPage1());
    }
}