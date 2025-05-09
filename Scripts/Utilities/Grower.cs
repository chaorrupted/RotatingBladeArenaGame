using UnityEngine;
using DG.Tweening;

namespace Utilities
{
    public class Grower: MonoBehaviour
    {
        [SerializeField] private Vector3 targetScale = Vector3.one;
        
        private void OnEnable()
        {
            GrowFromZeroToTargetScale();
        }

        private void GrowFromZeroToTargetScale()
        {
            transform.localScale = new Vector3(0, 0, 0);
            transform.DOScale(targetScale, 0.8f).SetEase(Ease.OutCubic); // todo: adjust duration, ease
        }
    }
}