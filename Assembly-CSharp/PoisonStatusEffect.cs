using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000313 RID: 787
public class PoisonStatusEffect : BaseStatusEffect, IDamageObj
{
	// Token: 0x17000D71 RID: 3441
	// (get) Token: 0x06001F0F RID: 7951 RVA: 0x00063F0D File Offset: 0x0006210D
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Poison;
		}
	}

	// Token: 0x17000D72 RID: 3442
	// (get) Token: 0x06001F10 RID: 7952 RVA: 0x00063F11 File Offset: 0x00062111
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000D73 RID: 3443
	// (get) Token: 0x06001F11 RID: 7953 RVA: 0x00063F14 File Offset: 0x00062114
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000D74 RID: 3444
	// (get) Token: 0x06001F12 RID: 7954 RVA: 0x00063F17 File Offset: 0x00062117
	public override float StartingDurationOverride
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000D75 RID: 3445
	// (get) Token: 0x06001F13 RID: 7955 RVA: 0x00063F1E File Offset: 0x0006211E
	public bool IsDotDamage
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000D76 RID: 3446
	// (get) Token: 0x06001F14 RID: 7956 RVA: 0x00063F24 File Offset: 0x00062124
	public float BaseDamage
	{
		get
		{
			float num = 0.05f;
			return Mathf.Max(1f, this.m_poisonMagicDmg * num);
		}
	}

	// Token: 0x17000D77 RID: 3447
	// (get) Token: 0x06001F15 RID: 7957 RVA: 0x00063F49 File Offset: 0x00062149
	public float ActualDamage
	{
		get
		{
			return this.BaseDamage * (float)base.TimesStacked;
		}
	}

	// Token: 0x17000D78 RID: 3448
	// (get) Token: 0x06001F16 RID: 7958 RVA: 0x00063F59 File Offset: 0x00062159
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

	// Token: 0x17000D79 RID: 3449
	// (get) Token: 0x06001F17 RID: 7959 RVA: 0x00063F73 File Offset: 0x00062173
	// (set) Token: 0x06001F18 RID: 7960 RVA: 0x00063F7B File Offset: 0x0006217B
	public float ActualCritDamage { get; private set; }

	// Token: 0x17000D7A RID: 3450
	// (get) Token: 0x06001F19 RID: 7961 RVA: 0x00063F84 File Offset: 0x00062184
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x17000D7B RID: 3451
	// (get) Token: 0x06001F1A RID: 7962 RVA: 0x00063F8B File Offset: 0x0006218B
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x17000D7C RID: 3452
	// (get) Token: 0x06001F1B RID: 7963 RVA: 0x00063F93 File Offset: 0x00062193
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x17000D7D RID: 3453
	// (get) Token: 0x06001F1C RID: 7964 RVA: 0x00063F9B File Offset: 0x0006219B
	// (set) Token: 0x06001F1D RID: 7965 RVA: 0x00063FA2 File Offset: 0x000621A2
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

	// Token: 0x17000D7E RID: 3454
	// (get) Token: 0x06001F1E RID: 7966 RVA: 0x00063FA4 File Offset: 0x000621A4
	// (set) Token: 0x06001F1F RID: 7967 RVA: 0x00063FAB File Offset: 0x000621AB
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

	// Token: 0x17000D7F RID: 3455
	// (get) Token: 0x06001F20 RID: 7968 RVA: 0x00063FAD File Offset: 0x000621AD
	public float KnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000D80 RID: 3456
	// (get) Token: 0x06001F21 RID: 7969 RVA: 0x00063FB4 File Offset: 0x000621B4
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x17000D81 RID: 3457
	// (get) Token: 0x06001F22 RID: 7970 RVA: 0x00063FB8 File Offset: 0x000621B8
	public float StatusEffectDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000D82 RID: 3458
	// (get) Token: 0x06001F23 RID: 7971 RVA: 0x00063FC0 File Offset: 0x000621C0
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

	// Token: 0x06001F24 RID: 7972 RVA: 0x00063FF0 File Offset: 0x000621F0
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#65A633", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#2B4500", out this.m_addColorPulseOn);
		ColorUtility.TryParseHtmlString("#000000", out this.m_addColorPulseOff);
		base.AppliesTint = true;
	}

	// Token: 0x06001F25 RID: 7973 RVA: 0x0006403F File Offset: 0x0006223F
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

	// Token: 0x06001F26 RID: 7974 RVA: 0x0006404E File Offset: 0x0006224E
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

	// Token: 0x06001F27 RID: 7975 RVA: 0x0006405D File Offset: 0x0006225D
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

	// Token: 0x06001F28 RID: 7976 RVA: 0x0006407A File Offset: 0x0006227A
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

	// Token: 0x06001F2A RID: 7978 RVA: 0x000640CA File Offset: 0x000622CA
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001BEC RID: 7148
	private const string MULTIPLY_COLOR = "#65A633";

	// Token: 0x04001BED RID: 7149
	private const string PULSE_ON_COLOR = "#2B4500";

	// Token: 0x04001BEE RID: 7150
	private const string PULSE_OFF_COLOR = "#000000";

	// Token: 0x04001BEF RID: 7151
	private Color m_multiplyColor;

	// Token: 0x04001BF0 RID: 7152
	private Color m_addColorPulseOn;

	// Token: 0x04001BF1 RID: 7153
	private Color m_addColorPulseOff;

	// Token: 0x04001BF2 RID: 7154
	private bool m_pulseOn;

	// Token: 0x04001BF3 RID: 7155
	private float m_poisonPulseRate = 0.1f;

	// Token: 0x04001BF4 RID: 7156
	private float m_poisonMagicDmg;

	// Token: 0x04001BF5 RID: 7157
	private BaseEffect m_poisonEffect;

	// Token: 0x04001BF6 RID: 7158
	private float m_poisonSkillCritTime;

	// Token: 0x04001BF7 RID: 7159
	private float m_poisonTicRemaining;
}
