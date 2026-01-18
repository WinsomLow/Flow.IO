using Flow.Core.Model;

namespace Flow.Core.ViewModel
{
  public class ProcessViewModel : FlowViewModel
  {
    public ProcessViewModel(FlowModel model) : base(model)
    {
      InNode = new NodeViewModel(model.InNode);
      OutNode = new NodeViewModel(model.OutNode);
    }

    public NodeViewModel InNode { get; }
    public NodeViewModel OutNode { get; }
  }
}
