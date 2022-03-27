using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FestivalPlaner.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T FestivalModel);
        Task<bool> UpdateItemAsync(T FestivalModel);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
