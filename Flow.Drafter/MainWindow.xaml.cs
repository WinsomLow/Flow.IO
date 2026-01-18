using Flow.Core.Common;
using Flow.Core.Control;
using Flow.Drafter.Common.Util;
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
    private Dictionary<string, FlowControl> m_flowControlCollectors = new(StringComparer.Ordinal);
    private readonly Editor m_editor;

    public MainWindow()
    {
      InitializeComponent();
      m_editor = new Editor(m_DesignCanvas);
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

      m_flowControlCollectors = PluginUtils.LoadFlowPlugins(path);
      m_FlowBlockList.Items.Clear();

      var itemLeft = m_flowControlCollectors.Count;
      foreach (string label in m_flowControlCollectors.Keys)
      {
        bool isLast = itemLeft-- == 1;
        m_FlowBlockList.Items.Add(CreateFlowBlockListItem(label, isLast));
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

      if (m_FlowBlockList.SelectedItem is not ListBoxItem item)
      {
        return;
      }

      string label = item.Content?.ToString() ?? string.Empty;
      if (string.IsNullOrWhiteSpace(label))
      {
        return;
      }

      DragDrop.DoDragDrop(m_FlowBlockList, label, DragDropEffects.Copy);
    }
    #endregion

    private void DesignCanvas_OnDragOver(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(typeof(string)))
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
      if (!e.Data.GetDataPresent(typeof(string)))
      {
        return;
      }

      string label = e.Data.GetData(typeof(string)) as string ?? string.Empty;
      if (string.IsNullOrWhiteSpace(label))
      {
        return;
      }

      if (!m_flowControlCollectors.TryGetValue(label, out FlowControl? flowControl))
      {
        return;
      }

      var canvasFlowControl = flowControl.CreateInstanceOnCanvas(m_editor);

      if (canvasFlowControl is not UIElement element)
      {
        return;
      }

      Point dropPosition = e.GetPosition(m_DesignCanvas);
      Canvas.SetLeft(element, dropPosition.X);
      Canvas.SetTop(element, dropPosition.Y);
      m_DesignCanvas.Children.Add(element);
      e.Handled = true;
    }
  }
}
