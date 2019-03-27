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

            return Color.FromRgb(
                255 - ((percentage <= 0.5) ? 123 * percentage*2 : 0), 
                255 - ((percentage > 0.5) ? 123 * (percentage-0.5)*2 : 0), 
                132);
        }
    }
}
