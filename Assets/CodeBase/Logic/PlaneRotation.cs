using UnityEngine;

namespace CodeBase.Logic {
  public class PlaneRotation : MonoBehaviour {
    [SerializeField]
    private bool _notRotate;
    [SerializeField]
    private Vector3 _rotationAngles;
    [SerializeField]
    private float _rotationSpeed;

    private void OnEnable() {
      _rotationAngles = Random.rotation.eulerAngles;
    }

    private void Update() {
      if (_notRotate) {
        return;
      }

      RotatePlanet(Time.deltaTime);
    }

    private void RotatePlanet(float deltaTime) {
      transform.Rotate(GetEulers(deltaTime));
    }

    private Vector3 GetEulers(float deltaTime) {
      float rotationAnglesX = _rotationAngles.x * _rotationSpeed * deltaTime;
      float rotationAnglesY = _rotationAngles.y * _rotationSpeed * deltaTime;
      float rotationAnglesZ = _rotationAngles.z * _rotationSpeed * deltaTime;
      return new Vector3(rotationAnglesX, rotationAnglesY, rotationAnglesZ);
    }
  }
}