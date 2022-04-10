using FestivalPlaner.Models;
using FestivalPlaner.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FestivalPlaner.Views
{
    public partial class NewItemPage : ContentPage
    {

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.ShowPopup(new MapPupUp(this)
            {
                IsLightDismissEnabled = false,
                BindingContext = this
                
            });
        }
        public void OnSafe(string place)
        {
            Place.Text = place;
            LocationButton.IsEnabled = false;
            LocationButton.IsVisible = false;
        }
    }
}