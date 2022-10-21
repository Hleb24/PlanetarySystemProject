using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CodeBase.Data.StaticData;
using CodeBase.Logic.Planetary;
using CodeBase.Logic.Planetary.Factory;
using CodeBase.Logic.Planetary.Object;
using CodeBase.Logic.Planetary.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI {
  public class ReloadContainer : MonoBehaviour {
    [SerializeField]
    private Button _reload;
    [SerializeField]
    private Slider _massSlider;
    [SerializeField]
    private Slider _cameraSlider;
    [SerializeField]
    private TextMeshProUGUI _massText;
    [SerializeField]
    private PlanetUI _planetUI;

    private IPlaneterySystemData _planeteryStaticData;
    private PlaneterySystemFactory _planeterySystemFactory;
    private IPlaneterySystem _planetarySystem;
    private Transform _cameraTransform;

    private void Awake() {
      if (Camera.main != null) {
        _cameraTransform = Camera.main.transform;
      }
    }

    private void OnEnable() {
      _reload.onClick.AddListener(OnReload);
      _massSlider.onValueChanged.AddListener(OnMassChanged);
      _cameraSlider.onValueChanged.AddListener(OnCameraSliderChanged);
    }

    private void OnDisable() {
      _reload.onClick.RemoveListener(OnReload);
      _massSlider.onValueChanged.RemoveListener(OnMassChanged);
      _cameraSlider.onValueChanged.RemoveListener(OnCameraSliderChanged);
    }

    public void Construct(PlaneterySystemFactory planeterySystemFactory, IPlaneterySystem planeterySystem, IPlaneterySystemData planeteryStaticData) {
      _planeterySystemFactory = planeterySystemFactory;
      _planetarySystem = planeterySystem;
      _planeteryStaticData = planeteryStaticData;
      PrepareUI();
    }

    private void OnCameraSliderChanged(float arg0) {
      Vector3 position = _cameraTransform.position;
      position.y = arg0;
      _cameraTransform.position = position;
    }

    private void PrepareUI() {
      _massText.text = _planeteryStaticData.TotalMass.ToString(CultureInfo.CurrentCulture);
      OnCameraSliderChanged(_cameraSlider.value);
      UpdatePlanetInfo();
    }

    private void UpdatePlanetInfo() {
      IEnumerable<IGrouping<MassClassEnum, IPlaneteryObject>> groupBy = _planetarySystem.PlaneteryObjects.GroupBy(p => p.MassClass);
      var existMassClass = new List<MassClassEnum>();
      foreach (IGrouping<MassClassEnum, IPlaneteryObject> planeteryObjects in groupBy) {
        existMassClass.Add(planeteryObjects.Key);
        var count = 0;
        foreach (IPlaneteryObject _ in planeteryObjects) {
          count++;
        }

        _planetUI.SetInfo((int)planeteryObjects.Key, planeteryObjects.Key.ToString(), count.ToString());
      }

      int length = Constants.NumberOfMassClass;
      for (var i = 0; i < length; i++) {
        if (existMassClass.Contains((MassClassEnum)i)) {
          continue;
        }

        _planetUI.SetInfo(i, ((MassClassEnum)i).ToString(), 0.ToString());
      }
    }

    private void OnReload() {
      Destroy(_planeterySystemFactory.PlaneterySystemBehaviour.gameObject);
      _planetarySystem = _planeterySystemFactory.Create(_planeteryStaticData.TotalMass);
      UpdatePlanetInfo();
    }

    private void OnMassChanged(float newMass) {
      _massText.text = newMass.ToString(CultureInfo.CurrentCulture);
      _planeteryStaticData.TotalMass = newMass;
    }
  }
}