using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MagicCARpet.Contracts.Models
{
    public class Node
    {
        public string Id { get; }
        public Vector3 Position { get; set; }
        public List<(Node, float)> Connections { get; } // (Zielknoten, Kantengewichtung)

        public Node(string id, Vector3 position)
        {
            Id = id;
            Connections = new List<(Node, float)>();
            Position = position;
        }
    }
}
