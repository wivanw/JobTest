using Data;
using UnityEngine;

namespace Views
{
    public class BottomView : InfrastructureView, IBottomView
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private SpriteRenderer _renderer;

        public void SetIncline(float angle)
        {
            _rigidbody.MoveRotation(angle);
        }

        public void SetSize(Vector2 size)
        {
            _collider.size = new Vector2(size.x, size.y);
            _collider.offset = new Vector2(_collider.offset.x, -size.y / 2.0f);
            _renderer.size = new Vector2(size.x, size.y);
        }

        private void OnMouseDown()
        {
            OnViewEvent(BottomEvent.ButtonDown, null);
        }

        private void OnMouseUp()
        {
            OnViewEvent(BottomEvent.ButtonUp, null);
        }

        private void OnMouseDrag()
        {
            OnViewEvent(BottomEvent.Drag, null);
        }
    }
}