using System;
using Hyperledger.Aries.Extensions;
using Xamarin.Forms;

namespace mikoba.UI.Controls
{
    public class BorderlessEntry : Entry
    {
        public delegate void BackButtonPressEventHandler(object sender, EventArgs e);

        public event BackButtonPressEventHandler OnBackButton;
        
        public BorderlessEntry() {}

        public void OnBackButtonPress(object sender, EventArgs e)
        {
            Console.WriteLine("Here's your custom event");
            Console.WriteLine(e.ToJson());
            if (OnBackButton != null)
            {
                OnBackButton(sender, e);
            }
        }
    }
}