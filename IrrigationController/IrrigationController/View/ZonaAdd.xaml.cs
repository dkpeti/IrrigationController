using IrrigationController.Model;
using IrrigationController.Service;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IrrigationController.ZonaEdit;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZonaAdd : BasePage
    {
        public Zona Zona { get; set; }
        public List<Pi> Pis { get; set; }
        public Pi SelPi { get; set; }
        public ObservableCollection<CheckedSensor> Szenzorok { get; set; }

        public ZonaAdd()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            try
            {
                IsBusy = true;
                base.OnAppearing();

                Pis = await GetPis();
                if (Pis == null) return;

                Szenzorok = new ObservableCollection<CheckedSensor>();

                BindingContext = this;
            }
            finally
            {
                IsBusy = false;
            }
            
        }

        // Menti a felhasználó által megadott zóna adatokat a szerverre
        public async void SaveClicked(object sender, EventArgs args)
        {
            if (String.IsNullOrEmpty(txtZonaNev.Text))                          // Ellenőrzi, hogy a név mező nem üres-e
            {
                await DisplayAlert("Hiba", "A név nem lehet üres!", "Ok");
                return;
            }
            else if(SelPi == null)                                              // Ellenőrzi, hogy pi legyen kiválasztva
            {
                await DisplayAlert("Hiba", "A pi nem lehet üres!", "Ok");
                return;
            }

            var vZona = new Zona()
            {
                Nev = txtZonaNev.Text,
                PiId = SelPi.Id,
                SzenzorLista = Szenzorok
                    .Where(szenzor => szenzor.Checked)
                    .Select(szenzor => szenzor.Szenzor.Id)
                    .ToArray()
            };
            try
            {
                IsBusy = true;
                var response = await App.ZonaService.CreateZonaItemAsync(vZona);

                // A válasz alapján megfelelő visszajelzés megjelenítése
                switch (response.Status)
                {
                    case Status.SUCCESS:
                        {
                            CrossToastPopUp.Current.ShowCustomToast($"{txtZonaNev.Text} zóna sikeresen hozzáadva", bgColor: "#636363", txtColor: "white", ToastLength.Short);       // Felugró értesítés a sikeres hozzáadásról
                            await Navigation.PopAsync();                                    // A PiAdd oldalról visszalép
                            await Navigation.PushAsync(new ZonaData(response.Data));        // Tovább lép az új ZonaData oldalra amely tartalmazza a mentett adatokat
                            break;
                        }
                    case Status.OTHER_ERROR:
                        {
                            await DisplayAlert("Hiba", response.StatusString, "Ok");        // Ha valamiért nem tudja menteni, hibaüzenet megjelenítése
                            break;
                        }
                }
            }
            finally
            {
                IsBusy = false;
            }
            
        }

        // Lekéri a szerverről pi-ket
        // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad visszajelzést
        private async Task<List<Pi>> GetPis()
        {
            var response = await App.PiService.GetAllPiAsync();

            // A válasz alapján megfelelő visszajelzés megjelenítése
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
            // Ha nem találtunk hibát, de nem sikerült adatot lekérni, akkor null-t adunk vissza
            return null;
        }

        // Lekéri a pi-hez tartozó szenzorokat
        // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad vissza értéket
        private async Task<List<Szenzor>> GetLehetsegesSzenzorok(int piId)              // A lekért szenzorokat tartalmazó lista, vagy null, ha valamilyen hiba történt
        {
            var response = await App.SzenzorService.GetAllSzenzorByPiIdAsync(piId);     // Lekéri az összes pi-hez tartozó szenzort

            // A válasz alapján megfelelő visszajelzés megjelenítése
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
            // Ha nem találtunk hibát, de nem sikerült adatot lekérni, akkor null-t adunk vissza
            return null;
        }

        // (Melyik szenzorok vannak kijelölve)
        public class CheckedSensor
        {
            public Szenzor Szenzor { get; set; }
            public bool Checked { get; set; }
        }

        // Frissíti az Szenzorok listát a Pi kiválasztásakor
        private async void PiPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lehetsegesSzenzorok = await GetLehetsegesSzenzorok(SelPi.Id);
            if (lehetsegesSzenzorok == null) return;

            Szenzorok.Clear();
            foreach (var szenzor in lehetsegesSzenzorok)
            {
                Szenzorok.Add(new CheckedSensor
                {
                    Szenzor = szenzor,
                    Checked =false 
                });
            }
        }
    }
}