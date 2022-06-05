using System;
using UnityEngine;

// Token: 0x020004E5 RID: 1253
public class TraitorMemoryUnlockIndexOverride : MonoBehaviour
{
	// Token: 0x17001195 RID: 4501
	// (get) Token: 0x06002EEF RID: 12015 RVA: 0x000A00AF File Offset: 0x0009E2AF
	public int TraitorMemoryUnlockIndex
	{
		get
		{
			return this.m_traitorMemoryUnlockIndex;
		}
	}

	// Token: 0x0400255D RID: 9565
	[SerializeField]
	private int m_traitorMemoryUnlockIndex = -1;
}
