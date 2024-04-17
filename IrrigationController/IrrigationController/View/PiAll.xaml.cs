using IrrigationController.Model;
using IrrigationController.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace IrrigationController
{

    public partial class PiAll : BasePage
    {
        public PiAll()
        {
            InitializeComponent();
        }

        // Lekéri a Raspberry Pi-k adatait a szerverről
        // Sikeres lekérés esetén az adatokat megjeleníti a felhasználónak a PiList nézetben
        // Hiba esetén hibaüzenetet dob
        protected override async void OnAppearing()
        {
            try
            {
                IsBusy = true;
                base.OnAppearing();    
                var response = await App.PiService.GetAllPiAsync();
                switch (response.Status)
                {
                    case Status.SUCCESS:
                        {
                            PiList.ItemsSource = response.Data;
                            break;
                        }
                    case Status.OTHER_ERROR:
                        {
                            PiList.ItemsSource = response.Data;
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

        // Ha a felhasználó kiválaszt egy elemet a PiList nézetből,
        // Továbbnavigál a PiData oldalra a kiválasztott Pi objektum részleteivel
        public async void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }   
            var vSelUser = (Pi)e.SelectedItem;                      // Kiválasztott Pi objektum lekérése
            await Navigation.PushAsync(new PiData(vSelUser));       // Az új PiData oldalra navigálás a kiválasztott Pi objektum részleteivel
            PiList.SelectedItem = null;                             // A kiválasztás megszüntetése a PiList nézetben
        }

        // PiAdd oldal megynitása, ami lehetővé teszi a felhasználó számára, hogy új Pi-t adjon hozzá.
        private async void PiAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PiAdd());
        }
    }
}