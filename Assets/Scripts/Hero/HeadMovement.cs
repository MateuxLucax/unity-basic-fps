using UnityEngine;

namespace Hero
{
    public class HeadMovement : MonoBehaviour
    {
        public float speed = 0.1f;
        public float power = 0.2f;
        public float originPoint;

        private Vector3 positionSave;

        private float _time;

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            var waveCut = 0.0f;
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            positionSave = transform.localPosition;

            if (horizontal == 0 && vertical == 0)
            {
                _time = 0.0f;
            }
            else
            {
                waveCut = Mathf.Sin(_time);
                _time += speed;

                if (waveCut > Mathf.PI * 2) _time -= Mathf.PI * 2;
            }

            if (waveCut != 0)
            {
                var changeMovement = waveCut * power;
                var totalAxis = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                totalAxis = Mathf.Clamp(totalAxis, 0.0f, 1.0f);
                changeMovement = totalAxis * changeMovement;
                positionSave.y = originPoint + changeMovement;
            }
            else
            {
                positionSave.y = originPoint;
            }
        }
    }
}