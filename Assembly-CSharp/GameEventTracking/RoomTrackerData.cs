using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008A9 RID: 2217
	[Serializable]
	public struct RoomTrackerData : IGameEventData
	{
		// Token: 0x06004856 RID: 18518 RVA: 0x00103FE8 File Offset: 0x001021E8
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

		// Token: 0x170017AE RID: 6062
		// (get) Token: 0x06004857 RID: 18519 RVA: 0x0010403F File Offset: 0x0010223F
		public BiomeType Biome
		{
			get
			{
				return this.m_biome;
			}
		}

		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x06004858 RID: 18520 RVA: 0x00104047 File Offset: 0x00102247
		public int BiomeControllerIndex
		{
			get
			{
				return this.m_biomeControllerIndex;
			}
		}

		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x06004859 RID: 18521 RVA: 0x0010404F File Offset: 0x0010224F
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x0600485A RID: 18522 RVA: 0x00104057 File Offset: 0x00102257
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x0600485B RID: 18523 RVA: 0x0010405F File Offset: 0x0010225F
		public Vector2 WorldPosition
		{
			get
			{
				return new Vector2(this.m_worldPositionX, this.m_worldPositionY);
			}
		}

		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x0600485C RID: 18524 RVA: 0x00104072 File Offset: 0x00102272
		public bool ViaBiomeTransitionDoor
		{
			get
			{
				return this.m_viaBiomeTransitionDoor;
			}
		}

		// Token: 0x04003D11 RID: 15633
		private BiomeType m_biome;

		// Token: 0x04003D12 RID: 15634
		private int m_biomeControllerIndex;

		// Token: 0x04003D13 RID: 15635
		private float m_timeStamp;

		// Token: 0x04003D14 RID: 15636
		private int m_timesLoaded;

		// Token: 0x04003D15 RID: 15637
		private float m_worldPositionX;

		// Token: 0x04003D16 RID: 15638
		private float m_worldPositionY;

		// Token: 0x04003D17 RID: 15639
		private bool m_viaBiomeTransitionDoor;
	}
}
