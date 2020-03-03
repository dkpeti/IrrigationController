using IrrigationController.Model;
using IrrigationController.Service;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZonaData : ContentPage
    {
        private readonly int zonaId;
        private Zona zona;

        public ZonaData()
        {
            InitializeComponent();
        }
        public ZonaData(Zona aSelZona)
        {
            InitializeComponent();
            zonaId = aSelZona.Id;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var response = await App.ZonaService.GetOneZonaByIdAsync(zonaId);
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        zona = response.Data;
                        BindingContext = response.Data;
                        break;
                    }
                case Status.NOT_FOUND:
                    {
                        await DisplayAlert("Error", response.StatusString, "Ok");
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

        public async void EditClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ZonaEdit(zona));
        }
        public async void DeleteClicked(object sender, EventArgs args)
        {
            bool accepted = await DisplayAlert("Törlés", $"Biztos törli a(z) {zona.Nev} zónát?", "Igen", "Nem");
            if (accepted)
            {
                var response = await App.ZonaService.DeleteTodoItemAsync(zona);
                switch (response.Status)
                {
                    case Status.SUCCESS:
                        {
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
}