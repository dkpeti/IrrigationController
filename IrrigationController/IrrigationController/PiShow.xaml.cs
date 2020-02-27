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
    public partial class PiShow : ContentPage
    {
        public PiShow()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {

        }
        public void EditClicked(object sender, EventArgs args)
        {
            
        }
        public void DeleteClicked(object sender, EventArgs args)
        {
            
        }
    }
}