using AMaze.Mazes;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;

namespace AMaze.Renderer;

public class MazeDrawer : FrameworkElement
{
    private IGrid? _grid;
    public Brush WallBrush { get; set; } = Brushes.Black;

    public MazeDrawer()
    {
        MouseLeftButtonDown += MazeDrawer_MouseLeftButtonDown;
    }

    public void Initialize(IGrid grid)
    {
        _grid = grid;
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);
        if (_grid == null) return;

        var cellsToDraw = _grid.DirtyCells.Count > 0 ? _grid.DirtyCells : _grid.Cells;

        foreach (var cell in cellsToDraw)
        {
            cell.Draw(dc);
        }

        _grid.ClearDirty();
    }

    private void MazeDrawer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (_grid == null) return;

        Point pos = e.GetPosition(this);
        Point gridCoords = _grid.ScreenToGridCoordinates(pos);

        int row = (int)gridCoords.Y;
        int col = (int)gridCoords.X;

        var cell = _grid[row, col];
        if (cell == null) return;

        cell.IsActive = !cell.IsActive;
        _grid.MarkDirty(cell);
        InvalidateVisual();
    }
}
