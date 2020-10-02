using System;
using System.Timers;

namespace mikoba.Services
{
    public class MediatorTimerService : IMediatorTimerService
    {
        private readonly Timer timer;
        private Action Action;

        public MediatorTimerService(Action action)
        {
            timer = new Timer
            {
                Enabled = false,
                AutoReset = true,
                Interval = TimeSpan.FromSeconds(10).TotalMilliseconds
            };
            this.Action = action;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.Action != null)
            {
                this.Action();
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
