using System.Windows;

namespace AMaze.Mazes;

public abstract class CellBase(int row, int column, IGrid grid) : ICell
{
    private readonly IGrid grid = grid; // readonly reference to parent grid
    protected readonly Dictionary<ICell, bool> links = [];

    public int Row { get; } = row;
    public int Column { get; } = column;

    private bool isActive = false;
    private bool isVisited = false;
    private bool isPath = false;

    public Point Coordinates => new(Column, Row);
    public virtual List<ICell> Links => links.Keys.ToList();
    public abstract List<ICell> Neighbours { get; }

    /// <summary>
    /// Automatically mark dirty when state changes
    /// </summary>
    public bool IsActive
    {
        get => isActive;
        set
        {
            if (isActive != value)
            {
                isActive = value;
                grid.MarkDirty(this);
            }
        }
    }

    public bool IsVisited
    {
        get => isVisited;
        set
        {
            if (isVisited != value)
            {
                isVisited = value;
                grid.MarkDirty(this);
            }
        }
    }

    public bool IsPath
    {
        get => isPath;
        set
        {
            if (isPath != value)
            {
                isPath = value;
                grid.MarkDirty(this);
            }
        }
    }

    public virtual void Link(ICell cell, bool bidirectional = true)
    {
        if (cell == null) return;

        links[cell] = true;
        grid.MarkDirty(this);

        if (bidirectional)
        {
            cell.Link(this, false);
        }
    }

    public virtual void Unlink(ICell cell, bool bidirectional = true)
    {
        if (cell == null) return;

        links.Remove(cell);
        grid.MarkDirty(this);

        if (bidirectional)
        {
            cell.Unlink(this, false);
        }
    }

    public bool IsLinked(ICell? cell) => cell != null && links.ContainsKey(cell);

    public Distances GetCellDistances()
    {
        Distances distances = new(this);
        List<ICell> queue = [this];

        while (queue.Count > 0)
        {
            List<ICell> newQueue = [];

            foreach (ICell cell in queue)
            {
                foreach (ICell neighbour in cell.Links)
                {
                    if (distances[neighbour] >= 0) continue;

                    distances[neighbour] = distances[cell] + 1;
                    newQueue.Add(neighbour);
                }
            }

            queue = newQueue;
        }

        return distances;
    }

    public abstract Point GetCenter(double cellSize, Point? gridCenter = null);
    public abstract Point[] GetPolygon(double cellSize, Point? gridCenter = null);
    public abstract IEnumerable<(Point Start, Point End)> GetEdges();
}
