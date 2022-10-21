using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI {
  public class PlanetUI : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI[] _nameOfPlanets;
    [SerializeField]
    private TextMeshProUGUI[] _numberOfPlanets;
    [SerializeField]
    private Button _quitButton;

    private void OnEnable() {
      _quitButton.onClick.AddListener(Quit.Out);
    }

    private void OnDisable() {
      _quitButton.onClick.RemoveListener(Quit.Out);
    }

    public void SetInfo(int index, string nameOfPlanet, string numberOfPlanets) {
      _nameOfPlanets[index].text = $"{nameOfPlanet}:";
      _numberOfPlanets[index].text = $"{numberOfPlanets}";
    }
  }
}