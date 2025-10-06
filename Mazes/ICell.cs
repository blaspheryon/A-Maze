using System.Windows.Media;
using Point = System.Windows.Point;

namespace AMaze.Mazes;

/// <summary>
/// An interface representing one Cell in the maze <see cref="IGrid"/> grid.
/// </summary>
/// <remarks>
/// Each cell maintains its links, neighbors, and state for maze generation or pathfinding.
/// States such as <see cref="IsActive"/>, <see cref="IsVisited"/>, and <see cref="IsPath"/> 
/// indicate the current visual and algorithmic status of the cell.
/// </remarks>
public interface ICell
{
    /// <summary>
    /// Gets the row coordinate of the <see cref="ICell"/> in the <see cref="IGrid"/> grid.
    /// </summary>
    int Row { get; }

    /// <summary>
    /// Gets the column coordinate of the <see cref="ICell"/> in the <see cref="IGrid"/> grid.
    /// </summary>
    int Column { get; }

    /// <summary>
    /// Indicates whether this cell is currently active in an algorithm step or being highlighted.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Indicates whether this cell has been visited by a maze generation or pathfinding algorithm.
    /// </summary>
    bool IsVisited { get; set; }

    /// <summary>
    /// Indicates whether this cell is part of the final solution path in a pathfinding algorithm.
    /// </summary>
    bool IsPath { get; set; }

    /// <summary>
    /// Gets the <see cref="Point"/> representing the location of the <see cref="ICell"/> in the maze grid.
    /// </summary>
    Point Coordinates { get; }

    /// <summary>
    /// Gets a list of all neighboring <see cref="ICell"/> cells.
    /// </summary>
    List<ICell> Neighbours { get; }

    /// <summary>
    /// Gets a list of all <see cref="ICell"/> cells linked to this cell.
    /// </summary>
    List<ICell> Links { get; }

    /// <summary>
    /// Links this cell to a given <see cref="ICell"/>. 
    /// By default, the link is bidirectional.
    /// </summary>
    /// <param name="cell">Cell to link to.</param>
    /// <param name="bidirectional">Whether to link bidirectionally.</param>
    void Link(ICell cell, bool bidirectional = true);

    /// <summary>
    /// Unlinks this cell from a given <see cref="ICell"/>. 
    /// By default, the unlink is bidirectional.
    /// </summary>
    /// <param name="cell">Cell to unlink.</param>
    /// <param name="bidirectional">Whether to unlink bidirectionally.</param>
    void Unlink(ICell cell, bool bidirectional = true);

    /// <summary>
    /// Determines whether this cell is linked to a given <see cref="ICell"/>.
    /// </summary>
    /// <param name="cell">Cell to check for a link.</param>
    /// <returns>True if linked, false otherwise.</returns>
    bool IsLinked(ICell? cell);

    /// <summary>
    /// Returns the center coordinates of this cell on screen.
    /// </summary>
    /// <param name="cellSize">The size of the cell in pixels.</param>
    /// <returns>Point representing the center of the cell.</returns>
    Point GetCenter(double cellSize, Point? gridCenter = null);

    /// <summary>
    /// Returns a <see cref="Distances"/> object containing distances from this cell
    /// to all other cells in the grid.
    /// </summary>
    /// <returns>A <see cref="Distances"/> object.</returns>
    Distances GetCellDistances();

    void Draw(DrawingContext dc);
}
