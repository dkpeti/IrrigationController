using IrrigationController.Model;
using IrrigationController.Service;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditZona : ContentPage
    {
        readonly private Zona mSelZona;

        public EditZona(Zona mSelZona)
        {
            InitializeComponent();
            this.mSelZona = mSelZona;
            BindingContext = mSelZona;
        }

        public async void OnSaveClicked(object sender, EventArgs args)
        {
            var response = await App.ZonaService.EditZonaItemAsync(mSelZona);
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        await Navigation.PushAsync(new MainPage());
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