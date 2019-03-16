namespace up_mobile.Map.Utils
{
    /// <summary>
    /// Class that provides a static cleanup method for the map environment
    /// </summary>
    public class MapCleaner
    {
        public static async void CleanUp()
        {
            MapContentPage.map = null;
        }
    }
}
