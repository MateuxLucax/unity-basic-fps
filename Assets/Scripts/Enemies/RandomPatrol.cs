using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class RandomPatrol : MonoBehaviour
    {
        private NavMeshAgent _agent;
        public float range;
        private float _time;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _time = 0;
        }

        private static bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            var randomPoint = center + Random.insideUnitSphere * range;

            if (NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }

            result = Vector3.zero;
            return false;
        }

        public void Walk()
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance || (_time >= 6.0f))
            {
                if (!RandomPoint(transform.position, range, out var point)) return;

                _agent.SetDestination(point);
                transform.LookAt(point);
                _time = 0;
            } else
            {
                _time += Time.deltaTime;
            }
        }
    }
}
