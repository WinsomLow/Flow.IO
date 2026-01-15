namespace Flow.Core
{
  public class Connection(Guid id)
  {
    public Connection() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; } = id;
    public Node FromNode { get; set; } = new Node();
    public Node ToNode { get; set; } = new Node();
  }
}
