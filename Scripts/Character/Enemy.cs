using System;
using System.Collections;
using System.Collections.Generic;
using Pickup;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character
{
    public class Enemy: Character
    {
        [SerializeField] private List<Transform> otherCharacterTransforms;
        
        private enum Mode
        {
            CollectWeapons,
            Fight,
            Chill
        }

        private Mode _currentMode;

        private void Awake()
        {
            StartCoroutine(ModeChangeRoutine());
            StartCoroutine(MovementVectorDecisionRoutine());
        }

        private IEnumerator MovementVectorDecisionRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
                
                DecideMovementVectorForNextSeconds();

                if (!this.gameObject.activeInHierarchy)
                {
                    yield break;
                }
            }
        }

        private IEnumerator ModeChangeRoutine()
        {
            while (true)
            {
                DecideEnemyMode();

                yield return new WaitForSeconds(2f + Random.value * 5);

                if (!this.gameObject.activeInHierarchy)
                {
                    yield break;
                }
            }
        }

        private void DecideEnemyMode()
        {
            if (weaponBelt.GetWeaponCount() < 3)
            {
                _currentMode = Mode.CollectWeapons;
                return;
            }

            if (weaponBelt.GetWeaponCount() > 6)
            {
                _currentMode = Mode.Fight;
                return;
            }

            _currentMode = Mode.Chill; // todo: probabilistic modes
            // mode: chill:
            // health full: 70% walk around chilling, 20% fight 10% drops
            // health low: 70% walk away from enemies, 20% drops 10% walk randomly
        }


        protected override void CalculateMovementVector()
        {
            // todo: set "target movement vector" from DecideMovementVectorForNextSeconds. slowly change movement vector.
            // less snappy and more natural movement for enemies
            return;
        }

        private void DecideMovementVectorForNextSeconds()
        {
            // mode: weapons:
            // full health: walk to closest weapon pickup.
            // health low: walk to closest pickup that does not have an enemy between me and pickup (if none, go for nearest)
            
            // mode: fights:
            // full health: walk to nearest enemy  Todo: maybe instead nearest enemy in "vision range" and random otherwise.
            // health low: walk to nearest enemy that has less weapons than me (if none, go for weapons instead)
            var lowHealth = (MaxHealth / 2) > Health;
            switch ((_currentMode, lowHealth))
            {
                case (Mode.CollectWeapons, lowHealth: true):
                    MovementVector = FindNearestSafePickup();
                    break;
                case (Mode.CollectWeapons, lowHealth: false):
                    MovementVector = FindNearestPickup();
                    break;
                case (Mode.Fight, lowHealth: true):
                    MovementVector =  FindNearestWeakEnemy();
                    break;
                case (Mode.Fight, lowHealth: false):
                    MovementVector =  FindNearestEnemy();
                    break;
                case (Mode.Chill, _):
                    MovementVector = RandomMovementVector(); 
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Vector2 FindNearestWeakEnemy()
        {
            return FindNearestEnemy(); // TODO
        }

        private Vector2 FindNearestEnemy()
        {
            var selfPosition = transform.position;
            float minimumDistance = 100000f;
            Vector3 nearestEnemyPosition = Vector3.zero;
            
            foreach (var enemyTransform in otherCharacterTransforms)
            {
                if (!enemyTransform.gameObject.activeInHierarchy)
                {
                    continue;
                }
                
                var enemyPosition = enemyTransform.position;
                var distanceSquared = (enemyPosition - selfPosition).sqrMagnitude;
                if (distanceSquared < minimumDistance)
                {
                    minimumDistance = distanceSquared;
                    nearestEnemyPosition = enemyPosition;
                }
            }

            if (nearestEnemyPosition == Vector3.zero)
            {
                return RandomMovementVector();
            }
            
            var movementVector3 = nearestEnemyPosition - selfPosition;
            var magnitude = MaximumSpeedMovementVectorMagnitude * Random.Range(0.5f, 1f);
            return new Vector2(movementVector3.x, movementVector3.y).normalized * magnitude;
        }


        private Vector2 FindNearestPickup()
        {
            if (pickupSpawner.transform.childCount == 0)
            {
                return RandomMovementVector();
            }
            
            var selfPosition = transform.position;
            float minimumDistance = 100000f;
            Vector3 bestPickupPosition = Vector3.zero;
            
            foreach (Transform pickup in pickupSpawner.transform)
            {
                var pickupPosition = pickup.position;
                var distanceSquared = (pickupPosition - selfPosition).sqrMagnitude;
                if (distanceSquared < minimumDistance)
                {
                    minimumDistance = distanceSquared;
                    bestPickupPosition = pickupPosition;
                }
            }

            var movementVector3 = bestPickupPosition - selfPosition;
            var magnitude = MaximumSpeedMovementVectorMagnitude * Random.Range(0.5f, 1f);
            return new Vector2(movementVector3.x, movementVector3.y).normalized * magnitude;
        }
        
        private Vector2 FindNearestSafePickup()
        {
            return FindNearestPickup(); // todo: delete after completing isSafe method
            
            if (pickupSpawner.transform.childCount == 0)
            {
                return RandomMovementVector();
            }
            
            var selfPosition = transform.position;
            List<(float, Vector3)> distancesToPositions = new();
            
            foreach (Transform pickup in pickupSpawner.transform)
            {
                var pickupPosition = pickup.position;
                var distanceSquared = (pickupPosition - selfPosition).sqrMagnitude;

                distancesToPositions.Add((distanceSquared, pickupPosition));
            }
            distancesToPositions.Sort((x, y) => x.Item1.CompareTo(y.Item1));

            Vector3 targetPickupPosition = Vector3.zero;
            var count = distancesToPositions.Count;
            for (int i = 0; i < count; i++)
            {
                if (IsSafe(distancesToPositions[i].Item2))
                {
                    targetPickupPosition = distancesToPositions[i].Item2;
                    break;
                }

                if (i == count-1)
                {
                    // none are safe :( take closest
                    targetPickupPosition = distancesToPositions[0].Item2;
                }
            }

            var movementVector3 = targetPickupPosition - selfPosition;
            var magnitude = MaximumSpeedMovementVectorMagnitude * Random.Range(0.5f, 1f);
            return new Vector2(movementVector3.x, movementVector3.y).normalized * magnitude;
        }

        private bool IsSafe(Vector3 item2)
        {
            return true; // TODO
        }
        
        
        private static Vector2 RandomMovementVector()
        {
            var magnitude = MaximumSpeedMovementVectorMagnitude * Random.Range(0.5f, 1f);
            return Random.insideUnitCircle.normalized * magnitude;
        }
    }
}