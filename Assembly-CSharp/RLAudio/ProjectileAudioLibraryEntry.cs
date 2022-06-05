using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E87 RID: 3719
	[Serializable]
	public class ProjectileAudioLibraryEntry : AudioLibraryEntry
	{
		// Token: 0x060068DD RID: 26845 RVA: 0x00180EC4 File Offset: 0x0017F0C4
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

		// Token: 0x060068DE RID: 26846 RVA: 0x00038BA2 File Offset: 0x00036DA2
		public ProjectileAudioLibraryEntry()
		{
		}

		// Token: 0x17002165 RID: 8549
		// (get) Token: 0x060068DF RID: 26847 RVA: 0x0003A173 File Offset: 0x00038373
		public string SpawnSingleEventPath
		{
			get
			{
				return this.m_spawnSingleEventPath;
			}
		}

		// Token: 0x17002166 RID: 8550
		// (get) Token: 0x060068E0 RID: 26848 RVA: 0x0003A17B File Offset: 0x0003837B
		public bool UseSpawnManyAudioWhenAppropriate
		{
			get
			{
				return this.m_useSpawnManyAudioWhenAppropriate;
			}
		}

		// Token: 0x17002167 RID: 8551
		// (get) Token: 0x060068E1 RID: 26849 RVA: 0x0003A183 File Offset: 0x00038383
		public string SpawnManyEventPath
		{
			get
			{
				return this.m_spawnManyEventPath;
			}
		}

		// Token: 0x17002168 RID: 8552
		// (get) Token: 0x060068E2 RID: 26850 RVA: 0x0003A18B File Offset: 0x0003838B
		public string HitSurfaceEventPath
		{
			get
			{
				return this.m_hitSurfaceEventPath;
			}
		}

		// Token: 0x17002169 RID: 8553
		// (get) Token: 0x060068E3 RID: 26851 RVA: 0x0003A193 File Offset: 0x00038393
		public string HitItemEventPath
		{
			get
			{
				return this.m_hitItemEventPath;
			}
		}

		// Token: 0x1700216A RID: 8554
		// (get) Token: 0x060068E4 RID: 26852 RVA: 0x0003A19B File Offset: 0x0003839B
		public string HitCharacterEventPath
		{
			get
			{
				return this.m_hitCharacterEventPath;
			}
		}

		// Token: 0x1700216B RID: 8555
		// (get) Token: 0x060068E5 RID: 26853 RVA: 0x0003A1A3 File Offset: 0x000383A3
		public string DeathEventPath
		{
			get
			{
				return this.m_deathEventPath;
			}
		}

		// Token: 0x1700216C RID: 8556
		// (get) Token: 0x060068E6 RID: 26854 RVA: 0x0003A1AB File Offset: 0x000383AB
		public string LifetimeEventPath
		{
			get
			{
				return this.m_lifetimeEventPath;
			}
		}

		// Token: 0x0400554A RID: 21834
		[SerializeField]
		[EventRef]
		private string m_spawnSingleEventPath;

		// Token: 0x0400554B RID: 21835
		[SerializeField]
		private bool m_useSpawnManyAudioWhenAppropriate;

		// Token: 0x0400554C RID: 21836
		[SerializeField]
		[EventRef]
		private string m_spawnManyEventPath;

		// Token: 0x0400554D RID: 21837
		[SerializeField]
		[EventRef]
		private string m_lifetimeEventPath;

		// Token: 0x0400554E RID: 21838
		[SerializeField]
		[EventRef]
		private string m_hitSurfaceEventPath;

		// Token: 0x0400554F RID: 21839
		[SerializeField]
		[EventRef]
		private string m_hitItemEventPath;

		// Token: 0x04005550 RID: 21840
		[SerializeField]
		[EventRef]
		private string m_hitCharacterEventPath;

		// Token: 0x04005551 RID: 21841
		[SerializeField]
		[EventRef]
		private string m_deathEventPath;
	}
}
