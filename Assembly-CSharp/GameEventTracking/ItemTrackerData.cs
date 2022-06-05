using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DDD RID: 3549
	[Serializable]
	public struct ItemTrackerData : IGameEventData
	{
		// Token: 0x060063BD RID: 25533 RVA: 0x00036FB2 File Offset: 0x000351B2
		public ItemTrackerData(float value, ItemDropType itemDropType)
		{
			this.m_value = value;
			this.m_itemDropType = itemDropType;
			this.m_timeStamp = (float)Time.frameCount;
			this.m_timesLoaded = SaveManager.StageSaveData.TimesTrackerWasLoaded;
		}

		// Token: 0x1700202E RID: 8238
		// (get) Token: 0x060063BE RID: 25534 RVA: 0x00036FDE File Offset: 0x000351DE
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x1700202F RID: 8239
		// (get) Token: 0x060063BF RID: 25535 RVA: 0x00036FE6 File Offset: 0x000351E6
		public float Value
		{
			get
			{
				return this.m_value;
			}
		}

		// Token: 0x17002030 RID: 8240
		// (get) Token: 0x060063C0 RID: 25536 RVA: 0x00036FEE File Offset: 0x000351EE
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x17002031 RID: 8241
		// (get) Token: 0x060063C1 RID: 25537 RVA: 0x00036FF6 File Offset: 0x000351F6
		public ItemDropType ItemDropType
		{
			get
			{
				return this.m_itemDropType;
			}
		}

		// Token: 0x0400515F RID: 20831
		private float m_timeStamp;

		// Token: 0x04005160 RID: 20832
		private float m_value;

		// Token: 0x04005161 RID: 20833
		private int m_timesLoaded;

		// Token: 0x04005162 RID: 20834
		private ItemDropType m_itemDropType;
	}
}
