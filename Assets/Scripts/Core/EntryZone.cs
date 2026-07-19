using UnityEngine;
using UnityEngine.Events;

namespace  Core
{
    public class EntryZone :  MonoBehaviour
    {
        public UnityEvent onTriggerEnter;
        [SerializeField] private LayerMask triggerMask;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggerMask == (triggerMask | (1 << other.gameObject.layer)))
            {
                onTriggerEnter?.Invoke();
            }
        }
    }
}

