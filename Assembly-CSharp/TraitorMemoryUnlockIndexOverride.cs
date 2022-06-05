using System;
using UnityEngine;

// Token: 0x02000830 RID: 2096
public class TraitorMemoryUnlockIndexOverride : MonoBehaviour
{
	// Token: 0x1700175C RID: 5980
	// (get) Token: 0x060040BB RID: 16571 RVA: 0x00023C5D File Offset: 0x00021E5D
	public int TraitorMemoryUnlockIndex
	{
		get
		{
			return this.m_traitorMemoryUnlockIndex;
		}
	}

	// Token: 0x0400329B RID: 12955
	[SerializeField]
	private int m_traitorMemoryUnlockIndex = -1;
}
