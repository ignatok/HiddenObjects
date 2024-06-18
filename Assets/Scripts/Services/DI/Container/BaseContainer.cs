using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.DI.Container
{
    public class BaseContainer : IContainer
    {
        private readonly Dictionary<Type, IContainerMember> _instances = new Dictionary<Type, IContainerMember>();
        private readonly List<ITickable> _tickables = new List<ITickable>();

        public void Initialization()
        {
            foreach (var instancesValue in _instances.Values)
            {
                instancesValue.Initialization();
            }
        }

        public void Dispose()
        {
            foreach (var instancesValue in _instances.Values)
            {
                instancesValue.Dispose();
            }
        }

        public void Tick()
        {
            foreach (var tickable in _tickables)
            {
                tickable.Tick();
            }
        }

        public T GetInstance<T>() where T : IContainerMember
        {
            if (_instances.ContainsKey(typeof(T)))
            {
                return (T) _instances[typeof(T)];
            }

            Debug.LogError($"Can't find instance of type= {typeof(T).Name}. Check all installers and their order");
            return default;
        }

        public void AddInstance(IContainerMember instance)
        {
            _instances[instance.GetType()] = instance;

            if (instance is ITickable tickable)
            {
                _tickables.Add(tickable);
            }
        }

        public void RemoveInstance(IContainerMember instance)
        {
            var key = instance.GetType();

            if (_instances.ContainsKey(key))
                _instances.Remove(key);

            if (instance is ITickable tickable)
            {
                _tickables.Remove(tickable);
            }
        }
    }
}