using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolingSystem
{
    public class ObjectPoolManager : MonoBehaviour
    {
        [SerializeField] private Transform pooledObjectsParent;
        [SerializeField] private List<PoolObject> poolItemPrefabs;

        private readonly Dictionary<PoolObjectType, ObjectPool> _poolTypeTpPoolItemDictionary = new();

        public void Awake()
        {

            Initialize();
        }
 
        public void Initialize(int initialObjectCount = 0)
        {
            CreatePools(initialObjectCount);
        }

        private void CreatePools(int initialObjectCount)
        {
            foreach (var poolObject in poolItemPrefabs)
            {
                var objectPool = new ObjectPool(poolObject, initialObjectCount).Initialize(pooledObjectsParent);
                _poolTypeTpPoolItemDictionary.Add(poolObject.poolObjectType, objectPool);
            }
        }

        public PoolObject GetObject(PoolObjectType poolObjectType, Transform parent = null)
        {
            return _poolTypeTpPoolItemDictionary[poolObjectType].GetPoolObject(parent);
        }

        public void ResetObject(PoolObject poolObject, Transform parent = null)
        {
            var pool = _poolTypeTpPoolItemDictionary[poolObject.poolObjectType];
            pool.ResetPoolObject(poolObject, parent == null ?  pooledObjectsParent : parent);
        }

        public void ResetPools(Transform parent = null)
        {
            foreach (var poolTypeToPoolItem in _poolTypeTpPoolItemDictionary)
            {
                poolTypeToPoolItem.Value.Reset(parent == null ?  pooledObjectsParent : parent);
            }
        }

        public int GetActiveObjectCountOfPool(PoolObjectType poolObjectType) => _poolTypeTpPoolItemDictionary[poolObjectType].ActiveObjectCount;
    }
}