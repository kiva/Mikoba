using System;
using Autofac;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Runtime;
using mikoba.Extensions;
using Xamarin.Forms;
using SVG.Forms.Plugin.Abstractions;
using Xamarin.Forms.Xaml;
using mikoba.ViewModels.Components;

namespace mikoba.UI.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Notification : ContentView
    {
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
        
        public Notification()
        {
            InitializeComponent();
            leftSvgImage.SetBinding(SvgImage.SvgPathProperty, new Binding("LeftSvgImage", source: this));
        }

        private void SwipeGestureRecognizer_OnSwiped(object sender, SwipedEventArgs e)
        {
            var eventAggregator =  App.Container.Resolve<IEventAggregator>();
            eventAggregator.Publish(new CoreDispatchedEvent() { Type = DispatchType.NotificationDismissed });
        }
    }
}
