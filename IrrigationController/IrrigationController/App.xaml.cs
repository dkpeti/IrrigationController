using IrrigationController.Network;
using IrrigationController.Service;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    public partial class App : Application
    {
        private static HttpAPI HttpAPI { get; set; }
        public static IZonaService ZonaService { get; private set; }
        public static IPiService PiService { get; private set; }
        public static ILoginService LoginService { get; private set; }

        public App()
        {
            InitializeComponent();
            HttpAPI = new HttpAPI("https://192.168.1.106:45455/api");

            ZonaService = new ZonaRestService(HttpAPI);
            PiService = new PiRestService(HttpAPI);
            LoginService = new LoginService(HttpAPI);
            MainPage = new NavigationPage(new Bejelentkezes());
        }

        protected override async void OnStart()
        {
            if(await LoginService.LoginCheck())
            {
                MainPage = new NavigationPage(new TabbedMainPage());
            }
            else
            {
                MainPage = new NavigationPage(new Bejelentkezes());
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
