using IrrigationController.Model;
using IrrigationController.Service;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Toast;
using Plugin.Toast.Abstractions;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PiAdd : BasePage
    {
        public PiAdd()
        {
            InitializeComponent();
        }

        // Menti a felhasználó által megadott Raspberry Pi adatokat a szerverre
        public async void SaveClicked(object sender, EventArgs args)
        {
            if (String.IsNullOrEmpty(txtPiNev.Text))                                // Ellenőrzi, hogy a név mező nem üres-e
            {
                await DisplayAlert("Hiba", "A név nem lehet üres!", "Ok");          
                return;
            }
            else if (String.IsNullOrEmpty(txtPiAzonosito.Text))                     // Ellenőrzi, hogy az azonosító mező nem üres-e
            {
                await DisplayAlert("Hiba", "Az azonosító nem lehet üres!", "Ok");   
                return;
            }

            // Létrehoz egy új Pi objektumot a felhasználó által megadott adatokkal
            var vPi = new Pi()
            {
                Nev = txtPiNev.Text,
                Azonosito = txtPiAzonosito.Text,
            };

            try
            {
                IsBusy = true;
                var response = await App.PiService.CreatePiItemAsync(vPi);       // Menteni az új Pi-t a szerverre
                
                // A válasz alapján megfelelő visszajelzés megjelenítése
                switch (response.Status)    
                {
                    case Status.SUCCESS:
                        {
                            CrossToastPopUp.Current.ShowCustomToast($"{txtPiNev.Text} Pi sikeresen hozzáadva", bgColor: "#636363", txtColor: "white", ToastLength.Short);       // Felugró értesítés
                            await Navigation.PopAsync();                                // A PiAdd oldalról visszalép
                            await Navigation.PushAsync(new PiData(response.Data));      // Tovább lép az új PiData oldalra amely tartalmazza a mentett adatokat
                            break;
                        }
                    case Status.OTHER_ERROR:
                        {
                            await DisplayAlert("Hiba", response.StatusString, "Ok");
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
}