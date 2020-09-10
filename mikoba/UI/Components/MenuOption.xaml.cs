using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;
using SVG.Forms.Plugin.Abstractions;

namespace mikoba.UI.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuOption : ContentView
    {
        public static readonly BindableProperty MenuOptionTextProperty =
            BindableProperty.Create("MenuOptionText", typeof(string), typeof(MenuOption), default(string));
        public string MenuOptionText
        {
            get { return (string)GetValue(MenuOptionTextProperty); }
            set { SetValue(MenuOptionTextProperty, value); }
        }
        
        public static readonly BindableProperty LeftSvgImageProperty =
            BindableProperty.Create("LeftSvgImage", typeof(string), typeof(MenuOption), default(string));
        public string LeftSvgImage
        {
            get { return (string)GetValue(LeftSvgImageProperty); }
            set { SetValue(LeftSvgImageProperty, value); }
        }
        
        public static readonly BindableProperty RightSvgImageProperty =
            BindableProperty.Create("RightSvgImage", typeof(string), typeof(MenuOption), default(string));
        public string RightSvgImage
        {
            get { return (string)GetValue(RightSvgImageProperty); }
            set { SetValue(RightSvgImageProperty, value); }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(MenuOption), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(MenuOption), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public MenuOption()
        {
            InitializeComponent();
            menuOptionTextLabel.SetBinding(Label.TextProperty, new Binding("MenuOptionText", source: this));
            leftSvgImage.SetBinding(SvgImage.SvgPathProperty, new Binding("LeftSvgImage", source: this));
            rightSvgImage.SetBinding(SvgImage.SvgPathProperty, new Binding("RightSvgImage", source: this));
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
