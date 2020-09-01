using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;
using SVG.Forms.Plugin.Abstractions;

namespace mikoba.UI.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActionButton : ContentView
    {
        public static readonly BindableProperty ActionButtonTextProperty =
            BindableProperty.Create("ActionButtonText", typeof(string), typeof(ActionButton), default(string));
        public string ActionButtonText
        {
            get { return (string)GetValue(ActionButtonTextProperty); }
            set { SetValue(ActionButtonTextProperty, value); }
        }
        
        public static readonly BindableProperty ActionButtonSvgProperty =
            BindableProperty.Create("ActionButtonSvg", typeof(string), typeof(ActionButton), default(string));
        public string ActionButtonSvg
        {
            get { return (string)GetValue(ActionButtonSvgProperty); }
            set { SetValue(ActionButtonSvgProperty, value); }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ActionButton), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(ActionButton), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public ActionButton()
        {
            InitializeComponent();
            ActionButtonTextLabel.SetBinding(Label.TextProperty, new Binding("ActionButtonText", source: this));
            actionButtonSvg.SetBinding(SvgImage.SvgPathProperty, new Binding("ActionButtonSvg", source: this));
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
