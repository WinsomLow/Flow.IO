using Flow.Core.Common;
using Flow.Core.Model;
using Flow.Core.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

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

      var connection = new Connection
      {
        FromNode = info.FromNode.Model,
        ToNode = toNode.Model
      };

      var connectionViewModel = new ConnectionViewModel(connection);
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
      editor.DesignCanvas.Children.Add(path);
      editor.DesignCanvas.Children.Add(labelBorder);

      void UpdatePath(object? _, System.ComponentModel.PropertyChangedEventArgs __)
      {
        connectionViewModel.Rebuild();
        path.Data = connectionViewModel.Path;
      }

      info.FromNode.PropertyChanged += UpdatePath;
      toNode.PropertyChanged += UpdatePath;

      editor.DragMode = DragMode.None;
      editor.DrawConnectionInfo = null;

      e.Handled = true;
    }
  }
}
