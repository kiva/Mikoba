using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.Services;
using mikoba.UI.Controls;
using Xamarin.Forms;
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
        
        public void SwitchFocus(object sender, TextChangedEventArgs e)
        {
            var entry = sender as BorderlessEntry;
            var newText = e.NewTextValue;
            var oldText = e.OldTextValue;
            var entries = InputContainer.Children;
            if (IsDelete(newText))
            {
                FocusBack(entries, entry);
            }
            else
            {
                FocusNext(entries, entry);
            }
        }

        private void FocusBack(Grid.IGridList<View> entries, BorderlessEntry e)
        {
            int index = entries.IndexOf(e);
            if (index > -1 && (index - 1) >= 0)
            {
                var prev = entries.ElementAt(index - 1);
                prev?.Focus();
            }
        }

        private void FocusNext(Grid.IGridList<View> entries, BorderlessEntry e)
        {
            int index = entries.IndexOf(e);
            if (index > -1 && (index + 1) <= entries.Count)
            {
                var next = entries.ElementAt(index + 1);
                next?.Focus();
            }
        }
        
        private bool IsDelete(string neu)
        {
            if (string.IsNullOrEmpty(neu))
            {
                return true;
            }
            return false;
        }

        public void SubmitPIN(object sender, TextChangedEventArgs e)
        {
            if (IsDelete(e.NewTextValue))
            {
                var entry = sender as BorderlessEntry;
                FocusBack(InputContainer.Children, entry);
            }
            else if (FinishCommand != null)
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
