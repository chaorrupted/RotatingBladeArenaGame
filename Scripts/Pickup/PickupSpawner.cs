using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Pickup
{
    public class PickupSpawner: MonoBehaviour
    {
        [SerializeField] private PoolManager poolManager;
        
        private const int CompletelyStopSpawningThreshold = 50;
        private const int TemporarilySkipSpawningThreshold = 10;
        private const float SquaredMinimumDistance = 2.25f;
        
        private GameObject[] _magnetsInScene;
        private int _spawnedSoFar = 0;
        
        private void Start()
        {
            _magnetsInScene = GameObject.FindGameObjectsWithTag("Magnet");
            StartCoroutine(SpawnRandomPickupsRoutine());
        }

        private IEnumerator SpawnRandomPickupsRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(2 + (Random.value * 3f));

                if (transform.childCount > TemporarilySkipSpawningThreshold)
                {
                    continue;
                }
                
                var count = Random.Range(1, 5);
                for (var i = 0; i < count; i++)
                {
                    SpawnRandomPickup();
                }
                _spawnedSoFar += count;

                if (CompletelyStopSpawningThreshold < _spawnedSoFar)
                {
                    yield break;
                }
            }
        }

        private void SpawnRandomPickup()
        {
            var position = new Vector3();
            do
            {
                position.x = Random.Range(-6, 5) + Random.value;
                position.y = Random.Range(-6, 5) + Random.value;
            } while (IsUndesiredPosition(position));

            SpawnPickupAtPosition(position);
        }

        private bool IsUndesiredPosition(Vector3 position)
        {
            foreach (var magnet in _magnetsInScene)
            {
                var distanceSquared = (magnet.transform.position - position).sqrMagnitude;
                if (distanceSquared < SquaredMinimumDistance)
                {
                    return true;
                }
            }

            return false;
        }

        private void SpawnPickupAtPosition(Vector3 position)
        {
            var pickup = poolManager.GetPickup();
            pickup.transform.parent = transform;
            pickup.transform.position = position;
            var pickupComponent = pickup.GetComponent<Pickup>();
            pickupComponent.Collected = false;
        }

        public void SpawnPickupsNearPosition(Vector3 position, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var offset = Random.onUnitSphere;
                offset.z = 0;
                SpawnPickupAtPosition(position + offset);
            }
        }
    }
}