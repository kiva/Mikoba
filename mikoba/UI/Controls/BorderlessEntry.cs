using System;
using Hyperledger.Aries.Extensions;
using mikoba.Extensions;
using Xamarin.Forms;

namespace mikoba.UI.Controls
{
    public class BorderlessEntry : Entry
    {
        public delegate void BackButtonPressEventHandler(object sender, BackButtonEventArgs e);

        public event BackButtonPressEventHandler OnBackButton;
        
        public BorderlessEntry() {}

        public void OnBackButtonPress(object sender, BackButtonEventArgs e)
        {
            if (OnBackButton != null)
            {
                OnBackButton(sender, e);
            }
        }
    }
}