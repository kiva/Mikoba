using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Hyperledger.Aries.Extensions;
using mikoba.Droid.Renderers;
using mikoba.UI.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace mikoba.Droid.Renderers
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public BorderlessEntryRenderer(Context context) : base(context) {}

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;
            }
            
            if (e.NewElement != null)
            {
                var entry = e.NewElement as BorderlessEntry;
            
                EditText.KeyPress += (sender, args) =>
                {
                    Console.WriteLine(args.ToJson());
                    if (args.KeyCode == Keycode.Del)
                    {
                        var customEvent = new BackButtonEventArgs(NewValue, OldValue);
                        entry.OnBackButtonPress(sender, customEvent);
                    }
                };
            
                entry.TextChanged += (sender, args) =>
                {
                    Console.WriteLine("Definitely happening");
                    OldValue = args.OldTextValue;
                    NewValue = args.NewTextValue;
                };
            }
        }
    }

    public class BackButtonEventArgs : EventArgs
    {
        public string NewValue { get; set; }
        public string OldValue { get; set; }

        public BackButtonEventArgs(string newValue, string oldValue)
        {
            NewValue = String.IsNullOrEmpty(newValue) ? "" : newValue;
            OldValue = String.IsNullOrEmpty(oldValue) ? "" : oldValue;
        }
    }
}