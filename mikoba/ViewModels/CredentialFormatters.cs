using System;
using mikoba.ViewModels.SSI;

namespace mikoba.ViewModels
{
    public class CredentialFormatters
    {
        
        public static string FormatCredentialValue(CredentialOverlay overlay, string name, string value)
        {
            if (overlay.Fields[name].DataType == "date")
            {
                var dateStr = "";
                if (TryFormatDateValue(value, out dateStr))
                {
                    return dateStr;
                }
                return value + " -> (Not A Valid Date)";
            } 
            
            if (overlay.Fields[name].DataType == "time")
            {
                TimeSpan timeObj;
                if (TimeSpan.TryParse(value, out timeObj))
                {
                    return timeObj.ToString("g");
                }
                return value + " -> (Not A Valid Time)";
            }
            
            return value;
        }

        public static bool TryFormatDateValue(string value, out string result)
        {
            try
            {
                //Support a preferredFormat attribute in the Credential Overlay.
                var millisecondUnixTimeStamp = Convert.ToDouble(value) * 1000;
                var date = UnixTimestampToDateTime(millisecondUnixTimeStamp);
                var format = "yyyy'-'MM'-'dd";
                result = date.ToString(format);
                return true;
            }
            catch
            {
                result = value;
                return false;
            }
        }
        
        private static DateTime UnixTimestampToDateTime(double unixTime)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddMilliseconds(unixTime);
            return dtDateTime;
        }
    }
}