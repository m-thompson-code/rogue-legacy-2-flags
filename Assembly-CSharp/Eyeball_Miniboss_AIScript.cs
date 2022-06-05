using System;
using System.Collections;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class Eyeball_Miniboss_AIScript : Eyeball_Basic_AIScript
{
	// Token: 0x0600072B RID: 1835 RVA: 0x0005C608 File Offset: 0x0005A808
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

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x0600072C RID: 1836 RVA: 0x00005756 File Offset: 0x00003956
	// (set) Token: 0x0600072D RID: 1837 RVA: 0x0000575E File Offset: 0x0000395E
	public bool IsReady { get; private set; }

	// Token: 0x0600072E RID: 1838 RVA: 0x00005767 File Offset: 0x00003967
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

	// Token: 0x0600072F RID: 1839 RVA: 0x000057A5 File Offset: 0x000039A5
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

	// Token: 0x06000730 RID: 1840 RVA: 0x000057DB File Offset: 0x000039DB
	protected override void OnDisable()
	{
		base.OnDisable();
		if (Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC.Contains(this))
		{
			Eyeball_Miniboss_AIScript.m_eyeballSyncList_STATIC.Remove(this);
		}
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x000057FC File Offset: 0x000039FC
	private void OnDestroy()
	{
		if (base.EnemyController != null)
		{
			base.EnemyController.HealthChangeRelay.RemoveListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit));
		}
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x00005829 File Offset: 0x00003A29
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.InitializeModeShiftEye();
		base.EnemyController.HealthChangeRelay.AddListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit), false);
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x0005C67C File Offset: 0x0005A87C
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

	// Token: 0x06000734 RID: 1844 RVA: 0x0005C6D8 File Offset: 0x0005A8D8
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

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x06000735 RID: 1845 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ZoneBlast_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06000736 RID: 1846 RVA: 0x00004565 File Offset: 0x00002765
	protected virtual float ZoneBlast_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06000737 RID: 1847 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ZoneBlast_TellHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06000738 RID: 1848 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ZoneBlast_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06000739 RID: 1849 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ZoneBlast_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x0600073A RID: 1850 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ZoneBlast_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x0600073B RID: 1851 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ZoneBlast_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x0600073C RID: 1852 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ZoneBlast_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x0600073D RID: 1853 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ZoneBlast_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x0600073E RID: 1854 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float ZoneBlast_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x0600073F RID: 1855 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float ZoneBlast_Exit_AttackCD
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000320 RID: 800
	// (get) Token: 0x06000740 RID: 1856 RVA: 0x00004762 File Offset: 0x00002962
	protected virtual int ZoneBlast_Attack_Amount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x06000741 RID: 1857 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ZoneBlast_AttackHold_DelayBetweenShots
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x06000742 RID: 1858 RVA: 0x0000547D File Offset: 0x0000367D
	protected virtual float ZoneBlast_Attack_AngleSpread
	{
		get
		{
			return 75f;
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x06000743 RID: 1859 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool ZoneBlast_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06000744 RID: 1860 RVA: 0x00005856 File Offset: 0x00003A56
	public IRelayLink ShoutAttackWarningAppearedRelay
	{
		get
		{
			return this.m_shoutAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06000745 RID: 1861 RVA: 0x00005863 File Offset: 0x00003A63
	public IRelayLink ShoutAttackExplodedRelay
	{
		get
		{
			return this.m_shoutAttackExplodedRelay.link;
		}
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00005870 File Offset: 0x00003A70
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

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06000747 RID: 1863 RVA: 0x00004A07 File Offset: 0x00002C07
	protected virtual int Spinning_Fireball_Amount
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06000748 RID: 1864 RVA: 0x000054AD File Offset: 0x000036AD
	protected virtual int SpinngFireball_Angle
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x06000749 RID: 1865 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool SpinningFireball_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x0000587F File Offset: 0x00003A7F
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

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x0600074B RID: 1867 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool HomingFireball_Sync
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x0000588E File Offset: 0x00003A8E
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

	// Token: 0x0600074D RID: 1869 RVA: 0x0000589D File Offset: 0x00003A9D
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

	// Token: 0x0600074E RID: 1870 RVA: 0x000058AC File Offset: 0x00003AAC
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

	// Token: 0x0600074F RID: 1871 RVA: 0x000058BB File Offset: 0x00003ABB
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

	// Token: 0x06000750 RID: 1872 RVA: 0x000058CA File Offset: 0x00003ACA
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

	// Token: 0x06000751 RID: 1873 RVA: 0x000058D9 File Offset: 0x00003AD9
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_shoutWarningProjectile);
		base.OnLBCompleteOrCancelled();
		this.IsReady = false;
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0005A3D4 File Offset: 0x000585D4
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

	// Token: 0x06000753 RID: 1875 RVA: 0x000058F4 File Offset: 0x00003AF4
	private void StopAttackEffects()
	{
		EffectManager.DisableAllEffectWithName("EyeballBossBlueFireballTell_Effect");
		EffectManager.DisableAllEffectWithName("EyeballBossBounceBulletsTell_Effect");
		EffectManager.DisableAllEffectWithName("EyeballBossCurseTell_Effect");
	}

	// Token: 0x04000A63 RID: 2659
	private const float PROJECTILE_SPAWN_X_OFFSET = 3f;

	// Token: 0x04000A64 RID: 2660
	public const EnemyRank MODESHIFT_EYE_RANK = EnemyRank.Expert;

	// Token: 0x04000A65 RID: 2661
	private static List<Eyeball_Miniboss_AIScript> m_eyeballSyncList_STATIC = new List<Eyeball_Miniboss_AIScript>();

	// Token: 0x04000A67 RID: 2663
	protected const string ZONE_BLAST_TELL_INTRO = "AreaShot_Tell_Intro";

	// Token: 0x04000A68 RID: 2664
	protected const string ZONE_BLAST_TELL_HOLD = "AreaShot_Tell_Hold";

	// Token: 0x04000A69 RID: 2665
	protected const string ZONE_BLAST_ATTACK_INTRO = "AreaShot_Attack_Intro";

	// Token: 0x04000A6A RID: 2666
	protected const string ZONE_BLAST_ATTACK_HOLD = "AreaShot_Attack_Hold";

	// Token: 0x04000A6B RID: 2667
	protected const string ZONE_BLAST_EXIT = "AreaShot_Exit";

	// Token: 0x04000A6C RID: 2668
	protected const string EXPERT_ZONE_BLAST_TELL_INTRO = "Explosion_Tell_Intro";

	// Token: 0x04000A6D RID: 2669
	protected const string EXPERT_ZONE_BLAST_TELL_HOLD = "Explosion_Tell_Hold";

	// Token: 0x04000A6E RID: 2670
	protected const string EXPERT_ZONE_BLAST_ATTACK_INTRO = "Explosion_Attack_Intro";

	// Token: 0x04000A6F RID: 2671
	protected const string EXPERT_ZONE_BLAST_ATTACK_HOLD = "Explosion_Attack_Hold";

	// Token: 0x04000A70 RID: 2672
	protected const string EXPERT_ZONE_BLAST_EXIT = "Explosion_Exit";

	// Token: 0x04000A71 RID: 2673
	protected const string ZONE_BULLET_PROJECTILE = "EyeballPassLineBoltProjectile";

	// Token: 0x04000A72 RID: 2674
	protected const string ZONE_BLAST_PROJECTILE = "EyeballExplosionProjectile";

	// Token: 0x04000A73 RID: 2675
	protected const string ZONE_BLASTWARNING_PROJECTILE = "EyeballWarningProjectile";

	// Token: 0x04000A74 RID: 2676
	private Relay m_shoutAttackWarningAppearedRelay = new Relay();

	// Token: 0x04000A75 RID: 2677
	private Relay m_shoutAttackExplodedRelay = new Relay();

	// Token: 0x04000A76 RID: 2678
	protected const string SPINNING_FIREBALL_TELL_INTRO = "BurstShot_Tell_Intro";

	// Token: 0x04000A77 RID: 2679
	protected const string SPINNING_FIREBALL_TELL_HOLD = "BurstShot_Tell_Hold";

	// Token: 0x04000A78 RID: 2680
	protected const string SPINNING_FIREBALL_ATTACK_INTRO = "BurstShot_Attack_Intro";

	// Token: 0x04000A79 RID: 2681
	protected const string SPINNING_FIREBALL_ATTACK_HOLD = "BurstShot_Attack_Hold";

	// Token: 0x04000A7A RID: 2682
	protected const string SPINNING_FIREBALL_EXIT = "BurstShot_Exit";

	// Token: 0x04000A7B RID: 2683
	protected const string SPINNING_FIREBALL_PROJECTILE = "EyeballBoltMinibossProjectile";

	// Token: 0x04000A7C RID: 2684
	private Projectile_RL m_shoutWarningProjectile;

	// Token: 0x04000A7D RID: 2685
	protected const string HOMING_FIREBALL_TELL_INTRO = "HomingShot_Tell_Intro";

	// Token: 0x04000A7E RID: 2686
	protected const string HOMING_FIREBALL_TELL_HOLD = "HomingShot_Tell_Hold";

	// Token: 0x04000A7F RID: 2687
	protected const string HOMING_FIREBALL_ATTACK_INTRO = "HomingShot_Attack_Intro";

	// Token: 0x04000A80 RID: 2688
	protected const string HOMING_FIREBALL_ATTACK_HOLD = "HomingShot_Attack_Hold";

	// Token: 0x04000A81 RID: 2689
	protected const string HOMING_FIREBALL_EXIT = "HomingShot_Exit";

	// Token: 0x04000A82 RID: 2690
	protected const string EXPERT_HOMING_FIREBALL_TELL_INTRO = "BounceBullets_Tell_Intro";

	// Token: 0x04000A83 RID: 2691
	protected const string EXPERT_HOMING_FIREBALL_TELL_HOLD = "BounceBullets_Tell_Hold";

	// Token: 0x04000A84 RID: 2692
	protected const string EXPERT_HOMING_FIREBALL_ATTACK_INTRO = "BounceBullets_Attack_Intro";

	// Token: 0x04000A85 RID: 2693
	protected const string EXPERT_HOMING_FIREBALL_ATTACK_HOLD = "BounceBullets_Attack_Hold";

	// Token: 0x04000A86 RID: 2694
	protected const string EXPERT_HOMING_FIREBALL_EXIT = "BounceBullets_Exit";

	// Token: 0x04000A87 RID: 2695
	protected const string HOMING_FIREBALL_PROJECTILE = "EyeballBounceBoltMinibossProjectile";

	// Token: 0x04000A88 RID: 2696
	protected const string CURSE_FIREBALL_PROJECTILE = "EyeballCurseBoltProjectile";

	// Token: 0x04000A89 RID: 2697
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x04000A8A RID: 2698
	protected const string SPAWN_INTRO = "Closed_Moving";

	// Token: 0x04000A8B RID: 2699
	protected const string SPAWN_OPEN = "Open";

	// Token: 0x04000A8C RID: 2700
	protected static int m_spawnOrder_STATIC = 0;

	// Token: 0x04000A8D RID: 2701
	protected const int NUM_OF_STARTING_EYES = 2;

	// Token: 0x04000A8E RID: 2702
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04000A8F RID: 2703
	protected float m_spawn_Idle_Delay = 1f;

	// Token: 0x04000A90 RID: 2704
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04000A91 RID: 2705
	protected float m_spawn_Intro_Delay = 0.3f;

	// Token: 0x04000A92 RID: 2706
	protected float m_spawn_Open_AnimSpeed = 1f;

	// Token: 0x04000A93 RID: 2707
	protected float m_spawn_Open_Delay = 1f;

	// Token: 0x04000A94 RID: 2708
	protected float m_secondSpawn_Idle_Delay = 1.2f;

	// Token: 0x04000A95 RID: 2709
	protected float m_secondSpawn_Intro_Delay = 0.4f;

	// Token: 0x04000A96 RID: 2710
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x04000A97 RID: 2711
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x04000A98 RID: 2712
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04000A99 RID: 2713
	protected float m_death_Intro_Delay;

	// Token: 0x04000A9A RID: 2714
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04000A9B RID: 2715
	protected float m_death_Hold_Delay = 4.5f;

	// Token: 0x04000A9C RID: 2716
	private const float m_global_AnimationOverride = 1f;

	// Token: 0x04000A9D RID: 2717
	private bool m_modeShifted;

	// Token: 0x04000A9E RID: 2718
	private const string MODE_SHIFT_INTRO = "ModeShift_Intro";

	// Token: 0x04000A9F RID: 2719
	private const string MODE_SHIFT_SCREAM_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000AA0 RID: 2720
	private const string MODE_SHIFT_SCREAM_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x04000AA1 RID: 2721
	private const string MODE_SHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000AA2 RID: 2722
	private const float m_modeShift_Intro_AnimSpeed = 1f;

	// Token: 0x04000AA3 RID: 2723
	private const float m_modeShift_Intro_Delay = 3.25f;

	// Token: 0x04000AA4 RID: 2724
	private const float m_modeShift_Scream_Intro_AnimSpeed = 1f;

	// Token: 0x04000AA5 RID: 2725
	private const float m_modeShift_Scream_Intro_Delay = 0f;

	// Token: 0x04000AA6 RID: 2726
	private const float m_modeShift_Scream_Hold_AnimSpeed = 1f;

	// Token: 0x04000AA7 RID: 2727
	private const float m_modeShift_Scream_Hold_Delay = 1.25f;

	// Token: 0x04000AA8 RID: 2728
	private const float m_modeShift_Exit_AnimSpeed = 1f;

	// Token: 0x04000AA9 RID: 2729
	private const float m_modeShift_Exit_Delay = 0f;

	// Token: 0x04000AAA RID: 2730
	private const float m_modeShift_IdleDuration = 0.1f;

	// Token: 0x04000AAB RID: 2731
	private const float m_modeShift_AttackCD = 99999f;

	// Token: 0x04000AAC RID: 2732
	public const float ModeShift_HealthMod = 0.5f;

	// Token: 0x04000AAD RID: 2733
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x04000AAE RID: 2734
	private const float m_modeShift_openEye_Idle_AnimSpeed = 1f;

	// Token: 0x04000AAF RID: 2735
	private const float m_modeShift_openEye_Idle_Delay = 0.5f;

	// Token: 0x04000AB0 RID: 2736
	private const float m_modeShift_openEye_Intro_AnimSpeed = 1f;

	// Token: 0x04000AB1 RID: 2737
	private const float m_modeShift_openEye_Intro_Delay = 0.75f;

	// Token: 0x04000AB2 RID: 2738
	private const float m_modeShift_openEye_Open_AnimSpeed = 1f;

	// Token: 0x04000AB3 RID: 2739
	private const float m_modeShift_openEye_Open_Delay = 1f;
}
