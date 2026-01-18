namespace Flow.Core.Model
{
  public class FlowDocument
  {
    public List<FlowModel> Blocks { get; set; } = new();
    public List<Connection> Connections { get; set; } = new();
  }
}
