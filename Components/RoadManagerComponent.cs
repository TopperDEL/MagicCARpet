using MagicCARpet.Contracts.Messages;
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
        private readonly IEventAggregator _eventAggregator;

        public RoadManagerComponent(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }


        public bool Enabled => true;

        public bool Initialize()
        {
            _eventAggregator.RegisterSubscriber(this);
            return true;
        }

        public void Shutdown()
        {
            _eventAggregator.UnregisterSubscriber(this);
        }

        public void Step()
        {
        }

        public void OnEvent(AddRoadNodeMessage eventData)
        {
        }
    }
}
