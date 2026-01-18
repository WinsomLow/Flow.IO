using Flow.Core.Common;
using Flow.Core.Model;
using System.Windows;

namespace Flow.Core.ViewModel
{
  public sealed class NodeViewModel(Node model) : NotifyObject
  {
    private readonly Node m_model = model;

    private Point? m_dragOffset;
    public Guid Id => m_model.Id;
    public Node Model => m_model;

    public double X
    {
      get => m_model.Position.X;
      set
      {
        if (Equals(m_model.Position.X, value))
        {
          return;
        }
        m_model.Position.X = value;
        Notify(nameof(X));
        Notify(nameof(Anchor));
      }
    }

    public double Y
    {
      get => m_model.Position.Y;
      set
      {
        if (Equals(m_model.Position.Y, value))
        {
          return;
        }
        m_model.Position.Y = value;
        Notify(nameof(Y));
        Notify(nameof(Anchor));
      }
    }

    public Point Anchor => new(X, Y);

    public void BeginDrag(Point mouseCanvasPosition)
    {
      m_dragOffset = new Point(mouseCanvasPosition.X - X, mouseCanvasPosition.Y - Y);
    }

    public void DragTo(Point mouseCanvasPosition)
    {
      if (m_dragOffset is Point offset)
      {
        m_model.Position.X = mouseCanvasPosition.X - offset.X;
        m_model.Position.Y = mouseCanvasPosition.Y - offset.Y;
        Notify(nameof(Anchor));
      }
    }

    public void EndDrag() => m_dragOffset = null;
  }
}
