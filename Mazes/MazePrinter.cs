namespace AMaze.Mazes;

using System;
using AMaze.Mazes.Square;


public static class MazePrinter
{
    /// <summary>
    /// Prints the maze to the console in ASCII.
    /// </summary>
    public static void Print(SquareGrid grid)
    {
        int rows = grid.Rows;
        int cols = grid.Columns;

        // Top border
        Console.Write("+");
        for (int c = 0; c < cols; c++)
            Console.Write("---+");
        Console.WriteLine();

        for (int r = 0; r < rows; r++)
        {
            string top = "|";     // Left wall of the row
            string bottom = "+";  // Bottom border of the row

            for (int c = 0; c < cols; c++)
            {
                var cell = grid[r, c] as SquareCell;
                if (cell == null) continue;

                // Body of the cell
                top += "   ";

                // East wall: if not linked to East, print wall, else space
                top += (cell.IsLinked(cell.East)) ? " " : "|";

                // South wall: if not linked to South, print wall, else space
                bottom += (cell.IsLinked(cell.South)) ? "   " : "---";
                bottom += "+";
            }

            Console.WriteLine(top);
            Console.WriteLine(bottom);
        }
    }
}
