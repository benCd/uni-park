using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile.Map
{
    /// <summary>
    /// MapView stores <see cref="MapContentPage"/> components for display.
    /// It will automatically generate the necessary maps if they are not 
    /// stored already and save them, just refreshing the pins when loaded.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : TabbedPage
    {
        /// <summary>
        /// Constructor for MapView instances. Creates new <see cref="MapContentPage"/>
        /// objects if necessary only!
        /// </summary>
        public MapView()
        {
            this.Children.Add(new MapContentPage(0));
        }
    }
}