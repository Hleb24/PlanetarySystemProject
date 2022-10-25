using ToolBox.Pools;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace CodeBase.Logic.Orbit {
  [RequireComponent(typeof(LineRenderer))]
  public class OrbitRenderer : MonoBehaviour, IPoolable {
    [SerializeField]
    private LineRenderer _line;
    [SerializeField, Range(30, 90)]
    private int _segments;
    [SerializeField]
    private float _orbitRadius = 2.0f;

    private void Awake() {
      _line ??= GetComponent<LineRenderer>();
    }

    public void OnReuse() { }

    public void OnRelease() { }

    private void CalculateOrbit() {
      int count = _segments + 1;
      var job = new OrbitRendererJob {
        Points = new NativeArray<Vector3>(count, Allocator.TempJob),
        Segments = _segments,
        OrbitRadius = _orbitRadius
      };
      int batchCount = _segments / 3;
      JobHandle handle = job.Schedule(count, batchCount);
      handle.Complete();

      DrawOrbit(count, job.Points);
      job.Points.Dispose();
    }

    private void DrawOrbit(int count, NativeArray<Vector3> points) {
      _line.positionCount = count;
      _line.SetPositions(points);
    }

    private void OnValidate() {
      CalculateOrbit();
    }

    public float Radius {
      set {
        _orbitRadius = value;
        CalculateOrbit();
      }
    }

    [BurstCompile]
    private struct OrbitRendererJob : IJobParallelFor {
      public NativeArray<Vector3> Points;
      [ReadOnly]
      public int Segments;
      [ReadOnly]
      public float OrbitRadius;

      [BurstCompile]
      public void Execute(int index) {
        float angle = index / (float)Segments * 360 * Mathf.Deg2Rad;
        float x = Mathf.Sin(angle) * OrbitRadius;
        float z = Mathf.Cos(angle) * OrbitRadius;
        Points[index] = new Vector3(x, 0, z);
      }
    }
  }
}