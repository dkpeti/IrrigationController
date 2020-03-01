using IrrigationController.Model;
using IrrigationController.Service;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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