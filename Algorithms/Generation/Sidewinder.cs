using AMaze.Mazes;
using AMaze.Mazes.Square;
using AMazer.Utils;

namespace AMaze.Algorithms.Generation;

/// <summary>
/// A class representing the Sidewinder maze generation algorithm.
/// </summary>
///
/// <remarks>
/// <para>
/// This class implements the <see cref="IGenerationAlgorithm"/> interface.
/// You can find more in the interface documentation.
/// </para>
///
/// The Sidewinder algorithm is a simple maze generation algorithm, which
/// is quite fast, but creates vertically biased mazes.
/// </remarks>
///
/// <param name="grid">
/// The <see cref="IGrid"/> on which the maze is generated.
/// </param>
///
/// <param name="seed">
/// An integer seed used for the consistent maze generation for the same seed.
/// </param>
/// <remarks>
/// Creates a new instance of the <see cref="Sidewinder"/> class.
/// </remarks>
/// <param name="grid">The <see cref="IGrid"/> which to generate the maze on.</param>
/// <param name="seed">The seed for random number generation.</param>
class Sidewinder(IGrid grid, int seed) : IGenerationAlgorithm
{
    private readonly Random rand = new(seed);

    // Collection of all the cells within the grid
    private readonly IEnumerator<ICell> CellIterator = grid.Cells.GetEnumerator();

    // List of cells representing currently visited cells in one row
    private List<ICell> run = [];

    // The current cell used in Step function
    private ICell CurrentCell => CellIterator.Current;
    private readonly IGrid grid = grid;
    public List<ICell> CurrentCells { get; } = [];

    /// <summary>
    /// Gets the "North" cell depending on the grid shape.
    /// </summary>
    /// <param name="cell"> The <see cref="ICell"/> cell whose "North" <see cref="ICell"/> neighbour we return. </param>
    /// <returns>The North <see cref="ICell"/> neighbour.</returns>
    private static ICell? GetNorthCell(ICell cell) => ((SquareCell)cell).North;

    /// <summary>
    /// Gets the "East" cell depending on the grid shape.
    /// </summary>
    /// <param name="cell"> The <see cref="ICell"/> cell whose "East" <see cref="ICell"/> neighbour we return. </param>
    /// <returns>The East <see cref="ICell"/> neighbour.</returns>
    private static ICell? GetEastCell(ICell cell) => ((SquareCell)cell).East;

    public IGrid CreateMaze()
    {
        /// Starts with top left cell and either link with east neighbour
        /// or picks a random cell from visited cells in row and links it with it's
        /// north neighbour. Then it continues doing this until it finishes the current row.
        /// Then it moves down and repeats until all cells were visited.

        foreach (List<ICell> row in grid.EachRow)
        {
            run.Clear();

            foreach (ICell cell in row)
            {
                run.Add(cell);

                bool hitEastBoundary = GetEastCell(cell) == null;
                bool hitNorthBoundary = GetNorthCell(cell) == null;
                bool shouldClose = hitEastBoundary || ((!hitNorthBoundary) && rand.Next(2) == 0);

                if (shouldClose)
                {
                    ICell runCell = run.RandomElement(rand);
                    ICell? northCell = GetNorthCell(runCell);

                    if (northCell != null) { runCell.Link(northCell); }

                    run.Clear();
                }
                else
                {
                    cell.Link(GetEastCell(cell)!);
                }
            }
        }

        return grid;
    }

    public bool Step()
    {
        /// CreateMaze rewritten to perform only a single step upon call.
        /// Used for showing maze generation animation.

        // Ensures that the center is linked in polar grids.

        if (!CellIterator.MoveNext()) { return false; }

        run = CurrentCell.Column == 0 ? [] : run;

        CurrentCells.Clear();
        run.Add(CurrentCell);

        bool hitEastBoundary = GetEastCell(CurrentCell) == null;
        bool hitNorthBoundary = GetNorthCell(CurrentCell) == null;
        bool shouldClose = hitEastBoundary || (!hitNorthBoundary && rand.Next(2) == 0);

        if (shouldClose)
        {
            ICell runCell = run.RandomElement(rand);
            ICell? northCell = GetNorthCell(runCell);

            if (northCell != null) { runCell.Link(northCell); }

            run = [];
        }
        else
        {
            CurrentCell.Link(GetEastCell(CurrentCell)!);
        }

        CurrentCells.Add(CurrentCell);

        return true;
    }
}
