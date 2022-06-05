using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class ElementalBounce_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600052D RID: 1325 RVA: 0x00057CCC File Offset: 0x00055ECC
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ElementalBounceBounceProjectile",
			"ElementalBounceBounceHomingProjectile",
			"ElementalBounceBounceFragmentProjectile",
			"ElementalBounceBounceFragmentHomingProjectile",
			"ElementalBounceBounceFragmentProjectile",
			"ElementalBounceBounceFragmentHomingProjectile",
			"ElementalBounceZoneWarningProjectile"
		};
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x0600052E RID: 1326 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x0600052F RID: 1327 RVA: 0x00004F67 File Offset: 0x00003167
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06000530 RID: 1328 RVA: 0x00004F67 File Offset: 0x00003167
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06000531 RID: 1329 RVA: 0x00004F78 File Offset: 0x00003178
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(3.5f, 10f);
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06000532 RID: 1330 RVA: 0x00004F89 File Offset: 0x00003189
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(1f, 6f);
		}
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00004F9A File Offset: 0x0000319A
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		enemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00004FB9 File Offset: 0x000031B9
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06000535 RID: 1333 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_fireSpire_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06000536 RID: 1334 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_fireSpire_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06000537 RID: 1335 RVA: 0x00004FDE File Offset: 0x000031DE
	protected virtual float m_fireSpire_Tell_Delay
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06000538 RID: 1336 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_fireSpire_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x06000539 RID: 1337 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_fireSpire_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x0600053A RID: 1338 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_fireSpire_AttackHold_AnimSpeed
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x0600053B RID: 1339 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_fireSpire_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x0600053C RID: 1340 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_fireSpire_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x0600053D RID: 1341 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_fireSpire_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x0600053E RID: 1342 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_fireSpire_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x0600053F RID: 1343 RVA: 0x00003C62 File Offset: 0x00001E62
	protected virtual float m_fireSpire_Exit_AttackCD
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06000540 RID: 1344 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_fireSpire_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06000541 RID: 1345 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_fireSpire_Appear_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06000542 RID: 1346 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_fireSpire_Hard_Version
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00004FEC File Offset: 0x000031EC
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator SpinAttack()
	{
		string projectileName = "ElementalBounceBounceFragmentProjectile";
		if (this.m_fireSpire_Hard_Version)
		{
			projectileName = "ElementalBounceBounceFragmentHomingProjectile";
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AlwaysFacing = false;
		Vector3 projectilePos = base.EnemyController.TargetController.transform.position;
		Vector3 projectilePos2 = base.EnemyController.TargetController.transform.position;
		Vector3 projectilePos3 = base.EnemyController.TargetController.transform.position;
		Vector3 projectilePos4 = base.EnemyController.TargetController.transform.position;
		Vector3 projectilePos5 = base.EnemyController.TargetController.transform.position;
		float groundYPos = this.GetTargetGroundYPos();
		projectilePos.y = groundYPos + 2f;
		projectilePos2.y = groundYPos + 2f;
		projectilePos3.y = groundYPos + 2f;
		projectilePos4.y = groundYPos + 2f;
		projectilePos5.y = groundYPos + 2f;
		projectilePos2.x -= 1.5f;
		projectilePos3.x += 1.5f;
		projectilePos4.x -= 3f;
		projectilePos5.x += 3f;
		this.m_bounceBallTellProjectile = this.FireProjectile("ElementalBounceZoneWarningProjectile", 1, true, 90f, 1f, true, true, true);
		projectilePos.z = this.m_bounceBallTellProjectile.transform.position.z;
		this.m_bounceBallTellProjectile.transform.position = projectilePos;
		this.m_bounceBallTellProjectile2 = this.FireProjectile("ElementalBounceZoneWarningProjectile", 1, true, 90f, 1f, true, true, true);
		projectilePos2.z = this.m_bounceBallTellProjectile2.transform.position.z;
		this.m_bounceBallTellProjectile2.transform.position = projectilePos2;
		this.m_bounceBallTellProjectile3 = this.FireProjectile("ElementalBounceZoneWarningProjectile", 1, true, 90f, 1f, true, true, true);
		projectilePos3.z = this.m_bounceBallTellProjectile3.transform.position.z;
		this.m_bounceBallTellProjectile3.transform.position = projectilePos3;
		this.m_bounceBallTellProjectile4 = this.FireProjectile("ElementalBounceZoneWarningProjectile", 1, true, 90f, 1f, true, true, true);
		projectilePos3.z = this.m_bounceBallTellProjectile3.transform.position.z;
		this.m_bounceBallTellProjectile4.transform.position = projectilePos4;
		this.m_bounceBallTellProjectile5 = this.FireProjectile("ElementalBounceZoneWarningProjectile", 1, true, 90f, 1f, true, true, true);
		projectilePos3.z = this.m_bounceBallTellProjectile3.transform.position.z;
		this.m_bounceBallTellProjectile5.transform.position = projectilePos5;
		yield return this.Default_TellIntroAndLoop("Spin_Tell_Intro", this.m_fireSpire_TellIntro_AnimSpeed, "Spin_Tell_Hold", this.m_fireSpire_TellHold_AnimSpeed, this.m_fireSpire_Tell_Delay);
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_fireSpire_AttackIntro_AnimSpeed, this.m_fireSpire_AttackIntro_Delay, true);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_fireSpire_AttackHold_AnimSpeed, 0f, false);
		int num;
		for (int i = 0; i < this.m_fireSpire_TotalShots; i = num + 1)
		{
			projectilePos.y = groundYPos + 0.35f;
			projectilePos2.y = groundYPos + 0.35f;
			projectilePos3.y = groundYPos + 0.35f;
			projectilePos4.y = groundYPos + 0.35f;
			projectilePos5.y = groundYPos + 0.35f;
			if (this.m_fireSpire_Appear_Delay > 0f)
			{
				yield return base.Wait(this.m_fireSpire_Appear_Delay, false);
			}
			base.StopProjectile(ref this.m_bounceBallTellProjectile);
			base.StopProjectile(ref this.m_bounceBallTellProjectile2);
			base.StopProjectile(ref this.m_bounceBallTellProjectile3);
			base.StopProjectile(ref this.m_bounceBallTellProjectile4);
			base.StopProjectile(ref this.m_bounceBallTellProjectile5);
			Projectile_RL projectile_RL = this.FireProjectile(projectileName, 0, false, 90f, 1f, true, true, true);
			projectilePos.z = projectile_RL.transform.position.z;
			projectile_RL.transform.position = projectilePos;
			projectile_RL = this.FireProjectile(projectileName, 0, false, 90f, 1f, true, true, true);
			projectilePos2.z = projectile_RL.transform.position.z;
			projectile_RL.transform.position = projectilePos2;
			projectile_RL = this.FireProjectile(projectileName, 0, false, 90f, 1f, true, true, true);
			projectilePos3.z = projectile_RL.transform.position.z;
			projectile_RL.transform.position = projectilePos3;
			projectile_RL = this.FireProjectile(projectileName, 0, false, 90f, 1f, true, true, true);
			projectilePos5.z = projectile_RL.transform.position.z;
			projectile_RL.transform.position = projectilePos4;
			projectile_RL = this.FireProjectile(projectileName, 0, false, 90f, 1f, true, true, true);
			projectilePos5.z = projectile_RL.transform.position.z;
			projectile_RL.transform.position = projectilePos5;
			num = i;
		}
		yield return this.Default_Animation("Spin_Exit", this.m_fireSpire_Exit_AnimSpeed, this.m_fireSpire_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_fireSpire_Exit_ForceIdle, this.m_fireSpire_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x06000544 RID: 1348 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06000545 RID: 1349 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06000546 RID: 1350 RVA: 0x00004FFB File Offset: 0x000031FB
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06000547 RID: 1351 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x06000548 RID: 1352 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06000549 RID: 1353 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x0600054A RID: 1354 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x0600054B RID: 1355 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x0600054C RID: 1356 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x0600054D RID: 1357 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x0600054E RID: 1358 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x0600054F RID: 1359 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000550 RID: 1360 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Appear_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000551 RID: 1361 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_shoot_Hard_Version
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00005002 File Offset: 0x00003202
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootBounceball()
	{
		string projectileName = "ElementalBounceBounceProjectile";
		if (this.m_shoot_Hard_Version)
		{
			projectileName = "ElementalBounceBounceHomingProjectile";
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_shoot_TellIntro_AnimSpeed, "Shoot_Tell_Hold", this.m_shoot_TellHold_AnimSpeed, this.m_shoot_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_shoot_AttackIntro_AnimSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_shoot_AttackHold_AnimSpeed, 0f, false);
		int num;
		for (int i = 0; i < this.m_shoot_TotalShots; i = num + 1)
		{
			if (this.m_shoot_Appear_Delay > 0f)
			{
				yield return base.Wait(this.m_shoot_Appear_Delay, false);
			}
			float angle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
			this.FireProjectile(projectileName, 0, false, angle, 1f, true, true, true);
			num = i;
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_shoot_Exit_AnimSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x00057D20 File Offset: 0x00055F20
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.StopProjectile(ref this.m_bounceBallTellProjectile);
		base.StopProjectile(ref this.m_bounceBallTellProjectile2);
		base.StopProjectile(ref this.m_bounceBallTellProjectile3);
		base.StopProjectile(ref this.m_bounceBallTellProjectile4);
		base.StopProjectile(ref this.m_bounceBallTellProjectile5);
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00057D70 File Offset: 0x00055F70
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

	// Token: 0x04000933 RID: 2355
	[SerializeField]
	private int m_elementalType;

	// Token: 0x04000934 RID: 2356
	protected Projectile_RL m_bounceBallTellProjectile;

	// Token: 0x04000935 RID: 2357
	protected Projectile_RL m_bounceBallTellProjectile2;

	// Token: 0x04000936 RID: 2358
	protected Projectile_RL m_bounceBallTellProjectile3;

	// Token: 0x04000937 RID: 2359
	protected Projectile_RL m_bounceBallTellProjectile4;

	// Token: 0x04000938 RID: 2360
	protected Projectile_RL m_bounceBallTellProjectile5;

	// Token: 0x04000939 RID: 2361
	protected const string FIRESPIRE_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x0400093A RID: 2362
	protected const string FIRESPIRE_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x0400093B RID: 2363
	protected const string FIRESPIRE_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x0400093C RID: 2364
	protected const string FIRESPIRE_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x0400093D RID: 2365
	protected const string FIRESPIRE_EXIT = "Spin_Exit";

	// Token: 0x0400093E RID: 2366
	protected const string FIRESPIRE_PROJECTILE = "ElementalBounceBounceFragmentProjectile";

	// Token: 0x0400093F RID: 2367
	protected const string FIRESPIRE_HARD_PROJECTILE = "ElementalBounceBounceFragmentHomingProjectile";

	// Token: 0x04000940 RID: 2368
	protected const string FIRESPIRE_TELL_PROJECTILE = "ElementalBounceZoneWarningProjectile";

	// Token: 0x04000941 RID: 2369
	protected const string BOUNCEBALL_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000942 RID: 2370
	protected const string BOUNCEBALL_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000943 RID: 2371
	protected const string BOUNCEBALL_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000944 RID: 2372
	protected const string BOUNCEBALL_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000945 RID: 2373
	protected const string BOUNCEBALL_EXIT = "Shoot_Exit";

	// Token: 0x04000946 RID: 2374
	protected const string BOUNCEBALL_PROJECTILE = "ElementalBounceBounceProjectile";

	// Token: 0x04000947 RID: 2375
	protected const string BOUNCEBALL_HARD_PROJECTILE = "ElementalBounceBounceHomingProjectile";

	// Token: 0x04000948 RID: 2376
	protected const string BOUNCEBALL_FRAGMENT_PROJECTILE = "ElementalBounceBounceFragmentProjectile";

	// Token: 0x04000949 RID: 2377
	protected const string BOUNCEBALL_FRAGMENT_HOMING_PROJECTILE = "ElementalBounceBounceFragmentHomingProjectile";
}
