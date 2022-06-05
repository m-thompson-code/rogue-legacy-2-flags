using System;
using UnityEngine;

// Token: 0x020007E2 RID: 2018
public class DashBouncePlatform : SpecialPlatform, IRootObj
{
	// Token: 0x170016BD RID: 5821
	// (get) Token: 0x06003E26 RID: 15910 RVA: 0x0002265D File Offset: 0x0002085D
	public override string EffectNameOverride
	{
		get
		{
			return this.m_bounceEffectName;
		}
	}

	// Token: 0x170016BE RID: 5822
	// (get) Token: 0x06003E27 RID: 15911 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003E28 RID: 15912 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06003E29 RID: 15913 RVA: 0x000FA8B4 File Offset: 0x000F8AB4
	public void Bounce()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CharacterDownStrike.ForceTriggerBounce(43.5f);
		EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, this.m_bounceEffectName, base.transform.position, -1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040030DA RID: 12506
	private string m_bounceEffectName = "BounceProjectileIndicatorBounced_Effect";
}
