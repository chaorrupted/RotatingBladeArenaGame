using DG.Tweening;
using UnityEngine;
using Weapons;

namespace Pickup
{
    public class Magnet: MonoBehaviour
    {
        [SerializeField] private SpinningWeaponBelt weaponBelt;
        [SerializeField] private PoolManager poolManager;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.activeInHierarchy)
            {
                return;
            }
            
            if (!this.gameObject.activeInHierarchy)
            {
                return;
            }
            
            if (other.gameObject.CompareTag("PickupRedSword"))
            {
                var pickupComponent = other.gameObject.GetComponent<Pickup>();
                if (pickupComponent.Collected)
                {
                    return;
                }

                pickupComponent.Collected = true;
                
                other.transform.parent = transform;
                other.transform.DOLocalMove(new Vector3(0, 0, 0), 0.4f) // todo: duration
                    .SetEase(Ease.InBack)
                    .OnComplete(
                        () => {
                            poolManager.ReleasePickup(other.gameObject);
                            // todo: play pickup bubble pop particles and sound
                            weaponBelt.AddWeapon(); 
                        });
            }
        }
    }
}