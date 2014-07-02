namespace Properties
{
    internal class Resources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static System.Globalization.CultureInfo resourceCulture;
        internal Resources()
        {
        }
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (ReferenceEquals(resourceMan, null))
                {
                    resourceMan = new System.Resources.ResourceManager("Properties.Resources", typeof(Resources).Assembly);
                }
                return resourceMan;
            }
        }
        internal static System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
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
