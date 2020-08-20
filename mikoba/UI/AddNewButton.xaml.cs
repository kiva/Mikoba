using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;

namespace mikoba.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewButton : ContentView
    {

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(AddNewButton), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(AddNewButton), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public AddNewButton()
        {
            InitializeComponent();
        }

        void ButtonClicked(System.Object sender, System.EventArgs e)
        {
            if (Command != null)
            {
                if (Command.CanExecute(CommandParameter))
                    Command.Execute(CommandParameter);
            }
        }
    }
}