using AMaze.Mazes;
using AMaze.Utils;

namespace AMaze.Algorithms.Generation;

public class RecursiveBacktracker : IGenerationAlgorithm
{
    private readonly Random rand;
    private readonly IGrid grid; // store the grid reference

    // The current cell used in Step function
    private ICell currentCell;
    public List<ICell> CurrentCells { get; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="RecursiveBacktracker"/> class.
    /// </summary>
    /// <param name="grid">The <see cref="IGrid"/> which to generate the maze on.</param>
    /// <param name="seed">The seed for random number generation.</param>
    public RecursiveBacktracker(IGrid grid, int seed)
    {
        this.grid = grid;
        rand = new Random(seed);
        currentCell = grid.GetRandomCell(seed);
        CurrentCells.Add(currentCell);
    }

    public IGrid CreateMaze()
    {
        while (CurrentCells.Count > 0)
        {
            Step();
        }

        return grid;
    }

    public bool Step()
    {
        if (CurrentCells.Count == 0) return false;

        currentCell = CurrentCells.Last();
        List<ICell> neighbours = currentCell.Neighbours
            .FindAll(c => c.Links.Count == 0);

        if (neighbours.Count == 0)
        {
            CurrentCells.RemoveAt(CurrentCells.Count - 1);
        }
        else
        {
            ICell neighbour = neighbours.RandomElement(rand);
            currentCell.Link(neighbour);
            CurrentCells.Add(neighbour);
        }

        return true;
    }
}
