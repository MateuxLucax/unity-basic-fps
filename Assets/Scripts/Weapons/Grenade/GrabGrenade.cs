using Hero;
using UnityEngine;

namespace Weapons.Grenade
{
    public class GrabGrenade : MonoBehaviour, IGrabbable
    {
        private Inventory _inventory;
        public int grenadesToAdd = 6;

        private void Start()
        {
            _inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        }

        public void Grab()
        {
            _inventory.grenades.AddGrenade(6);
        }
    }
}
