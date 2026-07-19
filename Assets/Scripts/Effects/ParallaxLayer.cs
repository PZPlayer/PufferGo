using UnityEngine;
using UnityEngine.Serialization;

namespace Effects
{
    public class ParallaxLayer : MonoBehaviour
    {
        
        // ReSharper disable once InconsistentNaming
        [FormerlySerializedAs("_parallaxCoefficient")] [SerializeField, Range(0, 1f)] private float _parallaxCoefficientX;
        [SerializeField, Range(0, 1f)] private float _parallaxCoefficientY;
        // ReSharper disable once InconsistentNaming
        [SerializeField] Transform _camera;

        // ReSharper disable once InconsistentNaming
        private float startBackgroundPosX;
        private float startBackgroundY;
        
        private float startCameraPosX;
        private float startCameraPosY;
        

        public void Awake()
        {

            _camera = _camera != null ? _camera : Camera.main.transform;

            startBackgroundPosX = transform.position.x;
            startBackgroundY = transform.position.y;
            startCameraPosX = _camera.transform.position.x;
            startCameraPosY = _camera.transform.position.y;
        }

        private void LateUpdate()
        {
            //5. Считаем, на сколько камера ушла от своей стартовой позиции:
            float cameraMovementX = _camera.transform.position.x - startCameraPosX;
            float cameraMovementY = _camera.transform.position.y - startCameraPosY;

            // 6. Считаем новую позицию для фона:
            float targetX = startBackgroundPosX + (cameraMovementX * _parallaxCoefficientX);
            float targetY = startBackgroundY + (cameraMovementY * _parallaxCoefficientY);

            transform.position = new Vector3(targetX, targetY, transform.position.z);
        }
    }
}