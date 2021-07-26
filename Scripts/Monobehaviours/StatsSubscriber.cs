using UnityEngine;

namespace Fralle.CharacterStats
{
	public abstract class StatsSubscriber : MonoBehaviour
	{
		protected abstract void Subscribe(CharacterStat stat);
	}
}
