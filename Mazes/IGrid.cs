using Point = System.Windows.Point;

namespace AMaze.Mazes;

public interface IGrid
{
    /// <summary>
    /// Gets the number of rows in the <see cref="IGrid"/> grid.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Gets the number of columns in the <see cref="IGrid"/> grid.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Gets the number of <see cref="ICell"/> cells in the <see cref="IGrid"/> grid.
    /// </summary>
    public int Size { get; }

    /// <summary>
    /// Gets the size of the <see cref="ICell"/> cells in pixels.
    /// </summary>
    public int CellSize { get; set; }

    /// <summary>
    /// List of cells that changed state since the last draw.
    /// </summary>
    public List<ICell> DirtyCells { get; }

    /// <summary>
    /// Gets the style of the Grid (e.g. wall color, active cell color, etc.)
    /// </summary>
    public GridStyle Style { get; }

    /// <summary>
    /// Marks a cell as dirty, so it will be redrawn on the next frame.
    /// </summary>
    /// <param name="cell">The <see cref="ICell"/> to mark as dirty.</param>
    public void MarkDirty(ICell cell);

    /// <summary>
    /// Clears the list of dirty cells after rendering.
    /// </summary>
    public void ClearDirty();

    /// <summary>
    /// Indexer which returns the <see cref="ICell"/> cell located at given row and column.
    /// </summary>
    public ICell this[int row, int column] { get; }

    /// <summary>
    /// Indexer which returns the <see cref="IGrid"/> row as a list of <see cref="ICell"/>.
    /// </summary>
    public List<ICell> this[int row] { get; }

    /// <summary>
    /// Gets a whole row (list of <see cref="ICell"/> cells) in the <see cref="IGrid"/>.
    /// </summary>
    public IEnumerable<List<ICell>> EachRow { get; }

    /// <summary>
    /// Gets a collection of all <see cref="ICell"/> cells in the <see cref="IGrid"/> grid.
    /// </summary>
    public IEnumerable<ICell> Cells { get; }

    /// <summary>
    /// Converts the screen coordinates to <see cref="IGrid"/> coordinates.
    /// </summary>
    public Point ScreenToGridCoordinates(Point screenCoords);

    /// <summary>
    /// Gets a random <see cref="ICell"/> cell in the <see cref="IGrid"/> maze.
    /// </summary>
    public ICell GetRandomCell(int seed);
}
