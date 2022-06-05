using System;
using UnityEngine;

// Token: 0x020006B8 RID: 1720
[Serializable]
public class MasterFerr2DEntry
{
	// Token: 0x1700159A RID: 5530
	// (get) Token: 0x06003F65 RID: 16229 RVA: 0x000E2134 File Offset: 0x000E0334
	// (set) Token: 0x06003F66 RID: 16230 RVA: 0x000E213C File Offset: 0x000E033C
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

	// Token: 0x06003F67 RID: 16231 RVA: 0x000E2145 File Offset: 0x000E0345
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

	// Token: 0x1700159B RID: 5531
	// (get) Token: 0x06003F68 RID: 16232 RVA: 0x000E2183 File Offset: 0x000E0383
	// (set) Token: 0x06003F69 RID: 16233 RVA: 0x000E218B File Offset: 0x000E038B
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

	// Token: 0x04002F1D RID: 12061
	[SerializeField]
	private Ferr2DT_PathTerrain m_master;

	// Token: 0x04002F1E RID: 12062
	[SerializeField]
	private Ferr2DSettings m_ferr2DSettings;
}
