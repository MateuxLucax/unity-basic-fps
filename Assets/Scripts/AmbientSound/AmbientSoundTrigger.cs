using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmbientSound
{
    public class AmbientSoundTrigger : MonoBehaviour
    {
        private AmbientSound _ambientSound;

        private void Start()
        {
            _ambientSound = GameObject.FindWithTag("AmbientSound").GetComponent<AmbientSound>();
        }

        void OnTriggerEnter(Collider other)
        {
            _ambientSound.PlayCathedralSound();
        }

        void OnTriggerExit(Collider other)
        {
            _ambientSound.PlayAmbientSound();
        }
    }
}