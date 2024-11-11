using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ObjectPoolingSystem
{
    public class ObjectPool
    {
        private readonly Stack<PoolObject> _passiveObjects = new();
        private readonly HashSet<PoolObject> _activeObjects = new();
        private readonly PoolObject _prefab;
        private readonly int _initialObjectCount;
        
        public int PassiveObjectCount => _passiveObjects.Count;
        public int ActiveObjectCount => _activeObjects.Count;

        public ObjectPool(PoolObject prefab, int initialObjectCount)
        {
            _prefab = prefab;
            _initialObjectCount = initialObjectCount;
        }

        public ObjectPool Initialize(Transform parent)
        {
            for (var i = 0; i < _initialObjectCount; i++)
            {
                var poolObject = Object.Instantiate(_prefab);
                ResetPoolObject(poolObject, parent);
            }

            return this;
        }

        public PoolObject GetPoolObject(Transform parent, Action<PoolObject> getObjectCallback = null, Action<PoolObject> resetObjectCallback = null)
        {
            var poolObject = PassiveObjectCount > 0 ? _passiveObjects.Pop() : Object.Instantiate(_prefab);
            _activeObjects.Add(poolObject);
            poolObject.Initialize(parent, resetObjectCallback);
            getObjectCallback?.Invoke(poolObject);
            return poolObject;
        }

        public void ResetPoolObject(PoolObject poolObject, Transform parent)
        {
            _passiveObjects.Push(poolObject);
            _activeObjects.Remove(poolObject);
            poolObject.Reset(parent);
        }

        public void Reset(Transform parent)
        {
            foreach (var activeObject in _activeObjects)
            {
                ResetPoolObject(activeObject, parent);
            }
        }
    }
}