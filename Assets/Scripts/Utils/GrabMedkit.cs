using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hero;

namespace Utils
{
    public class GrabMedkit : MonoBehaviour, IGrabbable
    {
        public int health = 25;
        private PlayerMovementController _playerMovementController;

        void Start()
        {
            _playerMovementController = GameObject.FindWithTag("Player").GetComponent<PlayerMovementController>();
        }

        public void Grab()
        {
            _playerMovementController.UpdateHealth(health);
        }
    }
}
