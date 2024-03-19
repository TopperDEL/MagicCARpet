using MagicCARpet.Contracts.Models;

namespace MagicCARpet.Services
{
    public class PathFinderService
    {
        public List<Node> FindShortestPath(Node start, Node target, List<Node> nodes)
        {
            Dictionary<Node, float> distance = new Dictionary<Node, float>();
            Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
            List<Node> unvisited = new List<Node>();

            foreach (var node in nodes)
            {
                distance[node] = int.MaxValue;
                previous[node] = null;
                unvisited.Add(node);
            }

            distance[start] = 0;

            while (unvisited.Count > 0)
            {
                Node current = null;
                foreach (var node in unvisited)
                {
                    if (current == null || distance[node] < distance[current])
                    {
                        current = node;
                    }
                }

                unvisited.Remove(current);

                foreach (var (neighbor, weight) in current.Connections)
                {
                    float alt = distance[current] + weight;
                    if (alt < distance[neighbor])
                    {
                        distance[neighbor] = alt;
                        previous[neighbor] = current;
                    }
                }
            }

            List<Node> path = new List<Node>();
            Node temp = target;
            while (temp != null)
            {
                path.Insert(0, temp);
                temp = previous[temp];
            }

            return path;
        }
    }
}
