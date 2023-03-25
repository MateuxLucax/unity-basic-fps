using Hero;
using UnityEngine;

namespace Weapons
{
    public class GlockMagazine : MonoBehaviour, IGrabbable
    {
        private Glock _glock;

        public void Grab()
        {
            _glock.AddMagazine();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            _glock = GameObject.FindWithTag("Weapon").GetComponent<Glock>();
        }
    }
}
