using Flow.Core.Common;
using Flow.Core.ViewModel;
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

    public Editor? Editor { get; set; }
    public NodeViewModel? NodeViewModel { get; set; }

    private void Ellipse_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (Editor is not Editor editor || NodeViewModel is not NodeViewModel fromNode)
      {
        return;
      }

      editor.DragMode = DragMode.DrawConnection;
      editor.DrawConnectionInfo = new DrawConnectionInfo
      {
        FromNode = fromNode
      };

      e.Handled = true;
    }
  }
}
