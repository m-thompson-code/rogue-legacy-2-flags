using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054C RID: 1356
public class PoisonStatusEffect : BaseStatusEffect, IDamageObj
{
	// Token: 0x1700117E RID: 4478
	// (get) Token: 0x06002B68 RID: 11112 RVA: 0x00006732 File Offset: 0x00004932
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Poison;
		}
	}

	// Token: 0x1700117F RID: 4479
	// (get) Token: 0x06002B69 RID: 11113 RVA: 0x0000F49B File Offset: 0x0000D69B
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17001180 RID: 4480
	// (get) Token: 0x06002B6A RID: 11114 RVA: 0x0000F49B File Offset: 0x0000D69B
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17001181 RID: 4481
	// (get) Token: 0x06002B6B RID: 11115 RVA: 0x0000611B File Offset: 0x0000431B
	public override float StartingDurationOverride
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17001182 RID: 4482
	// (get) Token: 0x06002B6C RID: 11116 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool IsDotDamage
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001183 RID: 4483
	// (get) Token: 0x06002B6D RID: 11117 RVA: 0x000C3A44 File Offset: 0x000C1C44
	public float BaseDamage
	{
		get
		{
			float num = 0.05f;
			return Mathf.Max(1f, this.m_poisonMagicDmg * num);
		}
	}

	// Token: 0x17001184 RID: 4484
	// (get) Token: 0x06002B6E RID: 11118 RVA: 0x00018358 File Offset: 0x00016558
	public float ActualDamage
	{
		get
		{
			return this.BaseDamage * (float)base.TimesStacked;
		}
	}

	// Token: 0x17001185 RID: 4485
	// (get) Token: 0x06002B6F RID: 11119 RVA: 0x00018368 File Offset: 0x00016568
	public float ActualCritChance
	{
		get
		{
			if (Time.time >= this.m_poisonSkillCritTime)
			{
				return 100f;
			}
			return 0f;
		}
	}

	// Token: 0x17001186 RID: 4486
	// (get) Token: 0x06002B70 RID: 11120 RVA: 0x00018382 File Offset: 0x00016582
	// (set) Token: 0x06002B71 RID: 11121 RVA: 0x0001838A File Offset: 0x0001658A
	public float ActualCritDamage { get; private set; }

	// Token: 0x17001187 RID: 4487
	// (get) Token: 0x06002B72 RID: 11122 RVA: 0x00005FA3 File Offset: 0x000041A3
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x17001188 RID: 4488
	// (get) Token: 0x06002B73 RID: 11123 RVA: 0x00018393 File Offset: 0x00016593
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x17001189 RID: 4489
	// (get) Token: 0x06002B74 RID: 11124 RVA: 0x0001839B File Offset: 0x0001659B
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x1700118A RID: 4490
	// (get) Token: 0x06002B75 RID: 11125 RVA: 0x00003CCB File Offset: 0x00001ECB
	// (set) Token: 0x06002B76 RID: 11126 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x1700118B RID: 4491
	// (get) Token: 0x06002B77 RID: 11127 RVA: 0x00003CCB File Offset: 0x00001ECB
	// (set) Token: 0x06002B78 RID: 11128 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x1700118C RID: 4492
	// (get) Token: 0x06002B79 RID: 11129 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float KnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700118D RID: 4493
	// (get) Token: 0x06002B7A RID: 11130 RVA: 0x000046FA File Offset: 0x000028FA
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x1700118E RID: 4494
	// (get) Token: 0x06002B7B RID: 11131 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float StatusEffectDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700118F RID: 4495
	// (get) Token: 0x06002B7C RID: 11132 RVA: 0x000C3A6C File Offset: 0x000C1C6C
	public override int StacksPerHit
	{
		get
		{
			int result = base.StacksPerHit;
			int level = SaveManager.PlayerSaveData.GetRelic(RelicType.WeaponsPoisonAdd).Level;
			if (level > 0)
			{
				result = level;
			}
			return result;
		}
	}

	// Token: 0x06002B7D RID: 11133 RVA: 0x000C3A9C File Offset: 0x000C1C9C
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#65A633", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#2B4500", out this.m_addColorPulseOn);
		ColorUtility.TryParseHtmlString("#000000", out this.m_addColorPulseOff);
		base.AppliesTint = true;
	}

	// Token: 0x06002B7E RID: 11134 RVA: 0x000183A3 File Offset: 0x000165A3
	private IEnumerator PulseCoroutine()
	{
		for (;;)
		{
			this.m_waitYield.CreateNew(this.m_poisonPulseRate, false);
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
					goto IL_10E;
				}
				goto IL_B1;
			}
			goto IL_B1;
			IL_10E:
			this.m_pulseOn = !this.m_pulseOn;
			continue;
			IL_B1:
			foreach (Renderer renderer2 in this.m_charController.RendererArray)
			{
				renderer2.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
				BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_addColorPulseOff);
				renderer2.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
			}
			goto IL_10E;
		}
		yield break;
	}

	// Token: 0x06002B7F RID: 11135 RVA: 0x000183B2 File Offset: 0x000165B2
	private IEnumerator DealDamageCoroutine()
	{
		int totalPoisonCount = (int)((base.Duration + this.m_poisonTicRemaining) / 0.5f);
		int poisonCount = 0;
		base.Duration += 0.15f;
		do
		{
			float delayTime = Time.time + 0.5f - this.m_poisonTicRemaining;
			while (Time.time < delayTime)
			{
				this.m_poisonTicRemaining = 0.5f - (delayTime - Time.time);
				yield return null;
			}
			int num = poisonCount;
			poisonCount = num + 1;
			this.m_poisonTicRemaining = 0f;
			this.m_charController.HitboxController.LastCollidedWith = null;
			this.m_charController.CharacterHitResponse.StartHitResponse(base.gameObject, this, -1f, false, true);
		}
		while (poisonCount < totalPoisonCount);
		yield break;
	}

	// Token: 0x06002B80 RID: 11136 RVA: 0x000183C1 File Offset: 0x000165C1
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		if (justCasted)
		{
			this.m_poisonSkillCritTime = Time.time + 6f;
		}
		base.TimesStacked = Mathf.Clamp(base.TimesStacked, 0, 10);
		Projectile_RL projectile_RL = caster as Projectile_RL;
		PlayerController playerController = projectile_RL ? (projectile_RL.OwnerController as PlayerController) : (caster as PlayerController);
		if (playerController)
		{
			this.m_poisonMagicDmg = playerController.ActualMagic;
			if (projectile_RL)
			{
				this.m_poisonMagicDmg *= 1f + projectile_RL.DamageMod;
			}
			float actualFocus = playerController.ActualFocus;
			float actualMagicCritDamage = playerController.ActualMagicCritDamage;
			float num = 0.55f;
			int num2 = Mathf.RoundToInt(actualFocus * num * actualMagicCritDamage);
			this.ActualCritDamage = (float)num2;
		}
		else if (projectile_RL)
		{
			this.m_poisonMagicDmg = projectile_RL.Magic * (1f + projectile_RL.DamageMod);
			this.ActualCritDamage = 0f;
		}
		else
		{
			this.m_poisonMagicDmg = 0f;
			this.ActualCritDamage = 0f;
		}
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Poison, base.Duration, 10, base.TimesStacked);
		this.m_pulseOn = false;
		foreach (Renderer renderer in this.m_charController.RendererArray)
		{
			renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._MultiplyColor, this.m_multiplyColor);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_addColorPulseOff);
			renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
		}
		this.m_poisonEffect = EffectManager.PlayEffect(this.m_charController.gameObject, this.m_charController.Animator, "EnemyPoisonStatus_Effect", Vector3.zero, base.Duration, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.m_poisonEffect.transform.SetParent(this.m_charController.transform, false);
		this.m_poisonEffect.transform.position = this.m_charController.Midpoint;
		base.StartCoroutine(this.PulseCoroutine());
		base.StartCoroutine(this.DealDamageCoroutine());
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002B81 RID: 11137 RVA: 0x000183DE File Offset: 0x000165DE
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (!interrupted)
		{
			this.m_poisonTicRemaining = 0f;
		}
		if (this.m_poisonEffect && this.m_poisonEffect.isActiveAndEnabled)
		{
			this.m_poisonEffect.Stop(EffectStopType.Gracefully);
		}
	}

	// Token: 0x06002B83 RID: 11139 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040024DD RID: 9437
	private const string MULTIPLY_COLOR = "#65A633";

	// Token: 0x040024DE RID: 9438
	private const string PULSE_ON_COLOR = "#2B4500";

	// Token: 0x040024DF RID: 9439
	private const string PULSE_OFF_COLOR = "#000000";

	// Token: 0x040024E0 RID: 9440
	private Color m_multiplyColor;

	// Token: 0x040024E1 RID: 9441
	private Color m_addColorPulseOn;

	// Token: 0x040024E2 RID: 9442
	private Color m_addColorPulseOff;

	// Token: 0x040024E3 RID: 9443
	private bool m_pulseOn;

	// Token: 0x040024E4 RID: 9444
	private float m_poisonPulseRate = 0.1f;

	// Token: 0x040024E5 RID: 9445
	private float m_poisonMagicDmg;

	// Token: 0x040024E6 RID: 9446
	private BaseEffect m_poisonEffect;

	// Token: 0x040024E7 RID: 9447
	private float m_poisonSkillCritTime;

	// Token: 0x040024E8 RID: 9448
	private float m_poisonTicRemaining;
}
