using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020000BC RID: 188
public class EyeballBoss_Basic_AIScript : Eyeball_Basic_AIScript, IAudioEventEmitter
{
	// Token: 0x17000216 RID: 534
	// (get) Token: 0x0600049D RID: 1181 RVA: 0x00015D5E File Offset: 0x00013F5E
	// (set) Token: 0x0600049E RID: 1182 RVA: 0x00015D66 File Offset: 0x00013F66
	public bool StartsDisabled { get; set; }

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x0600049F RID: 1183 RVA: 0x00015D6F File Offset: 0x00013F6F
	// (set) Token: 0x060004A0 RID: 1184 RVA: 0x00015D77 File Offset: 0x00013F77
	public bool DisableModeshift { get; set; }

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x060004A1 RID: 1185 RVA: 0x00015D80 File Offset: 0x00013F80
	protected bool IsKhidr
	{
		get
		{
			return base.EnemyController && base.EnemyController.EnemyRank == EnemyRank.Expert && base.EnemyController.EnemyType == EnemyType.EyeballBoss_Middle;
		}
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00015DB4 File Offset: 0x00013FB4
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

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00015E25 File Offset: 0x00014025
	// (set) Token: 0x060004A4 RID: 1188 RVA: 0x00015E2D File Offset: 0x0001402D
	public bool IsReady { get; private set; }

	// Token: 0x060004A5 RID: 1189 RVA: 0x00015E38 File Offset: 0x00014038
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

	// Token: 0x060004A6 RID: 1190 RVA: 0x00015E91 File Offset: 0x00014091
	private void Awake()
	{
		this.m_onPlayerEnterRoom = new Action<object, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onBossHit = new Action<object, HealthChangeEventArgs>(this.OnBossHit);
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00015EB8 File Offset: 0x000140B8
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

	// Token: 0x060004A8 RID: 1192 RVA: 0x00015F14 File Offset: 0x00014114
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

	// Token: 0x060004A9 RID: 1193 RVA: 0x00015F9C File Offset: 0x0001419C
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

	// Token: 0x060004AA RID: 1194 RVA: 0x00016000 File Offset: 0x00014200
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

	// Token: 0x060004AB RID: 1195 RVA: 0x0001611C File Offset: 0x0001431C
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

	// Token: 0x060004AC RID: 1196 RVA: 0x000161C0 File Offset: 0x000143C0
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

	// Token: 0x060004AD RID: 1197 RVA: 0x00016234 File Offset: 0x00014434
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

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x060004AE RID: 1198 RVA: 0x00016369 File Offset: 0x00014569
	protected virtual float ZoneBlast_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x060004AF RID: 1199 RVA: 0x00016370 File Offset: 0x00014570
	protected virtual float ZoneBlast_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00016377 File Offset: 0x00014577
	protected virtual float ZoneBlast_TellHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001637E File Offset: 0x0001457E
	protected virtual float ZoneBlast_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00016385 File Offset: 0x00014585
	protected virtual float ZoneBlast_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0001638C File Offset: 0x0001458C
	protected virtual float ZoneBlast_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00016393 File Offset: 0x00014593
	protected virtual float ZoneBlast_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0001639A File Offset: 0x0001459A
	protected virtual float ZoneBlast_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x060004B6 RID: 1206 RVA: 0x000163A1 File Offset: 0x000145A1
	protected virtual float ZoneBlast_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x060004B7 RID: 1207 RVA: 0x000163A8 File Offset: 0x000145A8
	protected virtual float ZoneBlast_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x060004B8 RID: 1208 RVA: 0x000163AF File Offset: 0x000145AF
	protected virtual float ZoneBlast_Exit_AttackCD
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x060004B9 RID: 1209 RVA: 0x000163B6 File Offset: 0x000145B6
	protected virtual int ZoneBlast_Attack_Amount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x060004BA RID: 1210 RVA: 0x000163B9 File Offset: 0x000145B9
	protected virtual float ZoneBlast_AttackHold_DelayBetweenShots
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x060004BB RID: 1211 RVA: 0x000163C0 File Offset: 0x000145C0
	protected virtual float ZoneBlast_Attack_AngleSpread
	{
		get
		{
			return 75f;
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x060004BC RID: 1212 RVA: 0x000163C7 File Offset: 0x000145C7
	protected virtual bool ZoneBlast_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x060004BD RID: 1213 RVA: 0x000163CA File Offset: 0x000145CA
	protected virtual bool ZoneBlast_Variant
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x060004BE RID: 1214 RVA: 0x000163CD File Offset: 0x000145CD
	public IRelayLink ShoutAttackWarningAppearedRelay
	{
		get
		{
			return this.m_shoutAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x060004BF RID: 1215 RVA: 0x000163DA File Offset: 0x000145DA
	public IRelayLink ShoutAttackExplodedRelay
	{
		get
		{
			return this.m_shoutAttackExplodedRelay.link;
		}
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x000163E7 File Offset: 0x000145E7
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

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x060004C1 RID: 1217 RVA: 0x000163F6 File Offset: 0x000145F6
	protected virtual int Spinning_Fireball_Amount
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x060004C2 RID: 1218 RVA: 0x000163F9 File Offset: 0x000145F9
	protected virtual int SpinngFireball_Angle
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x060004C3 RID: 1219 RVA: 0x000163FD File Offset: 0x000145FD
	protected virtual bool SpinningFireball_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00016400 File Offset: 0x00014600
	protected virtual int Spinning_Fireball_Variant_Amount
	{
		get
		{
			return 15;
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00016404 File Offset: 0x00014604
	protected virtual int SpinngFireball_Variant_Angle
	{
		get
		{
			return 16;
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00016408 File Offset: 0x00014608
	protected virtual bool SpinningFireball_Variant_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0001640B File Offset: 0x0001460B
	protected virtual bool SpinningFireball_Variant
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0001640E File Offset: 0x0001460E
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

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x060004C9 RID: 1225 RVA: 0x0001641D File Offset: 0x0001461D
	protected virtual bool HomingFireball_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x060004CA RID: 1226 RVA: 0x00016420 File Offset: 0x00014620
	protected virtual bool HomingFireball_Variant
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x060004CB RID: 1227 RVA: 0x00016423 File Offset: 0x00014623
	protected virtual float HomingFireball_Variant_SpeedMod
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x060004CC RID: 1228 RVA: 0x0001642A File Offset: 0x0001462A
	protected virtual bool HomingFireball_Variant_Blue
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0001642D File Offset: 0x0001462D
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

	// Token: 0x060004CE RID: 1230 RVA: 0x0001643C File Offset: 0x0001463C
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

	// Token: 0x060004CF RID: 1231 RVA: 0x0001644B File Offset: 0x0001464B
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

	// Token: 0x060004D0 RID: 1232 RVA: 0x0001645A File Offset: 0x0001465A
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

	// Token: 0x060004D1 RID: 1233 RVA: 0x00016469 File Offset: 0x00014669
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

	// Token: 0x060004D2 RID: 1234 RVA: 0x00016478 File Offset: 0x00014678
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_shoutWarningProjectile);
		base.OnLBCompleteOrCancelled();
		this.IsReady = false;
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00016494 File Offset: 0x00014694
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

	// Token: 0x060004D4 RID: 1236 RVA: 0x00016507 File Offset: 0x00014707
	private void StopAttackEffects()
	{
		EffectManager.DisableAllEffectWithName("EyeballBossBlueFireballTell_Effect");
		EffectManager.DisableAllEffectWithName("EyeballBossBounceBulletsTell_Effect");
		EffectManager.DisableAllEffectWithName("EyeballBossCurseTell_Effect");
		AudioManager.Stop(this.m_burstShotPrepLoopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_bounceShotPrepLoopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00016540 File Offset: 0x00014740
	public override void ResetScript()
	{
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		string forceExecuteLogicBlockName_OnceOnly = base.LogicController.ForceExecuteLogicBlockName_OnceOnly;
		base.ResetScript();
		base.LogicController.ForceExecuteLogicBlockName_OnceOnly = forceExecuteLogicBlockName_OnceOnly;
		base.LogicController.DisableLogicActivationByDistance = true;
		EyeballBoss_Basic_AIScript.m_deathWhiteFlashPlayed = false;
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0001658E File Offset: 0x0001478E
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		if (this.IsKhidr)
		{
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SummonRuleBroadcast, null, null);
		}
	}

	// Token: 0x0400084E RID: 2126
	[SerializeField]
	protected EyeballBoss_Basic_AIScript.EyeballType m_eyeballType;

	// Token: 0x0400084F RID: 2127
	protected const float PROJECTILE_SPAWN_X_OFFSET = 3f;

	// Token: 0x04000850 RID: 2128
	private static bool m_deathWhiteFlashPlayed = false;

	// Token: 0x04000851 RID: 2129
	private static bool m_modeShiftWhiteFlashPlayed = false;

	// Token: 0x04000852 RID: 2130
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x04000853 RID: 2131
	protected const string POSITION_ANIM_PARAM = "EyeballPosition";

	// Token: 0x04000854 RID: 2132
	private EventInstance m_burstShotPrepLoopInstance;

	// Token: 0x04000855 RID: 2133
	private EventInstance m_bounceShotPrepLoopInstance;

	// Token: 0x04000856 RID: 2134
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04000857 RID: 2135
	private Action<object, HealthChangeEventArgs> m_onBossHit;

	// Token: 0x0400085A RID: 2138
	protected static List<EyeballBoss_Basic_AIScript> m_eyeballSyncList_STATIC = new List<EyeballBoss_Basic_AIScript>();

	// Token: 0x0400085C RID: 2140
	protected const string ZONE_BLAST_TELL_INTRO = "AreaShot_Tell_Intro";

	// Token: 0x0400085D RID: 2141
	protected const string ZONE_BLAST_TELL_HOLD = "AreaShot_Tell_Hold";

	// Token: 0x0400085E RID: 2142
	protected const string ZONE_BLAST_ATTACK_INTRO = "AreaShot_Attack_Intro";

	// Token: 0x0400085F RID: 2143
	protected const string ZONE_BLAST_ATTACK_HOLD = "AreaShot_Attack_Hold";

	// Token: 0x04000860 RID: 2144
	protected const string ZONE_BLAST_EXIT = "AreaShot_Exit";

	// Token: 0x04000861 RID: 2145
	protected const string EXPERT_ZONE_BLAST_TELL_INTRO = "Explosion_Tell_Intro";

	// Token: 0x04000862 RID: 2146
	protected const string EXPERT_ZONE_BLAST_TELL_HOLD = "Explosion_Tell_Hold";

	// Token: 0x04000863 RID: 2147
	protected const string EXPERT_ZONE_BLAST_ATTACK_INTRO = "Explosion_Attack_Intro";

	// Token: 0x04000864 RID: 2148
	protected const string EXPERT_ZONE_BLAST_ATTACK_HOLD = "Explosion_Attack_Hold";

	// Token: 0x04000865 RID: 2149
	protected const string EXPERT_ZONE_BLAST_EXIT = "Explosion_Exit";

	// Token: 0x04000866 RID: 2150
	protected const string ZONE_BULLET_PROJECTILE = "EyeballPassLineBoltProjectile";

	// Token: 0x04000867 RID: 2151
	protected const string ZONE_BULLET_VARIANT_PROJECTILE = "EyeballPassLineBoltPrimeProjectile";

	// Token: 0x04000868 RID: 2152
	protected const string ZONE_BLAST_PROJECTILE = "EyeballExplosionProjectile";

	// Token: 0x04000869 RID: 2153
	protected const string ZONE_BLASTWARNING_PROJECTILE = "EyeballWarningProjectile";

	// Token: 0x0400086A RID: 2154
	private Relay m_shoutAttackWarningAppearedRelay = new Relay();

	// Token: 0x0400086B RID: 2155
	private Relay m_shoutAttackExplodedRelay = new Relay();

	// Token: 0x0400086C RID: 2156
	protected const string SPINNING_FIREBALL_TELL_INTRO = "BurstShot_Tell_Intro";

	// Token: 0x0400086D RID: 2157
	protected const string SPINNING_FIREBALL_TELL_HOLD = "BurstShot_Tell_Hold";

	// Token: 0x0400086E RID: 2158
	protected const string SPINNING_FIREBALL_ATTACK_INTRO = "BurstShot_Attack_Intro";

	// Token: 0x0400086F RID: 2159
	protected const string SPINNING_FIREBALL_ATTACK_HOLD = "BurstShot_Attack_Hold";

	// Token: 0x04000870 RID: 2160
	protected const string SPINNING_FIREBALL_EXIT = "BurstShot_Exit";

	// Token: 0x04000871 RID: 2161
	protected const string SPINNING_FIREBALL_PROJECTILE = "EyeballBoltMinibossProjectile";

	// Token: 0x04000872 RID: 2162
	private Projectile_RL m_shoutWarningProjectile;

	// Token: 0x04000873 RID: 2163
	protected const string HOMING_FIREBALL_TELL_INTRO = "HomingShot_Tell_Intro";

	// Token: 0x04000874 RID: 2164
	protected const string HOMING_FIREBALL_TELL_HOLD = "HomingShot_Tell_Hold";

	// Token: 0x04000875 RID: 2165
	protected const string HOMING_FIREBALL_ATTACK_INTRO = "HomingShot_Attack_Intro";

	// Token: 0x04000876 RID: 2166
	protected const string HOMING_FIREBALL_ATTACK_HOLD = "HomingShot_Attack_Hold";

	// Token: 0x04000877 RID: 2167
	protected const string HOMING_FIREBALL_EXIT = "HomingShot_Exit";

	// Token: 0x04000878 RID: 2168
	protected const string ELECTRO_HOMING_FIREBALL_TELL_INTRO = "BounceBullets_Tell_Intro";

	// Token: 0x04000879 RID: 2169
	protected const string ELECTRO_HOMING_FIREBALL_TELL_HOLD = "BounceBullets_Tell_Hold";

	// Token: 0x0400087A RID: 2170
	protected const string ELECTRO_HOMING_FIREBALL_ATTACK_INTRO = "BounceBullets_Attack_Intro";

	// Token: 0x0400087B RID: 2171
	protected const string ELECTRO_HOMING_FIREBALL_ATTACK_HOLD = "BounceBullets_Attack_Hold";

	// Token: 0x0400087C RID: 2172
	protected const string ELECTRO_HOMING_FIREBALL_EXIT = "BounceBullets_Exit";

	// Token: 0x0400087D RID: 2173
	protected const string HOMING_FIREBALL_PROJECTILE = "EyeballBounceBoltMinibossProjectile";

	// Token: 0x0400087E RID: 2174
	protected const string CURSE_FIREBALL_PROJECTILE = "EyeballCurseBoltProjectile";

	// Token: 0x0400087F RID: 2175
	protected const string CURSE_FIREBALL_BLUE_PROJECTILE = "EyeballCurseBoltBlueProjectile";

	// Token: 0x04000880 RID: 2176
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x04000881 RID: 2177
	protected const string SPAWN_INTRO = "Closed_Moving";

	// Token: 0x04000882 RID: 2178
	protected const string SPAWN_OPEN = "Open";

	// Token: 0x04000883 RID: 2179
	private const string SFX_EXPERT_SPAWN_INTRO_NAME = "event:/SFX/Enemies/sfx_khidirBoss_intro_rl1Grow_layer";

	// Token: 0x04000884 RID: 2180
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04000885 RID: 2181
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04000886 RID: 2182
	protected float m_spawn_Open_AnimSpeed = 1f;

	// Token: 0x04000887 RID: 2183
	protected float m_spawn_Open_Delay = 1f;

	// Token: 0x04000888 RID: 2184
	protected float m_leftSpawn_Idle_Delay = 1f;

	// Token: 0x04000889 RID: 2185
	protected float m_leftSpawn_Intro_Delay = 1f;

	// Token: 0x0400088A RID: 2186
	protected float m_rightSpawn_Idle_Delay = 1.4f;

	// Token: 0x0400088B RID: 2187
	protected float m_rightSpawn_Intro_Delay = 1.6f;

	// Token: 0x0400088C RID: 2188
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x0400088D RID: 2189
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x0400088E RID: 2190
	private const string SFX_EXPERT_DEATH_INTRO_NAME = "event:/SFX/Enemies/sfx_khidirBoss_death_rl1Layer";

	// Token: 0x0400088F RID: 2191
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04000890 RID: 2192
	protected float m_death_Intro_Delay;

	// Token: 0x04000891 RID: 2193
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04000892 RID: 2194
	protected float m_death_Hold_Delay = 4.5f;

	// Token: 0x04000893 RID: 2195
	private const float m_global_AnimationOverride = 1f;

	// Token: 0x04000894 RID: 2196
	private bool m_modeShifted;

	// Token: 0x04000895 RID: 2197
	private const string MODE_SHIFT_INTRO = "ModeShift_Intro";

	// Token: 0x04000896 RID: 2198
	private const string MODE_SHIFT_SCREAM_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000897 RID: 2199
	private const string MODE_SHIFT_SCREAM_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x04000898 RID: 2200
	private const string MODE_SHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000899 RID: 2201
	private const float m_modeShift_Intro_AnimSpeed = 1f;

	// Token: 0x0400089A RID: 2202
	private const float m_modeShift_Intro_Delay = 3.25f;

	// Token: 0x0400089B RID: 2203
	private const float m_modeShift_Scream_Intro_AnimSpeed = 1f;

	// Token: 0x0400089C RID: 2204
	private const float m_modeShift_Scream_Intro_Delay = 0f;

	// Token: 0x0400089D RID: 2205
	private const float m_modeShift_Scream_Hold_AnimSpeed = 1f;

	// Token: 0x0400089E RID: 2206
	private const float m_modeShift_Scream_Hold_Delay = 1.25f;

	// Token: 0x0400089F RID: 2207
	private const float m_modeShift_Exit_AnimSpeed = 1f;

	// Token: 0x040008A0 RID: 2208
	private const float m_modeShift_Exit_Delay = 0f;

	// Token: 0x040008A1 RID: 2209
	private const float m_modeShift_IdleDuration = 0.1f;

	// Token: 0x040008A2 RID: 2210
	private const float m_modeShift_AttackCD = 99999f;

	// Token: 0x040008A3 RID: 2211
	public const float ModeShift_HealthMod = 0.5f;

	// Token: 0x040008A4 RID: 2212
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x040008A5 RID: 2213
	private const float m_modeShift_openEye_Idle_AnimSpeed = 1f;

	// Token: 0x040008A6 RID: 2214
	private const float m_modeShift_openEye_Idle_Delay = 0.8f;

	// Token: 0x040008A7 RID: 2215
	private const float m_modeShift_openEye_Intro_AnimSpeed = 1f;

	// Token: 0x040008A8 RID: 2216
	private const float m_modeShift_openEye_Intro_Delay = 1.4f;

	// Token: 0x040008A9 RID: 2217
	private const float m_modeShift_openEye_Open_AnimSpeed = 1f;

	// Token: 0x040008AA RID: 2218
	private const float m_modeShift_openEye_Open_Delay = 0.2f;

	// Token: 0x020009CB RID: 2507
	protected enum EyeballType
	{
		// Token: 0x04004602 RID: 17922
		Left,
		// Token: 0x04004603 RID: 17923
		Right,
		// Token: 0x04004604 RID: 17924
		Bottom,
		// Token: 0x04004605 RID: 17925
		Middle
	}
}
