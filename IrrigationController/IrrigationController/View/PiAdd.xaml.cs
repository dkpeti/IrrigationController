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
    public partial class PiAdd : ContentPage
    {
        public PiAdd()
        {
            InitializeComponent();
        }
        public async void SaveClicked(object sender, EventArgs args)
        {
            if (String.IsNullOrEmpty(txtPiNev.Text))
            {
                await DisplayAlert("Error", "Név ne legyen üres!", "Ok");
                return;
            }
            else if (String.IsNullOrEmpty(txtPiAzonosito.Text))
            {
                await DisplayAlert("Error", "Azonosító nem lehet üres!", "Ok");
                return;
            }

            var vPi = new Pi()
            {
                Nev = txtPiNev.Text,
                Azonosito = txtPiAzonosito.Text,
            };

            var response = await App.PiService.CreatePiItemAsync(vPi);
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        CrossToastPopUp.Current.ShowCustomToast($"{txtPiNev.Text} sikeresen hozzáadva", bgColor: "#636363", txtColor: "white", ToastLength.Short);
                        await Navigation.PopAsync();
                        await Navigation.PushAsync(new PiData(response.Data));
                        break;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Error", response.StatusString, "Ok");
                        break;
                    }
            }
        }
    }
}