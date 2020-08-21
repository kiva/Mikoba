using System.Reflection;

namespace mikoba.ViewModels
{
    public class KivaBaseViewModel
    {
        public Assembly SvgAssembly
        {
            get { return typeof(App).GetTypeInfo().Assembly; }
        }

        public string KivaLogo
        {
            get { return "mikoba.Images.kiva.svg"; }
        }

        public string Wave
        {
            get { return "mikoba.Images.wave.svg"; }
        }

        public string Orange
        {
            get { return "mikoba.Images.orange.svg"; }
        }

        public string Dots
        {
            get { return "mikoba.Images.dots.svg"; }
        }
        
        public string Pink
        {
            get { return "mikoba.Images.pink.svg"; }
        }

        public string LocationPin
        {
            get { return "mikoba.Images.locationpin.svg"; }
        }
        
        public string QrCodeScan
        {
            get { return "mikoba.Images.qrCodeScan.svg"; }
        }
        
        public string RightCaret
        {
            get { return "mikoba.Images.rightCaret.svg"; }
        }
        
        public string Secure
        {
            get { return "mikoba.Images.secure.svg"; }
        }
        
        public string Selfie
        {
            get { return "mikoba.Images.selfie.svg"; }
        }
    }
}
