using System;
using UnityEngine;

// Token: 0x020006DF RID: 1759
[Serializable]
public class MaterialWeightObject : ILookWeight
{
	// Token: 0x170015D4 RID: 5588
	// (get) Token: 0x06003FE8 RID: 16360 RVA: 0x000E29B4 File Offset: 0x000E0BB4
	public Material Material
	{
		get
		{
			return this.m_material;
		}
	}

	// Token: 0x170015D5 RID: 5589
	// (get) Token: 0x06003FE9 RID: 16361 RVA: 0x000E29BC File Offset: 0x000E0BBC
	public float MaleWeight
	{
		get
		{
			return this.m_maleWeight;
		}
	}

	// Token: 0x170015D6 RID: 5590
	// (get) Token: 0x06003FEA RID: 16362 RVA: 0x000E29C4 File Offset: 0x000E0BC4
	public float FemaleWeight
	{
		get
		{
			return this.m_femaleWeight;
		}
	}

	// Token: 0x170015D7 RID: 5591
	// (get) Token: 0x06003FEB RID: 16363 RVA: 0x000E29CC File Offset: 0x000E0BCC
	public string[] Tags
	{
		get
		{
			return this.m_tags;
		}
	}

	// Token: 0x170015D8 RID: 5592
	// (get) Token: 0x06003FEC RID: 16364 RVA: 0x000E29D4 File Offset: 0x000E0BD4
	public bool ExcludeFromWeighing
	{
		get
		{
			return this.m_excludeFromWeighing;
		}
	}

	// Token: 0x04003066 RID: 12390
	[SerializeField]
	private Material m_material;

	// Token: 0x04003067 RID: 12391
	[SerializeField]
	private bool m_excludeFromWeighing;

	// Token: 0x04003068 RID: 12392
	[SerializeField]
	private float m_maleWeight = 1f;

	// Token: 0x04003069 RID: 12393
	[SerializeField]
	private float m_femaleWeight = 1f;

	// Token: 0x0400306A RID: 12394
	[SerializeField]
	private string[] m_tags;
}
