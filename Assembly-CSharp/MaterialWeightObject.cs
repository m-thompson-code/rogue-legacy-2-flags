using System;
using UnityEngine;

// Token: 0x02000B8A RID: 2954
[Serializable]
public class MaterialWeightObject : ILookWeight
{
	// Token: 0x17001DCC RID: 7628
	// (get) Token: 0x0600591F RID: 22815 RVA: 0x000307CC File Offset: 0x0002E9CC
	public Material Material
	{
		get
		{
			return this.m_material;
		}
	}

	// Token: 0x17001DCD RID: 7629
	// (get) Token: 0x06005920 RID: 22816 RVA: 0x000307D4 File Offset: 0x0002E9D4
	public float MaleWeight
	{
		get
		{
			return this.m_maleWeight;
		}
	}

	// Token: 0x17001DCE RID: 7630
	// (get) Token: 0x06005921 RID: 22817 RVA: 0x000307DC File Offset: 0x0002E9DC
	public float FemaleWeight
	{
		get
		{
			return this.m_femaleWeight;
		}
	}

	// Token: 0x17001DCF RID: 7631
	// (get) Token: 0x06005922 RID: 22818 RVA: 0x000307E4 File Offset: 0x0002E9E4
	public string[] Tags
	{
		get
		{
			return this.m_tags;
		}
	}

	// Token: 0x17001DD0 RID: 7632
	// (get) Token: 0x06005923 RID: 22819 RVA: 0x000307EC File Offset: 0x0002E9EC
	public bool ExcludeFromWeighing
	{
		get
		{
			return this.m_excludeFromWeighing;
		}
	}

	// Token: 0x040042B5 RID: 17077
	[SerializeField]
	private Material m_material;

	// Token: 0x040042B6 RID: 17078
	[SerializeField]
	private bool m_excludeFromWeighing;

	// Token: 0x040042B7 RID: 17079
	[SerializeField]
	private float m_maleWeight = 1f;

	// Token: 0x040042B8 RID: 17080
	[SerializeField]
	private float m_femaleWeight = 1f;

	// Token: 0x040042B9 RID: 17081
	[SerializeField]
	private string[] m_tags;
}
