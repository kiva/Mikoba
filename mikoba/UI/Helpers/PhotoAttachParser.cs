using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace mikoba.UI.Helpers
{
    public class PhotoAttachParser
    {
        public static string ReturnAttachment(string base64Data)
        {
            string ret = base64Data;
            try
            {
                PhotoAttach jsonObject = JsonConvert.DeserializeObject<PhotoAttach>(base64Data);
                if (jsonObject.data != null)
                {
                    ret = jsonObject.data;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return ret;
        }
    }
}