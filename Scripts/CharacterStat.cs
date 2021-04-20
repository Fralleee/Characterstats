using Fralle.Core.Attributes;
using System;
using System.Collections.Generic;
using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle.CharacterStats
{
	[Serializable]
	public abstract class CharacterStat
	{
		public event Action<CharacterStat> OnChanged = delegate { };

		public float BaseValue;

		protected bool IsDirty = true;
		protected float LastBaseValue;

		[SerializeField, Readonly]
		protected float _value;
		public float Value
		{
			get
			{
				if (!IsDirty && LastBaseValue.EqualsWithTolerance(BaseValue)) return _value;
				LastBaseValue = BaseValue;
				_value = CalculateFinalValue();
				IsDirty = false;
				return _value;
			}
		}

		protected readonly List<StatModifier> StatModifiers;

		protected CharacterStat()
		{
			StatModifiers = new List<StatModifier>();
		}

		protected CharacterStat(float baseValue) : this()
		{
			BaseValue = baseValue;
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
			if (StatModifiers.Remove(mod))
			{
				IsDirty = true;
				OnChangedDispatcher();
				return true;
			}
			return false;
		}

		public virtual bool RemoveAllModifiersFromSource(object source)
		{
			int numRemovals = StatModifiers.RemoveAll(mod => mod.Source == source);

			if (numRemovals <= 0) return false;

			IsDirty = true;
			OnChangedDispatcher();
			return true;
		}

		protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
		{
			if (a.Order < b.Order)
				return -1;
			if (a.Order > b.Order)
				return 1;

			return 0;
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

						if (i + 1 < StatModifiers.Count && StatModifiers[i + 1].Type == StatModType.PercentAdd) continue;
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

		protected virtual float CalculateFinalValue() => (float)Math.Round(CalculateModifiers(BaseValue), 4);
	}
}
