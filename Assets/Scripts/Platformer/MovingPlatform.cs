using UnityEngine;

namespace Platformer
{
    public class MovingPlatform : MonoBehaviour
    {
        public Transform finalPosition;
        public float speed = 0.5f;

        private Vector3 _startPos;
        private float _trackPercent = 0.0f;
        private bool _direction = true;

        void Start()
        {
            _startPos = transform.position;
        }

        void Update()
        {
            if (finalPosition == null) return;
            _trackPercent += (_direction ? 1 : -1) * speed * Time.deltaTime;
            Vector3 pos = (finalPosition.position - _startPos) * _trackPercent + _startPos;
            pos.z = _startPos.z;
            transform.position = pos;

            if ((_direction && _trackPercent > 0.9f)
                || (!_direction && _trackPercent < 0.1f))
            {
                _direction = !_direction;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (Application.isPlaying)
                Gizmos.DrawLine(_startPos, finalPosition.position);
            else
                Gizmos.DrawLine(transform.position, finalPosition.position);
        }
    }
}
