using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AMaze.Algorithms.Generation;
using AMaze.Mazes;
using AMaze.Mazes.Square;
using AMazer.Algorithms.Generation;
using Color = System.Windows.Media.Color;
using Pen = System.Windows.Media.Pen;

namespace AMaze;

public partial class MainWindow : Window
{
    private IGrid? _grid;

    public MainWindow()
    {
        InitializeComponent();

        // Populate algorithm ComboBox via reflection
        var algoTypes = Assembly.GetExecutingAssembly()
                                .GetTypes()
                                .Where(t => typeof(IGenerationAlgorithm).IsAssignableFrom(t)
                                            && !t.IsAbstract
                                            && t.IsClass)
                                .ToList();

        CbAlgorithms.ItemsSource = algoTypes;
        CbAlgorithms.DisplayMemberPath = "Name";
        CbAlgorithms.SelectedIndex = 0;

        // Handle resizing dynamically
        MazeDrawerControl.SizeChanged += MazeDrawerControl_SizeChanged;

        // Track parent container size for dynamic resizing
        if (MazeDrawerControl.Parent is FrameworkElement parent)
            parent.SizeChanged += MazeDrawerControl_SizeChanged;

        // Example: wall thickness slider (if you add one)
        // SliderWallThickness.ValueChanged += (s, e) =>
        // {
        //     MazeDrawerControl.WallThickness = SliderWallThickness.Value;
        //     MazeDrawerControl.InvalidateVisual();
        // };
    }

    private void CenterMazeDrawer()
    {
        if (_grid == null || MazeDrawerControl.Parent is not Canvas canvas) return;

        double totalWidth = _grid.Columns * _grid.CellSize;
        double totalHeight = _grid.Rows * _grid.CellSize;

        MazeDrawerControl.Width = totalWidth;
        MazeDrawerControl.Height = totalHeight;

        Canvas.SetLeft(MazeDrawerControl, Math.Max(0, (canvas.ActualWidth - totalWidth) / 2));
        Canvas.SetTop(MazeDrawerControl, Math.Max(0, (canvas.ActualHeight - totalHeight) / 2));
    }


    private void MazeDrawerControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_grid == null) return;

        RecalculateCellSize();
        CenterMazeDrawer();
        MazeDrawerControl.Initialize(_grid);
    }

    private void RecalculateCellSize()
    {
        if (_grid == null) return;

        int rows = _grid.Rows;
        int cols = _grid.Columns;

        // Use parent container size to calculate cell size
        double availableWidth = ((FrameworkElement)MazeDrawerControl.Parent).ActualWidth;
        double availableHeight = ((FrameworkElement)MazeDrawerControl.Parent).ActualHeight;

        int cellSize = (int)Math.Min(availableWidth / cols, availableHeight / rows);

        _grid.CellSize = cellSize;
    }

    private void GenerateMaze_Click(object sender, RoutedEventArgs e)
    {
        int rows = 20;
        int cols = 20;

        double availableWidth = ((FrameworkElement)MazeDrawerControl.Parent).ActualWidth;
        double availableHeight = ((FrameworkElement)MazeDrawerControl.Parent).ActualHeight;

        int cellSize = (int)Math.Min(availableWidth / cols, availableHeight / rows);

        _grid = new SquareGrid(rows, cols, cellSize);

        // Use reflection to create selected algorithm
        if (CbAlgorithms.SelectedItem is Type algoType)
        {
            var ctor = algoType.GetConstructor(new[] { typeof(IGrid), typeof(int) });
            IGenerationAlgorithm gen;

            if (ctor != null)
            {
                gen = (IGenerationAlgorithm)ctor.Invoke(new object?[] { _grid, 0 })!;
            }
            else
            {
                ctor = algoType.GetConstructor(new[] { typeof(IGrid) });
                if (ctor == null) throw new InvalidOperationException("No suitable constructor found.");
                gen = (IGenerationAlgorithm)ctor.Invoke(new object?[] { _grid })!;
            }

            gen.CreateMaze();
        }

        MazeDrawerControl.Initialize(_grid);
    }

    private void ResetMaze_Click(object sender, RoutedEventArgs e)
    {
        _grid = null;
        MazeDrawerControl.InvalidateVisual();
    }

    private void PickWallColor_Click(object sender, RoutedEventArgs e)
    {
        using var dlg = new ColorDialog();
        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            // Convert System.Drawing.Color to WPF Color
            var wpfColor = Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B);

            // Create a new Pen from a SolidColorBrush
            _grid!.Style.WallPen = new Pen(new SolidColorBrush(wpfColor), 1)
            {
                StartLineCap = PenLineCap.Square,
                EndLineCap = PenLineCap.Square
            };

            MazeDrawerControl.InvalidateVisual();
        }
    }


    private void FindPath_Click(object sender, RoutedEventArgs e)
    {
        if (_grid == null) return;

        // TODO: implement pathfinding
    }
}
