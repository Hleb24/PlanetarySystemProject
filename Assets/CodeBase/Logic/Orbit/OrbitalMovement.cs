using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Logic.Orbit {
  public class OrbitalMovement {
    private const float MAX_SPEED = 1.2f;
    private const float MIN_SPEED = 0.5f;
    private const float MAX_DISTANCE_DELTA = 10000f;
    private readonly float _speed;
    private readonly Transform _center;
    private readonly Transform _planet;
    private readonly int _clockWise;
    private float _orbitRadius = 2.0f;
    private float _angle;

    public OrbitalMovement(Transform center, Transform planet, float angle, IRandomService randomService) {
      _center = center;
      _planet = planet;
      _angle = angle;
      _speed = randomService.Next(MIN_SPEED, MAX_SPEED);
      _clockWise = randomService.Next(0, 2) == 0 ? 1 : -1;
    }

    public void MoveByOrbit(float deltaTime) {
      Vector3 current = GetCurrentPosition();
      Vector3 target = GetTargetPosition(deltaTime);
      _planet.localPosition = Vector3.MoveTowards(current, target, MAX_DISTANCE_DELTA);
    }

    private Vector3 GetCurrentPosition() {
      return _planet.position;
    }

    private Vector3 GetTargetPosition(float deltaTime) {
      float newAngle = UpdateAngle(deltaTime);
      float positionX = _center.position.x + Mathf.Sin(newAngle) * _orbitRadius;
      float positionY = _center.position.y;
      float positionZ = _center.position.z + Mathf.Cos(newAngle) * _orbitRadius;
      return new Vector3(positionX, positionY, positionZ);
    }

    private float UpdateAngle(float deltaTime) {
      return _angle += deltaTime * _speed * _clockWise;
    }

    public float Radius {
      set {
        _orbitRadius = value;
      }
    }
  }
}