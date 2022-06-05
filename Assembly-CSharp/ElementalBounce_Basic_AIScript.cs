using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class ElementalBounce_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600039C RID: 924 RVA: 0x00015250 File Offset: 0x00013450
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

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x0600039D RID: 925 RVA: 0x000152A1 File Offset: 0x000134A1
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x0600039E RID: 926 RVA: 0x000152B2 File Offset: 0x000134B2
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x0600039F RID: 927 RVA: 0x000152C3 File Offset: 0x000134C3
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x060003A0 RID: 928 RVA: 0x000152D4 File Offset: 0x000134D4
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(3.5f, 10f);
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x060003A1 RID: 929 RVA: 0x000152E5 File Offset: 0x000134E5
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(1f, 6f);
		}
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x000152F6 File Offset: 0x000134F6
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		enemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00015315 File Offset: 0x00013515
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x060003A4 RID: 932 RVA: 0x0001533A File Offset: 0x0001353A
	protected virtual float m_fireSpire_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x060003A5 RID: 933 RVA: 0x00015341 File Offset: 0x00013541
	protected virtual float m_fireSpire_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x060003A6 RID: 934 RVA: 0x00015348 File Offset: 0x00013548
	protected virtual float m_fireSpire_Tell_Delay
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x060003A7 RID: 935 RVA: 0x0001534F File Offset: 0x0001354F
	protected virtual float m_fireSpire_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x060003A8 RID: 936 RVA: 0x00015356 File Offset: 0x00013556
	protected virtual float m_fireSpire_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x060003A9 RID: 937 RVA: 0x0001535D File Offset: 0x0001355D
	protected virtual float m_fireSpire_AttackHold_AnimSpeed
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x060003AA RID: 938 RVA: 0x00015364 File Offset: 0x00013564
	protected virtual float m_fireSpire_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x060003AB RID: 939 RVA: 0x0001536B File Offset: 0x0001356B
	protected virtual float m_fireSpire_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x060003AC RID: 940 RVA: 0x00015372 File Offset: 0x00013572
	protected virtual float m_fireSpire_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x060003AD RID: 941 RVA: 0x00015379 File Offset: 0x00013579
	protected virtual float m_fireSpire_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x060003AE RID: 942 RVA: 0x00015380 File Offset: 0x00013580
	protected virtual float m_fireSpire_Exit_AttackCD
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x060003AF RID: 943 RVA: 0x00015387 File Offset: 0x00013587
	protected virtual int m_fireSpire_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x060003B0 RID: 944 RVA: 0x0001538A File Offset: 0x0001358A
	protected virtual float m_fireSpire_Appear_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x060003B1 RID: 945 RVA: 0x00015391 File Offset: 0x00013591
	protected virtual bool m_fireSpire_Hard_Version
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00015394 File Offset: 0x00013594
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

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x060003B3 RID: 947 RVA: 0x000153A3 File Offset: 0x000135A3
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x060003B4 RID: 948 RVA: 0x000153AA File Offset: 0x000135AA
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x060003B5 RID: 949 RVA: 0x000153B1 File Offset: 0x000135B1
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x060003B6 RID: 950 RVA: 0x000153B8 File Offset: 0x000135B8
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x060003B7 RID: 951 RVA: 0x000153BF File Offset: 0x000135BF
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x060003B8 RID: 952 RVA: 0x000153C6 File Offset: 0x000135C6
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x060003B9 RID: 953 RVA: 0x000153CD File Offset: 0x000135CD
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x060003BA RID: 954 RVA: 0x000153D4 File Offset: 0x000135D4
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x060003BB RID: 955 RVA: 0x000153DB File Offset: 0x000135DB
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x060003BC RID: 956 RVA: 0x000153E2 File Offset: 0x000135E2
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x060003BD RID: 957 RVA: 0x000153E9 File Offset: 0x000135E9
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x060003BE RID: 958 RVA: 0x000153F0 File Offset: 0x000135F0
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x060003BF RID: 959 RVA: 0x000153F3 File Offset: 0x000135F3
	protected virtual float m_shoot_Appear_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x060003C0 RID: 960 RVA: 0x000153FA File Offset: 0x000135FA
	protected virtual bool m_shoot_Hard_Version
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x000153FD File Offset: 0x000135FD
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

	// Token: 0x060003C2 RID: 962 RVA: 0x0001540C File Offset: 0x0001360C
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.StopProjectile(ref this.m_bounceBallTellProjectile);
		base.StopProjectile(ref this.m_bounceBallTellProjectile2);
		base.StopProjectile(ref this.m_bounceBallTellProjectile3);
		base.StopProjectile(ref this.m_bounceBallTellProjectile4);
		base.StopProjectile(ref this.m_bounceBallTellProjectile5);
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x0001545C File Offset: 0x0001365C
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

	// Token: 0x040007FE RID: 2046
	[SerializeField]
	private int m_elementalType;

	// Token: 0x040007FF RID: 2047
	protected Projectile_RL m_bounceBallTellProjectile;

	// Token: 0x04000800 RID: 2048
	protected Projectile_RL m_bounceBallTellProjectile2;

	// Token: 0x04000801 RID: 2049
	protected Projectile_RL m_bounceBallTellProjectile3;

	// Token: 0x04000802 RID: 2050
	protected Projectile_RL m_bounceBallTellProjectile4;

	// Token: 0x04000803 RID: 2051
	protected Projectile_RL m_bounceBallTellProjectile5;

	// Token: 0x04000804 RID: 2052
	protected const string FIRESPIRE_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x04000805 RID: 2053
	protected const string FIRESPIRE_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x04000806 RID: 2054
	protected const string FIRESPIRE_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x04000807 RID: 2055
	protected const string FIRESPIRE_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x04000808 RID: 2056
	protected const string FIRESPIRE_EXIT = "Spin_Exit";

	// Token: 0x04000809 RID: 2057
	protected const string FIRESPIRE_PROJECTILE = "ElementalBounceBounceFragmentProjectile";

	// Token: 0x0400080A RID: 2058
	protected const string FIRESPIRE_HARD_PROJECTILE = "ElementalBounceBounceFragmentHomingProjectile";

	// Token: 0x0400080B RID: 2059
	protected const string FIRESPIRE_TELL_PROJECTILE = "ElementalBounceZoneWarningProjectile";

	// Token: 0x0400080C RID: 2060
	protected const string BOUNCEBALL_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x0400080D RID: 2061
	protected const string BOUNCEBALL_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x0400080E RID: 2062
	protected const string BOUNCEBALL_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x0400080F RID: 2063
	protected const string BOUNCEBALL_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000810 RID: 2064
	protected const string BOUNCEBALL_EXIT = "Shoot_Exit";

	// Token: 0x04000811 RID: 2065
	protected const string BOUNCEBALL_PROJECTILE = "ElementalBounceBounceProjectile";

	// Token: 0x04000812 RID: 2066
	protected const string BOUNCEBALL_HARD_PROJECTILE = "ElementalBounceBounceHomingProjectile";

	// Token: 0x04000813 RID: 2067
	protected const string BOUNCEBALL_FRAGMENT_PROJECTILE = "ElementalBounceBounceFragmentProjectile";

	// Token: 0x04000814 RID: 2068
	protected const string BOUNCEBALL_FRAGMENT_HOMING_PROJECTILE = "ElementalBounceBounceFragmentHomingProjectile";
}
