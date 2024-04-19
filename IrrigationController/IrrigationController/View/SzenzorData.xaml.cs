using IrrigationController.Model;
using IrrigationController.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SzenzorData : BasePage
    {
        private readonly int szenzorId;  // Az aktuális szenzor azonosítója
        private Szenzor szenzor;         // Az aktuális szenzor objektum
        private List<Meres> meresek;     // A szenzorhoz tartozó mérések listája

        public SzenzorData()
        {
            InitializeComponent();
        }

        //beállítja az aktuális szenzor azonosítóját annak az objektumnak az azonosítójára, amit paraméterként kapott
        public SzenzorData(Szenzor aSelSzenzor)
        {
            InitializeComponent();
            szenzorId = aSelSzenzor.Id;
        }

        // A Szenzor adatai oldal tartalmának betöltése és frissítése, amikor az oldal megjelenik.
        // Lekéri a szenzort és a hozzá kapcsolódó méréseket
        protected override async void OnAppearing()
        {
            try
            {
                IsBusy = true;          // Az alkalmazás jelez, hogy elfoglalt, míg az adatok betöltődnek
                base.OnAppearing();     // Az ősosztály "OnAppearing" metódusát hívjuk meg
                
                szenzor = await GetSzenzor(szenzorId);      // A "szenzor" objektum lekérése az azonosító alapján
                if (szenzor == null) return;                // Ha a szenzor nem található, a metódus végrehajtása befejeződik

                meresek = await GetMeresek(szenzor);        // A szenzorhoz tartozó mérések lekérése
                if (meresek == null) return;                // Ha nem találhatóak mérések, a metódus végrehajtása befejeződik

                //létrehoz egy anonim típusú objektumot,
                //amely tartalmazza a szenzor, SzenzorTipus és meresek adatokat.
                //Ezek az adatok felhasználói felületen jelennek meg
                BindingContext = new
                {
                    Szenzor = szenzor,
                    SzenzorTipus = SzenzorTipus(szenzor),
                    Meresek = meresek.Select(meres => new
                    {
                        Mikor = meres.Mikor.ToString("yyyy-MM-dd hh:mm"),
                        MertAdat = MertAdat(szenzor, meres)
                    })
                };
            }
            finally
            {
                IsBusy = false;    // Az alkalmazás jelez, hogy már nem foglalt
            }
        }

        // Az "EditClicked" metódus az eseménykezelője a "Szerkesztés" (ceruza) ikon gomb lenyomásának
        // Átnavigál az "SzenzorEdit" oldalra, és átadja a szerkeszteni kívánt szenzort
        private async void EditClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SzenzorEdit(szenzor));
        }

        // A "DeleteClicked" metódus az eseménykezelője a "Törlés" (kuka) ikon gomb lenyomásának
        // Megjelenít egy megerősítő üzenetet a felhasználónak a szenzor törléséről, és várja a választ
        private async void DeleteClicked(object sender, EventArgs e)
        {
            bool accepted = await DisplayAlert("Törlés", $"Biztosan törli a(z) {szenzor.Nev} szenzort?", "Igen", "Nem");                 
            if (accepted)   // Szenzor törlése
            {
                try
                {
                    // az alkalmazás elfoglalt a törlési művelet végrehajtása közben
                    // megakadályozza, hogy a felhasználó újabb törlési műveletet kezdjen el indítani a közben zajló művelet végéig
                    IsBusy = true;  

                    var response = await App.SzenzorService.DeleteTodoItemAsync(szenzor);                                             
                    switch (response.Status)        // A kapott válasz státuszát megvizsgáljuk
                    {                        
                        case Status.SUCCESS:        // Ha a törlés sikeres volt, visszaléptetjük a felhasználót az előző oldalra
                            {
                                await Navigation.PopAsync();
                                break;
                            }                       
                        case Status.OTHER_ERROR:    // Ha bármilyen más hiba történt, megjelenítjük a hibaüzenetet
                            {
                                await DisplayAlert("Hiba", response.StatusString, "Ok");
                                break;
                            }
                    }
                }
                // visszaáll az eredeti állapotba, akkor is ha hiba történik a törlési művelet közben
                finally
                {
                    IsBusy = false;
                }
            }
        }

        // Lekéri a szenzort az azonosító alapján
        // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad visszajelzést
        private async Task<Szenzor> GetSzenzor(int szenzorId)
        {
            var response = await App.SzenzorService.GetOneSzenzorByIdAsync(szenzorId);  // Lekéri az adott azonosítójú szenzort    
            
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

        // Aszinkron módon lekéri a méréseket az adott szenzorhoz
        // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad vissza értéket
        private async Task<List<Meres>> GetMeresek(Szenzor szenzor)     //A lekért méréseket tartalmazó lista, vagy null, ha valamilyen hiba történt
        {
            var response = await App.MeresService.GetAllMeresBySzenzorIdAsync(szenzor.Id); // aszinkron módon kéri le az összes mérést a megadott szenzor azonosítója alapján
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
            // Ha nem találtunk hibát, de nem sikerült adatot lekérni, akkor is null-t adunk vissza.
            return null;
        }

        // megvizsgálja hogy a szenzor milyen típus, ennek megfelelően ad vissza értéket
        // (ha a szenzor típusa Homerseklet, akkor Hőmérsékletet ad vissza 
        private string SzenzorTipus(Szenzor szenzor)
        {
            switch (szenzor.Tipus)
            {
                case Model.SzenzorTipus.Homerseklet:
                    return "Hőmérséklet";
                case Model.SzenzorTipus.Talajnedvesseg:
                    return "Talajnedvesség";
                default:
                    return "";
            }
        }

        // megvizsgálja hogy a szenzor milyen típus, ennek megfelelően ad vissza értéket
        // (ha a szenzor típusa Talajnedvesseg szenzor, akkor a mért adatot %-ban adja vissza)
        private string MertAdat(Szenzor szenzor, Meres meres)
        {
            switch (szenzor.Tipus)
            {
                case Model.SzenzorTipus.Homerseklet:
                    return $"{meres.MertAdat} °C";
                case Model.SzenzorTipus.Talajnedvesseg:
                    return $"{meres.MertAdat} %";
                default:
                    return "";
            }
        }
    }
}