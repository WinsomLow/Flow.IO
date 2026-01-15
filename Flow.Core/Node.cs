namespace Flow.Core
{
  public class Node(Guid id)
  {
    public Node() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; } = id;
    public Point2D Position { get; set; } = new Point2D();
  }
}
