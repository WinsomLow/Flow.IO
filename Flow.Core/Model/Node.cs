using Flow.Core.Common;

namespace Flow.Core.Model
{
  public class Node(Guid id)
  {
    public Node() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; set; } = id;
    public Point2D Position { get; set; } = new Point2D();
  }
}
