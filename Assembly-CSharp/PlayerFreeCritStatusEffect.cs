using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000548 RID: 1352
public class PlayerFreeCritStatusEffect : BaseStatusEffect, IAudioEventEmitter
{
	// Token: 0x17001175 RID: 4469
	// (get) Token: 0x06002B51 RID: 11089 RVA: 0x00018298 File Offset: 0x00016498
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_FreeCrit;
		}
	}

	// Token: 0x17001176 RID: 4470
	// (get) Token: 0x06002B52 RID: 11090 RVA: 0x00004536 File Offset: 0x00002736
	public override float StartingDurationOverride
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17001177 RID: 4471
	// (get) Token: 0x06002B53 RID: 11091 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06002B54 RID: 11092 RVA: 0x0001829F File Offset: 0x0001649F
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.FreeCrit, base.Duration);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.LookController.SetCritBlinkEffectEnabled(true, CritBlinkEffectTriggerType.PlayerFreeCritStatusEffect);
		if (base.TimesStacked <= 1)
		{
			this.PlayChargedStartAudio(playerController);
		}
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002B55 RID: 11093 RVA: 0x000182AE File Offset: 0x000164AE
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().LookController.SetCritBlinkEffectEnabled(false, CritBlinkEffectTriggerType.PlayerFreeCritStatusEffect);
		}
	}

	// Token: 0x06002B56 RID: 11094 RVA: 0x000182CF File Offset: 0x000164CF
	private void PlayChargedStartAudio(PlayerController playerController)
	{
		if (playerController.CharacterClass.ClassType == ClassType.MagicWandClass)
		{
			AudioManager.PlayOneShotAttached(this, "event:/SFX/Weapons/sfx_weapon_crit_wand_charged", this.m_charController.gameObject);
			return;
		}
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Weapons/sfx_weapon_crit_charged_start", this.m_charController.gameObject);
	}
}
