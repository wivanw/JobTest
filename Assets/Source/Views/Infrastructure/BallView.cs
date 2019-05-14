using UnityEngine;

namespace Views
{
    public class BallView : InfrastructureView, IBallView
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private CircleCollider2D _collider;
        [SerializeField] private SpriteRenderer _renderer;
        private Vector3 _defaultScale;
        private float _defaultRadius;

        private void Awake()
        {
            _defaultScale = _renderer.transform.localScale;
            _defaultRadius = _collider.radius;
        }

        public void Resset()
        {
            Transform.position = Vector3.zero;
            Transform.rotation = Quaternion.identity;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0.0f;
        }

        public void SetSize(float scaleFactor)
        {
            _renderer.transform.localScale = _defaultScale * scaleFactor;
            _collider.radius = _defaultRadius * scaleFactor;
        }
    }
}