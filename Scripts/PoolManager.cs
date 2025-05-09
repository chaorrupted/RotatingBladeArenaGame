using UnityEngine;
using Utilities;

public class PoolManager: MonoBehaviour
{
    [SerializeField] private GameObject redSwordPrefab;
    [SerializeField] private GameObject pickupRedSwordPrefab;

    private GameObjectPool _redSwordPool;
    private GameObjectPool _pickupRedSwordPool;
    

    private void Awake()
    {
        _redSwordPool = new GameObjectPool(redSwordPrefab);
        _pickupRedSwordPool = new GameObjectPool(pickupRedSwordPrefab);
    }

    public GameObject GetRedSword()
    {
        return _redSwordPool.GetObject();
    }

    public void ReleaseRedSword(GameObject sword)
    {
        sword.transform.parent = this.transform;
        _redSwordPool.ReleaseObject(sword);
    }
    
    public GameObject GetPickup()
    {
        return _pickupRedSwordPool.GetObject();
    }

    public void ReleasePickup(GameObject pickup)
    {
        pickup.transform.parent = this.transform;
        _pickupRedSwordPool.ReleaseObject(pickup);
    }
}