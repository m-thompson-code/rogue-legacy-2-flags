using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class Cloak_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x06000D6C RID: 3436 RVA: 0x00028CFA File Offset: 0x00026EFA
	public bool IsCloaked
	{
		get
		{
			return this.m_isCloaked;
		}
	}

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x06000D6D RID: 3437 RVA: 0x00028D02 File Offset: 0x00026F02
	public override bool IgnoreStuckCheck
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000D6E RID: 3438 RVA: 0x00028D05 File Offset: 0x00026F05
	protected override void Awake()
	{
		base.Awake();
		this.m_matPropertyBlock = new MaterialPropertyBlock();
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x00028D18 File Offset: 0x00026F18
	private IEnumerator Start()
	{
		while (!this.m_abilityController)
		{
			yield return null;
		}
		this.m_lookController = this.m_abilityController.PlayerController.LookController;
		this.m_waitYield = new WaitRL_Yield(0f, false);
		yield break;
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x00028D27 File Offset: 0x00026F27
	public override void PreCastAbility()
	{
		this.m_abilityController.StopAllQueueCoroutines();
		base.PreCastAbility();
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x00028D3A File Offset: 0x00026F3A
	public override IEnumerator CastAbility()
	{
		this.m_beginCastingRelay.Dispatch();
		PlayerController playerController = this.m_abilityController.PlayerController;
		if (!this.m_isCloaked)
		{
			this.m_isCloaked = true;
			this.m_abilityController.StopAbility(CastAbilityType.Weapon, true);
			this.m_abilityController.StopAbility(CastAbilityType.Spell, true);
			playerController.CharacterHitResponse.StopInvincibleTime();
			playerController.CharacterHitResponse.SetInvincibleTime(3f, false, false);
			playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
			playerController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Cloak, 3f);
			playerController.MovementSpeedMod += 0.5f;
			this.ApplyCloakEffect(true);
			this.m_waitYield.CreateNew(3f, false);
			yield return this.m_waitYield;
			yield break;
		}
		if (this.m_blinkCoroutine != null)
		{
			base.StopCoroutine(this.m_blinkCoroutine);
			this.m_blinkCoroutine = null;
		}
		playerController.CharacterHitResponse.StopInvincibleTime();
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
		playerController.MovementSpeedMod -= 0.5f;
		this.RemoveCloakEffect(true);
		yield return null;
		yield break;
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x00028D49 File Offset: 0x00026F49
	private IEnumerator BlinkCoroutine()
	{
		float startTime = Time.time + 2f;
		while (Time.time < startTime)
		{
			yield return null;
		}
		float blinkTimer = Time.time + 1f;
		float toggleTimer = 0.1f;
		while (Time.time < blinkTimer && blinkTimer - Time.time > 0.1f)
		{
			if (toggleTimer <= 0f)
			{
				if (!this.m_baseEffectOn)
				{
					this.ApplyCloakEffect(false);
				}
				else
				{
					this.RemoveCloakEffect(false);
				}
				toggleTimer = 0.1f;
			}
			else
			{
				toggleTimer -= Time.deltaTime;
			}
			yield return null;
		}
		if (!this.m_baseEffectOn)
		{
			this.ApplyCloakEffect(false);
		}
		yield break;
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x00028D58 File Offset: 0x00026F58
	public override void StopAbility(bool abilityInterrupted)
	{
		if (!base.IsOnCooldown)
		{
			this.StartCooldownTimer();
		}
		this.m_isCloaked = false;
		this.m_abilityController.PlayerController.CharacterHitResponse.SetInvincibleTime(0.1f, false, false);
		this.m_abilityController.PlayerController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
		this.m_abilityController.PlayerController.StatusBarController.StopUIEffect(StatusBarEntryType.Cloak);
		if (this.m_baseEffectOn)
		{
			this.m_abilityController.PlayerController.MovementSpeedMod -= 0.5f;
			this.RemoveCloakEffect(true);
		}
		this.m_isCloakStriking = false;
		this.m_abilityController.PlayerController.CloakInterrupted = abilityInterrupted;
		if (this.m_abilityController.PlayerController.CastAbility.LastCastAbilityTypeCasted != CastAbilityType.Weapon)
		{
			this.m_abilityController.PlayerController.CloakInterrupted = false;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x00028E38 File Offset: 0x00027038
	private void ApplyCloakEffect(bool applyBaseEffect = true)
	{
		this.m_baseEffectOn = true;
		this.m_lookController.LeftEyeGeo.GetPropertyBlock(this.m_matPropertyBlock);
		this.m_storedEyeColor = this.m_matPropertyBlock.GetColor(ShaderID_RL._MainColor);
		this.m_storedAlphaBlendEyeColor = this.m_matPropertyBlock.GetColor(ShaderID_RL._AlphaBlendColor);
		this.m_storedRimEyeColor = this.m_matPropertyBlock.GetColor(ShaderID_RL._RimLightColor);
		this.m_abilityController.PlayerController.BlinkPulseEffect.ActivateBlackFill(BlackFillType.Cloak, 0f);
		this.m_matPropertyBlock.SetColor(ShaderID_RL._MainColor, this.m_eyeTint);
		this.m_matPropertyBlock.SetColor(ShaderID_RL._AlphaBlendColor, this.m_storedAlphaBlendEyeColor);
		this.m_matPropertyBlock.SetColor(ShaderID_RL._RimLightColor, this.m_storedRimEyeColor);
		this.m_lookController.LeftEyeGeo.SetPropertyBlock(this.m_matPropertyBlock);
		this.m_lookController.RightEyeGeo.SetPropertyBlock(this.m_matPropertyBlock);
		if (this.m_cloakEffect == null && applyBaseEffect)
		{
			EffectManager.PlayEffect(this.m_abilityController.PlayerController.gameObject, this.m_abilityController.PlayerController.Animator, "CloakPuff_Effect", this.m_abilityController.PlayerController.Midpoint, -1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			this.m_cloakEffect = EffectManager.PlayEffect(this.m_abilityController.PlayerController.gameObject, this.m_abilityController.PlayerController.Animator, "CloakTrail_Effect", Vector3.zero, 100000000f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			this.m_cloakEffect.transform.SetParent(this.m_abilityController.PlayerController.transform, false);
			this.m_cloakEffect.transform.position = this.m_abilityController.PlayerController.Midpoint;
			this.m_cloakEffect.DisableDestroyOnRoomChange = true;
		}
		this.m_cloakOnAudioEmitter.Play();
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x0002901C File Offset: 0x0002721C
	private void RemoveCloakEffect(bool removeBaseEffect = true)
	{
		this.m_baseEffectOn = false;
		this.m_abilityController.PlayerController.BlinkPulseEffect.DisableBlackFill(BlackFillType.Cloak, 0f);
		this.m_lookController.LeftEyeGeo.GetPropertyBlock(this.m_matPropertyBlock);
		this.m_matPropertyBlock.SetColor(ShaderID_RL._MainColor, this.m_storedEyeColor);
		this.m_matPropertyBlock.SetColor(ShaderID_RL._AlphaBlendColor, this.m_storedAlphaBlendEyeColor);
		this.m_matPropertyBlock.SetColor(ShaderID_RL._RimLightColor, this.m_storedRimEyeColor);
		this.m_lookController.LeftEyeGeo.SetPropertyBlock(this.m_matPropertyBlock);
		this.m_lookController.RightEyeGeo.SetPropertyBlock(this.m_matPropertyBlock);
		if (removeBaseEffect && this.m_cloakEffect != null && this.m_cloakEffect.IsPlaying && this.m_cloakEffect.Source == this.m_abilityController.PlayerController.gameObject)
		{
			EffectManager.PlayEffect(this.m_abilityController.PlayerController.gameObject, this.m_abilityController.PlayerController.Animator, "CloakPuff_Effect", this.m_abilityController.PlayerController.Midpoint, -1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			EffectManager.StopEffect(this.m_cloakEffect, EffectStopType.Gracefully);
			this.m_cloakEffect = null;
		}
		this.m_cloakOnAudioEmitter.Stop();
		this.m_cloakOffAudioEmitter.Play();
	}

	// Token: 0x040010CA RID: 4298
	[SerializeField]
	private StudioEventEmitter m_cloakOnAudioEmitter;

	// Token: 0x040010CB RID: 4299
	[SerializeField]
	private StudioEventEmitter m_cloakOffAudioEmitter;

	// Token: 0x040010CC RID: 4300
	private const float BLINK_DURATION = 1f;

	// Token: 0x040010CD RID: 4301
	private const float BLINK_INTERVAL = 0.1f;

	// Token: 0x040010CE RID: 4302
	private Color m_eyeTint = new Color(0.6f, 0f, 0f, 1f);

	// Token: 0x040010CF RID: 4303
	private WaitRL_Yield m_waitYield;

	// Token: 0x040010D0 RID: 4304
	private bool m_isCloaked;

	// Token: 0x040010D1 RID: 4305
	private PlayerLookController m_lookController;

	// Token: 0x040010D2 RID: 4306
	private BaseEffect m_cloakEffect;

	// Token: 0x040010D3 RID: 4307
	private bool m_baseEffectOn;

	// Token: 0x040010D4 RID: 4308
	private Coroutine m_blinkCoroutine;

	// Token: 0x040010D5 RID: 4309
	private bool m_isCloakStriking;

	// Token: 0x040010D6 RID: 4310
	private Color m_storedEyeColor;

	// Token: 0x040010D7 RID: 4311
	private Color m_storedAlphaBlendEyeColor;

	// Token: 0x040010D8 RID: 4312
	private Color m_storedRimEyeColor;

	// Token: 0x040010D9 RID: 4313
	private MaterialPropertyBlock m_matPropertyBlock;
}
