using IrrigationController.Network;
using IrrigationController.Service;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    public partial class App : Application
    {
        public static IZonaService ZonaService { get; private set; }

        public App()
        {
            InitializeComponent();

            ZonaService = new ZonaRestService();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
