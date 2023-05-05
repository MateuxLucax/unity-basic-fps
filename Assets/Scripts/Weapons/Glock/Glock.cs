using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Weapons.Glock
{
    public class Glock : MonoBehaviour
    {
        public Text ammoText;
        private Camera _mainCamera;
        public GameObject shootEffect;
        public GameObject postShootEffect;
        public GameObject spark;
        private Animator _animator;
        private RaycastHit _hit;
        private bool _isRunning;
        public AudioSource audioSource;
        private static readonly int ActionRunning = Animator.StringToHash("actionRunning");
        private static readonly int Aim = Animator.StringToHash("aim");
        public AudioClip[] clips;
        public const int MagazineCapacity = 17;
        private int _ammo;
        private int _totalAmmo;
        public GameObject cursorImage;

        // Start is called before the first frame update
        private void Start()
        {
            _isRunning = false;
            _animator = GetComponent<Animator>();
            _mainCamera = Camera.main;
            UpdateAmmoText();
        }

        private void Update()
        {
            if (_animator.GetBool(ActionRunning) || _isRunning) return;

            if (Input.GetButtonDown("Fire1"))
            {
                if (_ammo <= 0)
                {
                    StartCoroutine(Reload());
                    return;
                }

                _ammo--;
                _isRunning = true;
                StartCoroutine(Shooting());
            } 
            else if (Input.GetButton("Reload"))
            {
                StartCoroutine(Reload());
            } 
            else if (Input.GetButton("Fire2"))
            {
                _animator.SetBool(Aim, true);
                cursorImage.SetActive(false);
                _mainCamera.fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, 45, Time.deltaTime * 10);
            } 
            else
            {
                _animator.SetBool(Aim, false);
                cursorImage.SetActive(true);
                _mainCamera.fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, 60, Time.deltaTime * 10);
            }
        }

        private IEnumerator Shooting()
        {
            var screenX = Screen.width / 2;
            var screenY = Screen.height / 2;

            var ray = _mainCamera.ScreenPointToRay(new Vector3(screenX, screenY));
            _animator.Play("GlockShoot");

            GameObject sparkObj = null;
            var shootEffectObj = Instantiate(shootEffect, postShootEffect.transform.position,
                postShootEffect.transform.rotation);

            if (Physics.SphereCast(ray, 0.1f, out _hit))
            {
                sparkObj = Instantiate(spark, _hit.point, Quaternion.FromToRotation(Vector3.up, _hit.normal));
                if (_hit.transform.CompareTag("Draggable"))
                {
                    var direction = ray.direction;
                    _hit.rigidbody.AddForceAtPosition(direction * 500, _hit.point);
                }
                if (_hit.transform.CompareTag("TakeDamage"))
                {
                    var takeDamage = _hit.transform.GetComponent<ITakeDamage>();
                    takeDamage?.TakeDamage(5);
                }
            }

            audioSource.PlayOneShot(clips[0]);
            yield return new WaitForSeconds(0.3f);
            audioSource.Stop();
            Destroy(shootEffectObj);
            Destroy(sparkObj);

            UpdateAmmoText();
            _isRunning = false;
        }

        private IEnumerator Reload()
        {
            _isRunning = true;
            if (_totalAmmo > 0 && _ammo < 17)
            {
                _animator.Play("GlockReload");
                audioSource.PlayOneShot(clips[1]);
                yield return new WaitForSeconds(1.0f);
                audioSource.Stop();
                var ammoToReload = MagazineCapacity - _ammo;
                var availableAmmo = _totalAmmo >= ammoToReload ? ammoToReload : _totalAmmo;
                _ammo += availableAmmo;
                _totalAmmo -= availableAmmo;
            }
            else
            {
                audioSource.PlayOneShot(clips[2]);
                yield return new WaitForSeconds(0.1f);
                audioSource.Stop();
            }
            UpdateAmmoText();
            _isRunning = false;
        }

        private void UpdateAmmoText()
        {
            if (enabled)
            {
                ammoText.text = $"Ammo: {_ammo} / {_totalAmmo}";
            }
        }

        public IEnumerator AddMagazine(int ammo)
        {
			_totalAmmo += ammo;
            UpdateAmmoText();
            audioSource.PlayOneShot(clips[1]);
            yield return new WaitForSeconds(1.0f);
            audioSource.Stop();
        }

        public void GrabGlock()
        {
            _ammo = MagazineCapacity;
        }
    }
}