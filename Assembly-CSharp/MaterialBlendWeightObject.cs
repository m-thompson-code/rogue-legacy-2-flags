using System;
using UnityEngine;

// Token: 0x020006DB RID: 1755
[Serializable]
public class MaterialBlendWeightObject
{
	// Token: 0x170015CE RID: 5582
	// (get) Token: 0x06003FD8 RID: 16344 RVA: 0x000E292E File Offset: 0x000E0B2E
	// (set) Token: 0x06003FD9 RID: 16345 RVA: 0x000E2936 File Offset: 0x000E0B36
	public EquipmentType EquipmentType
	{
		get
		{
			return this.m_equipmentType;
		}
		set
		{
			this.m_equipmentType = value;
		}
	}

	// Token: 0x170015CF RID: 5583
	// (get) Token: 0x06003FDA RID: 16346 RVA: 0x000E293F File Offset: 0x000E0B3F
	// (set) Token: 0x06003FDB RID: 16347 RVA: 0x000E2947 File Offset: 0x000E0B47
	public Material Material
	{
		get
		{
			return this.m_material;
		}
		set
		{
			this.m_material = value;
		}
	}

	// Token: 0x170015D0 RID: 5584
	// (get) Token: 0x06003FDC RID: 16348 RVA: 0x000E2950 File Offset: 0x000E0B50
	// (set) Token: 0x06003FDD RID: 16349 RVA: 0x000E2958 File Offset: 0x000E0B58
	public int BlendWeight
	{
		get
		{
			return this.m_blendWeight;
		}
		set
		{
			this.m_blendWeight = value;
		}
	}

	// Token: 0x0400305E RID: 12382
	[SerializeField]
	private EquipmentType m_equipmentType;

	// Token: 0x0400305F RID: 12383
	[SerializeField]
	private Material m_material;

	// Token: 0x04003060 RID: 12384
	[SerializeField]
	private int m_blendWeight;
}
