using Flow.Core.Control;
using Flow.Core.Model;
using Flow.Core.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Flow.Core.Common
{
  // TODO: Use OOP state design pattern instead
  public enum DragMode
  {
    None = 0,
    MoveBlock,
    DrawConnection,
  }

  public class MoveBlockInfo
  {
    public required FlowControl ActiveControl { get; set; }

    public Point Offset { get; set; }
  }

  public class DrawConnectionInfo
  {
    public required NodeViewModel FromNode { get; set; }
  }

  public class Editor(Canvas designCanvas)
  {
    public Canvas DesignCanvas { get; } = designCanvas;

    public DragMode DragMode { get; set; } = DragMode.None;

    public MoveBlockInfo? MoveBlockInfo { get; set; } = null;
    public DrawConnectionInfo? DrawConnectionInfo { get; set; } = null;

    public List<ConnectionViewModel> Connections { get; } = new();

    public ConnectionViewModel CreateConnection(NodeViewModel fromNode, NodeViewModel toNode, string? label = null)
    {
      var connection = new Connection
      {
        FromNode = fromNode.Model,
        ToNode = toNode.Model,
        Label = label ?? string.Empty
      };
      var connectionViewModel = new ConnectionViewModel(connection);
      Connections.Add(connectionViewModel);

      var path = new Path
      {
        Stroke = Brushes.White,
        StrokeThickness = 2,
        Data = connectionViewModel.Path,
        IsHitTestVisible = false
      };

      var labelBorder = new Border
      {
        Background = Brushes.Transparent,
        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999")),
        BorderThickness = new Thickness(1),
        CornerRadius = new CornerRadius(6),
        Padding = new Thickness(8, 3, 8, 3),
        DataContext = connectionViewModel
      };

      var labelBox = new TextBox
      {
        BorderThickness = new Thickness(0),
        BorderBrush = Brushes.Transparent,
        Background = Brushes.Transparent,
        FontSize = 13,
        AcceptsReturn = false,
        Foreground = Brushes.White
      };

      labelBox.SetBinding(TextBox.TextProperty, new Binding(nameof(ConnectionViewModel.Label))
      {
        Mode = BindingMode.TwoWay,
        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
      });

      labelBorder.Child = labelBox;
      labelBorder.SetBinding(Canvas.LeftProperty, new Binding(nameof(ConnectionViewModel.MidPointX)));
      labelBorder.SetBinding(Canvas.TopProperty, new Binding(nameof(ConnectionViewModel.MidPointY)));

      Panel.SetZIndex(path, -1);
      DesignCanvas.Children.Add(path);
      DesignCanvas.Children.Add(labelBorder);

      void UpdatePath(object? _, System.ComponentModel.PropertyChangedEventArgs __)
      {
        connectionViewModel.Rebuild();
        path.Data = connectionViewModel.Path;
      }

      fromNode.PropertyChanged += UpdatePath;
      toNode.PropertyChanged += UpdatePath;

      return connectionViewModel;
    }
  }
}
