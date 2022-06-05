using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008AD RID: 2221
	[Serializable]
	public struct RuneTrackerData : ISpecialItemData, IGameEventData
	{
		// Token: 0x0600486F RID: 18543 RVA: 0x0010419A File Offset: 0x0010239A
		public RuneTrackerData(RuneType runeType)
		{
			this.m_runeType = runeType;
			this.m_timesLoaded = SaveManager.StageSaveData.TimesTrackerWasLoaded;
			this.m_timeStamp = (float)Time.frameCount;
		}

		// Token: 0x170017C3 RID: 6083
		// (get) Token: 0x06004870 RID: 18544 RVA: 0x001041BF File Offset: 0x001023BF
		public RuneType RuneType
		{
			get
			{
				return this.m_runeType;
			}
		}

		// Token: 0x170017C4 RID: 6084
		// (get) Token: 0x06004871 RID: 18545 RVA: 0x001041C7 File Offset: 0x001023C7
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x170017C5 RID: 6085
		// (get) Token: 0x06004872 RID: 18546 RVA: 0x001041CF File Offset: 0x001023CF
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x04003D27 RID: 15655
		private RuneType m_runeType;

		// Token: 0x04003D28 RID: 15656
		private float m_timeStamp;

		// Token: 0x04003D29 RID: 15657
		private int m_timesLoaded;
	}
}
