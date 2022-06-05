using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x0200090A RID: 2314
	[Serializable]
	public class ProjectileAudioLibraryEntry : AudioLibraryEntry
	{
		// Token: 0x06004BE2 RID: 19426 RVA: 0x00110CC0 File Offset: 0x0010EEC0
		public ProjectileAudioLibraryEntry(string key, string spawnSinglePath, string spawnManyPath, string lifeTimeEventPath, string hitSurfaceEventPath, string hitItemEventPath, string hitCharacterEventPath, string deathEventPath)
		{
			if (!Application.isPlaying)
			{
				this.Key = key;
				this.m_spawnSingleEventPath = spawnSinglePath;
				this.m_spawnManyEventPath = spawnManyPath;
				this.m_lifetimeEventPath = lifeTimeEventPath;
				this.m_hitSurfaceEventPath = hitSurfaceEventPath;
				this.m_hitItemEventPath = hitItemEventPath;
				this.m_hitCharacterEventPath = hitCharacterEventPath;
				this.m_deathEventPath = deathEventPath;
				return;
			}
			throw new Exception("This should never be called during runtime");
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x00110D22 File Offset: 0x0010EF22
		public ProjectileAudioLibraryEntry()
		{
		}

		// Token: 0x1700186C RID: 6252
		// (get) Token: 0x06004BE4 RID: 19428 RVA: 0x00110D2A File Offset: 0x0010EF2A
		public string SpawnSingleEventPath
		{
			get
			{
				return this.m_spawnSingleEventPath;
			}
		}

		// Token: 0x1700186D RID: 6253
		// (get) Token: 0x06004BE5 RID: 19429 RVA: 0x00110D32 File Offset: 0x0010EF32
		public bool UseSpawnManyAudioWhenAppropriate
		{
			get
			{
				return this.m_useSpawnManyAudioWhenAppropriate;
			}
		}

		// Token: 0x1700186E RID: 6254
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x00110D3A File Offset: 0x0010EF3A
		public string SpawnManyEventPath
		{
			get
			{
				return this.m_spawnManyEventPath;
			}
		}

		// Token: 0x1700186F RID: 6255
		// (get) Token: 0x06004BE7 RID: 19431 RVA: 0x00110D42 File Offset: 0x0010EF42
		public string HitSurfaceEventPath
		{
			get
			{
				return this.m_hitSurfaceEventPath;
			}
		}

		// Token: 0x17001870 RID: 6256
		// (get) Token: 0x06004BE8 RID: 19432 RVA: 0x00110D4A File Offset: 0x0010EF4A
		public string HitItemEventPath
		{
			get
			{
				return this.m_hitItemEventPath;
			}
		}

		// Token: 0x17001871 RID: 6257
		// (get) Token: 0x06004BE9 RID: 19433 RVA: 0x00110D52 File Offset: 0x0010EF52
		public string HitCharacterEventPath
		{
			get
			{
				return this.m_hitCharacterEventPath;
			}
		}

		// Token: 0x17001872 RID: 6258
		// (get) Token: 0x06004BEA RID: 19434 RVA: 0x00110D5A File Offset: 0x0010EF5A
		public string DeathEventPath
		{
			get
			{
				return this.m_deathEventPath;
			}
		}

		// Token: 0x17001873 RID: 6259
		// (get) Token: 0x06004BEB RID: 19435 RVA: 0x00110D62 File Offset: 0x0010EF62
		public string LifetimeEventPath
		{
			get
			{
				return this.m_lifetimeEventPath;
			}
		}

		// Token: 0x04003FEA RID: 16362
		[SerializeField]
		[EventRef]
		private string m_spawnSingleEventPath;

		// Token: 0x04003FEB RID: 16363
		[SerializeField]
		private bool m_useSpawnManyAudioWhenAppropriate;

		// Token: 0x04003FEC RID: 16364
		[SerializeField]
		[EventRef]
		private string m_spawnManyEventPath;

		// Token: 0x04003FED RID: 16365
		[SerializeField]
		[EventRef]
		private string m_lifetimeEventPath;

		// Token: 0x04003FEE RID: 16366
		[SerializeField]
		[EventRef]
		private string m_hitSurfaceEventPath;

		// Token: 0x04003FEF RID: 16367
		[SerializeField]
		[EventRef]
		private string m_hitItemEventPath;

		// Token: 0x04003FF0 RID: 16368
		[SerializeField]
		[EventRef]
		private string m_hitCharacterEventPath;

		// Token: 0x04003FF1 RID: 16369
		[SerializeField]
		[EventRef]
		private string m_deathEventPath;
	}
}
