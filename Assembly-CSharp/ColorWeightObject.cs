using System;
using UnityEngine;

// Token: 0x02000B7A RID: 2938
[Serializable]
public class ColorWeightObject : ILookWeight
{
	// Token: 0x17001DBE RID: 7614
	// (get) Token: 0x060058F5 RID: 22773 RVA: 0x00030617 File Offset: 0x0002E817
	public Color Color
	{
		get
		{
			return this.m_color;
		}
	}

	// Token: 0x17001DBF RID: 7615
	// (get) Token: 0x060058F6 RID: 22774 RVA: 0x0003061F File Offset: 0x0002E81F
	public float MaleWeight
	{
		get
		{
			return this.m_maleWeight;
		}
	}

	// Token: 0x17001DC0 RID: 7616
	// (get) Token: 0x060058F7 RID: 22775 RVA: 0x00030627 File Offset: 0x0002E827
	public float FemaleWeight
	{
		get
		{
			return this.m_femaleWeight;
		}
	}

	// Token: 0x17001DC1 RID: 7617
	// (get) Token: 0x060058F8 RID: 22776 RVA: 0x0003062F File Offset: 0x0002E82F
	public string[] Tags
	{
		get
		{
			return this.m_tags;
		}
	}

	// Token: 0x17001DC2 RID: 7618
	// (get) Token: 0x060058F9 RID: 22777 RVA: 0x00030637 File Offset: 0x0002E837
	public bool ExcludeFromWeighing
	{
		get
		{
			return this.m_excludeFromWeighing;
		}
	}

	// Token: 0x04004204 RID: 16900
	[SerializeField]
	private Color m_color = Color.white;

	// Token: 0x04004205 RID: 16901
	[SerializeField]
	private bool m_excludeFromWeighing;

	// Token: 0x04004206 RID: 16902
	[SerializeField]
	private float m_maleWeight = 1f;

	// Token: 0x04004207 RID: 16903
	[SerializeField]
	private float m_femaleWeight = 1f;

	// Token: 0x04004208 RID: 16904
	[SerializeField]
	private string[] m_tags;
}
