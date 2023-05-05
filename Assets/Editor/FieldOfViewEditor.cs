using Enemies;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : UnityEditor.Editor
    {
        private GameObject[] _player;

        private void Start()
        {
            _player = GameObject.FindGameObjectsWithTag("Player");
        }

        private void OnSceneGUI()
        {
            var fov = (FieldOfView)target;
            Handles.color = Color.white;
            var position = fov.transform.position;
            Handles.DrawWireArc(position, Vector3.up, Vector3.forward, 360, fov.viewDistance);

            var eulerAngles = fov.transform.eulerAngles;
            var ang01 = DirectionOfAGivenAngle(eulerAngles.y, -fov.viewAngle / 2);
            var ang02 = DirectionOfAGivenAngle(eulerAngles.y, fov.viewAngle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(position, position + ang01 * fov.viewDistance);
            Handles.DrawLine(position, position + ang02 * fov.viewDistance);

            if (!fov.canSeePlayer) return;

            Handles.color = Color.green;
            Handles.DrawLine(position, _player[0].transform.position);
        }

        private static Vector3 DirectionOfAGivenAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}
