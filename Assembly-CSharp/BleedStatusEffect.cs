using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000502 RID: 1282
public class BleedStatusEffect : BaseStatusEffect, IDamageObj
{
	// Token: 0x170010BF RID: 4287
	// (get) Token: 0x06002970 RID: 10608 RVA: 0x0000F49B File Offset: 0x0000D69B
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170010C0 RID: 4288
	// (get) Token: 0x06002971 RID: 10609 RVA: 0x0000F49B File Offset: 0x0000D69B
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170010C1 RID: 4289
	// (get) Token: 0x06002972 RID: 10610 RVA: 0x000086B8 File Offset: 0x000068B8
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Bleed;
		}
	}

	// Token: 0x170010C2 RID: 4290
	// (get) Token: 0x06002973 RID: 10611 RVA: 0x00005D18 File Offset: 0x00003F18
	public override float StartingDurationOverride
	{
		get
		{
			return 1.55f;
		}
	}

	// Token: 0x170010C3 RID: 4291
	// (get) Token: 0x06002974 RID: 10612 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool IsDotDamage
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170010C4 RID: 4292
	// (get) Token: 0x06002975 RID: 10613 RVA: 0x000BFF7C File Offset: 0x000BE17C
	public float BaseDamage
	{
		get
		{
			float num = 0.25f;
			return this.m_bleedDexDmg * num;
		}
	}

	// Token: 0x170010C5 RID: 4293
	// (get) Token: 0x06002976 RID: 10614 RVA: 0x00017572 File Offset: 0x00015772
	public float ActualDamage
	{
		get
		{
			return this.BaseDamage;
		}
	}

	// Token: 0x170010C6 RID: 4294
	// (get) Token: 0x06002977 RID: 10615 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170010C7 RID: 4295
	// (get) Token: 0x06002978 RID: 10616 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170010C8 RID: 4296
	// (get) Token: 0x06002979 RID: 10617 RVA: 0x00005FA3 File Offset: 0x000041A3
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x170010C9 RID: 4297
	// (get) Token: 0x0600297A RID: 10618 RVA: 0x0001757A File Offset: 0x0001577A
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x170010CA RID: 4298
	// (get) Token: 0x0600297B RID: 10619 RVA: 0x00017582 File Offset: 0x00015782
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x170010CB RID: 4299
	// (get) Token: 0x0600297C RID: 10620 RVA: 0x00003CCB File Offset: 0x00001ECB
	// (set) Token: 0x0600297D RID: 10621 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x170010CC RID: 4300
	// (get) Token: 0x0600297E RID: 10622 RVA: 0x00003CCB File Offset: 0x00001ECB
	// (set) Token: 0x0600297F RID: 10623 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x170010CD RID: 4301
	// (get) Token: 0x06002980 RID: 10624 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float KnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170010CE RID: 4302
	// (get) Token: 0x06002981 RID: 10625 RVA: 0x000046FA File Offset: 0x000028FA
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x170010CF RID: 4303
	// (get) Token: 0x06002982 RID: 10626 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float StatusEffectDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002983 RID: 10627 RVA: 0x000BFF98 File Offset: 0x000BE198
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#753B3B", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#451200", out this.m_addColorPulseOn);
		ColorUtility.TryParseHtmlString("#000000", out this.m_addColorPulseOff);
		base.AppliesTint = true;
	}

	// Token: 0x06002984 RID: 10628 RVA: 0x0001739D File Offset: 0x0001559D
	public override void StartEffect(float duration, IDamageObj caster)
	{
		if (!this.m_statusEffectController.HasStatusEffect(StatusEffectType.Enemy_Burn_Immunity))
		{
			base.StartEffect(duration, caster);
		}
	}

	// Token: 0x06002985 RID: 10629 RVA: 0x0001758A File Offset: 0x0001578A
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

	// Token: 0x06002986 RID: 10630 RVA: 0x00017599 File Offset: 0x00015799
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

	// Token: 0x06002987 RID: 10631 RVA: 0x000175A8 File Offset: 0x000157A8
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

	// Token: 0x06002988 RID: 10632 RVA: 0x000175BE File Offset: 0x000157BE
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

	// Token: 0x0600298A RID: 10634 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040023F6 RID: 9206
	private const string MULTIPLY_COLOR = "#753B3B";

	// Token: 0x040023F7 RID: 9207
	private const string PULSE_ON_COLOR = "#451200";

	// Token: 0x040023F8 RID: 9208
	private const string PULSE_OFF_COLOR = "#000000";

	// Token: 0x040023F9 RID: 9209
	private Color m_multiplyColor;

	// Token: 0x040023FA RID: 9210
	private Color m_addColorPulseOn;

	// Token: 0x040023FB RID: 9211
	private Color m_addColorPulseOff;

	// Token: 0x040023FC RID: 9212
	private bool m_pulseOn;

	// Token: 0x040023FD RID: 9213
	private float m_bleedDexDmg;

	// Token: 0x040023FE RID: 9214
	private BaseEffect m_bleedEffect;

	// Token: 0x040023FF RID: 9215
	private float m_bleedTicRemaining;
}
