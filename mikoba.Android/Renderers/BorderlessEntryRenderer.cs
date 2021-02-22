using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Hyperledger.Aries.Extensions;
using Java.Util;
using mikoba.Droid.Renderers;
using mikoba.Extensions;
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

        protected BackButtonEventArgs TheEvent { get; set; }

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

                ((EditText) Control).KeyPress += (sender, ev) =>
                {
                    ev.Handled = false;
                    if (ev.KeyCode == Keycode.Del)
                    {
                        if (TheEvent != null)
                        {
                            if (OldValue == TheEvent.OldValue)
                            {
                                OldValue = "";
                            }
                        }
                        TheEvent = new BackButtonEventArgs(NewValue, OldValue);
                        entry.OnBackButtonPress(entry, TheEvent);
                        ev.Handled = true;
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
}