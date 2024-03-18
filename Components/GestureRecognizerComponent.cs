using MagicCARpet.Contracts.Messages;
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
    internal class GestureRecognizerComponent : IStepper
    {
        private IEventAggregator _eventAggregator;

        public GestureRecognizerComponent(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public bool Enabled => true;

        public bool Initialize()
        {
            return true;
        }

        public void Shutdown()
        {
        }

        public void Step()
        {
            var leftHand = Input.Hand(Handed.Left);
            if (leftHand.IsJustPinched)
            {
                _eventAggregator.Publish(new PinchHappenedMessage(leftHand.Get(FingerId.Index, JointId.Tip).position));
            }

            var rightHand = Input.Hand(Handed.Right);
            if (rightHand.IsJustPinched)
            {
                _eventAggregator.Publish(new PinchHappenedMessage(rightHand.Get(FingerId.Index, JointId.Tip).position));
            }
        }
    }
}
