using System;
using UnityEngine;

// Token: 0x020001F1 RID: 497
public class DownstrikeKnockbackOverride : MonoBehaviour, IRootObj
{
	// Token: 0x17000A81 RID: 2689
	// (get) Token: 0x06001469 RID: 5225 RVA: 0x0003DF06 File Offset: 0x0003C106
	public float Value
	{
		get
		{
			return this.m_knockbackOverride;
		}
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x0003DF16 File Offset: 0x0003C116
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400142B RID: 5163
	[SerializeField]
	private float m_knockbackOverride;
}
