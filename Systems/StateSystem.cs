using MagicCARpet.Contracts.Messages;
using MagicCARpet.Contracts.Models;
using MvvmGen.Events;
using StereoKit;
using StereoKit.Framework;

namespace MagicCARpet.Systems
{
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
            _eventAggregator.Publish(new StateChangedMessage(_currentState));
            return true;
        }

        public void OnEvent(PinchHappenedMessage eventData)
        {
            if (_currentState == States.DrawingRoads)
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
            if (Input.Key(Key.Space).IsJustActive())
            {
                var newState = _currentState == States.DrawingRoads ? States.Running : States.DrawingRoads;
                _currentState = newState;
                _eventAggregator.Publish(new StateChangedMessage(newState));
            }
        }
    }
}
