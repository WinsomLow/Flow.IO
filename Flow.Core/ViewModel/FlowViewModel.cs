using Flow.Core.Common;
using Flow.Core.Model;

namespace Flow.Core.ViewModel
{
  public class FlowViewModel(FlowModel model) : NotifyObject
  {
    private FlowModel m_model = model;

    public string Content { get
      {
        return m_model.Content;
      } set
      {
        m_model.Content = value;
      }
    }
  }
}
