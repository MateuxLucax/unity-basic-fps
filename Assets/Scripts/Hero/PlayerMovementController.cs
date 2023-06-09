using UnityEngine;
using UnityEngine.UI;

namespace Hero
{
    public class PlayerMovementController : MonoBehaviour
    {
        public CharacterController controller;
        public float speed = 60f;
        public float gravity = -50;
        public float jumpHeight = 3f;

        public Transform groundCheck;
        public float sphereRadius = 0.4f;
        public LayerMask groundMask;
        public bool isGrounded;
        public float standUpHeight, crouchedHeight, standUpCameraPosition, crouchedCameraPosition;

        private Transform _cameraTransform;

        private Vector3 _decayVelocity;
        private bool _isCrouched;
        private bool _standUpBlocked;

        public AudioClip jumpSound;
        public AudioClip walkSound;
        private AudioSource _audioSource;

        const int MaxHealthPoints = 100;
        private int _healthPoints = MaxHealthPoints;
        private Slider _healthBar;

        private Inventory _inventory;

        // Start is called before the first frame update
        private void Start()
        {
            controller = GetComponent<CharacterController>();
            _audioSource = GetComponent<AudioSource>();
            _healthBar = GameObject.Find("HealthSlider").GetComponent<Slider>();
            _inventory = GetComponent<Inventory>();
            if (Camera.main != null) _cameraTransform = Camera.main.transform;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_healthPoints <= 0)
            {
                StartCoroutine(_inventory.GameOver(true));
                return;
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);

            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");

            var transform1 = transform;
            var move = transform1.right * x + transform1.forward * z;

            controller.Move(speed * Time.deltaTime * move);

            switch (isGrounded)
            {
                case true when Input.GetButtonDown("Jump"):
                    _decayVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    _audioSource.Stop();
                    _audioSource.clip = jumpSound;
                    _audioSource.Play();
                    break;
                case false:
                    _decayVelocity.y += gravity * Time.deltaTime;
                    break;
            }

            controller.Move(_decayVelocity * Time.deltaTime);

            if ((Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) && isGrounded)
            {
                if (!_audioSource.isPlaying)
                {
                    _audioSource.Stop();
                    _audioSource.clip = walkSound;
                    _audioSource.Play();
                }
            }

            CheckCrouchedHitBox();

            if (Input.GetKeyDown(KeyCode.LeftControl)) CrouchStandUp();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, sphereRadius);
        }

        private void CrouchStandUp()
        {
            if (_standUpBlocked || !isGrounded) return;
            _isCrouched = !_isCrouched;
            if (_isCrouched)
            {
                controller.height = crouchedHeight;
                _cameraTransform.localPosition = new Vector3(0, crouchedCameraPosition, 0);
            }
            else
            {
                controller.height = standUpHeight;
                _cameraTransform.localPosition = new Vector3(0, standUpCameraPosition, 0);
            }
        }

        private void CheckCrouchedHitBox()
        {
            _standUpBlocked = Physics.Raycast(_cameraTransform.position, Vector3.up, out _, 1.1f);
        }

        public void UpdateHealth(int newHealthPoints)
        {
            _healthPoints = Mathf.CeilToInt(Mathf.Clamp(_healthPoints + newHealthPoints, 0, MaxHealthPoints));
            _healthBar.value = _healthPoints;
        }
    }
}