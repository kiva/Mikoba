using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hyperledger.Aries.Extensions;
using mikoba.Annotations;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.UI.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PINInput : ContentView
    {
        public static readonly BindableProperty FinishCommandProperty =
            BindableProperty.Create("FinishCommand", typeof(ICommand), typeof(PINInput), null);

        public ICommand FinishCommand
        {
            get { return (ICommand) GetValue(FinishCommandProperty); }
            set { SetValue(FinishCommandProperty, value); }
        }

        public static readonly BindableProperty FinishCommandParameterProperty =
            BindableProperty.Create("FinishCommandParameter", typeof(object), typeof(PINInput), null);

        public object FinishCommandParameter
        {
            get { return GetValue(FinishCommandParameterProperty); }
            set { SetValue(FinishCommandParameterProperty, value); }
        }
        
        public static readonly BindableProperty FocusDelayProperty = 
            BindableProperty.Create("FocusDelay", typeof(object), typeof(PINInput), null);

        public object FocusDelay
        {
            get { return GetValue(FocusDelayProperty); }
            set { SetValue(FocusDelayProperty, value); }
        }

        public BackButtonEventArgs MostRecentBackPress { get; set; }

        public void SwitchFocus(object sender, TextChangedEventArgs e)
        {
            var entry = sender as BorderlessEntry;
            var newText = e.NewTextValue;
            var oldText = e.OldTextValue;
            bool isNewEmpty = String.IsNullOrEmpty(newText);
            bool isOldEmpty = String.IsNullOrEmpty(oldText);
            var entries = InputContainer.Children;
            if (!isOldEmpty && !isNewEmpty)
            {
                entry.Text = oldText;
                FocusNext(entries, entry, newText);
            }
            else if (!isNewEmpty)
            {
                FocusNext(entries, entry);
            }
        }

        private void FocusBack(Grid.IGridList<View> entries, BorderlessEntry e, bool deleteValue = false)
        {
            MostRecentBackPress = null;
            int index = entries.IndexOf(e);
            if (index > -1 && (index - 1) >= 0)
            {
                var prev = (BorderlessEntry) entries.ElementAt(index - 1);
                var input = prev.Text;
                prev?.Focus();
                if (deleteValue)
                {
                    prev.Text = "";
                    MostRecentBackPress = new BackButtonEventArgs("", input);
                }
            }
        }

        private void FocusNext(Grid.IGridList<View> entries, BorderlessEntry e, string input = "")
        {
            int index = entries.IndexOf(e);
            if (index > -1 && (index + 1) <= entries.Count)
            {
                var next = (BorderlessEntry) entries.ElementAt(index + 1);
                next?.Focus();
                next.Text = input;
            }
        }
        

        public void SubmitPIN(object sender, TextChangedEventArgs e)
        {
            if (FinishCommand != null && !String.IsNullOrEmpty(e.NewTextValue))
            {
                if (FinishCommand.CanExecute(FinishCommandParameter))
                {
                    FinishCommand.Execute(FinishCommandParameter);
                }
            }
        }

        public void SubmitWithReturn([CanBeNull] object sender, EventArgs e)
        {
            if (FinishCommand != null)
            {
                if (FinishCommand.CanExecute(FinishCommandParameter))
                {
                    FinishCommand.Execute(FinishCommandParameter);
                }
            }
        }

        public PINInput()
        {
            InitializeComponent();
        }

        public void HandleDelete(object sender, BackButtonEventArgs e)
        {
            var entry = sender as BorderlessEntry;
            var entries = InputContainer.Children;
            if (String.IsNullOrEmpty(e.OldValue) || (MostRecentBackPress != null && MostRecentBackPress.OldValue == e.OldValue))
            {
                FocusBack(entries, entry, true);
            }
            else
            {
                MostRecentBackPress = e;
            }
        }

        protected override async void OnParentSet()
        {
            base.OnParentSet();

            int delay = 300;
            if (FocusDelay != null)
            {
                string delayValue = FocusDelay.ToString();
                delay = int.Parse(delayValue);
            }
            
            await Task.Delay(delay);
            Initial.Focus();
        }
    }
}
