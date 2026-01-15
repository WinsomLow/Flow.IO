using System.Windows;

namespace Flow.Core
{
  public class Point2D
  {
    public double X { get; set; } = 0;
    public double Y { get; set; } = 0;

    public Point ToPoint()
    {
      return new Point(X, Y);
    }
  }
}
