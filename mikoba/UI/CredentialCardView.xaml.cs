using System;
using Xamarin.Forms;

namespace mikoba.UI
{
    public partial class CredentialCardView : ContentView
    {
        public static readonly BindableProperty CredentialTextProperty =
            BindableProperty.Create("CredentialText", typeof(string), typeof(ImageButton), default(string));

        public string CredentialText
        {
            get { return (string)GetValue(CredentialTextProperty); }
            set { SetValue(CredentialTextProperty, value); }
        }

        public event EventHandler Clicked;

        public CredentialCardView()
        {
            InitializeComponent();
            innerLabel.SetBinding(Label.TextProperty, new Binding("CredentialText", source: this));
        }
    }
}
