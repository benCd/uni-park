using System.Diagnostics;
using Xamarin.Forms;

namespace up_mobile.Map.Utils
{
    public class ColorMapper
    {
        public static Color MapPercentageToRGB(double percentage)
        {
            /*
             * Start: 132, 255, 132
             * Medium: 255, 255, 132
             * End: 255, 132, 132
             */

            int red = (int)(255.0 - ((percentage > 0.5) ? 123.0 * percentage * 2.0 : 0.0));
            int green = (int)(255 - ((percentage < 0.5) ? 123.0 * (percentage + 0.5) * 2.0 : 0.0));
            int blue = 132;


            Debug.Write(red + "\n" + green + "\n" + blue);

            return Color.FromRgb(red, green, blue);
        }
    }
}
