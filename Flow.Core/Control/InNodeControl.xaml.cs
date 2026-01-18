using Flow.Core.Common;
using Flow.Core.ViewModel;
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

    public Editor? Editor { get; set; }
    public NodeViewModel? NodeViewModel { get; set; }

    private void Ellipse_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (Editor is not Editor editor ||
        NodeViewModel is not NodeViewModel toNode ||
        editor.DragMode != DragMode.DrawConnection ||
        editor.DrawConnectionInfo is not DrawConnectionInfo info)
      {
        return;
      }

      editor.CreateConnection(info.FromNode, toNode);

      editor.DragMode = DragMode.None;
      editor.DrawConnectionInfo = null;

      e.Handled = true;
    }
  }
}
