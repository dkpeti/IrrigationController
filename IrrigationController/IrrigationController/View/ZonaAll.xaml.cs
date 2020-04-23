using IrrigationController.Model;
using IrrigationController.Service;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IrrigationController
{
    public partial class ZonaAll : BasePage
    {
        public ZonaAll()
        {
            InitializeComponent();
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                IsBusy = true;
                await Reload();
            }
            finally
            {
                IsBusy = false;
            }
            
        }

        public async void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            var vSelUser = (Zona)e.SelectedItem;
            await Navigation.PushAsync(new ZonaData(vSelUser));
            ZonaList.SelectedItem = null;
        }

        public async void ZonaAddClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ZonaAdd());
        }

        private async Task Reload()
        {
            var zonak = await GetZonas();
            if (zonak == null) return;

            BindingContext = new
            {
                Zonak = zonak.Select(zona => {
                    bool ontozesBe =
                        zona.UtolsoOntozesKezdese == null ||
                        zona.UtolsoOntozesKezdese?.AddMinutes(zona.UtolsoOntozesHossza ?? 0) <= DateTime.UtcNow;

                    return new
                    {
                        Nev = zona.Nev,
                        OntozesBe = ontozesBe,
                        OntozesKi = !ontozesBe,
                        OntozesBeCommand = new Command<int>(async (hossz) =>
                        {
                            await OntozesIndit(zona, hossz);
                        }),
                        OntozesKiCommand = new Command(async () =>
                        {
                            await OntozesLeallit(zona);
                        })
                    };
                })
            };
        }

        private async Task<List<Zona>> GetZonas()
        {
            var response = await App.ZonaService.GetAllZonaAsync();
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        return response.Data;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Error", response.StatusString, "Ok");
                        return null;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        private async Task OntozesIndit(Zona zona, int hossz)
        {
            var ontozes = new Ontozes()
            {
                Utasitas = OntozesUtasitas.KEZDES,
                Hossz = hossz
            };
            try
            {
                IsBusy = true;
                var response = await App.ZonaService.ZonaOntozesAsync(zona, ontozes);
                switch (response.Status)
                {
                    case Status.SUCCESS:
                        {
                            CrossToastPopUp.Current.ShowCustomToast($"Öntözés elindítva {hossz} percre", bgColor: "#636363", txtColor: "white", ToastLength.Short);
                            await Reload();
                            break;
                        }
                    case Status.OTHER_ERROR:
                        {
                            await DisplayAlert("Error", response.StatusString, "Ok");
                            return;
                        }
                    default:
                        {
                            return;
                        }
                }
            }
            finally
            {
                IsBusy = false;
            }
            
        }

        private async Task OntozesLeallit(Zona zona)
        {
            var ontozes = new Ontozes()
            {
                Utasitas = OntozesUtasitas.VEGE
            };
            try
            {
                IsBusy = true;
                var response = await App.ZonaService.ZonaOntozesAsync(zona, ontozes);
                switch (response.Status)
                {
                    case Status.SUCCESS:
                        {
                            CrossToastPopUp.Current.ShowCustomToast($"Öntözés leállítva", bgColor: "#636363", txtColor: "white", ToastLength.Long);
                            await Reload();
                            break;
                        }
                    case Status.OTHER_ERROR:
                        {
                            await DisplayAlert("Error", response.StatusString, "Ok");
                            return;
                        }
                    default:
                        {
                            return;
                        }
                }
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}