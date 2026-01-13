using System.Windows.Controls;

namespace Flow.Core
{
  public abstract class FlowControl: UserControl
  {
    public abstract string FlowType { get; }
  }
}
