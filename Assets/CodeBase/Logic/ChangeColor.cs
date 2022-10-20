using UnityEngine;

namespace CodeBase.Logic {
  [RequireComponent(typeof(Renderer))]
  public class ChangeColor : MonoBehaviour {
    private void Awake() {
      GetComponent<Renderer>().material.color = Random.ColorHSV(0.3f, 1, 0.5f, 1, 0, 1);
    }
  }
}