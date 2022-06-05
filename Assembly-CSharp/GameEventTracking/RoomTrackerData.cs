using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DDC RID: 3548
	[Serializable]
	public struct RoomTrackerData : IGameEventData
	{
		// Token: 0x060063B6 RID: 25526 RVA: 0x00172C58 File Offset: 0x00170E58
		public RoomTrackerData(BiomeType biome, int biomeControllerIndex, Vector2 worldPosition = default(Vector2), bool viaBiomeTransitionDoor = false)
		{
			this.m_timeStamp = (float)Time.frameCount;
			this.m_biome = biome;
			this.m_biomeControllerIndex = biomeControllerIndex;
			this.m_timesLoaded = SaveManager.StageSaveData.TimesTrackerWasLoaded;
			this.m_worldPositionX = worldPosition.x;
			this.m_worldPositionY = worldPosition.y;
			this.m_viaBiomeTransitionDoor = viaBiomeTransitionDoor;
		}

		// Token: 0x17002028 RID: 8232
		// (get) Token: 0x060063B7 RID: 25527 RVA: 0x00036F77 File Offset: 0x00035177
		public BiomeType Biome
		{
			get
			{
				return this.m_biome;
			}
		}

		// Token: 0x17002029 RID: 8233
		// (get) Token: 0x060063B8 RID: 25528 RVA: 0x00036F7F File Offset: 0x0003517F
		public int BiomeControllerIndex
		{
			get
			{
				return this.m_biomeControllerIndex;
			}
		}

		// Token: 0x1700202A RID: 8234
		// (get) Token: 0x060063B9 RID: 25529 RVA: 0x00036F87 File Offset: 0x00035187
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x1700202B RID: 8235
		// (get) Token: 0x060063BA RID: 25530 RVA: 0x00036F8F File Offset: 0x0003518F
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x1700202C RID: 8236
		// (get) Token: 0x060063BB RID: 25531 RVA: 0x00036F97 File Offset: 0x00035197
		public Vector2 WorldPosition
		{
			get
			{
				return new Vector2(this.m_worldPositionX, this.m_worldPositionY);
			}
		}

		// Token: 0x1700202D RID: 8237
		// (get) Token: 0x060063BC RID: 25532 RVA: 0x00036FAA File Offset: 0x000351AA
		public bool ViaBiomeTransitionDoor
		{
			get
			{
				return this.m_viaBiomeTransitionDoor;
			}
		}

		// Token: 0x04005158 RID: 20824
		private BiomeType m_biome;

		// Token: 0x04005159 RID: 20825
		private int m_biomeControllerIndex;

		// Token: 0x0400515A RID: 20826
		private float m_timeStamp;

		// Token: 0x0400515B RID: 20827
		private int m_timesLoaded;

		// Token: 0x0400515C RID: 20828
		private float m_worldPositionX;

		// Token: 0x0400515D RID: 20829
		private float m_worldPositionY;

		// Token: 0x0400515E RID: 20830
		private bool m_viaBiomeTransitionDoor;
	}
}
