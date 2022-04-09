using FestivalPlaner.Services;
using FestivalPlaner.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MongoDB.Driver;
using FestivalPlaner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FestivalPlaner.ViewModels;

namespace FestivalPlaner
{
    public partial class App : Application
    {
        private readonly static string connectionString = "mongodb://admin:Test123@festivalapp-shard-00-00.oheeh.mongodb.net:27017,festivalapp-shard-00-01.oheeh.mongodb.net:27017,festivalapp-shard-00-02.oheeh.mongodb.net:27017/myFirstDatabase?ssl=true&replicaSet=atlas-nvh5zi-shard-0&authSource=admin&retryWrites=true&w=majority";
        private readonly static string databaseName = "FestivalDB";
        public readonly static string collectionName = "Festival";
        public static MongoClient client;
        public static IMongoDatabase databaseBase;
        public static List<FestivalModel> festivals = new List<FestivalModel>();

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        async protected override void OnStart()
        {

            client = new MongoClient(App.connectionString);
            databaseBase = client.GetDatabase(App.databaseName);
            DependencyService.Register<MockDataStore>();
            GeoLocationService geoLocationService = new GeoLocationService();
            await loadFestivalsFromDB();
            await  geoLocationService.GetCurrentLocation();
            
        }
        public static async Task loadFestivalsFromDB()
        {
            festivals.Clear();


            var collection = databaseBase.GetCollection<FestivalModel>(App.collectionName);

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
                    latitude = item.latitude,
                    longitude = item.longitude,
                    price = item.price,
                    ticketCountAvailable = item.ticketCountAvailable
                };
                festivals.Add(festivalModel);
            }
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


    }
}
