using UnityEngine;
using UnityEngine.Pool;

namespace Utilities
{
    public class GameObjectPool
    //adapted from https://medium.com/@HMarcusWilliamson/how-to-object-pooling-with-unitys-objectpool-class-aa41dfb1bdad
    {
        private readonly GameObject _prefabToPool;
        private readonly ObjectPool<GameObject> _pool;

        public GameObjectPool(GameObject prefabToPool, int defaultSize = 20, int maxSize = 100)
        {
            _prefabToPool = prefabToPool;

            _pool = new ObjectPool<GameObject>(
                CreatePooledObject,
                OnGetFromPool,
                OnReturnToPool,
                OnDestroyPooledObject,
                true,
                defaultSize,
                maxSize
            );
        }

        public GameObject GetObject()
        {
            return _pool.Get();
        }
        
        public void ReleaseObject(GameObject obj)
        {
            _pool.Release(obj);
        }
        
        private GameObject CreatePooledObject()
        {
            GameObject newObject = GameObject.Instantiate(_prefabToPool);
            return newObject;
        }
        
        private static void OnGetFromPool(GameObject pooledObject)
        {
            pooledObject.SetActive(true);
        }
        
        private static void OnReturnToPool(GameObject pooledObject)
        {
            pooledObject.SetActive(false);
        }
        
        private static void OnDestroyPooledObject(GameObject pooledObject)
        {
            GameObject.Destroy(pooledObject);
        }
    }
}