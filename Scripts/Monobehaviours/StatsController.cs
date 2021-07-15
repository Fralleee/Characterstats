using System.Collections.Generic;
using UnityEngine;

namespace Fralle.CharacterStats
{
	public class StatsController : MonoBehaviour
	{
		[HideInInspector]
		public Dictionary<StatAttribute, CharacterStat> Stats;

		[Header("Major stats")]
		public CharacterMajorStat Agility;
		public CharacterMajorStat Dexterity;
		public CharacterMajorStat Strength;

		[Header("Minor stats")]
		public CharacterMinorStat Aim;
		public CharacterMinorStat JumpPower;
		public CharacterMinorStat ReloadSpeed;
		public CharacterMinorStat RunSpeed;

		void OnEnable()
		{
			Stats = new Dictionary<StatAttribute, CharacterStat>();

			AddMajorStatToDict(StatAttribute.Dexterity, Dexterity);
			AddMajorStatToDict(StatAttribute.Agility, Agility);
			AddMajorStatToDict(StatAttribute.Strength, Strength);

			AddMinorStatToDict(StatAttribute.Aim, Aim);
			AddMinorStatToDict(StatAttribute.Jumppower, JumpPower);
			AddMinorStatToDict(StatAttribute.Reloadspeed, ReloadSpeed);
			AddMinorStatToDict(StatAttribute.Runspeed, RunSpeed);
		}

		public void ModifyStat(StatAttribute statAttribute, float value, StatModType statModType)
		{
			Stats[statAttribute].AddModifier(new StatModifier(value, statModType));
		}

		public void ClearModifiers(StatAttribute statAttribute, object source)
		{
			Stats[statAttribute].RemoveAllModifiersFromSource(source);
		}

		void AddMajorStatToDict(StatAttribute attribute, CharacterMajorStat stat)
		{
			Stats.Add(attribute, stat);
		}

		void AddMinorStatToDict(StatAttribute attribute, CharacterMinorStat stat)
		{
			CharacterStat parent = Stats[stat.ParentAttribute];
			stat.SetupParentListener(parent);
			Stats.Add(attribute, stat);
		}

		public CharacterStat GetStat(StatAttribute attribute)
		{
			return Stats.TryGetValue(attribute, out CharacterStat value) ? value : null;
		}

	}
}
