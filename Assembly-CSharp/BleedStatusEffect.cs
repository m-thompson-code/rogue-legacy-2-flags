using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002F3 RID: 755
public class BleedStatusEffect : BaseStatusEffect, IDamageObj
{
	// Token: 0x17000D06 RID: 3334
	// (get) Token: 0x06001E13 RID: 7699 RVA: 0x00062651 File Offset: 0x00060851
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000D07 RID: 3335
	// (get) Token: 0x06001E14 RID: 7700 RVA: 0x00062654 File Offset: 0x00060854
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000D08 RID: 3336
	// (get) Token: 0x06001E15 RID: 7701 RVA: 0x00062657 File Offset: 0x00060857
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Bleed;
		}
	}

	// Token: 0x17000D09 RID: 3337
	// (get) Token: 0x06001E16 RID: 7702 RVA: 0x0006265E File Offset: 0x0006085E
	public override float StartingDurationOverride
	{
		get
		{
			return 1.55f;
		}
	}

	// Token: 0x17000D0A RID: 3338
	// (get) Token: 0x06001E17 RID: 7703 RVA: 0x00062665 File Offset: 0x00060865
	public bool IsDotDamage
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000D0B RID: 3339
	// (get) Token: 0x06001E18 RID: 7704 RVA: 0x00062668 File Offset: 0x00060868
	public float BaseDamage
	{
		get
		{
			float num = 0.25f;
			return this.m_bleedDexDmg * num;
		}
	}

	// Token: 0x17000D0C RID: 3340
	// (get) Token: 0x06001E19 RID: 7705 RVA: 0x00062683 File Offset: 0x00060883
	public float ActualDamage
	{
		get
		{
			return this.BaseDamage;
		}
	}

	// Token: 0x17000D0D RID: 3341
	// (get) Token: 0x06001E1A RID: 7706 RVA: 0x0006268B File Offset: 0x0006088B
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000D0E RID: 3342
	// (get) Token: 0x06001E1B RID: 7707 RVA: 0x00062692 File Offset: 0x00060892
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000D0F RID: 3343
	// (get) Token: 0x06001E1C RID: 7708 RVA: 0x00062699 File Offset: 0x00060899
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x17000D10 RID: 3344
	// (get) Token: 0x06001E1D RID: 7709 RVA: 0x000626A0 File Offset: 0x000608A0
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x17000D11 RID: 3345
	// (get) Token: 0x06001E1E RID: 7710 RVA: 0x000626A8 File Offset: 0x000608A8
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x17000D12 RID: 3346
	// (get) Token: 0x06001E1F RID: 7711 RVA: 0x000626B0 File Offset: 0x000608B0
	// (set) Token: 0x06001E20 RID: 7712 RVA: 0x000626B7 File Offset: 0x000608B7
	public float BaseStunStrength
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	// Token: 0x17000D13 RID: 3347
	// (get) Token: 0x06001E21 RID: 7713 RVA: 0x000626B9 File Offset: 0x000608B9
	// (set) Token: 0x06001E22 RID: 7714 RVA: 0x000626C0 File Offset: 0x000608C0
	public float BaseKnockbackStrength
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	// Token: 0x17000D14 RID: 3348
	// (get) Token: 0x06001E23 RID: 7715 RVA: 0x000626C2 File Offset: 0x000608C2
	public float KnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000D15 RID: 3349
	// (get) Token: 0x06001E24 RID: 7716 RVA: 0x000626C9 File Offset: 0x000608C9
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x17000D16 RID: 3350
	// (get) Token: 0x06001E25 RID: 7717 RVA: 0x000626CD File Offset: 0x000608CD
	public float StatusEffectDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x000626D4 File Offset: 0x000608D4
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#753B3B", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#451200", out this.m_addColorPulseOn);
		ColorUtility.TryParseHtmlString("#000000", out this.m_addColorPulseOff);
		base.AppliesTint = true;
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x00062723 File Offset: 0x00060923
	public override void StartEffect(float duration, IDamageObj caster)
	{
		if (!this.m_statusEffectController.HasStatusEffect(StatusEffectType.Enemy_Burn_Immunity))
		{
			base.StartEffect(duration, caster);
		}
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x0006273C File Offset: 0x0006093C
	private IEnumerator PulseCoroutine()
	{
		for (;;)
		{
			this.m_waitYield.CreateNew(0.1f, false);
			yield return this.m_waitYield;
			if (this.m_pulseOn)
			{
				using (List<Renderer>.Enumerator enumerator = this.m_charController.RendererArray.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Renderer renderer = enumerator.Current;
						renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
						BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_addColorPulseOn);
						renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
					}
					goto IL_10D;
				}
				goto IL_B0;
			}
			goto IL_B0;
			IL_10D:
			this.m_pulseOn = !this.m_pulseOn;
			continue;
			IL_B0:
			foreach (Renderer renderer2 in this.m_charController.RendererArray)
			{
				renderer2.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
				BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_addColorPulseOff);
				renderer2.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
			}
			goto IL_10D;
		}
		yield break;
	}

	// Token: 0x06001E29 RID: 7721 RVA: 0x0006274B File Offset: 0x0006094B
	private IEnumerator DealDamageCoroutine()
	{
		base.Duration += 0.05f;
		for (;;)
		{
			float delayTime = Time.time + 0.5f - this.m_bleedTicRemaining;
			while (Time.time < delayTime)
			{
				this.m_bleedTicRemaining = 0.5f - (delayTime - Time.time);
				yield return null;
			}
			this.m_bleedTicRemaining = 0f;
			this.m_charController.HitboxController.LastCollidedWith = null;
			this.m_charController.CharacterHitResponse.StartHitResponse(base.gameObject, this, -1f, false, true);
		}
		yield break;
	}

	// Token: 0x06001E2A RID: 7722 RVA: 0x0006275A File Offset: 0x0006095A
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		Projectile_RL projectile_RL = caster as Projectile_RL;
		PlayerController playerController = projectile_RL ? (projectile_RL.OwnerController as PlayerController) : (caster as PlayerController);
		if (playerController)
		{
			this.m_bleedDexDmg = playerController.ActualDexterity;
		}
		else if (projectile_RL)
		{
			this.m_bleedDexDmg = projectile_RL.Magic;
		}
		else
		{
			this.m_bleedDexDmg = 0f;
		}
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Bleed, base.Duration);
		this.m_pulseOn = false;
		foreach (Renderer renderer in this.m_charController.RendererArray)
		{
			renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._MultiplyColor, this.m_multiplyColor);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_addColorPulseOff);
			renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
		}
		this.m_bleedEffect = EffectManager.PlayEffect(this.m_charController.gameObject, this.m_charController.Animator, "EnemyBurnFlames_Effect", Vector3.zero, base.Duration, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.m_bleedEffect.transform.SetParent(this.m_charController.transform, false);
		this.m_bleedEffect.transform.position = this.m_charController.Midpoint;
		base.StartCoroutine(this.PulseCoroutine());
		base.StartCoroutine(this.DealDamageCoroutine());
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06001E2B RID: 7723 RVA: 0x00062770 File Offset: 0x00060970
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (!interrupted)
		{
			this.m_bleedTicRemaining = 0f;
		}
		if (this.m_bleedEffect && this.m_bleedEffect.isActiveAndEnabled)
		{
			this.m_bleedEffect.Stop(EffectStopType.Gracefully);
		}
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x000627B5 File Offset: 0x000609B5
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001B9B RID: 7067
	private const string MULTIPLY_COLOR = "#753B3B";

	// Token: 0x04001B9C RID: 7068
	private const string PULSE_ON_COLOR = "#451200";

	// Token: 0x04001B9D RID: 7069
	private const string PULSE_OFF_COLOR = "#000000";

	// Token: 0x04001B9E RID: 7070
	private Color m_multiplyColor;

	// Token: 0x04001B9F RID: 7071
	private Color m_addColorPulseOn;

	// Token: 0x04001BA0 RID: 7072
	private Color m_addColorPulseOff;

	// Token: 0x04001BA1 RID: 7073
	private bool m_pulseOn;

	// Token: 0x04001BA2 RID: 7074
	private float m_bleedDexDmg;

	// Token: 0x04001BA3 RID: 7075
	private BaseEffect m_bleedEffect;

	// Token: 0x04001BA4 RID: 7076
	private float m_bleedTicRemaining;
}
