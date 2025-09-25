using System;
using System.Collections.Generic;
using AMaze.Mazes;

namespace AMaze.Algorithms.Generation
{
    public class AldousBroder : IGenerationAlgorithm
    {
        private readonly Random rand;
        private readonly IGrid grid;
        private int unvisitedCount;
        private ICell currentCell;

        public List<ICell> CurrentCells { get; } = new List<ICell>();

        public AldousBroder(IGrid grid, int seed = 0)
        {
            this.grid = grid;
            this.rand = new Random(seed);
            this.currentCell = grid.GetRandomCell(seed);
            this.unvisitedCount = grid.Size - 1;

            // Mark starting cell as visited
            this.currentCell.IsVisited = true;
            grid.MarkDirty(currentCell);
        }

        /// <summary>
        /// Generates the full maze at once (in-place)
        /// </summary>
        public IGrid CreateMaze()
        {
            while (unvisitedCount > 0)
            {
                Step();
            }
            return grid; // return the same grid instance
        }

        /// <summary>
        /// Performs a single step of Aldous-Broder algorithm
        /// </summary>
        public bool Step()
        {
            if (unvisitedCount <= 0) return false;

            var neighbours = currentCell.Neighbours;
            if (neighbours.Count == 0) return false;

            var neighbour = neighbours[rand.Next(neighbours.Count)];

            if (neighbour.Links.Count == 0)
            {
                currentCell.Link(neighbour);    // carve passage
                neighbour.IsVisited = true;     // mark for drawing
                grid.MarkDirty(neighbour);      // ensure redraw
                unvisitedCount--;
                CurrentCells.Add(neighbour);
            }
            else
            {
                CurrentCells.Clear();
            }

            currentCell = neighbour;
            return true;
        }
    }
}
