using IrrigationController.Model;
using IrrigationController.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace IrrigationController
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var response = await App.ZonaService.GetAllZonaAsync();
            switch(response.Status)
            {
                case Status.SUCCESS:
                    {
                        lstData.ItemsSource = response.Data;
                        break;
                    }
                case Status.OTHER_ERROR:
                    {
                        lstData.ItemsSource = response.Data;
                        await DisplayAlert("Error", response.StatusString, "Ok");
                        break;
                    }
            }
        }

        public async void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            var vSelUser = (Zona)e.SelectedItem;
            await Navigation.PushAsync(new ShowZona(vSelUser));
            lstData.SelectedItem = null;
        }
        public async void ToolbarItem_ZonaAddClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new AddZona());
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