using System.Windows.Media;
using Point = System.Windows.Point;
using Brushes = System.Windows.Media.Brushes;
using Brush = System.Windows.Media.Brush;
using Pen = System.Windows.Media.Pen;

namespace AMaze.Mazes.Square;

public class SquareCell(int row, int column, IGrid grid) : CellBase(row, column, grid)
{
    /// <summary>
    /// Gets the West <see cref="SquareCell"/> neighbour.
    /// </summary>
    /// <value>West <see cref="SquareCell"/> neighbour.</value>
    public SquareCell? West { get; set; }

    /// <summary>
    /// Gets the East <see cref="SquareCell"/> neighbour.
    /// </summary>
    /// <value>East <see cref="SquareCell"/> neighbour.</value>
    public SquareCell? East { get; set; }

    /// <summary>
    /// Gets the North <see cref="SquareCell"/> neighbour.
    /// </summary>
    /// <value>North <see cref="SquareCell"/> neighbour.</value>
    public SquareCell? North { get; set; }

    /// <summary>
    /// Gets the South <see cref="SquareCell"/> neighbour.
    /// </summary>
    /// <value>South <see cref="SquareCell"/> neighbour.</value>
    public SquareCell? South { get; set; }

    public override List<ICell> Neighbours
    {
        get { return new List<ICell> { North!, South!, East!, West! }.FindAll(p => p != null); }
    }

    /// <summary>
    /// Returns the center of the square cell.
    /// </summary>
    public override Point GetCenter(double cellSize, Point? gridCenter = null)
    {
        // Calculates the center based on the position
        // in the grid and the cell size.
        double x = (Column + 0.5) * cellSize;
        double y = (Row + 0.5) * cellSize;

        return new Point(x, y);
    }

    public override void Draw(DrawingContext dc)
    {
        double size = grid.CellSize;
        double x = Column * size;
        double y = Row * size;

        // Calculate corner points
        Point topLeft = new(x, y);
        Point topRight = new(x + size, y);
        Point bottomRight = new(x + size, y + size);
        Point bottomLeft = new(x, y + size);

        // Fill color based on state
        Brush fillBrush = Brushes.White;
        if (IsVisited) fillBrush = Brushes.LightGray;
        if (IsPath) fillBrush = Brushes.Yellow;
        if (IsActive) fillBrush = Brushes.OrangeRed;

        // Draw cell background
        StreamGeometry fillGeometry = new();
        using (StreamGeometryContext ctx = fillGeometry.Open())
        {
            ctx.BeginFigure(topLeft, true, true);
            ctx.PolyLineTo(new[] { topRight, bottomRight, bottomLeft }, true, true);
        }
        dc.DrawGeometry(fillBrush, null, fillGeometry);

        // Draw walls if not linked in that direction
        if (North == null || !IsLinked(North))
            dc.DrawLine(grid.Style.WallPen, topLeft, topRight);

        if (East == null || !IsLinked(East))
            dc.DrawLine(grid.Style.WallPen, topRight, bottomRight);

        if (South == null || !IsLinked(South))
            dc.DrawLine(grid.Style.WallPen, bottomRight, bottomLeft);

        if (West == null || !IsLinked(West))
            dc.DrawLine(grid.Style.WallPen, bottomLeft, topLeft);
    }

}