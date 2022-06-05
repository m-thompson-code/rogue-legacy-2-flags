using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class ElementalDash_Basic_AIScript : BaseAIScript
{
	// Token: 0x060003FD RID: 1021 RVA: 0x000156DE File Offset: 0x000138DE
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ElementalDashVoidProjectile",
			"ElementalDashVoidZoneProjectile",
			"ElementalDashVoidZoneLargeProjectile",
			"ElementalDashVoidZoneWarningProjectile"
		};
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x060003FE RID: 1022 RVA: 0x0001570C File Offset: 0x0001390C
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x060003FF RID: 1023 RVA: 0x0001571D File Offset: 0x0001391D
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06000400 RID: 1024 RVA: 0x0001572E File Offset: 0x0001392E
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06000401 RID: 1025 RVA: 0x0001573F File Offset: 0x0001393F
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-5f, 5f);
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x06000402 RID: 1026 RVA: 0x00015750 File Offset: 0x00013950
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 5f);
		}
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x00015761 File Offset: 0x00013961
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		enemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x00015780 File Offset: 0x00013980
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x06000405 RID: 1029 RVA: 0x000157A5 File Offset: 0x000139A5
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x06000406 RID: 1030 RVA: 0x000157AC File Offset: 0x000139AC
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x000157B3 File Offset: 0x000139B3
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000408 RID: 1032 RVA: 0x000157BA File Offset: 0x000139BA
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000409 RID: 1033 RVA: 0x000157C1 File Offset: 0x000139C1
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x0600040A RID: 1034 RVA: 0x000157C8 File Offset: 0x000139C8
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x0600040B RID: 1035 RVA: 0x000157CF File Offset: 0x000139CF
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x0600040C RID: 1036 RVA: 0x000157D6 File Offset: 0x000139D6
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x0600040D RID: 1037 RVA: 0x000157DD File Offset: 0x000139DD
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x0600040E RID: 1038 RVA: 0x000157E4 File Offset: 0x000139E4
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x0600040F RID: 1039 RVA: 0x000157EB File Offset: 0x000139EB
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06000410 RID: 1040 RVA: 0x000157F2 File Offset: 0x000139F2
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000411 RID: 1041 RVA: 0x000157F5 File Offset: 0x000139F5
	protected virtual float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000412 RID: 1042 RVA: 0x000157FC File Offset: 0x000139FC
	protected virtual float m_shoot_RandAngleOffset
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06000413 RID: 1043 RVA: 0x00015803 File Offset: 0x00013A03
	protected virtual bool m_shoot_EnableSpreadShot
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06000414 RID: 1044 RVA: 0x00015806 File Offset: 0x00013A06
	protected virtual int m_shoot_SpreadShot_Angle
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x0001580A File Offset: 0x00013A0A
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootVoidball()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_shoot_TellIntro_AnimSpeed, "Shoot_Tell_Hold", this.m_shoot_TellHold_AnimSpeed, this.m_shoot_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_shoot_AttackIntro_AnimSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_shoot_AttackHold_AnimSpeed, 0f, false);
		float fireAngle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
		float shotInterval = this.m_shoot_TotalShotDuration / (float)this.m_shoot_TotalShots;
		int num2;
		for (int i = 0; i < this.m_shoot_TotalShots; i = num2 + 1)
		{
			float num = UnityEngine.Random.Range(-this.m_shoot_RandAngleOffset, this.m_shoot_RandAngleOffset);
			fireAngle += num;
			if (this.m_shoot_EnableSpreadShot)
			{
				this.FireProjectile("ElementalDashVoidProjectile", 0, false, fireAngle, 1f, true, true, true);
				this.FireProjectile("ElementalDashVoidProjectile", 0, false, fireAngle + (float)this.m_shoot_SpreadShot_Angle, 1f, true, true, true);
				this.FireProjectile("ElementalDashVoidProjectile", 0, false, fireAngle - (float)this.m_shoot_SpreadShot_Angle, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile("ElementalDashVoidProjectile", 0, false, fireAngle, 1f, true, true, true);
				if (shotInterval > 0f && i < this.m_shoot_TotalShots - 1)
				{
					yield return base.Wait(shotInterval, false);
				}
			}
			num2 = i;
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_shoot_Exit_AnimSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06000416 RID: 1046 RVA: 0x00015819 File Offset: 0x00013A19
	protected virtual float m_voidwall_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000417 RID: 1047 RVA: 0x00015820 File Offset: 0x00013A20
	protected virtual float m_voidWall_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06000418 RID: 1048 RVA: 0x00015827 File Offset: 0x00013A27
	protected virtual float m_voidWall_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000419 RID: 1049 RVA: 0x0001582E File Offset: 0x00013A2E
	protected virtual float m_voidWall_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x0600041A RID: 1050 RVA: 0x00015835 File Offset: 0x00013A35
	protected virtual float m_voidWall_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x0600041B RID: 1051 RVA: 0x0001583C File Offset: 0x00013A3C
	protected virtual float m_voidWall_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x0600041C RID: 1052 RVA: 0x00015843 File Offset: 0x00013A43
	protected virtual float m_voidWall_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x0600041D RID: 1053 RVA: 0x0001584A File Offset: 0x00013A4A
	protected virtual float m_voidWall_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x0600041E RID: 1054 RVA: 0x00015851 File Offset: 0x00013A51
	protected virtual float m_voidWall_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x0600041F RID: 1055 RVA: 0x00015858 File Offset: 0x00013A58
	protected virtual float m_voidWall_Exit_ForceIdle
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06000420 RID: 1056 RVA: 0x0001585F File Offset: 0x00013A5F
	protected virtual float m_voidWall_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000421 RID: 1057 RVA: 0x00015866 File Offset: 0x00013A66
	protected virtual bool m_voidWall_CreateCenterWall
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000422 RID: 1058 RVA: 0x00015869 File Offset: 0x00013A69
	protected virtual bool m_voidWall_CreateSideWalls
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000423 RID: 1059 RVA: 0x0001586C File Offset: 0x00013A6C
	protected virtual float m_voidWall_SideWallsOffset
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000424 RID: 1060 RVA: 0x00015873 File Offset: 0x00013A73
	protected virtual bool m_voidWall_LargeWalls
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000425 RID: 1061 RVA: 0x00015876 File Offset: 0x00013A76
	protected virtual float m_voidWall_Appear_Delay
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0001587D File Offset: 0x00013A7D
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator VoidWall()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_TellIntroAndLoop("Spin_Tell_Intro", this.m_voidwall_TellIntro_AnimSpeed, "Spin_Tell_Hold", this.m_voidWall_TellHold_AnimSpeed, this.m_voidWall_Tell_Delay);
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_voidWall_AttackIntro_AnimSpeed, this.m_voidWall_AttackIntro_Delay, true);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_voidWall_AttackHold_AnimSpeed, 0f, false);
		Vector3 projectilePos = base.EnemyController.TargetController.transform.position;
		this.m_voidWallTellProjectile = this.FireProjectile("ElementalDashVoidZoneWarningProjectile", 1, true, 90f, 1f, true, true, true);
		projectilePos.z = this.m_voidWallTellProjectile.transform.position.z;
		this.m_voidWallTellProjectile.transform.position = projectilePos;
		if (this.m_voidWall_CreateSideWalls)
		{
			Vector3 vector = projectilePos;
			vector += new Vector3(this.m_voidWall_SideWallsOffset, 0f, 0f);
			this.m_voidWallTellProjectile2 = this.FireProjectile("ElementalDashVoidZoneWarningProjectile", 1, true, 90f, 1f, true, true, true);
			vector.z = this.m_voidWallTellProjectile2.transform.position.z;
			this.m_voidWallTellProjectile2.transform.position = vector;
			vector = projectilePos;
			vector += new Vector3(-this.m_voidWall_SideWallsOffset, 0f, 0f);
			this.m_voidWallTellProjectile3 = this.FireProjectile("ElementalDashVoidZoneWarningProjectile", 1, true, 90f, 1f, true, true, true);
			vector.z = this.m_voidWallTellProjectile3.transform.position.z;
			this.m_voidWallTellProjectile3.transform.position = vector;
		}
		if (this.m_voidWall_Appear_Delay > 0f)
		{
			yield return base.Wait(this.m_voidWall_Appear_Delay, false);
		}
		base.StopProjectile(ref this.m_voidWallTellProjectile);
		if (this.m_voidWall_CreateSideWalls)
		{
			base.StopProjectile(ref this.m_voidWallTellProjectile2);
			base.StopProjectile(ref this.m_voidWallTellProjectile3);
		}
		if (this.m_voidWall_CreateCenterWall)
		{
			if (this.m_voidWall_LargeWalls)
			{
				Projectile_RL projectile_RL = this.FireProjectile("ElementalDashVoidZoneLargeProjectile", 0, false, 0f, 0f, true, true, true);
				projectilePos.z = projectile_RL.transform.position.z;
				projectile_RL.transform.position = projectilePos;
			}
			else if (!this.m_voidWall_LargeWalls)
			{
				Projectile_RL projectile_RL2 = this.FireProjectile("ElementalDashVoidZoneProjectile", 0, false, 0f, 0f, true, true, true);
				projectilePos.z = projectile_RL2.transform.position.z;
				projectile_RL2.transform.position = projectilePos;
			}
		}
		if (this.m_voidWall_CreateSideWalls)
		{
			Vector3 position = projectilePos;
			position.x += this.m_voidWall_SideWallsOffset;
			Projectile_RL projectile_RL3 = this.FireProjectile("ElementalDashVoidZoneProjectile", 0, false, 0f, 1f, true, true, true);
			position.z = projectile_RL3.transform.position.z;
			projectile_RL3.transform.position = position;
			position = projectilePos;
			position.x -= this.m_voidWall_SideWallsOffset;
			Projectile_RL projectile_RL4 = this.FireProjectile("ElementalDashVoidZoneProjectile", 0, false, 0f, 1f, true, true, true);
			position.z = projectile_RL4.transform.position.z;
			projectile_RL4.transform.position = position;
		}
		yield return this.Default_Animation("Spin_Exit", this.m_voidWall_Exit_AnimSpeed, this.m_voidWall_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_voidWall_Exit_ForceIdle, this.m_voidWall_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0001588C File Offset: 0x00013A8C
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.StopProjectile(ref this.m_voidWallTellProjectile);
		base.StopProjectile(ref this.m_voidWallTellProjectile2);
		base.StopProjectile(ref this.m_voidWallTellProjectile3);
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x000158B8 File Offset: 0x00013AB8
	private float GetTargetGroundYPos()
	{
		LayerMask mask = 256;
		mask |= 2048;
		RaycastHit2D hit = Physics2D.Raycast(base.EnemyController.TargetController.Midpoint, Vector2.down, 100f, mask);
		if (hit)
		{
			return hit.point.y;
		}
		return base.EnemyController.TargetController.transform.position.y;
	}

	// Token: 0x04000822 RID: 2082
	[SerializeField]
	private int m_elementalType;

	// Token: 0x04000823 RID: 2083
	protected const string VOID_WALL_TELL_PROJECTILE = "ElementalDashVoidZoneWarningProjectile";

	// Token: 0x04000824 RID: 2084
	protected Projectile_RL m_voidWallTellProjectile;

	// Token: 0x04000825 RID: 2085
	protected Projectile_RL m_voidWallTellProjectile2;

	// Token: 0x04000826 RID: 2086
	protected Projectile_RL m_voidWallTellProjectile3;

	// Token: 0x04000827 RID: 2087
	protected const string VOIDBALL_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000828 RID: 2088
	protected const string VOIDBALL_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000829 RID: 2089
	protected const string VOIDBALL_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x0400082A RID: 2090
	protected const string VOIDBALL_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x0400082B RID: 2091
	protected const string VOIDBALL_EXIT = "Shoot_Exit";

	// Token: 0x0400082C RID: 2092
	protected const string VOIDBALL_PROJECTILE = "ElementalDashVoidProjectile";

	// Token: 0x0400082D RID: 2093
	protected const string VOIDWALL_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x0400082E RID: 2094
	protected const string VOIDWALL_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x0400082F RID: 2095
	protected const string VOIDWALL_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x04000830 RID: 2096
	protected const string VOIDWALL_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x04000831 RID: 2097
	protected const string VOIDWALL_EXIT = "Spin_Exit";

	// Token: 0x04000832 RID: 2098
	protected const string VOIDWALL_PROJECTILE = "ElementalDashVoidZoneProjectile";

	// Token: 0x04000833 RID: 2099
	protected const string VOIDWALL_PROJECTILE_LARGE = "ElementalDashVoidZoneLargeProjectile";
}
