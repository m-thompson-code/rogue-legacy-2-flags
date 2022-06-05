using System;
using UnityEngine;

// Token: 0x02000694 RID: 1684
public class Barricade : MonoBehaviour
{
	// Token: 0x17001398 RID: 5016
	// (get) Token: 0x06003378 RID: 13176 RVA: 0x0001C324 File Offset: 0x0001A524
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

	// Token: 0x17001399 RID: 5017
	// (get) Token: 0x06003379 RID: 13177 RVA: 0x00003713 File Offset: 0x00001913
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x040029EF RID: 10735
	private Ferr2DT_PathTerrain m_ferr2D;
}
