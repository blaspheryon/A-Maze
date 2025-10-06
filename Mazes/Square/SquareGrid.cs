using Point = System.Windows.Point;

namespace AMaze.Mazes.Square;

public class SquareGrid : GridBase
{
    /// <summary>
    /// Creates a SquareGrid with the given rows, columns, and cell size
    /// </summary>
    public SquareGrid(int rows, int columns, int cellSize)
    {
        Rows = rows;
        Columns = columns;
        CellSize = cellSize;

        InitializeGrid();
        ConfigureCells();
    }

    /// <summary>
    /// Fill the grid with SquareCell instances
    /// </summary>
    private void InitializeGrid()
    {
        grid = [];

        for (int row = 0; row < Rows; row++)
        {
            List<ICell> rowCells = new List<ICell>();
            for (int col = 0; col < Columns; col++)
            {
                rowCells.Add(new SquareCell(row, col, this));
            }
            grid.Add(rowCells);
        }
    }

    /// <summary>
    /// Configure each cell's neighbors
    /// </summary>
    private void ConfigureCells()
    {
        foreach (SquareCell cell in Cells.Cast<SquareCell>())
        {
            cell.North = this[cell.Row - 1, cell.Column] as SquareCell;
            cell.South = this[cell.Row + 1, cell.Column] as SquareCell;
            cell.West = this[cell.Row, cell.Column - 1] as SquareCell;
            cell.East = this[cell.Row, cell.Column + 1] as SquareCell;
        }
    }

    /// <summary>
    /// Access a specific cell by row and column
    /// </summary>
    public override ICell this[int row, int column]
    {
        get
        {
            if (row < 0 || row >= Rows) return null;
            if (column < 0 || column >= Columns) return null;
            return grid[row][column];
        }
    }

    /// <summary>
    /// Convert screen coordinates to grid coordinates
    /// </summary>
    public override Point ScreenToGridCoordinates(Point screenCoords)
    {
        int col = (int)(screenCoords.X / CellSize);
        int row = (int)(screenCoords.Y / CellSize);

        if (col < 0 || col >= Columns || row < 0 || row >= Rows)
        {
            return new Point(-1, -1);
        }

        return new Point(col, row);
    }
}
