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
        public static IPiService PiService { get; private set; }

        public App()
        {
            InitializeComponent();

            ZonaService = new ZonaRestService();

            PiService = new PiRestService();

            MainPage = new NavigationPage(new TabbedMainPage());
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
