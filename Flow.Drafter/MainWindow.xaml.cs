using Flow.Drafter.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Flow.Drafter
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private bool _isDraggingNode;
    private Point _dragStart;
    private Point _nodeStart;
    private FrameworkElement? _draggedNode;

    public MainWindow()
    {
      InitializeComponent();
    }

    private void FlowBlockList_OnPreviewMouseMove(object sender, MouseEventArgs e)
    {
      if (e.LeftButton != MouseButtonState.Pressed)
      {
        return;
      }

      if (FlowBlockList.SelectedItem is not ListBoxItem item)
      {
        return;
      }

      string label = item.Content?.ToString() ?? string.Empty;
      if (string.IsNullOrWhiteSpace(label))
      {
        return;
      }

      DragDrop.DoDragDrop(FlowBlockList, label, DragDropEffects.Copy);
    }

    private void DesignCanvas_OnDragOver(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.StringFormat))
      {
        e.Effects = DragDropEffects.Copy;
      }
      else
      {
        e.Effects = DragDropEffects.None;
      }

      e.Handled = true;
    }

    private void DesignCanvas_OnDrop(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.StringFormat))
      {
        return;
      }

      string label = e.Data.GetData(DataFormats.StringFormat) as string ?? string.Empty;
      if (string.IsNullOrWhiteSpace(label))
      {
        return;
      }

      Point position = e.GetPosition(DesignCanvas);
      RoundedButton node = CreateFlowNode(label);

      Canvas.SetLeft(node, position.X - node.Width / 2);
      Canvas.SetTop(node, position.Y - node.Height / 2);
      DesignCanvas.Children.Add(node);
    }

    private RoundedButton CreateFlowNode(string label)
    {
      RoundedButton node = new()
      {
        Width = 140,
        Height = 48,
        Text = label
      };

      node.AddHandler(UIElement.PreviewMouseLeftButtonDownEvent,
        new MouseButtonEventHandler(Node_OnMouseLeftButtonDown),
        true);
      node.AddHandler(UIElement.PreviewMouseMoveEvent,
        new MouseEventHandler(Node_OnMouseMove),
        true);
      node.AddHandler(UIElement.PreviewMouseLeftButtonUpEvent,
        new MouseButtonEventHandler(Node_OnMouseLeftButtonUp),
        true);
      node.Cursor = Cursors.SizeAll;

      return node;
    }

    private void Node_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (sender is not FrameworkElement node)
      {
        return;
      }

      _draggedNode = node;
      _isDraggingNode = true;
      _dragStart = e.GetPosition(DesignCanvas);
      _nodeStart = new Point(GetCanvasLeft(node), GetCanvasTop(node));
      node.CaptureMouse();
      Canvas.SetZIndex(node, 1);
      e.Handled = true;
    }

    private void Node_OnMouseMove(object sender, MouseEventArgs e)
    {
      if (!_isDraggingNode || _draggedNode is null)
      {
        return;
      }

      Point current = e.GetPosition(DesignCanvas);
      Vector delta = current - _dragStart;
      Canvas.SetLeft(_draggedNode, _nodeStart.X + delta.X);
      Canvas.SetTop(_draggedNode, _nodeStart.Y + delta.Y);
    }

    private void Node_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (_draggedNode is null)
      {
        return;
      }

      _draggedNode.ReleaseMouseCapture();
      Canvas.SetZIndex(_draggedNode, 0);
      _draggedNode = null;
      _isDraggingNode = false;
      e.Handled = true;
    }

    private static double GetCanvasLeft(FrameworkElement element)
    {
      double left = Canvas.GetLeft(element);
      return double.IsNaN(left) ? 0 : left;
    }

    private static double GetCanvasTop(FrameworkElement element)
    {
      double top = Canvas.GetTop(element);
      return double.IsNaN(top) ? 0 : top;
    }
  }
}
