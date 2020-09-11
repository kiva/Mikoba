using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using mikoba.UI.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PINInput : ContentView
    {
        public static readonly BindableProperty FinishInputProperty =
            BindableProperty.Create("OnFinish", typeof(ICommand), typeof(PINInput), default(string));
        
        public ICommand OnFinish
        {
            get { return (ICommand)GetValue(FinishInputProperty); }
            set { SetValue(FinishInputProperty, value); }
        }

        public void FocusNext(object sender, EventArgs e)
        {
            var entry = sender as BorderlessEntry;
            var entries = InputContainer.Children;
            var index = entries.IndexOf(entry);
            if (index > -1)
            {
                var nextIndex = (index + 1) >= entries.Count ? 0 : index + 1;
                var next = entries.ElementAt(nextIndex);
                next?.Focus();
            }
        }

        public PINInput()
        {
            InitializeComponent();
        }

        public void OnFocused(object sender, EventArgs e)
        {
            Console.WriteLine("Aloha");
            Initial.Focus();
        }
    }
}