using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class Fireball_Basic_AIScript : BaseAIScript, IBodyOnEnterHitResponse, IHitResponse
{
	// Token: 0x0600083C RID: 2108 RVA: 0x00005F15 File Offset: 0x00004115
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.FIREBALL_PROJECTILE,
			this.FIREBALL_HOMING_PROJECTILE
		};
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00005F35 File Offset: 0x00004135
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.LogicController.OverrideLogicDelay(0.7f);
		this.m_trailRenderer = base.EnemyController.GetComponentInChildren<TrailRenderer>();
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x00005F64 File Offset: 0x00004164
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_trailRenderer)
		{
			this.m_trailRenderer.Clear();
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x0600083F RID: 2111 RVA: 0x00005F84 File Offset: 0x00004184
	protected virtual string FIREBALL_PROJECTILE
	{
		get
		{
			return "FireballEnemyProjectile";
		}
	}

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06000840 RID: 2112 RVA: 0x00005F8B File Offset: 0x0000418B
	protected virtual string FIREBALL_HOMING_PROJECTILE
	{
		get
		{
			return "FireballMinibossEnemyProjectile";
		}
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x06000841 RID: 2113 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x06000842 RID: 2114 RVA: 0x00005F92 File Offset: 0x00004192
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.5f, 1.5f);
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x06000843 RID: 2115 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06000844 RID: 2116 RVA: 0x00005FA3 File Offset: 0x000041A3
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x06000845 RID: 2117 RVA: 0x00005FA3 File Offset: 0x000041A3
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06000846 RID: 2118 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_fireballSpeedMultiplier
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x06000847 RID: 2119 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_dropsFireballsWhileWalking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06000848 RID: 2120 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected virtual float m_timeBetweenWalkTowardFireballDrops
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x000628C8 File Offset: 0x00060AC8
	private void FixedUpdate()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (this.m_dropsFireballsWhileWalking && base.LogicController && base.LogicController.CurrentLogicBlockName != null)
		{
			if (base.LogicController.CurrentLogicBlockName.StartsWith("Walk"))
			{
				if (this.m_timeSinceLastWalkTowardsFireball >= this.m_timeBetweenWalkTowardFireballDrops)
				{
					this.DropFireball(false);
					this.m_timeSinceLastWalkTowardsFireball = 0f;
					return;
				}
				this.m_timeSinceLastWalkTowardsFireball += Time.deltaTime;
				return;
			}
			else
			{
				this.m_timeSinceLastWalkTowardsFireball = 0f;
			}
		}
	}

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x0600084A RID: 2122 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x0600084B RID: 2123 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x0600084C RID: 2124 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x0600084D RID: 2125 RVA: 0x00005FAA File Offset: 0x000041AA
	protected virtual float m_dash_TellHold_Delay
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x0600084E RID: 2126 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x0600084F RID: 2127 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06000850 RID: 2128 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06000851 RID: 2129 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06000852 RID: 2130 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06000853 RID: 2131 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x06000854 RID: 2132 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x06000855 RID: 2133 RVA: 0x00005FB1 File Offset: 0x000041B1
	protected virtual float m_dash_Exit_AttackCD
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x06000856 RID: 2134 RVA: 0x00004548 File Offset: 0x00002748
	protected virtual float m_dash_Attack_ForwardSpeedOverride
	{
		get
		{
			return 27.5f;
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x06000857 RID: 2135 RVA: 0x00005FB8 File Offset: 0x000041B8
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 0.6f;
		}
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x06000858 RID: 2136 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x06000859 RID: 2137 RVA: 0x00003D8C File Offset: 0x00001F8C
	protected virtual float m_fireballDropDuringDashInterval
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00005FBF File Offset: 0x000041BF
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.DisableOffscreenWarnings = false;
		ProjectileManager.AttachOffscreenIcon(base.EnemyController, true);
		yield return this.Default_Animation("Dash_Tell_Intro", this.m_dash_TellIntro_AnimSpeed, this.m_dash_TellIntro_Delay, false);
		yield return this.Default_Animation("Dash_Tell_Hold", this.m_dash_TellHold_AnimSpeed, this.m_dash_TellHold_Delay, false);
		base.EnemyController.AttackingWithContactDamage = true;
		base.EnemyController.Heading = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		base.EnemyController.LockFlip = true;
		base.EnemyController.FollowTarget = false;
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dash_AttackIntro_AnimSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dash_AttackHold_AnimSpeed, this.m_dash_AttackHold_Delay, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_dash_Attack_ForwardSpeedOverride;
		if (this.m_dropsFireballsDuringDashAttack)
		{
			yield return this.DropFireballDuringDash();
		}
		else if (this.m_dash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_Duration, false);
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AttackingWithContactDamage = false;
		yield return this.Default_Animation("Dash_Exit", this.m_dash_Exit_AnimSpeed, this.m_dash_Exit_Delay, true);
		base.EnemyController.DisableOffscreenWarnings = true;
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_Exit_AttackCD);
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00005FCE File Offset: 0x000041CE
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator StunAttack()
	{
		this.StopAndFaceTarget();
		yield return this.Default_Animation("Stunned", this.m_dash_TellIntro_AnimSpeed, 5f, true);
		base.EnemyController.AttackingWithContactDamage = true;
		Vector2 pt = PlayerManager.GetPlayer().transform.position - base.EnemyController.transform.position;
		float angle = CDGHelper.VectorToAngle(pt);
		this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
		yield return base.Wait(0.15f, false);
		this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
		yield return base.Wait(0.15f, false);
		this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
		yield return base.Wait(0.15f, false);
		this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
		yield return base.Wait(0.15f, false);
		this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
		yield return base.Wait(0.15f, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AttackingWithContactDamage = false;
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_Exit_AttackCD);
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00005FDD File Offset: 0x000041DD
	private IEnumerator DropFireballDuringDash()
	{
		int numFireballs = (int)(this.m_dash_Attack_Duration / this.m_fireballDropDuringDashInterval);
		float remainingTime = this.m_dash_Attack_Duration - this.m_fireballDropDuringDashInterval * (float)numFireballs;
		int num;
		for (int i = 0; i < numFireballs; i = num + 1)
		{
			if (base.EnemyController.EnemyRank == EnemyRank.Miniboss)
			{
				this.DropFireball(false);
			}
			else
			{
				this.DropFireball(false);
			}
			if (this.m_fireballDropDuringDashInterval > 0f)
			{
				yield return base.Wait(this.m_fireballDropDuringDashInterval, false);
			}
			num = i;
		}
		if (remainingTime > 0f)
		{
			yield return base.Wait(remainingTime, false);
		}
		yield break;
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x0600085D RID: 2141 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int m_spawnFireballOnHitCount
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x0600085E RID: 2142 RVA: 0x00005FEC File Offset: 0x000041EC
	protected virtual Vector2 m_spawnFireballOnHitSpeedModRange
	{
		get
		{
			return new Vector2(1f, 1f);
		}
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00062958 File Offset: 0x00060B58
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (this.m_spawnFireballOnHitCount > 0)
		{
			int num = 360 / (this.m_spawnFireballOnHitCount + 1);
			for (int i = 0; i < this.m_spawnFireballOnHitCount; i++)
			{
				float angle = (float)UnityEngine.Random.Range(num * i, num * (i + 1));
				float speedMod = this.m_spawnFireballOnHitSpeedModRange.x;
				if (this.m_spawnFireballOnHitSpeedModRange.x != this.m_spawnFireballOnHitSpeedModRange.y)
				{
					speedMod = UnityEngine.Random.Range(this.m_spawnFireballOnHitSpeedModRange.x, this.m_spawnFireballOnHitSpeedModRange.y);
				}
				this.FireProjectile(this.FIREBALL_PROJECTILE, new Vector2(0f, 0f), false, angle, speedMod, true, true, true);
			}
		}
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00062A04 File Offset: 0x00060C04
	private void DropFireball(bool homing = false)
	{
		float num = 270f;
		if (homing)
		{
			num = CDGHelper.VectorToAngle(PlayerManager.GetPlayer().transform.position - base.EnemyController.transform.position);
			this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, num, this.m_fireballSpeedMultiplier, true, true, true);
			return;
		}
		if (base.EnemyController.EnemyRank == EnemyRank.Miniboss)
		{
			this.FireProjectile(this.FIREBALL_PROJECTILE, 0, false, num, this.m_fireballSpeedMultiplier, true, true, true);
			this.FireProjectile(this.FIREBALL_PROJECTILE, 0, false, num - 180f, this.m_fireballSpeedMultiplier, true, true, true);
			return;
		}
		this.FireProjectile(this.FIREBALL_PROJECTILE, 0, false, num, this.m_fireballSpeedMultiplier, true, true, true);
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00005FFD File Offset: 0x000041FD
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		base.EnemyController.DisableOffscreenWarnings = true;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x04000C1D RID: 3101
	private TrailRenderer m_trailRenderer;

	// Token: 0x04000C1E RID: 3102
	private float m_timeSinceLastWalkTowardsFireball;

	// Token: 0x04000C1F RID: 3103
	protected const string DASH_TELL_INTRO = "Dash_Tell_Intro";

	// Token: 0x04000C20 RID: 3104
	protected const string DASH_TELL_HOLD = "Dash_Tell_Hold";

	// Token: 0x04000C21 RID: 3105
	protected const string DASH_ATTACK_INTRO = "Dash_Attack_Intro";

	// Token: 0x04000C22 RID: 3106
	protected const string DASH_ATTACK_HOLD = "Dash_Attack_Hold";

	// Token: 0x04000C23 RID: 3107
	protected const string DASH_EXIT = "Dash_Exit";
}
