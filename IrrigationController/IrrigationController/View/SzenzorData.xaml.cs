using IrrigationController.Model;
using IrrigationController.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SzenzorData : ContentPage
    {
        private readonly int szenzorId;
        private Szenzor szenzor;

        //PiDatánál
        //await Navigation.PushAsync(new SzenzorData(vSelSzenzor));
        //Szenzor vSelSzenzor nem kell
        public SzenzorData()
        {
            InitializeComponent();
        }
        public SzenzorData(Szenzor aSelSzenzor)
        {
            InitializeComponent();
            szenzorId = aSelSzenzor.Id;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            szenzor = await GetSzenzor(szenzorId);
            if (szenzor == null) return;
            BindingContext = new
            {
                Szenzor = szenzor
            };
        }

        //szerkesztésre átnavigál
        private async void EditClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SzenzorEdit());
        }

        //törlés
        private async void DeleteClicked(object sender, EventArgs e)
        {
            bool accepted = await DisplayAlert("Törlés", $"Biztos törli a(z) szenzort?", "Igen", "Nem");
            if (accepted)
            {
                var response = await App.SzenzorService.DeleteTodoItemAsync(szenzor);
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

        //Lekéri a szenzorokat
        //hibakezelés
        private async Task<Szenzor> GetSzenzor(int szenzorId)
        {
            var response = await App.SzenzorService.GetOneSzenzorByIdAsync(szenzorId);
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        return response.Data;
                    }
                case Status.NOT_FOUND:
                    {
                        await DisplayAlert("Error", response.StatusString, "Ok");
                        await Navigation.PopAsync();
                        return null;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Error", response.StatusString, "Ok");
                        return null;
                    }
            }
            return null;
        }
    }
}