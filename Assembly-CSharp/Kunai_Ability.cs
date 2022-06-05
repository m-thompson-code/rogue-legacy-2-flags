using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F7 RID: 759
public class Kunai_Ability : Bow_Ability
{
	// Token: 0x17000B54 RID: 2900
	// (get) Token: 0x06001797 RID: 6039 RVA: 0x0000611B File Offset: 0x0000431B
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000B55 RID: 2901
	// (get) Token: 0x06001798 RID: 6040 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B56 RID: 2902
	// (get) Token: 0x06001799 RID: 6041 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000B57 RID: 2903
	// (get) Token: 0x0600179A RID: 6042 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B58 RID: 2904
	// (get) Token: 0x0600179B RID: 6043 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000B59 RID: 2905
	// (get) Token: 0x0600179C RID: 6044 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B5A RID: 2906
	// (get) Token: 0x0600179D RID: 6045 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float AttackAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000B5B RID: 2907
	// (get) Token: 0x0600179E RID: 6046 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B5C RID: 2908
	// (get) Token: 0x0600179F RID: 6047 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float ExitAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000B5D RID: 2909
	// (get) Token: 0x060017A0 RID: 6048 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B5E RID: 2910
	// (get) Token: 0x060017A1 RID: 6049 RVA: 0x0000BF1A File Offset: 0x0000A11A
	protected override Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(0f, 3f);
		}
	}

	// Token: 0x17000B5F RID: 2911
	// (get) Token: 0x060017A2 RID: 6050 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float GravityReduction
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B60 RID: 2912
	// (get) Token: 0x060017A3 RID: 6051 RVA: 0x00004762 File Offset: 0x00002962
	protected int NumKunais
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x17000B61 RID: 2913
	// (get) Token: 0x060017A4 RID: 6052 RVA: 0x0000452F File Offset: 0x0000272F
	protected float KunaiFireDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x0000BF2B File Offset: 0x0000A12B
	protected override void Awake()
	{
		base.Awake();
		this.m_fireDelayWaitYield = new WaitRL_Yield(this.KunaiFireDelay, false);
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x0000BF45 File Offset: 0x0000A145
	public override void PreCastAbility()
	{
		this.m_numKunaisShot = 0;
		base.PreCastAbility();
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x0000BF54 File Offset: 0x0000A154
	protected override void FireProjectile()
	{
		if (this.m_fireProjectileCoroutine != null)
		{
			base.StopCoroutine(this.m_fireProjectileCoroutine);
		}
		this.m_fireProjectileCoroutine = base.StartCoroutine(this.FireProjectileCoroutine());
	}

	// Token: 0x060017A8 RID: 6056 RVA: 0x0000BF7C File Offset: 0x0000A17C
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

	// Token: 0x060017A9 RID: 6057 RVA: 0x0000BF8B File Offset: 0x0000A18B
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

	// Token: 0x060017AA RID: 6058 RVA: 0x0000BFA1 File Offset: 0x0000A1A1
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_fireProjectileCoroutine != null)
		{
			base.StopCoroutine(this.m_fireProjectileCoroutine);
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x0400173D RID: 5949
	private int m_numKunaisShot;

	// Token: 0x0400173E RID: 5950
	private Coroutine m_fireProjectileCoroutine;

	// Token: 0x0400173F RID: 5951
	private WaitRL_Yield m_fireDelayWaitYield;
}
