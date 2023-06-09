using MAUICominityCares.Models;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Net.Http.Headers;

using Microsoft.Maui.Controls;

using Microsoft.Maui.Controls.Xaml;
using System.Collections.Generic;

using System.Net.Http;

namespace MAUICominityCares.Views;

public partial class Asilos : ContentPage
{
     List<Campaign> campaigns { get; set; }
    Conection conn;
     static HttpClient clientSQL = new HttpClient();
    string cone = "https://localhost:7036/";
    public Asilos()
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
        List<Donation> donac = await ObtenerDonacionDesdeAPI();
        List<Donor> donadore = await ObtenerDonorDesdeAPI();
        var campa�as =  items.Join(
            asilos , 
           cam =>cam.IdAssylum , 
           asil => asil.Id, 
           (cam, asil) => new {campaigns = cam , asilos = asil} ).Where(x=>x.asilos.Region == SessionClass.Region).Where(x=>x.campaigns.Status ==1 );
        itemCollectionView.ItemsSource = campa�as;
    }
    async Task<List<Campaign>> ObtenerDatosDesdeAPI()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(cone+"api/Campaign");
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

    async Task<List<Donor>> ObtenerDonorDesdeAPI()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(cone + "api/Donor");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Donor>>(json);
                return items;
            }
            else
            {
                // Manejo de errores en caso de respuesta no exitosa
                // ...
                return new List<Donor>();
            }
        }
    }
    async Task<List<Donation>> ObtenerDonacionDesdeAPI()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(cone + "api/Donation");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Donation>>(json);
                return items;
            }
            else
            {
                // Manejo de errores en caso de respuesta no exitosa
                // ...
                return new List<Donation>();
            }
        }
    }

    private void btnDonar_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        int datoId = (int)button.CommandParameter;

        // Navegar a la p�gina ViewItems y pasar el ID de donaci�n como par�metro
        Navigation.PushModalAsync(new DonacionAccion(datoId));
    }

    private  void btnCrearDonar_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        int datoId = (int)button.CommandParameter;

        // Navegar a la otra vista y pasar el ID como par�metro
        Navigation.PushModalAsync(new CrearDonacion(datoId));
    }

    private async void btnVolver_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NewPage1());
    }
}
