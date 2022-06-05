using System;
using UnityEngine;

// Token: 0x020007F3 RID: 2035
public class SuperBouncePlatform : SpecialPlatform, IRootObj
{
	// Token: 0x170016D9 RID: 5849
	// (get) Token: 0x06003EAE RID: 16046 RVA: 0x00022A63 File Offset: 0x00020C63
	public override string EffectNameOverride
	{
		get
		{
			return this.m_bounceEffectName;
		}
	}

	// Token: 0x170016DA RID: 5850
	// (get) Token: 0x06003EAF RID: 16047 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003EB0 RID: 16048 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06003EB2 RID: 16050 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400312A RID: 12586
	private string m_bounceEffectName = "BounceProjectileIndicatorBounced_Effect";
}
