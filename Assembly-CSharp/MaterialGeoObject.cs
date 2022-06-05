using System;
using UnityEngine;

// Token: 0x02000B88 RID: 2952
[Serializable]
public class MaterialGeoObject
{
	// Token: 0x17001DC9 RID: 7625
	// (get) Token: 0x06005917 RID: 22807 RVA: 0x00030799 File Offset: 0x0002E999
	// (set) Token: 0x06005918 RID: 22808 RVA: 0x000307A1 File Offset: 0x0002E9A1
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

	// Token: 0x17001DCA RID: 7626
	// (get) Token: 0x06005919 RID: 22809 RVA: 0x000307AA File Offset: 0x0002E9AA
	// (set) Token: 0x0600591A RID: 22810 RVA: 0x000307B2 File Offset: 0x0002E9B2
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

	// Token: 0x17001DCB RID: 7627
	// (get) Token: 0x0600591B RID: 22811 RVA: 0x000307BB File Offset: 0x0002E9BB
	// (set) Token: 0x0600591C RID: 22812 RVA: 0x000307C3 File Offset: 0x0002E9C3
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

	// Token: 0x040042B1 RID: 17073
	[SerializeField]
	private EquipmentType m_equipmentType;

	// Token: 0x040042B2 RID: 17074
	[SerializeField]
	private Material m_material;

	// Token: 0x040042B3 RID: 17075
	[SerializeField]
	private GameObject m_customGeo;
}
