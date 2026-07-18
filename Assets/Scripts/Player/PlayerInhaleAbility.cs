using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace PufferGo.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInhaleAbility : MonoBehaviour, IShowScale, IAirShowable
    {
        public event Action<Vector2> OnScaleChanged;
        public event Action<float> OnAirChanged;
        public float AirTimer => _airHoldTime;

        [SerializeField] private float _realodTime;
        [SerializeField] private float _scaleMultiplayer;
        [SerializeField] private float _maxTimeInhale;
        [SerializeField] private float _gravityNormal;
        [SerializeField] private float _gravityWhileInhaling;
        [SerializeField] private float _dampingWhileInhaling;
        [SerializeField] private float _angularDampingWhileInhaling;
        [SerializeField] private LayerMask _jumpFromLayers;
        [SerializeField] private float _firstJumpOffPower;
        [SerializeField] private float _jumpRadius = 1.6f;
        [SerializeField] private float _jumpCooldown = 0.05f;
        [SerializeField] private float _airHoldTime = 3f;

        private Vector2 normalScale;
        private Rigidbody2D rb;
        private bool buttonPresed;
        private bool isInflated;
        private float normalDamping;
        private float angularNormalDamping;
        private float lastTimeJumped;
        private float lastTimeExhaled;
        private float airHoldTimer;

        private void Start ()
        {
            normalScale = transform.localScale;
            rb = GetComponent<Rigidbody2D>();
            normalDamping = rb.linearDamping;
            angularNormalDamping = rb.angularDamping;
        }

        private void Update()
        {
            if (isInflated)
            {
                OnAirChanged?.Invoke(Time.time - lastTimeExhaled);
                airHoldTimer += Time.deltaTime;

                if (airHoldTimer >= _airHoldTime)
                {
                    Exhale();
                    GetComponent<IDamagble>().Damage(500);
                }
            }
        }

        private void Inhale()
        {
            if (Time.time - lastTimeExhaled < _realodTime) return;

            JumpOf();

            isInflated = true;
            OnScaleChanged?.Invoke(normalScale * _scaleMultiplayer);
            rb.gravityScale = _gravityWhileInhaling;
            rb.linearDamping = normalDamping;
            rb.angularDamping = _angularDampingWhileInhaling;
        }
        private void Exhale()
        {
            lastTimeExhaled = Time.time;
            isInflated = false;
            OnAirChanged?.Invoke(0);
            airHoldTimer = 0;

            OnScaleChanged?.Invoke(normalScale);

            rb.gravityScale = _gravityNormal;
            rb.linearDamping = _dampingWhileInhaling;
            rb.angularDamping = angularNormalDamping;
        }

        private void OnAbility(InputValue value)
        {
            buttonPresed = value.isPressed;

            if (buttonPresed)
            {
                Inhale();
            }
            else
            {
                Exhale();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isInflated) JumpOf();
        }

        private void JumpOf()
        {
            if (Time.time - lastTimeJumped < _jumpCooldown) return;

            Collider2D[] collidrs = Physics2D.OverlapCircleAll(transform.position, _jumpRadius, _jumpFromLayers);

            Vector2 sumDiresction = Vector2.zero;

            if (collidrs != null) rb.linearVelocityY = 0;

            foreach (Collider2D collider in collidrs)
            {
                sumDiresction += ((Vector2)transform.position - collider.ClosestPoint(transform.position)).normalized;

                Debug.DrawLine(collider.ClosestPoint(transform.position), collider.ClosestPoint(transform.position) + ((Vector2)transform.position - collider.ClosestPoint(transform.position)).normalized * _firstJumpOffPower, Color.yellow, 2);
            }

            rb.AddForce(sumDiresction.normalized * _firstJumpOffPower, ForceMode2D.Impulse); // Это сделано для ощущения. Так как прыжок в гору не чувствуеться вообще
            if (sumDiresction != Vector2.zero) lastTimeJumped = Time.time;
        }
    }
}