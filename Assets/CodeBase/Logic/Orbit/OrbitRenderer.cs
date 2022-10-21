using ToolBox.Pools;
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
      var points = new Vector3[count];
      for (var i = 0; i < count; i++) {
        float angle = i / (float)_segments * 360 * Mathf.Deg2Rad;
        float x = Mathf.Sin(angle) * _orbitRadius;
        float z = Mathf.Cos(angle) * _orbitRadius;
        points[i] = new Vector3(x, 0, z);
      }

      points[_segments] = points[0];

      DrawOrbit(count, points);
    }

    private void DrawOrbit(int count, Vector3[] points) {
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
  }
}