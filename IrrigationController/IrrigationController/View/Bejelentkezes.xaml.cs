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
        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
        public Bejelentkezes()
        {
            InitializeComponent();
            BindingContext = this;
            IsBusy = false;
            //a toolbar elrejtése
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public void LoginClicked(object sender, EventArgs args)
        {
            IsBusy = true;
            App.LoginService.LoginWithGoogle(OnLoginSucceeded, OnLoginFailed, OnLoginCanceled);
        }

        private async void OnLoginSucceeded(User user)
        {
            Navigation.InsertPageBefore(new TabbedMainPage(), this);
            await Navigation.PopAsync();
        }

        private void OnLoginCanceled()
        {
            IsBusy = false;
        }

        private async void OnLoginFailed(string reason)
        {
            IsBusy = false;
            await DisplayAlert("Error", reason, "Ok");
        }

        //private async void Felhasznalo_Clicked(object sender, EventArgs e)
        //{
        //    //await Navigation.PushAsync(new Profil());
        //}
    }

    public enum AuthProvider
    {
        Google
    }
}