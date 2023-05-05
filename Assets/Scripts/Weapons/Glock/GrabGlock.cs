using Hero;
using UnityEngine;

namespace Weapons.Glock
{
    public class GrabGlock : MonoBehaviour, IGrabbable
    {
        private Inventory _inventory;
        private Glock _glock;

        private void Start()
        {
            _inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
            _glock = _inventory.glock.GetComponent<Glock>();
        }

        public void Grab()
        {
            _inventory.glock.SetActive(true);
            _glock.GrabGlock();
        }
    }
}
