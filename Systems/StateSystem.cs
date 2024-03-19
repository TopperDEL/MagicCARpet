using MagicCARpet.Contracts.Messages;
using MvvmGen.Events;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCARpet.Systems
{
    public enum States
    {
        DrawingRoads,
        Running
    }

    internal class StateSystem : IStepper, IEventSubscriber<PinchHappenedMessage>
    {
        private readonly IEventAggregator _eventAggregator;
        private States _currentState;

        public StateSystem(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public bool Enabled => true;

        public bool Initialize()
        {
            _eventAggregator.RegisterSubscriber(this);
            return true;
        }

        public void OnEvent(PinchHappenedMessage eventData)
        {
            if(_currentState == States.DrawingRoads)
            {
                bool gotTriggered = false;
                //Check if the pinch happened on a road node
                _eventAggregator.Publish(new CheckRoadNodeTriggeredMessage(eventData.Position, (nodeGotTriggered, nodeId) =>
                {
                    gotTriggered = nodeGotTriggered;

                    _eventAggregator.Publish(new RoadNodeGotTriggeredMessage(nodeId));
                }));

                if (!gotTriggered)
                {
                    _eventAggregator.Publish(new AddRoadNodeMessage(eventData.Position));
                }
            }
        }

        public void Shutdown()
        {
            _eventAggregator.UnregisterSubscriber(this);
        }

        public void Step()
        {
        }
    }
}
