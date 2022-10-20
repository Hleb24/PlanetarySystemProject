using TMPro;
using UnityEngine;

namespace CodeBase.UI {
  public class PlanetUI : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI[] _nameOfPlanets;
    [SerializeField]
    private TextMeshProUGUI[] _numberOfPlanets;

    public void SetInfo(int index, string nameOfPlanet, string numberOfPlanets) {
      _nameOfPlanets[index].text = $"{nameOfPlanet}:";
      _numberOfPlanets[index].text = $"{numberOfPlanets}:";
    }
  }
}