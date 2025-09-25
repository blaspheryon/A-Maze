using System.Windows;

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

    /// <summary>
    /// Returns the 4 corner points of the square cell for drawing purposes.
    /// </summary>
    public override Point[] GetPolygon(double cellSize, Point? gridCenter = null)
    {
        double x = Column * cellSize;
        double y = Row * cellSize;

        return
        [
            new(x, y),                        // top-left
            new (x + cellSize, y),             // top-right
            new (x + cellSize, y + cellSize),  // bottom-right
            new (x, y + cellSize)              // bottom-left
        ];
    }


    public override IEnumerable<(Point Start, Point End)> GetEdges()
    {
        double x = Column * grid.CellSize;
        double y = Row * grid.CellSize;
        double size = grid.CellSize;

        Point topLeft = new(x, y);
        Point topRight = new(x + size, y);
        Point bottomRight = new(x + size, y + size);
        Point bottomLeft = new(x, y + size);

        if (!IsLinked(North)) yield return (topLeft, topRight);
        if (!IsLinked(East)) yield return (topRight, bottomRight);
        if (!IsLinked(South)) yield return (bottomRight, bottomLeft);
        if (!IsLinked(West)) yield return (bottomLeft, topLeft);
    }
}