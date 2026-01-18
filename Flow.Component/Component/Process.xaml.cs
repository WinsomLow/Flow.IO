using Flow.Core.Common;
using Flow.Core.Control;
using Flow.Core.Model;
using Flow.Core.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
      var inNode = new NodeViewModel(new Node());
      var outNode = new NodeViewModel(new Node());
      var flowModel = new FlowModel();
      ViewModel = new ProcessViewModel(inNode, outNode, flowModel);
    }

    public Process(ProcessViewModel viewModel, Editor editor) : this()
    {
      InitializeComponent();
      ViewModel = viewModel;
      m_editor = editor;
      DataContext = ViewModel;
      m_InNode.Editor = editor;
      m_OutNode.Editor = editor;
      m_InNode.NodeViewModel = viewModel.InNode;
      m_OutNode.NodeViewModel = viewModel.OutNode;
      Loaded += Process_OnLoaded;
    }

    public override string FlowType
    {
      get
      {
        return "Process";
      }
    }

    public override ProcessViewModel ViewModel { get; }

    public override FlowControl CreateInstanceOnCanvas(Editor editor)
    {
      var inNode = new NodeViewModel(new Node());
      var outNode = new NodeViewModel(new Node());
      var flowModel = new FlowModel();
      var flowViewModel = new ProcessViewModel(inNode, outNode, flowModel);
      return new Process(flowViewModel, editor);
    }

    private void Process_OnLoaded(object sender, RoutedEventArgs e)
    {
      UpdateNodePositions();
    }

    private void Process_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (m_editor is not Editor editor ||
        sender is not UIElement)
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

    private void Process_PreviewMouseMove(object sender, MouseEventArgs e)
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
      UpdateNodePositions();
    }

    private void Process_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

    private void UpdateNodePositions()
    {
      if (m_editor is not Editor editor)
      {
        return;
      }

      if (m_InNode is null || m_OutNode is null)
      {
        return;
      }

      GeneralTransform inTransform = m_InNode.TransformToAncestor(editor.DesignCanvas);
      Point inCenter = inTransform.Transform(new Point(m_InNode.ActualWidth / 2, m_InNode.ActualHeight / 2));
      ViewModel.InNode.X = inCenter.X;
      ViewModel.InNode.Y = inCenter.Y;

      GeneralTransform outTransform = m_OutNode.TransformToAncestor(editor.DesignCanvas);
      Point outCenter = outTransform.Transform(new Point(m_OutNode.ActualWidth / 2, m_OutNode.ActualHeight / 2));
      ViewModel.OutNode.X = outCenter.X;
      ViewModel.OutNode.Y = outCenter.Y;
    }
  }
}
