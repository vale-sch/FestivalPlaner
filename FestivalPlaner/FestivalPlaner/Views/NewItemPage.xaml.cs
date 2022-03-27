using FestivalPlaner.Models;
using FestivalPlaner.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FestivalPlaner.Views
{
    public partial class NewItemPage : ContentPage
    {
        public FestivalModel Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}