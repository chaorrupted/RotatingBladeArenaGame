using System.Collections;
using Pickup;
using ScratchCardAsset;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Character
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] protected ScratchCardManager cardManager;
        [SerializeField] private float movementSpeed = 5f;  // constants.characterMovementSpeed
        [SerializeField] private float xBound = 7;  // Constants.LevelMovementBounds
        [SerializeField] private float yBound = 7;
        [SerializeField] private Magnet magnet;
        [SerializeField] private GameObject flashSprite;
        
        [SerializeField] protected PickupSpawner pickupSpawner;
        [SerializeField] protected SpinningWeaponBelt weaponBelt;
        
        [SerializeField] public int scratcherID = 0;  // must be unique, and 0, 1, 2... across program.
                                                      // todo: scratchID generator
        
        private const int DefaultWeaponDropAmount = 2;
        
        protected Vector2 MovementVector = Vector2.zero;

        protected const float MaximumSpeedMovementVectorMagnitude = 200f;
        protected const int MaxHealth = 30;
        protected int Health { get; private set; } = MaxHealth;
     
        private void Start()
        {
            Vector2 position = cardManager.MainCamera.WorldToScreenPoint(transform.position);
            cardManager.Card.Input.Register(scratcherID, position);
        }
        
        private void LateUpdate()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            Vector2 position = cardManager.MainCamera.WorldToScreenPoint(transform.position);
            cardManager.Card.Input.Scratch(scratcherID, position);
        }
        
        private void Update()
        {
            // if (GameManager.paused) return; ????

            CalculateMovementVector();
            Move();
        }

        protected abstract void CalculateMovementVector();

        private void Move()
        {
            if (MovementVector == Vector2.zero){ return;}
        
            var magnitude = MovementVector.magnitude;
            float speedUtilization;
            if (magnitude > MaximumSpeedMovementVectorMagnitude)
            {
                speedUtilization = 1;
            }
            else
            {
                speedUtilization = magnitude / MaximumSpeedMovementVectorMagnitude;
            }
            var xyMovement = MovementVector.normalized * (movementSpeed * speedUtilization * Time.deltaTime);
            
            var position = transform.position;
            float x, y;
            if (position.x + xyMovement.x > xBound)
            {
                x = xBound;
            }
            else if (position.x + xyMovement.x < - xBound)
            {
                x = - xBound;
            }
            else
            {
                x = position.x + xyMovement.x;
            }
            
            if (position.y + xyMovement.y > xBound)
            {
                y = yBound;
            }
            else if (position.y + xyMovement.y < - yBound)
            {
                y = - yBound;
            }
            else
            {
                y = position.y + xyMovement.y;
            }
            
            transform.position = new Vector3(x, y, position.z);
        }

        private void TakeDamage(GameObject damagingObject)
        {
            StartCoroutine(DamageFlashRoutine());
            Health -= 10;
            if (Health <= 0)
            {
                Die();
                return;
            }

            // pushback from weapon position, white flash, etc.
        }

        private IEnumerator DamageFlashRoutine()
        {
            flashSprite.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            flashSprite.SetActive(false);
        }

        private void Die()
        {
            var weaponDropAmount = DefaultWeaponDropAmount + weaponBelt.GetWeaponCount();
            magnet.enabled = false;
            pickupSpawner.SpawnPickupsNearPosition(transform.position, weaponDropAmount);
            
            weaponBelt.ReleaseAllWeaponsOnDeath();
            
            // tween to shrink while spinning
            
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.activeInHierarchy)
            {
                return;
            }

            var otherGameObject = other.gameObject;
            if (otherGameObject.CompareTag("RedSword"))
            {
                var meleeWeaponComponent = otherGameObject.GetComponent<MeleeWeapon>();

                if (meleeWeaponComponent.ParentWeaponBeltID == 0)
                {
                    return;
                }

                if (meleeWeaponComponent.ParentWeaponBeltID == weaponBelt.id)
                {
                    return;
                }
                
                TakeDamage(otherGameObject);
            }
        }
    }
}
