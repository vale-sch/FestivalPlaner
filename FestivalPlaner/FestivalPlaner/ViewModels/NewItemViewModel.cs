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
        private DateTime endDate = DateTime.UtcNow;
        private string place;
        private double price;
        private int ticketCountAvailable;
 
        public NewItemViewModel()
        {
            Save = new Command(OnSave, ValidateSave);
            Cancel = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => Save.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return true;
            //return !String.IsNullOrWhiteSpace(text)
              //  && !String.IsNullOrWhiteSpace(description);
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
            get => Services.GeoLocationService.Latitude.ToString() + "," + Services.GeoLocationService.Longitude.ToString();
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

        private async void OnSave()
        {
            FestivalModel newFestival = new FestivalModel()
            {
                _id = Guid.NewGuid().ToString(),
                name = Name,
                startDate = StartDate,
                endDate = EndDate,
                place = Place,
                price = Price,
                ticketCountAvailable = TicketCountAvailable,
            };
            var collection = App.databaseBase.GetCollection<FestivalModel>(App.collectionName);


            await collection.InsertOneAsync(newFestival);
            await DataStore.AddItemAsync(newFestival);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
