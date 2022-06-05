using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004FD RID: 1277
public class BaneStatusEffect : BaseStatusEffect, IDamageObj
{
	// Token: 0x1700109B RID: 4251
	// (get) Token: 0x06002926 RID: 10534 RVA: 0x0000F49B File Offset: 0x0000D69B
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1700109C RID: 4252
	// (get) Token: 0x06002927 RID: 10535 RVA: 0x0000F49B File Offset: 0x0000D69B
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1700109D RID: 4253
	// (get) Token: 0x06002928 RID: 10536 RVA: 0x0001737E File Offset: 0x0001557E
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Bane;
		}
	}

	// Token: 0x1700109E RID: 4254
	// (get) Token: 0x06002929 RID: 10537 RVA: 0x00005D18 File Offset: 0x00003F18
	public override float StartingDurationOverride
	{
		get
		{
			return 1.55f;
		}
	}

	// Token: 0x1700109F RID: 4255
	// (get) Token: 0x0600292A RID: 10538 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool IsDotDamage
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170010A0 RID: 4256
	// (get) Token: 0x0600292B RID: 10539 RVA: 0x000BF930 File Offset: 0x000BDB30
	public float BaseDamage
	{
		get
		{
			float num = 1f;
			return this.m_baneFocusDmg * num;
		}
	}

	// Token: 0x170010A1 RID: 4257
	// (get) Token: 0x0600292C RID: 10540 RVA: 0x00017385 File Offset: 0x00015585
	public float ActualDamage
	{
		get
		{
			return this.BaseDamage;
		}
	}

	// Token: 0x170010A2 RID: 4258
	// (get) Token: 0x0600292D RID: 10541 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170010A3 RID: 4259
	// (get) Token: 0x0600292E RID: 10542 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170010A4 RID: 4260
	// (get) Token: 0x0600292F RID: 10543 RVA: 0x00005FA3 File Offset: 0x000041A3
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x170010A5 RID: 4261
	// (get) Token: 0x06002930 RID: 10544 RVA: 0x0001738D File Offset: 0x0001558D
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x170010A6 RID: 4262
	// (get) Token: 0x06002931 RID: 10545 RVA: 0x00017395 File Offset: 0x00015595
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x170010A7 RID: 4263
	// (get) Token: 0x06002932 RID: 10546 RVA: 0x00003CCB File Offset: 0x00001ECB
	// (set) Token: 0x06002933 RID: 10547 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x170010A8 RID: 4264
	// (get) Token: 0x06002934 RID: 10548 RVA: 0x00003CCB File Offset: 0x00001ECB
	// (set) Token: 0x06002935 RID: 10549 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x170010A9 RID: 4265
	// (get) Token: 0x06002936 RID: 10550 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float KnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170010AA RID: 4266
	// (get) Token: 0x06002937 RID: 10551 RVA: 0x000046FA File Offset: 0x000028FA
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x170010AB RID: 4267
	// (get) Token: 0x06002938 RID: 10552 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float StatusEffectDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002939 RID: 10553 RVA: 0x000BF94C File Offset: 0x000BDB4C
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#753B3B", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#451200", out this.m_addColorPulseOn);
		ColorUtility.TryParseHtmlString("#000000", out this.m_addColorPulseOff);
		base.AppliesTint = true;
	}

	// Token: 0x0600293A RID: 10554 RVA: 0x0001739D File Offset: 0x0001559D
	public override void StartEffect(float duration, IDamageObj caster)
	{
		if (!this.m_statusEffectController.HasStatusEffect(StatusEffectType.Enemy_Burn_Immunity))
		{
			base.StartEffect(duration, caster);
		}
	}

	// Token: 0x0600293B RID: 10555 RVA: 0x000173B6 File Offset: 0x000155B6
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

	// Token: 0x0600293C RID: 10556 RVA: 0x000173C5 File Offset: 0x000155C5
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

	// Token: 0x0600293D RID: 10557 RVA: 0x000173D4 File Offset: 0x000155D4
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

	// Token: 0x0600293E RID: 10558 RVA: 0x000173EA File Offset: 0x000155EA
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

	// Token: 0x06002940 RID: 10560 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040023D3 RID: 9171
	private const string MULTIPLY_COLOR = "#753B3B";

	// Token: 0x040023D4 RID: 9172
	private const string PULSE_ON_COLOR = "#451200";

	// Token: 0x040023D5 RID: 9173
	private const string PULSE_OFF_COLOR = "#000000";

	// Token: 0x040023D6 RID: 9174
	private Color m_multiplyColor;

	// Token: 0x040023D7 RID: 9175
	private Color m_addColorPulseOn;

	// Token: 0x040023D8 RID: 9176
	private Color m_addColorPulseOff;

	// Token: 0x040023D9 RID: 9177
	private bool m_pulseOn;

	// Token: 0x040023DA RID: 9178
	private float m_baneFocusDmg;

	// Token: 0x040023DB RID: 9179
	private BaseEffect m_baneEffect;

	// Token: 0x040023DC RID: 9180
	private float m_burnTicRemaining;
}
