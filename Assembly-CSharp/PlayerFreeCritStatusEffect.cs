using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000311 RID: 785
public class PlayerFreeCritStatusEffect : BaseStatusEffect, IAudioEventEmitter
{
	// Token: 0x17000D6C RID: 3436
	// (get) Token: 0x06001F04 RID: 7940 RVA: 0x00063E5C File Offset: 0x0006205C
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_FreeCrit;
		}
	}

	// Token: 0x17000D6D RID: 3437
	// (get) Token: 0x06001F05 RID: 7941 RVA: 0x00063E63 File Offset: 0x00062063
	public override float StartingDurationOverride
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000D6E RID: 3438
	// (get) Token: 0x06001F06 RID: 7942 RVA: 0x00063E6A File Offset: 0x0006206A
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x00063E72 File Offset: 0x00062072
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

	// Token: 0x06001F08 RID: 7944 RVA: 0x00063E81 File Offset: 0x00062081
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().LookController.SetCritBlinkEffectEnabled(false, CritBlinkEffectTriggerType.PlayerFreeCritStatusEffect);
		}
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x00063EA2 File Offset: 0x000620A2
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
