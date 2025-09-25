namespace AMaze.Mazes;

/// <summary>
/// Class representing a collection of distances from root <see cref="ICell"/> to all other cells.
/// </summary>
/// <remarks>
/// This class maintains a dictionary of distances for each cell from a specified root cell. 
/// It allows retrieving and setting distances for individual cells.
/// </remarks>

public class Distances
{
    // Dictionary which returns a distance from root cell for given cell
    private readonly Dictionary<ICell, int> cells;

    // Represents the root cell we measure all the distances from.
    private readonly ICell root;

    /// <summary>
    /// Gets the farthest <see cref="ICell"/> from the root <see cref="ICell"/>.
    /// </summary>
    /// <value>
    /// The <see cref="ICell"/> with highest distance value from the root <see cref="ICell"/>.
    /// </value>
    public ICell FarthestCell => cells.MaxBy(kvp => kvp.Value).Key;

    /// <summary>
    /// Gets the distance of the farthest <see cref="ICell"/> from the root <see cref="ICell"/>.
    /// </summary>
    /// <value>
    /// The distance of the farthest <see cref="ICell"/>.
    /// </value>
    public int Maximum => cells.Values.Max();

    /// <summary>
    /// Initializes a new instance of the <see cref="Distances"/> class with the specified <see cref="ICell"/> root.
    /// </summary>
    /// <param name="root">
    /// The root <see cref="ICell"/> from which distances to other <see cref="ICell"/> cells will be measured.
    /// </param>
    public Distances(ICell root)
    {
        cells = new Dictionary<ICell, int>
        {
            [root] = 0
        };
        this.root = root;
    }

    /// <summary>
    /// Indexer which gets or sets the distance from <see cref="ICell"/> root to <see cref="ICell"/> target.
    /// </summary>
    /// <param name="cell">
    /// The <see cref="ICell"/> for which the distance is being accessed or set.
    /// </param>
    /// <returns>
    /// The distance of the specified <see cref="ICell"/> from the root cell.
    /// <para>
    ///  Returns -1 if the cell is not in the dictionary.
    /// </para>
    /// </returns>
    public int this[ICell cell]
    {
        get
        {
            if (cells.TryGetValue(cell, out int value))
            {
                return value;
            }
            return -1;
        }
        set
        {
            cells[cell] = value;
        }
    }

    /// <summary>
    /// A function which backtracks through the cell distance dictionary to find path
    /// from the root <see cref="ICell"/> to the given end <see cref="ICell"/>.
    /// </summary>
    /// <param name="target">The <see cref="ICell"/> we want to find the path to.</param>
    /// <returns>The found path as a list of <see cref="ICell"/>.</returns>
    public List<ICell> GetPathTo(ICell target)
    {
        ICell current = target;
        List<ICell> path = [];

        while (current != root)
        {
            foreach (ICell neighbour in current.Links)
            {
                if (this[neighbour] < this[current])
                {
                    path.Add(current);
                    current = neighbour;
                }
            }
        }
        path.Add(root);

        return path;
    }
}
