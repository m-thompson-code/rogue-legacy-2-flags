using System;
using System.Collections;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public class Eyeball_Miniboss_AIScript : Eyeball_Basic_AIScript
{
	// Token: 0x06000516 RID: 1302 RVA: 0x00016988 File Offset: 0x00014B88
	public static bool CanFire()
	{
		for (int i = 0; i < Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC.Count; i++)
		{
			if (!Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC[i])
			{
				Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC.RemoveAt(i);
				i--;
			}
			else if (!Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC[i].IsReady && !Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC[i].EnemyController.IsDead)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000517 RID: 1303 RVA: 0x000169F9 File Offset: 0x00014BF9
	// (set) Token: 0x06000518 RID: 1304 RVA: 0x00016A01 File Offset: 0x00014C01
	public bool IsReady { get; private set; }

	// Token: 0x06000519 RID: 1305 RVA: 0x00016A0A File Offset: 0x00014C0A
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"EyeballBoltMinibossProjectile",
			"EyeballBounceBoltMinibossProjectile",
			"EyeballExplosionProjectile",
			"EyeballPassLineBoltProjectile",
			"EyeballWarningProjectile",
			"EyeballCurseBoltProjectile"
		};
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00016A48 File Offset: 0x00014C48
	private void OnEnable()
	{
		this.IsReady = false;
		this.m_modeShifted = false;
		if (!Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC.Contains(this))
		{
			Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC.Add(this);
		}
		if (base.IsInitialized)
		{
			this.InitializeModeShiftEye();
		}
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00016A7E File Offset: 0x00014C7E
	protected override void OnDisable()
	{
		base.OnDisable();
		if (Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC.Contains(this))
		{
			Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC.Remove(this);
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x00016A9F File Offset: 0x00014C9F
	private void OnDestroy()
	{
		if (base.EnemyController != null)
		{
			base.EnemyController.HealthChangeRelay.RemoveListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit));
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00016ACC File Offset: 0x00014CCC
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.InitializeModeShiftEye();
		base.EnemyController.HealthChangeRelay.AddListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit), false);
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00016AFC File Offset: 0x00014CFC
	private void InitializeModeShiftEye()
	{
		if (base.EnemyController.EnemyRank == EnemyRank.Expert)
		{
			base.EnemyController.TakesNoDamage = true;
			base.EnemyController.StatusBarController.Active = false;
			base.EnemyController.LogicController.enabled = false;
			base.EnemyController.LogicController.DisableLogicActivationByDistance = true;
		}
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00016B58 File Offset: 0x00014D58
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
		if (args.PrevHealthValue <= args.NewHealthValue && base.EnemyController.EnemyRank != EnemyRank.Expert)
		{
			return;
		}
		if (base.EnemyController.CurrentHealth <= (float)base.EnemyController.ActualMaxHealth * 0.5f)
		{
			this.m_modeShifted = true;
			if (base.EnemyController.EnemyRank == EnemyRank.Expert)
			{
				base.EnemyController.TakesNoDamage = false;
				base.EnemyController.StatusBarController.Active = true;
				base.EnemyController.LogicController.enabled = true;
				base.EnemyController.LogicController.DisableLogicActivationByDistance = false;
				base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "Mode_Shift_OpenEye";
				return;
			}
			base.LogicController.StopAllLogic(true);
			base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "Mode_Shift";
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06000520 RID: 1312 RVA: 0x00016C3C File Offset: 0x00014E3C
	protected virtual float ZoneBlast_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06000521 RID: 1313 RVA: 0x00016C43 File Offset: 0x00014E43
	protected virtual float ZoneBlast_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06000522 RID: 1314 RVA: 0x00016C4A File Offset: 0x00014E4A
	protected virtual float ZoneBlast_TellHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06000523 RID: 1315 RVA: 0x00016C51 File Offset: 0x00014E51
	protected virtual float ZoneBlast_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06000524 RID: 1316 RVA: 0x00016C58 File Offset: 0x00014E58
	protected virtual float ZoneBlast_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06000525 RID: 1317 RVA: 0x00016C5F File Offset: 0x00014E5F
	protected virtual float ZoneBlast_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000526 RID: 1318 RVA: 0x00016C66 File Offset: 0x00014E66
	protected virtual float ZoneBlast_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06000527 RID: 1319 RVA: 0x00016C6D File Offset: 0x00014E6D
	protected virtual float ZoneBlast_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06000528 RID: 1320 RVA: 0x00016C74 File Offset: 0x00014E74
	protected virtual float ZoneBlast_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06000529 RID: 1321 RVA: 0x00016C7B File Offset: 0x00014E7B
	protected virtual float ZoneBlast_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x0600052A RID: 1322 RVA: 0x00016C82 File Offset: 0x00014E82
	protected virtual float ZoneBlast_Exit_AttackCD
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x0600052B RID: 1323 RVA: 0x00016C89 File Offset: 0x00014E89
	protected virtual int ZoneBlast_Attack_Amount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x0600052C RID: 1324 RVA: 0x00016C8C File Offset: 0x00014E8C
	protected virtual float ZoneBlast_AttackHold_DelayBetweenShots
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x0600052D RID: 1325 RVA: 0x00016C93 File Offset: 0x00014E93
	protected virtual float ZoneBlast_Attack_AngleSpread
	{
		get
		{
			return 75f;
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x0600052E RID: 1326 RVA: 0x00016C9A File Offset: 0x00014E9A
	protected virtual bool ZoneBlast_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x0600052F RID: 1327 RVA: 0x00016C9D File Offset: 0x00014E9D
	public IRelayLink ShoutAttackWarningAppearedRelay
	{
		get
		{
			return this.m_shoutAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06000530 RID: 1328 RVA: 0x00016CAA File Offset: 0x00014EAA
	public IRelayLink ShoutAttackExplodedRelay
	{
		get
		{
			return this.m_shoutAttackExplodedRelay.link;
		}
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00016CB7 File Offset: 0x00014EB7
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
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
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
			while (!Eyeball_Miniboss_AIScript.CanFire())
			{
				yield return null;
			}
		}
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			this.m_shoutWarningProjectile = this.FireProjectile("EyeballWarningProjectile", 0, false, 0f, 1f, true, true, true);
		}
		base.EnemyController.Animator.SetBool("Centered", true);
		yield return this.Default_TellIntroAndLoop(TELL_INTRO, this.ZoneBlast_TellIntro_AnimSpeed, TELL_HOLD, this.ZoneBlast_TellHold_AnimSpeed, this.ZoneBlast_TellHold_Duration);
		yield return this.Default_Animation(ATTACK_INTRO, this.ZoneBlast_AttackIntro_AnimSpeed, this.ZoneBlast_AttackIntro_Delay, true);
		base.EnemyController.FollowTarget = false;
		CDGHelper.VectorToAngle(base.EnemyController.Heading);
		yield return this.ChangeAnimationState(ATTACK_HOLD);
		this.SetAnimationSpeedMultiplier(this.ZoneBlast_AttackHold_AnimSpeed);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			this.m_shoutWarningProjectile.transform.position = base.EnemyController.Midpoint;
			this.m_shoutWarningProjectile.transform.SetParent(base.EnemyController.transform, true);
			this.m_shoutAttackWarningAppearedRelay.Dispatch();
			base.StopProjectile(ref this.m_shoutWarningProjectile);
			this.m_shoutAttackExplodedRelay.Dispatch();
			this.FireProjectile("EyeballExplosionProjectile", 0, false, 0f, 1f, true, true, true);
		}
		else
		{
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 0f, 1f, true, true, true);
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 30f, 1f, true, true, true);
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 60f, 1f, true, true, true);
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 90f, 1f, true, true, true);
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 120f, 1f, true, true, true);
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 150f, 1f, true, true, true);
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 180f, 1f, true, true, true);
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 210f, 1f, true, true, true);
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 240f, 1f, true, true, true);
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 270f, 1f, true, true, true);
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 300f, 1f, true, true, true);
			this.FireProjectile("EyeballPassLineBoltProjectile", 0, false, 330f, 1f, true, true, true);
		}
		yield return this.Default_Animation(EXIT, this.ZoneBlast_Exit_AnimSpeed, this.ZoneBlast_Exit_Duration, true);
		base.EnemyController.Animator.SetBool("Centered", false);
		base.EnemyController.FollowTarget = false;
		yield return this.Default_Attack_Cooldown(this.ZoneBlast_Exit_ForceIdle, this.ZoneBlast_Exit_AttackCD);
		this.IsReady = false;
		yield break;
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06000532 RID: 1330 RVA: 0x00016CC6 File Offset: 0x00014EC6
	protected virtual int Spinning_Fireball_Amount
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06000533 RID: 1331 RVA: 0x00016CC9 File Offset: 0x00014EC9
	protected virtual int SpinngFireball_Angle
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000534 RID: 1332 RVA: 0x00016CCD File Offset: 0x00014ECD
	protected virtual bool SpinningFireball_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00016CD0 File Offset: 0x00014ED0
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
			while (!Eyeball_Miniboss_AIScript.CanFire())
			{
				yield return null;
			}
		}
		base.EnemyController.Animator.SetBool("Centered", true);
		yield return this.Default_TellIntroAndLoop("BurstShot_Tell_Intro", this.MultiShot_TellIntro_AnimSpeed, "BurstShot_Tell_Hold", this.MultiShot_TellHold_AnimSpeed, this.MultiShot_TellHold_Duration);
		yield return this.Default_Animation("BurstShot_Attack_Intro", this.MultiShot_AttackIntro_AnimSpeed, this.MultiShot_AttackIntro_Delay, true);
		base.EnemyController.FollowTarget = false;
		CDGHelper.VectorToAngle(base.EnemyController.Heading);
		yield return this.ChangeAnimationState("BurstShot_Attack_Hold");
		this.SetAnimationSpeedMultiplier(this.MultiShot_AttackHold_AnimSpeed);
		float num = (float)UnityEngine.Random.Range(0, 2);
		int flipper = -1;
		if (num > 1f)
		{
			flipper = 1;
		}
		int SpinningFireballCycles = this.SpinngFireball_Angle * this.Spinning_Fireball_Amount;
		for (int i = 0; i <= SpinningFireballCycles; i += this.SpinngFireball_Angle)
		{
			this.FireProjectile("EyeballBoltMinibossProjectile", 0, false, (float)(i * flipper), 1f, true, true, true);
			this.FireProjectile("EyeballBoltMinibossProjectile", 0, false, (float)(180 + i * flipper), 1f, true, true, true);
			yield return base.Wait(0.175f, false);
		}
		yield return base.Wait(this.MultiShot_AttackHold_Delay, false);
		base.EnemyController.Animator.SetBool("Centered", false);
		yield return this.Default_Animation("BurstShot_Exit", this.MultiShot_Exit_AnimSpeed, this.MultiShot_Exit_Duration, true);
		base.EnemyController.FollowTarget = false;
		yield return this.Default_Attack_Cooldown(this.MultiShot_Exit_ForceIdle, this.MultiShot_Exit_AttackCD);
		this.IsReady = false;
		yield break;
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000536 RID: 1334 RVA: 0x00016CDF File Offset: 0x00014EDF
	protected virtual bool HomingFireball_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x00016CE2 File Offset: 0x00014EE2
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
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			TELL_INTRO = "BounceBullets_Tell_Intro";
			TELL_HOLD = "BounceBullets_Tell_Hold";
			ATTACK_INTRO = "BounceBullets_Attack_Intro";
			ATTACK_HOLD = "BounceBullets_Attack_Hold";
			EXIT = "BounceBullets_Exit";
		}
		if (this.HomingFireball_Sync)
		{
			this.IsReady = true;
			while (!Eyeball_Miniboss_AIScript.CanFire())
			{
				yield return null;
			}
		}
		base.EnemyController.FollowTarget = false;
		float fireAngle = CDGHelper.VectorToAngle(base.EnemyController.Heading);
		yield return this.Default_TellIntroAndLoop(TELL_INTRO, this.SingleShot_TellIntro_AnimSpeed, TELL_HOLD, this.MultiShot_TellHold_AnimSpeed, this.MultiShot_TellHold_Duration);
		yield return this.Default_Animation(ATTACK_INTRO, this.SingleShot_AttackIntro_AnimSpeed, this.SingleShot_AttackIntro_Delay, true);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			int num;
			for (int i = 0; i < 2; i = num + 1)
			{
				Projectile_RL projectile = this.FireProjectile("EyeballBounceBoltMinibossProjectile", 1, false, fireAngle, 1f, true, true, true);
				this.RotateProjectileToHeading(projectile);
				yield return base.Wait(1.75f, false);
				num = i;
			}
		}
		else
		{
			int num;
			for (int i = 0; i < 2; i = num + 1)
			{
				Projectile_RL projectile2 = this.FireProjectile("EyeballCurseBoltProjectile", 1, false, fireAngle, 1f, true, true, true);
				this.RotateProjectileToHeading(projectile2);
				yield return base.Wait(1.75f, false);
				num = i;
			}
		}
		yield return this.Default_Animation(ATTACK_HOLD, this.SingleShot_AttackHold_AnimSpeed, this.SingleShot_AttackHold_Delay, true);
		yield return this.Default_Animation(EXIT, this.SingleShot_Exit_AnimSpeed, this.SingleShot_Exit_Duration, true);
		base.EnemyController.FollowTarget = false;
		yield return this.Default_Attack_Cooldown(this.SingleShot_Exit_ForceIdle, this.SingleShot_Exit_AttackCD);
		this.IsReady = false;
		yield break;
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x00016CF1 File Offset: 0x00014EF1
	public override IEnumerator SpawnAnim()
	{
		this.m_faceForward = true;
		int spawnOrder_STATIC = Eyeball_Miniboss_AIScript.m_spawnOrder_STATIC;
		Eyeball_Miniboss_AIScript.m_spawnOrder_STATIC = (Eyeball_Miniboss_AIScript.m_spawnOrder_STATIC + 1) % 2;
		float delay = this.m_spawn_Idle_Delay;
		float introDelay = this.m_spawn_Intro_Delay;
		if (spawnOrder_STATIC == 1)
		{
			delay = this.m_secondSpawn_Idle_Delay;
			introDelay = this.m_secondSpawn_Intro_Delay;
		}
		yield return this.Default_Animation("Intro_Idle", this.m_spawn_Idle_AnimSpeed, delay, true);
		yield return this.Default_Animation("Closed_Moving", this.m_spawn_Intro_AnimSpeed, introDelay, true);
		this.m_faceForward = false;
		base.EnemyController.ForceFaceTarget();
		yield return this.Default_Animation("Open", this.m_spawn_Open_AnimSpeed, this.m_spawn_Open_Delay, true);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 0f, false);
		yield break;
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x00016D00 File Offset: 0x00014F00
	public override IEnumerator DeathAnim()
	{
		if (base.EnemyController.EnemyRank == EnemyRank.Expert || Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC.Count <= 1)
		{
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

	// Token: 0x0600053A RID: 1338 RVA: 0x00016D0F File Offset: 0x00014F0F
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
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_Damage_Mod;
		base.EnemyController.Animator.SetBool("Centered", true);
		yield return this.Default_Animation("ModeShift_Intro", 1f, 3.25f, true);
		yield return this.Default_Animation("ModeShift_Scream_Intro", 1f, 0f, true);
		yield return this.Default_Animation("ModeShift_Scream_Hold", 1f, 1.25f, true);
		yield return this.Default_Animation("ModeShift_Scream_Exit", 1f, 0f, true);
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.ResetBaseValues();
		base.EnemyController.Animator.SetBool("Centered", false);
		yield return this.Default_Attack_Cooldown(0.1f, 99999f);
		yield break;
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x00016D1E File Offset: 0x00014F1E
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
		base.EnemyController.Animator.SetBool("Centered", true);
		this.StopAttackEffects();
		yield return base.DeathAnim();
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_Damage_Mod;
		yield return this.Default_Animation("Intro_Idle", 1f, 0.5f, true);
		yield return this.Default_Animation("Closed_Moving", 1f, 0.75f, true);
		yield return this.Default_Animation("Open", 1f, 1f, true);
		yield return this.Default_Animation("ModeShift_Scream_Intro", 1f, 0f, true);
		yield return this.Default_Animation("ModeShift_Scream_Hold", 1f, 1.25f, true);
		yield return this.Default_Animation("ModeShift_Scream_Exit", 1f, 0f, true);
		base.EnemyController.TakesNoDamage = false;
		base.EnemyController.StatusBarController.Active = true;
		base.EnemyController.Animator.SetBool("Centered", false);
		base.EnemyController.FollowTarget = true;
		this.m_faceForward = false;
		base.EnemyController.ForceFaceTarget();
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(0.1f, 99999f);
		yield break;
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00016D2D File Offset: 0x00014F2D
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_shoutWarningProjectile);
		base.OnLBCompleteOrCancelled();
		this.IsReady = false;
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x00016D48 File Offset: 0x00014F48
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

	// Token: 0x0600053E RID: 1342 RVA: 0x00016DBB File Offset: 0x00014FBB
	private void StopAttackEffects()
	{
		EffectManager.DisableAllEffectWithName("EyeballBossBlueFireballTell_Effect");
		EffectManager.DisableAllEffectWithName("EyeballBossBounceBulletsTell_Effect");
		EffectManager.DisableAllEffectWithName("EyeballBossCurseTell_Effect");
	}

	// Token: 0x040008B3 RID: 2227
	private const float PROJECTILE_SPAWN_X_OFFSET = 3f;

	// Token: 0x040008B4 RID: 2228
	public const EnemyRank MODESHIFT_EYE_RANK = EnemyRank.Expert;

	// Token: 0x040008B5 RID: 2229
	private static List<Eyeball_Miniboss_AIScript> m_eyeballSyncList_STATIC = new List<Eyeball_Miniboss_AIScript>();

	// Token: 0x040008B7 RID: 2231
	protected const string ZONE_BLAST_TELL_INTRO = "AreaShot_Tell_Intro";

	// Token: 0x040008B8 RID: 2232
	protected const string ZONE_BLAST_TELL_HOLD = "AreaShot_Tell_Hold";

	// Token: 0x040008B9 RID: 2233
	protected const string ZONE_BLAST_ATTACK_INTRO = "AreaShot_Attack_Intro";

	// Token: 0x040008BA RID: 2234
	protected const string ZONE_BLAST_ATTACK_HOLD = "AreaShot_Attack_Hold";

	// Token: 0x040008BB RID: 2235
	protected const string ZONE_BLAST_EXIT = "AreaShot_Exit";

	// Token: 0x040008BC RID: 2236
	protected const string EXPERT_ZONE_BLAST_TELL_INTRO = "Explosion_Tell_Intro";

	// Token: 0x040008BD RID: 2237
	protected const string EXPERT_ZONE_BLAST_TELL_HOLD = "Explosion_Tell_Hold";

	// Token: 0x040008BE RID: 2238
	protected const string EXPERT_ZONE_BLAST_ATTACK_INTRO = "Explosion_Attack_Intro";

	// Token: 0x040008BF RID: 2239
	protected const string EXPERT_ZONE_BLAST_ATTACK_HOLD = "Explosion_Attack_Hold";

	// Token: 0x040008C0 RID: 2240
	protected const string EXPERT_ZONE_BLAST_EXIT = "Explosion_Exit";

	// Token: 0x040008C1 RID: 2241
	protected const string ZONE_BULLET_PROJECTILE = "EyeballPassLineBoltProjectile";

	// Token: 0x040008C2 RID: 2242
	protected const string ZONE_BLAST_PROJECTILE = "EyeballExplosionProjectile";

	// Token: 0x040008C3 RID: 2243
	protected const string ZONE_BLASTWARNING_PROJECTILE = "EyeballWarningProjectile";

	// Token: 0x040008C4 RID: 2244
	private Relay m_shoutAttackWarningAppearedRelay = new Relay();

	// Token: 0x040008C5 RID: 2245
	private Relay m_shoutAttackExplodedRelay = new Relay();

	// Token: 0x040008C6 RID: 2246
	protected const string SPINNING_FIREBALL_TELL_INTRO = "BurstShot_Tell_Intro";

	// Token: 0x040008C7 RID: 2247
	protected const string SPINNING_FIREBALL_TELL_HOLD = "BurstShot_Tell_Hold";

	// Token: 0x040008C8 RID: 2248
	protected const string SPINNING_FIREBALL_ATTACK_INTRO = "BurstShot_Attack_Intro";

	// Token: 0x040008C9 RID: 2249
	protected const string SPINNING_FIREBALL_ATTACK_HOLD = "BurstShot_Attack_Hold";

	// Token: 0x040008CA RID: 2250
	protected const string SPINNING_FIREBALL_EXIT = "BurstShot_Exit";

	// Token: 0x040008CB RID: 2251
	protected const string SPINNING_FIREBALL_PROJECTILE = "EyeballBoltMinibossProjectile";

	// Token: 0x040008CC RID: 2252
	private Projectile_RL m_shoutWarningProjectile;

	// Token: 0x040008CD RID: 2253
	protected const string HOMING_FIREBALL_TELL_INTRO = "HomingShot_Tell_Intro";

	// Token: 0x040008CE RID: 2254
	protected const string HOMING_FIREBALL_TELL_HOLD = "HomingShot_Tell_Hold";

	// Token: 0x040008CF RID: 2255
	protected const string HOMING_FIREBALL_ATTACK_INTRO = "HomingShot_Attack_Intro";

	// Token: 0x040008D0 RID: 2256
	protected const string HOMING_FIREBALL_ATTACK_HOLD = "HomingShot_Attack_Hold";

	// Token: 0x040008D1 RID: 2257
	protected const string HOMING_FIREBALL_EXIT = "HomingShot_Exit";

	// Token: 0x040008D2 RID: 2258
	protected const string EXPERT_HOMING_FIREBALL_TELL_INTRO = "BounceBullets_Tell_Intro";

	// Token: 0x040008D3 RID: 2259
	protected const string EXPERT_HOMING_FIREBALL_TELL_HOLD = "BounceBullets_Tell_Hold";

	// Token: 0x040008D4 RID: 2260
	protected const string EXPERT_HOMING_FIREBALL_ATTACK_INTRO = "BounceBullets_Attack_Intro";

	// Token: 0x040008D5 RID: 2261
	protected const string EXPERT_HOMING_FIREBALL_ATTACK_HOLD = "BounceBullets_Attack_Hold";

	// Token: 0x040008D6 RID: 2262
	protected const string EXPERT_HOMING_FIREBALL_EXIT = "BounceBullets_Exit";

	// Token: 0x040008D7 RID: 2263
	protected const string HOMING_FIREBALL_PROJECTILE = "EyeballBounceBoltMinibossProjectile";

	// Token: 0x040008D8 RID: 2264
	protected const string CURSE_FIREBALL_PROJECTILE = "EyeballCurseBoltProjectile";

	// Token: 0x040008D9 RID: 2265
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x040008DA RID: 2266
	protected const string SPAWN_INTRO = "Closed_Moving";

	// Token: 0x040008DB RID: 2267
	protected const string SPAWN_OPEN = "Open";

	// Token: 0x040008DC RID: 2268
	protected static int m_spawnOrder_STATIC = 0;

	// Token: 0x040008DD RID: 2269
	protected const int NUM_OF_STARTING_EYES = 2;

	// Token: 0x040008DE RID: 2270
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x040008DF RID: 2271
	protected float m_spawn_Idle_Delay = 1f;

	// Token: 0x040008E0 RID: 2272
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x040008E1 RID: 2273
	protected float m_spawn_Intro_Delay = 0.3f;

	// Token: 0x040008E2 RID: 2274
	protected float m_spawn_Open_AnimSpeed = 1f;

	// Token: 0x040008E3 RID: 2275
	protected float m_spawn_Open_Delay = 1f;

	// Token: 0x040008E4 RID: 2276
	protected float m_secondSpawn_Idle_Delay = 1.2f;

	// Token: 0x040008E5 RID: 2277
	protected float m_secondSpawn_Intro_Delay = 0.4f;

	// Token: 0x040008E6 RID: 2278
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x040008E7 RID: 2279
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x040008E8 RID: 2280
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x040008E9 RID: 2281
	protected float m_death_Intro_Delay;

	// Token: 0x040008EA RID: 2282
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x040008EB RID: 2283
	protected float m_death_Hold_Delay = 4.5f;

	// Token: 0x040008EC RID: 2284
	private const float m_global_AnimationOverride = 1f;

	// Token: 0x040008ED RID: 2285
	private bool m_modeShifted;

	// Token: 0x040008EE RID: 2286
	private const string MODE_SHIFT_INTRO = "ModeShift_Intro";

	// Token: 0x040008EF RID: 2287
	private const string MODE_SHIFT_SCREAM_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x040008F0 RID: 2288
	private const string MODE_SHIFT_SCREAM_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x040008F1 RID: 2289
	private const string MODE_SHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x040008F2 RID: 2290
	private const float m_modeShift_Intro_AnimSpeed = 1f;

	// Token: 0x040008F3 RID: 2291
	private const float m_modeShift_Intro_Delay = 3.25f;

	// Token: 0x040008F4 RID: 2292
	private const float m_modeShift_Scream_Intro_AnimSpeed = 1f;

	// Token: 0x040008F5 RID: 2293
	private const float m_modeShift_Scream_Intro_Delay = 0f;

	// Token: 0x040008F6 RID: 2294
	private const float m_modeShift_Scream_Hold_AnimSpeed = 1f;

	// Token: 0x040008F7 RID: 2295
	private const float m_modeShift_Scream_Hold_Delay = 1.25f;

	// Token: 0x040008F8 RID: 2296
	private const float m_modeShift_Exit_AnimSpeed = 1f;

	// Token: 0x040008F9 RID: 2297
	private const float m_modeShift_Exit_Delay = 0f;

	// Token: 0x040008FA RID: 2298
	private const float m_modeShift_IdleDuration = 0.1f;

	// Token: 0x040008FB RID: 2299
	private const float m_modeShift_AttackCD = 99999f;

	// Token: 0x040008FC RID: 2300
	public const float ModeShift_HealthMod = 0.5f;

	// Token: 0x040008FD RID: 2301
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x040008FE RID: 2302
	private const float m_modeShift_openEye_Idle_AnimSpeed = 1f;

	// Token: 0x040008FF RID: 2303
	private const float m_modeShift_openEye_Idle_Delay = 0.5f;

	// Token: 0x04000900 RID: 2304
	private const float m_modeShift_openEye_Intro_AnimSpeed = 1f;

	// Token: 0x04000901 RID: 2305
	private const float m_modeShift_openEye_Intro_Delay = 0.75f;

	// Token: 0x04000902 RID: 2306
	private const float m_modeShift_openEye_Open_AnimSpeed = 1f;

	// Token: 0x04000903 RID: 2307
	private const float m_modeShift_openEye_Open_Delay = 1f;
}
