using System;
using mikoba.iOS.Renderers;
using mikoba.UI.Controls;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace mikoba.iOS.Renderers
{
    public class CustomTextField : UITextField
    {
        public delegate void DeleteBackwardKeyEventHandler(object sender, EventArgs e);
        
        public event DeleteBackwardKeyEventHandler OnDeleteBackwardKey;
        
        public void OnDeleteBackwardKeyPressed()
        {
            if (OnDeleteBackwardKey != null)
            {
                OnDeleteBackwardKey(null, null);
            }
        }

        public override void DeleteBackward()
        {
            base.DeleteBackward();
            OnDeleteBackwardKeyPressed();
        }
    }
}