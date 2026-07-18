using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;
using UnityEngine.UIElements;

namespace PufferGo.Effects
{
    [RequireComponent(typeof(IShowScale))]
    public class ScaleAnimation : MonoBehaviour
    {
        [SerializeField] private float _scaleTime = 0.5f;

        private IShowScale scaleable;

        private void Start()
        {
            scaleable = GetComponent<IShowScale>();
            if (scaleable != null)
            {
                scaleable.OnScaleChanged += HandleScaleChanged;
            }
        }

        private void HandleScaleChanged(Vector2 newScale)
        {

            transform.DOScale(newScale, _scaleTime).SetEase(Ease.InOutSine);
        }

        private void OnDestroy()
        {
            if (scaleable != null)
            {
                scaleable.OnScaleChanged -= HandleScaleChanged;
            }
        }
    }
}