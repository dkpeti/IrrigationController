using IrrigationController.Model;
using IrrigationController.Service;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZonaData : BasePage
    {
        private readonly int zonaId;
        private Zona zona;
        private Pi pi;
        private List<Szenzor> szenzorok;

        public ZonaData()
        {
            InitializeComponent();
        }
        public ZonaData(Zona aSelZona)
        {
            InitializeComponent();
            zonaId = aSelZona.Id;
        }

        protected override async void OnAppearing()
        {
            
            try
            {
                IsBusy = true;
                base.OnAppearing();
                zona = await GetZona(zonaId);
                if (zona == null) return;

                pi = await GetPi(zona.PiId);
                if (pi == null) return;

                szenzorok = await GetSzenzorok(zonaId);
                if (szenzorok == null) return;

                BindingContext = new
                {
                    Zona = zona,
                    Pi = pi,
                    Szenzorok = szenzorok,
                    SzenzorTappedCommand = new Command<Szenzor>(SzenzorokTapped)
                };
            }
            finally
            {
                IsBusy = false;
            }
            
        }

        //szerkesztés átnavigál
        public async void EditClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ZonaEdit(zona));
        }
        public async void DeleteClicked(object sender, EventArgs args)
        {
            bool accepted = await DisplayAlert("Törlés", $"Biztos törli a(z) {zona.Nev} zónát?", "Igen", "Nem");
            if (accepted)
            {
                var response = await App.ZonaService.DeleteTodoItemAsync(zona);
                switch (response.Status)
                {
                    case Status.SUCCESS:
                        {
                            CrossToastPopUp.Current.ShowCustomToast($"{zona.Nev} sikeresen törölve", bgColor: "#636363", txtColor: "white", ToastLength.Short);
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
        }

        private async Task<Zona> GetZona(int zonaId)
        {
            var response = await App.ZonaService.GetOneZonaByIdAsync(zonaId);
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

        private async Task<Pi> GetPi(int piId)
        {
            var response = await App.PiService.GetOnePiByIdAsync(piId);
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

        private async Task<List<Szenzor>> GetSzenzorok(int zonaId)
        {
            var response = await App.SzenzorService.GetAllSzenzorByZonaIdAsync(zonaId);
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

        private async void SzenzorokTapped(Szenzor szenzor)
        {
            await Navigation.PushAsync(new SzenzorData(szenzor));
        }
    }
}