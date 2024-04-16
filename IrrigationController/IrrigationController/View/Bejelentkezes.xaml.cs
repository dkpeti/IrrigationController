using IrrigationController.Model;
using Newtonsoft.Json;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Bejelentkezes : BasePage
    {
        //Megnyitja a felhasználási feltételeket böngészőben (url a Bejelentkezes.xaml oldalon)
        public ICommand Feltetelek => new Command<string>(async (url) => await Launcher.OpenAsync(url));    
        
        public Bejelentkezes()
        {
            InitializeComponent();
            BindingContext = this;
            IsBusy = false;                                     // Az alkalmazás nem foglalt
            NavigationPage.SetHasNavigationBar(this, false);    // Toolbar elrejtése a bejelentkezési oldalon
        }

        // Bejelentkezés gombra kattintás eseménykezelője
        public void LoginClicked(object sender, EventArgs args)
        {
            IsBusy = true;      
            App.LoginService.LoginWithGoogle(OnLoginSucceeded, OnLoginFailed, OnLoginCanceled);     // Google bejelentkezés szolgáltatásának meghívása
        }

        // Sikeres bejelentkezés eseménykezelője
        private async void OnLoginSucceeded(User user)
        {
            Navigation.InsertPageBefore(new TabbedMainPage(), this);    // A bejelentkezési oldal után a TabbedMainPage jelenik meg a felhasználónak
            await Navigation.PopAsync();
        }

        // A bejelentkezés megszakítva eseménykezelője (ha a felhasználó kilép a bejelentkezési ablakból anélkül, hogy bejelentkezne)
        private void OnLoginCanceled()
        {
            IsBusy = false;
        }

        // Sikertelen bejelentkezés eseménykezelője
        // Hibaüzenet megjelenítése
        private async void OnLoginFailed(string reason)
        {
            IsBusy = false;
            await DisplayAlert("Hiba", reason, "Ok");
        }
    }

    // Hitelesítő szolgáltató
    public enum AuthProvider
    {
        Google
    }
}