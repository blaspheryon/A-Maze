using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AMaze.Mazes;

namespace AMaze.Renderer
{
    public class MazeDrawer : FrameworkElement
    {
        private IGrid? _grid;

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

            // Draw only dirty cells if any, otherwise draw all
            var cellsToDraw = _grid.DirtyCells.Count > 0 ? _grid.DirtyCells : _grid.Cells;

            foreach (var cell in cellsToDraw)
            {
                DrawCell(dc, cell);
            }

            // Clear dirty list after drawing
            _grid.ClearDirty();
        }

        private void DrawCell(DrawingContext dc, ICell cell)
        {
            // 1. Fill background
            var polygon = cell.GetPolygon(_grid!.CellSize);
            if (polygon.Length > 2)
            {
                var geometry = new StreamGeometry();
                using (var ctx = geometry.Open())
                {
                    ctx.BeginFigure(polygon[0], true, true);
                    ctx.PolyLineTo(polygon[1..], true, true);
                }

                Brush brush = Brushes.White;
                if (cell.IsVisited) brush = Brushes.LightGray;
                if (cell.IsPath) brush = Brushes.Yellow;
                if (cell.IsActive) brush = Brushes.OrangeRed;

                dc.DrawGeometry(brush, null, geometry);
            }

            // 2. Draw walls
            var pen = new Pen(Brushes.Black, 1);
            foreach (var (start, end) in cell.GetEdges())
            {
                dc.DrawLine(pen, start, end);
            }
        }

        private void MazeDrawer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_grid == null) return;

            Point pos = e.GetPosition(this);

            // Let the grid resolve which cell was clicked
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
}
