using Flow.Core.Common;
using Flow.Core.ViewModel;
using System.Windows.Controls;

namespace Flow.Core.Control
{
  public abstract class FlowControl : UserControl
  {
    protected abstract FlowViewModel ViewModel { get; }
    public abstract string FlowType { get; }
    public abstract FlowControl CreateInstanceOnCanvas(Editor editor);
  }
}
