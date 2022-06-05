using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class SkeletonBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000881 RID: 2177 RVA: 0x0001C60C File Offset: 0x0001A80C
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

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06000882 RID: 2178 RVA: 0x0001C65D File Offset: 0x0001A85D
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06000883 RID: 2179 RVA: 0x0001C66E File Offset: 0x0001A86E
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06000884 RID: 2180 RVA: 0x0001C67F File Offset: 0x0001A87F
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06000885 RID: 2181 RVA: 0x0001C690 File Offset: 0x0001A890
	protected virtual bool m_jump_Land_Spawn_Curse
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x0001C693 File Offset: 0x0001A893
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

	// Token: 0x06000887 RID: 2183 RVA: 0x0001C6A2 File Offset: 0x0001A8A2
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

	// Token: 0x06000888 RID: 2184 RVA: 0x0001C6B1 File Offset: 0x0001A8B1
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

	// Token: 0x06000889 RID: 2185 RVA: 0x0001C6C0 File Offset: 0x0001A8C0
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

	// Token: 0x0600088A RID: 2186 RVA: 0x0001C6CF File Offset: 0x0001A8CF
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

	// Token: 0x0600088B RID: 2187 RVA: 0x0001C6DE File Offset: 0x0001A8DE
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

	// Token: 0x0600088C RID: 2188 RVA: 0x0001C6ED File Offset: 0x0001A8ED
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

	// Token: 0x0600088D RID: 2189 RVA: 0x0001C6FC File Offset: 0x0001A8FC
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

	// Token: 0x0600088E RID: 2190 RVA: 0x0001C712 File Offset: 0x0001A912
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

	// Token: 0x0600088F RID: 2191 RVA: 0x0001C721 File Offset: 0x0001A921
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

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06000890 RID: 2192 RVA: 0x0001C730 File Offset: 0x0001A930
	protected virtual float m_modeShift_Damage_Mod
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0001C737 File Offset: 0x0001A937
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

	// Token: 0x06000892 RID: 2194 RVA: 0x0001C748 File Offset: 0x0001A948
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

	// Token: 0x06000893 RID: 2195 RVA: 0x0001C809 File Offset: 0x0001AA09
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

	// Token: 0x06000894 RID: 2196 RVA: 0x0001C818 File Offset: 0x0001AA18
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

	// Token: 0x06000895 RID: 2197 RVA: 0x0001C828 File Offset: 0x0001AA28
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

	// Token: 0x06000896 RID: 2198 RVA: 0x0001C8B0 File Offset: 0x0001AAB0
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

	// Token: 0x06000897 RID: 2199 RVA: 0x0001C9BB File Offset: 0x0001ABBB
	public override void ResetScript()
	{
		this.m_isModeShifting = false;
		this.m_modeShiftSummons_Appeared = false;
		this.m_modeShiftIndex = 0;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		base.ResetScript();
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0001C9E9 File Offset: 0x0001ABE9
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.DisableOffscreenWarnings = true;
		base.EnemyController.AlwaysFacing = true;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0001CA0C File Offset: 0x0001AC0C
	public bool IsCollidingAtSpawnPoint(Vector3 spawnPoint)
	{
		int layerMask = base.EnemyController.ControllerCorgi.PlatformMask & ~base.EnemyController.ControllerCorgi.OneWayPlatformMask;
		return Physics2D.OverlapBox(spawnPoint, Vector2.one, 0f, layerMask);
	}

	// Token: 0x04000BB9 RID: 3001
	public UnityEvent_GameObject JumpEvent;

	// Token: 0x04000BBA RID: 3002
	protected int m_modeShiftIndex;

	// Token: 0x04000BBB RID: 3003
	protected float[] m_modeShiftLevels_Basic = new float[]
	{
		0.65f
	};

	// Token: 0x04000BBC RID: 3004
	protected float[] m_modeShiftLevels_Advanced_Skeleton_A = new float[]
	{
		0.66f,
		0.33f
	};

	// Token: 0x04000BBD RID: 3005
	protected float[] m_modeShiftLevels_Advanced_Skeleton_B = new float[]
	{
		0.5f
	};

	// Token: 0x04000BBE RID: 3006
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x04000BBF RID: 3007
	private bool m_isModeShifting;

	// Token: 0x04000BC0 RID: 3008
	protected float m_throwBone_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000BC1 RID: 3009
	protected float m_throwBone_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000BC2 RID: 3010
	protected const float m_throwBone_TellIntroAndHold_Delay = 0.65f;

	// Token: 0x04000BC3 RID: 3011
	protected float m_throwBone_AttackIntro_AnimationSpeed = 0.8f;

	// Token: 0x04000BC4 RID: 3012
	protected float m_throwBone_AttackIntro_Delay;

	// Token: 0x04000BC5 RID: 3013
	protected float m_throwBone_AttackHold_AnimationSpeed = 0.8f;

	// Token: 0x04000BC6 RID: 3014
	protected const float m_throwBone_AttackHold_Delay = 0.75f;

	// Token: 0x04000BC7 RID: 3015
	protected float m_throwBone_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000BC8 RID: 3016
	protected float m_throwBone_Exit_Delay = 0.15f;

	// Token: 0x04000BC9 RID: 3017
	protected int m_BoneNear_Angle = 83;

	// Token: 0x04000BCA RID: 3018
	protected int m_FarBoneAngle = 70;

	// Token: 0x04000BCB RID: 3019
	protected int m_miniBoss_Bone_ThrowAmount = 6;

	// Token: 0x04000BCC RID: 3020
	protected Vector2 m_miniBoss_Bone_ThrowAngle = new Vector2(55f, 85f);

	// Token: 0x04000BCD RID: 3021
	protected float m_throwBone_Exit_ForceIdle = 0.15f;

	// Token: 0x04000BCE RID: 3022
	protected float m_throwBone_AttackCD;

	// Token: 0x04000BCF RID: 3023
	protected float m_throwRib_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000BD0 RID: 3024
	protected float m_throwRib_TellHold_AnimationSpeed = 1.35f;

	// Token: 0x04000BD1 RID: 3025
	protected const float m_throwRib_TellIntroAndHold_Delay = 0.85f;

	// Token: 0x04000BD2 RID: 3026
	protected float m_throwRib_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000BD3 RID: 3027
	protected float m_throwRib_AttackIntro_Delay;

	// Token: 0x04000BD4 RID: 3028
	protected float m_throwRib_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x04000BD5 RID: 3029
	protected float m_throwRib_AttackHold_Delay = 1.25f;

	// Token: 0x04000BD6 RID: 3030
	protected float m_throwRib_Exit_AnimationSpeed = 1f;

	// Token: 0x04000BD7 RID: 3031
	protected float m_throwRib_Exit_Delay;

	// Token: 0x04000BD8 RID: 3032
	protected int m_throwRib_Angle;

	// Token: 0x04000BD9 RID: 3033
	protected float m_throwRib_secondRibSpeedMod = 1.35f;

	// Token: 0x04000BDA RID: 3034
	protected float m_throwRib_Exit_ForceIdle = 0.15f;

	// Token: 0x04000BDB RID: 3035
	protected float m_throwRib_Exit_AttackCD = 2f;

	// Token: 0x04000BDC RID: 3036
	protected float m_miniBoss_Rib_ThrowAmount = 1f;

	// Token: 0x04000BDD RID: 3037
	protected float m_miniBoss_Rib_ThrowPower = 1f;

	// Token: 0x04000BDE RID: 3038
	protected Vector2 m_miniBoss_Rib_ThrowHeight = new Vector2(6f, 6f);

	// Token: 0x04000BDF RID: 3039
	protected float m_miniBoss_Rib_LoopDelay = 0.9f;

	// Token: 0x04000BE0 RID: 3040
	protected float m_jump_Tell_Delay = 0.4f;

	// Token: 0x04000BE1 RID: 3041
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000BE2 RID: 3042
	protected Vector2 m_jump_Power = new Vector2(0f, 27f);

	// Token: 0x04000BE3 RID: 3043
	protected int m_miniBoss_Bone_JumpAmount = 5;

	// Token: 0x04000BE4 RID: 3044
	protected float m_jump_Exit_Delay;

	// Token: 0x04000BE5 RID: 3045
	protected float m_jump_Exit_ForceIdle = 0.15f;

	// Token: 0x04000BE6 RID: 3046
	protected float m_jump_Exit_AttackCD;

	// Token: 0x04000BE7 RID: 3047
	protected Vector2 m_jumpAndThrowBone_ProjectileSpawnDelay = new Vector2(0f, 0f);

	// Token: 0x04000BE8 RID: 3048
	protected float m_jumpAndThrowBone_Exit_ForceIdle = 0.15f;

	// Token: 0x04000BE9 RID: 3049
	protected float m_jumpAndThrowBone_Exit_AttackCD = 1.25f;

	// Token: 0x04000BEA RID: 3050
	protected Vector2 m_miniBoss_Land_ThrowAngle = new Vector2(25f, 30f);

	// Token: 0x04000BEB RID: 3051
	protected Vector2 m_miniBoss_Land_ThrowPower = new Vector2(0.75f, 1f);

	// Token: 0x04000BEC RID: 3052
	protected float m_dance_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000BED RID: 3053
	protected float m_dance_AttackIntro_Delay = 1f;

	// Token: 0x04000BEE RID: 3054
	protected float m_dance_AttackHold_Delay = 1f;

	// Token: 0x04000BEF RID: 3055
	protected float m_dance_Exit_ForceIdle = 1f;

	// Token: 0x04000BF0 RID: 3056
	protected float m_dance_Exit_AttackCD = 5f;

	// Token: 0x04000BF1 RID: 3057
	protected const string MODE_SHIFT_TELL_INTRO = "ModeShift_Intro";

	// Token: 0x04000BF2 RID: 3058
	protected const string MODE_SHIFT_ATTACK_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000BF3 RID: 3059
	protected const string MODE_SHIFT_ATTACK_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x04000BF4 RID: 3060
	protected const string MODE_SHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000BF5 RID: 3061
	protected float m_modeShift_Tell_Intro_AnimSpeed = 1f;

	// Token: 0x04000BF6 RID: 3062
	protected float m_modeShift_TellIntro_Delay = 0.5f;

	// Token: 0x04000BF7 RID: 3063
	protected float m_modeShift_Attack_Intro_AnimSpeed = 1f;

	// Token: 0x04000BF8 RID: 3064
	protected float m_modeShift_Attack_Intro_Delay;

	// Token: 0x04000BF9 RID: 3065
	protected float m_modeShift_Attack_Hold_AnimSpeed = 1f;

	// Token: 0x04000BFA RID: 3066
	protected float m_modeShift_Attack_Hold_Delay = 2f;

	// Token: 0x04000BFB RID: 3067
	protected float m_modeShift_Exit_AnimSpeed = 1f;

	// Token: 0x04000BFC RID: 3068
	protected float m_modeShift_Exit_Delay;

	// Token: 0x04000BFD RID: 3069
	protected bool m_modeShiftSummons_Appeared;

	// Token: 0x04000BFE RID: 3070
	[NonSerialized]
	public bool ModeShiftComplete_SpawnSecondBoss;

	// Token: 0x04000BFF RID: 3071
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x04000C00 RID: 3072
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x04000C01 RID: 3073
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04000C02 RID: 3074
	protected float m_spawn_Idle_Delay;

	// Token: 0x04000C03 RID: 3075
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04000C04 RID: 3076
	protected float m_spawn_Intro_Delay;

	// Token: 0x04000C05 RID: 3077
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x04000C06 RID: 3078
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x04000C07 RID: 3079
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04000C08 RID: 3080
	protected float m_death_Intro_Delay;

	// Token: 0x04000C09 RID: 3081
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04000C0A RID: 3082
	protected float m_death_Hold_Delay = 4.5f;
}
