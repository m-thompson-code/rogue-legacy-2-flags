using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DE0 RID: 3552
	[Serializable]
	public struct RuneTrackerData : ISpecialItemData, IGameEventData
	{
		// Token: 0x060063CF RID: 25551 RVA: 0x00037082 File Offset: 0x00035282
		public RuneTrackerData(RuneType runeType)
		{
			this.m_runeType = runeType;
			this.m_timesLoaded = SaveManager.StageSaveData.TimesTrackerWasLoaded;
			this.m_timeStamp = (float)Time.frameCount;
		}

		// Token: 0x1700203D RID: 8253
		// (get) Token: 0x060063D0 RID: 25552 RVA: 0x000370A7 File Offset: 0x000352A7
		public RuneType RuneType
		{
			get
			{
				return this.m_runeType;
			}
		}

		// Token: 0x1700203E RID: 8254
		// (get) Token: 0x060063D1 RID: 25553 RVA: 0x000370AF File Offset: 0x000352AF
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x1700203F RID: 8255
		// (get) Token: 0x060063D2 RID: 25554 RVA: 0x000370B7 File Offset: 0x000352B7
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x0400516E RID: 20846
		private RuneType m_runeType;

		// Token: 0x0400516F RID: 20847
		private float m_timeStamp;

		// Token: 0x04005170 RID: 20848
		private int m_timesLoaded;
	}
}
