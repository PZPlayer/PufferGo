using Unity.Cinemachine;
using UnityEngine;

namespace PufferGo.Effects
{

    public class CameraEffects : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource _impulseSource;

        public void SmallBump()
        {
            _impulseSource.GenerateImpulse();
        }
    }

}