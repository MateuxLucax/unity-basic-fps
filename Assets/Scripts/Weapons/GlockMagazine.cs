using Hero;
using UnityEngine;

namespace Weapons
{
    public sealed class GlockMagazine : MonoBehaviour, IGrabbable
    {
        private Glock _glock;

        public void Grab()
        {
            StartCoroutine(_glock.AddMagazine());
        }

        // Start is called before the first frame update
        private void Start()
        {
            _glock = GameObject.FindWithTag("Weapon").GetComponent<Glock>();
        }
    }
}
