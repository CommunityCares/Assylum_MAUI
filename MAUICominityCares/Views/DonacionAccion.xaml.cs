using MAUICominityCares.Properties;
using System.Net.Http.Headers;

namespace MAUICominityCares.Views;

public partial class DonacionAccion : ContentPage
{
    List<Donation> campaigns { get; set; }
    
    static HttpClient clientSQL = new HttpClient();
    string cone = "https://localhost:7036/";
    int aux;
    public DonacionAccion(int id)
	{
		InitializeComponent();
		aux = id;
        clientSQL.BaseAddress = new Uri("https://localhost:7036/");
        clientSQL.DefaultRequestHeaders.Accept.Clear();
        clientSQL.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        CargarDatosDesdeAPI();
    }

    async void CargarDatosDesdeAPI()
    {
        List<Donation> items = await ObtenerDatosDesdeAPI();
        itemCollectionView.ItemsSource = items;
    }
    async Task<List<Donation>> ObtenerDatosDesdeAPI()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(cone + "api/Campaign");
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
}