using FestivalPlaner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace FestivalPlaner.Services
{
    public class MockDataStore : IDataStore<FestivalModel>
    {
        private List<FestivalModel> items = new List<FestivalModel>();
        public MockDataStore()
        {

        }

        public async Task<bool> AddItemAsync(FestivalModel item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(FestivalModel item)
        {
            var oldItem = items.Where((FestivalModel arg) => arg._id == item._id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((FestivalModel arg) => arg._id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<FestivalModel> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s._id == id));
        }

        public async Task<IEnumerable<FestivalModel>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}