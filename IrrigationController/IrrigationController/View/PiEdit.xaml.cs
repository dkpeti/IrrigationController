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
            var response = await App.PiService.EditPiItemAsync(mSelPi);
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        CrossToastPopUp.Current.ShowCustomToast($"{txtPiNev.Text} sikeresen mentve", bgColor: "#636363", txtColor: "white", ToastLength.Short);
                        await Navigation.PopAsync();
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