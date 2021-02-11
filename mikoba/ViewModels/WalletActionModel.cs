using mikoba.ViewModels;
using System.Windows.Input;

namespace mikoba.UI.ViewModels
{
    public class WalletActionModel : MikobaBaseViewModel
    {
        public string RightIcon { get; set; }
        public string LeftIcon { get; set; }
        public string ActionLabel { get; set; }
        public ICommand ActionCommand { get; set; }
    }
}
