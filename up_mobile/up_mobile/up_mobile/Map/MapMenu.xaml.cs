using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Map.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Diagnostics;

namespace up_mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapMenu : ScrollView
    {
        /// <summary>
        /// 
        /// </summary>
        StackLayout Stack;

        public MapMenu()
        {
            Orientation = ScrollOrientation.Horizontal;
            HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 18;
            BackgroundColor = Color.Black;

            Stack = new StackLayout();
            Populate();
            Stack.Orientation = StackOrientation.Horizontal;
            Content = Stack;
        }

        private void Populate()
        {
            //-----------------------------------------------------------------
            //TODO Implement rest request for university data and their lot IDs
            //-----------------------------------------------------------------
            for (int i = 0; i < 3; i++)
            {
                Stack.Children.Add(new RememberButton("Button " + i, i));
            }
        }

        /// <summary>
        /// Button that keeps the necesssary information to switch maps in <see cref="MapContentPage"/> using <see cref="MapContentPage.MoveToLot(int)"/>
        /// </summary>
        private class RememberButton : Button
        {
            /// <summary>
            /// Gets the ID set to this button.
            /// </summary>
            public int IDval { get; }

            public RememberButton(string label, int idval) : base()
            {
                IDval = idval;
                Text = label;
                Clicked += async (sender, args) => MapContentPage.MoveToLot(IDval);
            }
            
        }
    }
}