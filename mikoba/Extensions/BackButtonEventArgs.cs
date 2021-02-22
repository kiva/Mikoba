using System;

namespace mikoba.Extensions
{
    public class BackButtonEventArgs : EventArgs
    {
        public string NewValue { get; set; }
        public string OldValue { get; set; }

        public BackButtonEventArgs(string newValue, string oldValue)
        {
            NewValue = String.IsNullOrEmpty(newValue) ? "" : newValue;
            OldValue = String.IsNullOrEmpty(oldValue) ? "" : oldValue;
        }
    }
}