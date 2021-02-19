using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Remoting.Channels;
using Hyperledger.Aries.Extensions;
using mikoba.iOS.Renderers;
using mikoba.UI.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace mikoba.iOS.Renderers
{
    public class BorderlessEntryRenderer : EntryRenderer, IUITextFieldDelegate
    {
        IElementController ElementController => Element as IElementController;

        public string OldValue { get; set; }
        public string NewValue { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            if (Element == null)
            {
                return;
            }

            if (Control != null)
            {
                Control.Layer.BorderWidth = 0;
                Control.BorderStyle = UITextBorderStyle.None;
                Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
            }

            var entry = (BorderlessEntry) Element;
            entry.TextChanged += (sender, ev) =>
            {
                OldValue = ev.OldTextValue;
                NewValue = ev.NewTextValue;
            };
            var textField = new CustomTextField();
            textField.EditingChanged += OnEditingChanged;
            textField.OnDeleteBackwardKey += (sender, ev) =>
            {
                var customEvent = new BackButtonEventArgs(NewValue, OldValue);
                entry.OnBackButtonPress(sender, customEvent);
            };

            SetNativeControl(textField);

            base.OnElementChanged(e);

            if (Element.Keyboard != null && Element.Keyboard == Keyboard.Numeric)
            {
                AddDoneButton();
            }
        }

        /*
         * Thanks to Evgeny Zborovsky for his writeup on adding a Done button to an iOS
         * numeric keyboard.
         */
        private void AddDoneButton()
        {
            var toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));

            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
            {
                Control.ResignFirstResponder();
                ((IEntryController)Element).SendCompleted();
            });

            toolbar.Items = new UIBarButtonItem[] {
                new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
                doneButton
            };
            Control.InputAccessoryView = toolbar;
        }

        void OnEditingChanged(object sender, EventArgs e)
        {
            ElementController.SetValueFromRenderer(Entry.TextProperty, Control.Text);
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