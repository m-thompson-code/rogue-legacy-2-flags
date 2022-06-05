using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class Plant_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600080B RID: 2059 RVA: 0x0001BD2C File Offset: 0x00019F2C
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

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x0600080C RID: 2060 RVA: 0x0001BD62 File Offset: 0x00019F62
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0001BD73 File Offset: 0x00019F73
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.LockFlip = true;
	}

	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x0600080E RID: 2062 RVA: 0x0001BD9E File Offset: 0x00019F9E
	protected virtual Vector2 m_numSporesFired
	{
		get
		{
			return new Vector2(4f, 7f);
		}
	}

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x0600080F RID: 2063 RVA: 0x0001BDAF File Offset: 0x00019FAF
	protected virtual Vector2 m_delayBetweenSporesFired
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06000810 RID: 2064 RVA: 0x0001BDC0 File Offset: 0x00019FC0
	protected virtual float m_spore_Exit_ForceIdle
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06000811 RID: 2065 RVA: 0x0001BDC7 File Offset: 0x00019FC7
	protected virtual float m_spore_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x0001BDCE File Offset: 0x00019FCE
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

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06000813 RID: 2067 RVA: 0x0001BDDD File Offset: 0x00019FDD
	protected virtual float m_altAttack_Exit_ForceIdle
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06000814 RID: 2068 RVA: 0x0001BDE4 File Offset: 0x00019FE4
	protected virtual float m_altAttack_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x0001BDEB File Offset: 0x00019FEB
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

	// Token: 0x04000B67 RID: 2919
	protected float m_spore_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000B68 RID: 2920
	protected float m_spore_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000B69 RID: 2921
	protected float m_spore_Tell_Delay = 1.15f;

	// Token: 0x04000B6A RID: 2922
	protected float m_spore_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000B6B RID: 2923
	protected float m_spore_AttackIntro_Delay;

	// Token: 0x04000B6C RID: 2924
	protected float m_spore_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000B6D RID: 2925
	protected float m_spore_AttackHold_Delay;

	// Token: 0x04000B6E RID: 2926
	protected float m_spore_Exit_AnimationSpeed = 1f;

	// Token: 0x04000B6F RID: 2927
	protected float m_spore_Exit_Delay;

	// Token: 0x04000B70 RID: 2928
	protected const string SPAWN_PROJECTILE = "PlantSpawnSporeProjectile";

	// Token: 0x04000B71 RID: 2929
	protected const string FALL_PROJECTILE = "PlantFallSporeProjectile";

	// Token: 0x04000B72 RID: 2930
	protected float m_altAttack_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000B73 RID: 2931
	protected float m_altAttack_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000B74 RID: 2932
	protected float m_altAttack_Tell_Delay = 1.15f;

	// Token: 0x04000B75 RID: 2933
	protected float m_altAttack_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000B76 RID: 2934
	protected float m_altAttack_AttackIntro_Delay;

	// Token: 0x04000B77 RID: 2935
	protected float m_altAttack_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000B78 RID: 2936
	protected float m_altAttack_AttackHold_Delay;

	// Token: 0x04000B79 RID: 2937
	protected float m_altAttack_Exit_AnimationSpeed = 1f;

	// Token: 0x04000B7A RID: 2938
	protected float m_altAttack_Exit_Delay;

	// Token: 0x04000B7B RID: 2939
	protected const string HOMING_PROJECTILE = "PlantHomingSporeProjectile";

	// Token: 0x04000B7C RID: 2940
	protected const string BOLT_PROJECTILE = "PlantBoltProjectile";

	// Token: 0x04000B7D RID: 2941
	protected const string HOMING_PROJECTILE_EXPERT = "PlantHomingSporeProjectileExpertVariant";
}
