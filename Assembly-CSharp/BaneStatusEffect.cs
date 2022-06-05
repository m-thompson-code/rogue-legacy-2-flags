using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public class BaneStatusEffect : BaseStatusEffect, IDamageObj
{
	// Token: 0x17000CE8 RID: 3304
	// (get) Token: 0x06001DDB RID: 7643 RVA: 0x000621FE File Offset: 0x000603FE
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000CE9 RID: 3305
	// (get) Token: 0x06001DDC RID: 7644 RVA: 0x00062201 File Offset: 0x00060401
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000CEA RID: 3306
	// (get) Token: 0x06001DDD RID: 7645 RVA: 0x00062204 File Offset: 0x00060404
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Bane;
		}
	}

	// Token: 0x17000CEB RID: 3307
	// (get) Token: 0x06001DDE RID: 7646 RVA: 0x0006220B File Offset: 0x0006040B
	public override float StartingDurationOverride
	{
		get
		{
			return 1.55f;
		}
	}

	// Token: 0x17000CEC RID: 3308
	// (get) Token: 0x06001DDF RID: 7647 RVA: 0x00062212 File Offset: 0x00060412
	public bool IsDotDamage
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000CED RID: 3309
	// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x00062218 File Offset: 0x00060418
	public float BaseDamage
	{
		get
		{
			float num = 1f;
			return this.m_baneFocusDmg * num;
		}
	}

	// Token: 0x17000CEE RID: 3310
	// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x00062233 File Offset: 0x00060433
	public float ActualDamage
	{
		get
		{
			return this.BaseDamage;
		}
	}

	// Token: 0x17000CEF RID: 3311
	// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x0006223B File Offset: 0x0006043B
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000CF0 RID: 3312
	// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x00062242 File Offset: 0x00060442
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000CF1 RID: 3313
	// (get) Token: 0x06001DE4 RID: 7652 RVA: 0x00062249 File Offset: 0x00060449
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x17000CF2 RID: 3314
	// (get) Token: 0x06001DE5 RID: 7653 RVA: 0x00062250 File Offset: 0x00060450
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x17000CF3 RID: 3315
	// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x00062258 File Offset: 0x00060458
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x17000CF4 RID: 3316
	// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x00062260 File Offset: 0x00060460
	// (set) Token: 0x06001DE8 RID: 7656 RVA: 0x00062267 File Offset: 0x00060467
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

	// Token: 0x17000CF5 RID: 3317
	// (get) Token: 0x06001DE9 RID: 7657 RVA: 0x00062269 File Offset: 0x00060469
	// (set) Token: 0x06001DEA RID: 7658 RVA: 0x00062270 File Offset: 0x00060470
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

	// Token: 0x17000CF6 RID: 3318
	// (get) Token: 0x06001DEB RID: 7659 RVA: 0x00062272 File Offset: 0x00060472
	public float KnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000CF7 RID: 3319
	// (get) Token: 0x06001DEC RID: 7660 RVA: 0x00062279 File Offset: 0x00060479
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x17000CF8 RID: 3320
	// (get) Token: 0x06001DED RID: 7661 RVA: 0x0006227D File Offset: 0x0006047D
	public float StatusEffectDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x00062284 File Offset: 0x00060484
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#753B3B", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#451200", out this.m_addColorPulseOn);
		ColorUtility.TryParseHtmlString("#000000", out this.m_addColorPulseOff);
		base.AppliesTint = true;
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x000622D3 File Offset: 0x000604D3
	public override void StartEffect(float duration, IDamageObj caster)
	{
		if (!this.m_statusEffectController.HasStatusEffect(StatusEffectType.Enemy_Burn_Immunity))
		{
			base.StartEffect(duration, caster);
		}
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x000622EC File Offset: 0x000604EC
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

	// Token: 0x06001DF1 RID: 7665 RVA: 0x000622FB File Offset: 0x000604FB
	private IEnumerator DealDamageCoroutine()
	{
		base.Duration += 0.05f;
		for (;;)
		{
			float delayTime = Time.time + 1.5f - this.m_burnTicRemaining;
			while (Time.time < delayTime)
			{
				this.m_burnTicRemaining = 1.5f - (delayTime - Time.time);
				yield return null;
			}
			this.m_burnTicRemaining = 0f;
			this.m_charController.HitboxController.LastCollidedWith = null;
			this.m_charController.CharacterHitResponse.StartHitResponse(base.gameObject, this, -1f, false, true);
		}
		yield break;
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x0006230A File Offset: 0x0006050A
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		Projectile_RL projectile_RL = caster as Projectile_RL;
		PlayerController playerController = projectile_RL ? (projectile_RL.OwnerController as PlayerController) : (caster as PlayerController);
		if (playerController)
		{
			this.m_baneFocusDmg = playerController.ActualFocus;
		}
		else if (projectile_RL)
		{
			this.m_baneFocusDmg = projectile_RL.Magic;
		}
		else
		{
			this.m_baneFocusDmg = 0f;
		}
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Bane, base.Duration);
		this.m_pulseOn = false;
		foreach (Renderer renderer in this.m_charController.RendererArray)
		{
			renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._MultiplyColor, this.m_multiplyColor);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_addColorPulseOff);
			renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
		}
		this.m_baneEffect = EffectManager.PlayEffect(this.m_charController.gameObject, this.m_charController.Animator, "EnemyBurnFlames_Effect", Vector3.zero, base.Duration, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.m_baneEffect.transform.SetParent(this.m_charController.transform, false);
		this.m_baneEffect.transform.position = this.m_charController.Midpoint;
		base.StartCoroutine(this.PulseCoroutine());
		base.StartCoroutine(this.DealDamageCoroutine());
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x00062320 File Offset: 0x00060520
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (!interrupted)
		{
			this.m_burnTicRemaining = 0f;
		}
		if (this.m_baneEffect && this.m_baneEffect.isActiveAndEnabled)
		{
			this.m_baneEffect.Stop(EffectStopType.Gracefully);
		}
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x00062365 File Offset: 0x00060565
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001B83 RID: 7043
	private const string MULTIPLY_COLOR = "#753B3B";

	// Token: 0x04001B84 RID: 7044
	private const string PULSE_ON_COLOR = "#451200";

	// Token: 0x04001B85 RID: 7045
	private const string PULSE_OFF_COLOR = "#000000";

	// Token: 0x04001B86 RID: 7046
	private Color m_multiplyColor;

	// Token: 0x04001B87 RID: 7047
	private Color m_addColorPulseOn;

	// Token: 0x04001B88 RID: 7048
	private Color m_addColorPulseOff;

	// Token: 0x04001B89 RID: 7049
	private bool m_pulseOn;

	// Token: 0x04001B8A RID: 7050
	private float m_baneFocusDmg;

	// Token: 0x04001B8B RID: 7051
	private BaseEffect m_baneEffect;

	// Token: 0x04001B8C RID: 7052
	private float m_burnTicRemaining;
}
