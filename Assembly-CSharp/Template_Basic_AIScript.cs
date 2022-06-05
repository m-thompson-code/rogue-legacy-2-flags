using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200022D RID: 557
public class Template_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000F76 RID: 3958 RVA: 0x000086FE File Offset: 0x000068FE
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ElementalFireBoltProjectile",
			"ElementalFireBoltLargeProjectile"
		};
	}

	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x06000F77 RID: 3959 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x06000F78 RID: 3960 RVA: 0x00004F67 File Offset: 0x00003167
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x17000731 RID: 1841
	// (get) Token: 0x06000F79 RID: 3961 RVA: 0x00004F67 File Offset: 0x00003167
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x17000732 RID: 1842
	// (get) Token: 0x06000F7A RID: 3962 RVA: 0x00004F78 File Offset: 0x00003178
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(3.5f, 10f);
		}
	}

	// Token: 0x17000733 RID: 1843
	// (get) Token: 0x06000F7B RID: 3963 RVA: 0x00004F89 File Offset: 0x00003189
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(1f, 6f);
		}
	}

	// Token: 0x17000734 RID: 1844
	// (get) Token: 0x06000F7C RID: 3964 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000735 RID: 1845
	// (get) Token: 0x06000F7D RID: 3965 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000736 RID: 1846
	// (get) Token: 0x06000F7E RID: 3966 RVA: 0x00004FFB File Offset: 0x000031FB
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000737 RID: 1847
	// (get) Token: 0x06000F7F RID: 3967 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000738 RID: 1848
	// (get) Token: 0x06000F80 RID: 3968 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000739 RID: 1849
	// (get) Token: 0x06000F81 RID: 3969 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700073A RID: 1850
	// (get) Token: 0x06000F82 RID: 3970 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x06000F83 RID: 3971 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x06000F84 RID: 3972 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700073D RID: 1853
	// (get) Token: 0x06000F85 RID: 3973 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700073E RID: 1854
	// (get) Token: 0x06000F86 RID: 3974 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x06000F87 RID: 3975 RVA: 0x000047A7 File Offset: 0x000029A7
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x06000F88 RID: 3976 RVA: 0x000052A9 File Offset: 0x000034A9
	protected virtual float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x06000F89 RID: 3977 RVA: 0x000052B0 File Offset: 0x000034B0
	protected virtual float m_shoot_RandAngleOffset
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x0000871C File Offset: 0x0000691C
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Move01_RANDOM_SHOOT()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_shoot_TellIntro_AnimSpeed, "Shoot_Tell_Hold", this.m_shoot_TellHold_AnimSpeed, this.m_shoot_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_shoot_AttackIntro_AnimSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_shoot_AttackHold_AnimSpeed, 0f, false);
		float shotInterval = this.m_shoot_TotalShotDuration / (float)this.m_shoot_TotalShots;
		int num3;
		for (int i = 0; i < this.m_shoot_TotalShots; i = num3 + 1)
		{
			float num = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
			float num2 = UnityEngine.Random.Range(-this.m_shoot_RandAngleOffset, this.m_shoot_RandAngleOffset);
			num += num2;
			this.FireProjectile("ElementalFireBoltProjectile", 0, false, num, 1f, true, true, true);
			if (shotInterval > 0f && i < this.m_shoot_TotalShots - 1)
			{
				yield return base.Wait(shotInterval, false);
			}
			num3 = i;
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_shoot_Exit_AnimSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x06000F8B RID: 3979 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x06000F8C RID: 3980 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000744 RID: 1860
	// (get) Token: 0x06000F8D RID: 3981 RVA: 0x00004FFB File Offset: 0x000031FB
	protected virtual float m_spinAttack_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x06000F8E RID: 3982 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x06000F8F RID: 3983 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinAttack_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x06000F90 RID: 3984 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_spinAttack_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x06000F91 RID: 3985 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_spinAttack_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x06000F92 RID: 3986 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x06000F93 RID: 3987 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinAttack_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x06000F94 RID: 3988 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_spinAttack_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x06000F95 RID: 3989 RVA: 0x00003C62 File Offset: 0x00001E62
	protected virtual float m_spinAttack_Exit_AttackCD
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x06000F97 RID: 3991 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x06000F98 RID: 3992 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected virtual int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000750 RID: 1872
	// (get) Token: 0x06000F99 RID: 3993 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x0000872B File Offset: 0x0000692B
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator SpinAttack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		bool alwaysFacing = true;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert || base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			alwaysFacing = false;
		}
		base.EnemyController.AlwaysFacing = alwaysFacing;
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_spinAttack_TellIntro_AnimSpeed, "Shoot_Tell_Hold", this.m_spinAttack_TellHold_AnimSpeed, this.m_spinAttack_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_spinAttack_AttackIntro_AnimSpeed, this.m_spinAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_spinAttack_AttackHold_AnimSpeed, 0f, false);
		float num = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.transform.position);
		int num2;
		for (int i = 0; i < this.m_spinAttack_ShotPatternLoops; i = num2 + 1)
		{
			num = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.transform.position);
			for (int j = 0; j < this.m_spinAttack_TotalShots; j++)
			{
				float angle = num + UnityEngine.Random.Range(-this.m_spinAttack_projectile_RandomSpread, this.m_spinAttack_projectile_RandomSpread);
				this.FireProjectile("ElementalFireBoltLargeProjectile", 0, false, angle, 1f, true, true, true);
			}
			if (i < this.m_spinAttack_ShotPatternLoops - 1 && this.m_spinAttack_TimesShotDelay > 0f)
			{
				yield return base.Wait(this.m_spinAttack_TimesShotDelay, false);
			}
			num2 = i;
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_spinAttack_Exit_AnimSpeed, this.m_spinAttack_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_spinAttack_Exit_ForceIdle, this.m_spinAttack_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x0400129E RID: 4766
	protected const string MOVE01_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x0400129F RID: 4767
	protected const string MOVE01_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x040012A0 RID: 4768
	protected const string MOVE01_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x040012A1 RID: 4769
	protected const string MOVE01_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x040012A2 RID: 4770
	protected const string MOVE01_EXIT = "Shoot_Exit";

	// Token: 0x040012A3 RID: 4771
	protected const string MOVE01_PROJECTILE = "ElementalFireBoltProjectile";

	// Token: 0x040012A4 RID: 4772
	protected const string SPINATTACK_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x040012A5 RID: 4773
	protected const string SPINATTACK_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x040012A6 RID: 4774
	protected const string SPINATTACK_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x040012A7 RID: 4775
	protected const string SPINATTACK_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x040012A8 RID: 4776
	protected const string SPINATTACK_EXIT = "Shoot_Exit";

	// Token: 0x040012A9 RID: 4777
	protected const string SPINATTACK_PROJECTILE = "ElementalFireBoltLargeProjectile";
}
