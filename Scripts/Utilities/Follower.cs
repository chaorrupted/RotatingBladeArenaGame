using Unity.Mathematics;
using UnityEngine;

namespace Utilities
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new(0, 0, -10);  // constants.FollowingOffset
        
        [SerializeField] private float xRange = 6;  // Constants.LevelFollowingBounds
        [SerializeField] private float yRange = 6;

        private void LateUpdate()
        {
            var targetPosition = target.transform.position;
            var selfPosition = transform.position;

            var x = math.abs(targetPosition.x) < xRange ? targetPosition.x : selfPosition.x;
            var y = math.abs(targetPosition.y) < yRange ? targetPosition.y : selfPosition.y;
            
            transform.position = new Vector3(x, y, targetPosition.z) + offset;
        }
    }

}