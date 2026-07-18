using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PufferGo.Core
{
    public class DamageZone : MonoBehaviour
    {
        [SerializeField] private float _damage;
        [SerializeField] private bool _justRespawn;

        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.CompareTag("Player") && other.TryGetComponent<IDamagble>(out IDamagble damagble))
            {
                if (_justRespawn)
                {
                    //GameManager.MANAGER.RespawnPlayer();
                    return;
                }

                damagble.Damage(_damage);
            }
        }
    }
}