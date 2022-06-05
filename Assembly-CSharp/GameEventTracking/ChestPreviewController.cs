using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameEventTracking
{
	// Token: 0x020008A2 RID: 2210
	public class ChestPreviewController : MonoBehaviour
	{
		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x06004821 RID: 18465 RVA: 0x001037CB File Offset: 0x001019CB
		public GameObject GameObject
		{
			get
			{
				return base.gameObject;
			}
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x001037D3 File Offset: 0x001019D3
		private void OnEnable()
		{
			this.m_coins.SetActive(false);
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x001037E4 File Offset: 0x001019E4
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

		// Token: 0x06004824 RID: 18468 RVA: 0x00103851 File Offset: 0x00101A51
		public void Initialise(ChestTrackerData saveData)
		{
			this.m_image.sprite = AssetPreviewManager.GetPreviewImage(saveData.ChestType);
			if (saveData.ContainsGold)
			{
				this.m_coins.SetActive(true);
			}
		}

		// Token: 0x04003CF0 RID: 15600
		[SerializeField]
		private ChestPreviewItemDescriptionController m_itemDescriptionPrefab;

		// Token: 0x04003CF1 RID: 15601
		[SerializeField]
		private float m_itemDescriptionPopDistance = 50f;

		// Token: 0x04003CF2 RID: 15602
		[SerializeField]
		private float m_popStartYPosition = -50f;

		// Token: 0x04003CF3 RID: 15603
		[SerializeField]
		private float m_popDistanceDecrement = 10f;

		// Token: 0x04003CF4 RID: 15604
		[SerializeField]
		private float m_itemDescriptionPopTime = 0.5f;

		// Token: 0x04003CF5 RID: 15605
		[SerializeField]
		private GameObject m_coins;

		// Token: 0x04003CF6 RID: 15606
		[SerializeField]
		private Image m_image;

		// Token: 0x04003CF7 RID: 15607
		[SerializeField]
		private ChestPreviewController.RarityColors m_colors;

		// Token: 0x02000E9E RID: 3742
		[Serializable]
		private class RarityColors
		{
			// Token: 0x040058A2 RID: 22690
			public Color Uncommon = Color.blue;

			// Token: 0x040058A3 RID: 22691
			public Color Rare = Color.yellow;

			// Token: 0x040058A4 RID: 22692
			public Color Unique = Color.magenta;
		}
	}
}
