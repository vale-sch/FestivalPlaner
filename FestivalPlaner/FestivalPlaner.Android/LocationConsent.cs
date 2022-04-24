using System.Threading.Tasks;
using Xamarin.Essentials;
using FestivalPlaner.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(LocationConsent))]
namespace FestivalPlaner.Droid
{
    public class LocationConsent : ILocationConsent
    {
        public async Task GetLocationConsent()
        {
            await Permissions.RequestAsync<Permissions.LocationAlways>();
        }
    }
}