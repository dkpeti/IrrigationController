using IrrigationController.Class;
using Realms;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditZona : ContentPage
    {
        readonly private Zona mSelZona;

        public EditZona(Zona mSelZona)
        {
            InitializeComponent();
            this.mSelZona = mSelZona;
            BindingContext = mSelZona;
        }

        public void OnSaveClicked(object sender, EventArgs args)
        {
            var vRealmDb = Realm.GetInstance();
            using (var trans = vRealmDb.BeginWrite())
            {
                mSelZona.Nev = txtZonaNev.Text;
                trans.Commit();
            }
            Navigation.PopToRootAsync();
        }
    }
}