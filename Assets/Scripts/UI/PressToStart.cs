using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PufferGo.UI
{
    public class PressToStart : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onAnyKeyPressed;

        private void Start ()
        {
            Time.timeScale = 0f;
        }

        private void Update()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.middleButton.wasPressedThisFrame)
            {
                _onAnyKeyPressed?.Invoke();
            }
        }

        public void StartGame()
        {
            Time.timeScale = 1;
        }
    }
}