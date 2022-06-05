using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class Plant_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000BA6 RID: 2982 RVA: 0x00007260 File Offset: 0x00005460
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"PlantSpawnSporeProjectile",
			"PlantFallSporeProjectile",
			"PlantHomingSporeProjectile",
			"PlantHomingSporeProjectileExpertVariant",
			"PlantBoltProjectile"
		};
	}

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x00007296 File Offset: 0x00005496
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.LockFlip = true;
	}

	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x000072C1 File Offset: 0x000054C1
	protected virtual Vector2 m_numSporesFired
	{
		get
		{
			return new Vector2(4f, 7f);
		}
	}

	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06000BAA RID: 2986 RVA: 0x00004A48 File Offset: 0x00002C48
	protected virtual Vector2 m_delayBetweenSporesFired
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x06000BAB RID: 2987 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_spore_Exit_ForceIdle
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x06000BAC RID: 2988 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spore_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x000072D2 File Offset: 0x000054D2
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootSpores()
	{
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_spore_TellIntro_AnimationSpeed, "Shoot_Tell_Hold", this.m_spore_TellHold_AnimationSpeed, this.m_spore_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_spore_AttackIntro_AnimationSpeed, this.m_spore_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_spore_AttackHold_AnimationSpeed, this.m_spore_AttackHold_Delay, false);
		UnityEngine.Random.Range((int)this.m_numSporesFired.x, (int)this.m_numSporesFired.y + 1);
		this.FireProjectile("PlantSpawnSporeProjectile", 0, false, 120f, 1f, true, true, true);
		this.FireProjectile("PlantSpawnSporeProjectile", 0, false, 90f, 1f, true, true, true);
		this.FireProjectile("PlantSpawnSporeProjectile", 0, false, 60f, 1f, true, true, true);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			this.FireProjectile("PlantSpawnSporeProjectile", 0, false, 30f, 1f, true, true, true);
			this.FireProjectile("PlantSpawnSporeProjectile", 0, false, 150f, 1f, true, true, true);
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_spore_Exit_AnimationSpeed, this.m_spore_Exit_Delay, true);
		yield return this.Default_Attack_Cooldown(this.m_spore_Exit_ForceIdle, this.m_spore_AttackCD);
		yield break;
	}

	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x06000BAE RID: 2990 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_altAttack_Exit_ForceIdle
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_altAttack_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x000072E1 File Offset: 0x000054E1
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator AltAttack()
	{
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_altAttack_TellIntro_AnimationSpeed, "Shoot_Tell_Hold", this.m_altAttack_TellHold_AnimationSpeed, this.m_altAttack_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_altAttack_AttackIntro_AnimationSpeed, this.m_altAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_altAttack_AttackHold_AnimationSpeed, this.m_altAttack_AttackHold_Delay, false);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			this.FireProjectile("PlantHomingSporeProjectileExpertVariant", 0, false, 90f, 1f, true, true, true);
		}
		else
		{
			this.FireProjectile("PlantHomingSporeProjectile", 0, false, 90f, 1f, true, true, true);
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_altAttack_Exit_AnimationSpeed, this.m_spore_Exit_Delay, true);
		yield return this.Default_Attack_Cooldown(this.m_altAttack_Exit_ForceIdle, this.m_spore_AttackCD);
		yield break;
	}

	// Token: 0x04000E31 RID: 3633
	protected float m_spore_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000E32 RID: 3634
	protected float m_spore_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000E33 RID: 3635
	protected float m_spore_Tell_Delay = 1.15f;

	// Token: 0x04000E34 RID: 3636
	protected float m_spore_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000E35 RID: 3637
	protected float m_spore_AttackIntro_Delay;

	// Token: 0x04000E36 RID: 3638
	protected float m_spore_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000E37 RID: 3639
	protected float m_spore_AttackHold_Delay;

	// Token: 0x04000E38 RID: 3640
	protected float m_spore_Exit_AnimationSpeed = 1f;

	// Token: 0x04000E39 RID: 3641
	protected float m_spore_Exit_Delay;

	// Token: 0x04000E3A RID: 3642
	protected const string SPAWN_PROJECTILE = "PlantSpawnSporeProjectile";

	// Token: 0x04000E3B RID: 3643
	protected const string FALL_PROJECTILE = "PlantFallSporeProjectile";

	// Token: 0x04000E3C RID: 3644
	protected float m_altAttack_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000E3D RID: 3645
	protected float m_altAttack_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000E3E RID: 3646
	protected float m_altAttack_Tell_Delay = 1.15f;

	// Token: 0x04000E3F RID: 3647
	protected float m_altAttack_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000E40 RID: 3648
	protected float m_altAttack_AttackIntro_Delay;

	// Token: 0x04000E41 RID: 3649
	protected float m_altAttack_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000E42 RID: 3650
	protected float m_altAttack_AttackHold_Delay;

	// Token: 0x04000E43 RID: 3651
	protected float m_altAttack_Exit_AnimationSpeed = 1f;

	// Token: 0x04000E44 RID: 3652
	protected float m_altAttack_Exit_Delay;

	// Token: 0x04000E45 RID: 3653
	protected const string HOMING_PROJECTILE = "PlantHomingSporeProjectile";

	// Token: 0x04000E46 RID: 3654
	protected const string BOLT_PROJECTILE = "PlantBoltProjectile";

	// Token: 0x04000E47 RID: 3655
	protected const string HOMING_PROJECTILE_EXPERT = "PlantHomingSporeProjectileExpertVariant";
}
