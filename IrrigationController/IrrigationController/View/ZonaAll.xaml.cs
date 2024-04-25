using IrrigationController.Model;
using IrrigationController.Service;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace IrrigationController
{
    public partial class ZonaAll : BasePage
    {
        public ZonaAll()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                IsBusy = true;
                await Reload();
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // Ha a felhasználó kiválaszt egy elemet a Zónák nézetből,
        // Továbbnavigál a ZonaData oldalra a kiválasztott Zóna részleteivel
        public async void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {   
                return;
            }
            var vSelUser = (Zona)e.SelectedItem.GetType().GetProperty("Zona").GetValue(e.SelectedItem, null);       // Kiválasztott Zóna objektum lekérése
            await Navigation.PushAsync(new ZonaData(vSelUser));                                                     // Az új ZonaData oldalra navigálás a kiválasztott Zóna objektum részleteivel
            ZonaList.SelectedItem = null;                                                                           // A kiválasztás megszüntetése a ZonaList nézetben (biztosítja, hogy a felhasználó ne tudja ismételten kiválasztani ugyanazt az elemet)
        }

        // ZonaAdd oldal megynitása, ami lehetővé teszi a felhasználó számára, hogy új Zónát adjon hozzá.
        public async void ZonaAddClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ZonaAdd());
        }

        // Újratölti az ZonaAll oldalt
        private async Task Reload()
        {
            var zonak = await GetZonas();
            if (zonak == null) return;

            // A nézet összekapcsolását az új adatokkal
            BindingContext = new
            {
                Zonak = zonak.Select(zona => {
                    // Meghatározza, hogy az öntözés be van-e kapcsolva
                    bool ontozesBe =
                        zona.UtolsoOntozesKezdese == null ||
                        zona.UtolsoOntozesKezdese?.AddMinutes(zona.UtolsoOntozesHossza ?? 0) <= DateTime.UtcNow;

                    // Kiszámolja hogy az öntözésből még mennyi idő van hátra
                    var ontozesHatra = (DateTime.UtcNow - (zona.UtolsoOntozesKezdese ?? DateTime.UtcNow)).TotalMinutes;
                    
                    // Kiszámolja az öntözési folyamat aktuális előrehaladását
                    double ontozesProgress = (double)ontozesHatra / (zona.UtolsoOntozesHossza ?? 1);

                    // Létrehoz egy objektumot, amely tárolja egy adott zóna öntözési állapotát,
                    // az öntözés előrehaladását és lehetővé teszi az öntözés indítását és leállítását
                    return new
                    {
                        Zona = zona,
                        OntozesBe = ontozesBe,
                        OntozesKi = !ontozesBe,
                        Hossz = 0,
                        OntozesProgress = ontozesProgress,
                        OntozesBeCommand = new Command<int>(async (hossz) =>
                        {
                            await OntozesIndit(zona, hossz);
                        }),
                        OntozesKiCommand = new Command(async () =>
                        {
                            await OntozesLeallit(zona);
                        })
                    };
                })
            };
        }

        // Lekéri a Zónákat, egy listát ad vissza a Zona objektumokkal
        // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad visszajelzést
        private async Task<List<Zona>> GetZonas()
        {
            var response = await App.ZonaService.GetAllZonaAsync();

            // A válasz alapján megfelelő visszajelzés megjelenítése
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        return response.Data;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Hiba", response.StatusString, "Ok");
                        return null;
                    }
                default:
                    {
                        return null;        // Ha nem találtunk hibát, de nem sikerült adatot lekérni, akkor null-t adunk vissza
                    }
            }
        }

        // Elindítja az öntözést a megadott zónában a megadott időtartamra
        private async Task OntozesIndit(Zona zona, int hossz)
        {
            // Létrehoz egy új Ontozes objektumot, amely az öntözés indításához szükséges adatokat tartalmazza
            var ontozes = new Ontozes()
            {
                Utasitas = OntozesUtasitas.KEZDES,      // Az öntözés utasítása "KEZDES" típusú
                Hossz = hossz                           // Az öntözés hossza a paraméterként kapott "hossz" érték
            };
            try
            {
                IsBusy = true;
                var response = await App.ZonaService.ZonaOntozesAsync(zona, ontozes);

                // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad vissza értéket
                switch (response.Status)
                {
                    case Status.SUCCESS:
                        {
                            CrossToastPopUp.Current.ShowCustomToast($"{zona.Nev} öntözése elindítva {hossz} percre", bgColor: "#636363", txtColor: "white", ToastLength.Long);      // Felugró értesítés az öntözés elindításáról
                            await Reload();     // Újratölti az ZonaAll oldalt (megjelenjen a ProgressBar)
                            break;
                        }
                    case Status.OTHER_ERROR:
                        {
                            await DisplayAlert("Hiba", response.StatusString, "Ok");
                            return;
                        }
                    default:
                        {
                            return;
                        }
                }
            }
            finally
            {
                IsBusy = false;
            }
            
        }

        // Leállítja az öntözést a megadott zónában
        private async Task OntozesLeallit(Zona zona)
        {
            var ontozes = new Ontozes()
            {
                Utasitas = OntozesUtasitas.VEGE     // Az öntözés utasítása "VEGE" típusú
            };
            try
            {
                IsBusy = true;
                var response = await App.ZonaService.ZonaOntozesAsync(zona, ontozes);

                // A kapott válasz státuszát megvizsgálja és ennek megfelelően ad vissza értéket
                switch (response.Status)
                {
                    case Status.SUCCESS:
                        {
                            CrossToastPopUp.Current.ShowCustomToast($"{zona.Nev} öntözése leállítva", bgColor: "#636363", txtColor: "white", ToastLength.Long);     // Felugró értesítés az öntözés leállításáról
                            await Reload();     // Újratölti az ZonaAll oldalt (eltűnjön a ProgressBar)
                            break;
                        }
                    case Status.OTHER_ERROR:
                        {
                            await DisplayAlert("Hiba", response.StatusString, "Ok");
                            return;
                        }
                    default:
                        {
                            return;
                        }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}