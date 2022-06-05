using System;
using UnityEngine;

// Token: 0x020006B9 RID: 1721
[Serializable]
public class Ferr2DSettings
{
	// Token: 0x1700159C RID: 5532
	// (get) Token: 0x06003F6B RID: 16235 RVA: 0x000E219C File Offset: 0x000E039C
	// (set) Token: 0x06003F6C RID: 16236 RVA: 0x000E21A4 File Offset: 0x000E03A4
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

	// Token: 0x1700159D RID: 5533
	// (get) Token: 0x06003F6D RID: 16237 RVA: 0x000E21AD File Offset: 0x000E03AD
	public Ferr2DT_Material Material
	{
		get
		{
			if (this.Master != null)
			{
				return this.Master.TerrainMaterial as Ferr2DT_Material;
			}
			return null;
		}
	}

	// Token: 0x1700159E RID: 5534
	// (get) Token: 0x06003F6E RID: 16238 RVA: 0x000E21CF File Offset: 0x000E03CF
	public bool InteriorGridVerts
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700159F RID: 5535
	// (get) Token: 0x06003F6F RID: 16239 RVA: 0x000E21D2 File Offset: 0x000E03D2
	public float GridSpacing
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x170015A0 RID: 5536
	// (get) Token: 0x06003F70 RID: 16240 RVA: 0x000E21D9 File Offset: 0x000E03D9
	public Ferr2DT_ColorType ColorType
	{
		get
		{
			return Ferr2DT_ColorType.SolidColor;
		}
	}

	// Token: 0x170015A1 RID: 5537
	// (get) Token: 0x06003F71 RID: 16241 RVA: 0x000E21DC File Offset: 0x000E03DC
	public float VertexGradientDistance
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x170015A2 RID: 5538
	// (get) Token: 0x06003F72 RID: 16242 RVA: 0x000E21E3 File Offset: 0x000E03E3
	public Gradient VertexGradient
	{
		get
		{
			if (this.Master != null)
			{
				return this.Master.vertexGradient;
			}
			return null;
		}
	}

	// Token: 0x170015A3 RID: 5539
	// (get) Token: 0x06003F73 RID: 16243 RVA: 0x000E2200 File Offset: 0x000E0400
	public float PixelsPerUnit
	{
		get
		{
			if (this.Master != null)
			{
				return this.Master.pixelsPerUnit;
			}
			return -1f;
		}
	}

	// Token: 0x170015A4 RID: 5540
	// (get) Token: 0x06003F74 RID: 16244 RVA: 0x000E2221 File Offset: 0x000E0421
	public bool CreateTangents
	{
		get
		{
			return this.Master != null && this.Master.createTangents;
		}
	}

	// Token: 0x170015A5 RID: 5541
	// (get) Token: 0x06003F75 RID: 16245 RVA: 0x000E223E File Offset: 0x000E043E
	public int SortingLayer
	{
		get
		{
			if (this.Master != null)
			{
				return this.Master.GetComponent<Renderer>().sortingLayerID;
			}
			return 0;
		}
	}

	// Token: 0x170015A6 RID: 5542
	// (get) Token: 0x06003F76 RID: 16246 RVA: 0x000E2260 File Offset: 0x000E0460
	public float FillZ
	{
		get
		{
			if (this.Master != null)
			{
				return this.Master.fillZ;
			}
			return 0f;
		}
	}

	// Token: 0x170015A7 RID: 5543
	// (get) Token: 0x06003F77 RID: 16247 RVA: 0x000E2281 File Offset: 0x000E0481
	public int OrderInLayer
	{
		get
		{
			if (this.Master != null)
			{
				return this.Master.GetComponent<Renderer>().sortingOrder;
			}
			return -1;
		}
	}

	// Token: 0x06003F78 RID: 16248 RVA: 0x000E22A3 File Offset: 0x000E04A3
	public void Update(Ferr2DT_PathTerrain masterPrefab)
	{
		this.Master = masterPrefab;
	}

	// Token: 0x04002F1F RID: 12063
	[SerializeField]
	private Ferr2DT_PathTerrain m_master;
}
