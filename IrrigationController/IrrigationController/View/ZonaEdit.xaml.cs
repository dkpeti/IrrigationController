using IrrigationController.Model;
using IrrigationController.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZonaEdit : ContentPage
    {
        public Zona Zona { get; set; }
        public List<Pi> Pis { get; set; }
        public Pi SelPi { get; set; }

        public ZonaEdit(Zona mSelZona)
        {
            InitializeComponent();
            this.Zona = mSelZona;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Pis = await GetPis();
            if (Pis == null) return;

            SelPi = Pis.Find(pi => pi.Id == Zona.PiId);
            BindingContext = this;
        }

        public async void SaveClicked(object sender, EventArgs args)
        {
            if (String.IsNullOrEmpty(txtZonaNev.Text))
            {
                await DisplayAlert("Error", "Név ne legyen üres!", "Ok");
                return;
            }
            else if (SelPi == null)
            {
                await DisplayAlert("Error", "Pi nem lehet üres!", "Ok");
                return;
            }

            Zona.PiId = SelPi.Id;
            var response = await App.ZonaService.EditZonaItemAsync(Zona);
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        await Navigation.PopAsync();
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