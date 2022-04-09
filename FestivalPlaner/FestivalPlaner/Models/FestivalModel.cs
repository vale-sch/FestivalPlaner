using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using Xamarin.Forms.Maps;

namespace FestivalPlaner.Models
{
    public class FestivalModel
    {
        [BsonId]
        public string _id { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string place { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double price { get; set; }
        public int ticketCountAvailable { get; set; }
    }
}
