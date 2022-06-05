using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DDF RID: 3551
	[Serializable]
	public struct BlueprintTrackerData : ISpecialItemData, IGameEventData
	{
		// Token: 0x060063CA RID: 25546 RVA: 0x00037036 File Offset: 0x00035236
		public BlueprintTrackerData(EquipmentCategoryType equipmentCategory, EquipmentType equipmentType)
		{
			this.m_equipmentCategory = equipmentCategory;
			this.m_equipmentType = equipmentType;
			this.m_timeStamp = (float)Time.frameCount;
			this.m_timesLoaded = SaveManager.StageSaveData.TimesTrackerWasLoaded;
		}

		// Token: 0x17002039 RID: 8249
		// (get) Token: 0x060063CB RID: 25547 RVA: 0x00037062 File Offset: 0x00035262
		public EquipmentCategoryType EquipmentCategory
		{
			get
			{
				return this.m_equipmentCategory;
			}
		}

		// Token: 0x1700203A RID: 8250
		// (get) Token: 0x060063CC RID: 25548 RVA: 0x0003706A File Offset: 0x0003526A
		public EquipmentType EquipmentType
		{
			get
			{
				return this.m_equipmentType;
			}
		}

		// Token: 0x1700203B RID: 8251
		// (get) Token: 0x060063CD RID: 25549 RVA: 0x00037072 File Offset: 0x00035272
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x1700203C RID: 8252
		// (get) Token: 0x060063CE RID: 25550 RVA: 0x0003707A File Offset: 0x0003527A
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x0400516A RID: 20842
		private EquipmentCategoryType m_equipmentCategory;

		// Token: 0x0400516B RID: 20843
		private EquipmentType m_equipmentType;

		// Token: 0x0400516C RID: 20844
		private float m_timeStamp;

		// Token: 0x0400516D RID: 20845
		private int m_timesLoaded;
	}
}
