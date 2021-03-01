
using System.Reflection;
using System.Windows.Input;
using SVG.Forms.Plugin.Abstractions;
using Xamarin.Forms;

namespace mikoba.UI.Components
{
    public class MikobaSvgImage : SvgImage
    {
        private string _imageKey;
        
        public MikobaSvgImage()
        {
            this.SvgAssembly = typeof(App).GetTypeInfo().Assembly;
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                if (this.Command != null && this.Command.CanExecute(this))
                {
                    this.Command.Execute(this);
                }
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public string ImageKey
        {
            set
            {
                _imageKey = value;
                this.SvgPath = "mikoba.Images." + value + ".svg";
            }
            get
            {
                return _imageKey;
            }
        }
        
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ActionButton), null);

        public ICommand Command
        {
            get
            {
                var command = (ICommand)GetValue(CommandProperty);
                return command;
            }
            set { SetValue(CommandProperty, value); }
        }
        
    }
}
