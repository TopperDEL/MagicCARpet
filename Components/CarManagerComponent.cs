using MagicCARpet.Contracts.Messages;
using MagicCARpet.Contracts.Models;
using MvvmGen.Events;
using StereoKit;
using StereoKit.Framework;
using System;
using System.Linq;

namespace MagicCARpet.Components
{
    internal class CarManagerComponent : IStepper,
        IEventSubscriber<StateChangedMessage>
    {
        private readonly IEventAggregator _eventAggregator;
        private Graph _graph;
        private Node _current;
        private Node _target;
        private Vec3 _position;
        private Model _carModel;
        private bool _active;
        private Random _random = new Random();
        private const float _speed = 0.2f; // meters per second

        public CarManagerComponent(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public bool Enabled => true;

        public bool Initialize()
        {
            _eventAggregator.RegisterSubscriber(this);
            _carModel = Model.FromMesh(Mesh.GenerateRoundedCube(Vec3.One * 0.05f, 0.01f), Material.Default);
            return true;
        }

        public void Shutdown()
        {
            _eventAggregator.UnregisterSubscriber(this);
        }

        public void Step()
        {
            if (!_active || _current == null || _target == null)
                return;

            var dir = _target.Position - _position;
            var distanceThisFrame = _speed * Time.Stepf;
            if (dir.Length() <= distanceThisFrame)
            {
                _position = _target.Position;
                _current = _target;
                if (_current.Connections.Count > 0)
                {
                    _target = _current.Connections[_random.Next(_current.Connections.Count)].Item1;
                }
                else
                {
                    _active = false;
                }
            }
            else
            {
                _position += dir.Normalized * distanceThisFrame;
            }

            _carModel.Draw(Matrix.TS(_position, Vec3.One * 0.05f));
        }

        public void OnEvent(StateChangedMessage eventData)
        {
            if (eventData.State == States.Running)
            {
                _eventAggregator.Publish(new RequestRoadGraphMessage(graph =>
                {
                    _graph = graph;
                    StartCar();
                }));
            }
            else
            {
                _active = false;
            }
        }

        private void StartCar()
        {
            var first = _graph?.Nodes?.FirstOrDefault();
            if (first == null || first.Connections.Count == 0)
            {
                _active = false;
                return;
            }

            _current = first;
            _target = _current.Connections[0].Item1;
            _position = _current.Position;
            _active = true;
        }
    }
}
