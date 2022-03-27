using FestivalPlaner.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FestivalPlaner.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}