using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
    public class RandomShuffler: MonoBehaviour
    {
        private void Start()
        {
            ShuffleChildLocalPositions();
        }

        private void ShuffleChildLocalPositions()
        {
            foreach (Transform tile in transform)
            {
                var randomVector2 = Random.insideUnitCircle * Random.Range(1.5f, 17f);
                tile.position = new Vector3(randomVector2.x, randomVector2.y, 0);
                // ideally should prevent placing on top of one another ...
            }
        }
    }
}