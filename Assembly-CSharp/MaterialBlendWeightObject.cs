using System;
using UnityEngine;

// Token: 0x02000B86 RID: 2950
[Serializable]
public class MaterialBlendWeightObject
{
	// Token: 0x17001DC6 RID: 7622
	// (get) Token: 0x0600590F RID: 22799 RVA: 0x00030766 File Offset: 0x0002E966
	// (set) Token: 0x06005910 RID: 22800 RVA: 0x0003076E File Offset: 0x0002E96E
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

	// Token: 0x17001DC7 RID: 7623
	// (get) Token: 0x06005911 RID: 22801 RVA: 0x00030777 File Offset: 0x0002E977
	// (set) Token: 0x06005912 RID: 22802 RVA: 0x0003077F File Offset: 0x0002E97F
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

	// Token: 0x17001DC8 RID: 7624
	// (get) Token: 0x06005913 RID: 22803 RVA: 0x00030788 File Offset: 0x0002E988
	// (set) Token: 0x06005914 RID: 22804 RVA: 0x00030790 File Offset: 0x0002E990
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

	// Token: 0x040042AD RID: 17069
	[SerializeField]
	private EquipmentType m_equipmentType;

	// Token: 0x040042AE RID: 17070
	[SerializeField]
	private Material m_material;

	// Token: 0x040042AF RID: 17071
	[SerializeField]
	private int m_blendWeight;
}
