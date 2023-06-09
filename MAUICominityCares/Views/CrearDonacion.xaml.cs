
using MAUICominityCares.Models;
using Microsoft.Maui;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MAUICominityCares.Views;

public partial class CrearDonacion : ContentPage
{
	int aux;
    string isAnonymos;
    int selectedId;
    static HttpClient clientSQL = new HttpClient();
    private List<Collect> collects;
    public CrearDonacion(int id)
	{
		InitializeComponent();
        clientSQL.BaseAddress = new Uri("https://localhost:7036/%22");
        clientSQL.DefaultRequestHeaders.Accept.Clear();
        clientSQL.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        aux = id;
        LoadDataAsync();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        collects = await GetUserAsync();
        collectPicker.ItemsSource = collects;
    }

    public async Task<List<Collect>> GetUserAsync()
    {
        List<Collect> list = null;
        HttpResponseMessage response = await clientSQL.GetAsync("api/Collect");
        if (response.IsSuccessStatusCode)
        {
            var resultString = await response.Content.ReadAsStringAsync();
            list = JsonConvert.DeserializeObject<List<Collect>>(resultString);
        }
        return list;
    }
    private void AnonimoCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (AnonimoCheckbox.IsChecked)
        {
            isAnonymos = "Y";
        }
        else
        {
            isAnonymos = "N";
        }
        
    }

    private void ItemsCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (ItemsCheckbox.IsChecked)
        {
            Descripcion.Text = "Descripción de Artículo o articulos:";
            Monto.Text = "";
            MontoEntry.IsEnabled = false;
            DescriptionEntry.IsEnabled = true;
        }
        else
        {
            Monto.Text = "";
            Descripcion.Text = "";
            MontoEntry.IsEnabled = false;
            DescriptionEntry.IsEnabled = false;
        }
      

       

        

      
       
    }

    private void DineroCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (DineroCheckbox.IsChecked)
        {
            Monto.Text = "Ingrese monto";
            Descripcion.Text = "";
            MontoEntry.IsEnabled = true;
            DescriptionEntry.IsEnabled = false;
        }
        else
        {
            Monto.Text = "";
            Descripcion.Text = "";
            MontoEntry.IsEnabled = false;
            DescriptionEntry.IsEnabled = false;
        }

    }

    private void collectPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedCollect = collectPicker.SelectedItem as Collect;
        if (selectedCollect != null)
        {
            selectedId = selectedCollect.Id;
            DisplayAlert("Éxito", selectedId.ToString(), "Aceptar");
            // Aquí puedes acceder a las propiedades del elemento seleccionado (por ejemplo, selectedCollect.Id, selectedCollect.Date, etc.)
        }
    }

  

    private async void Registrar_Clicked(object sender, EventArgs e)
    {
        try
        {
            MAUICominityCares.Models.Donation don = new MAUICominityCares.Models.Donation
            {
                DescriptionItems = DescriptionEntry.Text,
                DescriptionMonto = decimal.Parse(MontoEntry.Text),
                Status = 1,
                RegisterDate = DateTime.Now,
                IsAnonymus = isAnonymos,
                IsReceived = "N",
                IdCollect = selectedId,
                Hour = Hora.Text,
                IdCampaign = aux,
                IdDonnors = SessionClass.Id


            };
            var postResultPersonSQL = await CreatePersonAsyncSQL(don);
            


            await DisplayAlert("Éxito", "¡Registro exitoso!", "Aceptar");

            await Navigation.PushModalAsync(new Asilos());
        }
        catch (Exception ex)
        {
            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }
    static async Task<Uri> CreatePersonAsyncSQL(MAUICominityCares.Models.Donation person)
    {
        HttpResponseMessage responde = await clientSQL.PostAsJsonAsync("api/Donation", person);
        responde.EnsureSuccessStatusCode();
        return responde.Headers.Location;
    }

    private async void btnVolver_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NewPage1());
    }
}