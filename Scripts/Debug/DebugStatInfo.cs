using UnityEngine;
using UnityEngine.UI;

namespace Fralle.CharacterStats
{
  public class DebugStatInfo : MonoBehaviour
  {
    public StatAttribute attribute;
    public Text text;

    CharacterStat characterStat;

    void Start()
    {
      StatsControllerBase statsController = GetComponentInParent<StatsControllerBase>();
      characterStat = statsController.GetStat(attribute);
      if (characterStat == null)
        return;
      characterStat.OnChanged += StatChanged;
      StatChanged(characterStat);
    }

    private void StatChanged(CharacterStat obj)
    {
      text.text = obj.Value.ToString();
    }

    private void OnDestroy()
    {
      if (characterStat != null)
        characterStat.OnChanged -= StatChanged;
    }
  }
}
