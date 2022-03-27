using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

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
        public double price { get; set; }
        public int ticketCountAvailable { get; set; }
    }
}
