using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FestivalPlaner.Views
{
    public partial class MapPupUp : Popup 
    {
        public MapPupUp()
        {
            InitializeComponent();
            BindingContext = this;    
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
            
           
        }

        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(Convert.ToDouble(e.Position.Latitude), Convert.ToDouble(e.Position.Longitude)),
                Label = "custom pin",
                Address = "custom detail info"
            };
         
            Map.Pins.Add(pin);
        }
    }
}