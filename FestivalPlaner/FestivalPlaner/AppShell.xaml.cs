using FestivalPlaner.ViewModels;
using FestivalPlaner.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FestivalPlaner
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(MapPupUp), typeof(MapPupUp));
        }
    }
}
