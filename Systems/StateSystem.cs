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
                _eventAggregator.Publish(new AddRoadNodeMessage(eventData.position));
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
