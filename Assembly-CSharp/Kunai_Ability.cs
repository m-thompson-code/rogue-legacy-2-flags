using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200019D RID: 413
public class Kunai_Ability : Bow_Ability
{
	// Token: 0x170008B6 RID: 2230
	// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x0002DEE8 File Offset: 0x0002C0E8
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x170008B7 RID: 2231
	// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x0002DEEF File Offset: 0x0002C0EF
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008B8 RID: 2232
	// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x0002DEF6 File Offset: 0x0002C0F6
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x0002DEFD File Offset: 0x0002C0FD
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0002DF04 File Offset: 0x0002C104
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170008BB RID: 2235
	// (get) Token: 0x06000FBB RID: 4027 RVA: 0x0002DF0B File Offset: 0x0002C10B
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008BC RID: 2236
	// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0002DF12 File Offset: 0x0002C112
	protected override float AttackAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170008BD RID: 2237
	// (get) Token: 0x06000FBD RID: 4029 RVA: 0x0002DF19 File Offset: 0x0002C119
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008BE RID: 2238
	// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0002DF20 File Offset: 0x0002C120
	protected override float ExitAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170008BF RID: 2239
	// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0002DF27 File Offset: 0x0002C127
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008C0 RID: 2240
	// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0002DF2E File Offset: 0x0002C12E
	protected override Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(0f, 3f);
		}
	}

	// Token: 0x170008C1 RID: 2241
	// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0002DF3F File Offset: 0x0002C13F
	protected override float GravityReduction
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008C2 RID: 2242
	// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0002DF46 File Offset: 0x0002C146
	protected int NumKunais
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x170008C3 RID: 2243
	// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0002DF49 File Offset: 0x0002C149
	protected float KunaiFireDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x06000FC4 RID: 4036 RVA: 0x0002DF50 File Offset: 0x0002C150
	protected override void Awake()
	{
		base.Awake();
		this.m_fireDelayWaitYield = new WaitRL_Yield(this.KunaiFireDelay, false);
	}

	// Token: 0x06000FC5 RID: 4037 RVA: 0x0002DF6A File Offset: 0x0002C16A
	public override void PreCastAbility()
	{
		this.m_numKunaisShot = 0;
		base.PreCastAbility();
	}

	// Token: 0x06000FC6 RID: 4038 RVA: 0x0002DF79 File Offset: 0x0002C179
	protected override void FireProjectile()
	{
		if (this.m_fireProjectileCoroutine != null)
		{
			base.StopCoroutine(this.m_fireProjectileCoroutine);
		}
		this.m_fireProjectileCoroutine = base.StartCoroutine(this.FireProjectileCoroutine());
	}

	// Token: 0x06000FC7 RID: 4039 RVA: 0x0002DFA1 File Offset: 0x0002C1A1
	private IEnumerator FireProjectileCoroutine()
	{
		if (this.ProjectileName != null)
		{
			this.ApplyAbilityCosts();
			int num;
			for (int i = 0; i < this.NumKunais; i = num + 1)
			{
				Projectile_RL projectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, this.m_aimAngle, 1f, false, true, true, true);
				this.m_abilityController.InitializeProjectile(projectile);
				this.m_numKunaisShot++;
				if (this.m_numKunaisShot < this.NumKunais)
				{
					yield return this.m_fireDelayWaitYield;
				}
				num = i;
			}
		}
		yield break;
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x0002DFB0 File Offset: 0x0002C1B0
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack)
		{
			while (this.m_numKunaisShot < this.NumKunais)
			{
				yield return null;
			}
			yield return base.ChangeAnim(0f);
		}
		else
		{
			yield return base.ChangeAnim(duration);
		}
		yield break;
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x0002DFC6 File Offset: 0x0002C1C6
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_fireProjectileCoroutine != null)
		{
			base.StopCoroutine(this.m_fireProjectileCoroutine);
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x04001189 RID: 4489
	private int m_numKunaisShot;

	// Token: 0x0400118A RID: 4490
	private Coroutine m_fireProjectileCoroutine;

	// Token: 0x0400118B RID: 4491
	private WaitRL_Yield m_fireDelayWaitYield;
}
