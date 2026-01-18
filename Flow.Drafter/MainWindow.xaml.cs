using Flow.Core.Common;
using Flow.Core.Control;
using Flow.Core.Model;
using Flow.Core.ViewModel;
using Flow.Drafter.Common.Util;
using Microsoft.Win32;
using Newtonsoft.Json;
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
    private static readonly JsonSerializerSettings s_jsonSettings = new()
    {
      Formatting = Formatting.Indented
    };

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
      canvasFlowControl.ViewModel.Model.Position.X = dropPosition.X;
      canvasFlowControl.ViewModel.Model.Position.Y = dropPosition.Y;
      m_DesignCanvas.Children.Add(element);
      e.Handled = true;
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
      var dialog = new SaveFileDialog
      {
        Filter = "Flow files (*.json)|*.json|All files (*.*)|*.*",
        DefaultExt = "json"
      };

      if (dialog.ShowDialog(this) != true)
      {
        return;
      }

      var document = BuildDocument();
      string json = JsonConvert.SerializeObject(document, s_jsonSettings);
      File.WriteAllText(dialog.FileName, json);
    }

    private FlowDocument BuildDocument()
    {
      var document = new FlowDocument();
      var blocks = m_DesignCanvas.Children.OfType<FlowControl>().ToList();

      for (int i = 0; i < blocks.Count; i++)
      {
        FlowControl block = blocks[i];
        double left = Canvas.GetLeft(block);
        double top = Canvas.GetTop(block);

        if (double.IsNaN(left))
        {
          left = 0;
        }

        if (double.IsNaN(top))
        {
          top = 0;
        }

        block.ViewModel.Model.FlowType = block.FlowType;
        block.ViewModel.Model.Position.X = left;
        block.ViewModel.Model.Position.Y = top;
        document.Blocks.Add(block.ViewModel.Model);
      }

      foreach (var connection in m_editor.Connections)
      {
        document.Connections.Add(connection.Model);
      }

      return document;
    }

    private void LoadButton_OnClick(object sender, RoutedEventArgs e)
    {
      var dialog = new OpenFileDialog
      {
        Filter = "Flow files (*.json)|*.json|All files (*.*)|*.*",
        DefaultExt = "json"
      };

      if (dialog.ShowDialog(this) != true)
      {
        return;
      }

      string json = File.ReadAllText(dialog.FileName);
      var document = JsonConvert.DeserializeObject<FlowDocument>(json);
      if (document is null)
      {
        return;
      }

      LoadDocument(document);
    }

    private void LoadDocument(FlowDocument document)
    {
      m_editor.Connections.Clear();
      m_DesignCanvas.Children.Clear();

      var nodeLookup = new Dictionary<Guid, NodeViewModel>();

      foreach (var block in document.Blocks)
      {
        if (!m_flowControlCollectors.TryGetValue(block.FlowType, out FlowControl? prototype))
        {
          continue;
        }

        FlowControl instance = prototype.CreateInstanceOnCanvas(m_editor, block);
        if (instance is not UIElement element)
        {
          continue;
        }

        Canvas.SetLeft(element, block.Position.X);
        Canvas.SetTop(element, block.Position.Y);
        m_DesignCanvas.Children.Add(element);
        instance.RegisterNodes(nodeLookup);
      }

      m_DesignCanvas.UpdateLayout();

      foreach (var connection in document.Connections)
      {
        if (!nodeLookup.TryGetValue(connection.FromNode.Id, out var fromNode))
        {
          continue;
        }

        if (!nodeLookup.TryGetValue(connection.ToNode.Id, out var toNode))
        {
          continue;
        }

        m_editor.CreateConnection(fromNode, toNode, connection.Label);
      }
    }
  }
}
