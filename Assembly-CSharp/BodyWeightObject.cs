using System;
using UnityEngine;

// Token: 0x020006C5 RID: 1733
[Serializable]
public class BodyWeightObject : ILookWeight
{
	// Token: 0x170015BC RID: 5564
	// (get) Token: 0x06003FAA RID: 16298 RVA: 0x000E24FB File Offset: 0x000E06FB
	public BodyTypeWeightParam BodyTypeWeightParam
	{
		get
		{
			return this.m_bodyTypeWeightParam;
		}
	}

	// Token: 0x170015BD RID: 5565
	// (get) Token: 0x06003FAB RID: 16299 RVA: 0x000E2503 File Offset: 0x000E0703
	public int BlendShapeWeight
	{
		get
		{
			return this.m_blendShapeWeight;
		}
	}

	// Token: 0x170015BE RID: 5566
	// (get) Token: 0x06003FAC RID: 16300 RVA: 0x000E250B File Offset: 0x000E070B
	public float MaleWeight
	{
		get
		{
			return this.m_maleWeight;
		}
	}

	// Token: 0x170015BF RID: 5567
	// (get) Token: 0x06003FAD RID: 16301 RVA: 0x000E2513 File Offset: 0x000E0713
	public float FemaleWeight
	{
		get
		{
			return this.m_femaleWeight;
		}
	}

	// Token: 0x170015C0 RID: 5568
	// (get) Token: 0x06003FAE RID: 16302 RVA: 0x000E251B File Offset: 0x000E071B
	public string[] Tags
	{
		get
		{
			return this.m_tags;
		}
	}

	// Token: 0x170015C1 RID: 5569
	// (get) Token: 0x06003FAF RID: 16303 RVA: 0x000E2523 File Offset: 0x000E0723
	public bool ExcludeFromWeighing
	{
		get
		{
			return this.m_excludeFromWeighing;
		}
	}

	// Token: 0x04002F5D RID: 12125
	[SerializeField]
	private BodyTypeWeightParam m_bodyTypeWeightParam = BodyTypeWeightParam.None;

	// Token: 0x04002F5E RID: 12126
	[SerializeField]
	private int m_blendShapeWeight = 100;

	// Token: 0x04002F5F RID: 12127
	[SerializeField]
	private bool m_excludeFromWeighing;

	// Token: 0x04002F60 RID: 12128
	[SerializeField]
	private float m_maleWeight = 1f;

	// Token: 0x04002F61 RID: 12129
	[SerializeField]
	private float m_femaleWeight = 1f;

	// Token: 0x04002F62 RID: 12130
	[SerializeField]
	private string[] m_tags;
}
