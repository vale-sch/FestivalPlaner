using FestivalPlaner.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using MongoDB.Driver;
using MongoDB.Bson;

namespace FestivalPlaner.ViewModels
{

    [QueryProperty(nameof(ItemId), nameof(ItemId))]
  
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId { get; set; }
        private string name;
        private DateTime startDate;
        private DateTime endDate;
        private string place;
        private double price;
        private int ticketCountAvailable;

       
     
        public ItemDetailViewModel()
        {
            Delete = new Command(RemoveFestival);
        }
    
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public DateTime StartDate
        {
            get => startDate;
            set => SetProperty(ref startDate, value);
        }
        public DateTime EndDate
        {
            get => endDate;
            set => SetProperty(ref endDate, value);
        }
        public string Place
        {
            get => place;
            set => SetProperty(ref place, value);
        }
        public double Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }
        public int TicketCountAvailable
        {
            get => ticketCountAvailable;
            set => SetProperty(ref ticketCountAvailable, value);

        }
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                itemId = item._id;
                Name = item.name;
                StartDate = item.startDate;
                EndDate = item.endDate;
                Place = item.place;
                Price = item.price;
                TicketCountAvailable = item.ticketCountAvailable;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        public Command Delete { get; }
        private async void RemoveFestival()
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                var festivalToDelete = new FestivalModel()
                {
                    _id = item._id,
                    name = item.name,
                    startDate = item.startDate,
                    endDate = item.endDate,
                    place = item.place,
                    price = item.price,
                    ticketCountAvailable = item.ticketCountAvailable
                };
               
                var filterbak = Builders<FestivalModel>.Filter.Eq("_id", festivalToDelete._id);
                var collection = App.databaseBase.GetCollection<FestivalModel>(App.collectionName);
                await collection.DeleteOneAsync(filterbak);
                await DataStore.DeleteItemAsync(festivalToDelete._id);
                await Shell.Current.GoToAsync("..");

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Remove Item");
            }
           
        }
    }
}
