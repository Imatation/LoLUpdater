namespace LoLUpdater.Properties
{
    using System;
    internal class Resources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static System.Globalization.CultureInfo resourceCulture;
        internal Resources()
        {
        }
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    resourceMan = new global::System.Resources.ResourceManager("LoLUpdater.Properties.Resources", typeof(Resources).Assembly);
                }
                return resourceMan;
            }
        }
        internal static global::System.Globalization.CultureInfo Culture
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
        internal static byte[] Adobe_AIR
        {
            get
            {
                return ((byte[])(ResourceManager.GetObject("Adobe_AIR", resourceCulture)));
            }
        }
        internal static byte[] Cg_3_1_April2012_Setup
        {
            get
            {
                return ((byte[])(ResourceManager.GetObject("Cg_3_1_April2012_Setup", resourceCulture)));
            }
        }
        internal static byte[] NPSWF32
        {
            get
            {
                return ((byte[])(ResourceManager.GetObject("NPSWF32", resourceCulture)));
            }
        }
        internal static string Program_Main_Done_
        {
            get
            {
                return ResourceManager.GetString("Program_Main_Done_", resourceCulture);
            }
        }
        internal static string Program_Main_Patching_
        {
            get
            {
                return ResourceManager.GetString("Program_Main_Patching_", resourceCulture);
            }
        }
        internal static byte[] tbb
        {
            get
            {
                return ((byte[])(ResourceManager.GetObject("tbb", resourceCulture)));
            }
        }
        internal static string Terms
        {
            get
            {
                return ResourceManager.GetString("Terms", resourceCulture);
            }
        }
    }
}
