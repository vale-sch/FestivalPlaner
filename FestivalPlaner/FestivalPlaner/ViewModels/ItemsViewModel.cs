using FestivalPlaner.Models;
using FestivalPlaner.Services;
using FestivalPlaner.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace FestivalPlaner.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private FestivalModel _selectedItem;
        public ObservableCollection<FestivalModel> Festivals { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Xamarin.Forms.Command<FestivalModel> ItemTapped { get; }

        public ItemsViewModel()
        {
            Title = "Browse Festivals";
            Festivals = new ObservableCollection<FestivalModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Xamarin.Forms.Command<FestivalModel>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        public async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                if (!GeoLocationService.isCeckingDatabase)
                    await App.LoadFestivalsFromDB();

                Festivals.Clear();

                foreach (FestivalModel festivalModel in App.festivals)
                {

                    await DataStore.AddItemAsync(festivalModel);
                    await DataStore.UpdateItemAsync(festivalModel);
                    Festivals.Add(festivalModel);
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public FestivalModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            NewItemViewModel.place = "Add location via Maps";
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(FestivalModel item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item._id}");
        }
    }
}