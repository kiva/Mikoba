
using System.Reflection;
using SVG.Forms.Plugin.Abstractions;

namespace mikoba.UI.Components
{
    public class KivaSvgImage : SvgImage
    {
        private string _imageKey;
        
        public KivaSvgImage()
        {
            this.SvgAssembly = typeof(App).GetTypeInfo().Assembly;
        }

        public string ImageKey
        {
            set
            {
                _imageKey = value;
                this.SvgPath = "mikoba.Images." + value + ".svg";
            }
            get
            {
                return _imageKey;
            }
        }
    }
}
