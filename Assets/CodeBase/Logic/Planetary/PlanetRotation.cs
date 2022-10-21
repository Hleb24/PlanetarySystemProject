using UnityEngine;

namespace CodeBase.Logic.Planetary {
  public class PlanetRotation : MonoBehaviour {
    [SerializeField]
    private Transform _target;
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
      _target.Rotate(GetEulers(deltaTime));
    }

    private Vector3 GetEulers(float deltaTime) {
      float rotationAnglesX = _rotationAngles.x * _rotationSpeed * deltaTime;
      float rotationAnglesY = _rotationAngles.y * _rotationSpeed * deltaTime;
      float rotationAnglesZ = _rotationAngles.z * _rotationSpeed * deltaTime;
      return new Vector3(rotationAnglesX, rotationAnglesY, rotationAnglesZ);
    }
  }
}