using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000508 RID: 1288
public class BurnStatusEffect : BaseStatusEffect, IDamageObj
{
	// Token: 0x170010DA RID: 4314
	// (get) Token: 0x060029A7 RID: 10663 RVA: 0x0000F49B File Offset: 0x0000D69B
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170010DB RID: 4315
	// (get) Token: 0x060029A8 RID: 10664 RVA: 0x0000F49B File Offset: 0x0000D69B
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170010DC RID: 4316
	// (get) Token: 0x060029A9 RID: 10665 RVA: 0x000054AD File Offset: 0x000036AD
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Burn;
		}
	}

	// Token: 0x170010DD RID: 4317
	// (get) Token: 0x060029AA RID: 10666 RVA: 0x0001766A File Offset: 0x0001586A
	public override float StartingDurationOverride
	{
		get
		{
			return 3.05f;
		}
	}

	// Token: 0x170010DE RID: 4318
	// (get) Token: 0x060029AB RID: 10667 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool IsDotDamage
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170010DF RID: 4319
	// (get) Token: 0x060029AC RID: 10668 RVA: 0x000C0460 File Offset: 0x000BE660
	public float BaseDamage
	{
		get
		{
			float num = 0.55f;
			return this.m_burnMagicDmg * num;
		}
	}

	// Token: 0x170010E0 RID: 4320
	// (get) Token: 0x060029AD RID: 10669 RVA: 0x00017671 File Offset: 0x00015871
	public float ActualDamage
	{
		get
		{
			return this.BaseDamage;
		}
	}

	// Token: 0x170010E1 RID: 4321
	// (get) Token: 0x060029AE RID: 10670 RVA: 0x00017679 File Offset: 0x00015879
	public float ActualCritChance
	{
		get
		{
			if (this.m_isCritHit)
			{
				return 100f;
			}
			return 0f;
		}
	}

	// Token: 0x170010E2 RID: 4322
	// (get) Token: 0x060029AF RID: 10671 RVA: 0x0001768E File Offset: 0x0001588E
	// (set) Token: 0x060029B0 RID: 10672 RVA: 0x00017696 File Offset: 0x00015896
	public float ActualCritDamage { get; private set; }

	// Token: 0x170010E3 RID: 4323
	// (get) Token: 0x060029B1 RID: 10673 RVA: 0x00005FA3 File Offset: 0x000041A3
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x170010E4 RID: 4324
	// (get) Token: 0x060029B2 RID: 10674 RVA: 0x0001769F File Offset: 0x0001589F
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x170010E5 RID: 4325
	// (get) Token: 0x060029B3 RID: 10675 RVA: 0x000176A7 File Offset: 0x000158A7
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x170010E6 RID: 4326
	// (get) Token: 0x060029B4 RID: 10676 RVA: 0x00003CCB File Offset: 0x00001ECB
	// (set) Token: 0x060029B5 RID: 10677 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x170010E7 RID: 4327
	// (get) Token: 0x060029B6 RID: 10678 RVA: 0x00003CCB File Offset: 0x00001ECB
	// (set) Token: 0x060029B7 RID: 10679 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x170010E8 RID: 4328
	// (get) Token: 0x060029B8 RID: 10680 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float KnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170010E9 RID: 4329
	// (get) Token: 0x060029B9 RID: 10681 RVA: 0x000046FA File Offset: 0x000028FA
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x170010EA RID: 4330
	// (get) Token: 0x060029BA RID: 10682 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float StatusEffectDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x000C047C File Offset: 0x000BE67C
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#753B3B", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#451200", out this.m_addColorPulseOn);
		ColorUtility.TryParseHtmlString("#000000", out this.m_addColorPulseOff);
		base.AppliesTint = true;
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x0001739D File Offset: 0x0001559D
	public override void StartEffect(float duration, IDamageObj caster)
	{
		if (!this.m_statusEffectController.HasStatusEffect(StatusEffectType.Enemy_Burn_Immunity))
		{
			base.StartEffect(duration, caster);
		}
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x000176AF File Offset: 0x000158AF
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

	// Token: 0x060029BE RID: 10686 RVA: 0x000176BE File Offset: 0x000158BE
	private IEnumerator DealDamageCoroutine()
	{
		int totalBurnCount = (int)((base.Duration + this.m_burnTicRemaining) / 0.5f);
		int totalCritCount = (int)(base.Duration * 0.35f / 0.5f);
		int burnCount = 0;
		base.Duration += 0.05f;
		do
		{
			float delayTime = Time.time + 0.5f - this.m_burnTicRemaining;
			while (Time.time < delayTime)
			{
				this.m_burnTicRemaining = 0.5f - (delayTime - Time.time);
				yield return null;
			}
			int num = burnCount;
			burnCount = num + 1;
			if (totalBurnCount - burnCount < totalCritCount)
			{
				this.m_isCritHit = true;
			}
			else
			{
				this.m_isCritHit = false;
			}
			this.m_burnTicRemaining = 0f;
			this.m_charController.HitboxController.LastCollidedWith = null;
			this.m_charController.CharacterHitResponse.StartHitResponse(base.gameObject, this, -1f, false, true);
		}
		while (burnCount < totalBurnCount);
		yield break;
	}

	// Token: 0x060029BF RID: 10687 RVA: 0x000176CD File Offset: 0x000158CD
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		Projectile_RL projectile_RL = caster as Projectile_RL;
		PlayerController playerController = projectile_RL ? (projectile_RL.OwnerController as PlayerController) : (caster as PlayerController);
		if (playerController)
		{
			this.m_burnMagicDmg = playerController.ActualMagic;
			if (projectile_RL)
			{
				this.m_burnMagicDmg *= 1f + projectile_RL.DamageMod;
			}
			float actualFocus = playerController.ActualFocus;
			float actualMagicCritDamage = playerController.ActualMagicCritDamage;
			float num = 0.55f;
			int num2 = Mathf.RoundToInt(actualFocus * num * actualMagicCritDamage);
			this.ActualCritDamage = (float)num2;
		}
		else if (projectile_RL)
		{
			this.m_burnMagicDmg = projectile_RL.Magic * (1f + projectile_RL.DamageMod);
			this.ActualCritDamage = 0f;
		}
		else
		{
			this.m_burnMagicDmg = 0f;
			this.ActualCritDamage = 0f;
		}
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Burn, base.Duration);
		this.m_pulseOn = false;
		foreach (Renderer renderer in this.m_charController.RendererArray)
		{
			renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._MultiplyColor, this.m_multiplyColor);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_addColorPulseOff);
			renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
		}
		this.m_burnEffect = EffectManager.PlayEffect(this.m_charController.gameObject, this.m_charController.Animator, "EnemyBurnFlames_Effect", Vector3.zero, base.Duration, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.m_burnEffect.transform.SetParent(this.m_charController.transform, false);
		this.m_burnEffect.transform.position = this.m_charController.Midpoint;
		base.StartCoroutine(this.PulseCoroutine());
		base.StartCoroutine(this.DealDamageCoroutine());
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x060029C0 RID: 10688 RVA: 0x000176E3 File Offset: 0x000158E3
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (!interrupted)
		{
			this.m_burnTicRemaining = 0f;
		}
		if (this.m_burnEffect && this.m_burnEffect.isActiveAndEnabled)
		{
			this.m_burnEffect.Stop(EffectStopType.Gracefully);
		}
	}

	// Token: 0x060029C2 RID: 10690 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400240E RID: 9230
	private const string MULTIPLY_COLOR = "#753B3B";

	// Token: 0x0400240F RID: 9231
	private const string PULSE_ON_COLOR = "#451200";

	// Token: 0x04002410 RID: 9232
	private const string PULSE_OFF_COLOR = "#000000";

	// Token: 0x04002411 RID: 9233
	private Color m_multiplyColor;

	// Token: 0x04002412 RID: 9234
	private Color m_addColorPulseOn;

	// Token: 0x04002413 RID: 9235
	private Color m_addColorPulseOff;

	// Token: 0x04002414 RID: 9236
	private bool m_pulseOn;

	// Token: 0x04002415 RID: 9237
	private float m_burnMagicDmg;

	// Token: 0x04002416 RID: 9238
	private BaseEffect m_burnEffect;

	// Token: 0x04002417 RID: 9239
	private float m_burnTicRemaining;

	// Token: 0x04002418 RID: 9240
	private bool m_isCritHit;
}
