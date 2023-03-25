using UnityEngine;
using UnityEngine.UI;
using Outline = QuickOutline.Scripts.Outline;

namespace Hero
{
    public class ObjectCheck : MonoBehaviour
    {
        private GameObject _dragObject, _grabObject;
        private Text _keysText, _messageText;
        private float _targetDistance;
        private GameObject _targetObj;

        // Start is called before the first frame update
        private void Start()
        {
            _keysText = GameObject.Find("KeysText").GetComponent<Text>();
            _messageText = GameObject.Find("MessageText").GetComponent<Text>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Time.frameCount % 5 != 0) return;

            _dragObject = null;
            _grabObject = null;

            if (Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out var hit, 3.0f))
            {
                _targetDistance = hit.distance;

                if (!ReferenceEquals(_targetObj, null) && hit.transform.gameObject != _targetObj) Clear();

                _targetObj = hit.transform.gameObject;

                if (hit.transform.gameObject.CompareTag("Draggable"))
                {
                    _dragObject = _targetObj;

                    _keysText.text = "[F]";
                    _keysText.color = new Color(248 / 255f, 248255f, 13 / 255f);

                    _messageText.text = "Drag/Drop";
                    _messageText.color = _keysText.color;
                }
                else if (hit.transform.gameObject.CompareTag("Grabbable"))
                {
                    _grabObject = _targetObj;

                    _keysText.text = "[F]";
                    _keysText.color = new Color(51 / 255f, 1.0f, 0);

                    _messageText.text = "Grab";
                    _messageText.color = _keysText.color;
                }
                else
                {
                    _targetObj = null;
                }

                if (!ReferenceEquals(_targetObj, null)) _targetObj.GetComponent<Outline>().OutlineWidth = 5f;
            }
            else
            {
                if (!ReferenceEquals(_targetObj, null)) Clear();
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Clear()
        {
            _keysText.text = "";
            _messageText.text = "";
            if (Utils.IsNullOrDestroyed(_targetObj)) return;
            var outlineObj = _targetObj.GetComponent<Outline>();
            outlineObj.OutlineWidth = 0f;
            _targetObj = null;
        }

        public float GetTargetDistance()
        {
            return _targetDistance;
        }

        public GameObject GetDragObject()
        {
            return _dragObject;
        }

        public GameObject GetGrabObject()
        {
            return _grabObject;
        }
    }
}