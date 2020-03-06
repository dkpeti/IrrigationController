using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrrigationController
{
    [ContentProperty(nameof(MainContent))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasePage : ContentPage
    {
        public BasePage()
		{
			//SetBinding(IsBusyProperty, new Binding(nameof(IsBusy)));
			InitializeComponent();
        }

		public static BindableProperty MainContentProperty = BindableProperty.Create(nameof(MainContent), typeof(View), typeof(BasePage));

		public View MainContent
		{
			get { return (View)GetValue(MainContentProperty); }
			set { SetValue(MainContentProperty, value); }
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (MainContent == null)
			{
				return;
			}
			SetInheritedBindingContext(MainContent, BindingContext);
		}
	}
}