using System;
using UnityEngine;

// Token: 0x02000B70 RID: 2928
[Serializable]
public class BodyWeightObject : ILookWeight
{
	// Token: 0x17001DB4 RID: 7604
	// (get) Token: 0x060058E1 RID: 22753 RVA: 0x0003059A File Offset: 0x0002E79A
	public BodyTypeWeightParam BodyTypeWeightParam
	{
		get
		{
			return this.m_bodyTypeWeightParam;
		}
	}

	// Token: 0x17001DB5 RID: 7605
	// (get) Token: 0x060058E2 RID: 22754 RVA: 0x000305A2 File Offset: 0x0002E7A2
	public int BlendShapeWeight
	{
		get
		{
			return this.m_blendShapeWeight;
		}
	}

	// Token: 0x17001DB6 RID: 7606
	// (get) Token: 0x060058E3 RID: 22755 RVA: 0x000305AA File Offset: 0x0002E7AA
	public float MaleWeight
	{
		get
		{
			return this.m_maleWeight;
		}
	}

	// Token: 0x17001DB7 RID: 7607
	// (get) Token: 0x060058E4 RID: 22756 RVA: 0x000305B2 File Offset: 0x0002E7B2
	public float FemaleWeight
	{
		get
		{
			return this.m_femaleWeight;
		}
	}

	// Token: 0x17001DB8 RID: 7608
	// (get) Token: 0x060058E5 RID: 22757 RVA: 0x000305BA File Offset: 0x0002E7BA
	public string[] Tags
	{
		get
		{
			return this.m_tags;
		}
	}

	// Token: 0x17001DB9 RID: 7609
	// (get) Token: 0x060058E6 RID: 22758 RVA: 0x000305C2 File Offset: 0x0002E7C2
	public bool ExcludeFromWeighing
	{
		get
		{
			return this.m_excludeFromWeighing;
		}
	}

	// Token: 0x040041AC RID: 16812
	[SerializeField]
	private BodyTypeWeightParam m_bodyTypeWeightParam = BodyTypeWeightParam.None;

	// Token: 0x040041AD RID: 16813
	[SerializeField]
	private int m_blendShapeWeight = 100;

	// Token: 0x040041AE RID: 16814
	[SerializeField]
	private bool m_excludeFromWeighing;

	// Token: 0x040041AF RID: 16815
	[SerializeField]
	private float m_maleWeight = 1f;

	// Token: 0x040041B0 RID: 16816
	[SerializeField]
	private float m_femaleWeight = 1f;

	// Token: 0x040041B1 RID: 16817
	[SerializeField]
	private string[] m_tags;
}
