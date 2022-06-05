using System;
using UnityEngine;

// Token: 0x020003F7 RID: 1015
[Serializable]
public class ItemDropEntry
{
	// Token: 0x17000E66 RID: 3686
	// (get) Token: 0x060020A1 RID: 8353 RVA: 0x00011470 File Offset: 0x0000F670
	// (set) Token: 0x060020A2 RID: 8354 RVA: 0x00011478 File Offset: 0x0000F678
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

	// Token: 0x17000E67 RID: 3687
	// (get) Token: 0x060020A3 RID: 8355 RVA: 0x00011481 File Offset: 0x0000F681
	// (set) Token: 0x060020A4 RID: 8356 RVA: 0x00011489 File Offset: 0x0000F689
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

	// Token: 0x04001D8D RID: 7565
	[SerializeField]
	private int m_itemPoolSize = 5;

	// Token: 0x04001D8E RID: 7566
	[SerializeField]
	private BaseItemDrop m_itemDropPrefab;
}
