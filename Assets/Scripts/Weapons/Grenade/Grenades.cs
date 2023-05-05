using Hero;
using UnityEngine;
using UnityEngine.UI;

namespace Weapons.Grenade
{
    public class Grenades : MonoBehaviour
    {
        private struct Grenade
        {
            public float spawnSeconds;
            public float explosionSeconds;
            public GameObject grenadeObj;
            public GameObject explosionObj;
        }

        private float _lastGrenadeSec;
        public GameObject grenadePrefab;
        public GameObject explosionPrefab;
        public int remainingGrenades;
        public float timeUntilExplosion = 3.0f;
        public float explosionDuration = 3.0f;
        public float cooldown = .4f;
        public float forwardStrength = 14f;
        public float upStrength = 2f;
        public float damageRadius = 8f;
        public float maxDamage = 40f;
        public Text grenadeText;

        private const int Cap = 16;
        private int _head;
        private int _tail;

        private readonly Grenade[] _grenades = new Grenade[Cap];

        void Update()
        {
            var now = Time.time;

            for (var i = _head; i != _tail; i = (i + 1) % Cap)
            {
                var grenade = _grenades[i];
                var explosionObj = grenade.explosionObj;
                var grenadeObj = grenade.grenadeObj;
                var explosionSeconds = grenade.explosionSeconds;
                var spawnSeconds = grenade.spawnSeconds;

                if (explosionSeconds != 0)
                {
                    if (now - explosionSeconds >= explosionDuration)
                    {
                        Destroy(explosionObj);
                        Destroy(grenadeObj);
                        _head = (_head + 1) % Cap;
                    }
                }
                else if (now - spawnSeconds >= timeUntilExplosion)
                {
                    ExplodeGrenade(grenadeObj);
                    _grenades[i].explosionSeconds = now;
                    _grenades[i].explosionObj = Instantiate(
                        explosionPrefab,
                        grenadeObj.transform.position,
                        Quaternion.LookRotation(Vector3.up)
                    );
                    // Object needs to stay in memory to avoid audioSource from stopping
                    grenadeObj.GetComponent<MeshRenderer>().enabled = false;
                    grenadeObj.GetComponent<BoxCollider>().enabled = false;
                    grenadeObj.GetComponent<Rigidbody>().isKinematic = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Q) && now - _lastGrenadeSec >= cooldown)
            {
                TryThrowGrenade();
                _lastGrenadeSec = now;
            }
        }

        public void AddGrenade(int amount)
        {
            // Maybe play a sound here? so player knows he got a grenade
            remainingGrenades += amount;
            UpdateText();
        }

        private void TryThrowGrenade()
        {
            if ((_tail + 1) % Cap == _head) return;
            if (remainingGrenades <= 0) return;

            remainingGrenades--;
            UpdateText();

            var grenadeObj = Instantiate(
                grenadePrefab,
                transform.position + transform.forward,
                transform.rotation
            );

            var strengthVector = transform.forward * forwardStrength + transform.up * upStrength;
            var grenadeRigidBody = grenadeObj.GetComponent<Rigidbody>();
            grenadeRigidBody.AddForce(strengthVector, ForceMode.Impulse);
            grenadeRigidBody.AddTorque(transform.forward);

            _grenades[_tail].spawnSeconds = Time.time;
            _grenades[_tail].grenadeObj = grenadeObj;
            _tail = (_tail + 1) % Cap;
        }

        private void ExplodeGrenade(GameObject granada)
        {
            var colliders = Physics.OverlapSphere(granada.transform.position, damageRadius);
            foreach (var sphereCollider in colliders)
            {
                var distance = Vector3.Distance(sphereCollider.transform.position, granada.transform.position);
                var damage = (int) ((1 - distance / damageRadius) * maxDamage);

                if (sphereCollider.transform.TryGetComponent(out ITakeDamage enemy))
                {
                    enemy.TakeDamage(damage);
                }
                else if (sphereCollider.transform.TryGetComponent(out PlayerMovementController player))
                {
                    player.UpdateHealth(-damage);
                }
            }

            // Maybe grenade should have its own AudioSource, so we can spacify the sound
            var grenadeAudio = granada.GetComponent<AudioSource>();
            grenadeAudio.Play();
        }

        private void UpdateText()
        {
            grenadeText.text = "Grenades: " + remainingGrenades;
        }
    }
}
