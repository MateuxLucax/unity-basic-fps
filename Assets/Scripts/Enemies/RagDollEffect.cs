using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class RagDollEffect : MonoBehaviour
    {
        private Rigidbody _myRigidbody;
        private readonly List<Collider> _colliders = new();
        private readonly List<Rigidbody> _rigidbodies = new();
        private Animator _animator;
        private Collider _collider;

        // Start is called before the first frame update
        private void Start()
        {
            _collider = GetComponent<Collider>();
            _animator = GetComponent<Animator>();
            _myRigidbody = GetComponent<Rigidbody>();
        }

        public void Init()
        {
            var rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (var item in rigidbodies)
            {
                if (item == _myRigidbody) continue;

                _rigidbodies.Add(item);
                item.isKinematic = true;

                var component = item.gameObject.GetComponent<Collider>();
                component.enabled = false;
                _colliders.Add(component);
            }
        }

        public void Activate()
        {
            for (var i = 0; i < _rigidbodies.Count; i++)
            {
                _rigidbodies[i].isKinematic = false;
                _colliders[i].enabled = true;
            }

            _myRigidbody.isKinematic = true;
            _collider.enabled = false;

            StartCoroutine(FinishAnimation());
        }

        private IEnumerator FinishAnimation()
        {
            yield return new WaitForEndOfFrame();
            _animator.enabled = false;
        }
    }
}
