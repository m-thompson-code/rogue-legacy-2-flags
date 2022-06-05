using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class SkeletonBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000C76 RID: 3190 RVA: 0x0006D338 File Offset: 0x0006B538
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SkeletonBoneProjectile",
			"SkeletonBoneMinibossProjectile",
			"SkeletonRibProjectile",
			"SkeletonBoneBigProjectile",
			"SkeletonBossCurseProjectile",
			"SkeletonCannonBallProjectile",
			"SkeletonCannonBallWarningProjectile"
		};
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x06000C77 RID: 3191 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x06000C7A RID: 3194 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_jump_Land_Spawn_Curse
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x00007670 File Offset: 0x00005870
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
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_throwBone_Exit_ForceIdle, this.m_throwBone_AttackCD);
		yield break;
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x0000767F File Offset: 0x0000587F
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Throw_Bone_Far_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Single_Attack_Throw_Bone(this.m_FarBoneAngle);
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_throwBone_Exit_ForceIdle, this.m_throwBone_AttackCD);
		yield break;
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x0000768E File Offset: 0x0000588E
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Throw_Rib_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Single_Attack_Throw_Rib();
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_throwRib_Exit_ForceIdle, this.m_throwRib_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x0000769D File Offset: 0x0000589D
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Dance_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_Animation("Victory", this.m_dance_AttackIntro_AnimationSpeed, this.m_dance_AttackIntro_Delay, false);
		float y = base.EnemyController.Room.Bounds.max.y + 10f;
		float num = 5f;
		Vector2 vector = new Vector2(PlayerManager.GetPlayerController().Midpoint.x, y);
		Vector2 pos4;
		Vector2 pos3;
		Vector2 pos2;
		Vector2 pos = pos2 = (pos3 = (pos4 = vector));
		pos2.y = y;
		pos.x += num;
		pos3.x += -num;
		pos4.x += num * 2f;
		vector.x += -num * 2f;
		pos.y -= 2f;
		pos3.y -= 2f;
		pos2.y -= 4f;
		AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_enemy_skeletonBoss_portal_multiple", PlayerManager.GetPlayerController().Midpoint);
		this.FireProjectileAbsPos("SkeletonCannonBallWarningProjectile", pos2, false, 270f, 1f, true, true, true);
		this.FireProjectileAbsPos("SkeletonCannonBallWarningProjectile", pos, false, 270f, 1f, true, true, true);
		this.FireProjectileAbsPos("SkeletonCannonBallWarningProjectile", pos3, false, 270f, 1f, true, true, true);
		this.FireProjectileAbsPos("SkeletonCannonBallWarningProjectile", pos4, false, 270f, 1f, true, true, true);
		this.FireProjectileAbsPos("SkeletonCannonBallWarningProjectile", vector, false, 270f, 1f, true, true, true);
		Projectile_RL projectile_RL = this.FireProjectileAbsPos("SkeletonCannonBallProjectile", pos2, true, -90f, 1f, true, true, true);
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_skeletonBoss_cannon_projectiles_loop", projectile_RL.gameObject);
		this.FireProjectileAbsPos("SkeletonCannonBallProjectile", pos, true, -90f, 1f, true, true, true);
		this.FireProjectileAbsPos("SkeletonCannonBallProjectile", pos3, true, -90f, 1f, true, true, true);
		this.FireProjectileAbsPos("SkeletonCannonBallProjectile", pos4, true, -90f, 1f, true, true, true);
		this.FireProjectileAbsPos("SkeletonCannonBallProjectile", vector, true, -90f, 1f, true, true, true);
		yield return base.Wait(this.m_dance_AttackHold_Delay, false);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.AlwaysFacing = true;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Attack_Cooldown(this.m_dance_Exit_ForceIdle, this.m_dance_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x000076AC File Offset: 0x000058AC
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Jump_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		base.EnemyController.DisableOffscreenWarnings = false;
		ProjectileManager.AttachOffscreenIcon(base.EnemyController, true);
		yield return this.Default_Animation("Jump_Tell", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Single_Action_Jump();
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.SetVelocityX(0f, false);
		for (int i = 0; i < 2; i++)
		{
			float speedMod = UnityEngine.Random.Range(this.m_miniBoss_Land_ThrowPower.x, this.m_miniBoss_Land_ThrowPower.y);
			int num = UnityEngine.Random.Range((int)this.m_miniBoss_Land_ThrowAngle.x, (int)this.m_miniBoss_Land_ThrowAngle.y);
			this.FireProjectile("SkeletonBoneMinibossProjectile", 1, true, (float)num, speedMod, true, true, true);
			this.FireProjectile("SkeletonBoneMinibossProjectile", 1, true, (float)(180 - num), speedMod, true, true, true);
			this.FireProjectile("SkeletonBoneMinibossProjectile", 2, true, (float)num, speedMod, true, true, true);
			this.FireProjectile("SkeletonBoneMinibossProjectile", 2, true, (float)(180 - num), speedMod, true, true, true);
			this.FireProjectile("SkeletonBoneMinibossProjectile", 2, true, (float)num, speedMod, true, true, true);
			this.FireProjectile("SkeletonBoneMinibossProjectile", 2, true, (float)(180 - num), speedMod, true, true, true);
		}
		if (this.m_jump_Land_Spawn_Curse)
		{
			this.FireProjectile("SkeletonBossCurseProjectile", 2, true, 65f, 1f, true, true, true);
			this.FireProjectile("SkeletonBossCurseProjectile", 2, true, 115f, 1f, true, true, true);
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

	// Token: 0x06000C80 RID: 3200 RVA: 0x000076BB File Offset: 0x000058BB
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
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

	// Token: 0x06000C81 RID: 3201 RVA: 0x000076CA File Offset: 0x000058CA
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
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

	// Token: 0x06000C82 RID: 3202 RVA: 0x000076D9 File Offset: 0x000058D9
	public IEnumerator Single_Attack_Throw_Bone(int throwAngle)
	{
		yield return this.Default_TellIntroAndLoop("ThrowBone_Tell_Intro", this.m_throwBone_TellIntro_AnimationSpeed, "ThrowBone_Tell_Hold", this.m_throwBone_TellHold_AnimationSpeed, 0.65f);
		yield return this.Default_Animation("ThrowBone_Attack_Intro", this.m_throwBone_AttackIntro_AnimationSpeed, this.m_throwBone_AttackIntro_Delay, true);
		for (int i = 0; i < this.m_miniBoss_Bone_ThrowAmount; i++)
		{
			int num = 5 + i;
			if (num >= 9)
			{
				num = 5;
			}
			throwAngle = UnityEngine.Random.Range((int)this.m_miniBoss_Bone_ThrowAngle.x, (int)this.m_miniBoss_Bone_ThrowAngle.y);
			this.FireProjectile("SkeletonBoneMinibossProjectile", num, true, (float)throwAngle, 1f, true, true, true);
		}
		yield return this.Default_Animation("ThrowBone_Attack_Hold", this.m_throwBone_AttackHold_AnimationSpeed, 0f, false);
		base.Wait(0.75f, false);
		yield return this.Default_Animation("ThrowBone_Exit", this.m_throwBone_Exit_AnimationSpeed, this.m_throwBone_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield break;
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x000076EF File Offset: 0x000058EF
	public IEnumerator Single_Attack_Throw_Rib()
	{
		yield return this.Default_TellIntroAndLoop("ChestBone_Tell_Intro", this.m_throwRib_TellIntro_AnimationSpeed, "ChestBone_Tell_Hold", this.m_throwRib_TellHold_AnimationSpeed, 0.85f);
		yield return this.Default_Animation("ChestBone_Attack_Intro", this.m_throwRib_AttackIntro_AnimationSpeed, this.m_throwRib_AttackIntro_Delay, true);
		yield return this.Default_Animation("ChestBone_Attack_Hold", this.m_throwRib_AttackHold_AnimationSpeed, 0f, false);
		int i = 0;
		while ((float)i < this.m_miniBoss_Rib_ThrowAmount)
		{
			float miniBoss_Rib_ThrowPower = this.m_miniBoss_Rib_ThrowPower;
			UnityEngine.Random.Range(this.m_miniBoss_Rib_ThrowHeight.x, this.m_miniBoss_Rib_ThrowHeight.y);
			Projectile_RL projectile_RL = this.FireProjectile("SkeletonBoneBigProjectile", 4, true, (float)this.m_throwRib_Angle, miniBoss_Rib_ThrowPower, true, true, true);
			projectile_RL.RotationSpeed = projectile_RL.InitialRotationSpeed;
			if (this.m_miniBoss_Rib_LoopDelay > 0f)
			{
				yield return base.Wait(this.m_miniBoss_Rib_LoopDelay, false);
			}
			int num = i;
			i = num + 1;
		}
		yield return base.Wait(this.m_throwRib_AttackHold_Delay, false);
		yield return this.Default_Animation("ChestBone_Exit", this.m_throwRib_Exit_AnimationSpeed, this.m_throwRib_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield break;
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x000076FE File Offset: 0x000058FE
	public IEnumerator Single_Action_Jump()
	{
		if (!base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(this.m_jump_Power.x, true);
		}
		else
		{
			base.SetVelocityX(-this.m_jump_Power.x, true);
		}
		base.SetVelocityY(this.m_jump_Power.y, false);
		yield return this.ChangeAnimationState("JumpUp");
		yield return base.Wait(0.05f, false);
		yield break;
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float m_modeShift_Damage_Mod
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x0000770D File Offset: 0x0000590D
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Mode_Shift()
	{
		this.m_isModeShifting = true;
		this.ToDo("Mode_Shift " + this.m_modeShiftIndex.ToString());
		base.RemoveStatusEffects(false);
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = true;
		yield return base.DeathAnim();
		base.SetVelocity(0f, 0f, false);
		base.EnemyController.LockFlip = true;
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_Damage_Mod;
		yield return this.Default_Animation("ModeShift_Intro", this.m_modeShift_Tell_Intro_AnimSpeed, this.m_modeShift_TellIntro_Delay, true);
		yield return this.Default_Animation("ModeShift_Scream_Intro", this.m_modeShift_Attack_Intro_AnimSpeed, this.m_modeShift_Attack_Intro_Delay, true);
		yield return this.Default_Animation("ModeShift_Scream_Hold", this.m_modeShift_Attack_Hold_AnimSpeed, this.m_modeShift_Attack_Hold_Delay, true);
		this.ModeShiftComplete_SpawnSecondBoss = true;
		this.SummonModeShiftBosses(false);
		yield return this.Default_Animation("ModeShift_Scream_Exit", this.m_modeShift_Exit_AnimSpeed, this.m_modeShift_Exit_Delay, true);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.LockFlip = false;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		this.m_isModeShifting = false;
		yield break;
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x0006D38C File Offset: 0x0006B58C
	protected void SummonModeShiftBosses(bool forceSummon)
	{
		if (base.EnemyController.EnemyRank >= EnemyRank.Advanced && (forceSummon || this.m_modeShiftIndex > 1 || base.EnemyController.EnemyType == EnemyType.SkeletonBossB))
		{
			for (int i = 0; i < 2; i++)
			{
				Vector3 absoluteSpawnPositionAtIndex;
				if (i == 0)
				{
					absoluteSpawnPositionAtIndex = base.GetAbsoluteSpawnPositionAtIndex(9, false);
				}
				else if (i == 1)
				{
					absoluteSpawnPositionAtIndex = base.GetAbsoluteSpawnPositionAtIndex(10, false);
				}
				else
				{
					absoluteSpawnPositionAtIndex = base.GetAbsoluteSpawnPositionAtIndex(11, false);
				}
				if (this.IsCollidingAtSpawnPoint(absoluteSpawnPositionAtIndex))
				{
					absoluteSpawnPositionAtIndex.x = base.EnemyController.Midpoint.x;
				}
				EnemyManager.SummonEnemy(base.EnemyController, EnemyType.Skeleton, EnemyRank.Miniboss, absoluteSpawnPositionAtIndex, true, true, 1f, 1f).DisableCulling = true;
			}
			this.m_modeShiftSummons_Appeared = true;
		}
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x0000771C File Offset: 0x0000591C
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

	// Token: 0x06000C89 RID: 3209 RVA: 0x0000772B File Offset: 0x0000592B
	public override IEnumerator DeathAnim()
	{
		this.ModeShiftComplete_SpawnSecondBoss = true;
		yield return base.DeathAnim();
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 0.5f);
		yield return this.Default_Animation("Death_Intro", this.m_death_Intro_AnimSpeed, this.m_death_Intro_Delay, true);
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		base.EnemyController.GetComponent<BossModeShiftController>().DissolveMaterials(this.m_death_Hold_Delay);
		yield return this.Default_Animation("Death_Loop", this.m_death_Hold_AnimSpeed, this.m_death_Hold_Delay, true);
		yield break;
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x0006D450 File Offset: 0x0006B650
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.ModeShiftComplete_SpawnSecondBoss = false;
		base.LogicController.DisableLogicActivationByDistance = true;
		this.m_jump_Power = new Vector2(-11.5f, 45f);
		this.m_jump_Tell_Delay = 0.55f;
		this.m_jump_Exit_AttackCD = 6f;
		this.m_throwBone_AttackCD = 6f;
		this.m_throwRib_Exit_AttackCD = 6f;
		base.EnemyController.HealthChangeRelay.AddListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit), false);
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x0006D4D8 File Offset: 0x0006B6D8
	private void OnBossHit(object sender, HealthChangeEventArgs args)
	{
		if (this.m_isModeShifting)
		{
			return;
		}
		if (base.EnemyController.IsDead)
		{
			return;
		}
		if (base.EnemyController.EnemyType == EnemyType.SkeletonBossB && base.LogicController.EnemyLogicType == EnemyLogicType.Basic)
		{
			return;
		}
		if (args.PrevHealthValue <= args.NewHealthValue)
		{
			return;
		}
		float[] array = this.m_modeShiftLevels_Basic;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Advanced)
		{
			if (base.EnemyController.EnemyType == EnemyType.SkeletonBossA)
			{
				array = this.m_modeShiftLevels_Advanced_Skeleton_A;
			}
			else
			{
				array = this.m_modeShiftLevels_Advanced_Skeleton_B;
			}
		}
		if (this.m_modeShiftIndex >= array.Length)
		{
			return;
		}
		float num = array[this.m_modeShiftIndex] * (float)base.EnemyController.ActualMaxHealth;
		if (base.EnemyController.CurrentHealth <= num)
		{
			this.m_modeShiftIndex++;
			base.LogicController.StopAllLogic(true);
			base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "Mode_Shift";
			if (this.m_modeShiftEventArgs == null)
			{
				this.m_modeShiftEventArgs = new EnemyModeShiftEventArgs(base.EnemyController);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EnemyModeShift, this, this.m_modeShiftEventArgs);
		}
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x0000773A File Offset: 0x0000593A
	public override void ResetScript()
	{
		this.m_isModeShifting = false;
		this.m_modeShiftSummons_Appeared = false;
		this.m_modeShiftIndex = 0;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		base.ResetScript();
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x00007768 File Offset: 0x00005968
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.DisableOffscreenWarnings = true;
		base.EnemyController.AlwaysFacing = true;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x0006D5E4 File Offset: 0x0006B7E4
	public bool IsCollidingAtSpawnPoint(Vector3 spawnPoint)
	{
		int layerMask = base.EnemyController.ControllerCorgi.PlatformMask & ~base.EnemyController.ControllerCorgi.OneWayPlatformMask;
		return Physics2D.OverlapBox(spawnPoint, Vector2.one, 0f, layerMask);
	}

	// Token: 0x04000EBB RID: 3771
	public UnityEvent_GameObject JumpEvent;

	// Token: 0x04000EBC RID: 3772
	protected int m_modeShiftIndex;

	// Token: 0x04000EBD RID: 3773
	protected float[] m_modeShiftLevels_Basic = new float[]
	{
		0.65f
	};

	// Token: 0x04000EBE RID: 3774
	protected float[] m_modeShiftLevels_Advanced_Skeleton_A = new float[]
	{
		0.66f,
		0.33f
	};

	// Token: 0x04000EBF RID: 3775
	protected float[] m_modeShiftLevels_Advanced_Skeleton_B = new float[]
	{
		0.5f
	};

	// Token: 0x04000EC0 RID: 3776
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x04000EC1 RID: 3777
	private bool m_isModeShifting;

	// Token: 0x04000EC2 RID: 3778
	protected float m_throwBone_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000EC3 RID: 3779
	protected float m_throwBone_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000EC4 RID: 3780
	protected const float m_throwBone_TellIntroAndHold_Delay = 0.65f;

	// Token: 0x04000EC5 RID: 3781
	protected float m_throwBone_AttackIntro_AnimationSpeed = 0.8f;

	// Token: 0x04000EC6 RID: 3782
	protected float m_throwBone_AttackIntro_Delay;

	// Token: 0x04000EC7 RID: 3783
	protected float m_throwBone_AttackHold_AnimationSpeed = 0.8f;

	// Token: 0x04000EC8 RID: 3784
	protected const float m_throwBone_AttackHold_Delay = 0.75f;

	// Token: 0x04000EC9 RID: 3785
	protected float m_throwBone_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000ECA RID: 3786
	protected float m_throwBone_Exit_Delay = 0.15f;

	// Token: 0x04000ECB RID: 3787
	protected int m_BoneNear_Angle = 83;

	// Token: 0x04000ECC RID: 3788
	protected int m_FarBoneAngle = 70;

	// Token: 0x04000ECD RID: 3789
	protected int m_miniBoss_Bone_ThrowAmount = 6;

	// Token: 0x04000ECE RID: 3790
	protected Vector2 m_miniBoss_Bone_ThrowAngle = new Vector2(55f, 85f);

	// Token: 0x04000ECF RID: 3791
	protected float m_throwBone_Exit_ForceIdle = 0.15f;

	// Token: 0x04000ED0 RID: 3792
	protected float m_throwBone_AttackCD;

	// Token: 0x04000ED1 RID: 3793
	protected float m_throwRib_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000ED2 RID: 3794
	protected float m_throwRib_TellHold_AnimationSpeed = 1.35f;

	// Token: 0x04000ED3 RID: 3795
	protected const float m_throwRib_TellIntroAndHold_Delay = 0.85f;

	// Token: 0x04000ED4 RID: 3796
	protected float m_throwRib_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000ED5 RID: 3797
	protected float m_throwRib_AttackIntro_Delay;

	// Token: 0x04000ED6 RID: 3798
	protected float m_throwRib_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x04000ED7 RID: 3799
	protected float m_throwRib_AttackHold_Delay = 1.25f;

	// Token: 0x04000ED8 RID: 3800
	protected float m_throwRib_Exit_AnimationSpeed = 1f;

	// Token: 0x04000ED9 RID: 3801
	protected float m_throwRib_Exit_Delay;

	// Token: 0x04000EDA RID: 3802
	protected int m_throwRib_Angle;

	// Token: 0x04000EDB RID: 3803
	protected float m_throwRib_secondRibSpeedMod = 1.35f;

	// Token: 0x04000EDC RID: 3804
	protected float m_throwRib_Exit_ForceIdle = 0.15f;

	// Token: 0x04000EDD RID: 3805
	protected float m_throwRib_Exit_AttackCD = 2f;

	// Token: 0x04000EDE RID: 3806
	protected float m_miniBoss_Rib_ThrowAmount = 1f;

	// Token: 0x04000EDF RID: 3807
	protected float m_miniBoss_Rib_ThrowPower = 1f;

	// Token: 0x04000EE0 RID: 3808
	protected Vector2 m_miniBoss_Rib_ThrowHeight = new Vector2(6f, 6f);

	// Token: 0x04000EE1 RID: 3809
	protected float m_miniBoss_Rib_LoopDelay = 0.9f;

	// Token: 0x04000EE2 RID: 3810
	protected float m_jump_Tell_Delay = 0.4f;

	// Token: 0x04000EE3 RID: 3811
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000EE4 RID: 3812
	protected Vector2 m_jump_Power = new Vector2(0f, 27f);

	// Token: 0x04000EE5 RID: 3813
	protected int m_miniBoss_Bone_JumpAmount = 5;

	// Token: 0x04000EE6 RID: 3814
	protected float m_jump_Exit_Delay;

	// Token: 0x04000EE7 RID: 3815
	protected float m_jump_Exit_ForceIdle = 0.15f;

	// Token: 0x04000EE8 RID: 3816
	protected float m_jump_Exit_AttackCD;

	// Token: 0x04000EE9 RID: 3817
	protected Vector2 m_jumpAndThrowBone_ProjectileSpawnDelay = new Vector2(0f, 0f);

	// Token: 0x04000EEA RID: 3818
	protected float m_jumpAndThrowBone_Exit_ForceIdle = 0.15f;

	// Token: 0x04000EEB RID: 3819
	protected float m_jumpAndThrowBone_Exit_AttackCD = 1.25f;

	// Token: 0x04000EEC RID: 3820
	protected Vector2 m_miniBoss_Land_ThrowAngle = new Vector2(25f, 30f);

	// Token: 0x04000EED RID: 3821
	protected Vector2 m_miniBoss_Land_ThrowPower = new Vector2(0.75f, 1f);

	// Token: 0x04000EEE RID: 3822
	protected float m_dance_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000EEF RID: 3823
	protected float m_dance_AttackIntro_Delay = 1f;

	// Token: 0x04000EF0 RID: 3824
	protected float m_dance_AttackHold_Delay = 1f;

	// Token: 0x04000EF1 RID: 3825
	protected float m_dance_Exit_ForceIdle = 1f;

	// Token: 0x04000EF2 RID: 3826
	protected float m_dance_Exit_AttackCD = 5f;

	// Token: 0x04000EF3 RID: 3827
	protected const string MODE_SHIFT_TELL_INTRO = "ModeShift_Intro";

	// Token: 0x04000EF4 RID: 3828
	protected const string MODE_SHIFT_ATTACK_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000EF5 RID: 3829
	protected const string MODE_SHIFT_ATTACK_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x04000EF6 RID: 3830
	protected const string MODE_SHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000EF7 RID: 3831
	protected float m_modeShift_Tell_Intro_AnimSpeed = 1f;

	// Token: 0x04000EF8 RID: 3832
	protected float m_modeShift_TellIntro_Delay = 0.5f;

	// Token: 0x04000EF9 RID: 3833
	protected float m_modeShift_Attack_Intro_AnimSpeed = 1f;

	// Token: 0x04000EFA RID: 3834
	protected float m_modeShift_Attack_Intro_Delay;

	// Token: 0x04000EFB RID: 3835
	protected float m_modeShift_Attack_Hold_AnimSpeed = 1f;

	// Token: 0x04000EFC RID: 3836
	protected float m_modeShift_Attack_Hold_Delay = 2f;

	// Token: 0x04000EFD RID: 3837
	protected float m_modeShift_Exit_AnimSpeed = 1f;

	// Token: 0x04000EFE RID: 3838
	protected float m_modeShift_Exit_Delay;

	// Token: 0x04000EFF RID: 3839
	protected bool m_modeShiftSummons_Appeared;

	// Token: 0x04000F00 RID: 3840
	[NonSerialized]
	public bool ModeShiftComplete_SpawnSecondBoss;

	// Token: 0x04000F01 RID: 3841
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x04000F02 RID: 3842
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x04000F03 RID: 3843
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04000F04 RID: 3844
	protected float m_spawn_Idle_Delay;

	// Token: 0x04000F05 RID: 3845
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04000F06 RID: 3846
	protected float m_spawn_Intro_Delay;

	// Token: 0x04000F07 RID: 3847
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x04000F08 RID: 3848
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x04000F09 RID: 3849
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04000F0A RID: 3850
	protected float m_death_Intro_Delay;

	// Token: 0x04000F0B RID: 3851
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04000F0C RID: 3852
	protected float m_death_Hold_Delay = 4.5f;
}
