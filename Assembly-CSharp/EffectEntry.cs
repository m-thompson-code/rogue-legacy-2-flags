using System;
using UnityEngine;

// Token: 0x020003DE RID: 990
[Serializable]
public class EffectEntry
{
	// Token: 0x17000E4F RID: 3663
	// (get) Token: 0x0600202B RID: 8235 RVA: 0x000110D3 File Offset: 0x0000F2D3
	public string Name
	{
		get
		{
			if (this.m_effectPrefab != null)
			{
				return this.m_effectPrefab.name;
			}
			return "NULL";
		}
	}

	// Token: 0x17000E50 RID: 3664
	// (get) Token: 0x0600202C RID: 8236 RVA: 0x000110F4 File Offset: 0x0000F2F4
	// (set) Token: 0x0600202D RID: 8237 RVA: 0x000110FC File Offset: 0x0000F2FC
	public BaseEffect EffectPrefab
	{
		get
		{
			return this.m_effectPrefab;
		}
		set
		{
			this.m_effectPrefab = value;
		}
	}

	// Token: 0x17000E51 RID: 3665
	// (get) Token: 0x0600202E RID: 8238 RVA: 0x00011105 File Offset: 0x0000F305
	// (set) Token: 0x0600202F RID: 8239 RVA: 0x0001110D File Offset: 0x0000F30D
	public int EffectPoolSize
	{
		get
		{
			return this.m_effectPoolSize;
		}
		set
		{
			this.m_effectPoolSize = value;
		}
	}

	// Token: 0x17000E52 RID: 3666
	// (get) Token: 0x06002030 RID: 8240 RVA: 0x00011116 File Offset: 0x0000F316
	// (set) Token: 0x06002031 RID: 8241 RVA: 0x0001111E File Offset: 0x0000F31E
	public EffectType EffectType
	{
		get
		{
			return this.m_effectType;
		}
		set
		{
			this.m_effectType = value;
		}
	}

	// Token: 0x04001CD2 RID: 7378
	[SerializeField]
	private BaseEffect m_effectPrefab;

	// Token: 0x04001CD3 RID: 7379
	[SerializeField]
	private int m_effectPoolSize = 1;

	// Token: 0x04001CD4 RID: 7380
	[SerializeField]
	private EffectType m_effectType;
}
