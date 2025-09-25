using AMaze.Mazes;

namespace AMaze.Algorithms.Generation;

/// <summary>
/// Represents an interface that operates on a <see cref="IGrid"/> grid of <see cref="ICell"/> cells
/// to generate a maze.
/// </summary>
interface IGenerationAlgorithm
{
    /// <summary>
    /// Gets the list of <see cref="ICell"/> cells currently being processed by the generation algorithm.
    /// </summary>
    /// <value>
    /// A list of <see cref="ICell"/> representing the cells being processed by generation algorithm.
    /// </value>
    List<ICell> CurrentCells { get; }

    /// <summary>
    /// Creates a maze on the given <see cref="IGrid"/> grid.
    /// </summary>
    /// <param name="grid">
    /// The given <see cref="IGrid"/> grid to generate the maze on.
    /// </param>
    /// <returns>
    /// The modified <see cref="IGrid"/> grid after the maze generation is completed.
    /// </returns>
    IGrid CreateMaze();

    /// <summary>
    /// Performs a single step of the maze generation algorithm.
    /// </summary>
    /// <returns>
    /// A boolean value indicating whether there are remaining steps in the algorithm.
    /// Returns <c>true</c> if more steps are available, otherwise <c>false</c>.
    /// </returns>
    bool Step();
}
