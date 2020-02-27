using IrrigationController.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Fiok : ContentPage
    {

        OAuth2Authenticator _authenticator;

        public Fiok()
        {
            InitializeComponent();
        }
        public void LoginClicked(object sender, EventArgs args)
        {
            _authenticator = new OAuth2Authenticator(
               clientId: "128088518939-g0ht0124tg75nj1j661hqb21nccg4l70.apps.googleusercontent.com",
               clientSecret: "",
               scope: "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile",
               authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
               redirectUrl: new Uri("com.googleusercontent.apps.128088518939-g0ht0124tg75nj1j661hqb21nccg4l70:/oauth2redirect"),
               accessTokenUrl: new Uri("https://www.googleapis.com/oauth2/v4/token"),
               isUsingNativeUI: true
            );

            _authenticator.Completed += Authenticator_Completed;
            _authenticator.Error += _authenticator_Error;
          
            AuthenticationState.Authenticator = _authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(_authenticator);
        }

        private async void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            _authenticator.Completed -= Authenticator_Completed;
            if (e.IsAuthenticated)
            {
                var acc = e.Account;
                var usr = await GetGoogleUser(acc.Properties["id_token"], e.Account);
                await SignInToAPI(usr, AuthProvider.Google);
            }
        }

        private void _authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            _authenticator.Error -= _authenticator_Error;
            Debug.WriteLine(e.Message);
        }

        private async Task<User> GetGoogleUser(string idToken, Account account)
        {
            //Get user's profile info from Google
            var request = new OAuth2Request("GET", new Uri("https://www.googleapis.com/oauth2/v2/userinfo"), null, account);
            var response = await request.GetResponseAsync();
            User user = null;

            if (response != null)
            {
                string userJson = await response.GetResponseTextAsync();
                user = JsonConvert.DeserializeObject<User>(userJson);
                user.TokenId = idToken;
                return user;
            }
            throw new Exception("Could not get user");
        }

        private async Task SignInToAPI(User user, AuthProvider authProvider)
        {
            //Used Ngrok to tunnel My endpoint.
            var endPoint = "https://192.168.1.106:45455/api/felhasznalo";
            if (authProvider == AuthProvider.Google)
                endPoint = $"{endPoint}/google";
            else
                endPoint = $"{endPoint}/Account/Facebook";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            var httpClient = new HttpClient(clientHandler);
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync(endPoint, content);

            //We get the JWT Token which we will be used to make authorized request to the API.
            var jwt = await res.Content.ReadAsStringAsync();
            Debug.WriteLine(jwt);
            await DisplayAlert("Login", jwt, "OK");
        }
    }

    public enum AuthProvider
    {
        Google,
        Facebook
    }
    public class AuthenticationState
    {
        public static OAuth2Authenticator Authenticator;
    }
}