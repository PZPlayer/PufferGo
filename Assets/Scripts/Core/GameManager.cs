using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PufferGo.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager MANAGER { get; private set; }

        public GameObject Player {  get => _player; }

        public event Action OnPlayerRespawned;

        [SerializeField] private Transform _startSpawnPoint;
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _blackOut;

        private Coroutine loadNewLevel;
        private Transform lastSpawnPoint;
        int spawnPointIndex = 0;

        private void Awake()
        {
            if (MANAGER == null)
            {
                MANAGER = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetSpawnPoint(Transform spawn, int index)
        {
            if (spawnPointIndex < index)
            {
                lastSpawnPoint = spawn;
                spawnPointIndex = index;
            }
        }

        public void OnSetPlayer(GameObject player)
        {
            _player = player;
        }

        public void ChangeScene(int index)
        {
            if (loadNewLevel == null)
                loadNewLevel = StartCoroutine(LoadLevelAfter(index, 1));
        }

        private IEnumerator LoadLevelAfter(int index, float beforeSomeTime)
        {
            if (_blackOut != null)
                _blackOut.SetActive(true);

            yield return new WaitForSeconds(beforeSomeTime);

            SceneManager.LoadScene(index);
        }

        public void RespawnPlayer()
        {
            if (lastSpawnPoint != null)
            {
                _player.transform.position = lastSpawnPoint.position;
            }
            else
            {
                _player.transform.position = _startSpawnPoint.position;
            }

            _player.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, 0);
            _player.GetComponent<IHealble>().Heal(99999);
            OnPlayerRespawned?.Invoke();
        }
    }
}