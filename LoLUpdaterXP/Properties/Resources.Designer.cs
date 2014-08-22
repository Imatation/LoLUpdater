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
        internal static System.Drawing.Bitmap close
        {
            get
            {
                return ((System.Drawing.Bitmap)(ResourceManager.GetObject("close", resourceCulture)));
            }
        }
        internal static System.Drawing.Bitmap closemouseenter
        {
            get
            {
                return ((System.Drawing.Bitmap)(ResourceManager.GetObject("closemouseenter", resourceCulture)));
            }
        }

        internal static System.Drawing.Bitmap minimize
        {
            get
            {
                return ((System.Drawing.Bitmap)(ResourceManager.GetObject("minimize", resourceCulture)));
            }
        }
        internal static System.Drawing.Bitmap minimizemouseneter
        {
            get
            {
                return ((System.Drawing.Bitmap)(ResourceManager.GetObject("minimizemouseneter", resourceCulture)));
            }
        }
    }
}
