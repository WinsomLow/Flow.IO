using Flow.Core;
using System.Reflection;

namespace Flow.Drafter.Common.Helper
{
  internal class PluginUtils
  {
    public static Dictionary<string, Type> LoadFlowPlugins(string pluginPath)
    {
      var controls = new Dictionary<string, Type>();

      foreach (Type type in LoadFlowControlTypes(pluginPath))
      {
        if (type.IsAbstract || !typeof(FlowControl).IsAssignableFrom(type))
        {
          continue;
        }

        var label = GetFlowTypeLabel(type);
        controls.Add(label, type);
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
        return Array.Empty<Type>();
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
  }
}
