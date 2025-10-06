using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using Brushes = System.Windows.Media.Brushes;
using Brush = System.Windows.Media.Brush;
using Pen = System.Windows.Media.Pen;

namespace AMaze.Mazes;

public record GridStyle
{
    public Pen WallPen { get; set; } = new Pen(Brushes.Black, 1)
    {
        StartLineCap = PenLineCap.Square,
        EndLineCap = PenLineCap.Square
    };

    public Brush VisitedCellBrush { get; set; } = Brushes.LightGray;
    public Brush PathCellBrush { get; set; } = Brushes.Yellow;
    public Brush ActiveCellBrush { get; set; } = Brushes.OrangeRed;
    public Brush DefaultCellBrush { get; set; } = Brushes.White;

    // Insets (gap between walls), only valid for square mazes
    public double CellInset { get; set; } = 0;
}