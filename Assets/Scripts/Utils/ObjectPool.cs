using System.Collections.Generic;

namespace GameProject.TrickyTowers.Utils
{
    public interface IPoolableItem
    {
        event System.Action<IPoolableItem> OnDisabled;
        void ResetToDefault();
        void Activate();
    }

    public interface IObjectPool<T> where T : class, IPoolableItem
    {
        T GetObject();
        void Recycle(IPoolableItem obj);
    }

    public class ObjectPool<T> : IObjectPool<T> where T : class, IPoolableItem
    {
        public delegate T CreatePoolableObjectDelegate();

        private readonly List<T> _createdObjects;
        private readonly Queue<T> _recycledObjects;
        private readonly CreatePoolableObjectDelegate _factoryMethod;

        public ObjectPool(CreatePoolableObjectDelegate factoryMethod)
        {
            _createdObjects = new List<T>();
            _recycledObjects = new Queue<T>();
            _factoryMethod = factoryMethod;
        }

        public T GetObject()
        {
            var obj = _recycledObjects.Count > 0 ? _recycledObjects.Dequeue() : null;
            if (obj != null)
            {
                obj.ResetToDefault();
                obj.Activate();
                return obj;
            }

            _createdObjects.Add(obj);
            obj = _factoryMethod();
            obj.Activate();
            obj.OnDisabled += Recycle;
            return obj;
        }

        public void Recycle(IPoolableItem obj)
        {
            _recycledObjects.Enqueue((T) obj);
        }
    }
}