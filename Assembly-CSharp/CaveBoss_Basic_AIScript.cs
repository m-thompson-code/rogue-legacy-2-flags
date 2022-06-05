using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using RLAudio;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200009B RID: 155
public class CaveBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x1700009B RID: 155
	// (get) Token: 0x0600029C RID: 668 RVA: 0x000133CB File Offset: 0x000115CB
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.75f);
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x0600029D RID: 669 RVA: 0x000133DC File Offset: 0x000115DC
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.75f, 1.15f);
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x0600029E RID: 670 RVA: 0x000133ED File Offset: 0x000115ED
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.75f, 1.15f);
		}
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00013400 File Offset: 0x00011600
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

	// Token: 0x060002A0 RID: 672 RVA: 0x000134E0 File Offset: 0x000116E0
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

	// Token: 0x060002A1 RID: 673 RVA: 0x000135EC File Offset: 0x000117EC
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetBool("HammerFire", false);
		}
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00013618 File Offset: 0x00011818
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

	// Token: 0x060002A3 RID: 675 RVA: 0x0001373C File Offset: 0x0001193C
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

	// Token: 0x060002A4 RID: 676 RVA: 0x00013870 File Offset: 0x00011A70
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

	// Token: 0x060002A5 RID: 677 RVA: 0x000138C5 File Offset: 0x00011AC5
	protected override void OnDisable()
	{
		AudioManager.Stop(this.m_fallingRubbleEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_curseHandEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x000138E0 File Offset: 0x00011AE0
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

	// Token: 0x060002A7 RID: 679 RVA: 0x0001394C File Offset: 0x00011B4C
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

	// Token: 0x060002A8 RID: 680 RVA: 0x000139DD File Offset: 0x00011BDD
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

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060002A9 RID: 681 RVA: 0x000139EC File Offset: 0x00011BEC
	protected virtual Vector2 m_lineAttackAngleOffset
	{
		get
		{
			return new Vector2(-50f, 50f);
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060002AA RID: 682 RVA: 0x000139FD File Offset: 0x00011BFD
	protected virtual float m_lineAttackDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060002AB RID: 683 RVA: 0x00013A04 File Offset: 0x00011C04
	protected virtual int m_lineAttackCount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00013A07 File Offset: 0x00011C07
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

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060002AD RID: 685 RVA: 0x00013A16 File Offset: 0x00011C16
	protected virtual float m_hammerAttackDashSpeed
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060002AE RID: 686 RVA: 0x00013A1D File Offset: 0x00011C1D
	protected virtual float m_hammerAttackDashDuration
	{
		get
		{
			return 0.125f;
		}
	}

	// Token: 0x060002AF RID: 687 RVA: 0x00013A24 File Offset: 0x00011C24
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

	// Token: 0x060002B0 RID: 688 RVA: 0x00013A33 File Offset: 0x00011C33
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

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060002B1 RID: 689 RVA: 0x00013A42 File Offset: 0x00011C42
	protected virtual int m_jumpAttack_NumMagmaBalls
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060002B2 RID: 690 RVA: 0x00013A45 File Offset: 0x00011C45
	protected virtual float m_jumpAttackMagmaBallDuration
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00013A4C File Offset: 0x00011C4C
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

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060002B4 RID: 692 RVA: 0x00013A5B File Offset: 0x00011C5B
	protected virtual int m_enemiesSummoned
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060002B5 RID: 693 RVA: 0x00013A5E File Offset: 0x00011C5E
	protected virtual int m_maxNumberofEnemiesAllowed
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x060002B6 RID: 694 RVA: 0x00013A61 File Offset: 0x00011C61
	protected virtual int m_summonFreeHitStatusEffectCount
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x060002B7 RID: 695 RVA: 0x00013A64 File Offset: 0x00011C64
	protected virtual int m_enemyInvulnTimerDuration
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00013A67 File Offset: 0x00011C67
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

	// Token: 0x060002B9 RID: 697 RVA: 0x00013A76 File Offset: 0x00011C76
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

	// Token: 0x060002BA RID: 698 RVA: 0x00013A9A File Offset: 0x00011C9A
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

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x060002BB RID: 699 RVA: 0x00013AA9 File Offset: 0x00011CA9
	protected Vector2 m_waveAttackRandomDelay
	{
		get
		{
			return new Vector2(11f, 14f);
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x060002BC RID: 700 RVA: 0x00013ABA File Offset: 0x00011CBA
	protected Vector2 m_waveAttackRandomPosOffset
	{
		get
		{
			return new Vector2(-1f, 1f);
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x060002BD RID: 701 RVA: 0x00013ACB File Offset: 0x00011CCB
	protected Vector2 m_waveAttackRandomAngle
	{
		get
		{
			return new Vector2(-10f, 10f);
		}
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00013ADC File Offset: 0x00011CDC
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

	// Token: 0x060002BF RID: 703 RVA: 0x00013AF2 File Offset: 0x00011CF2
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

	// Token: 0x060002C0 RID: 704 RVA: 0x00013B08 File Offset: 0x00011D08
	private void StopWaveProjectilePersistentCoroutine()
	{
		this.StopPersistentCoroutine(this.m_waveAttack_PersistentCoroutine);
		base.StopProjectile(ref this.m_waveAttack_Projectile);
		base.StopProjectile(ref this.m_waveAttack_WarningProjectile);
		this.m_persistentWaveAttackRunning = false;
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00013B35 File Offset: 0x00011D35
	private void StopWaveProjectilePersistent2Coroutine()
	{
		this.StopPersistentCoroutine(this.m_waveAttack_PersistentCoroutine2);
		base.StopProjectile(ref this.m_waveAttack_Projectile2);
		base.StopProjectile(ref this.m_waveAttack_WarningProjectile2);
		this.m_persistentWaveAttackRunning2 = false;
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x060002C2 RID: 706 RVA: 0x00013B62 File Offset: 0x00011D62
	protected virtual float m_thrustFast_DashAttackAmount
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00013B69 File Offset: 0x00011D69
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

	// Token: 0x060002C4 RID: 708 RVA: 0x00013B78 File Offset: 0x00011D78
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

	// Token: 0x060002C5 RID: 709 RVA: 0x00013B87 File Offset: 0x00011D87
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

	// Token: 0x0400068F RID: 1679
	private float m_topPoint;

	// Token: 0x04000690 RID: 1680
	private float m_bottomPoint;

	// Token: 0x04000691 RID: 1681
	private float m_leftPoint;

	// Token: 0x04000692 RID: 1682
	private float m_rightPoint;

	// Token: 0x04000693 RID: 1683
	private readonly EnemyType[] m_summonEnemyTypeArray = new EnemyType[]
	{
		EnemyType.FlyingAxe,
		EnemyType.FlyingHammer,
		EnemyType.FlyingShield,
		EnemyType.FlyingSword
	};

	// Token: 0x04000694 RID: 1684
	private const float SUMMON_ENEMY_HP_MOD = 0f;

	// Token: 0x04000695 RID: 1685
	private BossModeShiftController m_modeShiftController;

	// Token: 0x04000696 RID: 1686
	private bool m_isModeShifting;

	// Token: 0x04000697 RID: 1687
	private GameObject m_regularWeaponHitbox;

	// Token: 0x04000698 RID: 1688
	private GameObject m_jumpingWeaponHitbox;

	// Token: 0x04000699 RID: 1689
	private Animator[] m_rootAnimatorsArray;

	// Token: 0x0400069A RID: 1690
	private EventInstance m_fallingRubbleEventInstance;

	// Token: 0x0400069B RID: 1691
	private EventInstance m_curseHandEventInstance;

	// Token: 0x0400069C RID: 1692
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x0400069D RID: 1693
	protected const string WALL_ATTACK_TELL_INTRO = "WallAttack_Tell_Intro";

	// Token: 0x0400069E RID: 1694
	protected const string WALL_ATTACK_TELL_HOLD = "WallAttack_Tell_Hold";

	// Token: 0x0400069F RID: 1695
	protected const string WALL_ATTACK_ATTACK_INTRO = "WallAttack_Attack_Intro";

	// Token: 0x040006A0 RID: 1696
	protected const string WALL_ATTACK_ATTACK_HOLD = "WallAttack_Attack_Hold";

	// Token: 0x040006A1 RID: 1697
	protected const string WALL_ATTACK_EXIT = "WallAttack_Exit";

	// Token: 0x040006A2 RID: 1698
	protected const string WALL_ATTACK_PROJECTILE = "CaveBossVoidWallProjectile";

	// Token: 0x040006A3 RID: 1699
	protected const string WALL_ATTACK_ADVANCED_PROJECTILE = "CaveBossVoidWallAdvancedProjectile";

	// Token: 0x040006A4 RID: 1700
	protected float m_wallAttack_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x040006A5 RID: 1701
	protected float m_wallAttack_TellHold_AnimSpeed = 1.2f;

	// Token: 0x040006A6 RID: 1702
	protected float m_wallAttack_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x040006A7 RID: 1703
	protected float m_wallAttack_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x040006A8 RID: 1704
	protected float m_wallAttack_AttackIntro_Delay;

	// Token: 0x040006A9 RID: 1705
	protected float m_wallAttack_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x040006AA RID: 1706
	protected float m_wallAttack_AttackHold_Delay = 0.45f;

	// Token: 0x040006AB RID: 1707
	protected float m_wallAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x040006AC RID: 1708
	protected float m_wallAttack_Exit_Delay = 0.45f;

	// Token: 0x040006AD RID: 1709
	protected float m_wallAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x040006AE RID: 1710
	protected float m_wallAttack_AttackCD = 12f;

	// Token: 0x040006AF RID: 1711
	protected float m_wallAttack_AttackExitDelay = 0.25f;

	// Token: 0x040006B0 RID: 1712
	protected float m_wallAttack_VariantAttackDelay = 0.5f;

	// Token: 0x040006B1 RID: 1713
	protected const string LINE_ATTACK_TELL_INTRO = "StreamProjectile_Tell_Intro";

	// Token: 0x040006B2 RID: 1714
	protected const string LINE_ATTACK_TELL_HOLD = "StreamProjectile_Tell_Hold";

	// Token: 0x040006B3 RID: 1715
	protected const string LINE_ATTACK_ATTACK_INTRO = "StreamProjectile_Attack_Intro";

	// Token: 0x040006B4 RID: 1716
	protected const string LINE_ATTACK_ATTACK_HOLD = "StreamProjectile_Attack_Hold";

	// Token: 0x040006B5 RID: 1717
	protected const string LINE_ATTACK_EXIT = "StreamProjectile_Exit";

	// Token: 0x040006B6 RID: 1718
	protected const string LINE_ATTACK_PROJECTILE = "CaveBossCurseProjectile";

	// Token: 0x040006B7 RID: 1719
	protected const string LINE_ATTACK_ADVANCED_PROJECTILE = "CaveBossCurseAdvancedProjectile";

	// Token: 0x040006B8 RID: 1720
	protected float m_lineAttack_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x040006B9 RID: 1721
	protected float m_lineAttack_TellHold_AnimSpeed = 1.2f;

	// Token: 0x040006BA RID: 1722
	protected float m_lineAttack_TellIntroAndHold_Delay = 1.1f;

	// Token: 0x040006BB RID: 1723
	protected float m_lineAttack_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x040006BC RID: 1724
	protected float m_lineAttack_AttackIntro_Delay;

	// Token: 0x040006BD RID: 1725
	protected float m_lineAttack_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x040006BE RID: 1726
	protected float m_lineAttack_AttackHold_Delay;

	// Token: 0x040006BF RID: 1727
	protected float m_lineAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x040006C0 RID: 1728
	protected float m_lineAttack_Exit_Delay = 0.55f;

	// Token: 0x040006C1 RID: 1729
	protected float m_lineAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x040006C2 RID: 1730
	protected float m_lineAttack_AttackCD = 12f;

	// Token: 0x040006C3 RID: 1731
	protected float m_lineAttack_AttackExitDelay = 0.25f;

	// Token: 0x040006C4 RID: 1732
	protected const string HAMMER_ATTACK_TELL_INTRO = "HammerAttack_Tell_Intro";

	// Token: 0x040006C5 RID: 1733
	protected const string HAMMER_ATTACK_TELL_HOLD = "HammerAttack_Tell_Hold";

	// Token: 0x040006C6 RID: 1734
	protected const string HAMMER_ATTACK_ATTACK_INTRO = "HammerAttack_Attack_Intro";

	// Token: 0x040006C7 RID: 1735
	protected const string HAMMER_ATTACK_ATTACK_HOLD = "HammerAttack_Attack_Hold";

	// Token: 0x040006C8 RID: 1736
	protected const string HAMMER_ATTACK_EXIT = "HammerAttack_Exit";

	// Token: 0x040006C9 RID: 1737
	protected const string HAMMER_ATTACK_PROJECTILE = "CaveBossSlashProjectile";

	// Token: 0x040006CA RID: 1738
	protected const string HAMMER_ATTACK_EXPLOSION_PROJECTILE = "CaveBossSlashExplosionProjectile";

	// Token: 0x040006CB RID: 1739
	protected const string HAMMER_ATTACK_ROLLING_PROJECTILE = "CaveBossRollingProjectile";

	// Token: 0x040006CC RID: 1740
	protected const string HAMMER_ATTACK_ROLLING_ADVANCED_PROJECTILE = "CaveBossRollingAdvancedProjectile";

	// Token: 0x040006CD RID: 1741
	protected const string HAMMER_ATTACK_VARIANT_PROJECTILE = "CaveBossSlashFireballProjectile";

	// Token: 0x040006CE RID: 1742
	protected float m_hammerAttack_TellIntro_AnimSpeed = 0.8f;

	// Token: 0x040006CF RID: 1743
	protected float m_hammerAttack_TellHold_AnimSpeed = 0.8f;

	// Token: 0x040006D0 RID: 1744
	protected float m_hammerAttack_TellIntroAndHold_Delay = 0.95f;

	// Token: 0x040006D1 RID: 1745
	protected float m_hammerAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x040006D2 RID: 1746
	protected float m_hammerAttack_AttackIntro_Delay;

	// Token: 0x040006D3 RID: 1747
	protected float m_hammerAttack_AttackHold_AnimSpeed = 1f;

	// Token: 0x040006D4 RID: 1748
	protected float m_hammerAttack_AttackHold_Delay;

	// Token: 0x040006D5 RID: 1749
	protected float m_hammerAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x040006D6 RID: 1750
	protected float m_hammerAttack_Exit_Delay = 0.45f;

	// Token: 0x040006D7 RID: 1751
	protected float m_hammerAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x040006D8 RID: 1752
	protected float m_hammerAttack_AttackCD = 12f;

	// Token: 0x040006D9 RID: 1753
	protected float m_hammerAttack_AttackExitDelay = 0.25f;

	// Token: 0x040006DA RID: 1754
	protected int m_hammerAttackVariantProjectileCount = 9;

	// Token: 0x040006DB RID: 1755
	protected const string JUMP_ATTACK_TELL_INTRO = "JumpAttack_Tell_Intro";

	// Token: 0x040006DC RID: 1756
	protected const string JUMP_ATTACK_TELL_HOLD = "JumpAttack_Tell_Hold";

	// Token: 0x040006DD RID: 1757
	protected const string JUMP_ATTACK_JUMP_INTRO = "JumpAttack_Jump_Intro_Intro";

	// Token: 0x040006DE RID: 1758
	protected const string JUMP_ATTACK_JUMP_HOLD = "JumpAttack_Jump_Hold";

	// Token: 0x040006DF RID: 1759
	protected const string JUMP_ATTACK_FALL_INTRO = "JumpAttack_Fall_Intro";

	// Token: 0x040006E0 RID: 1760
	protected const string JUMP_ATTACK_FALL_HOLD = "JumpAttack_Fall_Hold";

	// Token: 0x040006E1 RID: 1761
	protected const string JUMP_ATTACK_LAND_INTRO = "JumpAttack_Land_Intro";

	// Token: 0x040006E2 RID: 1762
	protected const string JUMP_ATTACK_LAND_HOLD = "JumpAttack_Land_Hold";

	// Token: 0x040006E3 RID: 1763
	protected const string JUMP_ATTACK_EXIT = "JumpAttack_Exit";

	// Token: 0x040006E4 RID: 1764
	protected const string JUMP_ATTACK_MAGMA_PROJECTILE = "CaveBossFallingRubbleProjectile";

	// Token: 0x040006E5 RID: 1765
	protected const string JUMP_VARIANT_PROJECTILE = "CaveBossJumpExplosionProjectile";

	// Token: 0x040006E6 RID: 1766
	protected float m_jump_TellIntro_AnimSpeed = 0.8f;

	// Token: 0x040006E7 RID: 1767
	protected float m_jump_TellHold_AnimSpeed = 0.8f;

	// Token: 0x040006E8 RID: 1768
	protected float m_jump_TellIntroAndHold_Delay = 1f;

	// Token: 0x040006E9 RID: 1769
	protected float m_jumpAttack_TellIntro_AnimSpeed = 1f;

	// Token: 0x040006EA RID: 1770
	protected float m_jumpAttack_TellHold_AnimSpeed = 1f;

	// Token: 0x040006EB RID: 1771
	protected float m_jumpAttack_TellIntroAndHold_Delay;

	// Token: 0x040006EC RID: 1772
	protected float m_jumpAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x040006ED RID: 1773
	protected float m_jumpAttack_AttackHold_AnimSpeed = 1f;

	// Token: 0x040006EE RID: 1774
	protected float m_jumpAttack_Fall_Intro_AnimSpeed = 1f;

	// Token: 0x040006EF RID: 1775
	protected float m_jumpAttack_Fall_Hold_AnimSpeed = 1f;

	// Token: 0x040006F0 RID: 1776
	protected float m_jumpAttack_Land_Intro_AnimSpeed = 1f;

	// Token: 0x040006F1 RID: 1777
	protected float m_jumpAttack_Land_Hold_AnimSpeed = 1f;

	// Token: 0x040006F2 RID: 1778
	protected float m_jumpAttack_LandHold_Delay = 1f;

	// Token: 0x040006F3 RID: 1779
	protected float m_jumpAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x040006F4 RID: 1780
	protected float m_jumpAttack_Exit_Delay = 0.45f;

	// Token: 0x040006F5 RID: 1781
	protected float m_jumpAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x040006F6 RID: 1782
	protected float m_jumpAttack_AttackCD = 12f;

	// Token: 0x040006F7 RID: 1783
	protected float m_jumpAttack_AttackExitDelay = 0.25f;

	// Token: 0x040006F8 RID: 1784
	protected float m_jumpAttack_VariantAttackDelay = 0.5f;

	// Token: 0x040006F9 RID: 1785
	protected Vector2 m_jumpVelocity = new Vector2(10f, 32f);

	// Token: 0x040006FA RID: 1786
	protected Vector2 m_jumpAttackMagmaBallSpawnOffset = new Vector2(-15f, 15f);

	// Token: 0x040006FB RID: 1787
	protected const string SUMMON_ATTACK_TELL_INTRO = "SummonEnemy_Tell_Intro";

	// Token: 0x040006FC RID: 1788
	protected const string SUMMON_ATTACK_TELL_HOLD = "SummonEnemy_Tell_Hold";

	// Token: 0x040006FD RID: 1789
	protected const string SUMMON_ATTACK_ATTACK_INTRO = "SummonEnemy_Attack_Intro";

	// Token: 0x040006FE RID: 1790
	protected const string SUMMON_ATTACK_ATTACK_HOLD = "SummonEnemy_Attack_Hold";

	// Token: 0x040006FF RID: 1791
	protected const string SUMMON_ATTACK_EXIT = "SummonEnemy_Exit";

	// Token: 0x04000700 RID: 1792
	protected float m_summonAttack_TellIntro_AnimSpeed = 0.6f;

	// Token: 0x04000701 RID: 1793
	protected float m_summonAttack_TellHold_AnimSpeed = 0.6f;

	// Token: 0x04000702 RID: 1794
	protected float m_summonAttack_TellIntroAndHold_Delay = 1.1f;

	// Token: 0x04000703 RID: 1795
	protected float m_summonAttack_AttackIntro_AnimSpeed = 0.6f;

	// Token: 0x04000704 RID: 1796
	protected float m_summonAttack_AttackIntro_Delay;

	// Token: 0x04000705 RID: 1797
	protected float m_summonAttack_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000706 RID: 1798
	protected float m_summonAttack_AttackHold_Delay = 1.15f;

	// Token: 0x04000707 RID: 1799
	protected float m_summonAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000708 RID: 1800
	protected float m_summonAttack_Exit_Delay = 0.45f;

	// Token: 0x04000709 RID: 1801
	protected float m_summonAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x0400070A RID: 1802
	protected float m_summonAttack_AttackCD = 25f;

	// Token: 0x0400070B RID: 1803
	private Projectile_RL m_summonAttack_WarningProjectile;

	// Token: 0x0400070C RID: 1804
	private Projectile_RL m_summonAttack_ShoutProjectile;

	// Token: 0x0400070D RID: 1805
	private float m_requiredHPDropToSummon;

	// Token: 0x0400070E RID: 1806
	private float HorizontalRoomIndent = 5f;

	// Token: 0x0400070F RID: 1807
	private float VerticalRoomIndent = 5f;

	// Token: 0x04000710 RID: 1808
	private bool m_summonsHaveFreeHitStatusEffect = true;

	// Token: 0x04000711 RID: 1809
	private const float m_hpReducRequiredBeforeSummoning = 0.21f;

	// Token: 0x04000712 RID: 1810
	private float m_summonFreeHitRegenerationAmount = 999f;

	// Token: 0x04000713 RID: 1811
	protected const string CHASE_ATTACK_TELL_INTRO = "ChaserProjectile_Tell_Intro";

	// Token: 0x04000714 RID: 1812
	protected const string CHASE_ATTACK_TELL_HOLD = "ChaserProjectile_Tell_Hold";

	// Token: 0x04000715 RID: 1813
	protected const string CHASE_ATTACK_ATTACK_INTRO = "ChaserProjectile_Attack_Intro";

	// Token: 0x04000716 RID: 1814
	protected const string CHASE_ATTACK_ATTACK_HOLD = "ChaserProjectile_Attack_Hold";

	// Token: 0x04000717 RID: 1815
	protected const string CHASE_ATTACK_EXIT = "ChaserProjectile_Exit";

	// Token: 0x04000718 RID: 1816
	protected const string CHASE_ATTACK_PROJECTILE = "CaveBossBombBoltProjectile";

	// Token: 0x04000719 RID: 1817
	protected const string CHASE_ATTACK_FLOOR_WARNING_PROJECTILE = "CaveBossFloorWarningProjectile";

	// Token: 0x0400071A RID: 1818
	protected const string CHASE_ATTACK_FLOOR_BOLT_PROJECTILE = "CaveBossFloorBoltProjectile";

	// Token: 0x0400071B RID: 1819
	protected const string CHASE_ATTACK_VERTICAL_BOLT_PROJECTILE = "CaveBossVerticalBoltProjectile";

	// Token: 0x0400071C RID: 1820
	protected float m_chaseAttack_TellIntro_AnimSpeed = 0.8f;

	// Token: 0x0400071D RID: 1821
	protected float m_chaseAttack_TellHold_AnimSpeed = 0.8f;

	// Token: 0x0400071E RID: 1822
	protected float m_chaseAttack_TellIntroAndHold_Delay = 1.25f;

	// Token: 0x0400071F RID: 1823
	protected float m_chaseAttack_AttackIntro_AnimSpeed = 0.8f;

	// Token: 0x04000720 RID: 1824
	protected float m_chaseAttack_AttackIntro_Delay;

	// Token: 0x04000721 RID: 1825
	protected float m_chaseAttack_AttackHold_AnimSpeed = 0.8f;

	// Token: 0x04000722 RID: 1826
	protected float m_chaseAttack_AttackHold_Delay = 1.25f;

	// Token: 0x04000723 RID: 1827
	protected float m_chaseAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000724 RID: 1828
	protected float m_chaseAttack_Exit_Delay = 0.45f;

	// Token: 0x04000725 RID: 1829
	protected float m_chaseAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x04000726 RID: 1830
	protected float m_chaseAttack_AttackCD = 12f;

	// Token: 0x04000727 RID: 1831
	private Projectile_RL m_chaseAttack_WarningProjectile;

	// Token: 0x04000728 RID: 1832
	private Projectile_RL m_chaseAttack_BoltProjectile;

	// Token: 0x04000729 RID: 1833
	private Projectile_RL m_chaseAttack_WarningProjectile2;

	// Token: 0x0400072A RID: 1834
	private Projectile_RL m_chaseAttack_BoltProjectile2;

	// Token: 0x0400072B RID: 1835
	protected const string WAVE_ATTACK_PROJECTILE = "CaveBossWaveProjectile";

	// Token: 0x0400072C RID: 1836
	protected const string WAVE_ATTACK_SMALL_PROJECTILE = "CaveBossWaveSmallProjectile";

	// Token: 0x0400072D RID: 1837
	protected const string WAVE_ATTACK_WARNING_PROJECTILE = "CaveBossWaveWarningProjectile";

	// Token: 0x0400072E RID: 1838
	protected float m_waveAttackWarningDuration = 1.5f;

	// Token: 0x0400072F RID: 1839
	protected float m_waveAttack_Initial_Delay = 1.5f;

	// Token: 0x04000730 RID: 1840
	private Projectile_RL m_waveAttack_WarningProjectile;

	// Token: 0x04000731 RID: 1841
	private Projectile_RL m_waveAttack_Projectile;

	// Token: 0x04000732 RID: 1842
	private Projectile_RL m_waveAttack_WarningProjectile2;

	// Token: 0x04000733 RID: 1843
	private Projectile_RL m_waveAttack_Projectile2;

	// Token: 0x04000734 RID: 1844
	private Coroutine m_waveAttack_PersistentCoroutine;

	// Token: 0x04000735 RID: 1845
	private Coroutine m_waveAttack_PersistentCoroutine2;

	// Token: 0x04000736 RID: 1846
	private bool m_persistentWaveAttackRunning;

	// Token: 0x04000737 RID: 1847
	private bool m_persistentWaveAttackRunning2;

	// Token: 0x04000738 RID: 1848
	public UnityEvent ModeShiftEvent;

	// Token: 0x04000739 RID: 1849
	private static List<EnemyType> m_enemySummonListHelper = new List<EnemyType>();

	// Token: 0x0400073A RID: 1850
	protected const string MODESHIFT_DOWNED = "ModeShift_Intro";

	// Token: 0x0400073B RID: 1851
	protected const string MODESHIFT_GETUP = "ModeShift_GetUp";

	// Token: 0x0400073C RID: 1852
	protected const string MODESHIFT_ATTACK_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x0400073D RID: 1853
	protected const string MODESHIFT_ATTACK_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x0400073E RID: 1854
	protected const string MODESHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x0400073F RID: 1855
	protected const string MODESHIFT_PROJECTILE_WARNING = "CaveBossShoutWarningProjectile";

	// Token: 0x04000740 RID: 1856
	protected const string MODESHIFT_PROJECTILE_ATTACK = "CaveBossShoutAttackProjectile";

	// Token: 0x04000741 RID: 1857
	protected const string MODESHIFT_PROJECTILE_SUMMON = "CaveBossInfiniteChaseProjectile";

	// Token: 0x04000742 RID: 1858
	protected const string MODESHIFT_PROJECTILE_SUMMON_VARIANT = "CaveBossInfiniteChaseProjectileVariant";

	// Token: 0x04000743 RID: 1859
	protected const string MODESHIFT_PROJECTILE_STATIC_WALL = "CaveBossStaticWallProjectile";

	// Token: 0x04000744 RID: 1860
	protected float m_modeShift_Downed_AnimSpeed = 0.8f;

	// Token: 0x04000745 RID: 1861
	protected float m_modeShift_Downed_Delay = 1.75f;

	// Token: 0x04000746 RID: 1862
	protected float m_modeShift_GetUp_AnimSpeed = 0.8f;

	// Token: 0x04000747 RID: 1863
	protected float m_modeShift_GetUp_Delay = 1.5f;

	// Token: 0x04000748 RID: 1864
	protected float m_modeShift_AttackIntro_AnimSpeed = 0.8f;

	// Token: 0x04000749 RID: 1865
	protected float m_modeShift_AttackIntro_Delay;

	// Token: 0x0400074A RID: 1866
	protected float m_modeShift_AttackHold_AnimSpeed = 0.8f;

	// Token: 0x0400074B RID: 1867
	protected float m_modeShift_AttackHold_Delay;

	// Token: 0x0400074C RID: 1868
	protected float m_modeShift_Exit_AnimSpeed = 0.65f;

	// Token: 0x0400074D RID: 1869
	protected float m_modeShift_Exit_Delay;

	// Token: 0x0400074E RID: 1870
	protected float m_modeShift_Exit_IdleDuration = 0.15f;

	// Token: 0x0400074F RID: 1871
	protected float m_modeShift_AttackCD = 99f;

	// Token: 0x04000750 RID: 1872
	protected float m_modeShift_AttackExitDelay = 0.5f;

	// Token: 0x04000751 RID: 1873
	protected int m_modeShiftIndex;

	// Token: 0x04000752 RID: 1874
	protected float[] m_modeShiftLevels_Basic = new float[]
	{
		0.7f,
		0.35f
	};

	// Token: 0x04000753 RID: 1875
	protected float[] m_modeShiftLevels_Advanced = new float[]
	{
		0.75f,
		0.5f,
		0.25f
	};

	// Token: 0x04000754 RID: 1876
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x04000755 RID: 1877
	protected bool m_canSummonDuplicates;

	// Token: 0x04000756 RID: 1878
	private Projectile_RL m_modeShiftWarningProjectile;

	// Token: 0x04000757 RID: 1879
	private Projectile_RL m_modeShiftShoutProjectile;

	// Token: 0x04000758 RID: 1880
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x04000759 RID: 1881
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x0400075A RID: 1882
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x0400075B RID: 1883
	protected float m_spawn_Idle_Delay = 2f;

	// Token: 0x0400075C RID: 1884
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x0400075D RID: 1885
	protected float m_spawn_Intro_Delay;

	// Token: 0x0400075E RID: 1886
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x0400075F RID: 1887
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x04000760 RID: 1888
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04000761 RID: 1889
	protected float m_death_Intro_Delay;

	// Token: 0x04000762 RID: 1890
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04000763 RID: 1891
	protected float m_death_Hold_Delay = 4.5f;
}
