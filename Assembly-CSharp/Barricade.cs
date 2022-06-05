using System;
using UnityEngine;

// Token: 0x020003EF RID: 1007
public class Barricade : MonoBehaviour
{
	// Token: 0x17000EEF RID: 3823
	// (get) Token: 0x0600251E RID: 9502 RVA: 0x0007B2B7 File Offset: 0x000794B7
	public Ferr2DT_PathTerrain Ferr2D
	{
		get
		{
			if (this.m_ferr2D == null)
			{
				this.m_ferr2D = base.GetComponent<Ferr2DT_PathTerrain>();
			}
			return this.m_ferr2D;
		}
	}

	// Token: 0x17000EF0 RID: 3824
	// (get) Token: 0x0600251F RID: 9503 RVA: 0x0007B2D9 File Offset: 0x000794D9
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x04001F5D RID: 8029
	private Ferr2DT_PathTerrain m_ferr2D;
}
