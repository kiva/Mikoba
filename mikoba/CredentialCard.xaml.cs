using System;
using Xamarin.Forms;

namespace mikoba
{
    public partial class CredentialCard : ContentView
    {
        public static readonly BindableProperty IssuedTextProperty =
            BindableProperty.Create("IssuedText", typeof(string), typeof(CredentialCard), default(string));
        public string IssuedText
        {
            get { return (string)GetValue(IssuedTextProperty); }
            set { SetValue(IssuedTextProperty, value); }
        }
        
        public static readonly BindableProperty OrganizationTextProperty =
            BindableProperty.Create("OrganizationText", typeof(string), typeof(CredentialCard), default(string));
        public string OrganizationText
        {
            get { return (string)GetValue(OrganizationTextProperty); }
            set { SetValue(OrganizationTextProperty, value); }
        }
        
        public static readonly BindableProperty MemberIdTextProperty =
            BindableProperty.Create("MemberIdText", typeof(string), typeof(CredentialCard), default(string));
        public string MemberIdText
        {
            get { return (string)GetValue(MemberIdTextProperty); }
            set { SetValue(MemberIdTextProperty, value); }
        }
        
        public static readonly BindableProperty ImageUrlSourceProperty =
            BindableProperty.Create("ImageUrlSource", typeof(string), typeof(CredentialCard), default(string));
        public string ImageUrlSource
        {
            get { return (string)GetValue(ImageUrlSourceProperty); }
            set { SetValue(ImageUrlSourceProperty, value); }
        }
        
        public CredentialCard()
        {
            InitializeComponent();
            issuedTextLabel.SetBinding(Label.TextProperty, new Binding("IssuedText", source: this));
            organizationTextLabel.SetBinding(Label.TextProperty, new Binding("OrganizationText", source: this));
            memberIdTextLabel.SetBinding(Label.TextProperty, new Binding("MemberIdText", source: this));
            imageUrlSource.SetBinding(Image.SourceProperty, new Binding("ImageUrlSource", source: this));
        }
    }
}