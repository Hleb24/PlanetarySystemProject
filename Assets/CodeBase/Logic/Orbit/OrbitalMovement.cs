using CodeBase.Infrastructure.Services;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
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

      var newPosition = new NativeArray<Vector3>(1, Allocator.TempJob);
      var job = new OrbitMovementJub {
        angle = newAngle,
        orbitRadius = _orbitRadius,
        center = _center.position,
        NewPosition = newPosition
      };

      JobHandle handle = job.Schedule();
      handle.Complete();

      Vector3 targetPosition = newPosition[0];
      newPosition.Dispose();

      return targetPosition;
    }

    private float UpdateAngle(float deltaTime) {
      return _angle += deltaTime * _speed * _clockWise;
    }

    public float Radius {
      set {
        _orbitRadius = value;
      }
    }

    private struct OrbitMovementJub : IJob {
      public NativeArray<Vector3> NewPosition;
      public Vector3 center;
      public float orbitRadius;
      public float angle;

      public void Execute() {
        math.sincos(angle, out float x, out float z);
        Vector3 position = NewPosition[0];
        position.x = center.x + x * orbitRadius;
        position.z = center.x + z * orbitRadius;
        NewPosition[0] = position;
      }
    }
  }
}