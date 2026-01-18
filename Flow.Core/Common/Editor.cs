using Flow.Core.Control;
using System.Windows;
using System.Windows.Controls;

namespace Flow.Core.Common
{
  // TODO: Use OOP state design pattern instead
  public enum DragMode
  {
    None = 0,
    MoveBlock,
    DrawConnection,
  }

  public class MoveBlockInfo
  {
    public required FlowControl ActiveControl { get; set; }

    public Point Offset { get; set; }
  }

  public class Editor(Canvas designCanvas)
  {
    public Canvas DesignCanvas { get; } = designCanvas;

    public DragMode DragMode { get; set; } = DragMode.None;

    public MoveBlockInfo? MoveBlockInfo { get; set; } = null;
  }
}
