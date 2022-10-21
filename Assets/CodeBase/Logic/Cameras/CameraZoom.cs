using UnityEngine;

namespace CodeBase.Logic.Cameras {
  public class CameraZoom {
    private readonly Transform _cameraTransform;

    public CameraZoom(Transform cameraTransform, ICameraDistancePublisher updateSystem) {
      _cameraTransform = cameraTransform;
      updateSystem.CameraDistanceChanged += UpdateSystemOnCameraDistanceChanged;
    }

    private void UpdateSystemOnCameraDistanceChanged(float obj) {
      Vector3 position = _cameraTransform.position;
      position.y = obj;
      _cameraTransform.position = position;
    }
  }
}