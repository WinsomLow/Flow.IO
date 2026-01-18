using System.Windows.Controls;

namespace Flow.Core.Control
{
  /// <summary>
  /// Interaction logic for OutNodeControl.xaml
  /// </summary>
  public partial class OutNodeControl : UserControl
  {
    public OutNodeControl()
    {
      InitializeComponent();
    }

    private void Ellipse_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      e.Handled = true;
    }
  }
}
