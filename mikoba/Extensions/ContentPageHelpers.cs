using Xamarin.Forms;

namespace mikoba.Extensions
{
    public static class ContentPageHelpers
    {
        public static void CorrectSafeMargin(this ContentPage source)
        {
            var current = source.Padding;
            current.Bottom = 0;
            source.Padding = current;
        }
    }
}
