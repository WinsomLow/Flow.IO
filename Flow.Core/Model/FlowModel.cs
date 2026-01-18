namespace Flow.Core.Model
{
  public class FlowModel(Guid id)
  {
    public FlowModel() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; } = id;

    public string Content { get; set; } = string.Empty;
  }
}