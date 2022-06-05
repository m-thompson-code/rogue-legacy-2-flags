using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000159 RID: 345
public class FlyingBurst_Miniboss_AIScript : FlyingBurst_Basic_AIScript
{
	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x0600091D RID: 2333 RVA: 0x000047A7 File Offset: 0x000029A7
	protected override int ShootPatternTotalLoops
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x0600091E RID: 2334 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ShootPatternTotalLoopsDelay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x0600091F RID: 2335 RVA: 0x000063F0 File Offset: 0x000045F0
	protected override float MeleeFireSpread
	{
		get
		{
			return 150f;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x06000920 RID: 2336 RVA: 0x00005320 File Offset: 0x00003520
	protected override float BasicFireSpread
	{
		get
		{
			return 40f;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x06000921 RID: 2337 RVA: 0x000063F7 File Offset: 0x000045F7
	protected override Vector2 FireballRandSpeedMod
	{
		get
		{
			return new Vector2(0.25f, 1.5f);
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x06000922 RID: 2338 RVA: 0x00004A07 File Offset: 0x00002C07
	protected override int NumFireballsMelee
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06000923 RID: 2339 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int NumFireballsBasic
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06000924 RID: 2340 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_huntAttack_NumFireballsBasic
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06000925 RID: 2341 RVA: 0x000063AC File Offset: 0x000045AC
	protected virtual float m_huntAttack_BasicFireSpread
	{
		get
		{
			return 50f;
		}
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00006408 File Offset: 0x00004608
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public IEnumerator HuntAttack()
	{
		base.EnemyController.AlwaysFacing = false;
		Vector3 absoluteSpawnPositionAtIndex = base.GetAbsoluteSpawnPositionAtIndex(3, false);
		float startingAngle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - absoluteSpawnPositionAtIndex);
		int angleAdder = 45;
		AudioManager.PlayOneShotAttached(null, "event:/SFX/Enemies/Sfx_enemy_flyingBurst_multishot_charge", base.EnemyController.gameObject);
		EffectManager.PlayEffect(base.EnemyController.gameObject, null, "FlyingBurstLanternChargeUp_Effect", absoluteSpawnPositionAtIndex, this.m_huntAttack_Tell_Delay, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.SetParent(base.EnemyController.Pivot.transform, true);
		yield return base.Wait(this.m_huntAttack_Tell_Delay, false);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/vo_flyingBurst_agro", base.EnemyController.Midpoint);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/sfx_enemy_flyingBurstBoss_multishot_shoot", base.EnemyController.Midpoint);
		absoluteSpawnPositionAtIndex = base.GetAbsoluteSpawnPositionAtIndex(3, false);
		EffectManager.PlayEffect(base.EnemyController.gameObject, null, "FlyingBurstLanternRelease_Effect", absoluteSpawnPositionAtIndex, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.FireProjectile("FlyingBurstBlueBoltMinibossProjectile", 3, false, startingAngle + 0f, 1f, true, true, true);
		this.FireProjectile("FlyingBurstBlueBoltMinibossProjectile", 3, false, startingAngle + (float)angleAdder, 1f, true, true, true);
		this.FireProjectile("FlyingBurstBlueBoltMinibossProjectile", 3, false, startingAngle + (float)(angleAdder * 2), 1f, true, true, true);
		this.FireProjectile("FlyingBurstBlueBoltMinibossProjectile", 3, false, startingAngle + (float)(angleAdder * 3), 1f, true, true, true);
		this.FireProjectile("FlyingBurstBlueBoltMinibossProjectile", 3, false, startingAngle + (float)(angleAdder * 4), 1f, true, true, true);
		this.FireProjectile("FlyingBurstBlueBoltMinibossProjectile", 3, false, startingAngle + (float)(angleAdder * 5), 1f, true, true, true);
		this.FireProjectile("FlyingBurstBlueBoltMinibossProjectile", 3, false, startingAngle + (float)(angleAdder * 6), 1f, true, true, true);
		this.FireProjectile("FlyingBurstBlueBoltMinibossProjectile", 3, false, startingAngle + (float)(angleAdder * 7), 1f, true, true, true);
		if (this.m_huntAttack_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_huntAttack_AttackHold_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_huntAttack_Exit_ForceIdle, this.m_huntAttack_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x00006417 File Offset: 0x00004617
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public IEnumerator ChaserAttack()
	{
		base.EnemyController.AlwaysFacing = false;
		Vector3 absoluteSpawnPositionAtIndex = base.GetAbsoluteSpawnPositionAtIndex(3, false);
		CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - absoluteSpawnPositionAtIndex);
		int angleAdder = 72;
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Sfx_enemy_flyingBurst_multishot_charge", base.EnemyController.Midpoint);
		EffectManager.PlayEffect(base.EnemyController.gameObject, null, "FlyingBurstCurseChargeUp_Effect", absoluteSpawnPositionAtIndex, this.m_chaseAttack_TellHold_Delay, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.SetParent(base.EnemyController.Pivot.transform, true);
		yield return base.Wait(this.m_chaseAttack_TellHold_Delay, false);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/vo_flyingBurst_agro", base.EnemyController.Midpoint);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/sfx_enemy_flyingBurst_multishot_shoot", base.EnemyController.Midpoint);
		absoluteSpawnPositionAtIndex = base.GetAbsoluteSpawnPositionAtIndex(3, false);
		EffectManager.PlayEffect(base.EnemyController.gameObject, null, "EnemyCurseProjectileSpawn_Effect", absoluteSpawnPositionAtIndex, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.FireProjectile("FlyingBurstSlashBoltMinibossProjectile", 3, false, 0f, 1f, true, true, true);
		this.FireProjectile("FlyingBurstSlashBoltMinibossProjectile", 3, false, (float)angleAdder, 1f, true, true, true);
		this.FireProjectile("FlyingBurstSlashBoltMinibossProjectile", 3, false, (float)(angleAdder * 2), 1f, true, true, true);
		this.FireProjectile("FlyingBurstSlashBoltMinibossProjectile", 3, false, (float)(angleAdder * 3), 1f, true, true, true);
		this.FireProjectile("FlyingBurstSlashBoltMinibossProjectile", 3, false, (float)(angleAdder * 4), 1f, true, true, true);
		if (this.m_chaseAttack_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_chaseAttack_AttackHold_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_chaseAttack_Exit_ForceIdle, this.m_chaseAttack_Exit_AttackCD);
		yield break;
	}

	// Token: 0x04000CCE RID: 3278
	protected float m_huntAttack_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000CCF RID: 3279
	protected float m_huntAttack_TellHold_AnimationSpeed = 2f;

	// Token: 0x04000CD0 RID: 3280
	protected float m_huntAttack_Tell_Delay = 1.15f;

	// Token: 0x04000CD1 RID: 3281
	protected float m_huntAttack_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000CD2 RID: 3282
	protected float m_huntAttack_AttackIntro_Delay;

	// Token: 0x04000CD3 RID: 3283
	protected float m_huntAttack_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000CD4 RID: 3284
	protected float m_huntAttack_AttackHold_Delay = 0.4f;

	// Token: 0x04000CD5 RID: 3285
	protected float m_huntAttack_Exit_AnimationSpeed = 1f;

	// Token: 0x04000CD6 RID: 3286
	protected float m_huntAttack_Exit_Delay;

	// Token: 0x04000CD7 RID: 3287
	protected float m_huntAttack_Exit_ForceIdle = 0.15f;

	// Token: 0x04000CD8 RID: 3288
	protected float m_huntAttack_Exit_AttackCD;

	// Token: 0x04000CD9 RID: 3289
	protected float m_chaseAttack_TellHold_Delay = 1.4f;

	// Token: 0x04000CDA RID: 3290
	protected float m_chaseAttack_AttackHold_Delay = 0.4f;

	// Token: 0x04000CDB RID: 3291
	protected float m_chaseAttack_Exit_AnimationSpeed = 1f;

	// Token: 0x04000CDC RID: 3292
	protected float m_chaseAttack_Exit_Delay;

	// Token: 0x04000CDD RID: 3293
	protected float m_chaseAttack_Exit_ForceIdle = 0.15f;

	// Token: 0x04000CDE RID: 3294
	protected float m_chaseAttack_Exit_AttackCD;
}
