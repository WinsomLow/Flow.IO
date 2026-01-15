using Flow.Core;
using System.Windows;
using System.Windows.Input;

namespace Flow.Component.Controls
{
  public partial class Process : FlowControl
  {
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(Process),
            new PropertyMetadata(string.Empty));

    public override string FlowType
    {
      get
      {
        return "Process";
      }
    }

    public string Text
    {
      get => (string)GetValue(TextProperty);
      set => SetValue(TextProperty, value);
    }

    public Process()
    {
      InitializeComponent();
    }

    private void ProcessTextBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      ProcessTextBox.Focus();
      ProcessTextBox.SelectAll();
    }

    private void Connector_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      e.Handled = true;
    }
  }
}
