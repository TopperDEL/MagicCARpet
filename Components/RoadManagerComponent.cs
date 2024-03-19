using MagicCARpet.Contracts.Messages;
using MagicCARpet.Contracts.Models;
using MvvmGen.Events;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCARpet.Components
{
    internal class RoadManagerComponent : IStepper, IEventSubscriber<AddRoadNodeMessage>
    {
        private Graph _roadGraph;
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
            return true;
        }

        public void Shutdown()
        {
            _eventAggregator.UnregisterSubscriber(this);
        }

        public void Step()
        {
        }

        private Node _previousNode;

        public void OnEvent(AddRoadNodeMessage eventData)
        {
            var createdNode = _roadGraph.AddNode(Guid.NewGuid().ToString(), eventData.position);

            if (_previousNode != null)
            {
                _roadGraph.AddConnection(_previousNode, createdNode, (eventData.position - _previousNode.Position).Length());
            }

            _previousNode = createdNode;
        }
    }
}
