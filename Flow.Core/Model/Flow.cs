namespace Flow.Core.Model
{
  public class Flow(Guid id)
  {
    public Flow() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; } = id;

    public string Content { get; set; } = string.Empty;
  }
}