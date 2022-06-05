using System;
using UnityEngine;

// Token: 0x020006CF RID: 1743
[Serializable]
public class ColorWeightObject : ILookWeight
{
	// Token: 0x170015C6 RID: 5574
	// (get) Token: 0x06003FBE RID: 16318 RVA: 0x000E25C0 File Offset: 0x000E07C0
	public Color Color
	{
		get
		{
			return this.m_color;
		}
	}

	// Token: 0x170015C7 RID: 5575
	// (get) Token: 0x06003FBF RID: 16319 RVA: 0x000E25C8 File Offset: 0x000E07C8
	public float MaleWeight
	{
		get
		{
			return this.m_maleWeight;
		}
	}

	// Token: 0x170015C8 RID: 5576
	// (get) Token: 0x06003FC0 RID: 16320 RVA: 0x000E25D0 File Offset: 0x000E07D0
	public float FemaleWeight
	{
		get
		{
			return this.m_femaleWeight;
		}
	}

	// Token: 0x170015C9 RID: 5577
	// (get) Token: 0x06003FC1 RID: 16321 RVA: 0x000E25D8 File Offset: 0x000E07D8
	public string[] Tags
	{
		get
		{
			return this.m_tags;
		}
	}

	// Token: 0x170015CA RID: 5578
	// (get) Token: 0x06003FC2 RID: 16322 RVA: 0x000E25E0 File Offset: 0x000E07E0
	public bool ExcludeFromWeighing
	{
		get
		{
			return this.m_excludeFromWeighing;
		}
	}

	// Token: 0x04002FB5 RID: 12213
	[SerializeField]
	private Color m_color = Color.white;

	// Token: 0x04002FB6 RID: 12214
	[SerializeField]
	private bool m_excludeFromWeighing;

	// Token: 0x04002FB7 RID: 12215
	[SerializeField]
	private float m_maleWeight = 1f;

	// Token: 0x04002FB8 RID: 12216
	[SerializeField]
	private float m_femaleWeight = 1f;

	// Token: 0x04002FB9 RID: 12217
	[SerializeField]
	private string[] m_tags;
}
