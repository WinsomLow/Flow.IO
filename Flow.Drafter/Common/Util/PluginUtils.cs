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

        if (!IsCompatiblePluginVersion(type))
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

    private static bool IsCompatiblePluginVersion(Type type)
    {
      var attribute = type.GetCustomAttribute<FlowPluginAttribute>();
      if (attribute is null)
      {
        return false;
      }

      if (!Version.TryParse(attribute.Version, out var pluginVersion))
      {
        return false;
      }

      var coreVersion = FlowControl.ApiVersion;

      if (pluginVersion.Major != coreVersion.Major ||
        pluginVersion.Minor != coreVersion.Minor)
      {
        return false;
      }

      if (pluginVersion.Build != -1 &&
        pluginVersion.Build != coreVersion.Build)
      {
        return false;
      }

      if (pluginVersion.Revision != -1 &&
        pluginVersion.Revision != coreVersion.Revision)
      {
        return false;
      }

      return true;
    }
  }
}
