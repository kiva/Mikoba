using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;
using Hyperledger.Aries.Contracts;
using mikoba.Extensions;
using SVG.Forms.Plugin.Abstractions;

namespace mikoba.UI.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Notification : ContentView
    {
        #region Services
        private readonly IEventAggregator eventAggregator;
        #endregion
        
        public static readonly BindableProperty NotificationTextProperty =
            BindableProperty.Create("NotificationText", typeof(string), typeof(Notification), default(string));
        public string NotificationText
        {
            get { return (string)GetValue(NotificationTextProperty); }
            set { SetValue(NotificationTextProperty, value); }
        }

        public static readonly BindableProperty LeftSvgImageProperty =
            BindableProperty.Create("LeftSvgImage", typeof(string), typeof(Notification), default(string));
        public string LeftSvgImage
        {
            get { return (string)GetValue(LeftSvgImageProperty); }
            set { SetValue(LeftSvgImageProperty, value); }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(MenuOption), null);
        

        public Notification()
        {
            InitializeComponent();
            leftSvgImage.SetBinding(SvgImage.SvgPathProperty, new Binding("LeftSvgImage", source: this));
        }

        void OnSwiped(System.Object sender, System.EventArgs e)
        {
            eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.NotificationDismissed});
        }
    }
}
