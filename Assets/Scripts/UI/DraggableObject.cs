using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UI
{
    public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private float dragEventDistance;

        private Vector3 _startingPosition;
        private Camera _mainCamera;

        public EventHandler draggedAway;


        private void Awake()
        {
            _startingPosition = gameObject.transform.position;
            _mainCamera = Camera.main;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Input.touchCount > 1)
                return;

            var zDistanceToCamera = Mathf.Abs(_startingPosition.z - _mainCamera.transform.position.z);

            var newPosition =
                _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    zDistanceToCamera));
            newPosition.x = _startingPosition.x;

            transform.position = newPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var zDistanceToCamera = Mathf.Abs(_startingPosition.z - _mainCamera.transform.position.z);
            var draggedPosition =
                _mainCamera.ScreenToWorldPoint(new Vector3(_startingPosition.x, Input.mousePosition.y,
                    zDistanceToCamera));
            draggedPosition.x = _startingPosition.x;
            var offset = _startingPosition - draggedPosition;
            
            Debug.Log(offset.magnitude);
            if (!(offset.magnitude >= dragEventDistance)) return;
            
            Debug.Log("dragged away");
            draggedAway(this, EventArgs.Empty);
        }

        public void ResetPosition(object sender, EventArgs eventArgs)
        {
            transform.position = _startingPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }
    }
}