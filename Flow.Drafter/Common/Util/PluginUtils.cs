using Flow.Core.Control;
using System.Reflection;

namespace Flow.Drafter.Common.Util
{
  internal class PluginUtils
  {
    public static Dictionary<string, FlowControl> LoadFlowPlugins(string pluginPath)
    {
      var controls = new Dictionary<string, FlowControl>();

      foreach (Type type in LoadFlowControlTypes(pluginPath))
      {
        if (type.IsAbstract || !typeof(FlowControl).IsAssignableFrom(type))
        {
          continue;
        }

        if (GetFlowTypeLabel(type, out var label, out var flowControl) &&
          flowControl is not null)
        {
          controls.Add(label, flowControl);
        }
      }

      return controls;
    }

    private static IEnumerable<Type> LoadFlowControlTypes(string pluginPath)
    {
      try
      {
        var assembly = Assembly.LoadFrom(pluginPath);
        return assembly.GetTypes();
      }
      catch (ReflectionTypeLoadException ex)
      {
        return ex.Types.Where(type => type is not null).Cast<Type>();
      }
      catch
      {
        return [];
      }
    }

    private static bool GetFlowTypeLabel(Type type, out string label, out FlowControl? flowControl)
    {
      label = string.Empty;
      flowControl = null;
      if (Activator.CreateInstance(type) is not FlowControl instance)
      {
        return false;
      }

      label = instance.FlowType;
      flowControl = instance;
      return true;
    }
  }
}
