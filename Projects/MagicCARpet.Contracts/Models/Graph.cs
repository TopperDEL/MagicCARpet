using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MagicCARpet.Contracts.Models
{
    public class Graph
    {
        private List<Node> nodes;

        public Graph()
        {
            nodes = new List<Node>();
        }

        public Node AddNode(string id, Vector3 position)
        {
            var node = new Node(id, position);
            nodes.Add(node);

            return node;
        }

        public void AddConnection(Node source, Node target, float weight)
        {
            source.Connections.Add((target, weight));
        }

        public IEnumerable<Node> Nodes => nodes;
    }
}
