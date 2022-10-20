using CodeBase.Logic.Planetary.System;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor {
  [CustomEditor(typeof(PlaneterySystemMarker))]
  public class PlaneterySystemMarkerEditor : UnityEditor.Editor {
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(PlaneterySystemMarker spawner, GizmoType gizmo) {
      CircleGizmo(spawner.transform, .5f, Color.red);
    }

    private static void CircleGizmo(Transform spawnerTransform, float radius, Color color) {
      Gizmos.color = color;
      Gizmos.DrawSphere(spawnerTransform.position, radius);
    }
  }
}