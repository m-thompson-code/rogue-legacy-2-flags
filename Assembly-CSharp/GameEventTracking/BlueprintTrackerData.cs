using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008AC RID: 2220
	[Serializable]
	public struct BlueprintTrackerData : ISpecialItemData, IGameEventData
	{
		// Token: 0x0600486A RID: 18538 RVA: 0x0010414E File Offset: 0x0010234E
		public BlueprintTrackerData(EquipmentCategoryType equipmentCategory, EquipmentType equipmentType)
		{
			this.m_equipmentCategory = equipmentCategory;
			this.m_equipmentType = equipmentType;
			this.m_timeStamp = (float)Time.frameCount;
			this.m_timesLoaded = SaveManager.StageSaveData.TimesTrackerWasLoaded;
		}

		// Token: 0x170017BF RID: 6079
		// (get) Token: 0x0600486B RID: 18539 RVA: 0x0010417A File Offset: 0x0010237A
		public EquipmentCategoryType EquipmentCategory
		{
			get
			{
				return this.m_equipmentCategory;
			}
		}

		// Token: 0x170017C0 RID: 6080
		// (get) Token: 0x0600486C RID: 18540 RVA: 0x00104182 File Offset: 0x00102382
		public EquipmentType EquipmentType
		{
			get
			{
				return this.m_equipmentType;
			}
		}

		// Token: 0x170017C1 RID: 6081
		// (get) Token: 0x0600486D RID: 18541 RVA: 0x0010418A File Offset: 0x0010238A
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x0600486E RID: 18542 RVA: 0x00104192 File Offset: 0x00102392
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x04003D23 RID: 15651
		private EquipmentCategoryType m_equipmentCategory;

		// Token: 0x04003D24 RID: 15652
		private EquipmentType m_equipmentType;

		// Token: 0x04003D25 RID: 15653
		private float m_timeStamp;

		// Token: 0x04003D26 RID: 15654
		private int m_timesLoaded;
	}
}
