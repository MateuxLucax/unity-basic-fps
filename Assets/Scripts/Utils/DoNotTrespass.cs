using System;
using UnityEngine;

namespace Utils
{
    public class DoNotTrespass : MonoBehaviour
    {
        public GameObject text;

        private void Start()
        {
            text.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                text.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                text.SetActive(false);
            }
        }
    }
}
