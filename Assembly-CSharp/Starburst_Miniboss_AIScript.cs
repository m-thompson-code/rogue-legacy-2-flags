using System;
using System.Collections;

// Token: 0x0200020E RID: 526
public class Starburst_Miniboss_AIScript : Starburst_Basic_AIScript
{
	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00004A07 File Offset: 0x00002C07
	protected override int NumberOfShots
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x06000E78 RID: 3704 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool AlternateShots
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x06000E79 RID: 3705 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected virtual int NumberOfCurse
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0000808F File Offset: 0x0000628F
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public IEnumerator ShootBoss()
	{
		yield return this.Default_TellIntroAndLoop("Spin_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "Spin_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_TellHold_Delay);
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, this.m_shoot_AttackHold_Delay, false);
		float num = 360f / (float)this.NumberOfCurse;
		float num2 = 0f;
		if (this.AlternateShots && this.AlternateCount == 1)
		{
			num2 = num + num / 2f;
			this.AlternateCount = 0;
		}
		else if (this.AlternateShots && this.AlternateCount == 0)
		{
			this.AlternateCount = 1;
		}
		for (int i = 0; i < this.NumberOfCurse; i++)
		{
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
			{
				this.FireProjectile("StarburstSlashBoltMinibossProjectile", 0, false, num * (float)i + num2, 1f, true, true, true);
			}
			else if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
			{
				this.FireProjectile("StarburstVoidProjectile", 0, false, num * (float)i + num2, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile("StarburstBoltProjectile", 0, false, num * (float)i + num2, 1f, true, true, true);
			}
		}
		if (this.m_shoot_AttackHold_Delay > 0f)
		{
			base.Wait(this.m_shoot_AttackHold_Delay, false);
		}
		yield return this.Default_Animation("Spin_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x06000E7B RID: 3707 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x06000E7C RID: 3708 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006C1 RID: 1729
	// (get) Token: 0x06000E7D RID: 3709 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x06000E7E RID: 3710 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_dash_TellHold_Delay
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x06000E7F RID: 3711 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06000E80 RID: 3712 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06000E81 RID: 3713 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06000E82 RID: 3714 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x06000E83 RID: 3715 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006C8 RID: 1736
	// (get) Token: 0x06000E84 RID: 3716 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06000E85 RID: 3717 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00005FB1 File Offset: 0x000041B1
	protected virtual float m_dash_Exit_AttackCD
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0000809E File Offset: 0x0000629E
	protected virtual float m_dash_Attack_ForwardSpeedMod
	{
		get
		{
			return 4.5f;
		}
	}

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06000E88 RID: 3720 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x06000E89 RID: 3721 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170006CE RID: 1742
	// (get) Token: 0x06000E8A RID: 3722 RVA: 0x00006772 File Offset: 0x00004972
	protected virtual float m_fireballDropDuringDashInterval
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x000080A5 File Offset: 0x000062A5
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		this.StopAndFaceTarget();
		yield return this.Default_Animation("Spin_Tell_Intro", this.m_dash_TellIntro_AnimSpeed, this.m_dash_TellIntro_Delay, false);
		yield return this.Default_Animation("Spin_Tell_Hold", this.m_dash_TellHold_AnimSpeed, this.m_dash_TellHold_Delay, false);
		base.EnemyController.Heading = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		base.EnemyController.LockFlip = true;
		base.EnemyController.FollowTarget = false;
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_dash_AttackIntro_AnimSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_dash_AttackHold_AnimSpeed, this.m_dash_AttackHold_Delay, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = base.EnemyController.BaseSpeed * this.m_dash_Attack_ForwardSpeedMod;
		if (this.m_dash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_Duration, false);
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		this.FireProjectile("StarburstSlashBoltMinibossProjectile", 0, false, 0f, 1f, true, true, true);
		this.FireProjectile("StarburstSlashBoltMinibossProjectile", 0, false, 180f, 1f, true, true, true);
		yield return this.Default_Animation("Spin_Exit", this.m_dash_Exit_AnimSpeed, this.m_dash_Exit_Delay, true);
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_Exit_AttackCD);
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		base.EnemyController.LockFlip = false;
		yield break;
	}

	// Token: 0x0400116E RID: 4462
	protected const string DASH_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x0400116F RID: 4463
	protected const string DASH_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x04001170 RID: 4464
	protected const string DASH_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x04001171 RID: 4465
	protected const string DASH_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x04001172 RID: 4466
	protected const string DASH_EXIT = "Spin_Exit";
}
