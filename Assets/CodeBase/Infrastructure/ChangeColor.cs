using UnityEngine;

namespace CodeBase.Infrastructure {
  [RequireComponent(typeof(Renderer))]
  public class ChangeColor : MonoBehaviour {
    private Material _material;

    private void Awake() {
      _material = GetComponent<Renderer>().material;
    }

    private void OnEnable() {
      _material.color = Random.ColorHSV(0.3f, 1, 0.5f, 1, 0, 1);
    }
  }
}