using Flow.Core.Common;

namespace Flow.Core.Model
{
  public class FlowModel(Guid id)
  {
    public FlowModel() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; set; } = id;

    public string Content { get; set; } = string.Empty;
    public string FlowType { get; set; } = string.Empty;
    public Point2D Position { get; set; } = new Point2D();
    public Node InNode { get; set; } = new Node();
    public Node OutNode { get; set; } = new Node();
  }
}
