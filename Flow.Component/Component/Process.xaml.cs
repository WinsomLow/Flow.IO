using Flow.Core.Control;

namespace Flow.Component.Component
{
  /// <summary>
  /// Interaction logic for Process.xaml
  /// </summary>
  public partial class Process : FlowControl
  {
    public Process()
    {
      InitializeComponent();
    }

    public override string FlowType
    {
      get
      {
        return "Process";
      }
    }
  }
}
