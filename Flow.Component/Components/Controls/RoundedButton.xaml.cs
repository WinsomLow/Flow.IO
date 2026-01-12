using System.Windows;
using System.Windows.Controls;

namespace Flow.Component.Controls
{
    public partial class RoundedButton : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(RoundedButton),
                new PropertyMetadata(string.Empty));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public RoundedButton()
        {
            InitializeComponent();
        }
    }
}
