using System;
using Xamarin.Forms;

namespace mikoba.UI
{
    public partial class AddNewButton : ContentView
    {
        public event EventHandler Clicked;

        public AddNewButton()
        {
            InitializeComponent();
        }

        private void OnScanQrCodePageButtonClicked(object sender, EventArgs e)
        {
            
        }
    }
}
