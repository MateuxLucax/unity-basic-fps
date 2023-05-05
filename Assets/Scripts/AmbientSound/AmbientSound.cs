using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmbientSound
{
    public class AmbientSound : MonoBehaviour
    {
        public AudioClip ambientSound;
        public AudioClip cathedralSound;
        private AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            PlayCathedralSound();
        }

        public void PlayCathedralSound()
        {
            audioSource.Stop();
            audioSource.clip = cathedralSound;
            audioSource.volume = 0.5f;
            audioSource.Play();
        }

        public void PlayAmbientSound()
        {
            audioSource.Stop();
            audioSource.clip = ambientSound;
            audioSource.Play();
        }
    }
}