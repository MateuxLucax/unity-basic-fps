using UnityEngine;

namespace Hero
{
    public class Breathing : MonoBehaviour
    {
        public float minHeight = -0.035f;
        public float maxHeight = 0.035f;

        [Range(0.0f, 5.0f)] public float breathingPower = 1.0f;

        private bool _isBreathing = true;

        private float _movement;

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (_isBreathing)
            {
                _movement = Mathf.Lerp(_movement, maxHeight, Time.deltaTime * breathingPower);
                transform.localPosition = new Vector3(transform.localPosition.x, _movement, transform.localPosition.z);

                if (_movement >= maxHeight - 0.01f) _isBreathing = false;
            }
            else
            {
                _movement = Mathf.Lerp(_movement, minHeight, Time.deltaTime * breathingPower);
                transform.localPosition = new Vector3(transform.localPosition.x, _movement, transform.localPosition.z);

                if (_movement <= minHeight + 0.01f) _isBreathing = true;
            }

            // Different breathing power depending if the player is running or not
            if (breathingPower > 1) breathingPower = Mathf.Lerp(breathingPower, 1, Time.deltaTime * 0.2f);
        }
    }
}