using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class DualBlades_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x06000F4C RID: 3916 RVA: 0x0002D67D File Offset: 0x0002B87D
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_projectileNameRight,
			this.m_secondProjectileName,
			this.m_thirdProjectileName
		};
	}

	// Token: 0x17000862 RID: 2146
	// (get) Token: 0x06000F4D RID: 3917 RVA: 0x0002D6AF File Offset: 0x0002B8AF
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.45f;
		}
	}

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x06000F4E RID: 3918 RVA: 0x0002D6B6 File Offset: 0x0002B8B6
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000864 RID: 2148
	// (get) Token: 0x06000F4F RID: 3919 RVA: 0x0002D6BD File Offset: 0x0002B8BD
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000865 RID: 2149
	// (get) Token: 0x06000F50 RID: 3920 RVA: 0x0002D6C4 File Offset: 0x0002B8C4
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000866 RID: 2150
	// (get) Token: 0x06000F51 RID: 3921 RVA: 0x0002D6CB File Offset: 0x0002B8CB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000867 RID: 2151
	// (get) Token: 0x06000F52 RID: 3922 RVA: 0x0002D6D2 File Offset: 0x0002B8D2
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000868 RID: 2152
	// (get) Token: 0x06000F53 RID: 3923 RVA: 0x0002D6D9 File Offset: 0x0002B8D9
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000869 RID: 2153
	// (get) Token: 0x06000F54 RID: 3924 RVA: 0x0002D6E0 File Offset: 0x0002B8E0
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x1700086A RID: 2154
	// (get) Token: 0x06000F55 RID: 3925 RVA: 0x0002D6E7 File Offset: 0x0002B8E7
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700086B RID: 2155
	// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0002D6EE File Offset: 0x0002B8EE
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700086C RID: 2156
	// (get) Token: 0x06000F57 RID: 3927 RVA: 0x0002D6F8 File Offset: 0x0002B8F8
	public override string ProjectileName
	{
		get
		{
			switch (this.m_attackCount)
			{
			default:
				return base.ProjectileName;
			case 1:
				return this.m_secondProjectileName;
			case 2:
				return this.m_thirdProjectileName;
			}
		}
	}

	// Token: 0x1700086D RID: 2157
	// (get) Token: 0x06000F58 RID: 3928 RVA: 0x0002D734 File Offset: 0x0002B934
	public override Vector2 ProjectileOffset
	{
		get
		{
			switch (this.m_attackCount)
			{
			default:
				return this.m_projectileOffset;
			case 1:
				return this.m_secondProjectileOffset;
			case 2:
				return this.m_thirdProjectileOffset;
			}
		}
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x0002D76E File Offset: 0x0002B96E
	protected override void Awake()
	{
		base.Awake();
		if (this.m_abilityTellIntroName != null)
		{
			this.m_abilityTellIntroRight = this.m_abilityTellIntroName.Replace("Left", "Right");
		}
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x0002D799 File Offset: 0x0002B999
	public override void PreCastAbility()
	{
		this.m_attackCount = 0;
		base.PreCastAbility();
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x0002D7A8 File Offset: 0x0002B9A8
	protected override IEnumerator ChangeAnim(float duration)
	{
		while (duration > 0f)
		{
			duration -= Time.deltaTime;
			yield return null;
		}
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack)
		{
			if (this.m_attackCount < 3)
			{
				this.m_animator.SetTrigger("ContinueCombo");
			}
			else
			{
				this.m_animator.SetTrigger("Change_Ability_Anim");
			}
		}
		else
		{
			this.m_animator.SetTrigger("Change_Ability_Anim");
		}
		base.PerformTurnAnimCheck();
		yield break;
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x0002D7C0 File Offset: 0x0002B9C0
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_attackCount++;
		if (this.m_attackCount >= 3 && this.m_firedProjectile.ActualCritChance < 100f)
		{
			this.m_firedProjectile.ActualCritChance += 100f;
		}
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x0002D813 File Offset: 0x0002BA13
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.gameObject.SetActive(false);
			this.m_firedProjectile = null;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x04001169 RID: 4457
	[SerializeField]
	private string m_projectileNameRight;

	// Token: 0x0400116A RID: 4458
	[Space(10f)]
	[SerializeField]
	private string m_secondProjectileName;

	// Token: 0x0400116B RID: 4459
	[SerializeField]
	private Vector2 m_secondProjectileOffset;

	// Token: 0x0400116C RID: 4460
	[SerializeField]
	private string m_thirdProjectileName;

	// Token: 0x0400116D RID: 4461
	[SerializeField]
	private Vector2 m_thirdProjectileOffset;

	// Token: 0x0400116E RID: 4462
	private const int NUM_ATTACKS = 3;

	// Token: 0x0400116F RID: 4463
	private string m_abilityTellIntroRight;

	// Token: 0x04001170 RID: 4464
	private float m_canAttackAgainCounter;

	// Token: 0x04001171 RID: 4465
	private int m_attackCount;
}
