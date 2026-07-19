using UnityEngine;

namespace PufferGo.Effects
{
    public class AirAnimation : MonoBehaviour
    {
        [SerializeField] private Color _baseColor;
        [SerializeField] private Color _noAirColor;

        private IAirShowable airShowable;
        private SpriteRenderer spriteRenderer;

        private void Start ()
        {
            airShowable = GetComponent<IAirShowable>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            airShowable.OnAirChanged += HandleAirChanged;
        }

        private void HandleAirChanged(float air)
        {
            if ((air / airShowable.AirTimer) < 0.5f)
            {
                spriteRenderer.color = _baseColor;
                return;
            }

            spriteRenderer.color = Color.Lerp(_baseColor, _noAirColor, air / airShowable.AirTimer);
        }

        private void OnDestroy()
        {
            airShowable.OnAirChanged -= HandleAirChanged;
        }
    }
}
