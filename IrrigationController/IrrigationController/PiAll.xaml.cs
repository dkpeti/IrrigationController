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
    public partial class PiAll : ContentPage
    {
        public PiAll()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {

        }
        public void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {

        }
        private async void PiAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PiAdd());
        }
    }
}