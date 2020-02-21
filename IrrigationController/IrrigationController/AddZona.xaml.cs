using IrrigationController.Model;
using IrrigationController.Service;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddZona : ContentPage
    {
        public AddZona()
        {
            InitializeComponent();
        }
            
        public async void OnSaveClicked(object sender, EventArgs args)
        {
            var vZona = new Zona()
            {
                Nev = txtZonaNev.Text
            };

            var response = await App.ZonaService.CreateZonaItemAsync(vZona);
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        await Navigation.PopAsync();
                        await Navigation.PushAsync(new ShowZona(response.Data));
                        break;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Error", response.StatusString, "Ok");
                        break;
                    }
            }
        }
    }
}