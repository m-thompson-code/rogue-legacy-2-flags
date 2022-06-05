using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class EyeballBoss_Basic_AIScript : Eyeball_Basic_AIScript, IAudioEventEmitter
{
	// Token: 0x170002AE RID: 686
	// (get) Token: 0x0600066A RID: 1642 RVA: 0x000053F3 File Offset: 0x000035F3
	// (set) Token: 0x0600066B RID: 1643 RVA: 0x000053FB File Offset: 0x000035FB
	public bool StartsDisabled { get; set; }

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x0600066C RID: 1644 RVA: 0x00005404 File Offset: 0x00003604
	// (set) Token: 0x0600066D RID: 1645 RVA: 0x0000540C File Offset: 0x0000360C
	public bool DisableModeshift { get; set; }

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x0600066E RID: 1646 RVA: 0x00005415 File Offset: 0x00003615
	protected bool IsKhidr
	{
		get
		{
			return base.EnemyController && base.EnemyController.EnemyRank == EnemyRank.Expert && base.EnemyController.EnemyType == EnemyType.EyeballBoss_Middle;
		}
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00059E50 File Offset: 0x00058050
	public static bool CanFire()
	{
		for (int i = 0; i < EyeballBoss_Basic_AIScript.m_eyeballSyncList_STATIC.Count; i++)
		{
			if (!EyeballBoss_Basic_AIScript.m_eyeballSyncList_STATIC[i])
			{
				EyeballBoss_Basic_AIScript.m_eyeballSyncList_STATIC.RemoveAt(i);
				i--;
			}
			else if (!EyeballBoss_Basic_AIScript.m_eyeballSyncList_STATIC[i].IsReady && !EyeballBoss_Basic_AIScript.m_eyeballSyncList_STATIC[i].EnemyController.IsDead)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06000670 RID: 1648 RVA: 0x00005446 File Offset: 0x00003646
	// (set) Token: 0x06000671 RID: 1649 RVA: 0x0000544E File Offset: 0x0000364E
	public bool IsReady { get; private set; }

	// Token: 0x06000672 RID: 1650 RVA: 0x00059EC4 File Offset: 0x000580C4
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"EyeballBoltMinibossProjectile",
			"EyeballBounceBoltMinibossProjectile",
			"EyeballExplosionProjectile",
			"EyeballPassLineBoltProjectile",
			"EyeballPassLineBoltPrimeProjectile",
			"EyeballWarningProjectile",
			"EyeballCurseBoltProjectile",
			"EyeballCurseBoltBlueProjectile"
		};
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00005457 File Offset: 0x00003657
	private void Awake()
	{
		this.m_onPlayerEnterRoom = new Action<object, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onBossHit = new Action<object, HealthChangeEventArgs>(this.OnBossHit);
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x00059F20 File Offset: 0x00058120
	private void OnEnable()
	{
		this.IsReady = false;
		this.m_modeShifted = false;
		EyeballBoss_Basic_AIScript.m_modeShiftWhiteFlashPlayed = false;
		if (!EyeballBoss_Basic_AIScript.m_eyeballSyncList_STATIC.Contains(this))
		{
			EyeballBoss_Basic_AIScript.m_eyeballSyncList_STATIC.Add(this);
		}
		if (base.IsInitialized)
		{
			this.InitializeModeShiftEye();
			if (this.IsKhidr)
			{
				Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			}
		}
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x00059F7C File Offset: 0x0005817C
	protected override void OnDisable()
	{
		base.OnDisable();
		AudioManager.Stop(this.m_burstShotPrepLoopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_bounceShotPrepLoopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (EyeballBoss_Basic_AIScript.m_eyeballSyncList_STATIC.Contains(this))
		{
			EyeballBoss_Basic_AIScript.m_eyeballSyncList_STATIC.Remove(this);
		}
		this.DisableModeshift = false;
		this.StartsDisabled = false;
		if (this.IsKhidr)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}
		if (!GameManager.IsApplicationClosing && base.Animator)
		{
			base.Animator.WriteDefaultValues();
		}
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0005A004 File Offset: 0x00058204
	private void OnDestroy()
	{
		if (base.EnemyController)
		{
			base.EnemyController.HealthChangeRelay.RemoveListener(this.m_onBossHit);
		}
		if (this.m_burstShotPrepLoopInstance.isValid())
		{
			this.m_burstShotPrepLoopInstance.release();
		}
		if (this.m_bounceShotPrepLoopInstance.isValid())
		{
			this.m_bounceShotPrepLoopInstance.release();
		}
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0005A068 File Offset: 0x00058268
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.InitializeModeShiftEye();
		base.EnemyController.HealthChangeRelay.AddListener(this.m_onBossHit, false);
		switch (this.m_eyeballType)
		{
		case EyeballBoss_Basic_AIScript.EyeballType.Left:
			base.Animator.SetInteger("EyeballPosition", 0);
			this.m_burstShotPrepLoopInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_eyeballBoss_shootBlue_prepLoop_left", base.transform);
			break;
		case EyeballBoss_Basic_AIScript.EyeballType.Right:
			base.Animator.SetInteger("EyeballPosition", 2);
			this.m_burstShotPrepLoopInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_eyeballBoss_shootBlue_prepLoop_right", base.transform);
			break;
		case EyeballBoss_Basic_AIScript.EyeballType.Bottom:
			base.Animator.SetInteger("EyeballPosition", 1);
			this.m_burstShotPrepLoopInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_eyeballBoss_shootBlue_prepLoop_center", base.transform);
			break;
		case EyeballBoss_Basic_AIScript.EyeballType.Middle:
			base.Animator.SetInteger("EyeballPosition", 1);
			this.m_burstShotPrepLoopInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_eyeballBoss_shootBlue_prepLoop_center", base.transform);
			break;
		}
		this.m_bounceShotPrepLoopInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_eyeballBoss_shootBounce_prepLoop_center", base.transform);
		if (this.IsKhidr)
		{
			this.OnPlayerEnterRoom(null, null);
		}
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x0005A184 File Offset: 0x00058384
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		if (!base.IsInitialized)
		{
			return;
		}
		base.EnemyController.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
		base.EnemyController.Heading = Vector2.zero;
		base.EnemyController.FollowTarget = false;
		base.Animator.SetBool("Centered", true);
		base.Animator.Play(BaseAIScript.NEUTRAL_STATE, 0, 1f);
		if (!base.EnemyController.CharacterCorgi.IsFacingRight)
		{
			base.EnemyController.CharacterCorgi.Flip(true, true);
		}
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0005A228 File Offset: 0x00058428
	private void InitializeModeShiftEye()
	{
		if (this.StartsDisabled)
		{
			base.EnemyController.TakesNoDamage = true;
			base.EnemyController.StatusBarController.Active = false;
			base.EnemyController.LogicController.enabled = false;
			base.EnemyController.LogicController.DisableLogicActivationByDistance = true;
			base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = true;
		}
		base.LogicController.ForceExecuteLogicBlockName_OnceOnly = null;
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0005A29C File Offset: 0x0005849C
	private void OnBossHit(object sender, HealthChangeEventArgs args)
	{
		if (this.m_modeShifted)
		{
			return;
		}
		if (base.EnemyController.IsDead)
		{
			return;
		}
		if (this.DisableModeshift)
		{
			return;
		}
		if (args.PrevHealthValue <= args.NewHealthValue && !this.StartsDisabled)
		{
			return;
		}
		if (base.EnemyController.CurrentHealth <= (float)base.EnemyController.ActualMaxHealth * 0.5f)
		{
			this.m_modeShifted = true;
			if (this.StartsDisabled)
			{
				if (base.EnemyController.CurrentHealth > 0f)
				{
					base.EnemyController.TakesNoDamage = false;
					base.EnemyController.StatusBarController.Active = true;
					base.EnemyController.LogicController.enabled = true;
					base.EnemyController.LogicController.DisableLogicActivationByDistance = false;
					base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "Mode_Shift_OpenEye";
				}
				else
				{
					base.EnemyController.KillCharacter(PlayerManager.GetPlayerController().gameObject, true);
				}
				if (this.m_modeShiftEventArgs == null)
				{
					this.m_modeShiftEventArgs = new EnemyModeShiftEventArgs(base.EnemyController);
				}
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EnemyModeShift, this, this.m_modeShiftEventArgs);
				return;
			}
			base.LogicController.StopAllLogic(true);
			base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "Mode_Shift";
		}
	}

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x0600067B RID: 1659 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ZoneBlast_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x0600067C RID: 1660 RVA: 0x00004565 File Offset: 0x00002765
	protected virtual float ZoneBlast_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x0600067D RID: 1661 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ZoneBlast_TellHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x0600067E RID: 1662 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ZoneBlast_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x0600067F RID: 1663 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ZoneBlast_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06000680 RID: 1664 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ZoneBlast_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06000681 RID: 1665 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ZoneBlast_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06000682 RID: 1666 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ZoneBlast_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06000683 RID: 1667 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ZoneBlast_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06000684 RID: 1668 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float ZoneBlast_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06000685 RID: 1669 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float ZoneBlast_Exit_AttackCD
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06000686 RID: 1670 RVA: 0x00004762 File Offset: 0x00002962
	protected virtual int ZoneBlast_Attack_Amount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06000687 RID: 1671 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ZoneBlast_AttackHold_DelayBetweenShots
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06000688 RID: 1672 RVA: 0x0000547D File Offset: 0x0000367D
	protected virtual float ZoneBlast_Attack_AngleSpread
	{
		get
		{
			return 75f;
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06000689 RID: 1673 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool ZoneBlast_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x0600068A RID: 1674 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool ZoneBlast_Variant
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x0600068B RID: 1675 RVA: 0x00005484 File Offset: 0x00003684
	public IRelayLink ShoutAttackWarningAppearedRelay
	{
		get
		{
			return this.m_shoutAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x0600068C RID: 1676 RVA: 0x00005491 File Offset: 0x00003691
	public IRelayLink ShoutAttackExplodedRelay
	{
		get
		{
			return this.m_shoutAttackExplodedRelay.link;
		}
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x0000549E File Offset: 0x0000369E
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ZoneBlast()
	{
		string TELL_INTRO = "AreaShot_Tell_Intro";
		string TELL_HOLD = "AreaShot_Tell_Hold";
		string ATTACK_INTRO = "AreaShot_Attack_Intro";
		string ATTACK_HOLD = "AreaShot_Attack_Hold";
		string EXIT = "AreaShot_Exit";
		int randomMove = UnityEngine.Random.Range(0, 100);
		int OddSplitter = 60;
		if (this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Bottom || randomMove <= OddSplitter)
		{
			TELL_INTRO = "Explosion_Tell_Intro";
			TELL_HOLD = "Explosion_Tell_Hold";
			ATTACK_INTRO = "Explosion_Attack_Intro";
			ATTACK_HOLD = "Explosion_Attack_Hold";
			EXIT = "Explosion_Exit";
		}
		if (this.ZoneBlast_Sync)
		{
			this.IsReady = true;
			while (!EyeballBoss_Basic_AIScript.CanFire())
			{
				yield return null;
			}
		}
		if (this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Bottom || randomMove <= OddSplitter)
		{
			this.m_shoutWarningProjectile = this.FireProjectile("EyeballWarningProjectile", 0, false, 0f, 1f, true, true, true);
		}
		base.EnemyController.Animator.SetBool("Centered", true);
		yield return this.Default_TellIntroAndLoop(TELL_INTRO, this.ZoneBlast_TellIntro_AnimSpeed, TELL_HOLD, this.ZoneBlast_TellHold_AnimSpeed, this.ZoneBlast_TellHold_Duration);
		yield return this.Default_Animation(ATTACK_INTRO, this.ZoneBlast_AttackIntro_AnimSpeed, this.ZoneBlast_AttackIntro_Delay, true);
		CDGHelper.VectorToAngle(base.EnemyController.Heading);
		yield return this.ChangeAnimationState(ATTACK_HOLD);
		this.SetAnimationSpeedMultiplier(this.ZoneBlast_AttackHold_AnimSpeed);
		if (this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Bottom || randomMove <= OddSplitter)
		{
			this.m_shoutWarningProjectile.transform.position = base.EnemyController.Midpoint;
			this.m_shoutWarningProjectile.transform.SetParent(base.EnemyController.transform, true);
			this.m_shoutAttackWarningAppearedRelay.Dispatch();
			base.StopProjectile(ref this.m_shoutWarningProjectile);
			this.m_shoutAttackExplodedRelay.Dispatch();
			this.FireProjectile("EyeballExplosionProjectile", 0, false, 0f, 1f, true, true, true);
			if (this.ZoneBlast_Variant && this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Middle)
			{
				float num = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
				float num2 = 15f;
				float num3 = 24f;
				Vector2 vector = new Vector2(-9f, 9f);
				float speedMod = 1f;
				if (base.LogicController.EnemyLogicType != EnemyLogicType.Expert)
				{
					int num4 = 0;
					while ((float)num4 < num2)
					{
						float num5 = UnityEngine.Random.Range(vector.x, vector.y);
						this.FireProjectile("EyeballBoltMinibossProjectile", 0, false, num + num5 + num3 * (float)num4, speedMod, true, true, true);
						num4++;
					}
				}
			}
		}
		else
		{
			string projectileName = "EyeballPassLineBoltProjectile";
			if (this.ZoneBlast_Variant && this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Middle)
			{
				projectileName = "EyeballPassLineBoltPrimeProjectile";
			}
			this.FireProjectile(projectileName, 0, false, 0f, 1f, true, true, true);
			this.FireProjectile(projectileName, 0, false, 30f, 1f, true, true, true);
			this.FireProjectile(projectileName, 0, false, 60f, 1f, true, true, true);
			this.FireProjectile(projectileName, 0, false, 90f, 1f, true, true, true);
			this.FireProjectile(projectileName, 0, false, 120f, 1f, true, true, true);
			this.FireProjectile(projectileName, 0, false, 150f, 1f, true, true, true);
			this.FireProjectile(projectileName, 0, false, 180f, 1f, true, true, true);
			this.FireProjectile(projectileName, 0, false, 210f, 1f, true, true, true);
			this.FireProjectile(projectileName, 0, false, 240f, 1f, true, true, true);
			this.FireProjectile(projectileName, 0, false, 270f, 1f, true, true, true);
			this.FireProjectile(projectileName, 0, false, 300f, 1f, true, true, true);
			this.FireProjectile(projectileName, 0, false, 330f, 1f, true, true, true);
		}
		yield return this.Default_Animation(EXIT, this.ZoneBlast_Exit_AnimSpeed, this.ZoneBlast_Exit_Duration, true);
		base.EnemyController.Animator.SetBool("Centered", false);
		yield return this.Default_Attack_Cooldown(this.ZoneBlast_Exit_ForceIdle, this.ZoneBlast_Exit_AttackCD);
		this.IsReady = false;
		yield break;
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x0600068E RID: 1678 RVA: 0x00004A07 File Offset: 0x00002C07
	protected virtual int Spinning_Fireball_Amount
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x0600068F RID: 1679 RVA: 0x000054AD File Offset: 0x000036AD
	protected virtual int SpinngFireball_Angle
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06000690 RID: 1680 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool SpinningFireball_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06000691 RID: 1681 RVA: 0x000054B1 File Offset: 0x000036B1
	protected virtual int Spinning_Fireball_Variant_Amount
	{
		get
		{
			return 15;
		}
	}

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x06000692 RID: 1682 RVA: 0x000054B5 File Offset: 0x000036B5
	protected virtual int SpinngFireball_Variant_Angle
	{
		get
		{
			return 16;
		}
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06000693 RID: 1683 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool SpinningFireball_Variant_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06000694 RID: 1684 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool SpinningFireball_Variant
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x000054B9 File Offset: 0x000036B9
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public override IEnumerator ShootSpinningFireball()
	{
		if (this.SpinningFireball_Sync)
		{
			this.IsReady = true;
			while (!EyeballBoss_Basic_AIScript.CanFire())
			{
				yield return null;
			}
		}
		base.EnemyController.Animator.SetBool("Centered", true);
		AudioManager.PlayAttached(this, this.m_burstShotPrepLoopInstance, base.gameObject);
		yield return this.Default_TellIntroAndLoop("BurstShot_Tell_Intro", this.MultiShot_TellIntro_AnimSpeed, "BurstShot_Tell_Hold", this.MultiShot_TellHold_AnimSpeed, this.MultiShot_TellHold_Duration);
		yield return this.Default_Animation("BurstShot_Attack_Intro", this.MultiShot_AttackIntro_AnimSpeed, this.MultiShot_AttackIntro_Delay, true);
		CDGHelper.VectorToAngle(base.EnemyController.Heading);
		yield return this.ChangeAnimationState("BurstShot_Attack_Hold");
		this.SetAnimationSpeedMultiplier(this.MultiShot_AttackHold_AnimSpeed);
		float num = (float)UnityEngine.Random.Range(0, 2);
		int flipper = -1;
		if (num >= 1f)
		{
			flipper = 1;
		}
		int spinAngle = this.SpinngFireball_Angle;
		int num2 = this.Spinning_Fireball_Amount;
		if (this.SpinningFireball_Variant && this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Middle)
		{
			spinAngle = this.Spinning_Fireball_Variant_Amount;
			num2 = this.Spinning_Fireball_Variant_Amount;
		}
		int SpinningFireballCycles = spinAngle * num2;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert && this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Middle)
		{
			SpinningFireballCycles += spinAngle * 8;
		}
		for (int i = 0; i <= SpinningFireballCycles; i += spinAngle)
		{
			this.FireProjectile("EyeballBoltMinibossProjectile", 0, false, (float)(i * flipper), 1f, true, true, true);
			this.FireProjectile("EyeballBoltMinibossProjectile", 0, false, (float)(180 + i * flipper), 1f, true, true, true);
			if (this.SpinningFireball_Variant && this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Middle && base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
			{
				yield return base.Wait(0.125f, false);
			}
			else
			{
				yield return base.Wait(0.175f, false);
			}
		}
		yield return base.Wait(this.MultiShot_AttackHold_Delay, false);
		base.EnemyController.Animator.SetBool("Centered", false);
		AudioManager.Stop(this.m_burstShotPrepLoopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		yield return this.Default_Animation("BurstShot_Exit", this.MultiShot_Exit_AnimSpeed, this.MultiShot_Exit_Duration, true);
		yield return this.Default_Attack_Cooldown(this.MultiShot_Exit_ForceIdle, this.MultiShot_Exit_AttackCD);
		this.IsReady = false;
		yield break;
	}

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06000696 RID: 1686 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool HomingFireball_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06000697 RID: 1687 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool HomingFireball_Variant
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06000698 RID: 1688 RVA: 0x00004A6C File Offset: 0x00002C6C
	protected virtual float HomingFireball_Variant_SpeedMod
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06000699 RID: 1689 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool HomingFireball_Variant_Blue
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x000054C8 File Offset: 0x000036C8
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public override IEnumerator ShootHomingFireball()
	{
		string TELL_INTRO = "HomingShot_Tell_Intro";
		string TELL_HOLD = "HomingShot_Tell_Hold";
		string ATTACK_INTRO = "HomingShot_Attack_Intro";
		string ATTACK_HOLD = "HomingShot_Attack_Hold";
		string EXIT = "HomingShot_Exit";
		int num = UnityEngine.Random.Range(0, 2);
		bool centreEyeball = false;
		bool doingElectroAttack = this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Bottom || num == 1;
		if (doingElectroAttack)
		{
			TELL_INTRO = "BounceBullets_Tell_Intro";
			TELL_HOLD = "BounceBullets_Tell_Hold";
			ATTACK_INTRO = "BounceBullets_Attack_Intro";
			ATTACK_HOLD = "BounceBullets_Attack_Hold";
			EXIT = "BounceBullets_Exit";
		}
		if (!doingElectroAttack && this.IsKhidr)
		{
			TELL_INTRO = "BurstShot_Tell_Intro";
			TELL_HOLD = "BurstShot_Tell_Hold";
			ATTACK_INTRO = "BurstShot_Attack_Intro";
			ATTACK_HOLD = "BurstShot_Attack_Hold";
			EXIT = "BurstShot_Exit";
			centreEyeball = true;
		}
		if (this.HomingFireball_Sync)
		{
			this.IsReady = true;
			while (!EyeballBoss_Basic_AIScript.CanFire())
			{
				yield return null;
			}
		}
		if (doingElectroAttack)
		{
			AudioManager.PlayAttached(this, this.m_bounceShotPrepLoopInstance, base.gameObject);
		}
		if (centreEyeball)
		{
			base.Animator.SetBool("Centered", true);
		}
		else
		{
			base.EnemyController.FollowTarget = false;
		}
		float fireAngle = CDGHelper.VectorToAngle(base.EnemyController.Heading);
		yield return this.Default_TellIntroAndLoop(TELL_INTRO, this.SingleShot_TellIntro_AnimSpeed, TELL_HOLD, this.MultiShot_TellHold_AnimSpeed, this.MultiShot_TellHold_Duration);
		yield return this.Default_Animation(ATTACK_INTRO, this.SingleShot_AttackIntro_AnimSpeed, this.SingleShot_AttackIntro_Delay, true);
		string curseProjectileName = "EyeballCurseBoltProjectile";
		if (this.HomingFireball_Variant_Blue)
		{
			curseProjectileName = "EyeballCurseBoltBlueProjectile";
		}
		if (doingElectroAttack)
		{
			if (this.HomingFireball_Variant && this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Middle)
			{
				Projectile_RL projectile = this.FireProjectile("EyeballBounceBoltMinibossProjectile", 1, false, fireAngle, this.HomingFireball_Variant_SpeedMod, true, true, true);
				this.RotateProjectileToHeading(projectile);
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
				{
					yield return base.Wait(0.35f, false);
				}
				else
				{
					yield return base.Wait(0.75f, false);
				}
				projectile = this.FireProjectile("EyeballBounceBoltMinibossProjectile", 1, false, fireAngle, this.HomingFireball_Variant_SpeedMod, true, true, true);
				this.RotateProjectileToHeading(projectile);
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
				{
					yield return base.Wait(0.35f, false);
				}
				else
				{
					yield return base.Wait(0.75f, false);
				}
				projectile = this.FireProjectile("EyeballBounceBoltMinibossProjectile", 1, false, fireAngle, this.HomingFireball_Variant_SpeedMod, true, true, true);
				this.RotateProjectileToHeading(projectile);
			}
			else
			{
				Projectile_RL projectile = this.FireProjectile("EyeballBounceBoltMinibossProjectile", 1, false, fireAngle, 1f, true, true, true);
				this.RotateProjectileToHeading(projectile);
				yield return base.Wait(1.75f, false);
				projectile = this.FireProjectile("EyeballBounceBoltMinibossProjectile", 1, false, fireAngle, 1f, true, true, true);
				this.RotateProjectileToHeading(projectile);
			}
		}
		else if (this.HomingFireball_Variant && this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Middle)
		{
			float speedMod = 1f;
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
			{
				float fireAngle2 = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
				float shotcount = 10f;
				float angleSpread = 36f;
				Vector2 randomizer = new Vector2(-6f, 6f);
				speedMod = 1f;
				int num4;
				for (int i = 0; i < 3; i = num4 + 1)
				{
					int num2 = 0;
					while ((float)num2 < shotcount)
					{
						float num3 = UnityEngine.Random.Range(randomizer.x, randomizer.y);
						this.FireProjectile("EyeballBoltMinibossProjectile", 0, false, fireAngle2 + num3 + angleSpread * (float)num2, speedMod, true, true, true);
						num2++;
					}
					yield return base.Wait(0.8f, false);
					num4 = i;
				}
				randomizer = default(Vector2);
			}
			else
			{
				Projectile_RL projectile2 = this.FireProjectile(curseProjectileName, 1, false, fireAngle, speedMod, true, true, true);
				this.RotateProjectileToHeading(projectile2);
				yield return base.Wait(0.25f, false);
				projectile2 = this.FireProjectile(curseProjectileName, 1, false, fireAngle, speedMod, true, true, true);
				this.RotateProjectileToHeading(projectile2);
				yield return base.Wait(0.25f, false);
				projectile2 = this.FireProjectile(curseProjectileName, 1, false, fireAngle, speedMod, true, true, true);
				this.RotateProjectileToHeading(projectile2);
				yield return base.Wait(0.25f, false);
				projectile2 = this.FireProjectile(curseProjectileName, 1, false, fireAngle, speedMod, true, true, true);
				this.RotateProjectileToHeading(projectile2);
				yield return base.Wait(0.25f, false);
				projectile2 = this.FireProjectile(curseProjectileName, 1, false, fireAngle, speedMod, true, true, true);
				this.RotateProjectileToHeading(projectile2);
			}
		}
		else
		{
			Projectile_RL projectile2 = this.FireProjectile(curseProjectileName, 1, false, fireAngle, 1f, true, true, true);
			this.RotateProjectileToHeading(projectile2);
			yield return base.Wait(1.75f, false);
			projectile2 = this.FireProjectile(curseProjectileName, 1, false, fireAngle, 1f, true, true, true);
			this.RotateProjectileToHeading(projectile2);
		}
		yield return this.Default_Animation(ATTACK_HOLD, this.SingleShot_AttackHold_AnimSpeed, this.SingleShot_AttackHold_Delay, true);
		if (doingElectroAttack)
		{
			AudioManager.Stop(this.m_bounceShotPrepLoopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		yield return this.Default_Animation(EXIT, this.SingleShot_Exit_AnimSpeed, this.SingleShot_Exit_Duration, true);
		if (centreEyeball)
		{
			base.Animator.SetBool("Centered", false);
		}
		base.EnemyController.FollowTarget = true;
		yield return this.Default_Attack_Cooldown(this.SingleShot_Exit_ForceIdle, this.SingleShot_Exit_AttackCD);
		this.IsReady = false;
		yield break;
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x000054D7 File Offset: 0x000036D7
	public override IEnumerator SpawnAnim()
	{
		if (this.IsKhidr)
		{
			this.m_faceForward = true;
			float scale = EnemyClassLibrary.GetEnemyData(base.EnemyController.EnemyType, base.EnemyController.EnemyRank).Scale;
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/sfx_khidirBoss_intro_rl1Grow_layer", base.EnemyController.Midpoint);
			TweenManager.TweenBy(base.EnemyController.transform, 3f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
			{
				"localEulerAngles.z",
				-1800
			});
			yield return TweenManager.TweenTo(base.EnemyController.transform, 2f, new EaseDelegate(Ease.Bounce.EaseOut), new object[]
			{
				"delay",
				1,
				"localScale.x",
				scale,
				"localScale.y",
				scale
			}).TweenCoroutine;
			base.Animator.SetBool("Centered", false);
			this.m_faceForward = false;
		}
		else
		{
			this.m_faceForward = true;
			float delay;
			float introDelay;
			if (this.m_eyeballType == EyeballBoss_Basic_AIScript.EyeballType.Left)
			{
				delay = this.m_leftSpawn_Idle_Delay;
				introDelay = this.m_leftSpawn_Intro_Delay;
			}
			else
			{
				delay = this.m_rightSpawn_Idle_Delay;
				introDelay = this.m_rightSpawn_Intro_Delay;
			}
			yield return this.Default_Animation("Intro_Idle", this.m_spawn_Idle_AnimSpeed, delay, true);
			yield return this.Default_Animation("Closed_Moving", this.m_spawn_Intro_AnimSpeed, introDelay, true);
			this.m_faceForward = false;
			base.EnemyController.ForceFaceTarget();
			MusicManager.PlayMusic(SongID.TowerBossBGM_ASITP_Boss_3, false, false);
			yield return this.Default_Animation("Open", this.m_spawn_Open_AnimSpeed, this.m_spawn_Open_Delay, true);
			yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 0f, false);
		}
		yield break;
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x000054E6 File Offset: 0x000036E6
	public override IEnumerator DeathAnim()
	{
		if (this.IsKhidr)
		{
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/sfx_khidirBoss_death_rl1Layer", base.EnemyController.Midpoint);
		}
		if (!EyeballBoss_Basic_AIScript.m_deathWhiteFlashPlayed)
		{
			EyeballBoss_Basic_AIScript.m_deathWhiteFlashPlayed = true;
			yield return base.DeathAnim();
			this.StopAttackEffects();
			RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 0.5f);
			yield return this.Default_Animation("Death_Intro", this.m_death_Intro_AnimSpeed, this.m_death_Intro_Delay, true);
			RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
			base.EnemyController.GetComponent<BossModeShiftController>().DissolveMaterials(this.m_death_Hold_Delay);
			yield return this.Default_Animation("Death_Loop", this.m_death_Hold_AnimSpeed, this.m_death_Hold_Delay, true);
		}
		else
		{
			yield return this.Default_Animation("Death_Intro", this.m_death_Intro_AnimSpeed, this.m_death_Intro_Delay, true);
			base.EnemyController.GetComponent<BossModeShiftController>().DissolveMaterials(this.m_death_Hold_Delay);
			yield return this.Default_Animation("Death_Loop", this.m_death_Hold_AnimSpeed, this.m_death_Hold_Delay, true);
		}
		yield break;
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x000054F5 File Offset: 0x000036F5
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[WanderLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Mode_Shift()
	{
		base.RemoveStatusEffects(false);
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = true;
		this.StopAttackEffects();
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_Damage_Mod;
		if (!EyeballBoss_Basic_AIScript.m_modeShiftWhiteFlashPlayed)
		{
			EyeballBoss_Basic_AIScript.m_modeShiftWhiteFlashPlayed = true;
			yield return base.DeathAnim();
		}
		base.EnemyController.Animator.SetBool("Centered", false);
		base.EnemyController.FollowTarget = true;
		if (base.EnemyController.EnemyRank != EnemyRank.Basic)
		{
			yield return this.Default_Animation("ModeShift_Intro", 1f, 0f, true);
		}
		else
		{
			yield return this.Default_Animation("ModeShift_Intro", 1f, 3.25f, true);
		}
		base.EnemyController.Animator.SetBool("Centered", true);
		base.EnemyController.FollowTarget = false;
		yield return this.Default_Animation("ModeShift_Scream_Intro", 1f, 0f, true);
		if (base.EnemyController.EnemyRank != EnemyRank.Basic && !this.IsKhidr)
		{
			yield return this.Default_Animation("ModeShift_Scream_Hold", 1f, 10f, true);
		}
		else
		{
			yield return this.Default_Animation("ModeShift_Scream_Hold", 1f, 1.25f, true);
		}
		yield return this.Default_Animation("ModeShift_Scream_Exit", 1f, 0f, true);
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.ResetBaseValues();
		base.EnemyController.Animator.SetBool("Centered", false);
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		yield return this.Default_Attack_Cooldown(0.1f, 99999f);
		yield break;
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x00005504 File Offset: 0x00003704
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[WanderLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Mode_Shift_OpenEye()
	{
		base.RemoveStatusEffects(false);
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = true;
		base.EnemyController.Animator.SetBool("Centered", true);
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_Damage_Mod;
		if (!EyeballBoss_Basic_AIScript.m_modeShiftWhiteFlashPlayed)
		{
			EyeballBoss_Basic_AIScript.m_modeShiftWhiteFlashPlayed = true;
			yield return base.DeathAnim();
		}
		yield return this.Default_Animation("Intro_Idle", 1f, 0.8f, true);
		yield return this.Default_Animation("Closed_Moving", 1f, 1.4f, true);
		yield return this.Default_Animation("Open", 1f, 0.2f, true);
		yield return this.Default_Animation("ModeShift_Scream_Intro", 1f, 0f, true);
		yield return this.Default_Animation("ModeShift_Scream_Hold", 1f, 1.25f, true);
		yield return this.Default_Animation("ModeShift_Scream_Exit", 1f, 0f, true);
		base.EnemyController.TakesNoDamage = false;
		base.EnemyController.StatusBarController.Active = true;
		base.EnemyController.Animator.SetBool("Centered", false);
		base.EnemyController.FollowTarget = true;
		this.m_faceForward = false;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		base.EnemyController.ForceFaceTarget();
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(0.1f, 99999f);
		yield break;
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x00005513 File Offset: 0x00003713
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_shoutWarningProjectile);
		base.OnLBCompleteOrCancelled();
		this.IsReady = false;
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x0005A3D4 File Offset: 0x000585D4
	private void RotateProjectileToHeading(Projectile_RL projectile)
	{
		Vector3 position = projectile.transform.position;
		Vector3 vector = position;
		vector.x += 3f;
		Vector2 vector2 = CDGHelper.RotatedPoint(vector, position, CDGHelper.VectorToAngle(base.EnemyController.Heading));
		vector.x = vector2.x;
		vector.y = vector2.y;
		projectile.transform.position = vector;
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0000552E File Offset: 0x0000372E
	private void StopAttackEffects()
	{
		EffectManager.DisableAllEffectWithName("EyeballBossBlueFireballTell_Effect");
		EffectManager.DisableAllEffectWithName("EyeballBossBounceBulletsTell_Effect");
		EffectManager.DisableAllEffectWithName("EyeballBossCurseTell_Effect");
		AudioManager.Stop(this.m_burstShotPrepLoopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_bounceShotPrepLoopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x0005A448 File Offset: 0x00058648
	public override void ResetScript()
	{
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		string forceExecuteLogicBlockName_OnceOnly = base.LogicController.ForceExecuteLogicBlockName_OnceOnly;
		base.ResetScript();
		base.LogicController.ForceExecuteLogicBlockName_OnceOnly = forceExecuteLogicBlockName_OnceOnly;
		base.LogicController.DisableLogicActivationByDistance = true;
		EyeballBoss_Basic_AIScript.m_deathWhiteFlashPlayed = false;
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00005566 File Offset: 0x00003766
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		if (this.IsKhidr)
		{
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SummonRuleBroadcast, null, null);
		}
	}

	// Token: 0x040009BA RID: 2490
	[SerializeField]
	protected EyeballBoss_Basic_AIScript.EyeballType m_eyeballType;

	// Token: 0x040009BB RID: 2491
	protected const float PROJECTILE_SPAWN_X_OFFSET = 3f;

	// Token: 0x040009BC RID: 2492
	private static bool m_deathWhiteFlashPlayed = false;

	// Token: 0x040009BD RID: 2493
	private static bool m_modeShiftWhiteFlashPlayed = false;

	// Token: 0x040009BE RID: 2494
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x040009BF RID: 2495
	protected const string POSITION_ANIM_PARAM = "EyeballPosition";

	// Token: 0x040009C0 RID: 2496
	private EventInstance m_burstShotPrepLoopInstance;

	// Token: 0x040009C1 RID: 2497
	private EventInstance m_bounceShotPrepLoopInstance;

	// Token: 0x040009C2 RID: 2498
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x040009C3 RID: 2499
	private Action<object, HealthChangeEventArgs> m_onBossHit;

	// Token: 0x040009C6 RID: 2502
	protected static List<EyeballBoss_Basic_AIScript> m_eyeballSyncList_STATIC = new List<EyeballBoss_Basic_AIScript>();

	// Token: 0x040009C8 RID: 2504
	protected const string ZONE_BLAST_TELL_INTRO = "AreaShot_Tell_Intro";

	// Token: 0x040009C9 RID: 2505
	protected const string ZONE_BLAST_TELL_HOLD = "AreaShot_Tell_Hold";

	// Token: 0x040009CA RID: 2506
	protected const string ZONE_BLAST_ATTACK_INTRO = "AreaShot_Attack_Intro";

	// Token: 0x040009CB RID: 2507
	protected const string ZONE_BLAST_ATTACK_HOLD = "AreaShot_Attack_Hold";

	// Token: 0x040009CC RID: 2508
	protected const string ZONE_BLAST_EXIT = "AreaShot_Exit";

	// Token: 0x040009CD RID: 2509
	protected const string EXPERT_ZONE_BLAST_TELL_INTRO = "Explosion_Tell_Intro";

	// Token: 0x040009CE RID: 2510
	protected const string EXPERT_ZONE_BLAST_TELL_HOLD = "Explosion_Tell_Hold";

	// Token: 0x040009CF RID: 2511
	protected const string EXPERT_ZONE_BLAST_ATTACK_INTRO = "Explosion_Attack_Intro";

	// Token: 0x040009D0 RID: 2512
	protected const string EXPERT_ZONE_BLAST_ATTACK_HOLD = "Explosion_Attack_Hold";

	// Token: 0x040009D1 RID: 2513
	protected const string EXPERT_ZONE_BLAST_EXIT = "Explosion_Exit";

	// Token: 0x040009D2 RID: 2514
	protected const string ZONE_BULLET_PROJECTILE = "EyeballPassLineBoltProjectile";

	// Token: 0x040009D3 RID: 2515
	protected const string ZONE_BULLET_VARIANT_PROJECTILE = "EyeballPassLineBoltPrimeProjectile";

	// Token: 0x040009D4 RID: 2516
	protected const string ZONE_BLAST_PROJECTILE = "EyeballExplosionProjectile";

	// Token: 0x040009D5 RID: 2517
	protected const string ZONE_BLASTWARNING_PROJECTILE = "EyeballWarningProjectile";

	// Token: 0x040009D6 RID: 2518
	private Relay m_shoutAttackWarningAppearedRelay = new Relay();

	// Token: 0x040009D7 RID: 2519
	private Relay m_shoutAttackExplodedRelay = new Relay();

	// Token: 0x040009D8 RID: 2520
	protected const string SPINNING_FIREBALL_TELL_INTRO = "BurstShot_Tell_Intro";

	// Token: 0x040009D9 RID: 2521
	protected const string SPINNING_FIREBALL_TELL_HOLD = "BurstShot_Tell_Hold";

	// Token: 0x040009DA RID: 2522
	protected const string SPINNING_FIREBALL_ATTACK_INTRO = "BurstShot_Attack_Intro";

	// Token: 0x040009DB RID: 2523
	protected const string SPINNING_FIREBALL_ATTACK_HOLD = "BurstShot_Attack_Hold";

	// Token: 0x040009DC RID: 2524
	protected const string SPINNING_FIREBALL_EXIT = "BurstShot_Exit";

	// Token: 0x040009DD RID: 2525
	protected const string SPINNING_FIREBALL_PROJECTILE = "EyeballBoltMinibossProjectile";

	// Token: 0x040009DE RID: 2526
	private Projectile_RL m_shoutWarningProjectile;

	// Token: 0x040009DF RID: 2527
	protected const string HOMING_FIREBALL_TELL_INTRO = "HomingShot_Tell_Intro";

	// Token: 0x040009E0 RID: 2528
	protected const string HOMING_FIREBALL_TELL_HOLD = "HomingShot_Tell_Hold";

	// Token: 0x040009E1 RID: 2529
	protected const string HOMING_FIREBALL_ATTACK_INTRO = "HomingShot_Attack_Intro";

	// Token: 0x040009E2 RID: 2530
	protected const string HOMING_FIREBALL_ATTACK_HOLD = "HomingShot_Attack_Hold";

	// Token: 0x040009E3 RID: 2531
	protected const string HOMING_FIREBALL_EXIT = "HomingShot_Exit";

	// Token: 0x040009E4 RID: 2532
	protected const string ELECTRO_HOMING_FIREBALL_TELL_INTRO = "BounceBullets_Tell_Intro";

	// Token: 0x040009E5 RID: 2533
	protected const string ELECTRO_HOMING_FIREBALL_TELL_HOLD = "BounceBullets_Tell_Hold";

	// Token: 0x040009E6 RID: 2534
	protected const string ELECTRO_HOMING_FIREBALL_ATTACK_INTRO = "BounceBullets_Attack_Intro";

	// Token: 0x040009E7 RID: 2535
	protected const string ELECTRO_HOMING_FIREBALL_ATTACK_HOLD = "BounceBullets_Attack_Hold";

	// Token: 0x040009E8 RID: 2536
	protected const string ELECTRO_HOMING_FIREBALL_EXIT = "BounceBullets_Exit";

	// Token: 0x040009E9 RID: 2537
	protected const string HOMING_FIREBALL_PROJECTILE = "EyeballBounceBoltMinibossProjectile";

	// Token: 0x040009EA RID: 2538
	protected const string CURSE_FIREBALL_PROJECTILE = "EyeballCurseBoltProjectile";

	// Token: 0x040009EB RID: 2539
	protected const string CURSE_FIREBALL_BLUE_PROJECTILE = "EyeballCurseBoltBlueProjectile";

	// Token: 0x040009EC RID: 2540
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x040009ED RID: 2541
	protected const string SPAWN_INTRO = "Closed_Moving";

	// Token: 0x040009EE RID: 2542
	protected const string SPAWN_OPEN = "Open";

	// Token: 0x040009EF RID: 2543
	private const string SFX_EXPERT_SPAWN_INTRO_NAME = "event:/SFX/Enemies/sfx_khidirBoss_intro_rl1Grow_layer";

	// Token: 0x040009F0 RID: 2544
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x040009F1 RID: 2545
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x040009F2 RID: 2546
	protected float m_spawn_Open_AnimSpeed = 1f;

	// Token: 0x040009F3 RID: 2547
	protected float m_spawn_Open_Delay = 1f;

	// Token: 0x040009F4 RID: 2548
	protected float m_leftSpawn_Idle_Delay = 1f;

	// Token: 0x040009F5 RID: 2549
	protected float m_leftSpawn_Intro_Delay = 1f;

	// Token: 0x040009F6 RID: 2550
	protected float m_rightSpawn_Idle_Delay = 1.4f;

	// Token: 0x040009F7 RID: 2551
	protected float m_rightSpawn_Intro_Delay = 1.6f;

	// Token: 0x040009F8 RID: 2552
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x040009F9 RID: 2553
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x040009FA RID: 2554
	private const string SFX_EXPERT_DEATH_INTRO_NAME = "event:/SFX/Enemies/sfx_khidirBoss_death_rl1Layer";

	// Token: 0x040009FB RID: 2555
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x040009FC RID: 2556
	protected float m_death_Intro_Delay;

	// Token: 0x040009FD RID: 2557
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x040009FE RID: 2558
	protected float m_death_Hold_Delay = 4.5f;

	// Token: 0x040009FF RID: 2559
	private const float m_global_AnimationOverride = 1f;

	// Token: 0x04000A00 RID: 2560
	private bool m_modeShifted;

	// Token: 0x04000A01 RID: 2561
	private const string MODE_SHIFT_INTRO = "ModeShift_Intro";

	// Token: 0x04000A02 RID: 2562
	private const string MODE_SHIFT_SCREAM_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000A03 RID: 2563
	private const string MODE_SHIFT_SCREAM_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x04000A04 RID: 2564
	private const string MODE_SHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000A05 RID: 2565
	private const float m_modeShift_Intro_AnimSpeed = 1f;

	// Token: 0x04000A06 RID: 2566
	private const float m_modeShift_Intro_Delay = 3.25f;

	// Token: 0x04000A07 RID: 2567
	private const float m_modeShift_Scream_Intro_AnimSpeed = 1f;

	// Token: 0x04000A08 RID: 2568
	private const float m_modeShift_Scream_Intro_Delay = 0f;

	// Token: 0x04000A09 RID: 2569
	private const float m_modeShift_Scream_Hold_AnimSpeed = 1f;

	// Token: 0x04000A0A RID: 2570
	private const float m_modeShift_Scream_Hold_Delay = 1.25f;

	// Token: 0x04000A0B RID: 2571
	private const float m_modeShift_Exit_AnimSpeed = 1f;

	// Token: 0x04000A0C RID: 2572
	private const float m_modeShift_Exit_Delay = 0f;

	// Token: 0x04000A0D RID: 2573
	private const float m_modeShift_IdleDuration = 0.1f;

	// Token: 0x04000A0E RID: 2574
	private const float m_modeShift_AttackCD = 99999f;

	// Token: 0x04000A0F RID: 2575
	public const float ModeShift_HealthMod = 0.5f;

	// Token: 0x04000A10 RID: 2576
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x04000A11 RID: 2577
	private const float m_modeShift_openEye_Idle_AnimSpeed = 1f;

	// Token: 0x04000A12 RID: 2578
	private const float m_modeShift_openEye_Idle_Delay = 0.8f;

	// Token: 0x04000A13 RID: 2579
	private const float m_modeShift_openEye_Intro_AnimSpeed = 1f;

	// Token: 0x04000A14 RID: 2580
	private const float m_modeShift_openEye_Intro_Delay = 1.4f;

	// Token: 0x04000A15 RID: 2581
	private const float m_modeShift_openEye_Open_AnimSpeed = 1f;

	// Token: 0x04000A16 RID: 2582
	private const float m_modeShift_openEye_Open_Delay = 0.2f;

	// Token: 0x0200010F RID: 271
	protected enum EyeballType
	{
		// Token: 0x04000A18 RID: 2584
		Left,
		// Token: 0x04000A19 RID: 2585
		Right,
		// Token: 0x04000A1A RID: 2586
		Bottom,
		// Token: 0x04000A1B RID: 2587
		Middle
	}
}
