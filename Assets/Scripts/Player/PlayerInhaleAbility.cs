using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Events;
using static UnityEngine.LightAnchor;

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
        [SerializeField] private GameObject _partclsPool;
        [SerializeField] private GameObject _aim;
        [SerializeField] private UnityEvent _onJumpOff;
        [SerializeField] private UnityEvent _onInhale;
        [SerializeField] private UnityEvent _onExhale;

        private Vector2 normalScale;
        private Vector2 aimNormalScale;
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
            aimNormalScale = _aim.transform.localScale;
        }

        private void Update()
        {
            if (isInflated)
            {
                OnAirChanged?.Invoke(airHoldTimer);
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

            _onInhale?.Invoke();
            JumpOf();

            isInflated = true;
            OnScaleChanged?.Invoke(normalScale * _scaleMultiplayer);
            _aim.transform.localScale = aimNormalScale / _scaleMultiplayer;
            rb.gravityScale = _gravityWhileInhaling;
            rb.linearDamping = normalDamping;
            rb.angularDamping = _angularDampingWhileInhaling;
        }
        private void Exhale()
        {
            if (!isInflated) return;

            lastTimeExhaled = Time.time;
            isInflated = false;
            OnAirChanged?.Invoke(0);
            airHoldTimer = 0;

            _aim.transform.localScale = aimNormalScale;
            _onExhale?.Invoke();
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

            if (collidrs == null || collidrs.Length == 0) return;

            rb.linearVelocityY = 0;

            foreach (Collider2D collider in collidrs)
            {
                Vector2 closestPos = collider.ClosestPoint(transform.position);

                float distance = Vector2.Distance(transform.position, closestPos) - 0.01f;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, ((Vector2)transform.position - closestPos).normalized, distance, _jumpFromLayers);

                if (hit.collider == null)
                {
                    sumDiresction += ((Vector2)transform.position - closestPos).normalized;
                }

                if (_partclsPool != null && collidrs != null)
                {
                    IPoolable pool = _partclsPool.GetComponent<IPoolable>();
                    GameObject partcl = pool.GetObject();
                    partcl.SetActive(true);
                    partcl.transform.position = closestPos;
                    partcl.transform.rotation = Quaternion.LookRotation(sumDiresction);
                    StartCoroutine(TurnObjectOffAfter(partcl, 3));
                }

                Debug.DrawLine(collider.ClosestPoint(transform.position), collider.ClosestPoint(transform.position) + ((Vector2)transform.position - collider.ClosestPoint(transform.position)).normalized * _firstJumpOffPower, Color.yellow, 2);
            }

            _onJumpOff?.Invoke();
            rb.AddForce(sumDiresction.normalized * _firstJumpOffPower, ForceMode2D.Impulse); // Это сделано для ощущения. Так как прыжок в гору не чувствуеться вообще
            if (sumDiresction != Vector2.zero) lastTimeJumped = Time.time;
        }

        private IEnumerator TurnObjectOffAfter(GameObject objct, float time)
        {
            yield return new WaitForSeconds(time);

            objct.SetActive(false);
        }
    }
}