using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008AA RID: 2218
	[Serializable]
	public struct ItemTrackerData : IGameEventData
	{
		// Token: 0x0600485D RID: 18525 RVA: 0x0010407A File Offset: 0x0010227A
		public ItemTrackerData(float value, ItemDropType itemDropType)
		{
			this.m_value = value;
			this.m_itemDropType = itemDropType;
			this.m_timeStamp = (float)Time.frameCount;
			this.m_timesLoaded = SaveManager.StageSaveData.TimesTrackerWasLoaded;
		}

		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x0600485E RID: 18526 RVA: 0x001040A6 File Offset: 0x001022A6
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x0600485F RID: 18527 RVA: 0x001040AE File Offset: 0x001022AE
		public float Value
		{
			get
			{
				return this.m_value;
			}
		}

		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x06004860 RID: 18528 RVA: 0x001040B6 File Offset: 0x001022B6
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x170017B7 RID: 6071
		// (get) Token: 0x06004861 RID: 18529 RVA: 0x001040BE File Offset: 0x001022BE
		public ItemDropType ItemDropType
		{
			get
			{
				return this.m_itemDropType;
			}
		}

		// Token: 0x04003D18 RID: 15640
		private float m_timeStamp;

		// Token: 0x04003D19 RID: 15641
		private float m_value;

		// Token: 0x04003D1A RID: 15642
		private int m_timesLoaded;

		// Token: 0x04003D1B RID: 15643
		private ItemDropType m_itemDropType;
	}
}
