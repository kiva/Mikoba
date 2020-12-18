using West.Extensions.Xamarin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mikoba.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowError(string message,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                buttonText);

            if (afterHideCallback != null)
            {
                afterHideCallback();
            }
        }

        public async Task ShowError(
            Exception error,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                error.Message,
                buttonText);

            if (afterHideCallback != null)
            {
                afterHideCallback();
            }
        }

        public async Task ShowMessage(
            string message,
            string title)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                "OK");
        }

        public async Task ShowMessage(
            string message,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                buttonText);

            if (afterHideCallback != null)
            {
                afterHideCallback();
            }
        }

        public async Task<bool> ShowMessage(
            string message,
            string title,
            string buttonConfirmText,
            string buttonCancelText,
            Action<bool> afterHideCallback)
        {
            var result = await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                buttonConfirmText,
                buttonCancelText);

            if (afterHideCallback != null)
            {
                afterHideCallback(result);
            }

            return result;
        }

        public async Task ShowMessageBox(
            string message,
            string title)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                "OK");
        }

        public async Task ShowAlertAsync(string message, string title, string cancelLabel)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                cancelLabel);
        }

        public async Task<bool> ShowConfirmAsync(string message, string title, string acceptLabel, string cancelLabel)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SelectActionAsync(string message, string title, IEnumerable<string> buttons)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SelectActionAsync(string message, string title, string cancelLabel, IEnumerable<string> buttons)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SelectActionAsync(string message, string title, string cancelLabel, string destructiveLabel, IEnumerable<string> buttons)
        {
            throw new NotImplementedException();
        }
    }
}
