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

        // Az oldal megjelenítésekor meghívódik
        protected override async void OnAppearing()
        {
            
            try
            {
                IsBusy = true;
                base.OnAppearing();
                zona = await GetZona(zonaId);       //Zona Id kekérése
                if (zona == null) return;

                pi = await GetPi(zona.PiId);        //Pi Id lekérése
                if (pi == null) return;

                szenzorok = await GetSzenzorok(zonaId);     //Szenzor Id lekérése
                if (szenzorok == null) return;

                // A nézet összekapcsolása az adatokkal
                BindingContext = new
                {
                    Zona = zona,
                    Pi = pi,
                    Szenzorok = szenzorok,
                    SzenzorTappedCommand = new Command<Szenzor>(SzenzorokTapped)    // Szenzorra kattintás
                };
            }
            finally
            {
                IsBusy = false;
            }
            
        }

        // Zóna adatai szerkesztése, ZonaEdit oldal megnyitása
        public async void EditClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ZonaEdit(zona));
        }

        // A metódus az eseménykezelője a "Törlés" (kuka) ikon gomb lenyomásának
        // Megjelenít egy megerősítő üzenetet a felhasználónak a zóna törléséről, és várja a választ
        public async void DeleteClicked(object sender, EventArgs args)
        {
            bool accepted = await DisplayAlert("Törlés", $"Biztosan törli a(z) {zona.Nev} zónát?", "Igen", "Nem");
            if (accepted)
            {
                try
                {
                    IsBusy = true;
                    var response = await App.ZonaService.DeleteTodoItemAsync(zona);

                    // A válasz alapján megfelelő visszajelzés megjelenítése
                    switch (response.Status)
                    {
                        case Status.SUCCESS:
                            {   
                                CrossToastPopUp.Current.ShowCustomToast($"{zona.Nev} sikeresen törölve", bgColor: "#636363", txtColor: "white", ToastLength.Short);     // Felugró értesítés a sikeres törlésről
                                await Navigation.PopAsync();        // A ZonaData oldalról visszalép
                                break;
                            }
                        case Status.OTHER_ERROR:
                            {
                                await DisplayAlert("Hiba", response.StatusString, "Ok");        // Ha valamiért nem tudja törölni, hibaüzenet megjelenítése
                                break;
                            }
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        // Lekéri a Zónát-t az azonosító alapján
        // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad visszajelzést
        private async Task<Zona> GetZona(int zonaId)
        {
            var response = await App.ZonaService.GetOneZonaByIdAsync(zonaId);   // Lekéri az adott azonosítójú Zónát

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

        // Lekéri a Pi-t az azonosító alapján
        // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad visszajelzést
        private async Task<Pi> GetPi(int piId)
        {
            var response = await App.PiService.GetOnePiByIdAsync(piId);     // Lekéri az adott azonosítójú Pi-t 

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

        // Lekéri a szenzorokat
        // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad visszajelzést
        private async Task<List<Szenzor>> GetSzenzorok(int zonaId)
        {
            var response = await App.SzenzorService.GetAllSzenzorByZonaIdAsync(zonaId);

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

        // Ha rákattintok egy szenzorra átnavigál a szenzor adataira
        private async void SzenzorokTapped(Szenzor szenzor)
        {
            await Navigation.PushAsync(new SzenzorData(szenzor));
        }
    }
}