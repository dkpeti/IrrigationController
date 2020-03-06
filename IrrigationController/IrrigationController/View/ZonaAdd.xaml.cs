using IrrigationController.Model;
using IrrigationController.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZonaAdd : ContentPage
    {
        public Zona Zona { get; set; }
        public List<Pi> Pis { get; set; }
        public Pi SelPi { get; set; }

        public ZonaAdd()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Pis = await GetPis();
            if (Pis == null) return;

            BindingContext = this;

        }

        public async void SaveClicked(object sender, EventArgs args)
        {
            if (String.IsNullOrEmpty(txtZonaNev.Text))
            {
                await DisplayAlert("Error", "Név ne legyen üres!", "Ok");
                return;
            }
            else if(SelPi == null)
            {
                await DisplayAlert("Error", "Pi nem lehet üres!", "Ok");
                return;
            }

            var vZona = new Zona()
            {
                Nev = txtZonaNev.Text,
                PiId = SelPi.Id
            };
            
            var response = await App.ZonaService.CreateZonaItemAsync(vZona);
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        await Navigation.PopAsync();
                        await Navigation.PushAsync(new ZonaData(response.Data));
                        break;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Error", response.StatusString, "Ok");
                        break;
                    }
            }
        }
        private async Task<List<Pi>> GetPis()
        {
            var response = await App.PiService.GetAllPiAsync();
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        return response.Data;
                    }
                case Status.NOT_FOUND:
                    {
                        await DisplayAlert("Error", response.StatusString, "Ok");
                        await Navigation.PopAsync();
                        return null;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Error", response.StatusString, "Ok");
                        return null;
                    }
            }
            return null;
        }
    }
}