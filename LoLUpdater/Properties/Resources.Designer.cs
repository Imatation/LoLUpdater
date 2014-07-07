namespace LoLUpdater.Properties
{
    internal class Resources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static System.Globalization.CultureInfo resourceCulture;
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    resourceMan = new System.Resources.ResourceManager("LoLUpdater.Properties.Resources", typeof(Resources).Assembly);
                }
                return resourceMan;
            }
        }
        internal static byte[] Cg_3_1_April2012_Setup
        {
            get
            {
                return ((byte[])(ResourceManager.GetObject("Cg_3_1_April2012_Setup", resourceCulture)));
            }
        }
        internal static byte[] tbb
        {
            get
            {
                return ((byte[])(ResourceManager.GetObject("tbb", resourceCulture)));
            }
        }
    }
}