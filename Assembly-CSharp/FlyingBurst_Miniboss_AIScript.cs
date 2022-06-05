using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class FlyingBurst_Miniboss_AIScript : FlyingBurst_Basic_AIScript
{
	// Token: 0x17000320 RID: 800
	// (get) Token: 0x0600063C RID: 1596 RVA: 0x0001904A File Offset: 0x0001724A
	protected override int ShootPatternTotalLoops
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x0600063D RID: 1597 RVA: 0x0001904D File Offset: 0x0001724D
	protected override float ShootPatternTotalLoopsDelay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x0600063E RID: 1598 RVA: 0x00019054 File Offset: 0x00017254
	protected override float MeleeFireSpread
	{
		get
		{
			return 150f;
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x0600063F RID: 1599 RVA: 0x0001905B File Offset: 0x0001725B
	protected override float BasicFireSpread
	{
		get
		{
			return 40f;
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06000640 RID: 1600 RVA: 0x00019062 File Offset: 0x00017262
	protected override Vector2 FireballRandSpeedMod
	{
		get
		{
			return new Vector2(0.25f, 1.5f);
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06000641 RID: 1601 RVA: 0x00019073 File Offset: 0x00017273
	protected override int NumFireballsMelee
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06000642 RID: 1602 RVA: 0x00019076 File Offset: 0x00017276
	protected override int NumFireballsBasic
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06000643 RID: 1603 RVA: 0x00019079 File Offset: 0x00017279
	protected virtual int m_huntAttack_NumFireballsBasic
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x06000644 RID: 1604 RVA: 0x0001907C File Offset: 0x0001727C
	protected virtual float m_huntAttack_BasicFireSpread
	{
		get
		{
			return 50f;
		}
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x00019083 File Offset: 0x00017283
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

	// Token: 0x06000646 RID: 1606 RVA: 0x00019092 File Offset: 0x00017292
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

	// Token: 0x04000A78 RID: 2680
	protected float m_huntAttack_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000A79 RID: 2681
	protected float m_huntAttack_TellHold_AnimationSpeed = 2f;

	// Token: 0x04000A7A RID: 2682
	protected float m_huntAttack_Tell_Delay = 1.15f;

	// Token: 0x04000A7B RID: 2683
	protected float m_huntAttack_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000A7C RID: 2684
	protected float m_huntAttack_AttackIntro_Delay;

	// Token: 0x04000A7D RID: 2685
	protected float m_huntAttack_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000A7E RID: 2686
	protected float m_huntAttack_AttackHold_Delay = 0.4f;

	// Token: 0x04000A7F RID: 2687
	protected float m_huntAttack_Exit_AnimationSpeed = 1f;

	// Token: 0x04000A80 RID: 2688
	protected float m_huntAttack_Exit_Delay;

	// Token: 0x04000A81 RID: 2689
	protected float m_huntAttack_Exit_ForceIdle = 0.15f;

	// Token: 0x04000A82 RID: 2690
	protected float m_huntAttack_Exit_AttackCD;

	// Token: 0x04000A83 RID: 2691
	protected float m_chaseAttack_TellHold_Delay = 1.4f;

	// Token: 0x04000A84 RID: 2692
	protected float m_chaseAttack_AttackHold_Delay = 0.4f;

	// Token: 0x04000A85 RID: 2693
	protected float m_chaseAttack_Exit_AnimationSpeed = 1f;

	// Token: 0x04000A86 RID: 2694
	protected float m_chaseAttack_Exit_Delay;

	// Token: 0x04000A87 RID: 2695
	protected float m_chaseAttack_Exit_ForceIdle = 0.15f;

	// Token: 0x04000A88 RID: 2696
	protected float m_chaseAttack_Exit_AttackCD;
}
