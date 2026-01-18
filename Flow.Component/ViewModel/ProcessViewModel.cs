using Flow.Core.Common;
using Flow.Core.Model;

namespace Flow.Core.ViewModel
{
  public class ProcessViewModel : FlowViewModel
  {
    public ProcessViewModel(NodeViewModel inNode, NodeViewModel outNode, FlowModel model): 
      base(model)
    {
      InNode = inNode;
      OutNode = outNode;
    }

    public NodeViewModel InNode { get; }
    public NodeViewModel OutNode { get; }
  }
}
