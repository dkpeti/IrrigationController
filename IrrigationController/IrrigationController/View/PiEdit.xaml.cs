using IrrigationController.Model;
using IrrigationController.Service;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PiEdit : BasePage
    {
        readonly private Pi mSelPi;
        public PiEdit(Pi mSelPi)
        {
            InitializeComponent();
            this.mSelPi = mSelPi;
            BindingContext = mSelPi;
        }
        
        // Menti a felhasználó által szerkesztett Pi adatokat a szerverre
        public async void SaveClicked(object sender, EventArgs args)
        {
            if (String.IsNullOrEmpty(txtPiNev.Text))
            {
                await DisplayAlert("Hiba", "A név nem lehet üres!", "Ok");          // Ellenőrzi, hogy a név mező nem üres-e
                return;
            }
            else if (String.IsNullOrEmpty(txtPiAzonosito.Text))
            {
                await DisplayAlert("Hiba", "Az azonosító nem lehet üres!", "Ok");   // Ellenőrzi, hogy az azonosító mező nem üres-e
                return;
            }
            var response = await App.PiService.EditPiItemAsync(mSelPi);

            // A válasz alapján megfelelő visszajelzés megjelenítése
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        CrossToastPopUp.Current.ShowCustomToast($"{txtPiNev.Text} Pi sikeresen módosítva", bgColor: "#636363", txtColor: "white", ToastLength.Short);       // Felugró értesítés a sikeres módosításról
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
    }
}