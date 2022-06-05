using System;
using UnityEngine;

// Token: 0x02000B63 RID: 2915
[Serializable]
public class MasterFerr2DEntry
{
	// Token: 0x17001D92 RID: 7570
	// (get) Token: 0x0600589C RID: 22684 RVA: 0x00030276 File Offset: 0x0002E476
	// (set) Token: 0x0600589D RID: 22685 RVA: 0x0003027E File Offset: 0x0002E47E
	public Ferr2DT_PathTerrain Master
	{
		get
		{
			return this.m_master;
		}
		private set
		{
			this.m_master = value;
		}
	}

	// Token: 0x0600589E RID: 22686 RVA: 0x00030287 File Offset: 0x0002E487
	public void SetMaster(Ferr2DT_PathTerrain masterPrefab)
	{
		this.Master = masterPrefab;
		if (this.Master != null)
		{
			if (this.Ferr2DSettings == null)
			{
				this.Ferr2DSettings = new Ferr2DSettings();
			}
			this.Ferr2DSettings.Update(masterPrefab);
			return;
		}
		this.Ferr2DSettings = null;
	}

	// Token: 0x17001D93 RID: 7571
	// (get) Token: 0x0600589F RID: 22687 RVA: 0x000302C5 File Offset: 0x0002E4C5
	// (set) Token: 0x060058A0 RID: 22688 RVA: 0x000302CD File Offset: 0x0002E4CD
	public Ferr2DSettings Ferr2DSettings
	{
		get
		{
			return this.m_ferr2DSettings;
		}
		private set
		{
			this.m_ferr2DSettings = value;
		}
	}

	// Token: 0x0400416C RID: 16748
	[SerializeField]
	private Ferr2DT_PathTerrain m_master;

	// Token: 0x0400416D RID: 16749
	[SerializeField]
	private Ferr2DSettings m_ferr2DSettings;
}
