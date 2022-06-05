using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public class FlyingShield_Basic_AIScript : BaseAIScript
{
	// Token: 0x060006AA RID: 1706 RVA: 0x00019B2E File Offset: 0x00017D2E
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingShieldWaterProjectile",
			"FlyingShieldWaterExpertProjectile",
			"FlyingShieldWaterMinibossProjectile"
		};
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x060006AB RID: 1707 RVA: 0x00019B54 File Offset: 0x00017D54
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.7f, 1f);
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x060006AC RID: 1708 RVA: 0x00019B65 File Offset: 0x00017D65
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x060006AD RID: 1709 RVA: 0x00019B76 File Offset: 0x00017D76
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x060006AE RID: 1710 RVA: 0x00019B87 File Offset: 0x00017D87
	protected virtual float m_spinMove_TellIntro_AnimationSpeed
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x060006AF RID: 1711 RVA: 0x00019B8E File Offset: 0x00017D8E
	protected virtual float m_spinMove_TellIntro_EndDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00019B95 File Offset: 0x00017D95
	protected virtual float m_spinMove_TellHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00019B9C File Offset: 0x00017D9C
	protected virtual float m_spinMove_TellHold_Duration
	{
		get
		{
			return 0.425f;
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00019BA3 File Offset: 0x00017DA3
	protected virtual float m_spinMove_AttackIntro_AnimationSpeed
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00019BAA File Offset: 0x00017DAA
	protected virtual float m_spinMove_AttackIntro_EndDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x060006B4 RID: 1716 RVA: 0x00019BB1 File Offset: 0x00017DB1
	protected virtual float m_spinMove_Attack_FlameDrops
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x060006B5 RID: 1717 RVA: 0x00019BB8 File Offset: 0x00017DB8
	protected virtual float m_spinMove_Attack_Duration
	{
		get
		{
			return 3.25f;
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00019BBF File Offset: 0x00017DBF
	protected virtual float m_spinMove_Attack_MoveSpeed
	{
		get
		{
			return 22f;
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00019BC6 File Offset: 0x00017DC6
	protected virtual float m_spinMove_ExitIntro_AnimationSpeed
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00019BCD File Offset: 0x00017DCD
	protected virtual float m_spinMove_ExitIntro_EndDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00019BD4 File Offset: 0x00017DD4
	protected virtual bool m_hasTailSpin
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x060006BA RID: 1722 RVA: 0x00019BD7 File Offset: 0x00017DD7
	protected virtual bool m_hasTailRam
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x060006BB RID: 1723 RVA: 0x00019BDA File Offset: 0x00017DDA
	protected virtual float m_fireball_TailSpawnDelay
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x060006BC RID: 1724 RVA: 0x00019BE1 File Offset: 0x00017DE1
	protected virtual float m_fireball_TailRamSpawnDelay
	{
		get
		{
			return 0.015f;
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x060006BD RID: 1725 RVA: 0x00019BE8 File Offset: 0x00017DE8
	protected virtual float m_spinMove_Exit_ForceIdle
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x060006BE RID: 1726 RVA: 0x00019BEF File Offset: 0x00017DEF
	protected virtual float m_spinMove_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x060006BF RID: 1727 RVA: 0x00019BF6 File Offset: 0x00017DF6
	protected virtual bool m_raiseKnockbackDefenseWhileAttacking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x060006C0 RID: 1728 RVA: 0x00019BF9 File Offset: 0x00017DF9
	protected virtual int m_knockbackDefenseBoostOverride
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00019BFC File Offset: 0x00017DFC
	protected virtual float m_ramMove_ChaseDuration
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00019C03 File Offset: 0x00017E03
	protected virtual float m_ramMove_TellIntro_AnimationSpeed
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00019C0A File Offset: 0x00017E0A
	protected virtual float m_ramMove_TellHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00019C11 File Offset: 0x00017E11
	protected virtual float m_ramMove_TellIntroAndHold_Delay
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00019C18 File Offset: 0x00017E18
	protected virtual float m_ramMove_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00019C1F File Offset: 0x00017E1F
	protected virtual float m_ramMove_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00019C26 File Offset: 0x00017E26
	protected virtual float m_ramMove_AttackHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x060006C8 RID: 1736 RVA: 0x00019C2D File Offset: 0x00017E2D
	protected virtual float m_ramMove_AttackHold_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00019C34 File Offset: 0x00017E34
	protected virtual float m_ramMove_Attack_Duration
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x060006CA RID: 1738 RVA: 0x00019C3B File Offset: 0x00017E3B
	protected virtual float m_ramMove_Attack_Speed
	{
		get
		{
			return 41f;
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x060006CB RID: 1739 RVA: 0x00019C42 File Offset: 0x00017E42
	protected virtual float m_ramMove_ExitIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x060006CC RID: 1740 RVA: 0x00019C49 File Offset: 0x00017E49
	protected virtual float m_ramMove_ExitIntro_EndDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x060006CD RID: 1741 RVA: 0x00019C50 File Offset: 0x00017E50
	protected virtual float m_ramMove_ExitIntro_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x060006CE RID: 1742 RVA: 0x00019C57 File Offset: 0x00017E57
	protected virtual float m_ramMove_Attack_CD
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00019C5E File Offset: 0x00017E5E
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.DisableFriction = true;
		this.m_fireball_TailCounter = this.m_fireball_TailSpawnDelay;
		this.m_bounceLogic = base.EnemyController.GetComponent<BounceCollision>();
		this.m_bounceLogic.enabled = false;
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x00019C9C File Offset: 0x00017E9C
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Ram_Attack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.GenerateRandomFollowOffset(new Vector2(1f, 1f), Vector2.zero);
		if (this.m_ramMove_ChaseDuration > 0f)
		{
			yield return base.Wait(this.m_ramMove_ChaseDuration, false);
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_TellIntroAndLoop("Ram_Tell_Intro", this.m_ramMove_TellIntro_AnimationSpeed, "Ram_Tell_Hold", this.m_ramMove_TellHold_AnimationSpeed, this.m_ramMove_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_Animation("Ram_Attack_Intro", this.m_ramMove_AttackIntro_AnimationSpeed, this.m_ramMove_AttackIntro_Delay, true);
		yield return this.Default_Animation("Ram_Attack_Hold", this.m_ramMove_AttackHold_AnimationSpeed, this.m_ramMove_AttackHold_Duration, false);
		this.m_enableTail = this.m_hasTailRam;
		this.m_isRamAttack = true;
		if (base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(this.m_ramMove_Attack_Speed, false);
		}
		else
		{
			base.SetVelocityX(-this.m_ramMove_Attack_Speed, false);
		}
		if (this.m_ramMove_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_ramMove_Attack_Duration, false);
		}
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		this.m_enableTail = false;
		this.m_isRamAttack = false;
		yield return this.Default_Animation("Ram_Exit", this.m_ramMove_ExitIntro_AnimationSpeed, this.m_ramMove_ExitIntro_EndDelay, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_ramMove_ExitIntro_ForceIdle, this.m_ramMove_Attack_CD);
		yield break;
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x00019CAB File Offset: 0x00017EAB
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Spin_Attack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowTarget = true;
		base.EnemyController.FollowOffset = Vector3.zero;
		base.EnemyController.ForceFaceTarget();
		base.EnemyController.BaseTurnSpeed = this.m_spinAttackTurnSpeedOverride;
		this.m_aim = true;
		yield return this.Default_Animation("Spin_Tell_Intro", this.m_spinMove_TellIntro_AnimationSpeed, this.m_spinMove_TellIntro_EndDelay, true);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Spin_Tell_Hold", this.m_spinMove_TellIntro_AnimationSpeed, 0f, true);
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_spinMove_TellIntro_AnimationSpeed, 0f, true);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_spinMove_TellHold_AnimationSpeed, this.m_spinMove_TellHold_Duration, true);
		base.EnemyController.FollowTarget = false;
		if (this.m_raiseKnockbackDefenseWhileAttacking)
		{
			base.EnemyController.BaseKnockbackDefense = (float)this.m_knockbackDefenseBoostOverride;
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
		base.SetVelocity(base.EnemyController.HeadingX * this.m_spinMove_Attack_MoveSpeed, base.EnemyController.HeadingY * this.m_spinMove_Attack_MoveSpeed, false);
		this.m_bounceLogic.enabled = true;
		this.m_aim = false;
		base.EnemyController.HitboxController.CollisionType = CollisionType.EnemyProjectile;
		this.m_enableTail = this.m_hasTailSpin;
		if (this.m_spinMove_Attack_FlameDrops > 0f)
		{
			int i = 0;
			while ((float)i < this.m_spinMove_Attack_FlameDrops)
			{
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.FireProjectile("FlyingShieldWaterMinibossProjectile", 0, false, 0f, 0f, true, true, true);
				}
				else
				{
					this.FireProjectile("FlyingShieldWaterProjectile", 0, false, 0f, 0f, true, true, true);
				}
				if (this.m_spinMove_Attack_Duration / this.m_spinMove_Attack_FlameDrops > 0f)
				{
					yield return base.Wait(this.m_spinMove_Attack_Duration / this.m_spinMove_Attack_FlameDrops, false);
				}
				int num = i;
				i = num + 1;
			}
		}
		else if (this.m_spinMove_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_spinMove_Attack_Duration, false);
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		this.m_bounceLogic.enabled = false;
		base.EnemyController.HitboxController.CollisionType = CollisionType.Enemy;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.ChangeAnimationState("Spin_Exit");
		yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		base.Animator.ResetTrigger("Turn");
		base.EnemyController.ForceFaceTarget();
		base.EnemyController.FollowTarget = true;
		this.m_enableTail = false;
		base.EnemyController.ResetBaseValues();
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		this.TriggerAttackCooldown(this.m_spinMove_Exit_AttackCD, false);
		this.TriggerRestState(false);
		yield break;
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x00019CBA File Offset: 0x00017EBA
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		this.m_aim = false;
		this.m_aimSprite.SetActive(false);
		this.m_enableTail = false;
		base.EnemyController.HitboxController.CollisionType = CollisionType.Enemy;
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x00019CED File Offset: 0x00017EED
	public override void ResetScript()
	{
		base.ResetScript();
		this.m_bounceLogic.enabled = false;
		this.m_enableTail = false;
		this.m_fireball_TailCounter = this.m_fireball_TailSpawnDelay;
		this.m_aim = false;
		this.m_aimSprite.SetActive(false);
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x00019D28 File Offset: 0x00017F28
	protected void Update()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (this.m_enableTail && this.m_fireball_TailCounter > 0f)
		{
			this.m_fireball_TailCounter -= Time.deltaTime;
			if (this.m_fireball_TailCounter <= 0f)
			{
				if (this.m_isRamAttack)
				{
					this.m_fireball_TailCounter = this.m_fireball_TailRamSpawnDelay;
				}
				else
				{
					this.m_fireball_TailCounter = this.m_fireball_TailSpawnDelay;
				}
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Advanced)
				{
					this.FireProjectile("FlyingShieldWaterProjectile", 0, false, 0f, 0f, true, true, true);
				}
				else if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
				{
					this.FireProjectile("FlyingShieldWaterExpertProjectile", 0, false, 0f, 0f, true, true, true);
				}
				else
				{
					this.FireProjectile("FlyingShieldWaterMinibossProjectile", 0, false, 0f, 0f, true, true, true);
				}
			}
		}
		if (this.m_aim)
		{
			if (!this.m_aimSprite.activeSelf)
			{
				this.m_aimSprite.SetActive(true);
			}
			this.m_aimSprite.transform.SetLocalEulerZ(CDGHelper.VectorToAngle(base.EnemyController.Heading));
			return;
		}
		if (this.m_aimSprite.activeSelf)
		{
			this.m_aimSprite.SetActive(false);
		}
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x00019E69 File Offset: 0x00018069
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_aimSprite.activeSelf)
		{
			this.m_aimSprite.SetActive(false);
		}
	}

	// Token: 0x04000AD5 RID: 2773
	[SerializeField]
	private GameObject m_aimSprite;

	// Token: 0x04000AD6 RID: 2774
	private float m_fireball_TailCounter;

	// Token: 0x04000AD7 RID: 2775
	private bool m_enableTail;

	// Token: 0x04000AD8 RID: 2776
	private bool m_isRamAttack;

	// Token: 0x04000AD9 RID: 2777
	private bool m_aim;

	// Token: 0x04000ADA RID: 2778
	private BounceCollision m_bounceLogic;

	// Token: 0x04000ADB RID: 2779
	protected Vector3 m_ramMove_SetRamPosition = new Vector3(-1f, 0f, 0f);

	// Token: 0x04000ADC RID: 2780
	private float m_spinAttackTurnSpeedOverride = 175f;
}
