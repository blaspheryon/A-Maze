using System.ComponentModel;

namespace AMazer.Algorithms.Generation;

/// <summary>An enum to represent all the maze generation algorithms in <see cref="Generation"/></summary>
public enum GenerationAlgorithms
{
    [Description("Aldous Broder")]
    AldousBroder,

    [Description("Binary Tree")]
    BinaryTree,

    [Description("Hunt and Kill")]
    HuntAndKill,

    [Description("Recursive Backtracker")]
    RecursiveBacktracker,

    [Description("Sidewinder")]
    Sidewinder,
}
