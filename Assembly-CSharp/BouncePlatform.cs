using System;
using UnityEngine;

// Token: 0x020007DD RID: 2013
public class BouncePlatform : SpecialPlatform, IRootObj
{
	// Token: 0x170016B5 RID: 5813
	// (get) Token: 0x06003DFF RID: 15871 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003E00 RID: 15872 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06003E02 RID: 15874 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}
}
