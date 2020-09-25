using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Timers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Routing;
using Hyperledger.Aries.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sentry;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using mikoba.Services;
using mikoba.UI.Pages;
using West.Extensions.Xamarin;

namespace mikoba
{
    public partial class App : Application
    {
        public new static App Current => Application.Current as App;
        public static IContainer Container { get; set; }
        private static IHost Host { get; set; }
        public App(IHost host) : this() => Host = host;

        private MediatorTimerService _mediatorTimerService;

        public App()
        {
            InitializeComponent();
            this.StartServices();
            Preferences.Clear();
        }

        private void StartServices()
        {
            _mediatorTimerService = new MediatorTimerService();
        }

        protected override async void OnStart()
        {
            await Host.StartAsync();
            MainPage = NavigationService.CreateMainPage(() => new SplashPage());
            _mediatorTimerService.Start();
        }
        
        protected override void OnSleep()
        {
            _mediatorTimerService.Pause();
        }

        protected override void OnResume()
        {
            _mediatorTimerService.Resume();
        }

    }
}
