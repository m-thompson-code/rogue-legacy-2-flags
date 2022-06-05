using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class RogueKnight_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600083B RID: 2107 RVA: 0x0001C1A6 File Offset: 0x0001A3A6
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"RogueKnightBounceBoltProjectile"
		};
	}

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x0600083C RID: 2108 RVA: 0x0001C1BC File Offset: 0x0001A3BC
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.35f, 0.75f);
		}
	}

	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x0600083D RID: 2109 RVA: 0x0001C1CD File Offset: 0x0001A3CD
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.5f, 0.85f);
		}
	}

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x0600083E RID: 2110 RVA: 0x0001C1DE File Offset: 0x0001A3DE
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.5f, 0.5f);
		}
	}

	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x0600083F RID: 2111 RVA: 0x0001C1EF File Offset: 0x0001A3EF
	protected virtual float m_dash_TellIntro_AnimationSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x06000840 RID: 2112 RVA: 0x0001C1F6 File Offset: 0x0001A3F6
	protected virtual float m_dash_TellHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x06000841 RID: 2113 RVA: 0x0001C1FD File Offset: 0x0001A3FD
	protected virtual float m_dash_TellIntroAndHold_Delay
	{
		get
		{
			return 0.8f;
		}
	}

	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x06000842 RID: 2114 RVA: 0x0001C204 File Offset: 0x0001A404
	protected virtual float m_dash_RepeatTellIntroAndHold_Delay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x06000843 RID: 2115 RVA: 0x0001C20B File Offset: 0x0001A40B
	protected virtual float m_dash_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x06000844 RID: 2116 RVA: 0x0001C212 File Offset: 0x0001A412
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x06000845 RID: 2117 RVA: 0x0001C219 File Offset: 0x0001A419
	protected virtual float m_dash_AttackHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x06000846 RID: 2118 RVA: 0x0001C220 File Offset: 0x0001A420
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x06000847 RID: 2119 RVA: 0x0001C227 File Offset: 0x0001A427
	protected virtual float m_dash_Exit_AnimationSpeed
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x06000848 RID: 2120 RVA: 0x0001C22E File Offset: 0x0001A42E
	protected virtual float m_dash_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x06000849 RID: 2121 RVA: 0x0001C235 File Offset: 0x0001A435
	protected virtual float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x0600084A RID: 2122 RVA: 0x0001C23C File Offset: 0x0001A43C
	protected virtual float m_dash_AttackCD
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x0600084B RID: 2123 RVA: 0x0001C243 File Offset: 0x0001A443
	protected virtual bool m_raiseKnockbackDefenseWhileAttacking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x0600084C RID: 2124 RVA: 0x0001C246 File Offset: 0x0001A446
	protected virtual int m_knockbackDefenseBoostOverride
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x0600084D RID: 2125 RVA: 0x0001C249 File Offset: 0x0001A449
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 0.325f;
		}
	}

	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x0600084E RID: 2126 RVA: 0x0001C250 File Offset: 0x0001A450
	protected virtual float m_dash_Attack_LockDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x0600084F RID: 2127 RVA: 0x0001C257 File Offset: 0x0001A457
	protected virtual float m_dash_Attack_Speed
	{
		get
		{
			return 22f;
		}
	}

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x06000850 RID: 2128 RVA: 0x0001C25E File Offset: 0x0001A45E
	protected virtual float m_dash_RepeatAttack_Duration
	{
		get
		{
			return 0.325f;
		}
	}

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x06000851 RID: 2129 RVA: 0x0001C265 File Offset: 0x0001A465
	protected virtual float m_dash_RepeatAttack_LockDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x06000852 RID: 2130 RVA: 0x0001C26C File Offset: 0x0001A46C
	protected virtual float m_dash_RepeatAttack_Speed
	{
		get
		{
			return 22f;
		}
	}

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06000853 RID: 2131 RVA: 0x0001C273 File Offset: 0x0001A473
	protected virtual float m_dash_EndGravity_Delay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06000854 RID: 2132 RVA: 0x0001C27A File Offset: 0x0001A47A
	protected virtual int m_dash_NumDashes
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06000855 RID: 2133 RVA: 0x0001C27D File Offset: 0x0001A47D
	protected virtual int m_dash_OddstoThrowVersusDashing
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06000856 RID: 2134 RVA: 0x0001C280 File Offset: 0x0001A480
	protected virtual float m_throw_Exit_ForceIdle
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06000857 RID: 2135 RVA: 0x0001C287 File Offset: 0x0001A487
	protected virtual float m_throw_AttackCD
	{
		get
		{
			return 3.5f;
		}
	}

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06000858 RID: 2136 RVA: 0x0001C28E File Offset: 0x0001A48E
	protected virtual bool m_throw_Attack_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06000859 RID: 2137 RVA: 0x0001C291 File Offset: 0x0001A491
	protected virtual int m_throw_Attack_ProjectileAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x0600085A RID: 2138 RVA: 0x0001C294 File Offset: 0x0001A494
	protected virtual float m_throw_Attack_ProjectileDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x0600085B RID: 2139 RVA: 0x0001C29B File Offset: 0x0001A49B
	protected virtual bool m_throw_RepeatAttack_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x0600085C RID: 2140 RVA: 0x0001C29E File Offset: 0x0001A49E
	protected virtual int m_throw_RepeatAttack_ProjectileAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x0600085D RID: 2141 RVA: 0x0001C2A1 File Offset: 0x0001A4A1
	protected virtual float m_throw_RepeatAttack_ProjectileDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0001C2A8 File Offset: 0x0001A4A8
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		Vector3 midpoint = base.EnemyController.Midpoint;
		midpoint.z = -3f;
		this.m_aimIndicator.transform.position = midpoint;
		this.m_aimIndicator.layer = 29;
		this.m_aimIndicator.SetActive(false);
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0001C2FE File Offset: 0x0001A4FE
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		base.EnemyController.ControllerCorgi.GravityActive(false);
		base.EnemyController.LockFlip = false;
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		this.m_aimIndicator.SetActive(true);
		float desiredRotation = CDGHelper.AngleBetweenPts(base.EnemyController.Midpoint, base.EnemyController.TargetController.Midpoint);
		Vector3 localEulerAngles = this.m_aimIndicator.transform.localEulerAngles;
		localEulerAngles.z = desiredRotation;
		this.m_aimIndicator.transform.localEulerAngles = localEulerAngles;
		yield return this.Default_TellIntroAndLoop("Dash_Tell_Intro", this.m_dash_TellIntro_AnimationSpeed, "Dash_Tell_Hold", this.m_dash_TellHold_AnimationSpeed, this.m_dash_TellIntroAndHold_Delay - this.m_dash_Attack_LockDelay);
		localEulerAngles = this.m_aimIndicator.transform.localEulerAngles;
		localEulerAngles.z = desiredRotation;
		this.m_aimIndicator.transform.localEulerAngles = localEulerAngles;
		this.m_aimIndicator.SetActive(false);
		if (this.m_dash_Attack_LockDelay > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_LockDelay, false);
		}
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dash_AttackIntro_AnimationSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dash_AttackHold_AnimationSpeed, this.m_dash_AttackHold_Delay, false);
		Vector2 vector = CDGHelper.AngleToVector(desiredRotation);
		if (this.m_raiseKnockbackDefenseWhileAttacking)
		{
			base.EnemyController.BaseKnockbackDefense = (float)this.m_knockbackDefenseBoostOverride;
		}
		base.EnemyController.ControllerCorgi.DisableOneWayCollision = true;
		base.EnemyController.DisableFriction = true;
		base.EnemyController.FallLedge = true;
		base.EnemyController.SetVelocity(vector.x * this.m_dash_Attack_Speed, vector.y * this.m_dash_Attack_Speed, false);
		if (this.m_dash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_Duration, false);
		}
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.ControllerCorgi.DisableOneWayCollision = false;
		base.EnemyController.DisableFriction = false;
		base.EnemyController.FallLedge = false;
		if (UnityEngine.Random.Range(0, this.m_dash_OddstoThrowVersusDashing) == 0)
		{
			if (this.m_dash_NumDashes >= 2)
			{
				base.EnemyController.LockFlip = false;
				int num;
				for (int i = 1; i < this.m_dash_NumDashes; i = num + 1)
				{
					yield return this.DashRepeat();
					num = i;
				}
			}
			if (this.m_dash_EndGravity_Delay > 0f)
			{
				yield return base.Wait(this.m_dash_EndGravity_Delay, false);
			}
			base.EnemyController.LockFlip = false;
			base.EnemyController.ControllerCorgi.GravityActive(true);
			yield return this.Default_Animation("Dash_Exit", this.m_dash_Exit_AnimationSpeed, this.m_dash_Exit_Delay, true);
		}
		else
		{
			yield return this.ThrowRepeat();
			if (this.m_dash_EndGravity_Delay > 0f)
			{
				yield return base.Wait(this.m_dash_EndGravity_Delay, false);
			}
			base.EnemyController.LockFlip = false;
			base.EnemyController.ControllerCorgi.GravityActive(true);
			yield return this.Default_Animation("Throw_Exit", this.m_throw_Exit_AnimationSpeed, this.m_throw_Exit_Delay, true);
		}
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_AttackCD);
		yield break;
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0001C30D File Offset: 0x0001A50D
	protected IEnumerator DashRepeat()
	{
		base.EnemyController.LockFlip = false;
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		this.m_aimIndicator.SetActive(true);
		float desiredRotation = CDGHelper.AngleBetweenPts(this.m_aimIndicator.transform.position, base.EnemyController.TargetController.Midpoint);
		Vector3 localEulerAngles = this.m_aimIndicator.transform.localEulerAngles;
		localEulerAngles.z = desiredRotation;
		this.m_aimIndicator.transform.localEulerAngles = localEulerAngles;
		yield return this.Default_TellIntroAndLoop("Dash_Tell_Intro", this.m_dash_TellIntro_AnimationSpeed, "Dash_Tell_Hold", this.m_dash_TellHold_AnimationSpeed, this.m_dash_RepeatTellIntroAndHold_Delay - this.m_dash_RepeatAttack_LockDelay);
		localEulerAngles = this.m_aimIndicator.transform.localEulerAngles;
		localEulerAngles.z = desiredRotation;
		this.m_aimIndicator.transform.localEulerAngles = localEulerAngles;
		this.m_aimIndicator.SetActive(false);
		if (this.m_dash_RepeatAttack_LockDelay > 0f)
		{
			yield return base.Wait(this.m_dash_RepeatAttack_LockDelay, false);
		}
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dash_AttackIntro_AnimationSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dash_AttackHold_AnimationSpeed, this.m_dash_AttackHold_Delay, false);
		if (this.m_raiseKnockbackDefenseWhileAttacking)
		{
			base.EnemyController.BaseKnockbackDefense = (float)this.m_knockbackDefenseBoostOverride;
		}
		Vector2 vector = CDGHelper.AngleToVector(desiredRotation);
		base.EnemyController.SetVelocity(vector.x * this.m_dash_RepeatAttack_Speed, vector.y * this.m_dash_RepeatAttack_Speed, false);
		if (this.m_dash_RepeatAttack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_RepeatAttack_Duration, false);
		}
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.LockFlip = false;
		yield break;
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0001C31C File Offset: 0x0001A51C
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ThrowAttack()
	{
		this.StopAndFaceTarget();
		if (!this.m_throw_Attack_TargetPlayer)
		{
			base.EnemyController.LockFlip = true;
		}
		yield return this.Default_TellIntroAndLoop("Throw_Tell_Intro", this.m_throw_TellIntro_AnimationSpeed, "Throw_Tell_Hold", this.m_throw_TellHold_AnimationSpeed, this.m_throw_TellIntroAndHold_Delay);
		yield return this.Default_Animation("Throw_Attack_Intro", this.m_throw_AttackIntro_AnimationSpeed, this.m_throw_AttackIntro_Delay, true);
		yield return this.Default_Animation("Throw_Attack_Hold", this.m_throw_AttackHold_AnimationSpeed, 0f, false);
		int num2;
		for (int i = 0; i < this.m_throw_Attack_ProjectileAmount; i = num2 + 1)
		{
			if (this.m_throw_Attack_TargetPlayer)
			{
				float num = Mathf.Atan2(base.EnemyController.TargetController.Midpoint.y - base.EnemyController.Midpoint.y, base.EnemyController.TargetController.Midpoint.x - base.EnemyController.Midpoint.x);
				num = CDGHelper.WrapAngleDegrees(CDGHelper.ToDegrees(num), false);
				this.FireProjectile("RogueKnightBounceBoltProjectile", 1, false, num, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile("RogueKnightBounceBoltProjectile", 1, true, 0f, 1f, true, true, true);
			}
			if (this.m_throw_Attack_ProjectileAmount > 1 && this.m_throw_Attack_ProjectileDelay > 0f)
			{
				yield return base.Wait(this.m_throw_Attack_ProjectileDelay, false);
			}
			num2 = i;
		}
		yield return base.Wait(this.m_throw_AttackHold_Delay, false);
		yield return this.Default_Animation("Throw_Exit", this.m_throw_Exit_AnimationSpeed, this.m_throw_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_throw_Exit_ForceIdle, this.m_throw_AttackCD);
		yield break;
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x0001C32B File Offset: 0x0001A52B
	protected IEnumerator ThrowRepeat()
	{
		this.StopAndFaceTarget();
		if (!this.m_throw_Attack_TargetPlayer)
		{
			base.EnemyController.LockFlip = true;
		}
		yield return this.Default_TellIntroAndLoop("Throw_Tell_Intro", this.m_throw_TellIntro_AnimationSpeed, "Throw_Tell_Hold", this.m_throw_TellHold_AnimationSpeed, this.m_throw_RepeatTellIntroAndHold_Delay);
		yield return this.Default_Animation("Throw_Attack_Intro", this.m_throw_AttackIntro_AnimationSpeed, this.m_throw_AttackIntro_Delay, true);
		yield return this.Default_Animation("Throw_Attack_Hold", this.m_throw_AttackHold_AnimationSpeed, 0f, false);
		int num2;
		for (int i = 0; i < this.m_throw_RepeatAttack_ProjectileAmount; i = num2 + 1)
		{
			if (this.m_throw_RepeatAttack_TargetPlayer)
			{
				float num = Mathf.Atan2(base.EnemyController.TargetController.Midpoint.y - base.EnemyController.Midpoint.y, base.EnemyController.TargetController.Midpoint.x - base.EnemyController.Midpoint.x);
				num = CDGHelper.WrapAngleDegrees(CDGHelper.ToDegrees(num), false);
				this.FireProjectile("RogueKnightBounceBoltProjectile", 1, false, num, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile("RogueKnightBounceBoltProjectile", 1, true, 0f, 1f, true, true, true);
			}
			if (this.m_throw_RepeatAttack_ProjectileAmount > 1 && this.m_throw_Attack_ProjectileDelay > 0f)
			{
				yield return base.Wait(this.m_throw_RepeatAttack_ProjectileDelay, false);
			}
			num2 = i;
		}
		yield break;
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x0001C33C File Offset: 0x0001A53C
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.EnemyController.ControllerCorgi.DisableOneWayCollision = false;
		base.EnemyController.DisableFriction = false;
		this.m_aimIndicator.SetActive(false);
		base.EnemyController.ControllerCorgi.GravityActive(true);
		base.EnemyController.LockFlip = false;
	}

	// Token: 0x04000B93 RID: 2963
	protected float m_throw_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000B94 RID: 2964
	protected float m_throw_TellHold_AnimationSpeed = 0.65f;

	// Token: 0x04000B95 RID: 2965
	protected float m_throw_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04000B96 RID: 2966
	protected float m_throw_RepeatTellIntroAndHold_Delay = 0.5f;

	// Token: 0x04000B97 RID: 2967
	protected float m_throw_AttackIntro_AnimationSpeed = 2f;

	// Token: 0x04000B98 RID: 2968
	protected float m_throw_AttackIntro_Delay;

	// Token: 0x04000B99 RID: 2969
	protected float m_throw_AttackHold_AnimationSpeed = 2f;

	// Token: 0x04000B9A RID: 2970
	protected float m_throw_AttackHold_Delay = 0.25f;

	// Token: 0x04000B9B RID: 2971
	protected float m_throw_Exit_AnimationSpeed = 0.45f;

	// Token: 0x04000B9C RID: 2972
	protected float m_throw_Exit_Delay = 0.15f;

	// Token: 0x04000B9D RID: 2973
	[SerializeField]
	private GameObject m_aimIndicator;
}
