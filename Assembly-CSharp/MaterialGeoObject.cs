using System;
using UnityEngine;

// Token: 0x020006DD RID: 1757
[Serializable]
public class MaterialGeoObject
{
	// Token: 0x170015D1 RID: 5585
	// (get) Token: 0x06003FE0 RID: 16352 RVA: 0x000E2971 File Offset: 0x000E0B71
	// (set) Token: 0x06003FE1 RID: 16353 RVA: 0x000E2979 File Offset: 0x000E0B79
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

	// Token: 0x170015D2 RID: 5586
	// (get) Token: 0x06003FE2 RID: 16354 RVA: 0x000E2982 File Offset: 0x000E0B82
	// (set) Token: 0x06003FE3 RID: 16355 RVA: 0x000E298A File Offset: 0x000E0B8A
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

	// Token: 0x170015D3 RID: 5587
	// (get) Token: 0x06003FE4 RID: 16356 RVA: 0x000E2993 File Offset: 0x000E0B93
	// (set) Token: 0x06003FE5 RID: 16357 RVA: 0x000E299B File Offset: 0x000E0B9B
	public GameObject CustomGeo
	{
		get
		{
			return this.m_customGeo;
		}
		set
		{
			this.m_customGeo = value;
		}
	}

	// Token: 0x04003062 RID: 12386
	[SerializeField]
	private EquipmentType m_equipmentType;

	// Token: 0x04003063 RID: 12387
	[SerializeField]
	private Material m_material;

	// Token: 0x04003064 RID: 12388
	[SerializeField]
	private GameObject m_customGeo;
}
