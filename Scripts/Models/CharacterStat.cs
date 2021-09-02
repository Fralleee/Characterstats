using Fralle.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.CharacterStats
{
  [Serializable]
  public abstract class CharacterStat
  {
    public event Action<CharacterStat> OnChanged = delegate { };

    public float baseValue;

    protected bool IsDirty = true;
    protected float LastBaseValue;

    [SerializeField, ReadOnly]
    protected float value;
    public float Value
    {
      get
      {
        if (!IsDirty && LastBaseValue.EqualsWithTolerance(baseValue))
          return value;
        LastBaseValue = baseValue;
        value = CalculateFinalValue();
        IsDirty = false;
        return value;
      }
    }

    protected readonly List<StatModifier> StatModifiers;

    protected CharacterStat()
    {
      StatModifiers = new List<StatModifier>();
    }

    protected CharacterStat(float baseValue) : this()
    {
      this.baseValue = baseValue;
    }

    protected virtual void OnChangedDispatcher()
    {
      OnChanged(this);
    }

    public virtual void AddModifier(StatModifier mod)
    {
      IsDirty = true;
      StatModifiers.Add(mod);
      OnChangedDispatcher();
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
      if (!StatModifiers.Remove(mod))
        return false;

      IsDirty = true;
      OnChangedDispatcher();
      return true;
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
      int numRemovals = StatModifiers.RemoveAll(mod => mod.Source == source);

      if (numRemovals <= 0)
        return false;

      IsDirty = true;
      OnChangedDispatcher();
      return true;
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
    {
      return a.Order < b.Order ? -1 : a.Order > b.Order ? 1 : 0;
    }

    protected virtual float CalculateModifiers(float baseValue)
    {
      float modifierValue = baseValue;
      float sumPercentAdd = 0;

      StatModifiers.Sort(CompareModifierOrder);

      for (int i = 0; i < StatModifiers.Count; i++)
      {
        StatModifier mod = StatModifiers[i];

        switch (mod.Type)
        {
          case StatModType.Flat:
            modifierValue += mod.Value;
            break;
          case StatModType.PercentAdd:
          {
            sumPercentAdd += mod.Value;

            if (i + 1 < StatModifiers.Count && StatModifiers[i + 1].Type == StatModType.PercentAdd)
              continue;
            modifierValue *= 1 + sumPercentAdd;
            sumPercentAdd = 0;
            break;
          }
          case StatModType.PercentMult:
            modifierValue *= 1 + mod.Value;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      // Workaround for float calculation errors, like displaying 12.00001 instead of 12
      return (float)Math.Round(modifierValue, 4);
    }

    protected virtual float CalculateFinalValue() => (float)Math.Round(CalculateModifiers(baseValue), 4);
  }
}
