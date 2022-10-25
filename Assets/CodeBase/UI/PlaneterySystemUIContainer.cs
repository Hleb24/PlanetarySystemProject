using System;
using System.Globalization;
using CodeBase.Logic.Cameras;
using CodeBase.Logic.Planetary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI {
  public class PlaneterySystemUIContainer : MonoBehaviour, ICameraDistancePublisher, IReloadPublisher {
    public event Action Reload;
    public event Action<float> CameraDistanceChanged;
    public event Action<float> TotalMassChanged;
    [SerializeField]
    private Button _reload;
    [SerializeField]
    private Slider _massSlider;
    [SerializeField]
    private Slider _cameraSlider;
    [SerializeField]
    private TextMeshProUGUI _massText;

    private void OnEnable() {
      _reload.onClick.AddListener(OnReloadClicked);
      _massSlider.onValueChanged.AddListener(OnMassChanged);
      _cameraSlider.onValueChanged.AddListener(OnCameraSliderChanged);
    }

    private void OnDisable() {
      _reload.onClick.RemoveListener(OnReloadClicked);
      _massSlider.onValueChanged.RemoveListener(OnMassChanged);
      _cameraSlider.onValueChanged.RemoveListener(OnCameraSliderChanged);
    }

    private void OnDestroy() {
      ClearEvents();
    }

    public void Construct(float mass) {
      _massText.text = mass.ToString(CultureInfo.CurrentCulture);
    }

    private void OnMassChanged(float newMass) {
      _massText.text = newMass.ToString(CultureInfo.CurrentCulture);
      TotalMassChanged?.Invoke(newMass);
    }

    private void OnCameraSliderChanged(float arg0) {
      CameraDistanceChanged?.Invoke(arg0);
    }

    private void OnReloadClicked() {
      Reload?.Invoke();
    }

    private void ClearEvents() {
      Reload = null;
      CameraDistanceChanged = null;
      TotalMassChanged = null;
    }
  }
}