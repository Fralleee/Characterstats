using System.Collections.Generic;
using UnityEngine;

namespace Fralle.CharacterStats
{
  public class StatsControllerBase : MonoBehaviour
  {
    public Dictionary<StatAttribute, CharacterStat> Stats;

    public CharacterStat GetStat(StatAttribute attribute)
    {
      return Stats.TryGetValue(attribute, out CharacterStat value) ? value : null;
    }

    public void ModifyStat(StatAttribute statAttribute, float value, StatModType statModType)
    {
      Stats[statAttribute].AddModifier(new StatModifier(value, statModType));
    }

    // ReSharper disable once UnusedMember.Global
    public void ClearModifiers(StatAttribute statAttribute, object source)
    {
      Stats[statAttribute].RemoveAllModifiersFromSource(source);
    }

    protected void AddMajorStatToDict(StatAttribute attribute, CharacterMajorStat stat)
    {
      Stats.Add(attribute, stat);
    }

    protected void AddMinorStatToDict(StatAttribute attribute, CharacterMinorStat stat)
    {
      CharacterStat parent = Stats[stat.parentAttribute];
      stat.SetupParentListener(parent);
      Stats.Add(attribute, stat);
    }

    protected virtual void Awake()
    {
      Stats = new Dictionary<StatAttribute, CharacterStat>();
    }

  }
}
