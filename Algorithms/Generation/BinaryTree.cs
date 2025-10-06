using System;
using System.Collections.Generic;
using System.Linq;
using AMaze.Mazes;
using AMaze.Mazes.Square;

namespace AMaze.Algorithms.Generation;

/// <summary>
/// Binary Tree maze generation algorithm compatible with current GridBase/CellBase.
/// </summary>
public class BinaryTree(IGrid grid, int seed = 0) : IGenerationAlgorithm
{
    private readonly Random _rand = new Random(seed);
    private readonly IGrid _grid = grid ?? throw new ArgumentNullException(nameof(grid));
    private IEnumerator<ICell>? _cellIterator;

    public List<ICell> CurrentCells { get; private set; } = new();

    /// <summary>
    /// Fully generate the maze at once.
    /// </summary>
    public IGrid CreateMaze()
    {
        foreach (var cell in _grid.Cells)
        {
            LinkRandomNeighbour(cell);
        }

        return _grid;
    }

    /// <summary>
    /// Performs a single step of maze generation for animation.
    /// </summary>
    public bool Step()
    {
        if (_cellIterator == null)
            _cellIterator = _grid.Cells.GetEnumerator();

        if (!_cellIterator.MoveNext())
            return false;

        var current = _cellIterator.Current!;
        CurrentCells = [current];
        LinkRandomNeighbour(current);

        return true;
    }

    /// <summary>
    /// Chooses either North or East neighbour (for square grids) randomly and links.
    /// </summary>
    private void LinkRandomNeighbour(ICell cell)
    {
        if (cell is not SquareCell squareCell) return;

        var neighbours = new List<ICell>();
        if (squareCell.North != null) neighbours.Add(squareCell.North);
        if (squareCell.East != null) neighbours.Add(squareCell.East);

        if (neighbours.Count > 0)
        {
            var neighbour = neighbours[_rand.Next(neighbours.Count)];
            cell.Link(neighbour);
        }
    }
}
