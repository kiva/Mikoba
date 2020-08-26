using Xamarin.Forms;
using SVG.Forms.Plugin.Abstractions;

namespace mikoba.UI
{
    public partial class CredentialCard : ContentView
    {
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
        
        public static readonly BindableProperty LogoProperty =
            BindableProperty.Create("Logo", typeof(string), typeof(CredentialCard), default(string));
        public string Logo
        {
            get { return (string)GetValue(LogoProperty); }
            set { SetValue(LogoProperty, value); }
        }

        public CredentialCard()
        {
            InitializeComponent();
            organizationTextLabel.SetBinding(Label.TextProperty, new Binding("OrganizationText", source: this));
            memberIdTextLabel.SetBinding(Label.TextProperty, new Binding("MemberIdText", source: this));
            logo.SetBinding(SvgImage.SvgPathProperty, new Binding("Logo", source: this));

        }
    }
} 