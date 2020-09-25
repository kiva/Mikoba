using Autofac;
using Microsoft.Extensions.Hosting;
using Xamarin.Essentials;
using Xamarin.Forms;
using mikoba.Services;
using mikoba.UI.Pages;

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
