using System;
using UnityEngine;

// Token: 0x02000227 RID: 551
[Serializable]
public class EffectEntry
{
	// Token: 0x17000B26 RID: 2854
	// (get) Token: 0x06001686 RID: 5766 RVA: 0x00046465 File Offset: 0x00044665
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

	// Token: 0x17000B27 RID: 2855
	// (get) Token: 0x06001687 RID: 5767 RVA: 0x00046486 File Offset: 0x00044686
	// (set) Token: 0x06001688 RID: 5768 RVA: 0x0004648E File Offset: 0x0004468E
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

	// Token: 0x17000B28 RID: 2856
	// (get) Token: 0x06001689 RID: 5769 RVA: 0x00046497 File Offset: 0x00044697
	// (set) Token: 0x0600168A RID: 5770 RVA: 0x0004649F File Offset: 0x0004469F
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

	// Token: 0x17000B29 RID: 2857
	// (get) Token: 0x0600168B RID: 5771 RVA: 0x000464A8 File Offset: 0x000446A8
	// (set) Token: 0x0600168C RID: 5772 RVA: 0x000464B0 File Offset: 0x000446B0
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

	// Token: 0x040015C2 RID: 5570
	[SerializeField]
	private BaseEffect m_effectPrefab;

	// Token: 0x040015C3 RID: 5571
	[SerializeField]
	private int m_effectPoolSize = 1;

	// Token: 0x040015C4 RID: 5572
	[SerializeField]
	private EffectType m_effectType;
}
