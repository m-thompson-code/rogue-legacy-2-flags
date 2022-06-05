using System;
using UnityEngine;

// Token: 0x020004C1 RID: 1217
public class BouncePlatform : SpecialPlatform, IRootObj
{
	// Token: 0x17001146 RID: 4422
	// (get) Token: 0x06002D3B RID: 11579 RVA: 0x000994B2 File Offset: 0x000976B2
	public override bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002D3C RID: 11580 RVA: 0x000994B5 File Offset: 0x000976B5
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06002D3E RID: 11582 RVA: 0x000994BF File Offset: 0x000976BF
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}
}
