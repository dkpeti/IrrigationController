using IrrigationController.Class;
using Realms;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddZona : ContentPage
    {
        public AddZona()
        {
            InitializeComponent();
        }
        public void OnSaveClicked(object sender, EventArgs args)
        {
            var vRealmDb = Realm.GetInstance();
            var vZonaId = vRealmDb.All<Zona>().Count() + 1;
            var vZona = new Zona()
            {
                ZonaId = vZonaId,
                Nev = txtZonaNev.Text
            };
            vRealmDb.Write(() => {
                vZona = vRealmDb.Add(vZona);
            });
            Navigation.PopAsync();
        }
    }
}