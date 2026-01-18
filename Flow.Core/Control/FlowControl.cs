using Flow.Core.Common;
using Flow.Core.Model;
using Flow.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Flow.Core.Control
{
  public abstract class FlowControl : UserControl
  {
    public abstract FlowViewModel ViewModel { get; }
    public abstract string FlowType { get; }
    public abstract FlowControl CreateInstanceOnCanvas(Editor editor);

    public virtual FlowControl CreateInstanceOnCanvas(Editor editor, FlowModel model)
    {
      return CreateInstanceOnCanvas(editor);
    }

    public virtual void RegisterNodes(Dictionary<Guid, NodeViewModel> nodeLookup)
    {
    }
  }
}
