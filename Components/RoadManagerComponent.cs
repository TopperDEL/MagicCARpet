using MagicCARpet.Contracts.Messages;
using MagicCARpet.Contracts.Models;
using MvvmGen.Events;
using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCARpet.Components
{
    internal class RoadManagerComponent : IStepper,
        IEventSubscriber<AddRoadNodeMessage>,
        IEventSubscriber<CheckRoadNodeTriggeredMessage>,
        IEventSubscriber<RoadNodeGotTriggeredMessage>
    {
        private Graph _roadGraph;
        private Model _cube;
        private Model _highlightedCube;
        private readonly IEventAggregator _eventAggregator;

        public RoadManagerComponent(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }


        public bool Enabled => true;

        public bool Initialize()
        {
            _eventAggregator.RegisterSubscriber(this);
            _roadGraph = new Graph();

            _cube = Model.FromMesh(Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f), Material.UI);
            _highlightedCube = Model.FromMesh(Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f), Material.UIBox);

            return true;
        }

        public void Shutdown()
        {
            _eventAggregator.UnregisterSubscriber(this);
        }

        public void Step()
        {
            foreach (var node in _roadGraph.Nodes)
            {
                if (_previousNode != null && node.Id == _previousNode.Id)
                {
                    _highlightedCube.Draw(new Pose(node.Position).ToMatrix());
                }
                else
                {
                    _cube.Draw(new Pose(node.Position).ToMatrix());
                }

                foreach (var connection in node.Connections)
                {
                    //Draw a line between the nodes
                    Lines.Add(connection.Item1.Position, node.Position, Color.Black, 1 * U.cm);
                }
            }
        }

        private Node _previousNode;

        public void OnEvent(AddRoadNodeMessage eventData)
        {
            var createdNode = _roadGraph.AddNode(Guid.NewGuid().ToString(), eventData.Position);

            if (_previousNode != null)
            {
                _roadGraph.AddConnection(_previousNode, createdNode, (eventData.Position - _previousNode.Position).Length());
            }

            _previousNode = createdNode;
        }

        public void OnEvent(CheckRoadNodeTriggeredMessage eventData)
        {
            foreach (var node in _roadGraph.Nodes)
            {
                if ((node.Position - eventData.Position).Length() < 0.1f)
                {
                    eventData.ResultFunction(true, node.Id);
                    return;
                }
            }
        }

        public void OnEvent(RoadNodeGotTriggeredMessage eventData)
        {
            _previousNode = _roadGraph.Nodes.First(n => n.Id == eventData.NodeId);
        }
    }
}
