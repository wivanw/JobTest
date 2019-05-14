using System;
using UnityEngine;

namespace Source.Game.Model
{
    public class Bottom : IBottom
    {
        private readonly Settings _settings;
        private readonly Camera _camera;
        private float _angle;
        private float _offset;
        private float _oldOffset;
        private bool _negative;

        public Bottom(Settings settings, Camera camera)
        {
            _settings = settings;
            _camera = camera;
        }

        public float CulcBottomPivot(Vector2 screenSize)
        {
            Vector2 topRightPos = _camera.ScreenToWorldPoint(screenSize);
            var tan = Mathf.Tan(_settings.MaxAngle * Mathf.Deg2Rad);
            var height = tan * topRightPos.x;
            height -= topRightPos.y;
            return height;
        }

        public Vector2 CulcBottomSize(Vector2 viewPivot)
        {
            //calculate width of bottom 
            Vector2 downLeftPos = _camera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, -_camera.transform.position.z));
            var c = downLeftPos - viewPivot;
            var alpha = Vector2.Angle(Vector2.left, c);
            alpha -= _settings.MaxAngle;
            var cosAlpha = Mathf.Cos(alpha * Mathf.Deg2Rad);
            var width = c.magnitude * cosAlpha;
            width *= 2.0f;
            //calculate height of bottom
            var beta = Vector2.Angle(Vector2.left, c);
            beta += _settings.MaxAngle;
            var sinBeta = Mathf.Sin(beta * Mathf.Deg2Rad);
            var height = sinBeta * c.magnitude;
            return new Vector2(width, height);
        }

        public void StartDrag()
        {
            _offset = Input.mousePosition.y;
            _negative = Input.mousePosition.x <= Screen.width / 2.0f;
        }

        public float Drag()
        {
            var temp = Input.mousePosition.y - _offset;
            var axis = Mathf.Clamp(_oldOffset + (_negative ? -temp : temp), -Screen.height, Screen.height) * 4.0f /
                       Screen.height;
            _angle = axis * _settings.MaxAngle;
            _angle *= _settings.Sensitivity;
            _angle = Mathf.Clamp(_angle, -_settings.MaxAngle, _settings.MaxAngle);
            return _angle;
        }

        public void EndDrag()
        {
            _offset = Input.mousePosition.y - _offset;
            _oldOffset = Mathf.Clamp(_oldOffset + (_negative ? -_offset : _offset), -Screen.height, Screen.height);
        }

        public void Resset()
        {
            _oldOffset = 0.0f;
        }

        [Serializable]
        public class Settings
        {
            [Range(0.5f, 3.0f)] public float Sensitivity;
            [Range(5.0f, 20.0f)] public float MaxAngle;
        }
    }
}