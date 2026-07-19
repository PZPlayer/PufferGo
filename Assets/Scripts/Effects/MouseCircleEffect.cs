using UnityEngine;
using UnityEngine.InputSystem;

namespace PufferGo.Effects
{
    public class MouseCircleEffect : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToRotate;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _maxDistance = 5f;
        [SerializeField] private float _minAlpha = 0.1f;
        [SerializeField] private float _maxAlpha = 1f;

        private Camera _mainCamera;
        private bool _isAttacking;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePosition.z = 0;

            Vector3 directionToMouse = mousePosition - _objectToRotate.transform.position;
            float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
            _objectToRotate.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            float distance = Vector2.Distance(transform.position, mousePosition);
            float alpha = 0f;

            if (_isAttacking)
            {
                float clampedDistance = Mathf.Clamp(distance, 0, _maxDistance);
                float t = clampedDistance / _maxDistance;
                alpha = Mathf.Lerp(_minAlpha, _maxAlpha, t);
            }

            Color color = _spriteRenderer.color;
            color.a = alpha;
            _spriteRenderer.color = color;
        }

        public void OnAttack(InputValue context)
        {
            _isAttacking = context.isPressed;
        }
    }
}