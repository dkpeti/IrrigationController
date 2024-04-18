using IrrigationController.Model;
using IrrigationController.Service;
using Plugin.Toast;
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
    public partial class SzenzorEdit : BasePage
    {
        public Szenzor Szenzor { get; set; }
        public SzenzorEdit(Szenzor mSelSzenzor)
        {
            InitializeComponent();
            this.Szenzor = mSelSzenzor;
            BindingContext = this.Szenzor;
        }

        // Menti a felhasználó által szerkesztett szenzor adatokat a szerverre
        private async void SaveClicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSzenzorNev.Text))
            {
                await DisplayAlert("Hiba", "A név nem lehet üres!", "Ok");          // Ellenőrzi, hogy a név mező nem üres-e
                return;
            }
            else if (String.IsNullOrEmpty(txtSzenzorMegjegyzes.Text))
            {
                await DisplayAlert("Hiba", "A megjegyzés nem lehet üres!", "Ok");   // Ellenőrzi, hogy az azonosító mező nem üres-e
                return;
            }
            var response = await App.SzenzorService.EditSzenzorItemAsync(Szenzor);

            // A válasz alapján megfelelő visszajelzés megjelenítése
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {                       
                        await Navigation.PopAsync();
                        break;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Hiba", response.StatusString, "Ok");
                        break;
                    }
            }
        }
    }
}