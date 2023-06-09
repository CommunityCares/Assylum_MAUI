
using System.Net.Http;
using System.Net.Http.Headers;
using MAUICominityCares.Models;
using Newtonsoft.Json;
using MAUICominityCares.Views;

namespace MAUICominityCares;

public partial class MainPage : ContentPage
{
    static HttpClient client = new HttpClient();

    

	public MainPage()
	{
		InitializeComponent();
        client.BaseAddress = new Uri("https://localhost:7004/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
    static async Task<List<User>> GetUserAsync()
    {
        List<User> list = null;
        HttpResponseMessage response = await client.GetAsync("api/User");
        if (response.IsSuccessStatusCode)
        {
            var resultString = await response.Content.ReadAsStringAsync();
            list = JsonConvert.DeserializeObject<List<User>>(resultString);
        }
        return list;
    }
    static async Task<List<Donor>> GetDonor()
    {
        List<Donor> list = null;
        HttpResponseMessage response = await client.GetAsync("api/Donor");
        if (response.IsSuccessStatusCode)
        {
            var resultString = await response.Content.ReadAsStringAsync();
            list = JsonConvert.DeserializeObject<List<Donor>>(resultString);
        }
        return list;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var userList = await GetUserAsync();
            var donor = await GetDonor();

            if (userList == null || donor == null)
            {
                return false; // No se pudo obtener la lista de usuarios o la lista de donantes
            }

            var user = userList.FirstOrDefault(x => x.Email == email && x.Password == password);

            if (user == null)
            {
                return false; // Las credenciales no coinciden o el usuario no existe
            }

            var don = donor.FirstOrDefault(x => x.Id == user.Id);

            if (don == null)
            {
                return false; // No se encontró el donante correspondiente al usuario
            }

            SessionClass.Id = user.Id;
            SessionClass.Region = don.Region;

            return true; // Inicio de sesión exitoso
        }
        catch (Exception ex)
        {
            // Manejo de la excepción
            Console.WriteLine("Error al iniciar sesión: " + ex.Message);
            return false;
        }
    }


    private async void OnLoginButtonClicked(object sender, EventArgs e)
	{

        bool loggedIn = await LoginAsync(EmailEntry.Text, PasswordEntry.Text);
        if (loggedIn)
        {
            await Navigation.PushModalAsync(new NewPage1());
            await DisplayAlert("Éxito", "¡Inicio de sesión!", "Aceptar");
            Console.WriteLine("Inicio de sesión exitoso");
        }
        else
        {
            await DisplayAlert("Error", "¡Ingrese bien los datos!", "Aceptar");
        }

        

	}

    private async void btnRegistro_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new RegisterView());
    }
}

