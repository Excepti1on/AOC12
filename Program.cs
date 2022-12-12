string[] input = File.ReadAllLines("input.txt");

Graph graph = new(input.Length * input[0].Length);
int s = 0;
int e = 0;
input = input.Select(x => x.Replace('S', (char)('a' - 1)).Replace('E', (char)('z' + 1))).ToArray();
for (int i = 0; i < input.Length; i++)
{
    for (int j = 0; j < input[i].Length; j++)
    {
        s = input[i][j] == (char)('a' - 1) ? j + i * input[i].Length : s;
        e = input[i][j] == (char)('z' + 1) ? j + i * input[i].Length : e;
        CalculateNeighbors(i, j, input, input[i].Length, graph, 1);
    }
}
graph.PrintShortestDistance(s, e, null);

Graph graph2 = new(input.Length * input[0].Length);
List<int> aLocations = new();
for (int i = 0; i < input.Length; i++)
{
    for (int j = 0; j < input[i].Length; j++)
    {
        s = input[i][j] == (char)('z' + 1) ? j + i * input[i].Length : s;
        if (input[i][j] == 'a')
        {
            aLocations.Add(j + i * input[i].Length);
        }
        CalculateNeighbors(i, j, input, input[i].Length, graph2, 2);
    }
}
graph2.PrintShortestDistance(s, null, aLocations);

static void CalculateNeighbors(int i, int j, string[] input, int rowLength, Graph graph, int round)
{
    char fromChar;
    fromChar = input[i][j];

    if (i > 0 && input[i - 1][j] - fromChar < 2)
    {
        int from = j + i * rowLength;
        int to = j + (i - 1) * rowLength;
        if (round == 1)
            graph.AddEdge(from, to);
        else
            graph.AddEdge(to, from);
    }
    if (i < input.Length - 1 && input[i + 1][j] - fromChar < 2)
    {
        int from = j + i * rowLength;
        int to = j + (i + 1) * rowLength;
        if (round == 1)
            graph.AddEdge(from, to);
        else
            graph.AddEdge(to, from);
    }
    if (j > 0 && input[i][j - 1] - fromChar < 2)
    {
        int from = j + i * rowLength;
        int to = j - 1 + i * rowLength;
        if (round == 1)
            graph.AddEdge(from, to);
        else
            graph.AddEdge(to, from);
    }
    if (j < input[i].Length - 1 && input[i][j + 1] - fromChar < 2)
    {
        int from = j + i * rowLength;
        int to = j + 1 + i * rowLength;
        if (round == 1)
            graph.AddEdge(from, to);
        else
            graph.AddEdge(to, from);
    }
}

class Graph
{
    private readonly int vertecies;
    private readonly List<List<int>> adjecencyList;
    public Graph(int vertecies)
    {
        this.vertecies = vertecies;
        adjecencyList = new();
        for (int i = 0; i < vertecies; i++)
        {
            adjecencyList.Add(new List<int>());
        }
    }
    public void AddEdge(int from, int to)
    {
        adjecencyList[from].Add(to);
    }
    public void PrintShortestDistance(int src, int? dest, List<int>? aLocations)
    {
        int[] dist = new int[vertecies];
        if (BFS(src, dest, dist, aLocations) == false)
        {
            Console.WriteLine("Given source and destination are not connected");
            return;
        }
        if (dest != null)
        {
            Console.WriteLine("Shortest path length is: " + dist[(int)dest]);
            return;
        }
        if (aLocations != null)
        {
            List<int> distances = new();
            foreach (int i in aLocations)
            {
                distances.Add(dist[i]);
            }
            Console.WriteLine("Shortest path length is: " + distances.Min());
            return;
        }
    }
    private bool BFS(int src, int? dest, int[] dist, List<int>? aLocations)
    {
        Queue<int> queue = new();
        bool[] visited = Enumerable.Repeat(false, vertecies).ToArray();
        for (int i = 0; i < vertecies; i++)
        {
            dist[i] = int.MaxValue;
        }
        visited[src] = true;
        dist[src] = 0;
        queue.Enqueue(src);
        while (queue.Count != 0)
        {
            int previousVertex = queue.Dequeue();

            foreach (int vertex in adjecencyList[previousVertex])
            {
                if (visited[vertex] == false)
                {
                    visited[vertex] = true;
                    dist[vertex] = dist[previousVertex] + 1;
                    queue.Enqueue(vertex);

                    if (dest != null && vertex == dest)
                        return true;
                    if (aLocations != null && aLocations.Contains(vertex))
                        return true;
                }
            }
        }
        return false;
    }
}