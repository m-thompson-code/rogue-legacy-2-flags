using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class Skeleton_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000CE2 RID: 3298 RVA: 0x0006ECB4 File Offset: 0x0006CEB4
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SkeletonBoneProjectile",
			"SkeletonBoneMinibossSmallProjectile",
			"SkeletonRibProjectile",
			"SkeletonBoneBigSmallProjectile",
			"SkeletonRibMinibossProjectile",
			"SkeletonBossCurseProjectile",
			"SkeletonCannonBallWarningProjectile",
			"SkeletonCannonBallProjectile"
		};
	}

	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_jumpLandingMakesBones
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_jump_tweak_X
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x000078BB File Offset: 0x00005ABB
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Throw_Bone_Near_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Single_Attack_Throw_Bone(this.m_BoneNear_Angle);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_throwBone_Exit_ForceIdle, this.m_throwBone_AttackCD);
		yield break;
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x000078CA File Offset: 0x00005ACA
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	public IEnumerator Throw_Bone_Far_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Single_Attack_Throw_Bone(this.m_FarBoneAngle);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_throwBone_Exit_ForceIdle, this.m_throwBone_AttackCD);
		yield break;
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x000078D9 File Offset: 0x00005AD9
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Throw_Rib_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Single_Attack_Throw_Rib();
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_throwRib_Exit_ForceIdle, this.m_throwRib_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x000078E8 File Offset: 0x00005AE8
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Jump_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		if (base.EnemyController.EnemyRank == EnemyRank.Miniboss)
		{
			base.EnemyController.DisableOffscreenWarnings = false;
			ProjectileManager.AttachOffscreenIcon(base.EnemyController, true);
		}
		yield return this.Default_Animation("Jump_Tell", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Single_Action_Jump();
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.SetVelocityX(0f, false);
		if (this.m_jumpLandingMakesBones)
		{
			for (int i = 0; i < 2; i++)
			{
				float speedMod = UnityEngine.Random.Range(this.m_miniBoss_Land_ThrowPower.x, this.m_miniBoss_Land_ThrowPower.y);
				int num = UnityEngine.Random.Range((int)this.m_miniBoss_Land_ThrowAngle.x, (int)this.m_miniBoss_Land_ThrowAngle.y);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 1, true, (float)num, speedMod, true, true, true);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 1, true, (float)(180 - num), speedMod, true, true, true);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 2, true, (float)num, speedMod, true, true, true);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 2, true, (float)(180 - num), speedMod, true, true, true);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 2, true, (float)num, speedMod, true, true, true);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 2, true, (float)(180 - num), speedMod, true, true, true);
			}
		}
		base.EnemyController.DisableOffscreenWarnings = true;
		if (this.m_jump_Exit_Delay > 0f)
		{
			yield return base.Wait(this.m_jump_Exit_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_jump_Exit_ForceIdle, this.m_jump_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x000078F7 File Offset: 0x00005AF7
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[AdvancedEnemy]
	[ExpertEnemy]
	public IEnumerator Jump_And_Throw_Bone_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Jump_Tell", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		yield return this.Single_Action_Jump();
		float num = UnityEngine.Random.Range(this.m_jumpAndThrowBone_ProjectileSpawnDelay.x, this.m_jumpAndThrowBone_ProjectileSpawnDelay.y);
		if (num > 0f)
		{
			yield return base.Wait(num, false);
		}
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			yield return this.Single_Attack_Throw_Bone(this.m_BoneNear_Angle);
		}
		else
		{
			yield return this.Single_Attack_Throw_Bone(this.m_FarBoneAngle);
		}
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		if (this.m_jump_Exit_Delay > 0f)
		{
			yield return base.Wait(this.m_jump_Exit_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_jumpAndThrowBone_Exit_ForceIdle, this.m_jumpAndThrowBone_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x00007906 File Offset: 0x00005B06
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[AdvancedEnemy]
	[ExpertEnemy]
	public IEnumerator Jump_And_Throw_Rib_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Jump_Tell", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		yield return this.Single_Action_Jump();
		float num = UnityEngine.Random.Range(this.m_jumpAndThrowBone_ProjectileSpawnDelay.x, this.m_jumpAndThrowBone_ProjectileSpawnDelay.y);
		if (num > 0f)
		{
			yield return base.Wait(num, false);
		}
		yield return this.Single_Attack_Throw_Rib();
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		if (this.m_jump_Exit_Delay > 0f)
		{
			yield return base.Wait(this.m_jump_Exit_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_jumpAndThrowBone_Exit_ForceIdle, this.m_jumpAndThrowBone_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x00007915 File Offset: 0x00005B15
	public IEnumerator Single_Attack_Throw_Bone(int throwAngle)
	{
		yield return this.Default_TellIntroAndLoop("ThrowBone_Tell_Intro", this.m_throwBone_TellIntro_AnimationSpeed, "ThrowBone_Tell_Hold", this.m_throwBone_TellHold_AnimationSpeed, 0.5f);
		yield return this.Default_Animation("ThrowBone_Attack_Intro", this.m_throwBone_AttackIntro_AnimationSpeed, this.m_throwBone_AttackIntro_Delay, true);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			for (int i = 0; i < this.m_miniBoss_Bone_ThrowAmount; i++)
			{
				int num = 5 + i;
				if (num >= 9)
				{
					num = 5;
				}
				throwAngle = UnityEngine.Random.Range((int)this.m_miniBoss_Bone_ThrowAngle.x, (int)this.m_miniBoss_Bone_ThrowAngle.y);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", num, true, (float)throwAngle, 1f, true, true, true);
			}
		}
		else if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			this.FireProjectile("SkeletonBoneProjectile", 0, true, (float)this.m_FarBoneAngle, 1f, true, true, true);
			this.FireProjectile("SkeletonBoneProjectile", 0, true, (float)this.m_BoneNear_Angle, 1f, true, true, true);
			yield return base.Wait(0.1f, false);
			this.FireProjectile("SkeletonBoneProjectile", 0, true, (float)this.m_FarBoneAngle, 1f, true, true, true);
			this.FireProjectile("SkeletonBoneProjectile", 0, true, (float)this.m_BoneNear_Angle, 1f, true, true, true);
		}
		else
		{
			this.FireProjectile("SkeletonBoneProjectile", 0, true, (float)throwAngle, 1f, true, true, true);
		}
		yield return this.Default_Animation("ThrowBone_Attack_Hold", this.m_throwBone_AttackHold_AnimationSpeed, 0f, false);
		yield return base.Wait(0.6f, false);
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("ThrowBone_Exit", this.m_throwBone_Exit_AnimationSpeed, this.m_throwBone_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield break;
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x0000792B File Offset: 0x00005B2B
	public IEnumerator Single_Attack_Throw_Rib()
	{
		yield return this.Default_TellIntroAndLoop("ChestBone_Tell_Intro", this.m_throwRib_TellIntro_AnimationSpeed, "ChestBone_Tell_Hold", this.m_throwRib_TellHold_AnimationSpeed, 0.75f);
		yield return this.Default_Animation("ChestBone_Attack_Intro", this.m_throwRib_AttackIntro_AnimationSpeed, this.m_throwRib_AttackIntro_Delay, true);
		yield return this.Default_Animation("ChestBone_Attack_Hold", this.m_throwRib_AttackHold_AnimationSpeed, 0f, false);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			int i = 0;
			while ((float)i < this.m_miniBoss_Rib_ThrowAmount)
			{
				float miniBoss_Rib_ThrowPower = this.m_miniBoss_Rib_ThrowPower;
				UnityEngine.Random.Range(this.m_miniBoss_Rib_ThrowHeight.x, this.m_miniBoss_Rib_ThrowHeight.y);
				this.FireProjectile("SkeletonBossCurseProjectile", 4, true, (float)this.m_throwRib_Angle, miniBoss_Rib_ThrowPower, true, true, true);
				if (this.m_miniBoss_Rib_LoopDelay > 0f)
				{
					yield return base.Wait(this.m_miniBoss_Rib_LoopDelay, false);
				}
				int num = i;
				i = num + 1;
			}
		}
		else
		{
			this.FireProjectile("SkeletonRibProjectile", 4, true, (float)this.m_throwRib_Angle, 1f, true, true, true);
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
			{
				this.FireProjectile("SkeletonRibProjectile", 4, true, (float)this.m_throwRib_Angle, this.m_throwRib_secondRibSpeedMod, true, true, true);
			}
		}
		yield return base.Wait(this.m_throwRib_AttackHold_Delay, false);
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("ChestBone_Exit", this.m_throwRib_Exit_AnimationSpeed, this.m_throwRib_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield break;
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0000793A File Offset: 0x00005B3A
	public IEnumerator Single_Action_Jump()
	{
		float num = this.m_jump_Power.x;
		if (this.m_jump_tweak_X)
		{
			num += UnityEngine.Random.Range(this.m_jump_Tweaker_Add.x, this.m_jump_Tweaker_Add.y);
		}
		if (!base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(num, true);
		}
		else
		{
			base.SetVelocityX(-num, true);
		}
		base.SetVelocityY(this.m_jump_Power.y, false);
		yield return this.ChangeAnimationState("JumpUp");
		yield return base.Wait(0.05f, false);
		yield break;
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x00007949 File Offset: 0x00005B49
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public IEnumerator Simple_Dance_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_Animation("Victory", this.m_dance_AttackIntro_AnimationSpeed, this.m_dance_AttackIntro_Delay, false);
		float y = base.EnemyController.Room.Bounds.max.y + 10f;
		float num = 5f;
		Vector2 vector = new Vector2(PlayerManager.GetPlayerController().Midpoint.x, y);
		Vector2 vector4;
		Vector2 vector3;
		Vector2 pos;
		Vector2 vector2 = pos = (vector3 = (vector4 = vector));
		pos.y = y;
		vector2.x += num;
		vector3.x += -num;
		vector4.x += num * 2f;
		vector.x += -num * 2f;
		AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_enemy_skeletonBoss_portal_single", PlayerManager.GetPlayerController().Midpoint);
		this.FireProjectileAbsPos("SkeletonCannonBallWarningProjectile", pos, false, 270f, 1f, true, true, true);
		Projectile_RL projectile_RL = this.FireProjectileAbsPos("SkeletonCannonBallProjectile", pos, true, -90f, 1f, true, true, true);
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_skeletonBoss_small_cannon_projectile_loop", projectile_RL.gameObject);
		yield return base.Wait(0.1f, false);
		yield return base.Wait(this.m_dance_AttackHold_Delay, false);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.AlwaysFacing = true;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Attack_Cooldown(this.m_dance_Exit_ForceIdle, this.m_dance_Exit_AttackCD);
		yield break;
	}

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float m_modeShift_Damage_Mod
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x00007958 File Offset: 0x00005B58
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Mode_Shift()
	{
		yield return base.DeathAnim();
		base.SetVelocity(0f, 0f, false);
		base.EnemyController.LockFlip = true;
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_Damage_Mod;
		yield return this.Default_Animation("ModeShift_Intro", this.m_modeShift_Tell_Intro_AnimSpeed, this.m_modeShift_TellIntro_Delay, true);
		yield return this.Default_Animation("ModeShift_Scream_Intro", this.m_modeShift_Attack_Intro_AnimSpeed, this.m_modeShift_Attack_Intro_Delay, true);
		yield return this.Default_Animation("ModeShift_Scream_Hold", this.m_modeShift_Attack_Hold_AnimSpeed, this.m_modeShift_Attack_Hold_Delay, true);
		this.ModeShiftComplete_SpawnSecondBoss = true;
		yield return this.Default_Animation("ModeShift_Scream_Exit", this.m_modeShift_Exit_AnimSpeed, this.m_modeShift_Exit_Delay, true);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.LockFlip = false;
		yield break;
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x00007967 File Offset: 0x00005B67
	public override IEnumerator SpawnAnim()
	{
		base.EnemyController.Visuals.SetActive(false);
		yield return base.Wait(1.5f, false);
		yield return this.ChangeAnimationState("Fall");
		Vector3 position = base.EnemyController.transform.position;
		position.y += 18f;
		base.EnemyController.transform.position = position;
		base.EnemyController.Visuals.SetActive(true);
		yield return base.WaitUntilIsGrounded();
		yield return base.Wait(0.5f, false);
		yield return this.Default_Animation("Intro_Idle", this.m_spawn_Idle_AnimSpeed, this.m_spawn_Idle_Delay, true);
		yield return this.Default_Animation("Intro", this.m_spawn_Intro_AnimSpeed, this.m_spawn_Intro_Delay, true);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		yield break;
	}

	// Token: 0x06000CF5 RID: 3317 RVA: 0x00007976 File Offset: 0x00005B76
	public override IEnumerator DeathAnim()
	{
		BossRoomController component = base.EnemyController.Room.gameObject.GetComponent<BossRoomController>();
		bool flag;
		if (component)
		{
			flag = component.AllBossesDeadOrDying;
		}
		else
		{
			flag = (EnemyManager.NumActiveEnemies == 0 && EnemyManager.NumActiveSummonedEnemies == 0);
		}
		if (flag)
		{
			yield return base.DeathAnim();
		}
		yield break;
	}

	// Token: 0x06000CF6 RID: 3318 RVA: 0x00007985 File Offset: 0x00005B85
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.ModeShiftComplete_SpawnSecondBoss = false;
		if (enemyController.IsBoss)
		{
			base.LogicController.DisableLogicActivationByDistance = true;
		}
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x00007768 File Offset: 0x00005968
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.DisableOffscreenWarnings = true;
		base.EnemyController.AlwaysFacing = true;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x04000F36 RID: 3894
	public UnityEvent_GameObject JumpEvent;

	// Token: 0x04000F37 RID: 3895
	protected float m_throwBone_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000F38 RID: 3896
	protected float m_throwBone_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000F39 RID: 3897
	protected const float m_throwBone_TellIntroAndHold_Delay = 0.5f;

	// Token: 0x04000F3A RID: 3898
	protected float m_throwBone_AttackIntro_AnimationSpeed = 0.8f;

	// Token: 0x04000F3B RID: 3899
	protected float m_throwBone_AttackIntro_Delay;

	// Token: 0x04000F3C RID: 3900
	protected float m_throwBone_AttackHold_AnimationSpeed = 0.8f;

	// Token: 0x04000F3D RID: 3901
	protected const float m_throwBone_AttackHold_Delay = 0.6f;

	// Token: 0x04000F3E RID: 3902
	protected float m_throwBone_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000F3F RID: 3903
	protected float m_throwBone_Exit_Delay = 0.15f;

	// Token: 0x04000F40 RID: 3904
	protected int m_BoneNear_Angle = 83;

	// Token: 0x04000F41 RID: 3905
	protected int m_FarBoneAngle = 70;

	// Token: 0x04000F42 RID: 3906
	protected int m_miniBoss_Bone_ThrowAmount = 8;

	// Token: 0x04000F43 RID: 3907
	protected Vector2 m_miniBoss_Bone_ThrowAngle = new Vector2(55f, 85f);

	// Token: 0x04000F44 RID: 3908
	protected float m_throwBone_Exit_ForceIdle = 0.15f;

	// Token: 0x04000F45 RID: 3909
	protected float m_throwBone_AttackCD;

	// Token: 0x04000F46 RID: 3910
	protected float m_throwRib_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000F47 RID: 3911
	protected float m_throwRib_TellHold_AnimationSpeed = 1.35f;

	// Token: 0x04000F48 RID: 3912
	protected const float m_throwRib_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04000F49 RID: 3913
	protected float m_throwRib_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000F4A RID: 3914
	protected float m_throwRib_AttackIntro_Delay;

	// Token: 0x04000F4B RID: 3915
	protected float m_throwRib_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x04000F4C RID: 3916
	protected float m_throwRib_AttackHold_Delay = 1f;

	// Token: 0x04000F4D RID: 3917
	protected float m_throwRib_Exit_AnimationSpeed = 1f;

	// Token: 0x04000F4E RID: 3918
	protected float m_throwRib_Exit_Delay;

	// Token: 0x04000F4F RID: 3919
	protected int m_throwRib_Angle;

	// Token: 0x04000F50 RID: 3920
	protected float m_throwRib_secondRibSpeedMod = 1.35f;

	// Token: 0x04000F51 RID: 3921
	protected float m_throwRib_Exit_ForceIdle = 0.15f;

	// Token: 0x04000F52 RID: 3922
	protected float m_throwRib_Exit_AttackCD = 2f;

	// Token: 0x04000F53 RID: 3923
	protected float m_miniBoss_Rib_ThrowAmount = 1f;

	// Token: 0x04000F54 RID: 3924
	protected float m_miniBoss_Rib_ThrowPower = 1f;

	// Token: 0x04000F55 RID: 3925
	protected Vector2 m_miniBoss_Rib_ThrowHeight = new Vector2(6f, 6f);

	// Token: 0x04000F56 RID: 3926
	protected float m_miniBoss_Rib_LoopDelay = 0.9f;

	// Token: 0x04000F57 RID: 3927
	protected float m_jump_Tell_Delay = 0.4f;

	// Token: 0x04000F58 RID: 3928
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000F59 RID: 3929
	protected Vector2 m_jump_Power = new Vector2(0f, 27f);

	// Token: 0x04000F5A RID: 3930
	protected int m_miniBoss_Bone_JumpAmount = 5;

	// Token: 0x04000F5B RID: 3931
	protected float m_jump_Exit_Delay;

	// Token: 0x04000F5C RID: 3932
	protected float m_jump_Exit_ForceIdle = 0.15f;

	// Token: 0x04000F5D RID: 3933
	protected float m_jump_Exit_AttackCD;

	// Token: 0x04000F5E RID: 3934
	protected Vector2 m_jumpAndThrowBone_ProjectileSpawnDelay = new Vector2(0f, 0f);

	// Token: 0x04000F5F RID: 3935
	protected float m_jumpAndThrowBone_Exit_ForceIdle = 0.15f;

	// Token: 0x04000F60 RID: 3936
	protected float m_jumpAndThrowBone_Exit_AttackCD = 1.25f;

	// Token: 0x04000F61 RID: 3937
	protected Vector2 m_jump_Tweaker_Add = new Vector2(-12f, 0f);

	// Token: 0x04000F62 RID: 3938
	protected Vector2 m_miniBoss_Land_ThrowAngle = new Vector2(25f, 30f);

	// Token: 0x04000F63 RID: 3939
	protected Vector2 m_miniBoss_Land_ThrowPower = new Vector2(0.75f, 1f);

	// Token: 0x04000F64 RID: 3940
	protected float m_dance_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000F65 RID: 3941
	protected float m_dance_AttackIntro_Delay = 1f;

	// Token: 0x04000F66 RID: 3942
	protected float m_dance_AttackHold_Delay = 1f;

	// Token: 0x04000F67 RID: 3943
	protected float m_dance_Exit_ForceIdle = 1f;

	// Token: 0x04000F68 RID: 3944
	protected float m_dance_Exit_AttackCD = 5f;

	// Token: 0x04000F69 RID: 3945
	protected const string MODE_SHIFT_TELL_INTRO = "ModeShift_Intro";

	// Token: 0x04000F6A RID: 3946
	protected const string MODE_SHIFT_ATTACK_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000F6B RID: 3947
	protected const string MODE_SHIFT_ATTACK_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x04000F6C RID: 3948
	protected const string MODE_SHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000F6D RID: 3949
	protected float m_modeShift_Tell_Intro_AnimSpeed = 1f;

	// Token: 0x04000F6E RID: 3950
	protected float m_modeShift_TellIntro_Delay = 0.5f;

	// Token: 0x04000F6F RID: 3951
	protected float m_modeShift_Attack_Intro_AnimSpeed = 1f;

	// Token: 0x04000F70 RID: 3952
	protected float m_modeShift_Attack_Intro_Delay;

	// Token: 0x04000F71 RID: 3953
	protected float m_modeShift_Attack_Hold_AnimSpeed = 1f;

	// Token: 0x04000F72 RID: 3954
	protected float m_modeShift_Attack_Hold_Delay = 2f;

	// Token: 0x04000F73 RID: 3955
	protected float m_modeShift_Exit_AnimSpeed = 1f;

	// Token: 0x04000F74 RID: 3956
	protected float m_modeShift_Exit_Delay;

	// Token: 0x04000F75 RID: 3957
	[NonSerialized]
	public bool ModeShiftComplete_SpawnSecondBoss;

	// Token: 0x04000F76 RID: 3958
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x04000F77 RID: 3959
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x04000F78 RID: 3960
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04000F79 RID: 3961
	protected float m_spawn_Idle_Delay;

	// Token: 0x04000F7A RID: 3962
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04000F7B RID: 3963
	protected float m_spawn_Intro_Delay;

	// Token: 0x04000F7C RID: 3964
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x04000F7D RID: 3965
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x04000F7E RID: 3966
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04000F7F RID: 3967
	protected float m_death_Intro_Delay;

	// Token: 0x04000F80 RID: 3968
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04000F81 RID: 3969
	protected float m_death_Hold_Delay = 4.5f;
}
