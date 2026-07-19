using UnityEngine;

namespace PufferGo.Core
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private int _spawnIndex = 0;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.MANAGER.SetSpawnPoint(transform, _spawnIndex);
            }
        }
    }
}