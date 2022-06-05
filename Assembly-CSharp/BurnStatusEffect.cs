using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002F5 RID: 757
public class BurnStatusEffect : BaseStatusEffect, IDamageObj
{
	// Token: 0x17000D19 RID: 3353
	// (get) Token: 0x06001E32 RID: 7730 RVA: 0x000627DF File Offset: 0x000609DF
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000D1A RID: 3354
	// (get) Token: 0x06001E33 RID: 7731 RVA: 0x000627E2 File Offset: 0x000609E2
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000D1B RID: 3355
	// (get) Token: 0x06001E34 RID: 7732 RVA: 0x000627E5 File Offset: 0x000609E5
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Burn;
		}
	}

	// Token: 0x17000D1C RID: 3356
	// (get) Token: 0x06001E35 RID: 7733 RVA: 0x000627E9 File Offset: 0x000609E9
	public override float StartingDurationOverride
	{
		get
		{
			return 3.05f;
		}
	}

	// Token: 0x17000D1D RID: 3357
	// (get) Token: 0x06001E36 RID: 7734 RVA: 0x000627F0 File Offset: 0x000609F0
	public bool IsDotDamage
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000D1E RID: 3358
	// (get) Token: 0x06001E37 RID: 7735 RVA: 0x000627F4 File Offset: 0x000609F4
	public float BaseDamage
	{
		get
		{
			float num = 0.55f;
			return this.m_burnMagicDmg * num;
		}
	}

	// Token: 0x17000D1F RID: 3359
	// (get) Token: 0x06001E38 RID: 7736 RVA: 0x0006280F File Offset: 0x00060A0F
	public float ActualDamage
	{
		get
		{
			return this.BaseDamage;
		}
	}

	// Token: 0x17000D20 RID: 3360
	// (get) Token: 0x06001E39 RID: 7737 RVA: 0x00062817 File Offset: 0x00060A17
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

	// Token: 0x17000D21 RID: 3361
	// (get) Token: 0x06001E3A RID: 7738 RVA: 0x0006282C File Offset: 0x00060A2C
	// (set) Token: 0x06001E3B RID: 7739 RVA: 0x00062834 File Offset: 0x00060A34
	public float ActualCritDamage { get; private set; }

	// Token: 0x17000D22 RID: 3362
	// (get) Token: 0x06001E3C RID: 7740 RVA: 0x0006283D File Offset: 0x00060A3D
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x17000D23 RID: 3363
	// (get) Token: 0x06001E3D RID: 7741 RVA: 0x00062844 File Offset: 0x00060A44
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x17000D24 RID: 3364
	// (get) Token: 0x06001E3E RID: 7742 RVA: 0x0006284C File Offset: 0x00060A4C
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x17000D25 RID: 3365
	// (get) Token: 0x06001E3F RID: 7743 RVA: 0x00062854 File Offset: 0x00060A54
	// (set) Token: 0x06001E40 RID: 7744 RVA: 0x0006285B File Offset: 0x00060A5B
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

	// Token: 0x17000D26 RID: 3366
	// (get) Token: 0x06001E41 RID: 7745 RVA: 0x0006285D File Offset: 0x00060A5D
	// (set) Token: 0x06001E42 RID: 7746 RVA: 0x00062864 File Offset: 0x00060A64
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

	// Token: 0x17000D27 RID: 3367
	// (get) Token: 0x06001E43 RID: 7747 RVA: 0x00062866 File Offset: 0x00060A66
	public float KnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000D28 RID: 3368
	// (get) Token: 0x06001E44 RID: 7748 RVA: 0x0006286D File Offset: 0x00060A6D
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x17000D29 RID: 3369
	// (get) Token: 0x06001E45 RID: 7749 RVA: 0x00062871 File Offset: 0x00060A71
	public float StatusEffectDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001E46 RID: 7750 RVA: 0x00062878 File Offset: 0x00060A78
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#753B3B", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#451200", out this.m_addColorPulseOn);
		ColorUtility.TryParseHtmlString("#000000", out this.m_addColorPulseOff);
		base.AppliesTint = true;
	}

	// Token: 0x06001E47 RID: 7751 RVA: 0x000628C7 File Offset: 0x00060AC7
	public override void StartEffect(float duration, IDamageObj caster)
	{
		if (!this.m_statusEffectController.HasStatusEffect(StatusEffectType.Enemy_Burn_Immunity))
		{
			base.StartEffect(duration, caster);
		}
	}

	// Token: 0x06001E48 RID: 7752 RVA: 0x000628E0 File Offset: 0x00060AE0
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

	// Token: 0x06001E49 RID: 7753 RVA: 0x000628EF File Offset: 0x00060AEF
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

	// Token: 0x06001E4A RID: 7754 RVA: 0x000628FE File Offset: 0x00060AFE
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

	// Token: 0x06001E4B RID: 7755 RVA: 0x00062914 File Offset: 0x00060B14
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

	// Token: 0x06001E4D RID: 7757 RVA: 0x00062959 File Offset: 0x00060B59
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001BA5 RID: 7077
	private const string MULTIPLY_COLOR = "#753B3B";

	// Token: 0x04001BA6 RID: 7078
	private const string PULSE_ON_COLOR = "#451200";

	// Token: 0x04001BA7 RID: 7079
	private const string PULSE_OFF_COLOR = "#000000";

	// Token: 0x04001BA8 RID: 7080
	private Color m_multiplyColor;

	// Token: 0x04001BA9 RID: 7081
	private Color m_addColorPulseOn;

	// Token: 0x04001BAA RID: 7082
	private Color m_addColorPulseOff;

	// Token: 0x04001BAB RID: 7083
	private bool m_pulseOn;

	// Token: 0x04001BAC RID: 7084
	private float m_burnMagicDmg;

	// Token: 0x04001BAD RID: 7085
	private BaseEffect m_burnEffect;

	// Token: 0x04001BAE RID: 7086
	private float m_burnTicRemaining;

	// Token: 0x04001BAF RID: 7087
	private bool m_isCritHit;
}
