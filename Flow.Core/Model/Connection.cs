namespace Flow.Core.Model
{
  public class Connection(Guid id)
  {
    public Connection() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; set; } = id;
    public Node FromNode { get; set; } = new Node();
    public Node ToNode { get; set; } = new Node();
    public string Label { get; set; } = "";
  }
}
