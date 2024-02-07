using Core;
using Managers.Interfaces;
using UnityEngine;

namespace Controllers
{
    public class CameraController : IController
    {
        public bool IsInit { get; }
        private Camera _mainCamera;
        private Transform _cameraTransform => _mainCamera.transform;
        private LevelController _levelController;

        public void Init()
        {
            _mainCamera = Camera.main;
            _levelController = GameClient.Instance.GetService<IGameplayManager>().GetController<LevelController>();
        }

        public void InsertCameraIntoLabyrinth()
        {
            _mainCamera.orthographicSize = CalculateRequiredCameraSize(_levelController.CalculateBoundsMaze());
            _cameraTransform.position = _levelController.GetCenterWorldMaze() - Vector3.forward;
        }

        private float CalculateRequiredCameraSize(Bounds bounds)
        {
            float screenAspect = (float)Screen.width / Screen.height;
            float boundsAspect = bounds.size.x / bounds.size.y;

            if (screenAspect >= boundsAspect)
            {
                return bounds.size.y / 2;
            }
            else
            {
                return bounds.size.x / 2 / screenAspect;
            }
        }
    }
}