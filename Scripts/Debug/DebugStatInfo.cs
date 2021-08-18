using UnityEngine;
using UnityEngine.UI;

namespace Fralle.CharacterStats
{
  public class DebugStatInfo : MonoBehaviour
  {
    public StatAttribute Attribute;
    public Text Text;

    CharacterStat characterStat;

    void Start()
    {
      StatsControllerBase statsController = GetComponentInParent<StatsControllerBase>();
      characterStat = statsController.GetStat(Attribute);
      if (characterStat == null)
        return;
      characterStat.OnChanged += StatChanged;
      StatChanged(characterStat);
    }

    private void StatChanged(CharacterStat obj)
    {
      Text.text = obj.Value.ToString();
    }

    private void OnDestroy()
    {
      if (characterStat != null)
        characterStat.OnChanged -= StatChanged;
    }
  }
}
