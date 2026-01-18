using Flow.Core.Common;
using Flow.Core.Model;
using System.Windows;
using System.Windows.Media;

namespace Flow.Core.ViewModel
{
  public sealed class ConnectionViewModel : NotifyObject
  {
    private readonly Connection m_model;
    private Geometry m_path = Geometry.Empty;
    private double _midX;
    private double _midY;

    public ConnectionViewModel(Connection model)
    {
      m_model = model;
      _midX = 0;
      _midY = 0;
      Rebuild();
    }

    public Connection Model => m_model;

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

    public double MidPointX
    {
      get => _midX;
      private set
      {
        if (Equals(_midX, value))
        {
          return;
        }
        _midX = value;
        Notify();
      }
    }

    public double MidPointY
    {
      get => _midY;
      private set
      {
        if (Equals(_midY, value))
        {
          return;
        }
        _midY = value;
        Notify();
      }
    }

    public string Label
    {
      get => m_model.Label;
      set
      {
        if (Equals(m_model.Label, value))
        {
          return;
        }
        m_model.Label = value;
        Notify();
      }
    }

    public void Rebuild()
    {
      Point p1 = m_model.FromNode.Position.ToPoint();
      Point p2 = m_model.ToNode.Position.ToPoint();

      // Simple horizontal bezier
      double dx = Math.Max(60, Math.Abs(p2.X - p1.X) * 0.5);
      var c1 = new Point(p1.X + dx, p1.Y);
      var c2 = new Point(p2.X - dx, p2.Y);
      var seg = new BezierSegment(c1, c2, p2, true);

      var figure = new PathFigure
      {
        StartPoint = p1,
        Segments = { seg },
        IsFilled = false,
        IsClosed = false
      };

      var center = GetBezierCenter(figure, seg);
      MidPointX = center.X;
      MidPointY = center.Y;
      Path = new PathGeometry(new[] { figure });
    }

    private static Point GetBezierCenter(PathFigure figure, BezierSegment seg)
    {
      Point p0 = figure.StartPoint;
      Point p1 = seg.Point1;
      Point p2 = seg.Point2;
      Point p3 = seg.Point3;

      return new Point(
          (p0.X + 3 * p1.X + 3 * p2.X + p3.X) / 8.0,
          (p0.Y + 3 * p1.Y + 3 * p2.Y + p3.Y) / 8.0
      );
    }
  }
}
