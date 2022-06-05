using System;
using UnityEngine;

// Token: 0x02000B64 RID: 2916
[Serializable]
public class Ferr2DSettings
{
	// Token: 0x17001D94 RID: 7572
	// (get) Token: 0x060058A2 RID: 22690 RVA: 0x000302D6 File Offset: 0x0002E4D6
	// (set) Token: 0x060058A3 RID: 22691 RVA: 0x000302DE File Offset: 0x0002E4DE
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

	// Token: 0x17001D95 RID: 7573
	// (get) Token: 0x060058A4 RID: 22692 RVA: 0x000302E7 File Offset: 0x0002E4E7
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

	// Token: 0x17001D96 RID: 7574
	// (get) Token: 0x060058A5 RID: 22693 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public bool InteriorGridVerts
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001D97 RID: 7575
	// (get) Token: 0x060058A6 RID: 22694 RVA: 0x00030309 File Offset: 0x0002E509
	public float GridSpacing
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x17001D98 RID: 7576
	// (get) Token: 0x060058A7 RID: 22695 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public Ferr2DT_ColorType ColorType
	{
		get
		{
			return Ferr2DT_ColorType.SolidColor;
		}
	}

	// Token: 0x17001D99 RID: 7577
	// (get) Token: 0x060058A8 RID: 22696 RVA: 0x00030309 File Offset: 0x0002E509
	public float VertexGradientDistance
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x17001D9A RID: 7578
	// (get) Token: 0x060058A9 RID: 22697 RVA: 0x00030310 File Offset: 0x0002E510
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

	// Token: 0x17001D9B RID: 7579
	// (get) Token: 0x060058AA RID: 22698 RVA: 0x0003032D File Offset: 0x0002E52D
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

	// Token: 0x17001D9C RID: 7580
	// (get) Token: 0x060058AB RID: 22699 RVA: 0x0003034E File Offset: 0x0002E54E
	public bool CreateTangents
	{
		get
		{
			return this.Master != null && this.Master.createTangents;
		}
	}

	// Token: 0x17001D9D RID: 7581
	// (get) Token: 0x060058AC RID: 22700 RVA: 0x0003036B File Offset: 0x0002E56B
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

	// Token: 0x17001D9E RID: 7582
	// (get) Token: 0x060058AD RID: 22701 RVA: 0x0003038D File Offset: 0x0002E58D
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

	// Token: 0x17001D9F RID: 7583
	// (get) Token: 0x060058AE RID: 22702 RVA: 0x000303AE File Offset: 0x0002E5AE
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

	// Token: 0x060058AF RID: 22703 RVA: 0x000303D0 File Offset: 0x0002E5D0
	public void Update(Ferr2DT_PathTerrain masterPrefab)
	{
		this.Master = masterPrefab;
	}

	// Token: 0x0400416E RID: 16750
	[SerializeField]
	private Ferr2DT_PathTerrain m_master;
}
