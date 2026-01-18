using System.Windows.Controls;

namespace Flow.Core.Control
{
  /// <summary>
  /// Interaction logic for InNode.xaml
  /// </summary>
  public partial class InNodeControl : UserControl
  {
    public InNodeControl()
    {
      InitializeComponent();
    }

    private void Ellipse_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      e.Handled = true;
    }
  }
}
