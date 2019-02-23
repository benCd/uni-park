using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile.Map
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : TabbedPage
    {
        public MapView()
        {
            this.Children.Add(new MapContentPage(0));
            //InitializeComponent();
        }
    }
}