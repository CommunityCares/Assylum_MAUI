namespace MAUICominityCares.Views;

public partial class NewPage1 : ContentPage
{
	public NewPage1()
	{
		InitializeComponent();
	}



    private async void btnAsilos_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Asilos());
    }

 

    private async void BtnCerradas_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new CampañasCerradas());
    }

    private async void BtnSalir_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MainPage());
    }
}