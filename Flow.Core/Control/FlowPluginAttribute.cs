namespace Flow.Core.Control
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public sealed class FlowPluginAttribute : Attribute
  {
    public FlowPluginAttribute(string version)
    {
      Version = version;
    }

    public string Version { get; }
  }
}
