﻿using IrrigationController.Model;
using IrrigationController.Network;
using IrrigationController.Service;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace IrrigationController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PiData : ContentPage
    {
        private readonly int piId;
        private Pi pi;
        public PiData()
        {
            InitializeComponent();
            BindingContext = new Pi();
        }
        public PiData(Pi aSelPi)
        {
            InitializeComponent();
            piId = aSelPi.Id;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var response = await App.PiService.GetOnePiByIdAsync(piId);
            switch (response.Status)
            {
                case Status.SUCCESS:
                    {
                        pi = response.Data;
                        BindingContext = response.Data;
                        break;
                    }
                case Status.NOT_FOUND:
                    {
                        await DisplayAlert("Error", response.StatusString, "Ok");
                        await Navigation.PopAsync();
                        break;
                    }
                case Status.OTHER_ERROR:
                    {
                        await DisplayAlert("Error", response.StatusString, "Ok");
                        break;
                    }
            }
        }
        public async void EditClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new PiEdit(pi));
        }
        public async void DeleteClicked(object sender, EventArgs args)
        {
            bool accepted = await DisplayAlert("Törlés",$"Biztos törli a(z) {pi.Nev} pit?", "Igen", "Nem");
            if (accepted)
            {
                var response = await App.PiService.DeleteTodoItemAsync(pi);
                switch (response.Status)
                {
                    case Status.SUCCESS:
                        {
                            await Navigation.PopAsync();
                            break;
                        }
                    case Status.OTHER_ERROR:
                        {
                            await DisplayAlert("Error", response.StatusString, "Ok");
                            break;
                        }
                }
            }
        }
    }
}