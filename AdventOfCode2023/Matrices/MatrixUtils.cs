namespace NEAConsole.Matrices;

public static class MatrixUtils
{
    static (double val, int row, int col) MinValue(Matrix m, HashSet<int> availableRows, HashSet<int> availableCols)
    {
        double val = double.MaxValue;
        int row = -1, col = -1;
        foreach (int i in availableRows)
        {
            foreach (int j in availableCols)
            {
                if (m[i, j] < val)
                {
                    val = m[i, j];
                    row = i;
                    col = j;
                }
            }
        }

        return (val, row, col);
    }

    /// <summary>
    /// Performs Prim's algorithm to generate the minimum spanning tree of a graph.
    /// </summary>
    /// <param name="adjacency">Adjacency Matrix describing edges of the graph.</param>
    /// <returns>Array of row & column indices representing the selected edges.</returns>
    public static (int row, int col)[] Prims(Matrix adjacency)
    {
        HashSet<int> availableRows = new(Enumerable.Range(1, adjacency.Rows - 1));
        HashSet<int> availableCols = new(adjacency.Columns);

        (int, int)[] selectedNodes = new (int, int)[adjacency.Columns - 1];
        int nodesSelected = 0, row = 0, col;

        while (nodesSelected < selectedNodes.Length)
        {
            availableCols.Add(row);
            (_, row, col) = MinValue(adjacency, availableRows, availableCols);
            availableRows.Remove(row);// prevent cycles
            selectedNodes[nodesSelected++] = (row, col);
        }

        return selectedNodes;
    }

    /// <summary>
    /// Performs Prim's algorithm to generate the minimum spanning tree of a graph, and returns all arcs inside, as well as alternative arcs that could've been used in a tie.
    /// </summary>
    /// <param name="adjacency">Adjacency Matrix describing edges of the graph.</param>
    /// <returns>Array of row & column indices representing the selected edges.</returns>
    public static List<(int row, int col)> PrimsModified(Matrix adjacency)
    {
        HashSet<int> availableRows = new(Enumerable.Range(1, adjacency.Rows - 1));
        HashSet<int> availableCols = new(adjacency.Columns);

        List<(int, int)> returns = new();

        (int, int)[] selectedNodes = new (int, int)[adjacency.Columns - 1];
        int nodesSelected = 0, row = 0, col;

        while (nodesSelected < selectedNodes.Length)
        {
            availableCols.Add(row);
            foreach (var (r, c) in MinValues(adjacency, availableRows, availableCols).edges)
            {
                returns.Add((r, c));
                row = r; col = c;
                selectedNodes[nodesSelected] = (r, c);
            }
            nodesSelected++;
            availableRows.Remove(row);// prevent cycles
        }

        return returns;
    }

    static (double weight, List<(int row, int col)> edges) MinValues(Matrix m, HashSet<int> availableRows, HashSet<int> availableCols)
    {
        double weight = double.MaxValue;
        List<(int row, int col)> mins = [];
        foreach (int i in availableRows)
        {
            foreach (int j in availableCols)
            {
                if (m[i, j] <= weight)
                {
                    weight = m[i, j];
                    mins.Add((i, j));
                }
            }
        }

        return (weight, mins);
    }
}