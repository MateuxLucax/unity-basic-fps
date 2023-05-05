using Hero;
using UnityEngine;

namespace Weapons.Glock
{
    public sealed class GlockMagazine : MonoBehaviour, IGrabbable
    {
        private Inventory _inventory;
        private Glock _glock;
        public int capacity = Glock.MagazineCapacity;

        private void Start()
        {
            _inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
            _glock = _inventory.glock.GetComponent<Glock>();
        }

        public void Grab()
        {
            StartCoroutine(_glock.AddMagazine(capacity));
        }
    }
}
