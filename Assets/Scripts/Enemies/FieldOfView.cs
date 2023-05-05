using UnityEngine;
using UnityEngine.Animations;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Enemies
{
    public class FieldOfView : MonoBehaviour
    {
        public float viewDistance;
        [Range(0, 360)]
        public float viewAngle;
        public bool canSeePlayer;

        private void FixedUpdate()
        {
            FindVisibleTarget();
        }

        private void FindVisibleTarget()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            var targetsInsideSphere = Physics.OverlapSphere(transform.position, viewDistance);

            foreach (var target in targetsInsideSphere)
            {
                if (!target.gameObject.CompareTag("Player")) continue;
                
                var position = transform.position;
                var distanceToTarget = Vector3.Distance(position, target.transform.position);
                var directionToTarget = (target.transform.position - position).normalized;
                var angle = Vector3.Angle(transform.forward, directionToTarget);

                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget)) continue;

                // ReSharper disable once InvertIf
                if (distanceToTarget <= 2 || angle < viewAngle / 2) // We need this because the angle is not enough to detect the player when its near
                {
                    canSeePlayer = true;
                    LookAtPlayer(target.gameObject);
                    return;
                }
            }

            canSeePlayer = false;
        }

        private void LookAtPlayer(GameObject player)
        {
            var direction = player.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * 300); // Same as Lerp
        }
    }
}
