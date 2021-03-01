using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BackButton : ContentView
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(BackButton), null);

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(BackButton), null);

        public object CommandParameter
        {
            get { return (object) GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public BackButton()
        {
            InitializeComponent();
            SvgImage.Command = new Command(() =>
            {
                if (this.Command != null)
                {
                    this.Command.Execute(null);
                }
            });
        }
    }
}
