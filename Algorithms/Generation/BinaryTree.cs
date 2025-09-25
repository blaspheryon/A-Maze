using AMaze.Mazes;
using AMaze.Mazes.Square;
using AMazer.Utils;

namespace AMazer.Algorithms.Generation;

/// <summary>
///  A class representing the Binary Tree maze generating algorithm.
/// </summary>
/// <remarks>
/// <para>
/// This class implements the <see cref="IGenerationAlgorithm"/> interface.
/// You can find more in the interface documentation.
/// </para>
///
/// <para>
/// The Binary Tree is a simple and fast algorithm, but it's mazes are very biased.
/// </para>
/// </remarks>
///
/// <param name="grid">
/// <see cref="IGrid"/> grid on which we generate the maze.
/// </param>
///
/// <param name="seed">
/// An integer seed used for the consistent maze generation for the same seed.
/// </param>
class BinaryTree(IGrid grid, int seed) : IGenerationAlgorithm
{
    private readonly Random rand = new(seed);

    // Collection of all the cells within the grid
    private readonly IGrid grid_ = grid;
    private readonly IEnumerator<ICell> CellIterator = grid.Cells.GetEnumerator();

    // Gets the current cell used in Step function
    private ICell CurrentCell => CellIterator.Current;
    public List<ICell> CurrentCells { get; private set; } = [];

    public IGrid CreateMaze()
    {
        /// Iterates through all cells in the grid, and for each
        /// chooses to link with either north neighbour or east neighbour.
        /// For polar grids it chooses either the inward or clockwise neighbour.
        while (CellIterator.MoveNext())
        {
            ICell cell = CellIterator.Current;
            List<ICell> neighbours = GetAdjacentCells(cell);

            if (neighbours.Count > 0)
            {
                ICell neighbour = neighbours.RandomElement(rand);
                cell.Link(neighbour);
            }
        }

        return grid_;
    }

    public bool Step()
    {
        /// CreateMaze rewritten to perform only a single step upon call.
        /// Used for showing maze generation animation
        if (!CellIterator.MoveNext()) { return false; }

        CurrentCells = [CurrentCell];

        List<ICell> neighbours = GetAdjacentCells(CurrentCell!);

        if (neighbours.Count > 0)
        {
            ICell neighbour = neighbours.RandomElement(rand);
            CurrentCell.Link(neighbour);
        }

        return true;
    }

    /// <summary>
    /// Gets the neighbours for Binary Tree algorithm from which we choose one randomly.
    /// </summary>
    /// <param name="cell">
    /// The <see cref="ICell"/> cell whose neighbours we return.
    /// </param>
    /// <returns>
    /// List of <see cref="ICell"/> neighbours for Binary Tree algorithm.
    /// </returns>
    private List<ICell> GetAdjacentCells(ICell cell)
    {
        SquareCell squareCell = (SquareCell)cell;

        return new List<ICell> { squareCell.North, squareCell.East }.FindAll(c => c != null);
    }
}
