using System;
using UnityEngine;

// Token: 0x0200023A RID: 570
[Serializable]
public class ItemDropEntry
{
	// Token: 0x17000B39 RID: 2873
	// (get) Token: 0x060016EE RID: 5870 RVA: 0x00047B45 File Offset: 0x00045D45
	// (set) Token: 0x060016EF RID: 5871 RVA: 0x00047B4D File Offset: 0x00045D4D
	public int ItemDropPoolSize
	{
		get
		{
			return this.m_itemPoolSize;
		}
		set
		{
			this.m_itemPoolSize = value;
		}
	}

	// Token: 0x17000B3A RID: 2874
	// (get) Token: 0x060016F0 RID: 5872 RVA: 0x00047B56 File Offset: 0x00045D56
	// (set) Token: 0x060016F1 RID: 5873 RVA: 0x00047B5E File Offset: 0x00045D5E
	public BaseItemDrop ItemDropPrefab
	{
		get
		{
			return this.m_itemDropPrefab;
		}
		set
		{
			this.m_itemDropPrefab = value;
		}
	}

	// Token: 0x04001675 RID: 5749
	[SerializeField]
	private int m_itemPoolSize = 5;

	// Token: 0x04001676 RID: 5750
	[SerializeField]
	private BaseItemDrop m_itemDropPrefab;
}
