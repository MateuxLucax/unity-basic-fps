using UnityEngine;
using UnityEngine.UI;

namespace Hero
{
    public class ObjectActions : MonoBehaviour
    {
        private bool _grabbed;
        private Text _keysText, _messageText;

        private ObjectCheck _objectId;

        // Start is called before the first frame update
        private void Start()
        {
            _objectId = GetComponent<ObjectCheck>();

            _keysText = GameObject.Find("KeysText").GetComponent<Text>();
            _messageText = GameObject.Find("MessageText").GetComponent<Text>();
        }

        // Update is called once per frame
        private void Update()
        {
            var distance = _objectId.GetTargetDistance();

            if (distance < 3)
            {
                if (!Input.GetKeyDown(KeyCode.F)) return;

                _keysText.gameObject.SetActive(true);
                _messageText.gameObject.SetActive(true);

                if (!ReferenceEquals(_objectId.GetGrabObject(), null))
                {
                    Grab();
                }
                else if (!ReferenceEquals(_objectId.GetDragObject(), null))
                {
                    if (!_grabbed)
                        Drag();
                    else
                        Drop();
                    _grabbed = !_grabbed;
                }
            }
            else
            {
                _keysText.gameObject.SetActive(false);
                _messageText.gameObject.SetActive(false);
            }
        }

        private void Grab()
        {
            var grabbable = _objectId.GetGrabObject().GetComponent<IGrabbable>();
            grabbable?.Grab();

            Destroy(_objectId.GetGrabObject());
        }

        private void Drag()
        {
            var obj = _objectId.GetDragObject();

            if (!obj) return;
            var objComponent = obj.GetComponent<Rigidbody>();
            objComponent.isKinematic = true;
            objComponent.useGravity = false;
            obj.transform.SetParent(transform);
            obj.transform.localPosition = new Vector3(0, 0, 3);
            obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        private void Drop()
        {
            var obj = _objectId.GetDragObject();

            if (ReferenceEquals(obj, null)) return;
            obj.transform.localPosition = new Vector3(0, 0, 3);
            obj.transform.SetParent(null);
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}