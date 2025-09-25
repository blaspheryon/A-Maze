using AMaze.Mazes;
using AMazer.Utils;

namespace AMazer.Algorithms.Generation;

/// <summary>
///  A class representing the Hunt and Kill maze generating algorithm.
/// </summary>
/// <remarks>
/// <para>
/// This class implements the <see cref="IGenerationAlgorithm"/> interface.
/// You can find more in the interface documentation.
/// </para>
///
/// <para>
/// The Hunt-and-Kill algorithm is a mix of a random walk and a systematic search to generate a maze.
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
class HuntAndKill(IGrid grid, int seed) : IGenerationAlgorithm
{
    private readonly Random rand = new(seed);

    // The current cell used in Step function
    private readonly IGrid grid_ = grid;
    private ICell? currentCell = grid.GetRandomCell(seed);
    public List<ICell> CurrentCells { get; } = [];

    public IGrid CreateMaze()
    {
        /// Starts from random cell and finds its unvisited neighbours
        /// and performs a random walk (Depth-First search).
        /// When it hits a dead end, it starts "Hunting".
        /// It scans the grid from top to bottom until it finds a first unvisited cell
        /// adjacent to an already visited cell. It starts random walk again and this repeats
        /// until there are no unvisited cells.
        while (currentCell != null)
        {
            List<ICell> unvisited = currentCell.Neighbours.FindAll(c => c.Links.Count == 0);

            if (unvisited.Count > 0)
            {
                ICell neighbour = unvisited.RandomElement(rand);
                currentCell.Link(neighbour);
                currentCell = neighbour;
            }
            else
            {
                Hunt();
            }
        }

        return grid_;
    }

    public bool Step()
    {
        /// CreateMaze rewritten to perform only a single step upon call.
        /// Used for showing maze generation animation.
        if (currentCell == null) { return false; }

        CurrentCells.Add(currentCell);
        List<ICell> unvisited = currentCell.Neighbours.FindAll(c => c.Links.Count == 0);

        if (unvisited.Count != 0)
        {
            ICell neighbour = unvisited.RandomElement(rand);
            currentCell.Link(neighbour);
            currentCell = neighbour;
        }
        else
        {
            CurrentCells.Clear();
            Hunt();
        }

        return true;
    }

    /// <summary>
    /// Scans the <see cref="IGrid"/> grid from top to bottom until it finds a first
    /// unvisited <see cref="ICell"/> cell adjacent to an already visited <see cref="ICell"/> cell.
    /// </summary>
    private void Hunt()
    {
        currentCell = null;
        foreach (ICell cell in grid.Cells)
        {
            List<ICell> visited = cell.Neighbours.Where(c => c.Links.Count > 0).ToList();
            if (cell.Links.Count == 0 && visited.Count > 0)
            {
                ICell neighbour = visited.RandomElement(rand);
                currentCell = cell;
                currentCell.Link(neighbour);
                break;
            }
        }
    }
}
