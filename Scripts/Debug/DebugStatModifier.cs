using UnityEngine;
using UnityEngine.UI;

namespace Fralle.CharacterStats
{
	public class DebugStatModifier : MonoBehaviour
	{
		public StatAttribute Attribute;

		public Button decreasePercentage;
		public Button decreaseFlat;
		public Button increaseFlat;
		public Button increasePercentage;

		StatsController statsController;

		void Awake()
		{
			statsController = GetComponentInParent<StatsController>();

			decreasePercentage.onClick.AddListener(DecreasePercentage);
			decreaseFlat.onClick.AddListener(DecreaseFlat);
			increaseFlat.onClick.AddListener(IncreaseFlat);
			increasePercentage.onClick.AddListener(IncreasePercentage);
		}

		void DecreasePercentage()
		{
			statsController.ModifyStat(Attribute, -0.05f, StatModType.PercentAdd);
		}

		void DecreaseFlat()
		{
			statsController.ModifyStat(Attribute, -1, StatModType.Flat);
		}

		void IncreaseFlat()
		{
			statsController.ModifyStat(Attribute, 1, StatModType.Flat);
		}

		void IncreasePercentage()
		{
			statsController.ModifyStat(Attribute, 0.05f, StatModType.PercentAdd);
		}


		void OnDestroy()
		{
			decreasePercentage.onClick.RemoveAllListeners();
			decreaseFlat.onClick.RemoveAllListeners();
			increaseFlat.onClick.RemoveAllListeners();
			increasePercentage.onClick.RemoveAllListeners();
		}

	}
}
