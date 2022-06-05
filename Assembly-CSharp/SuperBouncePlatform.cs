using System;
using UnityEngine;

// Token: 0x020004CB RID: 1227
public class SuperBouncePlatform : SpecialPlatform, IRootObj
{
	// Token: 0x17001152 RID: 4434
	// (get) Token: 0x06002DA2 RID: 11682 RVA: 0x0009A228 File Offset: 0x00098428
	public override string EffectNameOverride
	{
		get
		{
			return this.m_bounceEffectName;
		}
	}

	// Token: 0x17001153 RID: 4435
	// (get) Token: 0x06002DA3 RID: 11683 RVA: 0x0009A230 File Offset: 0x00098430
	public override bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002DA4 RID: 11684 RVA: 0x0009A233 File Offset: 0x00098433
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06002DA6 RID: 11686 RVA: 0x0009A248 File Offset: 0x00098448
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400248D RID: 9357
	private string m_bounceEffectName = "BounceProjectileIndicatorBounced_Effect";
}
