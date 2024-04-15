using IrrigationController.Model;
using IrrigationController.Service;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZonaEdit : BasePage
    {
        public Zona Zona { get; set; }
        public List<Pi> Pis { get; set; }
        public Pi SelPi { get; set; }
        public ObservableCollection<CheckedSensor> Szenzorok { get; set; }


        public ZonaEdit(Zona mSelZona)
        {
            InitializeComponent();
            this.Zona = mSelZona;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                IsBusy = true;
                Pis = await GetPis();
                if (Pis == null) return;
                SelPi = Pis.Find(pi => pi.Id == Zona.PiId);

                var lehetsegesSzenzorok = await GetLehetsegesSzenzorok(Zona.PiId);
                var jelenlegiSzenzorok = await GetJelenlegiSzenzorok(Zona.Id);
                if (lehetsegesSzenzorok == null || jelenlegiSzenzorok == null) return;

                Szenzorok = new ObservableCollection<CheckedSensor>();
                foreach (var szenzor in lehetsegesSzenzorok)
                {
                    Szenzorok.Add(new CheckedSensor
                    {
                        Szenzor = szenzor,
                        Checked = jelenlegiSzenzorok.Find(s => s.Id == szenzor.Id) != null
                    }
                    );
                }
            }
            finally
            {
                IsBusy = false;
            }

            BindingContext = this;
        }

        public async void SaveClicked(object sender, EventArgs args)
        {
            if (String.IsNullOrEmpty(txtZonaNev.Text))
            {
                await DisplayAlert("Hiba", "A név nem lehet üres!", "Ok");
                return;
            }
            else if (SelPi == null)
            {
                await DisplayAlert("Hiba", "A Pi nem lehet üres!", "Ok");
                return;
            }

            Zona.PiId = SelPi.Id;
            Zona.SzenzorLista = Szenzorok
                .Where(szenzor => szenzor.Checked)
                .Select(szenzor => szenzor.Szenzor.Id)
                .ToArray();

            var response = await App.ZonaService.EditZonaItemAsync(Zona);
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        CrossToastPopUp.Current.ShowCustomToast($"{txtZonaNev.Text} zóna sikeresen módosítva", bgColor: "#636363", txtColor: "white", ToastLength.Short);
                        await Navigation.PopAsync();
                        break;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Hiba", response.StatusString, "Ok");
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
                        await DisplayAlert("Hiba", response.StatusString, "Ok");
                        await Navigation.PopAsync();
                        return null;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Hiba", response.StatusString, "Ok");
                        return null;
                    }
            }
            return null;
        }

        private async Task<List<Szenzor>> GetJelenlegiSzenzorok(int zonaId)
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
                        await DisplayAlert("Hiba", response.StatusString, "Ok");
                        await Navigation.PopAsync();
                        return null;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Hiba", response.StatusString, "Ok");
                        return null;
                    }
            }
            return null;
        }

        private async Task<List<Szenzor>> GetLehetsegesSzenzorok(int piId)
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
                        await DisplayAlert("Hiba", response.StatusString, "Ok");
                        await Navigation.PopAsync();
                        return null;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Hiba", response.StatusString, "Ok");
                        return null;
                    }
            }
            return null;
        }

        public class CheckedSensor
        {
            public Szenzor Szenzor { get; set; }
            public bool Checked { get; set; }
        }

        private async void PiPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lehetsegesSzenzorok = await GetLehetsegesSzenzorok(SelPi.Id);
            var jelenlegiSzenzorok = await GetJelenlegiSzenzorok(Zona.Id);
            if (lehetsegesSzenzorok == null || jelenlegiSzenzorok == null) return;

            Szenzorok.Clear();
            foreach (var szenzor in lehetsegesSzenzorok)
            {
                Szenzorok.Add(new CheckedSensor
                {
                    Szenzor = szenzor,
                    Checked = jelenlegiSzenzorok.Find(s => s.Id == szenzor.Id) != null
                }
                );
            }
        }
    }
}