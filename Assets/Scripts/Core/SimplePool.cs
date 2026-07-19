using System.Collections.Generic;
using UnityEngine;

namespace PufferGo.Core
{
    public class SimplePool : MonoBehaviour, IPoolable
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _initialSize = 10;

        private List<GameObject> _pool = new List<GameObject>();

        private void Awake()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                CreateObject();
            }
        }

        private GameObject CreateObject()
        {
            GameObject newObj = Instantiate(_prefab, transform);
            newObj.transform.parent = transform;
            newObj.SetActive(false);
            _pool.Add(newObj);
            return newObj;
        }

        public GameObject GetObject()
        {
            foreach (GameObject obj in _pool)
            {
                if (!obj.activeSelf)
                {
                    return obj;
                }
            }

            GameObject newObj = CreateObject();
            
            return newObj;
        }
    }
}