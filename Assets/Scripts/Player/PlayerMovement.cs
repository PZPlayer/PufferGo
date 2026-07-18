using PufferGo.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PufferGo.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _flyingSpeed;
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;

        private bool isButtonPressed;
        private Vector2 initialMouseClickPos;
        private Rigidbody2D rb;
        private bool isGround;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            GameManager.MANAGER.OnSetPlayer(gameObject);
        }

        private void FixedUpdate()
        {
            if (!isButtonPressed) return;

            initialMouseClickPos = Mouse.current.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(initialMouseClickPos);
            Vector2 direction = Vector2.zero;
            Vector2 delta = worldPos - (Vector2)transform.position;

            if (Mathf.Abs(delta.x) > _minDistance && isGround)
            {
                direction.x = Mathf.Clamp(delta.x, -1, 1) * Mathf.Lerp(_minSpeed, _maxSpeed, Mathf.Abs(delta.x) / _maxDistance);
            }
            else
            {
                direction.x = Mathf.Clamp(delta.x, -1, 1) * _flyingSpeed;
            }

            direction.y = 0;

            rb.AddForce(direction);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            isGround = true;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            isGround = false;
        }

        public void OnAttack(InputValue value)
        {
            isButtonPressed = value.isPressed;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _minDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _maxDistance);
        }
    }
}