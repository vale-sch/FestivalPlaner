using FestivalPlaner.Models;
using FestivalPlaner.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using MongoDB.Driver;
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

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Festivals.Clear();
                
              
                var collection = App.databaseBase.GetCollection<FestivalModel>(App.collectionName);

                var results = await collection.FindAsync(_ => true);
                foreach (var item in results.ToList())
                {
                    var festivalModel = new FestivalModel
                    {
                        _id = item._id,
                        name = item.name,
                        startDate = item.startDate,
                        endDate = item.endDate,
                        place = item.place,
                        price = item.price,
                        ticketCountAvailable = item.ticketCountAvailable
                    };
                     Festivals.Add(festivalModel);
                   await DataStore.AddItemAsync(festivalModel);
                    await DataStore.UpdateItemAsync(festivalModel);
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