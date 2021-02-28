using Acr.UserDialogs;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mikoba.Services
{
    public class DialogService : IUserDialogs
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
                "Ok",
                "Cancel");
        }

        public async Task AlertAsync(
            string message,
            string title = null,
            string okText = null,
            CancellationToken? cancelToken = null)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                okText);
        }
        
        public async Task AlertAsync(AlertConfig config, CancellationToken? cancelToken = null) {
            await Application.Current.MainPage.DisplayAlert(
                config.Title,
                config.Message,
                config.OkText);
        }

        public async Task<string> ActionSheetAsync(
            string title,
            string cancel,
            string destructive,
            CancellationToken? cancelToken = null,
            params string[] buttons)
        {
            throw new NotImplementedException();
        }

        public IDisposable ActionSheet(ActionSheetConfig config)
        {
            throw new NotImplementedException();
        }


        public IDisposable Alert(AlertConfig alertConfig)
        {
            return Application.Current.MainPage.DisplayAlert(
                alertConfig.Title,
                alertConfig.Message,
                alertConfig.OkText
            );
        }
        
        public IDisposable Alert(string message, string title, string cancelLabel)
        {
            return Application.Current.MainPage.DisplayAlert(
                title,
                message,
                cancelLabel
                );
        }


        public IDisposable Confirm(ConfirmConfig config)
        {
            return Application.Current.MainPage.DisplayAlert(
                config.Title,
                config.CancelText,
                config.Message,
                config.OkText
            );
        }

        public async Task<bool> ConfirmAsync(
          string message,
          string title = null,
          string okText = null,
          string cancelText = null,
          CancellationToken? cancelToken = null)
        {
           return await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                okText,
                cancelText);
        }

        public async Task<bool> ConfirmAsync(ConfirmConfig config, CancellationToken? cancelToken = null)
        {
            return await Application.Current.MainPage.DisplayAlert(
                config.Title,
                config.CancelText,
                config.Message,
                config.OkText
                );
        }

        public IDisposable DatePrompt(DatePromptConfig config)
        {
            throw new NotImplementedException();
        }

        public async Task<DatePromptResult> DatePromptAsync(
          DatePromptConfig config,
          CancellationToken? cancelToken = null)
        {
            throw new NotImplementedException();
        }

        public async Task<DatePromptResult> DatePromptAsync(
          string title = null,
          DateTime? selectedDate = null,
          CancellationToken? cancelToken = null)
        {
            throw new NotImplementedException();
        }

        public IDisposable TimePrompt(TimePromptConfig config)
        {
            throw new NotImplementedException();
        }

        public async Task<TimePromptResult> TimePromptAsync(
          TimePromptConfig config,
          CancellationToken? cancelToken = null)
        {
            throw new NotImplementedException();
        }

        public async Task<TimePromptResult> TimePromptAsync(
          string title = null,
          TimeSpan? selectedTime = null,
          CancellationToken? cancelToken = null)
        {
            throw new NotImplementedException();
        }

        public IDisposable Prompt(PromptConfig config)
        {
            throw new NotImplementedException();
        }

        public async Task<PromptResult> PromptAsync(
          string message,
          string title = null,
          string okText = null,
          string cancelText = null,
          string placeholder = "",
          InputType inputType = InputType.Default,
          CancellationToken? cancelToken = null)
        {
            var res = await Application.Current.MainPage.DisplayPromptAsync(
                title,
                message,
                okText,
                cancelText);
            var promptResult = new PromptResult(true, res);
            return promptResult;
        }

        public async Task<PromptResult> PromptAsync(
          PromptConfig config,
          CancellationToken? cancelToken = null)
        {
            var res = await Application.Current.MainPage.DisplayPromptAsync(
                config.Title,
                config.CancelText,
                config.Message,
                config.OkText
            );
            var promptResult = new PromptResult(true, res);
            return promptResult;
        }

        public IDisposable Login(LoginConfig config)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResult> LoginAsync(
          string title = null,
          string message = null,
          CancellationToken? cancelToken = null)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResult> LoginAsync(LoginConfig config, CancellationToken? cancelToken = null)
        {
            throw new NotImplementedException();
        }

        public IProgressDialog Progress(ProgressDialogConfig config)
        {
            throw new NotImplementedException();
        }

        public IProgressDialog Loading(
          string title = null,
          Action onCancel = null,
          string cancelText = null,
          bool show = true,
          MaskType? maskType = null)
        {
            throw new NotImplementedException();
        }

        public IProgressDialog Progress(
          string title = null,
          Action onCancel = null,
          string cancelText = null,
          bool show = true,
          MaskType? maskType = null)
        {
            throw new NotImplementedException();
        }

        public void ShowLoading(string title = null, MaskType? maskType = null)
        {
            throw new NotImplementedException();
        }

        public void HideLoading()
        {
            throw new NotImplementedException();
        }

        public IDisposable Toast(string title, TimeSpan? dismissTimer = null)
        {
            throw new NotImplementedException();
        }

        public IDisposable Toast(ToastConfig cfg)
        {
            throw new NotImplementedException();
        }
    }
}
