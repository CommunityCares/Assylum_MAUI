
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using MAUICominityCares.Models;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MAUICominityCares.Views;

public partial class RegisterView : ContentPage
{
    static HttpClient client = new HttpClient();
    static HttpClient clientSQL = new HttpClient();
    string region;
    public RegisterView()
	{
		InitializeComponent();
        client.BaseAddress = new Uri("https://localhost:7004/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //People
        clientSQL.BaseAddress = new Uri("https://localhost:7036/");
        clientSQL.DefaultRequestHeaders.Accept.Clear();
        clientSQL.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    static async Task<Uri> CreatePersonAsyncSQL(MAUICominityCares.Models.Person person)
    {
        HttpResponseMessage responde = await clientSQL.PostAsJsonAsync("api/People", person);
        responde.EnsureSuccessStatusCode();
        return responde.Headers.Location;
    }
    static async Task<Uri> CreateDonorAsyncSQL(Donor donor)
    {
        HttpResponseMessage responde = await clientSQL.PostAsJsonAsync("api/Donor", donor);
        responde.EnsureSuccessStatusCode();
        return responde.Headers.Location;
    }
    static async Task<Uri> CreateUserAsyncSQL(User user)
    {
        HttpResponseMessage responde = await clientSQL.PostAsJsonAsync("api/User", user);
        responde.EnsureSuccessStatusCode();
        return responde.Headers.Location;
    }



    static async Task<Uri> CreatePersonAsync(MAUICominityCares.Models.Person person)
    {
        HttpResponseMessage responde = await client.PostAsJsonAsync("api/Person", person);
        responde.EnsureSuccessStatusCode();
        return responde.Headers.Location;
    }
    static async Task<Uri> CreateDonorAsync(Donor donor)
    {
        HttpResponseMessage responde = await client.PostAsJsonAsync("api/Donor", donor);
        responde.EnsureSuccessStatusCode();
        return responde.Headers.Location;
    }
    static async Task<Uri> CreateUserAsync(User user)
    {
        HttpResponseMessage responde = await client.PostAsJsonAsync("api/User", user);
        responde.EnsureSuccessStatusCode();
        return responde.Headers.Location;
    }
    static async Task<int> GetMaxProductIdAsync()
    {
        int maxId = 0;
        HttpResponseMessage response = await clientSQL.GetAsync("api/People");
        if (response.IsSuccessStatusCode)
        {
            var resultString = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<Person>>(resultString);
            foreach (var item in list)
            {
                if (item.Id > maxId)
                {
                    maxId = item.Id;
                }
            }
        }
        return maxId;
    }
    private async void OnRegistrarClicked(object sender, EventArgs e)
    {

       
        try
        {
             MAUICominityCares.Models.Person person = new MAUICominityCares.Models.Person
            {               
                Name = NombreEntry.Text,
                LastName = ApellidoEntry.Text,
                SecondLastName = SegundoApellidoEntry.Text,
                status=1,
                RegisterDate=DateTime.Now,
                Ci = NitEntry.Text,
                PhoneNumber = PhoneEntry.Text
            };
            var postResultPersonSQL = await CreatePersonAsyncSQL(person);
            int maxProductId = await GetMaxProductIdAsync();
            MAUICominityCares.Models.Person personMongo = new MAUICominityCares.Models.Person
            {
                Id= maxProductId,
                Name = NombreEntry.Text,
                LastName = ApellidoEntry.Text,
                SecondLastName = SegundoApellidoEntry.Text,
                status = 1,
                RegisterDate = DateTime.Now,
                Ci = NitEntry.Text,
                PhoneNumber = PhoneEntry.Text
            };
            Donor donor = new Donor
            {
                Id= maxProductId,
                Address=AdrressEntry.Text,
                Region = region
            };
            var postResultDonorSQL = await CreateDonorAsyncSQL(donor);
            User user = new User
            {
                Id = maxProductId,
                Password = ContraseñaEntry.Text,
                Role = "donor",
                Email = EmailEntry.Text
            };
            var postResultUserSQL = await CreateUserAsyncSQL(user);

            var postResultPerson = await CreatePersonAsync(personMongo);
            var postResultDonor = await CreateDonorAsync(donor);
            var postResultUser = await CreateUserAsync(user);


            await DisplayAlert("Éxito", "¡Registro exitoso!", "Aceptar");

            await Navigation.PushModalAsync(new MainPage());
        }
        catch (Exception ex)
        {
            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
           region = (string)picker.ItemsSource[selectedIndex];
        }
    }

    private async void BtnRegresar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MainPage());
    }
}