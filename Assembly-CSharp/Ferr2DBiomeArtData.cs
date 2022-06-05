using System;
using UnityEngine;

// Token: 0x020006BA RID: 1722
[Serializable]
public class Ferr2DBiomeArtData
{
	// Token: 0x170015A8 RID: 5544
	// (get) Token: 0x06003F7A RID: 16250 RVA: 0x000E22B4 File Offset: 0x000E04B4
	// (set) Token: 0x06003F7B RID: 16251 RVA: 0x000E22BC File Offset: 0x000E04BC
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

	// Token: 0x170015A9 RID: 5545
	// (get) Token: 0x06003F7C RID: 16252 RVA: 0x000E22C5 File Offset: 0x000E04C5
	// (set) Token: 0x06003F7D RID: 16253 RVA: 0x000E22CD File Offset: 0x000E04CD
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

	// Token: 0x170015AA RID: 5546
	// (get) Token: 0x06003F7E RID: 16254 RVA: 0x000E22D6 File Offset: 0x000E04D6
	public MasterFerr2DEntry TerrainMaster
	{
		get
		{
			return this.m_terrainMaster;
		}
	}

	// Token: 0x170015AB RID: 5547
	// (get) Token: 0x06003F7F RID: 16255 RVA: 0x000E22DE File Offset: 0x000E04DE
	public MasterFerr2DEntry OneWayMaster
	{
		get
		{
			return this.m_oneWayMaster;
		}
	}

	// Token: 0x06003F80 RID: 16256 RVA: 0x000E22E6 File Offset: 0x000E04E6
	public void SetTint(Color tint)
	{
		this.Tint = tint;
	}

	// Token: 0x06003F81 RID: 16257 RVA: 0x000E22EF File Offset: 0x000E04EF
	public void SetMapColor(Color color)
	{
		this.MapColor = color;
	}

	// Token: 0x04002F20 RID: 12064
	[SerializeField]
	private MasterFerr2DEntry m_terrainMaster;

	// Token: 0x04002F21 RID: 12065
	[SerializeField]
	private MasterFerr2DEntry m_oneWayMaster;

	// Token: 0x04002F22 RID: 12066
	[SerializeField]
	private Color m_tint = Color.white;

	// Token: 0x04002F23 RID: 12067
	[SerializeField]
	private Color m_mapColor = Color.white;
}
