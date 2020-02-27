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
    public partial class PiEdit : ContentPage
    {
        public PiEdit()
        {
            InitializeComponent();
        }
        public void SaveClicked(object sender, EventArgs args)
        {

        }
    }
}