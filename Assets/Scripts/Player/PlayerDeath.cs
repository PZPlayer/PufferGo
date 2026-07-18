using PufferGo.Core;
using System.Collections;
using UnityEngine;

namespace PufferGo.Player
{
    [RequireComponent(typeof(IHealthShowable))]
    public class PlayerDeath : MonoBehaviour
    {
        private IHealthShowable healthShowable;
        private Coroutine corutine;

        private void Start()
        {
            healthShowable = GetComponent<IHealthShowable>();
            healthShowable.OnDeathEvent += Die;
        }

        private void Die()
        {
            Time.timeScale = 0.1f;
            if(corutine == null)
                corutine = StartCoroutine(RespawnAfter(0.5f));
        }

        private IEnumerator RespawnAfter(float time)
        {
            yield return new WaitForSecondsRealtime(time);

            Time.timeScale = 1f;
            GameManager.MANAGER.RespawnPlayer();
            corutine = null;
        }

        private void OnDestroy()
        {
            healthShowable.OnDeathEvent -= Die;
        }
    }
}