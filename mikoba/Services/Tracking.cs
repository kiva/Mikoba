using System;
using Hyperledger.Indy.PoolApi;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Sentry;
using Sentry.Protocol;

namespace mikoba.Services
{
    public static class Tracking
    {
        public static void TrackException(Exception ex, string message = "")
        {
            Crashes.TrackError(ex);
            SentrySdk.CaptureException(ex);
            if (message.Length > 0)
            {
                SentrySdk.CaptureMessage(message);
            }
        }

        public static void TrackEvent(string @event)
        {
            SentrySdk.CaptureEvent(new SentryEvent()
            {
                Message = @event,
                Level = SentryLevel.Info
            });
            Analytics.TrackEvent(@event);
        }
    }
}
