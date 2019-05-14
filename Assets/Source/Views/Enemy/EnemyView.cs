using System;
using Core.Pool;
using Data;
using UnityEngine;

namespace Views.Enemy
{
    public abstract class EnemyView : UnityPoolObject<Data.Enum.Enemy>
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private CircleCollider2D _collider;
        [SerializeField] private SpriteRenderer _renderer;
        
        private float _defaultRadius;
        private Vector3 _defaultScale;
        private float _defaultGravityScale;
        public event Action<EnemyView> DieEvent;

        private void Awake()
        {
            _defaultRadius = _collider.radius;
            _defaultScale = _renderer.transform.localScale;
            _defaultGravityScale = _rigidbody.gravityScale;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Const.Ball))
            {
                OnViewEvent(PointViewEvent.BallTrigger, this);
            }
        }

        public void Die()
        {
            Push();
            DieEvent?.Invoke(this);
        }

        public void SetSize(float factor)
        {
            _collider.radius = _defaultRadius * factor;
            _renderer.transform.localScale = _defaultScale * factor;
        }

        public void SetSpeed(float factor)
        {
            _rigidbody.gravityScale = _defaultGravityScale * factor;
        }
    }
}