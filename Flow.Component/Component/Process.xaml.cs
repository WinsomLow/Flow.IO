using Flow.Core.Common;
using Flow.Core.Control;
using Flow.Core.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Flow.Component.Component
{
  /// <summary>
  /// Interaction logic for Process.xaml
  /// </summary>
  public partial class Process : FlowControl
  {
    private readonly Editor? m_editor = null;
    public Process()
    {
      ViewModel = new FlowViewModel();
    }

    public Process(FlowViewModel viewModel, Editor editor) : this()
    {
      InitializeComponent();
      ViewModel = viewModel;
      m_editor = editor;
    }

    public override string FlowType
    {
      get
      {
        return "Process";
      }
    }

    protected override FlowViewModel ViewModel { get; }

    public override FlowControl CreateInstanceOnCanvas(Editor editor)
    {
      var flowViewModel = new FlowViewModel();
      return new Process(flowViewModel, editor);
    }

    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if(m_editor is not Editor editor ||
        sender is not UIElement element)
      {
        return;
      }

      editor.DragMode = DragMode.MoveBlock;
      Point mousePos = e.GetPosition(m_editor.DesignCanvas);
      double left = Canvas.GetLeft(this);
      double top = Canvas.GetTop(this);

      editor.MoveBlockInfo = new MoveBlockInfo()
      {
        ActiveControl = this,
        Offset = new Point(mousePos.X - left, mousePos.Y - top)
      };

      CaptureMouse();
      e.Handled = true;
    }

    private void Border_PreviewMouseMove(object sender, MouseEventArgs e)
    {
      if (m_editor is not Editor editor || 
        e.LeftButton != MouseButtonState.Pressed ||
        editor.DragMode != DragMode.MoveBlock ||
        editor.MoveBlockInfo is not MoveBlockInfo moveBlockInfo)
      {
        return;
      }

      Point mousePos = e.GetPosition(m_editor.DesignCanvas);

      mousePos.X = mousePos.X < 0 ? 0 : mousePos.X;
      mousePos.Y = mousePos.Y < 0 ? 0 : mousePos.Y;

      var x = mousePos.X - moveBlockInfo.Offset.X; 
      var y = mousePos.Y - moveBlockInfo.Offset.Y;

      Canvas.SetLeft(moveBlockInfo.ActiveControl, x);
      Canvas.SetTop(moveBlockInfo.ActiveControl, y);
    }

    private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (m_editor is not Editor editor ||
         editor.DragMode != DragMode.MoveBlock ||
         editor.MoveBlockInfo is not MoveBlockInfo moveBlockInfo)
      {
        return;
      }

      moveBlockInfo.ActiveControl.ReleaseMouseCapture();
      editor.DragMode = DragMode.None;
      editor.MoveBlockInfo = null;
      e.Handled = true;
    }
  }
}
