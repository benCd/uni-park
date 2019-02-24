using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Diagnostics;

namespace up_mobile.Map
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapMenu : ScrollView
    {
        public MapMenu()
        {
            Orientation = ScrollOrientation.Horizontal;
            HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 18;
            BackgroundColor = Color.Black;
        }

        private void Populate()
        {
            //-----------------------------------------------------------------
            //TODO Implement rest request for university data and their lot IDs
            //-----------------------------------------------------------------
        }

    }
}