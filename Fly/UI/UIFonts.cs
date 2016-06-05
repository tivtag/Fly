
namespace Fly.UI
{
    using Atom.Xna.Fonts;

    public static class UIFonts
    {
        public static void Load( IFontLoader fontLoader )
        {
            tahoma10 = fontLoader.Load( "Tahoma10" );
            tahoma14 = fontLoader.Load( "Tahoma14" );
            quartz10 = fontLoader.Load( "Quartz10" );
            quartz14 = fontLoader.Load( "Quartz14" );
        }

        public static IFont Tahoma14
        {
            get
            {
                return tahoma14;
            }
        }
        
        public static IFont Tahoma10
        {
            get
            {
                return tahoma10;
            }
        }

        public static IFont Quartz10
        {
            get
            {
                return quartz10;
            }
        }

        public static IFont Quartz14
        {
            get
            {
                return quartz14;
            }
        }

        private static IFont tahoma10, tahoma14;
        private static IFont quartz10, quartz14;
    }
}
