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
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            var response = await App.PiService.GetAllPiAsync();
            IsBusy = false;
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
            var vSelUser = (Pi)e.SelectedItem;
            await Navigation.PushAsync(new PiData(vSelUser));
            PiList.SelectedItem = null;
        }
        private async void PiAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PiAdd());
        }
    }
}