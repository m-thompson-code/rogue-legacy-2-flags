using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x020002C0 RID: 704
public class Cloak_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x170009B7 RID: 2487
	// (get) Token: 0x060014C9 RID: 5321 RVA: 0x0000A65F File Offset: 0x0000885F
	public bool IsCloaked
	{
		get
		{
			return this.m_isCloaked;
		}
	}

	// Token: 0x170009B8 RID: 2488
	// (get) Token: 0x060014CA RID: 5322 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool IgnoreStuckCheck
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x0000A667 File Offset: 0x00008867
	protected override void Awake()
	{
		base.Awake();
		this.m_matPropertyBlock = new MaterialPropertyBlock();
	}

	// Token: 0x060014CC RID: 5324 RVA: 0x0000A67A File Offset: 0x0000887A
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

	// Token: 0x060014CD RID: 5325 RVA: 0x0000A689 File Offset: 0x00008889
	public override void PreCastAbility()
	{
		this.m_abilityController.StopAllQueueCoroutines();
		base.PreCastAbility();
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x0000A69C File Offset: 0x0000889C
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

	// Token: 0x060014CF RID: 5327 RVA: 0x0000A6AB File Offset: 0x000088AB
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

	// Token: 0x060014D0 RID: 5328 RVA: 0x00087DA4 File Offset: 0x00085FA4
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

	// Token: 0x060014D1 RID: 5329 RVA: 0x00087E84 File Offset: 0x00086084
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

	// Token: 0x060014D2 RID: 5330 RVA: 0x00088068 File Offset: 0x00086268
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

	// Token: 0x0400162A RID: 5674
	[SerializeField]
	private StudioEventEmitter m_cloakOnAudioEmitter;

	// Token: 0x0400162B RID: 5675
	[SerializeField]
	private StudioEventEmitter m_cloakOffAudioEmitter;

	// Token: 0x0400162C RID: 5676
	private const float BLINK_DURATION = 1f;

	// Token: 0x0400162D RID: 5677
	private const float BLINK_INTERVAL = 0.1f;

	// Token: 0x0400162E RID: 5678
	private Color m_eyeTint = new Color(0.6f, 0f, 0f, 1f);

	// Token: 0x0400162F RID: 5679
	private WaitRL_Yield m_waitYield;

	// Token: 0x04001630 RID: 5680
	private bool m_isCloaked;

	// Token: 0x04001631 RID: 5681
	private PlayerLookController m_lookController;

	// Token: 0x04001632 RID: 5682
	private BaseEffect m_cloakEffect;

	// Token: 0x04001633 RID: 5683
	private bool m_baseEffectOn;

	// Token: 0x04001634 RID: 5684
	private Coroutine m_blinkCoroutine;

	// Token: 0x04001635 RID: 5685
	private bool m_isCloakStriking;

	// Token: 0x04001636 RID: 5686
	private Color m_storedEyeColor;

	// Token: 0x04001637 RID: 5687
	private Color m_storedAlphaBlendEyeColor;

	// Token: 0x04001638 RID: 5688
	private Color m_storedRimEyeColor;

	// Token: 0x04001639 RID: 5689
	private MaterialPropertyBlock m_matPropertyBlock;
}
