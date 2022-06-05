using System;
using UnityEngine;

// Token: 0x020004C3 RID: 1219
public class DashBouncePlatform : SpecialPlatform, IRootObj
{
	// Token: 0x17001148 RID: 4424
	// (get) Token: 0x06002D50 RID: 11600 RVA: 0x00099701 File Offset: 0x00097901
	public override string EffectNameOverride
	{
		get
		{
			return this.m_bounceEffectName;
		}
	}

	// Token: 0x17001149 RID: 4425
	// (get) Token: 0x06002D51 RID: 11601 RVA: 0x00099709 File Offset: 0x00097909
	public override bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x0009970C File Offset: 0x0009790C
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x00099710 File Offset: 0x00097910
	public void Bounce()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CharacterDownStrike.ForceTriggerBounce(43.5f);
		EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, this.m_bounceEffectName, base.transform.position, -1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x00099770 File Offset: 0x00097970
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400245C RID: 9308
	private string m_bounceEffectName = "BounceProjectileIndicatorBounced_Effect";
}
