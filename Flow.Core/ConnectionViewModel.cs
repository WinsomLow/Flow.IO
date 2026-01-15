using System.Windows;
using System.Windows.Media;

namespace Flow.Core
{
  public sealed class ConnectionViewModel : NotifyObject
  {
    private readonly Connection m_model;
    private Geometry m_path = Geometry.Empty;

    public ConnectionViewModel(Connection model)
    {
      m_model = model;
      Rebuild();
    }

    public Geometry Path
    {
      get => m_path;

      private set
      {
        if (Equals(m_path, value))
        {
          return;
        }
        m_path = value;
        Notify();
      }
    }

    private void Rebuild()
    {
      Point p1 = m_model.FromNode.Position.ToPoint();
      Point p2 = m_model.ToNode.Position.ToPoint();

      // Simple horizontal bezier
      double dx = Math.Max(60, Math.Abs(p2.X - p1.X) * 0.5);
      var c1 = new Point(p1.X + dx, p1.Y);
      var c2 = new Point(p2.X - dx, p2.Y);

      var figure = new PathFigure
      {
        StartPoint = p1,
        Segments = { new BezierSegment(c1, c2, p2, true) },
        IsFilled = false,
        IsClosed = false
      };

      Path = new PathGeometry(new[] { figure });
    }
  }
}
