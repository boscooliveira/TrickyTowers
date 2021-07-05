using System;
using System.Collections.Generic;
using GameProject.TrickyTowers.Engine;

namespace GameProject.TrickyTowers.Service
{
    public class ServiceFactory
    {
        public Dictionary<Type, object> _services;

        private static ServiceFactory _instance;
        public static ServiceFactory Instance
        {
            get
            {
                return _instance ??= new ServiceFactory();
            }
        }

        private ServiceFactory()
        {
            _services = new Dictionary<Type, object>();
        }

        public void Register<T>(T obj) where T : IService
        {
            Type type = typeof(T);
            if (!type.IsInterface)
            {
                GameLogger.LogError($"[ServiceFactory::Register] Could not register object {obj} of type {type.FullName} : Interface expected");
                return;
            }
            if (_services.ContainsKey(type))
            {
                GameLogger.LogError($"[ServiceFactory::Register] {type.FullName} already registered");
                return;
            }
            _services[type] = obj;
        }

        public T Resolve<T>(bool instantiateIfNull = false) where T : IService
        {
            Type type = typeof(T);
            _services.TryGetValue(type, out var service);

            if (service == null && instantiateIfNull)
            {
                service = Activator.CreateInstance(type);
                _services[type] = service;
            }

            return (T)service;
        }

        public void DisposeAndReset()
        {
            foreach (var service in _services.Values)
            {
                if (service is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            _services.Clear();
        }
    }

    public interface IService
    {
    }
}
