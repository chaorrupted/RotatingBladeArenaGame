using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Utilities
{
    public class Spinner: MonoBehaviour
    {
        [SerializeField] private float spinDuration = 2f;
        [SerializeField] private bool randomStart = false;
        
        private void Awake()
        {
            if (randomStart)
            {
                transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            }
            StartSpinning();
        }

        private void StartSpinning()
        {
            transform.DORotate(new Vector3(0, 0, -360), spinDuration, RotateMode.FastBeyond360)
                .SetRelative(true)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
        }
    }
}