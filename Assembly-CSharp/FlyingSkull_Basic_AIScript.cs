using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class FlyingSkull_Basic_AIScript : BaseAIScript, IAudioEventEmitter
{
	// Token: 0x06000A01 RID: 2561 RVA: 0x000068E9 File Offset: 0x00004AE9
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingSkullBoneProjectile"
		};
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06000A03 RID: 2563 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06000A04 RID: 2564 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06000A05 RID: 2565 RVA: 0x00005065 File Offset: 0x00003265
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-5f, 5f);
		}
	}

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06000A06 RID: 2566 RVA: 0x00005076 File Offset: 0x00003276
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 5f);
		}
	}

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06000A07 RID: 2567 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06000A08 RID: 2568 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_shoot_TellHold_AnimationSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06000A09 RID: 2569 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06000A0A RID: 2570 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06000A0B RID: 2571 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06000A0C RID: 2572 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_shoot_AttackHold_AnimationSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06000A0D RID: 2573 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x06000A0E RID: 2574 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_Exit_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06000A0F RID: 2575 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x06000A10 RID: 2576 RVA: 0x000050CB File Offset: 0x000032CB
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06000A11 RID: 2577 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06000A12 RID: 2578 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_BeforeTellDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06000A13 RID: 2579 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_AfterAttackDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_shoot_MultiShotDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x06000A16 RID: 2582 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_shoot_ShootNear
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_shoot_ShootFar
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_shoot_ShootMirror
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x000068FF File Offset: 0x00004AFF
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootFireball()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AlwaysFacing = false;
		if (this.m_shoot_BeforeTellDelay > 0f)
		{
			yield return base.Wait(this.m_shoot_BeforeTellDelay, false);
		}
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_flyingSkull_shoot_prep", base.gameObject);
		yield return this.Default_TellIntroAndLoop("MultiShot_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "MultiShot_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_Tell_Delay);
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_flyingSkull_shoot", base.gameObject);
		yield return this.Default_Animation("MultiShot_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("MultiShot_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, 0f, false);
		int num;
		for (int i = 0; i < this.m_shoot_TotalShots; i = num + 1)
		{
			if (this.m_shoot_ShootNear)
			{
				this.FireProjectile("FlyingSkullBoneProjectile", 0, true, (float)(90 - this.m_NearBone_Angle), 1f, true, true, true);
				if (this.m_shoot_ShootMirror)
				{
					this.FireProjectile("FlyingSkullBoneProjectile", 0, true, (float)(90 + this.m_NearBone_Angle), 1f, true, true, true);
				}
			}
			if (this.m_shoot_ShootFar)
			{
				this.FireProjectile("FlyingSkullBoneProjectile", 0, true, (float)(90 - this.m_FarBoneAngle), 1f, true, true, true);
				if (this.m_shoot_ShootMirror)
				{
					this.FireProjectile("FlyingSkullBoneProjectile", 0, true, (float)(90 + this.m_FarBoneAngle), 1f, true, true, true);
				}
			}
			if (this.m_shoot_MultiShotDelay > 0f)
			{
				yield return base.Wait(this.m_shoot_MultiShotDelay, false);
			}
			num = i;
		}
		if (this.m_shoot_AfterAttackDelay > 0f)
		{
			yield return base.Wait(this.m_shoot_AfterAttackDelay, false);
		}
		yield return this.Default_Animation("MultiShot_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x06000A1A RID: 2586 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06000A1C RID: 2588 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected virtual float m_dash_TellHold_AnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06000A1D RID: 2589 RVA: 0x00006780 File Offset: 0x00004980
	protected virtual float m_dash_TellHold_Delay
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06000A1E RID: 2590 RVA: 0x00004573 File Offset: 0x00002773
	protected virtual float m_dash_AttackIntro_AnimSpeed
	{
		get
		{
			return 2.75f;
		}
	}

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06000A1F RID: 2591 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06000A20 RID: 2592 RVA: 0x00004573 File Offset: 0x00002773
	protected virtual float m_dash_AttackHold_AnimSpeed
	{
		get
		{
			return 2.75f;
		}
	}

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06000A21 RID: 2593 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06000A22 RID: 2594 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x06000A23 RID: 2595 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x06000A24 RID: 2596 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x06000A25 RID: 2597 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_Exit_AttackCD
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06000A26 RID: 2598 RVA: 0x00003DA4 File Offset: 0x00001FA4
	protected virtual float m_dash_Attack_Speed
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06000A27 RID: 2599 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_Tell_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06000A28 RID: 2600 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06000A29 RID: 2601 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_dropsBonesDuringDashAttack
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06000A2A RID: 2602 RVA: 0x000047A7 File Offset: 0x000029A7
	protected virtual int m_bonesDroppedDuringDashCount
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0000690E File Offset: 0x00004B0E
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		this.ToDo("DASH!");
		this.StopAndFaceTarget();
		base.EnemyController.BaseTurnSpeed = 0f;
		base.EnemyController.FollowTarget = false;
		base.EnemyController.ForceFaceTarget();
		base.EnemyController.LockFlip = true;
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_flyingSkull_dash_prep", base.gameObject);
		yield return this.Default_TellIntroAndLoop("SingleShot_Tell_Intro", this.m_dash_TellIntro_AnimSpeed, "SingleShot_Tell_Hold", this.m_dash_TellHold_AnimSpeed, this.m_dash_Tell_Duration);
		this.LeanIntoDash();
		this.SetDashVelocity();
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_flyingSkull_dash", base.gameObject);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 0f, true);
		if (this.m_dropsBonesDuringDashAttack)
		{
			yield return this.DropBonesDuringDash();
		}
		else if (this.m_dash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_Duration, false);
		}
		base.EnemyController.Pivot.transform.localRotation = Quaternion.identity;
		base.EnemyController.BaseTurnSpeed = 0f;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		if (this.m_dash_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_dash_AttackHold_Delay, false);
		}
		base.EnemyController.BaseSpeed = base.EnemyController.EnemyData.Speed;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_Exit_AttackCD);
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x00067988 File Offset: 0x00065B88
	private void SetDashVelocity()
	{
		Vector3 v = Vector2.left;
		if (base.EnemyController.IsFacingRight)
		{
			v = Vector2.right;
		}
		base.EnemyController.Heading = v;
		base.EnemyController.BaseSpeed = this.m_dash_Attack_Speed;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x000679E8 File Offset: 0x00065BE8
	private void LeanIntoDash()
	{
		float z = 20f;
		if (base.EnemyController.IsFacingRight)
		{
			z = -20f;
		}
		base.EnemyController.Pivot.transform.localRotation = Quaternion.Euler(0f, 0f, z);
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x0000691D File Offset: 0x00004B1D
	private IEnumerator DropBonesDuringDash()
	{
		float timeBetweenDrops = this.m_dash_Attack_Duration / (float)this.m_bonesDroppedDuringDashCount;
		float angle = 80f;
		if (base.EnemyController.IsFacingRight)
		{
			angle = 110f;
		}
		float elapsedDashTime = 0f;
		int num;
		for (int i = 0; i < this.m_bonesDroppedDuringDashCount; i = num + 1)
		{
			this.FireProjectile("FlyingSkullBoneProjectile", 0, false, angle, 1f, true, true, true);
			yield return base.Wait(timeBetweenDrops, false);
			elapsedDashTime += timeBetweenDrops;
			num = i;
		}
		float num2 = this.m_dash_Attack_Duration - elapsedDashTime;
		if (num2 > 0f)
		{
			yield return base.Wait(num2, false);
		}
		yield break;
	}

	// Token: 0x04000D59 RID: 3417
	private const string SHOOT_PREP_AUDIO_EVENT_PATH = "event:/SFX/Enemies/sfx_enemy_flyingSkull_shoot_prep";

	// Token: 0x04000D5A RID: 3418
	private const string SHOOT_AUDIO_EVENT_PATH = "event:/SFX/Enemies/sfx_enemy_flyingSkull_shoot";

	// Token: 0x04000D5B RID: 3419
	private const string DASH_PREP_AUDIO_EVENT_PATH = "event:/SFX/Enemies/sfx_enemy_flyingSkull_dash_prep";

	// Token: 0x04000D5C RID: 3420
	private const string DASH_AUDIO_EVENT_PATH = "event:/SFX/Enemies/sfx_enemy_flyingSkull_dash";

	// Token: 0x04000D5D RID: 3421
	protected int m_NearBone_Angle = 7;

	// Token: 0x04000D5E RID: 3422
	protected int m_FarBoneAngle = 20;

	// Token: 0x04000D5F RID: 3423
	private const string BONE_PROJECTILE = "FlyingSkullBoneProjectile";

	// Token: 0x04000D60 RID: 3424
	protected const string SINGLE_TELL_INTRO = "SingleShot_Tell_Intro";

	// Token: 0x04000D61 RID: 3425
	protected const string SINGLE_TELL_HOLD = "SingleShot_Tell_Hold";

	// Token: 0x04000D62 RID: 3426
	protected const string SINGLE_ATTACK_INTRO = "SingleShot_Attack_Intro";

	// Token: 0x04000D63 RID: 3427
	protected const string SINGLE_ATTACK_HOLD = "SingleShot_Attack_Hold";

	// Token: 0x04000D64 RID: 3428
	protected const string SINGLE_ATTACK_EXIT = "SingleShot_Exit";

	// Token: 0x04000D65 RID: 3429
	protected const string MULTI_TELL_INTRO = "MultiShot_Tell_Intro";

	// Token: 0x04000D66 RID: 3430
	protected const string MULTI_TELL_HOLD = "MultiShot_Tell_Hold";

	// Token: 0x04000D67 RID: 3431
	protected const string MULTI_ATTACK_INTRO = "MultiShot_Attack_Intro";

	// Token: 0x04000D68 RID: 3432
	protected const string MULTI_ATTACK_HOLD = "MultiShot_Attack_Hold";

	// Token: 0x04000D69 RID: 3433
	protected const string MULTI_ATTACK_EXIT = "MultiShot_Exit";
}
