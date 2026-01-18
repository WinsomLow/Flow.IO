using Flow.Core.Common;
using Flow.Core.Model;
using Flow.Core.ViewModel;
using System.Reflection;
using System.Windows.Controls;

namespace Flow.Core.Control
{
  public abstract class FlowControl : UserControl
  {
    public const string ApiVersionString = "2.0.0";
    public static Version ApiVersion { get; } = Version.Parse(ApiVersionString);

    public abstract FlowViewModel ViewModel { get; }
    public abstract string FlowType { get; }
    public abstract FlowControl CreateInstanceOnCanvas(Editor editor);

    public virtual string PluginVersion
    {
      get
      {
        var attribute = GetType().GetCustomAttribute<FlowPluginAttribute>();
        return attribute?.Version ?? string.Empty;
      }
    }

    public virtual FlowControl CreateInstanceOnCanvas(Editor editor, FlowModel model)
    {
      return CreateInstanceOnCanvas(editor);
    }

    public virtual void RegisterNodes(Dictionary<Guid, NodeViewModel> nodeLookup)
    {
    }
  }
}
