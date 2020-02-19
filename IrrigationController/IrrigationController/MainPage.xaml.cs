using IrrigationController.Class;
using Realms;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace IrrigationController
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vRealmDb = Realm.GetInstance();
            var vAllZona = vRealmDb.All<Zona>();
            lstData.ItemsSource = vAllZona;
        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            var vSelUser = (Zona)e.SelectedItem;
            Navigation.PushAsync(new ShowZona(vSelUser));
        }
        public void ToolbarItem_ZonaAddClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new AddZona());
        }
        void InditasImageTapped(object sender, EventArgs args)
        {
            try
            {
                DisplayAlert("Öntözés elindítása", "Nincs kapcsolat a szerverrel", "Ok");
            }
            catch
            {
                DisplayAlert("Öntözés elindítása most nem lehetséges", "Nincs kapcsolat a szerverrel", "Ok");
            }
        }
    }
}