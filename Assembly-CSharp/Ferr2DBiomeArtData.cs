using System;
using UnityEngine;

// Token: 0x02000B65 RID: 2917
[Serializable]
public class Ferr2DBiomeArtData
{
	// Token: 0x17001DA0 RID: 7584
	// (get) Token: 0x060058B1 RID: 22705 RVA: 0x000303D9 File Offset: 0x0002E5D9
	// (set) Token: 0x060058B2 RID: 22706 RVA: 0x000303E1 File Offset: 0x0002E5E1
	public Color Tint
	{
		get
		{
			return this.m_tint;
		}
		private set
		{
			this.m_tint = value;
		}
	}

	// Token: 0x17001DA1 RID: 7585
	// (get) Token: 0x060058B3 RID: 22707 RVA: 0x000303EA File Offset: 0x0002E5EA
	// (set) Token: 0x060058B4 RID: 22708 RVA: 0x000303F2 File Offset: 0x0002E5F2
	public Color MapColor
	{
		get
		{
			return this.m_mapColor;
		}
		private set
		{
			this.m_mapColor = value;
		}
	}

	// Token: 0x17001DA2 RID: 7586
	// (get) Token: 0x060058B5 RID: 22709 RVA: 0x000303FB File Offset: 0x0002E5FB
	public MasterFerr2DEntry TerrainMaster
	{
		get
		{
			return this.m_terrainMaster;
		}
	}

	// Token: 0x17001DA3 RID: 7587
	// (get) Token: 0x060058B6 RID: 22710 RVA: 0x00030403 File Offset: 0x0002E603
	public MasterFerr2DEntry OneWayMaster
	{
		get
		{
			return this.m_oneWayMaster;
		}
	}

	// Token: 0x060058B7 RID: 22711 RVA: 0x0003040B File Offset: 0x0002E60B
	public void SetTint(Color tint)
	{
		this.Tint = tint;
	}

	// Token: 0x060058B8 RID: 22712 RVA: 0x00030414 File Offset: 0x0002E614
	public void SetMapColor(Color color)
	{
		this.MapColor = color;
	}

	// Token: 0x0400416F RID: 16751
	[SerializeField]
	private MasterFerr2DEntry m_terrainMaster;

	// Token: 0x04004170 RID: 16752
	[SerializeField]
	private MasterFerr2DEntry m_oneWayMaster;

	// Token: 0x04004171 RID: 16753
	[SerializeField]
	private Color m_tint = Color.white;

	// Token: 0x04004172 RID: 16754
	[SerializeField]
	private Color m_mapColor = Color.white;
}
