using Point = System.Windows.Point;

namespace AMaze.Mazes;

public abstract class GridBase : IGrid
{
    // 2D grid of cells
    protected List<List<ICell>> grid = [];

    public int Rows { get; protected set; }
    public int Columns { get; protected set; }
    public int CellSize { get; set; }

    public GridStyle Style { get; } = new();

    /// <summary>
    /// Cells that changed state since the last draw.
    /// </summary>
    public List<ICell> DirtyCells { get; } = new();

    /// <summary>
    /// Total number of cells in the grid
    /// </summary>
    public virtual int Size => grid.Sum(row => row.Count);

    /// <summary>
    /// Enumerates all cells in the grid
    /// </summary>
    public IEnumerable<ICell> Cells
    {
        get
        {
            foreach (var row in grid)
                foreach (var cell in row)
                    yield return cell;
        }
    }

    /// <summary>
    /// Enumerates all rows
    /// </summary>
    public IEnumerable<List<ICell>> EachRow => grid;

    /// <summary>
    /// Marks a cell as dirty, to be redrawn on next frame
    /// </summary>
    public void MarkDirty(ICell cell)
    {
        if (cell != null && !DirtyCells.Contains(cell))
            DirtyCells.Add(cell);
    }

    /// <summary>
    /// Clears the list of dirty cells after rendering
    /// </summary>
    public void ClearDirty() => DirtyCells.Clear();

    /// <summary>
    /// Indexer for accessing a row directly
    /// </summary>
    public List<ICell> this[int row] => row >= 0 && row < Rows ? grid[row] : [];

    /// <summary>
    /// Returns a random cell from the grid
    /// </summary>
    public virtual ICell GetRandomCell(int seed = 0)
    {
        var rand = new Random(seed);
        int row = rand.Next(Rows);
        int column = rand.Next(grid[row].Count);
        return this[row, column];
    }

    /// <summary>
    /// Converts screen coordinates to grid coordinates
    /// Must be implemented by derived classes (SquareGrid, PolarGrid, etc.)
    /// </summary>
    public abstract Point ScreenToGridCoordinates(Point screenCoords);

    /// <summary>
    /// Indexer for accessing a specific cell
    /// Must be implemented by derived classes to handle specific grid layout
    /// </summary>
    public abstract ICell this[int row, int column] { get; }
}
