using Flow.Core;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Path = System.IO.Path;

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
    private readonly Dictionary<string, Type> _flowControlTypes = new(StringComparer.OrdinalIgnoreCase);

    public MainWindow()
    {
      InitializeComponent();
      LoadFlowPlugins();
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
      FlowControl? node = CreateFlowNode(label);
      if (node is null)
      {
        return;
      }

      Canvas.SetLeft(node, position.X - node.Width / 2);
      Canvas.SetTop(node, position.Y - node.Height / 2);
      DesignCanvas.Children.Add(node);
    }

    private FlowControl? CreateFlowNode(string label)
    {
      if (!_flowControlTypes.TryGetValue(label, out Type? flowType))
      {
        MessageBox.Show($"No FlowControl registered for '{label}'.",
          "Flow.IO",
          MessageBoxButton.OK,
          MessageBoxImage.Warning);
        return null;
      }

      if (Activator.CreateInstance(flowType) is not FlowControl node)
      {
        MessageBox.Show($"Failed to create FlowControl '{flowType.FullName}'.",
          "Flow.IO",
          MessageBoxButton.OK,
          MessageBoxImage.Warning);
        return null;
      }

      node.Width = 140;
      node.Height = 48;
      TrySetLabel(node, label);

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

    private void LoadFlowPlugins()
    {
      List<(string Label, Type Type)> controls = new();

      foreach (Type type in LoadFlowControlTypes())
      {
        if (type.IsAbstract || !typeof(FlowControl).IsAssignableFrom(type))
        {
          continue;
        }

        string label = GetFlowTypeLabel(type);
        if (string.IsNullOrWhiteSpace(label))
        {
          label = type.Name;
        }

        controls.Add((label, type));
      }

      if (controls.Count == 0)
      {
        return;
      }

      _flowControlTypes.Clear();
      FlowBlockList.Items.Clear();

      for (int i = 0; i < controls.Count; i++)
      {
        bool isLast = i == controls.Count - 1;
        (string label, Type type) = controls[i];
        _flowControlTypes[label] = type;
        FlowBlockList.Items.Add(CreateFlowBlockListItem(label, isLast));
      }
    }

    private static ListBoxItem CreateFlowBlockListItem(string label, bool isLast)
    {
      return new ListBoxItem
      {
        Content = label,
        Padding = new Thickness(8),
        Margin = isLast ? new Thickness(0) : new Thickness(0, 0, 0, 6)
      };
    }

    private static IEnumerable<Type> LoadFlowControlTypes()
    {
      string? dir = Directory.GetParent(AppContext.BaseDirectory)?.FullName;

      if (string.IsNullOrWhiteSpace(dir))
      {
        return Array.Empty<Type>();
      }

      string? path = Path.Combine(dir, "Flow.Component.dll");

      if (string.IsNullOrWhiteSpace(path))
      {
        return Array.Empty<Type>();
      }

      Assembly assembly;
      try
      {
        assembly = Assembly.LoadFrom(path);
      }
      catch
      {
        return Array.Empty<Type>();
      }

      try
      {
        return assembly.GetTypes();
      }
      catch (ReflectionTypeLoadException ex)
      {
        return ex.Types.Where(type => type is not null).Cast<Type>();
      }
    }

    private static string GetFlowTypeLabel(Type type)
    {
      if (Activator.CreateInstance(type) is not FlowControl instance)
      {
        return type.Name;
      }

      return instance.FlowType;
    }

    private static void TrySetLabel(FlowControl control, string label)
    {
      PropertyInfo? textProperty = control.GetType().GetProperty("Text", BindingFlags.Instance | BindingFlags.Public);
      if (textProperty?.CanWrite == true && textProperty.PropertyType == typeof(string))
      {
        textProperty.SetValue(control, label);
      }
    }
  }
}
