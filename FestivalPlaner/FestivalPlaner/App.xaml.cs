using FestivalPlaner.Services;
using FestivalPlaner.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MongoDB.Driver;

namespace FestivalPlaner
{
    public partial class App : Application
    {
        public readonly static string connectionString = "mongodb://admin:Test123@festivalapp-shard-00-00.oheeh.mongodb.net:27017,festivalapp-shard-00-01.oheeh.mongodb.net:27017,festivalapp-shard-00-02.oheeh.mongodb.net:27017/myFirstDatabase?ssl=true&replicaSet=atlas-nvh5zi-shard-0&authSource=admin&retryWrites=true&w=majority";
        public readonly static string databaseName = "FestivalDB";
        public readonly static string collectionName = "Festival";
        public static MongoClient client;
        public static IMongoDatabase databaseBase;


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
            await GeoLocationService.GetCurrentLocation();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


    }
}
