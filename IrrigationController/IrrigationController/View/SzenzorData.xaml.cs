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
        private List<Meres> meresek;

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

            meresek = await GetMeresek(szenzor);
            if (meresek == null) return;

            BindingContext = new
            {
                Szenzor = szenzor,
                SzenzorTipus = SzenzorTipus(szenzor),
                Meresek = meresek.Select(meres => new
                {
                    Mikor = meres.Mikor.ToString("yyyy-MM-dd hh:mm"),
                    MertAdat = MertAdat(szenzor, meres)
                })
            };
        }

        //szerkesztésre átnavigál
        private async void EditClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SzenzorEdit(szenzor));
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

        private async Task<List<Meres>> GetMeresek(Szenzor szenzor)
        {
            var response = await App.MeresService.GetAllMeresBySzenzorIdAsync(szenzor.Id);
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

        private string SzenzorTipus(Szenzor szenzor)
        {
            switch (szenzor.Tipus)
            {
                case Model.SzenzorTipus.Homerseklet:
                    return "Hőmérséklet";
                case Model.SzenzorTipus.Talajnedvesseg:
                    return "Talajnedvesség";
                default:
                    return "";
            }
        }
        private string MertAdat(Szenzor szenzor, Meres meres)
        {
            switch (szenzor.Tipus)
            {
                case Model.SzenzorTipus.Homerseklet:
                    return $"{meres.MertAdat} °C";
                case Model.SzenzorTipus.Talajnedvesseg:
                    return $"{meres.MertAdat} %";
                default:
                    return "";
            }
        }
    }
}