using System.Net.Http.Headers;
using MAUICominityCares.Models;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.Collections.Generic;
using System.Net.Http;
using System.ComponentModel;

namespace MAUICominityCares.Views;

public partial class DonacionesCerrada : ContentPage
{
    List<Campaign> campaigns { get; set; }
    Conection conn;
    static HttpClient clientSQL = new HttpClient();
    string cone = "https://localhost:7036/";
    int aux;
    
	public DonacionesCerrada(int id)
	{
		aux = id;
		InitializeComponent();
        clientSQL.BaseAddress = new Uri("https://localhost:7036/");
        clientSQL.DefaultRequestHeaders.Accept.Clear();
        clientSQL.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        CargarDatosDesdeAPI();
        
    }

    private async Task CargarDatosDesdeAPI()
    {
        List<Donation> items = await ObtenerDatosDesdeAPI();
        List<Donor> items2 = await ObtenerDonorDesdeAPI();
        List<Person> items3 = await ObtenerPersonDesdeAPI();
        var campañas = items.Join(
     items2,
     cam => cam.IdDonnors,
     asil => asil.Id,
     (cam, asil) => new { donacion = cam, donador = asil })
     .Join(
         items3,
         camp => camp.donador.Id,
         user => user.Id,
         (camp, user) => new { donacion = camp.donacion, donador = camp.donador, usuario = user })
     .Where(x => x.donacion.IdCampaign == aux)
     .Select(x =>
     {
         if (x.donacion.IsAnonymus == "N")
         {
             x.donador.Address = x.usuario.Name;
             x.donador.Region = x.usuario.LastName;
         }
         else if (x.donacion.IsAnonymus == "Y")
         {
             x.donador.Address = "donador anónimo";
             x.donador.Region = "donador anónimo";
         }

         return x;
     })
     .ToList();


        itemCollectionView.ItemsSource = campañas;

        


    }
    async Task<List<Donation>> ObtenerDatosDesdeAPI()
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

    async Task<List<Person>> ObtenerPersonDesdeAPI()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(cone + "api/People");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(json);
                return items;
            }
            else
            {
                // Manejo de errores en caso de respuesta no exitosa
                // ...
                return new List<Person>();
            }
        }
    }


    private string _nombre;
    public string Name
    {
        get { return _nombre; }
        set
        {
            _nombre = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private string _apellido;
    public string LastName
    {
        get { return _apellido; }
        set
        {
            _apellido = value;
            OnPropertyChanged(nameof(LastName));
        }
    }

    private string _items;
    public string DescriptionItems
    {
        get { return _items; }
        set
        {
            _items = value;
            OnPropertyChanged(nameof(DescriptionItems));
        }
    }

    private string _monto;
    public string DescriptionMonto
    {
        get { return _monto; }
        set
        {
            _monto = value;
            OnPropertyChanged(nameof(DescriptionMonto));
        }
    }

    private string _anonimo;
    public string IsAnonymus
    {
        get { return _anonimo; }
        set
        {
            _anonimo = value;
            OnPropertyChanged(nameof(IsAnonymus));
           
        }
    }

    // Implementa la interfaz INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async void btnVolver_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NewPage1());
    }
}