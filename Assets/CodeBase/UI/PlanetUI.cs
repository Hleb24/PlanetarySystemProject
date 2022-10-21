using System;
using CodeBase.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI {
  public class PlanetUI : MonoBehaviour, IQuitable {
    public event Action<IQuitable> Quit;
    [SerializeField]
    private TextMeshProUGUI[] _nameOfPlanets;
    [SerializeField]
    private TextMeshProUGUI[] _numberOfPlanets;
    [SerializeField]
    private Button _quitButton;

    private void OnEnable() {
      _quitButton.onClick.AddListener(OnQuit);
    }

    private void OnDisable() {
      _quitButton.onClick.RemoveListener(OnQuit);
    }

    private void OnDestroy() {
      Quit = null;
    }

    public void SetInfo(int index, string nameOfPlanet, int numberOfPlanets) {
      _nameOfPlanets[index].text = $"{nameOfPlanet}:";
      _numberOfPlanets[index].text = $"{numberOfPlanets}";
    }

    private void OnQuit() {
      Quit?.Invoke(this);
    }
  }
}