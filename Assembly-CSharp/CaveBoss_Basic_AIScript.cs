using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using RLAudio;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000C2 RID: 194
public class CaveBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600036A RID: 874 RVA: 0x00004706 File Offset: 0x00002906
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.75f);
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x0600036B RID: 875 RVA: 0x00004717 File Offset: 0x00002917
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.75f, 1.15f);
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x0600036C RID: 876 RVA: 0x00004717 File Offset: 0x00002917
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.75f, 1.15f);
		}
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00051940 File Offset: 0x0004FB40
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"CaveBossVoidWallProjectile",
			"CaveBossVoidWallAdvancedProjectile",
			"CaveBossCurseProjectile",
			"CaveBossCurseAdvancedProjectile",
			"CaveBossSlashProjectile",
			"CaveBossSlashExplosionProjectile",
			"CaveBossSlashFireballProjectile",
			"CaveBossRollingProjectile",
			"CaveBossRollingAdvancedProjectile",
			"CaveBossFallingRubbleProjectile",
			"CaveBossJumpExplosionProjectile",
			"CaveBossBombBoltProjectile",
			"CaveBossShoutWarningProjectile",
			"CaveBossShoutAttackProjectile",
			"CaveBossInfiniteChaseProjectile",
			"CaveBossInfiniteChaseProjectileVariant",
			"CaveBossStaticWallProjectile",
			"CaveBossFloorWarningProjectile",
			"CaveBossFloorBoltProjectile",
			"CaveBossVerticalBoltProjectile",
			"CaveBossWaveProjectile",
			"CaveBossWaveSmallProjectile",
			"CaveBossWaveWarningProjectile"
		};
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00051A20 File Offset: 0x0004FC20
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.HealthChangeRelay.AddListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit), false);
		this.m_modeShiftController = base.EnemyController.GetComponent<BossModeShiftController>();
		foreach (GameObject gameObject in (base.EnemyController.HitboxController as HitboxController).WeaponHitboxList)
		{
			if (gameObject.name.Contains("Jumping"))
			{
				this.m_jumpingWeaponHitbox = gameObject;
			}
			else
			{
				this.m_regularWeaponHitbox = gameObject;
			}
		}
		this.m_regularWeaponHitbox.SetActive(true);
		this.m_jumpingWeaponHitbox.SetActive(false);
		this.m_rootAnimatorsArray = base.EnemyController.Visuals.GetComponentsInChildren<Animator>();
		base.EnemyController.Animator.SetBool("HammerFire", false);
		base.LogicController.DisableLogicActivationByDistance = true;
		this.m_fallingRubbleEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_tubalBoss_fallingSwords_initiate_loop", base.transform);
		this.m_curseHandEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_tubalBoss_curseHand_start_loop", base.transform);
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00051B2C File Offset: 0x0004FD2C
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetBool("HammerFire", false);
		}
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00051B58 File Offset: 0x0004FD58
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		Vector3 center = base.EnemyController.Room.Bounds.center;
		center.x -= 5f;
		center.y -= 5f;
		LayerMask mask = 256;
		mask |= 2048;
		this.m_topPoint = Physics2D.Raycast(center, Vector2.up, 100f, mask).point.y;
		this.m_bottomPoint = Physics2D.Raycast(center, Vector2.down, 100f, mask).point.y;
		this.m_leftPoint = Physics2D.Raycast(center, Vector2.left, 100f, mask).point.x;
		this.m_rightPoint = Physics2D.Raycast(center, Vector2.right, 100f, mask).point.x;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00051C7C File Offset: 0x0004FE7C
	private void OnBossHit(object sender, HealthChangeEventArgs args)
	{
		if (this.m_isModeShifting)
		{
			return;
		}
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Basic && this.m_modeShiftIndex >= this.m_modeShiftLevels_Basic.Length)
		{
			return;
		}
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Advanced && this.m_modeShiftIndex >= this.m_modeShiftLevels_Advanced.Length)
		{
			return;
		}
		if (base.EnemyController.IsDead)
		{
			return;
		}
		if (args.PrevHealthValue <= args.NewHealthValue)
		{
			return;
		}
		float num = 0f;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Basic)
		{
			num = this.m_modeShiftLevels_Basic[this.m_modeShiftIndex] * (float)base.EnemyController.ActualMaxHealth;
		}
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Advanced)
		{
			num = this.m_modeShiftLevels_Advanced[this.m_modeShiftIndex] * (float)base.EnemyController.ActualMaxHealth;
		}
		if (base.EnemyController.CurrentHealth <= num)
		{
			this.m_modeShiftIndex++;
			base.LogicController.StopAllLogic(true);
			base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "Mode_Shift";
			this.m_modeShiftController.ChangeMaterials();
			if (this.m_modeShiftEventArgs == null)
			{
				this.m_modeShiftEventArgs = new EnemyModeShiftEventArgs(base.EnemyController);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EnemyModeShift, this, this.m_modeShiftEventArgs);
		}
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00051DB0 File Offset: 0x0004FFB0
	public override void ResetScript()
	{
		base.ResetScript();
		this.m_isModeShifting = false;
		this.m_modeShiftIndex = 0;
		this.m_requiredHPDropToSummon = 0f;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		base.LogicController.DisableLogicActivationByDistance = true;
		this.StopWaveProjectilePersistentCoroutine();
		this.StopWaveProjectilePersistent2Coroutine();
	}

	// Token: 0x06000373 RID: 883 RVA: 0x00004728 File Offset: 0x00002928
	protected override void OnDisable()
	{
		AudioManager.Stop(this.m_fallingRubbleEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_curseHandEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06000374 RID: 884 RVA: 0x00051E08 File Offset: 0x00050008
	private void OnDestroy()
	{
		if (base.EnemyController)
		{
			base.EnemyController.HealthChangeRelay.RemoveListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit));
		}
		if (this.m_fallingRubbleEventInstance.isValid())
		{
			this.m_fallingRubbleEventInstance.release();
		}
		if (this.m_curseHandEventInstance.isValid())
		{
			this.m_curseHandEventInstance.release();
		}
	}

	// Token: 0x06000375 RID: 885 RVA: 0x00051E74 File Offset: 0x00050074
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.EnemyController.StatusEffectController.RemoveStatusEffectImmunity(StatusEffectType.Enemy_Freeze);
		base.StopProjectile(ref this.m_modeShiftWarningProjectile);
		base.StopProjectile(ref this.m_modeShiftShoutProjectile);
		base.StopProjectile(ref this.m_chaseAttack_WarningProjectile);
		base.StopProjectile(ref this.m_chaseAttack_BoltProjectile);
		base.StopProjectile(ref this.m_chaseAttack_BoltProjectile2);
		base.StopProjectile(ref this.m_summonAttack_WarningProjectile);
		base.StopProjectile(ref this.m_summonAttack_ShoutProjectile);
		this.m_regularWeaponHitbox.SetActive(true);
		this.m_jumpingWeaponHitbox.SetActive(false);
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00004742 File Offset: 0x00002942
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Wall_Attack()
	{
		this.ToDo("Wall_Attack");
		base.SetVelocityX(0f, false);
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("WallAttack_Tell_Intro", this.m_wallAttack_TellIntro_AnimSpeed, "WallAttack_Tell_Hold", this.m_wallAttack_TellHold_AnimSpeed, this.m_wallAttack_TellIntroAndHold_Delay);
		yield return this.Default_Animation("WallAttack_Attack_Intro", this.m_wallAttack_AttackIntro_AnimSpeed, this.m_wallAttack_AttackIntro_Delay, true);
		if (base.EnemyController.EnemyRank == EnemyRank.Basic)
		{
			this.FireProjectile("CaveBossVoidWallProjectile", 0, false, 0f, 1f, true, true, true);
			Projectile_RL projectile_RL = this.FireProjectile("CaveBossVoidWallProjectile", 0, false, 180f, 1f, true, true, true);
			projectile_RL.transform.SetLocalScaleX(-projectile_RL.transform.localScale.x);
		}
		else if (base.EnemyController.EnemyRank == EnemyRank.Advanced)
		{
			yield return base.Wait(this.m_wallAttack_VariantAttackDelay, false);
			this.FireProjectile("CaveBossVoidWallAdvancedProjectile", 0, false, 0f, 1f, true, true, true);
			Projectile_RL projectile_RL2 = this.FireProjectile("CaveBossVoidWallAdvancedProjectile", 0, false, 180f, 1f, true, true, true);
			projectile_RL2.transform.SetLocalScaleX(-projectile_RL2.transform.localScale.x);
		}
		yield return this.Default_Animation("WallAttack_Attack_Hold", this.m_wallAttack_AttackHold_AnimSpeed, this.m_wallAttack_AttackHold_Delay, false);
		yield return base.Wait(this.m_wallAttack_AttackExitDelay, false);
		yield return this.Default_Animation("WallAttack_Exit", this.m_wallAttack_Exit_AnimSpeed, this.m_wallAttack_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_wallAttack_Exit_IdleDuration, this.m_wallAttack_AttackCD);
		yield break;
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000377 RID: 887 RVA: 0x00004751 File Offset: 0x00002951
	protected virtual Vector2 m_lineAttackAngleOffset
	{
		get
		{
			return new Vector2(-50f, 50f);
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000378 RID: 888 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_lineAttackDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000379 RID: 889 RVA: 0x00004762 File Offset: 0x00002962
	protected virtual int m_lineAttackCount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00004765 File Offset: 0x00002965
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Line_Attack()
	{
		this.ToDo("Line_Attack");
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		AudioManager.PlayAttached(this, this.m_curseHandEventInstance, base.gameObject);
		yield return this.Default_TellIntroAndLoop("StreamProjectile_Tell_Intro", this.m_lineAttack_TellIntro_AnimSpeed, "StreamProjectile_Tell_Hold", this.m_lineAttack_TellHold_AnimSpeed, this.m_lineAttack_TellIntroAndHold_Delay);
		yield return this.Default_Animation("StreamProjectile_Attack_Intro", this.m_lineAttack_AttackIntro_AnimSpeed, this.m_lineAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("StreamProjectile_Attack_Hold", this.m_lineAttack_AttackHold_AnimSpeed, this.m_lineAttack_AttackHold_Delay, false);
		float attackIntervalDelay = this.m_lineAttackDuration / (float)this.m_lineAttackCount;
		int num;
		for (int i = 0; i < this.m_lineAttackCount; i = num + 1)
		{
			float angle = UnityEngine.Random.Range(this.m_lineAttackAngleOffset.x, this.m_lineAttackAngleOffset.y);
			if (base.EnemyController.EnemyRank == EnemyRank.Basic)
			{
				this.FireProjectile("CaveBossCurseProjectile", 8, true, angle, 1f, true, true, true);
			}
			else if (base.EnemyController.EnemyRank == EnemyRank.Advanced)
			{
				this.FireProjectile("CaveBossCurseAdvancedProjectile", 8, true, angle, 1f, true, true, true);
			}
			yield return base.Wait(attackIntervalDelay, false);
			num = i;
		}
		yield return base.Wait(this.m_lineAttack_AttackExitDelay, false);
		AudioManager.Stop(this.m_curseHandEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		yield return this.Default_Animation("StreamProjectile_Exit", this.m_lineAttack_Exit_AnimSpeed, this.m_lineAttack_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_lineAttack_Exit_IdleDuration, this.m_lineAttack_AttackCD);
		yield break;
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x0600037B RID: 891 RVA: 0x00003DA4 File Offset: 0x00001FA4
	protected virtual float m_hammerAttackDashSpeed
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x0600037C RID: 892 RVA: 0x00003C5B File Offset: 0x00001E5B
	protected virtual float m_hammerAttackDashDuration
	{
		get
		{
			return 0.125f;
		}
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00004774 File Offset: 0x00002974
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Hammer_Attack()
	{
		this.ToDo("Hammer_Attack");
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		float dashSpeed = this.m_hammerAttackDashSpeed;
		if (!base.EnemyController.IsTargetToMyRight)
		{
			dashSpeed = -dashSpeed;
		}
		yield return this.Default_TellIntroAndLoop("HammerAttack_Tell_Intro", this.m_hammerAttack_TellIntro_AnimSpeed, "HammerAttack_Tell_Hold", this.m_hammerAttack_TellHold_AnimSpeed, this.m_hammerAttack_TellIntroAndHold_Delay);
		yield return this.Default_Animation("HammerAttack_Attack_Intro", this.m_hammerAttack_AttackIntro_AnimSpeed, this.m_hammerAttack_AttackIntro_Delay, true);
		this.FireProjectile("CaveBossSlashProjectile", 14, true, 0f, 1f, true, true, true);
		base.SetAttackingWithContactDamage(true, 0f);
		yield return this.Default_Animation("HammerAttack_Attack_Hold", this.m_hammerAttack_AttackHold_AnimSpeed, 0f, false);
		base.SetVelocityX(dashSpeed, false);
		base.EnemyController.GroundHorizontalVelocity = dashSpeed;
		base.EnemyController.DisableFriction = true;
		yield return base.Wait(0.1f, false);
		this.FireProjectile("CaveBossSlashExplosionProjectile", 15, true, 0f, 1f, true, true, true);
		if (base.EnemyController.EnemyRank == EnemyRank.Basic)
		{
			this.FireProjectile("CaveBossRollingProjectile", 3, false, 0f, 1f, true, true, true).Flip();
			this.FireProjectile("CaveBossRollingProjectile", 3, false, 180f, 1f, true, true, true);
		}
		else if (base.EnemyController.EnemyRank == EnemyRank.Advanced)
		{
			this.FireProjectile("CaveBossRollingAdvancedProjectile", 3, false, 0f, 1f, true, true, true).Flip();
			this.FireProjectile("CaveBossRollingAdvancedProjectile", 3, false, 180f, 1f, true, true, true);
			this.FireProjectile("CaveBossSlashFireballProjectile", 15, false, 60f, 1f, true, true, true);
			this.FireProjectile("CaveBossSlashFireballProjectile", 15, false, 75f, 1f, true, true, true);
			this.FireProjectile("CaveBossSlashFireballProjectile", 15, false, 90f, 1f, true, true, true);
			this.FireProjectile("CaveBossSlashFireballProjectile", 15, false, 105f, 1f, true, true, true);
			this.FireProjectile("CaveBossSlashFireballProjectile", 15, false, 120f, 1f, true, true, true);
		}
		yield return base.Wait(this.m_hammerAttackDashDuration, false);
		yield return base.WaitUntilIsGrounded();
		base.SetVelocityX(0f, false);
		base.EnemyController.GroundHorizontalVelocity = 0f;
		base.EnemyController.DisableFriction = false;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return base.Wait(this.m_hammerAttack_AttackExitDelay, false);
		yield return this.Default_Animation("HammerAttack_Exit", this.m_hammerAttack_Exit_AnimSpeed, this.m_hammerAttack_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_hammerAttack_Exit_IdleDuration, this.m_hammerAttack_AttackCD);
		yield break;
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00004783 File Offset: 0x00002983
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Jump_Attack()
	{
		this.ToDo("Jump_Attack");
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("JumpAttack_Tell_Intro", this.m_jump_TellIntro_AnimSpeed, "JumpAttack_Tell_Hold", this.m_jump_TellHold_AnimSpeed, this.m_jump_TellIntroAndHold_Delay);
		Vector2 jumpVelocity = this.m_jumpVelocity;
		if (!base.EnemyController.IsFacingRight)
		{
			jumpVelocity.x = -jumpVelocity.x;
		}
		base.SetVelocity(jumpVelocity, false);
		base.SetAttackingWithContactDamage(true, 0f);
		yield return this.Default_Animation("JumpAttack_Jump_Intro_Intro", this.m_jumpAttack_AttackIntro_AnimSpeed, 0f, true);
		yield return this.Default_Animation("JumpAttack_Jump_Hold", this.m_jumpAttack_AttackHold_AnimSpeed, 0f, false);
		yield return base.Wait(0.05f, false);
		this.m_regularWeaponHitbox.SetActive(false);
		this.m_jumpingWeaponHitbox.SetActive(true);
		while (base.EnemyController.Velocity.y >= 0f)
		{
			yield return null;
		}
		yield return this.Default_Animation("JumpAttack_Fall_Intro", this.m_jumpAttack_AttackIntro_AnimSpeed, 0f, true);
		yield return this.Default_Animation("JumpAttack_Fall_Hold", this.m_jumpAttack_AttackHold_AnimSpeed, 0f, false);
		yield return base.WaitUntilIsGrounded();
		base.SetVelocityX(0f, false);
		this.RunPersistentCoroutine(this.DropMagmaBalls());
		if (base.EnemyController.EnemyRank == EnemyRank.Advanced)
		{
			this.FireProjectile("CaveBossJumpExplosionProjectile", 0, false, 0f, 1f, true, true, true);
		}
		yield return this.Default_Animation("JumpAttack_Land_Intro", this.m_jumpAttack_AttackIntro_AnimSpeed, 0f, true);
		this.m_regularWeaponHitbox.SetActive(true);
		this.m_jumpingWeaponHitbox.SetActive(false);
		yield return this.Default_Animation("JumpAttack_Land_Hold", this.m_jumpAttack_AttackHold_AnimSpeed, this.m_jumpAttack_LandHold_Delay, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("JumpAttack_Exit", this.m_jumpAttack_Exit_AnimSpeed, this.m_jumpAttack_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_jumpAttack_Exit_IdleDuration, this.m_jumpAttack_AttackCD);
		yield break;
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x0600037F RID: 895 RVA: 0x00004792 File Offset: 0x00002992
	protected virtual int m_jumpAttack_NumMagmaBalls
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000380 RID: 896 RVA: 0x00003D93 File Offset: 0x00001F93
	protected virtual float m_jumpAttackMagmaBallDuration
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00004795 File Offset: 0x00002995
	private IEnumerator DropMagmaBalls()
	{
		AudioManager.Play(this, this.m_fallingRubbleEventInstance);
		Projectile_RL lastProjectile = null;
		float magmaBallInterval = this.m_jumpAttackMagmaBallDuration / (float)this.m_jumpAttack_NumMagmaBalls;
		float yPos = base.EnemyController.Room.Bounds.max.y + 4f;
		float yPos2 = base.EnemyController.Room.Bounds.max.y + 9f;
		float yPos3 = base.EnemyController.Room.Bounds.max.y + 14f;
		int num2;
		for (int i = 0; i < this.m_jumpAttack_NumMagmaBalls; i = num2 + 1)
		{
			float delayTime = Time.time + magmaBallInterval;
			float num = UnityEngine.Random.Range(this.m_jumpAttackMagmaBallSpawnOffset.x, this.m_jumpAttackMagmaBallSpawnOffset.y);
			float x = PlayerManager.GetPlayerController().transform.localPosition.x + num;
			this.FireProjectileAbsPos("CaveBossFallingRubbleProjectile", new Vector2(x, yPos), false, -90f, 1f, true, true, true);
			num = UnityEngine.Random.Range(this.m_jumpAttackMagmaBallSpawnOffset.x, this.m_jumpAttackMagmaBallSpawnOffset.y);
			x = PlayerManager.GetPlayerController().transform.localPosition.x + num * 2f;
			this.FireProjectileAbsPos("CaveBossFallingRubbleProjectile", new Vector2(x, yPos2), false, -90f, 1f, true, true, true);
			num = UnityEngine.Random.Range(this.m_jumpAttackMagmaBallSpawnOffset.x, this.m_jumpAttackMagmaBallSpawnOffset.y);
			x = PlayerManager.GetPlayerController().transform.localPosition.x + num * 3f;
			lastProjectile = this.FireProjectileAbsPos("CaveBossFallingRubbleProjectile", new Vector2(x, yPos3), false, -90f, 1f, true, true, true);
			while (Time.time < delayTime)
			{
				yield return null;
			}
			num2 = i;
		}
		if (lastProjectile)
		{
			float delayTime = Time.time + 2f;
			while (Time.time < delayTime)
			{
				yield return null;
			}
			AudioManager.Stop(this.m_fallingRubbleEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		yield break;
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000382 RID: 898 RVA: 0x000047A4 File Offset: 0x000029A4
	protected virtual int m_enemiesSummoned
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000383 RID: 899 RVA: 0x000047A4 File Offset: 0x000029A4
	protected virtual int m_maxNumberofEnemiesAllowed
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000384 RID: 900 RVA: 0x000047A7 File Offset: 0x000029A7
	protected virtual int m_summonFreeHitStatusEffectCount
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000385 RID: 901 RVA: 0x00003E42 File Offset: 0x00002042
	protected virtual int m_enemyInvulnTimerDuration
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x06000386 RID: 902 RVA: 0x000047AA File Offset: 0x000029AA
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Summon_Attack()
	{
		if (EnemyManager.NumActiveSummonedEnemies >= 2)
		{
			yield break;
		}
		if (this.m_requiredHPDropToSummon > 0f)
		{
			if (base.EnemyController.CurrentHealth > this.m_requiredHPDropToSummon)
			{
				yield break;
			}
		}
		else if (this.m_requiredHPDropToSummon < 0f)
		{
			yield break;
		}
		base.EnemyController.StatusEffectController.AddStatusEffectImmunity(StatusEffectType.Enemy_Freeze);
		this.ToDo("Summon_Attack");
		base.SetVelocityX(0f, false);
		base.EnemyController.LockFlip = true;
		this.m_summonAttack_WarningProjectile = this.FireProjectile("CaveBossShoutWarningProjectile", 7, true, 0f, 1f, true, true, true);
		yield return this.Default_TellIntroAndLoop("SummonEnemy_Tell_Intro", this.m_summonAttack_TellIntro_AnimSpeed, "SummonEnemy_Tell_Hold", this.m_summonAttack_TellHold_AnimSpeed, this.m_summonAttack_TellIntroAndHold_Delay);
		base.StopProjectile(ref this.m_summonAttack_WarningProjectile);
		yield return this.Default_Animation("SummonEnemy_Attack_Intro", this.m_summonAttack_AttackIntro_AnimSpeed, this.m_summonAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("SummonEnemy_Attack_Hold", this.m_summonAttack_AttackHold_AnimSpeed, 0f, false);
		yield return base.Wait(0.15f, false);
		this.m_summonAttack_ShoutProjectile = this.FireProjectile("CaveBossShoutAttackProjectile", 7, true, 0f, 1f, true, true, true);
		int num = UnityEngine.Random.Range(0, this.m_summonEnemyTypeArray.Length);
		EnemyType enemyType = this.m_summonEnemyTypeArray[num];
		EnemyRank rank = EnemyRank.Advanced;
		if (base.EnemyController.EnemyRank == EnemyRank.Advanced)
		{
			rank = EnemyRank.Advanced;
		}
		Bounds bounds = base.EnemyController.Room.Bounds;
		Vector2 spawnOffset = new Vector2(UnityEngine.Random.Range(bounds.min.x + this.HorizontalRoomIndent, bounds.max.x - this.HorizontalRoomIndent), UnityEngine.Random.Range(bounds.min.y + this.VerticalRoomIndent, bounds.max.y - this.VerticalRoomIndent));
		for (int i = 0; i < this.m_enemiesSummoned; i++)
		{
			spawnOffset = new Vector2(UnityEngine.Random.Range(bounds.min.x + this.HorizontalRoomIndent, bounds.max.x - this.HorizontalRoomIndent), UnityEngine.Random.Range(bounds.min.y + this.VerticalRoomIndent, bounds.max.y - this.VerticalRoomIndent));
			this.RunPersistentCoroutine(this.AddStatusEffectSummonCoroutine(enemyType, rank, spawnOffset));
		}
		this.m_requiredHPDropToSummon = base.EnemyController.CurrentHealth - (float)base.EnemyController.ActualMaxHealth * 0.21f;
		yield return base.Wait(this.m_summonAttack_AttackHold_Delay, false);
		base.StopProjectile(ref this.m_summonAttack_ShoutProjectile);
		yield return this.Default_Animation("SummonEnemy_Exit", this.m_summonAttack_Exit_AnimSpeed, this.m_summonAttack_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_summonAttack_Exit_IdleDuration, this.m_summonAttack_AttackCD);
		yield break;
	}

	// Token: 0x06000387 RID: 903 RVA: 0x000047B9 File Offset: 0x000029B9
	private IEnumerator AddStatusEffectSummonCoroutine(EnemyType enemyType, EnemyRank rank, Vector2 spawnOffset)
	{
		SummonEnemyController component = base.EnemyController.gameObject.GetComponent<SummonEnemyController>();
		EnemyController enemyToSummon = null;
		if (component)
		{
			if (component.Contains(enemyType, rank))
			{
				enemyToSummon = EnemyManager.SummonEnemy(base.EnemyController, enemyType, rank, spawnOffset, true, false, 1f, 1f);
			}
			else
			{
				Debug.Log(string.Concat(new string[]
				{
					"Could not summon enemy: ",
					enemyType.ToString(),
					" - ",
					rank.ToString(),
					". Enemy not found in SummonEnemyController."
				}));
			}
		}
		if (enemyToSummon)
		{
			yield return EnemyManager.RunSummonAnimCoroutine(enemyToSummon, 1f, false, 0f);
			if (this.m_summonsHaveFreeHitStatusEffect && !enemyToSummon.IsDead)
			{
				EnemyFreeHitStatusEffect enemyFreeHitStatusEffect = enemyToSummon.StatusEffectController.GetStatusEffect(StatusEffectType.Enemy_FreeHit) as EnemyFreeHitStatusEffect;
				enemyFreeHitStatusEffect.FreeHitCountOverride = this.m_summonFreeHitStatusEffectCount;
				enemyFreeHitStatusEffect.FreeHitRegenerationOverride = this.m_summonFreeHitRegenerationAmount;
				enemyToSummon.StatusEffectController.StartStatusEffect(StatusEffectType.Enemy_FreeHit, 0f, null);
				enemyFreeHitStatusEffect.FreeHitCountOverride = -1;
				enemyFreeHitStatusEffect.FreeHitRegenerationOverride = -1f;
				enemyToSummon.StatusEffectController.StartStatusEffect(StatusEffectType.Enemy_InvulnTimer, (float)this.m_enemyInvulnTimerDuration, null);
			}
		}
		yield break;
	}

	// Token: 0x06000388 RID: 904 RVA: 0x000047DD File Offset: 0x000029DD
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Chase_Attack()
	{
		this.ToDo("Chase_Attack");
		base.SetVelocityX(0f, false);
		base.EnemyController.LockFlip = true;
		this.m_chaseAttack_WarningProjectile = this.FireProjectileAbsPos("CaveBossFloorWarningProjectile", new Vector2(this.m_leftPoint, this.m_bottomPoint), false, 0f, 1f, true, true, true);
		yield return this.Default_TellIntroAndLoop("ChaserProjectile_Tell_Intro", this.m_chaseAttack_TellIntro_AnimSpeed, "ChaserProjectile_Tell_Hold", this.m_chaseAttack_TellHold_AnimSpeed, this.m_chaseAttack_TellIntroAndHold_Delay);
		yield return this.Default_Animation("ChaserProjectile_Attack_Intro", this.m_chaseAttack_AttackIntro_AnimSpeed, this.m_chaseAttack_AttackIntro_Delay, true);
		base.StopProjectile(ref this.m_chaseAttack_WarningProjectile);
		this.m_chaseAttack_BoltProjectile = this.FireProjectileAbsPos("CaveBossFloorBoltProjectile", new Vector2(this.m_leftPoint, this.m_bottomPoint), false, 0f, 1f, true, true, true);
		this.FireProjectileAbsPos("CaveBossVerticalBoltProjectile", base.EnemyController.transform.position, false, 0f, 1f, true, true, true);
		yield return this.Default_Animation("ChaserProjectile_Attack_Hold", this.m_chaseAttack_AttackHold_AnimSpeed, this.m_chaseAttack_AttackHold_Delay, true);
		base.StopProjectile(ref this.m_chaseAttack_BoltProjectile);
		yield return this.Default_Animation("ChaserProjectile_Exit", this.m_chaseAttack_Exit_AnimSpeed, this.m_chaseAttack_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_chaseAttack_Exit_IdleDuration, this.m_chaseAttack_AttackCD);
		yield break;
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000389 RID: 905 RVA: 0x000047EC File Offset: 0x000029EC
	protected Vector2 m_waveAttackRandomDelay
	{
		get
		{
			return new Vector2(11f, 14f);
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x0600038A RID: 906 RVA: 0x000047FD File Offset: 0x000029FD
	protected Vector2 m_waveAttackRandomPosOffset
	{
		get
		{
			return new Vector2(-1f, 1f);
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x0600038B RID: 907 RVA: 0x00003A3D File Offset: 0x00001C3D
	protected Vector2 m_waveAttackRandomAngle
	{
		get
		{
			return new Vector2(-10f, 10f);
		}
	}

	// Token: 0x0600038C RID: 908 RVA: 0x0000480E File Offset: 0x00002A0E
	public IEnumerator Wave_Attack_PersistentCoroutine(bool isSecond)
	{
		if (!isSecond)
		{
			this.m_persistentWaveAttackRunning = true;
		}
		else
		{
			this.m_persistentWaveAttackRunning2 = true;
		}
		float delay = Time.time + this.m_waveAttack_Initial_Delay;
		for (;;)
		{
			if (Time.time >= delay)
			{
				yield return this.FireWaveProjectile(isSecond);
				delay = Time.time + UnityEngine.Random.Range(this.m_waveAttackRandomDelay.x, this.m_waveAttackRandomDelay.y);
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00004824 File Offset: 0x00002A24
	private IEnumerator FireWaveProjectile(bool isSecond)
	{
		float startingAngle = 0f;
		Vector2 startingPos = base.EnemyController.TargetController.Midpoint;
		int num = CDGHelper.RandomPlusMinus();
		int num2 = CDGHelper.RandomPlusMinus();
		float num3 = UnityEngine.Random.Range(this.m_waveAttackRandomPosOffset.x, this.m_waveAttackRandomPosOffset.y);
		if (num > 0)
		{
			if (num2 > 0)
			{
				startingAngle = 0f;
				startingPos.x = base.EnemyController.Room.BoundsRect.xMin - 1f;
			}
			else
			{
				startingAngle = 180f;
				startingPos.x = base.EnemyController.Room.BoundsRect.xMax + 1f;
			}
			startingPos.y += num3;
		}
		else
		{
			if (num2 > 0)
			{
				startingAngle = -90f;
				startingPos.y = base.EnemyController.Room.BoundsRect.yMax + 1f;
			}
			else
			{
				startingAngle = 90f;
				startingPos.y = base.EnemyController.Room.BoundsRect.yMin - 1f;
			}
			startingPos.x += num3;
		}
		float num4 = UnityEngine.Random.Range(this.m_waveAttackRandomAngle.x, this.m_waveAttackRandomAngle.y);
		startingAngle += num4;
		startingAngle = CDGHelper.WrapAngleDegrees(startingAngle, true);
		if (!isSecond)
		{
			this.m_waveAttack_WarningProjectile = this.FireProjectileAbsPos("CaveBossWaveWarningProjectile", startingPos, false, startingAngle, 1f, true, true, true);
		}
		else
		{
			this.m_waveAttack_WarningProjectile2 = this.FireProjectileAbsPos("CaveBossWaveWarningProjectile", startingPos, false, startingAngle, 1f, true, true, true);
		}
		float delay = Time.time + this.m_waveAttackWarningDuration;
		while (Time.time < delay)
		{
			yield return null;
		}
		if (!isSecond)
		{
			base.StopProjectile(ref this.m_waveAttack_WarningProjectile);
			this.m_waveAttack_Projectile = this.FireProjectileAbsPos("CaveBossWaveProjectile", startingPos, false, startingAngle, 1f, true, true, true);
		}
		else
		{
			base.StopProjectile(ref this.m_waveAttack_WarningProjectile2);
			this.m_waveAttack_Projectile2 = this.FireProjectileAbsPos("CaveBossWaveProjectile", startingPos, false, startingAngle, 1f, true, true, true);
		}
		yield break;
	}

	// Token: 0x0600038E RID: 910 RVA: 0x0000483A File Offset: 0x00002A3A
	private void StopWaveProjectilePersistentCoroutine()
	{
		this.StopPersistentCoroutine(this.m_waveAttack_PersistentCoroutine);
		base.StopProjectile(ref this.m_waveAttack_Projectile);
		base.StopProjectile(ref this.m_waveAttack_WarningProjectile);
		this.m_persistentWaveAttackRunning = false;
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00004867 File Offset: 0x00002A67
	private void StopWaveProjectilePersistent2Coroutine()
	{
		this.StopPersistentCoroutine(this.m_waveAttack_PersistentCoroutine2);
		base.StopProjectile(ref this.m_waveAttack_Projectile2);
		base.StopProjectile(ref this.m_waveAttack_WarningProjectile2);
		this.m_persistentWaveAttackRunning2 = false;
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000390 RID: 912 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_thrustFast_DashAttackAmount
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00004894 File Offset: 0x00002A94
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
		base.RemoveStatusEffects(false);
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = true;
		yield return base.DeathAnim();
		this.ToDo("Mode_Shift " + this.m_modeShiftIndex.ToString());
		base.EnemyController.LockFlip = true;
		base.SetVelocityX(0f, false);
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_Damage_Mod;
		if (this.m_modeShiftIndex == 1)
		{
			MusicManager.SetBossEncounterParam(0.34f);
		}
		else if (this.m_modeShiftIndex == 2)
		{
			MusicManager.SetBossEncounterParam(0.67f);
		}
		yield return this.Default_Animation("ModeShift_Intro", this.m_modeShift_Downed_AnimSpeed, this.m_modeShift_Downed_Delay, true);
		yield return this.Default_Animation("ModeShift_GetUp", this.m_modeShift_GetUp_AnimSpeed, 0f, true);
		if (this.ModeShiftEvent != null)
		{
			this.ModeShiftEvent.Invoke();
		}
		this.m_modeShiftWarningProjectile = this.FireProjectile("CaveBossShoutWarningProjectile", 7, true, 0f, 1f, true, true, true);
		if (this.m_modeShift_GetUp_Delay > 0f)
		{
			yield return base.Wait(this.m_modeShift_GetUp_Delay, false);
		}
		yield return this.Default_Animation("ModeShift_Scream_Intro", this.m_modeShift_AttackIntro_AnimSpeed, this.m_modeShift_AttackIntro_Delay, true);
		base.StopProjectile(ref this.m_modeShiftWarningProjectile);
		yield return this.Default_Animation("ModeShift_Scream_Hold", this.m_modeShift_AttackHold_AnimSpeed, this.m_modeShift_AttackHold_Delay, false);
		this.m_modeShiftShoutProjectile = this.FireProjectile("CaveBossShoutAttackProjectile", 7, true, 0f, 1f, true, true, true);
		if (base.EnemyController.EnemyRank == EnemyRank.Basic)
		{
			if (this.m_modeShiftIndex == 1)
			{
				this.FireProjectile("CaveBossInfiniteChaseProjectile", 7, true, 90f, 1f, true, true, true);
			}
			else if (this.m_modeShiftIndex == 2)
			{
				float x = base.EnemyController.Room.Bounds.min.x + 2.5f;
				float num = base.EnemyController.Room.Bounds.min.y + 3f;
				this.FireProjectileAbsPos("CaveBossStaticWallProjectile", new Vector2(x, num + 0f), false, 0f, 1f, true, true, true);
				this.FireProjectileAbsPos("CaveBossStaticWallProjectile", new Vector2(x, num + 11f), false, 0f, 1f, true, true, true);
				this.FireProjectileAbsPos("CaveBossStaticWallProjectile", new Vector2(x, num + 22f), false, 0f, 1f, true, true, true);
				x = base.EnemyController.Room.Bounds.max.x - 3.25f;
				this.FireProjectileAbsPos("CaveBossStaticWallProjectile", new Vector2(x, num + 0f), false, 180f, 1f, true, true, true);
				this.FireProjectileAbsPos("CaveBossStaticWallProjectile", new Vector2(x, num + 11f), false, 180f, 1f, true, true, true);
				this.FireProjectileAbsPos("CaveBossStaticWallProjectile", new Vector2(x, num + 22f), false, 180f, 1f, true, true, true);
			}
		}
		else if (base.EnemyController.EnemyRank == EnemyRank.Advanced)
		{
			if (this.m_modeShiftIndex >= 1 && !this.m_persistentWaveAttackRunning && base.EnemyController.EnemyRank == EnemyRank.Advanced)
			{
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 0f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 45f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 90f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 135f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 180f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 225f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 270f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 315f, 1f, true, true, true);
				this.m_waveAttack_PersistentCoroutine = this.RunPersistentCoroutine(this.Wave_Attack_PersistentCoroutine(false));
			}
			if (this.m_modeShiftIndex == 2)
			{
				float x2 = base.EnemyController.Room.Bounds.min.x + 3.25f;
				float y = base.EnemyController.Room.Bounds.min.y + 3f;
				x2 = base.EnemyController.Room.Bounds.max.x - 3.25f;
				x2 = base.EnemyController.Room.Bounds.min.x + 28f;
				y = base.EnemyController.Room.Bounds.min.y + 4f;
				x2 = base.EnemyController.Room.Bounds.min.x + 3.25f;
				this.FireProjectileAbsPos("CaveBossStaticWallProjectile", new Vector2(x2, y), false, 90f, 1f, true, true, true);
				x2 = base.EnemyController.Room.Bounds.max.x - 3.25f;
				this.FireProjectileAbsPos("CaveBossStaticWallProjectile", new Vector2(x2, y), false, 90f, 1f, true, true, true);
			}
			if (this.m_modeShiftIndex >= 3 && !this.m_persistentWaveAttackRunning2 && base.EnemyController.EnemyRank == EnemyRank.Advanced)
			{
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 0f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 45f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 90f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 135f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 185f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 225f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 270f, 1f, true, true, true);
				this.FireProjectile("CaveBossWaveSmallProjectile", 7, true, 315f, 1f, true, true, true);
				this.m_waveAttack_PersistentCoroutine2 = this.RunPersistentCoroutine(this.Wave_Attack_PersistentCoroutine(true));
			}
		}
		yield return base.Wait(this.m_modeShift_AttackExitDelay, false);
		base.StopProjectile(ref this.m_modeShiftShoutProjectile);
		yield return this.Default_Animation("ModeShift_Scream_Exit", this.m_modeShift_Exit_AnimSpeed, this.m_modeShift_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.ResetBaseValues();
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		yield return this.Default_Attack_Cooldown(this.m_modeShift_Exit_IdleDuration, this.m_modeShift_AttackCD);
		this.m_isModeShifting = false;
		yield break;
	}

	// Token: 0x06000392 RID: 914 RVA: 0x000048A3 File Offset: 0x00002AA3
	public override IEnumerator SpawnAnim()
	{
		yield return this.Default_Animation("Intro_Idle", this.m_spawn_Idle_AnimSpeed, this.m_spawn_Idle_Delay, true);
		this.SetAnimationSpeedMultiplier(this.m_spawn_Intro_AnimSpeed);
		yield return this.ChangeAnimationState("Intro");
		while (base.EnemyController.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.15f)
		{
			yield return null;
		}
		MusicManager.PlayMusic(SongID.CaveBossBGM_Tettix_Blacksmith_Boss, false, false);
		int trigger = Animator.StringToHash("Retract");
		Animator[] rootAnimatorsArray = this.m_rootAnimatorsArray;
		for (int i = 0; i < rootAnimatorsArray.Length; i++)
		{
			rootAnimatorsArray[i].SetTrigger(trigger);
		}
		base.EnemyController.Animator.SetBool("HammerFire", true);
		yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		yield break;
	}

	// Token: 0x06000393 RID: 915 RVA: 0x000048B2 File Offset: 0x00002AB2
	public override IEnumerator DeathAnim()
	{
		yield return base.DeathAnim();
		EnemyManager.KillAllSummonedEnemies();
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 0.5f);
		AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_tubalBoss_deathSequence", base.gameObject.transform.position);
		yield return this.Default_Animation("Death_Intro", this.m_death_Intro_AnimSpeed, this.m_death_Intro_Delay, true);
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		base.EnemyController.GetComponent<BossModeShiftController>().DissolveMaterials(this.m_death_Hold_Delay);
		yield return this.Default_Animation("Death_Loop", this.m_death_Hold_AnimSpeed, this.m_death_Hold_Delay, true);
		yield break;
	}

	// Token: 0x04000735 RID: 1845
	private float m_topPoint;

	// Token: 0x04000736 RID: 1846
	private float m_bottomPoint;

	// Token: 0x04000737 RID: 1847
	private float m_leftPoint;

	// Token: 0x04000738 RID: 1848
	private float m_rightPoint;

	// Token: 0x04000739 RID: 1849
	private readonly EnemyType[] m_summonEnemyTypeArray = new EnemyType[]
	{
		EnemyType.FlyingAxe,
		EnemyType.FlyingHammer,
		EnemyType.FlyingShield,
		EnemyType.FlyingSword
	};

	// Token: 0x0400073A RID: 1850
	private const float SUMMON_ENEMY_HP_MOD = 0f;

	// Token: 0x0400073B RID: 1851
	private BossModeShiftController m_modeShiftController;

	// Token: 0x0400073C RID: 1852
	private bool m_isModeShifting;

	// Token: 0x0400073D RID: 1853
	private GameObject m_regularWeaponHitbox;

	// Token: 0x0400073E RID: 1854
	private GameObject m_jumpingWeaponHitbox;

	// Token: 0x0400073F RID: 1855
	private Animator[] m_rootAnimatorsArray;

	// Token: 0x04000740 RID: 1856
	private EventInstance m_fallingRubbleEventInstance;

	// Token: 0x04000741 RID: 1857
	private EventInstance m_curseHandEventInstance;

	// Token: 0x04000742 RID: 1858
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x04000743 RID: 1859
	protected const string WALL_ATTACK_TELL_INTRO = "WallAttack_Tell_Intro";

	// Token: 0x04000744 RID: 1860
	protected const string WALL_ATTACK_TELL_HOLD = "WallAttack_Tell_Hold";

	// Token: 0x04000745 RID: 1861
	protected const string WALL_ATTACK_ATTACK_INTRO = "WallAttack_Attack_Intro";

	// Token: 0x04000746 RID: 1862
	protected const string WALL_ATTACK_ATTACK_HOLD = "WallAttack_Attack_Hold";

	// Token: 0x04000747 RID: 1863
	protected const string WALL_ATTACK_EXIT = "WallAttack_Exit";

	// Token: 0x04000748 RID: 1864
	protected const string WALL_ATTACK_PROJECTILE = "CaveBossVoidWallProjectile";

	// Token: 0x04000749 RID: 1865
	protected const string WALL_ATTACK_ADVANCED_PROJECTILE = "CaveBossVoidWallAdvancedProjectile";

	// Token: 0x0400074A RID: 1866
	protected float m_wallAttack_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x0400074B RID: 1867
	protected float m_wallAttack_TellHold_AnimSpeed = 1.2f;

	// Token: 0x0400074C RID: 1868
	protected float m_wallAttack_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x0400074D RID: 1869
	protected float m_wallAttack_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x0400074E RID: 1870
	protected float m_wallAttack_AttackIntro_Delay;

	// Token: 0x0400074F RID: 1871
	protected float m_wallAttack_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000750 RID: 1872
	protected float m_wallAttack_AttackHold_Delay = 0.45f;

	// Token: 0x04000751 RID: 1873
	protected float m_wallAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000752 RID: 1874
	protected float m_wallAttack_Exit_Delay = 0.45f;

	// Token: 0x04000753 RID: 1875
	protected float m_wallAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x04000754 RID: 1876
	protected float m_wallAttack_AttackCD = 12f;

	// Token: 0x04000755 RID: 1877
	protected float m_wallAttack_AttackExitDelay = 0.25f;

	// Token: 0x04000756 RID: 1878
	protected float m_wallAttack_VariantAttackDelay = 0.5f;

	// Token: 0x04000757 RID: 1879
	protected const string LINE_ATTACK_TELL_INTRO = "StreamProjectile_Tell_Intro";

	// Token: 0x04000758 RID: 1880
	protected const string LINE_ATTACK_TELL_HOLD = "StreamProjectile_Tell_Hold";

	// Token: 0x04000759 RID: 1881
	protected const string LINE_ATTACK_ATTACK_INTRO = "StreamProjectile_Attack_Intro";

	// Token: 0x0400075A RID: 1882
	protected const string LINE_ATTACK_ATTACK_HOLD = "StreamProjectile_Attack_Hold";

	// Token: 0x0400075B RID: 1883
	protected const string LINE_ATTACK_EXIT = "StreamProjectile_Exit";

	// Token: 0x0400075C RID: 1884
	protected const string LINE_ATTACK_PROJECTILE = "CaveBossCurseProjectile";

	// Token: 0x0400075D RID: 1885
	protected const string LINE_ATTACK_ADVANCED_PROJECTILE = "CaveBossCurseAdvancedProjectile";

	// Token: 0x0400075E RID: 1886
	protected float m_lineAttack_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x0400075F RID: 1887
	protected float m_lineAttack_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000760 RID: 1888
	protected float m_lineAttack_TellIntroAndHold_Delay = 1.1f;

	// Token: 0x04000761 RID: 1889
	protected float m_lineAttack_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000762 RID: 1890
	protected float m_lineAttack_AttackIntro_Delay;

	// Token: 0x04000763 RID: 1891
	protected float m_lineAttack_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000764 RID: 1892
	protected float m_lineAttack_AttackHold_Delay;

	// Token: 0x04000765 RID: 1893
	protected float m_lineAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000766 RID: 1894
	protected float m_lineAttack_Exit_Delay = 0.55f;

	// Token: 0x04000767 RID: 1895
	protected float m_lineAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x04000768 RID: 1896
	protected float m_lineAttack_AttackCD = 12f;

	// Token: 0x04000769 RID: 1897
	protected float m_lineAttack_AttackExitDelay = 0.25f;

	// Token: 0x0400076A RID: 1898
	protected const string HAMMER_ATTACK_TELL_INTRO = "HammerAttack_Tell_Intro";

	// Token: 0x0400076B RID: 1899
	protected const string HAMMER_ATTACK_TELL_HOLD = "HammerAttack_Tell_Hold";

	// Token: 0x0400076C RID: 1900
	protected const string HAMMER_ATTACK_ATTACK_INTRO = "HammerAttack_Attack_Intro";

	// Token: 0x0400076D RID: 1901
	protected const string HAMMER_ATTACK_ATTACK_HOLD = "HammerAttack_Attack_Hold";

	// Token: 0x0400076E RID: 1902
	protected const string HAMMER_ATTACK_EXIT = "HammerAttack_Exit";

	// Token: 0x0400076F RID: 1903
	protected const string HAMMER_ATTACK_PROJECTILE = "CaveBossSlashProjectile";

	// Token: 0x04000770 RID: 1904
	protected const string HAMMER_ATTACK_EXPLOSION_PROJECTILE = "CaveBossSlashExplosionProjectile";

	// Token: 0x04000771 RID: 1905
	protected const string HAMMER_ATTACK_ROLLING_PROJECTILE = "CaveBossRollingProjectile";

	// Token: 0x04000772 RID: 1906
	protected const string HAMMER_ATTACK_ROLLING_ADVANCED_PROJECTILE = "CaveBossRollingAdvancedProjectile";

	// Token: 0x04000773 RID: 1907
	protected const string HAMMER_ATTACK_VARIANT_PROJECTILE = "CaveBossSlashFireballProjectile";

	// Token: 0x04000774 RID: 1908
	protected float m_hammerAttack_TellIntro_AnimSpeed = 0.8f;

	// Token: 0x04000775 RID: 1909
	protected float m_hammerAttack_TellHold_AnimSpeed = 0.8f;

	// Token: 0x04000776 RID: 1910
	protected float m_hammerAttack_TellIntroAndHold_Delay = 0.95f;

	// Token: 0x04000777 RID: 1911
	protected float m_hammerAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000778 RID: 1912
	protected float m_hammerAttack_AttackIntro_Delay;

	// Token: 0x04000779 RID: 1913
	protected float m_hammerAttack_AttackHold_AnimSpeed = 1f;

	// Token: 0x0400077A RID: 1914
	protected float m_hammerAttack_AttackHold_Delay;

	// Token: 0x0400077B RID: 1915
	protected float m_hammerAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x0400077C RID: 1916
	protected float m_hammerAttack_Exit_Delay = 0.45f;

	// Token: 0x0400077D RID: 1917
	protected float m_hammerAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x0400077E RID: 1918
	protected float m_hammerAttack_AttackCD = 12f;

	// Token: 0x0400077F RID: 1919
	protected float m_hammerAttack_AttackExitDelay = 0.25f;

	// Token: 0x04000780 RID: 1920
	protected int m_hammerAttackVariantProjectileCount = 9;

	// Token: 0x04000781 RID: 1921
	protected const string JUMP_ATTACK_TELL_INTRO = "JumpAttack_Tell_Intro";

	// Token: 0x04000782 RID: 1922
	protected const string JUMP_ATTACK_TELL_HOLD = "JumpAttack_Tell_Hold";

	// Token: 0x04000783 RID: 1923
	protected const string JUMP_ATTACK_JUMP_INTRO = "JumpAttack_Jump_Intro_Intro";

	// Token: 0x04000784 RID: 1924
	protected const string JUMP_ATTACK_JUMP_HOLD = "JumpAttack_Jump_Hold";

	// Token: 0x04000785 RID: 1925
	protected const string JUMP_ATTACK_FALL_INTRO = "JumpAttack_Fall_Intro";

	// Token: 0x04000786 RID: 1926
	protected const string JUMP_ATTACK_FALL_HOLD = "JumpAttack_Fall_Hold";

	// Token: 0x04000787 RID: 1927
	protected const string JUMP_ATTACK_LAND_INTRO = "JumpAttack_Land_Intro";

	// Token: 0x04000788 RID: 1928
	protected const string JUMP_ATTACK_LAND_HOLD = "JumpAttack_Land_Hold";

	// Token: 0x04000789 RID: 1929
	protected const string JUMP_ATTACK_EXIT = "JumpAttack_Exit";

	// Token: 0x0400078A RID: 1930
	protected const string JUMP_ATTACK_MAGMA_PROJECTILE = "CaveBossFallingRubbleProjectile";

	// Token: 0x0400078B RID: 1931
	protected const string JUMP_VARIANT_PROJECTILE = "CaveBossJumpExplosionProjectile";

	// Token: 0x0400078C RID: 1932
	protected float m_jump_TellIntro_AnimSpeed = 0.8f;

	// Token: 0x0400078D RID: 1933
	protected float m_jump_TellHold_AnimSpeed = 0.8f;

	// Token: 0x0400078E RID: 1934
	protected float m_jump_TellIntroAndHold_Delay = 1f;

	// Token: 0x0400078F RID: 1935
	protected float m_jumpAttack_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000790 RID: 1936
	protected float m_jumpAttack_TellHold_AnimSpeed = 1f;

	// Token: 0x04000791 RID: 1937
	protected float m_jumpAttack_TellIntroAndHold_Delay;

	// Token: 0x04000792 RID: 1938
	protected float m_jumpAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000793 RID: 1939
	protected float m_jumpAttack_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000794 RID: 1940
	protected float m_jumpAttack_Fall_Intro_AnimSpeed = 1f;

	// Token: 0x04000795 RID: 1941
	protected float m_jumpAttack_Fall_Hold_AnimSpeed = 1f;

	// Token: 0x04000796 RID: 1942
	protected float m_jumpAttack_Land_Intro_AnimSpeed = 1f;

	// Token: 0x04000797 RID: 1943
	protected float m_jumpAttack_Land_Hold_AnimSpeed = 1f;

	// Token: 0x04000798 RID: 1944
	protected float m_jumpAttack_LandHold_Delay = 1f;

	// Token: 0x04000799 RID: 1945
	protected float m_jumpAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x0400079A RID: 1946
	protected float m_jumpAttack_Exit_Delay = 0.45f;

	// Token: 0x0400079B RID: 1947
	protected float m_jumpAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x0400079C RID: 1948
	protected float m_jumpAttack_AttackCD = 12f;

	// Token: 0x0400079D RID: 1949
	protected float m_jumpAttack_AttackExitDelay = 0.25f;

	// Token: 0x0400079E RID: 1950
	protected float m_jumpAttack_VariantAttackDelay = 0.5f;

	// Token: 0x0400079F RID: 1951
	protected Vector2 m_jumpVelocity = new Vector2(10f, 32f);

	// Token: 0x040007A0 RID: 1952
	protected Vector2 m_jumpAttackMagmaBallSpawnOffset = new Vector2(-15f, 15f);

	// Token: 0x040007A1 RID: 1953
	protected const string SUMMON_ATTACK_TELL_INTRO = "SummonEnemy_Tell_Intro";

	// Token: 0x040007A2 RID: 1954
	protected const string SUMMON_ATTACK_TELL_HOLD = "SummonEnemy_Tell_Hold";

	// Token: 0x040007A3 RID: 1955
	protected const string SUMMON_ATTACK_ATTACK_INTRO = "SummonEnemy_Attack_Intro";

	// Token: 0x040007A4 RID: 1956
	protected const string SUMMON_ATTACK_ATTACK_HOLD = "SummonEnemy_Attack_Hold";

	// Token: 0x040007A5 RID: 1957
	protected const string SUMMON_ATTACK_EXIT = "SummonEnemy_Exit";

	// Token: 0x040007A6 RID: 1958
	protected float m_summonAttack_TellIntro_AnimSpeed = 0.6f;

	// Token: 0x040007A7 RID: 1959
	protected float m_summonAttack_TellHold_AnimSpeed = 0.6f;

	// Token: 0x040007A8 RID: 1960
	protected float m_summonAttack_TellIntroAndHold_Delay = 1.1f;

	// Token: 0x040007A9 RID: 1961
	protected float m_summonAttack_AttackIntro_AnimSpeed = 0.6f;

	// Token: 0x040007AA RID: 1962
	protected float m_summonAttack_AttackIntro_Delay;

	// Token: 0x040007AB RID: 1963
	protected float m_summonAttack_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x040007AC RID: 1964
	protected float m_summonAttack_AttackHold_Delay = 1.15f;

	// Token: 0x040007AD RID: 1965
	protected float m_summonAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x040007AE RID: 1966
	protected float m_summonAttack_Exit_Delay = 0.45f;

	// Token: 0x040007AF RID: 1967
	protected float m_summonAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x040007B0 RID: 1968
	protected float m_summonAttack_AttackCD = 25f;

	// Token: 0x040007B1 RID: 1969
	private Projectile_RL m_summonAttack_WarningProjectile;

	// Token: 0x040007B2 RID: 1970
	private Projectile_RL m_summonAttack_ShoutProjectile;

	// Token: 0x040007B3 RID: 1971
	private float m_requiredHPDropToSummon;

	// Token: 0x040007B4 RID: 1972
	private float HorizontalRoomIndent = 5f;

	// Token: 0x040007B5 RID: 1973
	private float VerticalRoomIndent = 5f;

	// Token: 0x040007B6 RID: 1974
	private bool m_summonsHaveFreeHitStatusEffect = true;

	// Token: 0x040007B7 RID: 1975
	private const float m_hpReducRequiredBeforeSummoning = 0.21f;

	// Token: 0x040007B8 RID: 1976
	private float m_summonFreeHitRegenerationAmount = 999f;

	// Token: 0x040007B9 RID: 1977
	protected const string CHASE_ATTACK_TELL_INTRO = "ChaserProjectile_Tell_Intro";

	// Token: 0x040007BA RID: 1978
	protected const string CHASE_ATTACK_TELL_HOLD = "ChaserProjectile_Tell_Hold";

	// Token: 0x040007BB RID: 1979
	protected const string CHASE_ATTACK_ATTACK_INTRO = "ChaserProjectile_Attack_Intro";

	// Token: 0x040007BC RID: 1980
	protected const string CHASE_ATTACK_ATTACK_HOLD = "ChaserProjectile_Attack_Hold";

	// Token: 0x040007BD RID: 1981
	protected const string CHASE_ATTACK_EXIT = "ChaserProjectile_Exit";

	// Token: 0x040007BE RID: 1982
	protected const string CHASE_ATTACK_PROJECTILE = "CaveBossBombBoltProjectile";

	// Token: 0x040007BF RID: 1983
	protected const string CHASE_ATTACK_FLOOR_WARNING_PROJECTILE = "CaveBossFloorWarningProjectile";

	// Token: 0x040007C0 RID: 1984
	protected const string CHASE_ATTACK_FLOOR_BOLT_PROJECTILE = "CaveBossFloorBoltProjectile";

	// Token: 0x040007C1 RID: 1985
	protected const string CHASE_ATTACK_VERTICAL_BOLT_PROJECTILE = "CaveBossVerticalBoltProjectile";

	// Token: 0x040007C2 RID: 1986
	protected float m_chaseAttack_TellIntro_AnimSpeed = 0.8f;

	// Token: 0x040007C3 RID: 1987
	protected float m_chaseAttack_TellHold_AnimSpeed = 0.8f;

	// Token: 0x040007C4 RID: 1988
	protected float m_chaseAttack_TellIntroAndHold_Delay = 1.25f;

	// Token: 0x040007C5 RID: 1989
	protected float m_chaseAttack_AttackIntro_AnimSpeed = 0.8f;

	// Token: 0x040007C6 RID: 1990
	protected float m_chaseAttack_AttackIntro_Delay;

	// Token: 0x040007C7 RID: 1991
	protected float m_chaseAttack_AttackHold_AnimSpeed = 0.8f;

	// Token: 0x040007C8 RID: 1992
	protected float m_chaseAttack_AttackHold_Delay = 1.25f;

	// Token: 0x040007C9 RID: 1993
	protected float m_chaseAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x040007CA RID: 1994
	protected float m_chaseAttack_Exit_Delay = 0.45f;

	// Token: 0x040007CB RID: 1995
	protected float m_chaseAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x040007CC RID: 1996
	protected float m_chaseAttack_AttackCD = 12f;

	// Token: 0x040007CD RID: 1997
	private Projectile_RL m_chaseAttack_WarningProjectile;

	// Token: 0x040007CE RID: 1998
	private Projectile_RL m_chaseAttack_BoltProjectile;

	// Token: 0x040007CF RID: 1999
	private Projectile_RL m_chaseAttack_WarningProjectile2;

	// Token: 0x040007D0 RID: 2000
	private Projectile_RL m_chaseAttack_BoltProjectile2;

	// Token: 0x040007D1 RID: 2001
	protected const string WAVE_ATTACK_PROJECTILE = "CaveBossWaveProjectile";

	// Token: 0x040007D2 RID: 2002
	protected const string WAVE_ATTACK_SMALL_PROJECTILE = "CaveBossWaveSmallProjectile";

	// Token: 0x040007D3 RID: 2003
	protected const string WAVE_ATTACK_WARNING_PROJECTILE = "CaveBossWaveWarningProjectile";

	// Token: 0x040007D4 RID: 2004
	protected float m_waveAttackWarningDuration = 1.5f;

	// Token: 0x040007D5 RID: 2005
	protected float m_waveAttack_Initial_Delay = 1.5f;

	// Token: 0x040007D6 RID: 2006
	private Projectile_RL m_waveAttack_WarningProjectile;

	// Token: 0x040007D7 RID: 2007
	private Projectile_RL m_waveAttack_Projectile;

	// Token: 0x040007D8 RID: 2008
	private Projectile_RL m_waveAttack_WarningProjectile2;

	// Token: 0x040007D9 RID: 2009
	private Projectile_RL m_waveAttack_Projectile2;

	// Token: 0x040007DA RID: 2010
	private Coroutine m_waveAttack_PersistentCoroutine;

	// Token: 0x040007DB RID: 2011
	private Coroutine m_waveAttack_PersistentCoroutine2;

	// Token: 0x040007DC RID: 2012
	private bool m_persistentWaveAttackRunning;

	// Token: 0x040007DD RID: 2013
	private bool m_persistentWaveAttackRunning2;

	// Token: 0x040007DE RID: 2014
	public UnityEvent ModeShiftEvent;

	// Token: 0x040007DF RID: 2015
	private static List<EnemyType> m_enemySummonListHelper = new List<EnemyType>();

	// Token: 0x040007E0 RID: 2016
	protected const string MODESHIFT_DOWNED = "ModeShift_Intro";

	// Token: 0x040007E1 RID: 2017
	protected const string MODESHIFT_GETUP = "ModeShift_GetUp";

	// Token: 0x040007E2 RID: 2018
	protected const string MODESHIFT_ATTACK_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x040007E3 RID: 2019
	protected const string MODESHIFT_ATTACK_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x040007E4 RID: 2020
	protected const string MODESHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x040007E5 RID: 2021
	protected const string MODESHIFT_PROJECTILE_WARNING = "CaveBossShoutWarningProjectile";

	// Token: 0x040007E6 RID: 2022
	protected const string MODESHIFT_PROJECTILE_ATTACK = "CaveBossShoutAttackProjectile";

	// Token: 0x040007E7 RID: 2023
	protected const string MODESHIFT_PROJECTILE_SUMMON = "CaveBossInfiniteChaseProjectile";

	// Token: 0x040007E8 RID: 2024
	protected const string MODESHIFT_PROJECTILE_SUMMON_VARIANT = "CaveBossInfiniteChaseProjectileVariant";

	// Token: 0x040007E9 RID: 2025
	protected const string MODESHIFT_PROJECTILE_STATIC_WALL = "CaveBossStaticWallProjectile";

	// Token: 0x040007EA RID: 2026
	protected float m_modeShift_Downed_AnimSpeed = 0.8f;

	// Token: 0x040007EB RID: 2027
	protected float m_modeShift_Downed_Delay = 1.75f;

	// Token: 0x040007EC RID: 2028
	protected float m_modeShift_GetUp_AnimSpeed = 0.8f;

	// Token: 0x040007ED RID: 2029
	protected float m_modeShift_GetUp_Delay = 1.5f;

	// Token: 0x040007EE RID: 2030
	protected float m_modeShift_AttackIntro_AnimSpeed = 0.8f;

	// Token: 0x040007EF RID: 2031
	protected float m_modeShift_AttackIntro_Delay;

	// Token: 0x040007F0 RID: 2032
	protected float m_modeShift_AttackHold_AnimSpeed = 0.8f;

	// Token: 0x040007F1 RID: 2033
	protected float m_modeShift_AttackHold_Delay;

	// Token: 0x040007F2 RID: 2034
	protected float m_modeShift_Exit_AnimSpeed = 0.65f;

	// Token: 0x040007F3 RID: 2035
	protected float m_modeShift_Exit_Delay;

	// Token: 0x040007F4 RID: 2036
	protected float m_modeShift_Exit_IdleDuration = 0.15f;

	// Token: 0x040007F5 RID: 2037
	protected float m_modeShift_AttackCD = 99f;

	// Token: 0x040007F6 RID: 2038
	protected float m_modeShift_AttackExitDelay = 0.5f;

	// Token: 0x040007F7 RID: 2039
	protected int m_modeShiftIndex;

	// Token: 0x040007F8 RID: 2040
	protected float[] m_modeShiftLevels_Basic = new float[]
	{
		0.7f,
		0.35f
	};

	// Token: 0x040007F9 RID: 2041
	protected float[] m_modeShiftLevels_Advanced = new float[]
	{
		0.75f,
		0.5f,
		0.25f
	};

	// Token: 0x040007FA RID: 2042
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x040007FB RID: 2043
	protected bool m_canSummonDuplicates;

	// Token: 0x040007FC RID: 2044
	private Projectile_RL m_modeShiftWarningProjectile;

	// Token: 0x040007FD RID: 2045
	private Projectile_RL m_modeShiftShoutProjectile;

	// Token: 0x040007FE RID: 2046
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x040007FF RID: 2047
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x04000800 RID: 2048
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04000801 RID: 2049
	protected float m_spawn_Idle_Delay = 2f;

	// Token: 0x04000802 RID: 2050
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04000803 RID: 2051
	protected float m_spawn_Intro_Delay;

	// Token: 0x04000804 RID: 2052
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x04000805 RID: 2053
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x04000806 RID: 2054
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04000807 RID: 2055
	protected float m_death_Intro_Delay;

	// Token: 0x04000808 RID: 2056
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04000809 RID: 2057
	protected float m_death_Hold_Delay = 4.5f;
}
