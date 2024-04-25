using IrrigationController.Model;
using IrrigationController.Network;
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
    public partial class PiData : BasePage
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

        // Az oldal megjelenítésekor meghívódik
        protected async override void OnAppearing()
        {
            try
            {
                IsBusy = true;
                base.OnAppearing();

                pi = await GetPi(pi.Id);                            //Pi Id lekérése
                if (pi == null) return;

                zonak = await GetZonakByPi(pi.Id);                  // A Pi-hez tartozó zónák lekérése
                if (zonak == null) return;

                szenzorok = await GetSzenzorokByPiId(pi.Id);        // A Pi-hez tartozó szenzorok lekérése
                if (szenzorok == null) return;

                // A nézet összekapcsolása az adatokkal
                BindingContext = new
                {
                    Pi = pi,
                    Zonak = zonak,
                    ZonaTappedCommand = new Command<Zona>(ZonaTapped),              // Zónára kattintás
                    Szenzorok = szenzorok,
                    SzenzorTappedCommand = new Command<Szenzor>(SzenzorTapped)      // Szenzorra kattintás
                };
            }
            finally
            {
                IsBusy = false;
            }
           
        }

        // Pi adatai szerkesztése, PiEdit oldal megnyitása
        public async void EditClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new PiEdit(pi));
        }

        // A metódus az eseménykezelője a "Törlés" (kuka) ikon gomb lenyomásának
        // Megjelenít egy megerősítő üzenetet a felhasználónak a pi törléséről, és várja a választ
        public async void DeleteClicked(object sender, EventArgs args)
        {
            bool accepted = await DisplayAlert("Törlés",$"Biztosan törli a(z) {pi.Nev} pi-t?", "Igen", "Nem");
            if (accepted)
            {               
                var response = await App.PiService.DeleteTodoItemAsync(pi);
                
                // A válasz alapján megfelelő visszajelzés megjelenítése
                switch (response.Status)
                {
                    case Status.SUCCESS:
                        {
                            CrossToastPopUp.Current.ShowCustomToast($"{pi.Nev} sikeresen törölve", bgColor: "#636363", txtColor: "white", ToastLength.Short);       // Felugró értesítés a sikeres törlésről
                            await Navigation.PopAsync();        // A PiData oldalról visszalép
                            break;
                        }
                    case Status.OTHER_ERROR:
                        {
                            await DisplayAlert("Hiba", response.StatusString, "Ok");        // Ha valamiért nem tudja törölni, hibaüzenet megjelenítése
                            break;
                        }
                }
            }
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

        // Lekéri a Pi-hez tartozó zónákat a Pi Id alapján
        // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad visszajelzést
        private async Task<List<Zona>> GetZonakByPi(int piId)
        {
            var response = await App.ZonaService.GetAllZonaByPiIdAsync(piId);       // Lekéri a Pi-hez tartozó zónákat 

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

        // Lekéri a Pi-hez tartozó szenzorokat a Pi Id alapján
        // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad visszajelzést
        private async Task<List<Szenzor>> GetSzenzorokByPiId(int piId)          // Lekéri a Pi-hez tartozó szenzorokat 
        {
            var response = await App.SzenzorService.GetAllSzenzorByPiIdAsync(piId);

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

        // Ha rákattintok egy zónára átnavigál a zóna adataira
        private async void ZonaTapped(Zona zona)
        {
            await Navigation.PushAsync(new ZonaData(zona));
        }

        // Ha rákattintok egy szenzorra átnavigál a szenzor adataira
        private async void SzenzorTapped(Szenzor szenzor)
        {
            await Navigation.PushAsync(new SzenzorData(szenzor));
        }
    }
}