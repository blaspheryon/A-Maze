using System.Windows;
using AMaze.Algorithms;
using AMaze.Algorithms.Generation;
using AMaze.Mazes;
using AMaze.Mazes.Square;
using AMaze.Renderer;

namespace A_Maze_ZPCT
{
    public partial class MainWindow : Window
    {
        private IGrid? _grid;
        private bool _modified;

        public MainWindow()
        {
            InitializeComponent();

            // Create a simple 50x50 grid with 12px cells
            _grid = new SquareGrid(50, 50, 12);
            MazeDrawerControl.Initialize(_grid);
        }

        private void GenerateMaze_Click(object sender, RoutedEventArgs e)
        {
            if (_grid == null || _modified) return;

            // Use the Aldous-Broder algorithm to generate the maze in-place
            var alg = new AldousBroder(_grid, 12);
            alg.CreateMaze();                     // modifies the existing grid
            _modified = true;

            MazeDrawerControl.InvalidateVisual();
        }


        private void ResetStartEnd_Click(object sender, RoutedEventArgs e)
        {
            if (_grid == null) return;

            foreach (var cell in _grid.Cells)
            {
                cell.IsActive = false;
                cell.IsPath = false;
                cell.IsVisited = false;
            }

            MazeDrawerControl.InvalidateVisual();
        }
    }
}
