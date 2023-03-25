using UnityEngine;

namespace Weapons
{
    public class GunMovement : MonoBehaviour
    {
        public float value = 0.1f;
        public float maxValue = 0.6f;
        public float smoothValue = 6;
        private Vector3 _initialPosition;

        // Start is called before the first frame update
        private void Start()
        {
            _initialPosition = transform.localPosition;
        }

        // Update is called once per frame
        private void Update()
        {
            var xMovement = Input.GetAxis("MouseX") * value;
            var yMovement = Input.GetAxis("MouseY") * value;

            xMovement = Mathf.Clamp(xMovement, -maxValue, maxValue);
            yMovement = Mathf.Clamp(yMovement, -maxValue, maxValue);

            var targetPosition = new Vector3(xMovement, yMovement, 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition + _initialPosition, Time.deltaTime * smoothValue);
        }
    }
}