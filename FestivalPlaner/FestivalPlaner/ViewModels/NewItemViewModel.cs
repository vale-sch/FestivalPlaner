using FestivalPlaner.Models;
using System;
using Xamarin.Forms;
using MongoDB.Driver;

namespace FestivalPlaner.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {

        private string name;
        private DateTime startDate = DateTime.UtcNow;
        private DateTime endDate = DateTime.UtcNow.AddDays(3).AddMinutes(50).AddSeconds(45);
        public static string place = "Add location via Maps";
        public static double latitude;
        public static double longitude;
        private double price;
        private int ticketCountAvailable;

        public NewItemViewModel()
        {
            Save = new Command(OnSave);
            Cancel = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => Save.ChangeCanExecute();
        }



        public Double Latitude
        {
            get => latitude;
            set => SetProperty(ref latitude, value);
        }
        public Double Longitude
        {
            get => longitude;
            set => SetProperty(ref longitude, value);
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

        public Command Save { get; }
        public Command Cancel { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        public async void OnSave()
        {
            FestivalModel newFestival = new FestivalModel()
            {
                _id = Guid.NewGuid().ToString(),
                name = Name,
                startDate = StartDate,
                endDate = EndDate,
                place = Place,
                latitude = Latitude,
                longitude = Longitude,
                price = Price,
                ticketCountAvailable = TicketCountAvailable,
            };
            var collection = App.databaseBase.GetCollection<FestivalModel>(App.collectionName);


            await collection.InsertOneAsync(newFestival);
            await DataStore.AddItemAsync(newFestival);
            App.festivals.Add(newFestival);
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
