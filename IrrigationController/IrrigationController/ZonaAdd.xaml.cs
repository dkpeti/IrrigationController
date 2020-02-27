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
    public partial class ZonaAdd : ContentPage
    {
        public ZonaAdd()
        {
            InitializeComponent();
        }
            
        public async void SaveClicked(object sender, EventArgs args)
        {
            var vZona = new Zona()
            {
                Nev = txtZonaNev.Text,
                PiId = 2
            };

            var response = await App.ZonaService.CreateZonaItemAsync(vZona);
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        await Navigation.PopAsync();
                        await Navigation.PushAsync(new ZonaShow(response.Data));
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