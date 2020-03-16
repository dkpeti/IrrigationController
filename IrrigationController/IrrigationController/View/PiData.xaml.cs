using IrrigationController.Model;
using IrrigationController.Network;
using IrrigationController.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PiData : ContentPage
    {
        private readonly int piId;
        private Pi pi;
        private List<Zona> zonak;
        private List<Szenzor> szenzorok;

        public PiData()
        {
            InitializeComponent();
            BindingContext = new Pi();
        }
        public PiData(Pi aSelPi)
        {
            InitializeComponent();
            pi = aSelPi;
            piId = aSelPi.Id;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            pi = await GetPi(pi.Id);
            if (pi == null) return;

            zonak = await GetZonakByPi(pi.Id);
            if (zonak == null) return;

            szenzorok = await GetSzenzorokByPiId(pi.Id);
            if (szenzorok == null) return;

            BindingContext = new
            {
                Pi = pi,
                Zonak = zonak,
                ZonaTappedCommand = new Command<Zona>(ZonaTapped),
                Szenzorok = szenzorok,
                SzenzorTappedCommand = new Command<Szenzor>(SzenzorTapped)
            };
        }
        public async void EditClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new PiEdit(pi));
        }
        public async void DeleteClicked(object sender, EventArgs args)
        {
            bool accepted = await DisplayAlert("Törlés",$"Biztos törli a(z) {pi.Nev} pit?", "Igen", "Nem");
            if (accepted)
            {
                var response = await App.PiService.DeleteTodoItemAsync(pi);
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

        private async Task<List<Zona>> GetZonakByPi(int piId)
        {
            var response = await App.ZonaService.GetAllZonaByPiIdAsync(piId);
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

        private async Task<List<Szenzor>> GetSzenzorokByPiId(int piId)
        {
            var response = await App.SzenzorService.GetAllSzenzorByPiIdAsync(piId);
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

        //listában a ha rákattintok valamelyik zónára
        private async void ZonaTapped(Zona zona)
        {
            await Navigation.PushAsync(new ZonaData(zona));
        }

        //listában a ha rákattintok valamelyik szenzorra

        private async void SzenzorTapped(Szenzor szenzor)
        {
            await Navigation.PushAsync(new SzenzorData(szenzor));
        }
    }
}