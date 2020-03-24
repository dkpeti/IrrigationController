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
    public partial class SzenzorEdit : ContentPage
    {
        public Szenzor Szenzor { get; set; }

        readonly private Szenzor mSelSzenzor;
        public SzenzorEdit(Szenzor mSelSzenzor)
        {
            InitializeComponent();
            this.mSelSzenzor = mSelSzenzor;
            BindingContext = mSelSzenzor;
        }

        private async void SaveClicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSzenzorNev.Text))
            {
                await DisplayAlert("Error", "Név ne legyen üres!", "Ok");
                return;
            }
            else if (String.IsNullOrEmpty(txtSzenzorMegjegyzes.Text))
            {
                await DisplayAlert("Error", "Megjegyzes nem lehet üres!", "Ok");
                return;
            }
            var response = await App.SzenzorService.EditSzenzorItemAsync(Szenzor);
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