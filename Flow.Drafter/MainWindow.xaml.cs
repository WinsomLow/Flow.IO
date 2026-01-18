using Flow.Drafter.Common.Helper;
using System.IO;
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
    public MainWindow()
    {
      InitializeComponent();
      DataContext = new MainWindowViewModel();
      LoadFlowPlugins();
    }

    private void LoadFlowPlugins()
    {
      string? dir = Directory.GetParent(AppContext.BaseDirectory)?.FullName;

      if (string.IsNullOrWhiteSpace(dir))
      {
        return;
      }

      string? path = Path.Combine(dir, "Flow.Component.dll");

      var flowControlTypes = PluginUtils.LoadFlowPlugins(path);
      FlowBlockList.Items.Clear();

      var itemLeft = flowControlTypes.Count;
      foreach ((string label, Type type) in flowControlTypes)
      {
        bool isLast = itemLeft-- == 1;
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

    #region Event
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
    #endregion

    private void Ellipse_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var a = 123;
    }

    private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var a = 123;
    }
  }
}
