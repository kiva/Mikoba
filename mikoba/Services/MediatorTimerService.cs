using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Autofac;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Routing;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.Services
{
    
    
    public class MediatorTimerService : IMediatorTimerService
    {
        private readonly Timer timer;

        public MediatorTimerService()
        {
            timer = new Timer
            {
                Enabled = false,
                AutoReset = true,
                Interval = TimeSpan.FromSeconds(10).TotalMilliseconds
            };
            timer.Elapsed += Timer_Elapsed;
        }
        
        public MediatorTimerService(Action action)
        {
            timer = new Timer
            {
                Enabled = false,
                AutoReset = true,
                Interval = TimeSpan.FromSeconds(10).TotalMilliseconds
            };
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Preferences.Get(AppConstant.LocalWalletProvisioned, false))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        var context = await App.Container.Resolve<IAgentProvider>().GetContextAsync();
                        var results = await App.Container.Resolve<IEdgeClientService>().FetchInboxAsync(context);
                        if (results.unprocessedItems.Count() > 0)
                        {
                            foreach(var item in results.unprocessedItems)
                            {
                                Console.WriteLine(item.Type);
                            }    
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                });
            }
        }

        public void Start()
        {
            this.timer.Enabled = true;
        }
        public void Pause()
        {
            this.timer.Enabled = false;
        }
        
        public void Resume()
        {
            this.timer.Enabled = true;
        }
    }
}
