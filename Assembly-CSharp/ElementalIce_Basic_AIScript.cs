using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B8 RID: 184
public class ElementalIce_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600046A RID: 1130 RVA: 0x00015B8A File Offset: 0x00013D8A
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ElementalIceBoltProjectile",
			"ElementalIceBoltExplosionProjectile"
		};
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x0600046B RID: 1131 RVA: 0x00015BA8 File Offset: 0x00013DA8
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x0600046C RID: 1132 RVA: 0x00015BB9 File Offset: 0x00013DB9
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x0600046D RID: 1133 RVA: 0x00015BCA File Offset: 0x00013DCA
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x0600046E RID: 1134 RVA: 0x00015BDB File Offset: 0x00013DDB
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-5f, 5f);
		}
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x0600046F RID: 1135 RVA: 0x00015BEC File Offset: 0x00013DEC
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 5f);
		}
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x00015BFD File Offset: 0x00013DFD
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		enemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00015C1C File Offset: 0x00013E1C
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		}
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000472 RID: 1138 RVA: 0x00015C41 File Offset: 0x00013E41
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000473 RID: 1139 RVA: 0x00015C48 File Offset: 0x00013E48
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000474 RID: 1140 RVA: 0x00015C4F File Offset: 0x00013E4F
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000475 RID: 1141 RVA: 0x00015C56 File Offset: 0x00013E56
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06000476 RID: 1142 RVA: 0x00015C5D File Offset: 0x00013E5D
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06000477 RID: 1143 RVA: 0x00015C64 File Offset: 0x00013E64
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06000478 RID: 1144 RVA: 0x00015C6B File Offset: 0x00013E6B
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06000479 RID: 1145 RVA: 0x00015C72 File Offset: 0x00013E72
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x0600047A RID: 1146 RVA: 0x00015C79 File Offset: 0x00013E79
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x0600047B RID: 1147 RVA: 0x00015C80 File Offset: 0x00013E80
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x0600047C RID: 1148 RVA: 0x00015C87 File Offset: 0x00013E87
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x0600047D RID: 1149 RVA: 0x00015C8E File Offset: 0x00013E8E
	protected virtual bool m_shoot_CentreShot
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x0600047E RID: 1150 RVA: 0x00015C91 File Offset: 0x00013E91
	protected virtual float m_shoot_TimesShotDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x0600047F RID: 1151 RVA: 0x00015C98 File Offset: 0x00013E98
	protected virtual int m_shoot_TotalSideShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06000480 RID: 1152 RVA: 0x00015C9B File Offset: 0x00013E9B
	protected virtual int m_shoot_ShotPatternLoops
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x06000481 RID: 1153 RVA: 0x00015C9E File Offset: 0x00013E9E
	protected virtual float m_shoot_Projectile_Spread
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00015CA5 File Offset: 0x00013EA5
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootIceball()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		bool alwaysFacing = true;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert || base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			alwaysFacing = false;
		}
		base.EnemyController.AlwaysFacing = alwaysFacing;
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_shoot_TellIntro_AnimSpeed, "Shoot_Tell_Hold", this.m_shoot_TellHold_AnimSpeed, this.m_shoot_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_shoot_AttackIntro_AnimSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_shoot_AttackHold_AnimSpeed, 0f, false);
		Vector2 pt = base.EnemyController.TargetController.Midpoint - base.EnemyController.transform.position;
		float initialFireAngle = CDGHelper.VectorToAngle(pt);
		float angleDelta = 0f;
		angleDelta = this.m_shoot_Projectile_Spread;
		if (this.m_shoot_CentreShot)
		{
			this.FireProjectile("ElementalIceBoltProjectile", 0, false, initialFireAngle, 1f, true, true, true);
		}
		int num2;
		for (int i = 0; i < this.m_shoot_ShotPatternLoops; i = num2 + 1)
		{
			for (int j = 1; j < this.m_shoot_TotalSideShots + 1; j++)
			{
				float num = initialFireAngle;
				num += angleDelta * (float)j;
				this.FireProjectile("ElementalIceBoltProjectile", 0, false, num, 1f, true, true, true);
				num = initialFireAngle;
				num -= angleDelta * (float)j;
				this.FireProjectile("ElementalIceBoltProjectile", 0, false, num, 1f, true, true, true);
			}
			if (i < this.m_shoot_ShotPatternLoops - 1 && this.m_shoot_TimesShotDelay > 0f)
			{
				yield return base.Wait(this.m_shoot_TimesShotDelay, false);
			}
			num2 = i;
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_shoot_Exit_AnimSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x06000483 RID: 1155 RVA: 0x00015CB4 File Offset: 0x00013EB4
	protected virtual float m_spinAttack_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x06000484 RID: 1156 RVA: 0x00015CBB File Offset: 0x00013EBB
	protected virtual float m_spinAttack_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06000485 RID: 1157 RVA: 0x00015CC2 File Offset: 0x00013EC2
	protected virtual float m_spinAttack_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x06000486 RID: 1158 RVA: 0x00015CC9 File Offset: 0x00013EC9
	protected virtual float m_spinAttack_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06000487 RID: 1159 RVA: 0x00015CD0 File Offset: 0x00013ED0
	protected virtual float m_spinAttack_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06000488 RID: 1160 RVA: 0x00015CD7 File Offset: 0x00013ED7
	protected virtual float m_spinAttack_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06000489 RID: 1161 RVA: 0x00015CDE File Offset: 0x00013EDE
	protected virtual float m_spinAttack_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x0600048A RID: 1162 RVA: 0x00015CE5 File Offset: 0x00013EE5
	protected virtual float m_spinAttack_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x0600048B RID: 1163 RVA: 0x00015CEC File Offset: 0x00013EEC
	protected virtual float m_spinAttack_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x0600048C RID: 1164 RVA: 0x00015CF3 File Offset: 0x00013EF3
	protected virtual float m_spinAttack_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x0600048D RID: 1165 RVA: 0x00015CFA File Offset: 0x00013EFA
	protected virtual float m_spinAttack_Exit_AttackCD
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x0600048E RID: 1166 RVA: 0x00015D01 File Offset: 0x00013F01
	protected virtual float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x0600048F RID: 1167 RVA: 0x00015D08 File Offset: 0x00013F08
	protected virtual int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06000490 RID: 1168 RVA: 0x00015D0B File Offset: 0x00013F0B
	protected virtual int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000491 RID: 1169 RVA: 0x00015D0E File Offset: 0x00013F0E
	protected virtual float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 180f;
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00015D15 File Offset: 0x00013F15
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
		yield return this.Default_TellIntroAndLoop("Spin_Tell_Intro", this.m_spinAttack_TellIntro_AnimSpeed, "Spin_Tell_Hold", this.m_spinAttack_TellHold_AnimSpeed, this.m_spinAttack_Tell_Delay);
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_spinAttack_AttackIntro_AnimSpeed, this.m_spinAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_spinAttack_AttackHold_AnimSpeed, 0f, false);
		Vector2 pt = base.EnemyController.TargetController.Midpoint - base.EnemyController.transform.position;
		float initialFireAngle = CDGHelper.VectorToAngle(pt);
		if (this.m_spinAttack_TotalShots > 0)
		{
			int num = 360 / this.m_spinAttack_TotalShots;
		}
		int num2;
		for (int i = 0; i < this.m_spinAttack_ShotPatternLoops; i = num2 + 1)
		{
			for (int j = 0; j < this.m_spinAttack_TotalShots; j++)
			{
				float angle = initialFireAngle + UnityEngine.Random.Range(-this.m_spinAttack_projectile_RandomSpread, this.m_spinAttack_projectile_RandomSpread);
				this.FireProjectile("ElementalIceBoltProjectile", 0, false, angle, 1f, true, true, true);
			}
			if (i < this.m_spinAttack_ShotPatternLoops - 1 && this.m_spinAttack_TimesShotDelay > 0f)
			{
				yield return base.Wait(this.m_spinAttack_TimesShotDelay, false);
			}
			num2 = i;
		}
		yield return this.Default_Animation("Spin_Exit", this.m_spinAttack_Exit_AnimSpeed, this.m_spinAttack_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_spinAttack_Exit_ForceIdle, this.m_spinAttack_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x04000841 RID: 2113
	[SerializeField]
	private int m_elementalType;

	// Token: 0x04000842 RID: 2114
	protected const string ICEBALL_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000843 RID: 2115
	protected const string ICEBALL_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000844 RID: 2116
	protected const string ICEBALL_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000845 RID: 2117
	protected const string ICEBALL_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000846 RID: 2118
	protected const string ICEBALL_EXIT = "Shoot_Exit";

	// Token: 0x04000847 RID: 2119
	protected const string ICEBALL_PROJECTILE = "ElementalIceBoltProjectile";

	// Token: 0x04000848 RID: 2120
	protected const string SPINATTACK_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x04000849 RID: 2121
	protected const string SPINATTACK_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x0400084A RID: 2122
	protected const string SPINATTACK_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x0400084B RID: 2123
	protected const string SPINATTACK_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x0400084C RID: 2124
	protected const string SPINATTACK_EXIT = "Spin_Exit";

	// Token: 0x0400084D RID: 2125
	protected const string SPINATTACK_PROJECTILE = "ElementalIceBoltProjectile";
}
