using Flow.Core;
using System.Windows;

namespace Flow.Component.Controls
{
    public partial class RoundedButton : FlowControl
  {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(RoundedButton),
                new PropertyMetadata(string.Empty));

    public override string FlowType
    {
      get
      {
        return "Rounded Button";
      }
    }

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
