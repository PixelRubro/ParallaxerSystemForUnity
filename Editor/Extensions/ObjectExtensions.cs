namespace PixelSpark.Parallaxer.Extensions
{
    public static class ObjectExtensions 
    {
        public static bool ToBool(this System.Object self, out bool result)
        {
            result = false;

            if (self == null)
                return false;

            if (self is bool value)
            {
                result = value;
                return true;
            }

            return false;
        }
    }
}