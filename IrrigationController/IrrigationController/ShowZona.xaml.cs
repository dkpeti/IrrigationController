using IrrigationController.Class;
using Realms;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowZona : ContentPage
    {
        readonly Zona mSelZona;
        public ShowZona()
        {
            InitializeComponent();
        }
        public ShowZona(Zona aSelZona)
        {
            InitializeComponent();
            mSelZona = aSelZona;
            BindingContext = mSelZona;
        }

        public void OnEditClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new EditZona(mSelZona));
        }
        public async void OnDeleteClicked(object sender, EventArgs args)
        {
            bool accepted = await DisplayAlert("Confirm", "Biztos törölni akarod?", "Igen", "Nem");
            if (accepted)
            {
                var vRealmDb = Realm.GetInstance();
                var vSelZona = vRealmDb.All<Zona>().First(b => b.ZonaId == mSelZona.ZonaId);

                // Delete an object with a transaction  
                using (var trans = vRealmDb.BeginWrite())
                {
                    vRealmDb.Remove(vSelZona);
                    trans.Commit();
                }
            }
            await Navigation.PopToRootAsync();
        }
    }
}