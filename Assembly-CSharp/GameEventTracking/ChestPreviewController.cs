using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameEventTracking
{
	// Token: 0x02000DCF RID: 3535
	public class ChestPreviewController : MonoBehaviour
	{
		// Token: 0x17002011 RID: 8209
		// (get) Token: 0x0600635C RID: 25436 RVA: 0x00003713 File Offset: 0x00001913
		public GameObject GameObject
		{
			get
			{
				return base.gameObject;
			}
		}

		// Token: 0x0600635D RID: 25437 RVA: 0x00036B4D File Offset: 0x00034D4D
		private void OnEnable()
		{
			this.m_coins.SetActive(false);
		}

		// Token: 0x0600635E RID: 25438 RVA: 0x001722F0 File Offset: 0x001704F0
		private string GetNameText(ISpecialItemData specialItemData)
		{
			string result = "";
			if (specialItemData is RuneTrackerData)
			{
				result = ((RuneTrackerData)specialItemData).RuneType.ToString();
			}
			else if (specialItemData is BlueprintTrackerData)
			{
				BlueprintTrackerData blueprintTrackerData = (BlueprintTrackerData)specialItemData;
				result = string.Format("{0} {1}", blueprintTrackerData.EquipmentType, blueprintTrackerData.EquipmentCategory);
			}
			return result;
		}

		// Token: 0x0600635F RID: 25439 RVA: 0x00036B5B File Offset: 0x00034D5B
		public void Initialise(ChestTrackerData saveData)
		{
			this.m_image.sprite = AssetPreviewManager.GetPreviewImage(saveData.ChestType);
			if (saveData.ContainsGold)
			{
				this.m_coins.SetActive(true);
			}
		}

		// Token: 0x0400511E RID: 20766
		[SerializeField]
		private ChestPreviewItemDescriptionController m_itemDescriptionPrefab;

		// Token: 0x0400511F RID: 20767
		[SerializeField]
		private float m_itemDescriptionPopDistance = 50f;

		// Token: 0x04005120 RID: 20768
		[SerializeField]
		private float m_popStartYPosition = -50f;

		// Token: 0x04005121 RID: 20769
		[SerializeField]
		private float m_popDistanceDecrement = 10f;

		// Token: 0x04005122 RID: 20770
		[SerializeField]
		private float m_itemDescriptionPopTime = 0.5f;

		// Token: 0x04005123 RID: 20771
		[SerializeField]
		private GameObject m_coins;

		// Token: 0x04005124 RID: 20772
		[SerializeField]
		private Image m_image;

		// Token: 0x04005125 RID: 20773
		[SerializeField]
		private ChestPreviewController.RarityColors m_colors;

		// Token: 0x02000DD0 RID: 3536
		[Serializable]
		private class RarityColors
		{
			// Token: 0x04005126 RID: 20774
			public Color Uncommon = Color.blue;

			// Token: 0x04005127 RID: 20775
			public Color Rare = Color.yellow;

			// Token: 0x04005128 RID: 20776
			public Color Unique = Color.magenta;
		}
	}
}
