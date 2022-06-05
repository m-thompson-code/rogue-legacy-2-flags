using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using RL_Windows;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class FinalBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000547 RID: 1351 RVA: 0x00016FC8 File Offset: 0x000151C8
	public IRelayLink<bool, float> ColourShiftRelay
	{
		get
		{
			return this.m_colourShiftRelay.link;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000548 RID: 1352 RVA: 0x00016FD5 File Offset: 0x000151D5
	public bool IsInWhiteMode
	{
		get
		{
			return this.m_isInWhiteMode;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06000549 RID: 1353 RVA: 0x00016FDD File Offset: 0x000151DD
	protected virtual bool m_isPrime_Version
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x0600054A RID: 1354 RVA: 0x00016FE0 File Offset: 0x000151E0
	protected virtual float m_Prime_Version_Final_Mode_Delay_Force_Idle_Add
	{
		get
		{
			return 1.35f;
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x0600054B RID: 1355 RVA: 0x00016FE7 File Offset: 0x000151E7
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.75f);
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x0600054C RID: 1356 RVA: 0x00016FF8 File Offset: 0x000151F8
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.75f, 1.15f);
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x0600054D RID: 1357 RVA: 0x00017009 File Offset: 0x00015209
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.75f, 1.15f);
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x0600054E RID: 1358 RVA: 0x0001701A File Offset: 0x0001521A
	protected override float WalkAnimSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00017024 File Offset: 0x00015224
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FinalBossWhiteTreeProjectile",
			"FinalBossBlackTreeProjectile",
			"FinalBossTreeExplosionProjectile",
			"FinalBossTreeWarningProjectile",
			"FinalBossWhiteTreeBoltProjectile",
			"FinalBossBlackTreeBoltProjectile",
			"FinalBossWhiteVerticalProjectile",
			"FinalBossVerticalCurseProjectile",
			"FinalBossVerticalCurseBlueProjectile",
			"FinalBossBlackVerticalProjectile",
			"FinalBossBlackVerticalWarningProjectile",
			"FinalBossBounceOrbVerticalProjectile",
			"FinalBossVerticalVoidSmallProjectile",
			"FinalBossBlackVerticalBoltProjectile",
			"SpellSwordAxeSpinProjectile",
			"FinalBossSwordSlashProjectile",
			"FinalBossMagmaProjectile",
			"FinalBossRollingProjectile",
			"SpellSwordSlashDownProjectile",
			"FinalBossBoneBigProjectile",
			"FinalBossBoneBigHorizontalProjectile",
			"FinalBossBoneBigDiagonalProjectile",
			"FinalBossBoneSmallProjectile",
			"FinalBossWrappedBounceSmallProjectile",
			"FinalBossWrappedVoidSmallProjectile",
			"FinalBossWrappedBounceBigProjectile",
			"FinalBossWrappedVoidBigProjectile",
			"FinalBossPotionProjectile",
			"FinalBossPotionExplosionProjectile",
			"FinalBossPortalLineProjectile",
			"FinalBossPortalBoltProjectile",
			"FinalBossPortalExplosionProjectile",
			"FinalBossPortalWarningProjectile",
			"FinalBossPortalInverseWarningProjectile",
			"FinalBossPrayerVoidProjectile",
			"FinalBossPrayerBounceProjectile",
			this.MODESHIFT_BOLT_PROJECTILE,
			this.MODESHIFT_SHOUT_PROJECTILE,
			this.MODESHIFT_SHOUT_WARNING_PROJECTILE
		};
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x00017198 File Offset: 0x00015398
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.HealthChangeRelay.AddListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit), false);
		this.m_colourShiftController = enemyController.GetComponent<FinalBossColourShiftController>();
		HitboxController hitboxController = base.EnemyController.HitboxController as HitboxController;
		foreach (GameObject gameObject in hitboxController.WeaponHitboxList)
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
		foreach (GameObject gameObject2 in hitboxController.BodyHitboxList)
		{
			if (gameObject2.name.Contains("Jumping"))
			{
				this.m_jumpingBodyHitbox = gameObject2;
			}
			else if (gameObject2.name.Contains("Praying"))
			{
				this.m_prayingBodyHitbox = gameObject2;
			}
			else
			{
				this.m_regularBodyHitbox = gameObject2;
			}
		}
		this.m_regularWeaponHitbox.SetActive(true);
		this.m_regularBodyHitbox.SetActive(true);
		this.m_jumpingWeaponHitbox.SetActive(false);
		this.m_jumpingBodyHitbox.SetActive(false);
		this.m_prayingBodyHitbox.SetActive(false);
		base.LogicController.DisableLogicActivationByDistance = true;
		this.m_model = enemyController.gameObject.FindObjectReference("Model");
		this.m_statue = enemyController.gameObject.FindObjectReference("Statue");
		this.m_statue.gameObject.SetActive(false);
		this.m_isInWhiteMode = true;
		if (!this.m_rumbleEventInstance.isValid())
		{
			this.m_rumbleEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/Cain/sfx_cain_death_finalGlow_start_rumble_loop", base.EnemyController.transform);
		}
		if (!this.m_collapseSpiritsEventInstance.isValid())
		{
			this.m_collapseSpiritsEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/Cain/sfx_cain_death_collapse_spirits_loop", base.EnemyController.transform);
		}
		if (!this.m_verticalBeamBlackEventInstance.isValid())
		{
			this.m_verticalBeamBlackEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/Cain/sfx_cain_dark_staff_attack_lightning_loop", base.EnemyController.transform);
		}
		if (!this.m_verticalBeamBlackFloorLoopEventInstance.isValid())
		{
			this.m_verticalBeamBlackFloorLoopEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/Cain/sfx_cain_dark_staff_attack_floor_loop", base.EnemyController.transform);
		}
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x000173AC File Offset: 0x000155AC
	private void OnBossHit(object sender, HealthChangeEventArgs args)
	{
		if (this.m_isModeShifting)
		{
			return;
		}
		if (this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
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
		float num = FinalBoss_Basic_AIScript.MODESHIFT_ARRAY[this.m_modeShiftIndex] * (float)base.EnemyController.ActualMaxHealth;
		if (base.EnemyController.CurrentHealth <= num)
		{
			this.m_modeShiftIndex++;
			base.LogicController.StopAllLogic(true);
			base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "Mode_Shift";
		}
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00017444 File Offset: 0x00015644
	public override void ResetScript()
	{
		this.m_isInWhiteMode = true;
		this.m_isModeShifting = false;
		this.m_modeShiftIndex = 0;
		this.m_currentColourShiftCount = 0;
		this.m_requiredColourShiftCount = 0;
		if (!GameManager.IsApplicationClosing)
		{
			this.ColourShift(true, 0f, false);
		}
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		this.StopTreePersistentCoroutine();
		this.StopPortalWhitePersistentCoroutine();
		this.StopPersistentCoroutine(this.m_swordAttackBlackSecondProjectileCoroutine);
		if (this.m_model)
		{
			this.m_model.SetActive(true);
		}
		if (this.m_statue)
		{
			this.m_statue.SetActive(false);
		}
		base.ResetScript();
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x000174EC File Offset: 0x000156EC
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.StopProjectile(ref this.m_spinningJumpProjectile);
		base.StopProjectile(ref this.m_portalWarningProj);
		base.StopProjectile(ref this.m_summonTreeWarningProjectile);
		base.StopProjectile(ref this.m_verticalBeamBlackWarningProjectile);
		base.StopProjectile(ref this.m_verticalBeamBlackProjectile);
		base.StopProjectile(ref this.m_verticalBeamBoltBlackProjectile);
		base.StopProjectile(ref this.m_modeShiftShoutWarningProj);
		base.StopProjectile(ref this.m_modeShiftShoutProj);
		if (this.m_modeShiftProjectileCoroutine != null)
		{
			base.StopCoroutine(this.m_modeShiftProjectileCoroutine);
		}
		if (this.m_delayVerticalBeamBlackCoroutine != null)
		{
			base.StopCoroutine(this.m_delayVerticalBeamBlackCoroutine);
		}
		this.m_delayVerticalBeamBlackCoroutine = null;
		if (this.m_verticalBeamBlackEventInstance.isValid())
		{
			AudioManager.Stop(this.m_verticalBeamBlackEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_verticalBeamBlackFloorLoopEventInstance.isValid())
		{
			AudioManager.Stop(this.m_verticalBeamBlackFloorLoopEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		this.m_regularWeaponHitbox.SetActive(true);
		this.m_regularBodyHitbox.SetActive(true);
		this.m_jumpingWeaponHitbox.SetActive(false);
		this.m_jumpingBodyHitbox.SetActive(false);
		this.m_prayingBodyHitbox.SetActive(false);
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x000175FC File Offset: 0x000157FC
	private void PerformColourShiftCheck()
	{
		if (this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
		{
			this.m_currentColourShiftCount++;
			if (this.m_currentColourShiftCount >= this.m_requiredColourShiftCount)
			{
				this.m_currentColourShiftCount = 0;
				this.m_requiredColourShiftCount = UnityEngine.Random.Range(FinalBoss_Basic_AIScript.COLOURSHIFT_MINMAX_COUNT.x, FinalBoss_Basic_AIScript.COLOURSHIFT_MINMAX_COUNT.y + 1);
				this.ColourShift(!this.m_isInWhiteMode, 0.1f, true);
			}
		}
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x00017671 File Offset: 0x00015871
	private void PerformForcedColourShift()
	{
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00017674 File Offset: 0x00015874
	private void PerformColourShiftCheckRandomChance()
	{
		if (this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length && CDGHelper.RandomPlusMinus() > 0)
		{
			this.m_currentColourShiftCount++;
			if (this.m_currentColourShiftCount >= this.m_requiredColourShiftCount)
			{
				this.m_currentColourShiftCount = 0;
				this.m_requiredColourShiftCount = UnityEngine.Random.Range(FinalBoss_Basic_AIScript.COLOURSHIFT_MINMAX_COUNT.x, FinalBoss_Basic_AIScript.COLOURSHIFT_MINMAX_COUNT.y + 1);
				this.ColourShift(!this.m_isInWhiteMode, 0.1f, true);
			}
		}
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x000176F1 File Offset: 0x000158F1
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
		this.m_isModeShifting = true;
		this.ToDo("Mode Shift");
		base.RemoveStatusEffects(false);
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = true;
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		base.SetVelocityX(0f, false);
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_Damage_Mod;
		yield return base.DeathAnim();
		yield return this.Default_Animation("ModeShift_Intro", this.m_modeShift_Downed_AnimSpeed, this.m_modeShift_Downed_Delay, true);
		yield return this.Default_Animation("ModeShift_GetUp", this.m_modeShift_GetUp_AnimSpeed, 0f, true);
		this.m_modeShiftShoutWarningProj = this.FireProjectile(this.MODESHIFT_SHOUT_WARNING_PROJECTILE, this.MODESHIFT_SHOUT_POSITION_INDEX, false, 0f, 1f, true, true, true);
		yield return this.Default_TellIntroAndLoop("ModeShift_Scream_Intro", this.m_modeShift_TellIntro_AnimSpeed, "ModeShift_Tell_Loop", this.m_modeShift_TellHold_AnimSpeed, this.m_modeShift_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("ModeShift_Scream_Hold", this.m_modeShift_AttackIntro_AnimSpeed, 0f, false);
		this.ColourShift(!this.m_isInWhiteMode, 0.1f, true);
		base.StopProjectile(ref this.m_modeShiftShoutWarningProj);
		this.m_modeShiftShoutProj = this.FireProjectile(this.MODESHIFT_SHOUT_PROJECTILE, this.MODESHIFT_SHOUT_POSITION_INDEX, false, 0f, 1f, true, true, true);
		this.m_modeShiftProjectileCoroutine = base.StartCoroutine(this.ModeShiftFireProjectilesCoroutine());
		yield return this.Default_Animation("ModeShift_Action_Loop", this.m_modeShift_AttackHold_AnimSpeed, this.m_modeShift_AttackHold_Delay, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		if (this.m_modeShiftProjectileCoroutine != null)
		{
			base.StopCoroutine(this.m_modeShiftProjectileCoroutine);
		}
		base.StopProjectile(ref this.m_modeShiftShoutProj);
		yield return this.Default_Animation("ModeShift_Scream_Exit", this.m_modeShift_Exit_AnimSpeed, this.m_modeShift_Exit_Delay, true);
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.ResetBaseValues();
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		yield return this.Default_Attack_Cooldown(this.m_modeShift_Exit_IdleDuration, this.m_modeShift_AttackCD);
		base.EnemyController.LockFlip = false;
		this.m_isModeShifting = false;
		yield break;
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x00017700 File Offset: 0x00015900
	private IEnumerator ModeShiftFireProjectilesCoroutine()
	{
		int num3;
		for (int i = 0; i < this.m_modeShift_Loops; i = num3 + 1)
		{
			float speedMod = UnityEngine.Random.Range(this.m_modeShift_ThrowPower.x, this.m_modeShift_ThrowPower.y);
			int num = UnityEngine.Random.Range((int)this.m_modeShift_ThrowAngle.x, (int)this.m_modeShift_ThrowAngle.y);
			float speedMod2 = UnityEngine.Random.Range(this.m_modeShift_ThrowPower.x, this.m_modeShift_ThrowPower.y);
			int num2 = UnityEngine.Random.Range((int)this.m_modeShift_ThrowAngle.x, (int)this.m_modeShift_ThrowAngle.y);
			this.FireProjectile(this.MODESHIFT_BOLT_PROJECTILE, this.MODESHIFT_POSITION_INDEX, false, (float)num, speedMod, true, true, true);
			this.FireProjectile(this.MODESHIFT_BOLT_PROJECTILE, this.MODESHIFT_POSITION_INDEX, false, (float)(180 - num2), speedMod2, true, true, true);
			float delay = this.m_modeShift_LoopDelay + Time.time;
			while (Time.time < delay)
			{
				yield return null;
			}
			num3 = i;
		}
		this.m_modeShiftProjectileCoroutine = null;
		yield break;
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x00017710 File Offset: 0x00015910
	private void ColourShift(bool shiftToWhite, float lerpSpeed, bool runEvents)
	{
		this.ToDo("Colour Shift");
		this.m_colourShiftController.ShiftColour(shiftToWhite, lerpSpeed, base.EnemyController.EnemyRank > EnemyRank.Basic);
		if (runEvents)
		{
			if (shiftToWhite)
			{
				EffectManager.PlayEffect(base.EnemyController.gameObject, base.EnemyController.Animator, "FinalBoss_DarkToLight_Effect", base.EnemyController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.SetParent(base.EnemyController.transform, true);
			}
			else
			{
				EffectManager.PlayEffect(base.EnemyController.gameObject, base.EnemyController.Animator, "FinalBoss_LightToDark_Effect", base.EnemyController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.SetParent(base.EnemyController.transform, true);
			}
			this.m_colourShiftRelay.Dispatch(shiftToWhite, lerpSpeed);
		}
		this.m_isInWhiteMode = shiftToWhite;
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x0600055A RID: 1370 RVA: 0x000177F2 File Offset: 0x000159F2
	protected string SUMMON_TREE_TELL_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PrayBlack_Tell_Intro";
			}
			return "PrayWhite_Tell_Intro";
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x0600055B RID: 1371 RVA: 0x00017807 File Offset: 0x00015A07
	protected string SUMMON_TREE_TELL_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PrayBlack_Tell_Hold";
			}
			return "PrayWhite_Tell_Hold";
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x0600055C RID: 1372 RVA: 0x0001781C File Offset: 0x00015A1C
	protected string SUMMON_TREE_ATTACK_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PrayBlack_Attack_Intro";
			}
			return "PrayWhite_Attack_Intro";
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x0600055D RID: 1373 RVA: 0x00017831 File Offset: 0x00015A31
	protected string SUMMON_TREE_ATTACK_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PrayBlack_Attack_Hold";
			}
			return "PrayWhite_Attack_Hold";
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x0600055E RID: 1374 RVA: 0x00017846 File Offset: 0x00015A46
	protected string SUMMON_TREE_EXIT
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PrayBlack_Exit";
			}
			return "PrayWhite_Exit ";
		}
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0001785B File Offset: 0x00015A5B
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator SummonTree()
	{
		if (this.m_summonTreeProjectile && this.m_summonTreeProjectile.isActiveAndEnabled && this.m_isInWhiteMode == this.m_summonedWhiteTree)
		{
			yield break;
		}
		this.ToDo("Summon Tree");
		base.SetVelocityX(0f, false);
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop(this.SUMMON_TREE_TELL_INTRO, this.m_summonTree_TellIntro_AnimSpeed, this.SUMMON_TREE_TELL_HOLD, this.m_summonTree_TellHold_AnimSpeed, this.m_summonTree_TellIntroAndHold_Delay);
		Vector2 summonTreeWarningPos = new Vector2(base.EnemyController.TargetController.Midpoint.x, base.EnemyController.Midpoint.y);
		Vector2 summonTreeSpawnPos = new Vector2(base.EnemyController.TargetController.Midpoint.x, base.EnemyController.transform.position.y);
		this.m_summonTreeWarningProjectile = this.FireProjectileAbsPos("FinalBossTreeWarningProjectile", summonTreeWarningPos, false, 0f, 1f, true, true, true);
		if (this.m_summonTree_Warning_Delay > 0f)
		{
			yield return base.Wait(this.m_summonTree_Warning_Delay, false);
		}
		base.StopProjectile(ref this.m_summonTreeWarningProjectile);
		yield return this.Default_Animation(this.SUMMON_TREE_ATTACK_INTRO, this.m_summonTree_AttackIntro_AnimSpeed, this.m_summonTree_AttackIntro_Delay, true);
		yield return this.Default_Animation(this.SUMMON_TREE_ATTACK_HOLD, this.m_summonTree_AttackHold_AnimSpeed, this.m_summonTree_AttackHold_Delay, false);
		this.FireProjectileAbsPos("FinalBossTreeExplosionProjectile", summonTreeWarningPos, false, 0f, 1f, true, true, true);
		this.StopTreePersistentCoroutine();
		this.m_treePersistentCoroutine = this.RunPersistentCoroutine(this.TreePersistentCoroutine(this.m_isInWhiteMode, summonTreeSpawnPos));
		if (this.m_summonTree_AttackHold_ExitDelay > 0f)
		{
			yield return base.Wait(this.m_summonTree_AttackHold_ExitDelay, false);
		}
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_summonTree_Exit_IdleDuration, this.m_summonTree_AttackCD);
		this.PerformColourShiftCheck();
		yield break;
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x0001786A File Offset: 0x00015A6A
	private IEnumerator TreePersistentCoroutine(bool summonWhiteTree, Vector2 treeSpawnPos)
	{
		this.m_summonedWhiteTree = summonWhiteTree;
		base.StopProjectile(ref this.m_summonTreeProjectile);
		string projectileName = "FinalBossWhiteTreeProjectile";
		if (!summonWhiteTree)
		{
			projectileName = "FinalBossBlackTreeProjectile";
		}
		this.m_summonTreeProjectile = this.FireProjectileAbsPos(projectileName, treeSpawnPos, false, 0f, 1f, true, true, true);
		yield return null;
		CameraLayerController component = this.m_summonTreeProjectile.GetComponent<CameraLayerController>();
		if (component.CameraLayer != CameraLayer.Background_ORTHO)
		{
			component.SetCameraLayer(CameraLayer.Background_ORTHO);
		}
		BlinkPulseEffect blinkPulseEffect = this.m_summonTreeProjectile.GetComponent<BlinkPulseEffect>();
		Vector2 treeMidPos = this.m_summonTreeProjectile.transform.position;
		Vector2 treeLeftPos = this.m_summonTreeProjectile.transform.position;
		Vector2 treeRightPos = this.m_summonTreeProjectile.transform.position;
		float num = 5.5f;
		float num2 = 10f;
		treeMidPos.y += num;
		treeLeftPos.y += num;
		treeLeftPos.x += num2;
		treeRightPos.y += num;
		treeRightPos.x += -num2;
		float fireDelay = this.m_treeProjectile_InitialDelay + Time.time;
		blinkPulseEffect.StartInvincibilityEffect(this.m_treeProjectile_InitialDelay);
		while (Time.time < fireDelay)
		{
			yield return null;
		}
		if (summonWhiteTree)
		{
			while (this.m_summonTreeProjectile)
			{
				if (!this.m_summonTreeProjectile.isActiveAndEnabled)
				{
					break;
				}
				this.FireProjectileAbsPos("FinalBossWhiteTreeBoltProjectile", treeLeftPos, false, 0f, 1f, true, true, true);
				this.FireProjectileAbsPos("FinalBossWhiteTreeBoltProjectile", treeRightPos, false, 180f, 1f, true, true, true);
				fireDelay = this.m_whiteTreeProjectile_Delay - 1f + Time.time;
				while (Time.time < fireDelay)
				{
					yield return null;
				}
				blinkPulseEffect.StartInvincibilityEffect(1f);
				float blinkDelay = 1f + Time.time;
				while (Time.time < blinkDelay)
				{
					yield return null;
				}
			}
		}
		else
		{
			float blinkDelay = 360f / (float)this.m_numTreeVoidProjectiles;
			while (this.m_summonTreeProjectile.isActiveAndEnabled)
			{
				for (int i = 0; i < this.m_numTreeVoidProjectiles; i++)
				{
					this.FireProjectileAbsPos("FinalBossBlackTreeBoltProjectile", treeMidPos, false, (float)i * blinkDelay, 1f, true, true, true);
					this.FireProjectileAbsPos("FinalBossBlackTreeBoltProjectile", treeMidPos, false, (float)i * blinkDelay + blinkDelay / 2f, this.m_treeVoidSpeedMod, true, true, true);
				}
				fireDelay = this.m_blackTreeProjectile_Delay + Time.time;
				while (Time.time < fireDelay)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x00017887 File Offset: 0x00015A87
	private void StopTreePersistentCoroutine()
	{
		this.StopPersistentCoroutine(this.m_treePersistentCoroutine);
		base.StopProjectile(ref this.m_summonTreeProjectile);
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000562 RID: 1378 RVA: 0x000178A1 File Offset: 0x00015AA1
	protected string PRAYER_TELL_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PrayBlack_Tell_Intro";
			}
			return "PrayWhite_Tell_Intro";
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000563 RID: 1379 RVA: 0x000178B6 File Offset: 0x00015AB6
	protected string PRAYER_TELL_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PrayBlack_Tell_Hold";
			}
			return "PrayWhite_Tell_Hold";
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000564 RID: 1380 RVA: 0x000178CB File Offset: 0x00015ACB
	protected string PRAYER_ATTACK_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PrayBlack_Attack_Intro";
			}
			return "PrayWhite_Attack_Intro";
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000565 RID: 1381 RVA: 0x000178E0 File Offset: 0x00015AE0
	protected string PRAYER_ATTACK_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PrayBlack_Attack_Hold";
			}
			return "PrayWhite_Attack_Hold";
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000566 RID: 1382 RVA: 0x000178F5 File Offset: 0x00015AF5
	protected string PRAYER_EXIT
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PrayBlack_Exit";
			}
			return "PrayWhite_Exit";
		}
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0001790A File Offset: 0x00015B0A
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Prayer()
	{
		base.SetVelocityX(0f, false);
		base.EnemyController.LockFlip = true;
		this.m_regularWeaponHitbox.SetActive(true);
		this.m_regularBodyHitbox.SetActive(false);
		this.m_jumpingWeaponHitbox.SetActive(false);
		this.m_jumpingBodyHitbox.SetActive(false);
		this.m_prayingBodyHitbox.SetActive(true);
		yield return this.Default_TellIntroAndLoop(this.PRAYER_TELL_INTRO, this.m_prayer_TellIntro_AnimSpeed, this.PRAYER_TELL_HOLD, this.m_prayer_TellHold_AnimSpeed, this.m_prayer_TellIntroAndHold_Delay);
		yield return this.Default_Animation(this.PRAYER_ATTACK_INTRO, this.m_prayer_AttackIntro_AnimSpeed, this.m_prayer_AttackIntro_Delay, true);
		yield return this.Default_Animation(this.PRAYER_ATTACK_HOLD, this.m_prayer_AttackHold_AnimSpeed, this.m_prayer_AttackHold_Delay, false);
		if (this.m_isInWhiteMode)
		{
			if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
			{
				this.PerformForcedColourShift();
				if (this.m_prayer_BounceAdvancedVoidInitialDelay > 0f)
				{
					yield return base.Wait(this.m_prayer_BounceAdvancedVoidInitialDelay, false);
				}
				this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 0f, 1f, true, true, true);
				Projectile_RL projectile_RL = this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 180f, 1f, true, true, true);
				projectile_RL.transform.SetLocalScaleX(-projectile_RL.transform.localScale.x);
			}
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_light_prayer_launch_firstWave", base.EnemyController.Midpoint);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 9, false, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 9, false, 180f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 10, false, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 10, false, 180f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 11, false, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 11, false, 180f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 12, false, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 12, false, 180f, 1f, true, true, true);
			if (this.m_isPrime_Version)
			{
				if (CDGHelper.RandomPlusMinus() > 0)
				{
					if (this.m_prayer_BounceLoopDelay > 0f)
					{
						yield return base.Wait(this.m_prayer_BounceLoopAdvancedDelay, false);
					}
					this.FireProjectile("FinalBossPrayerBounceProjectile", 12, false, 0f, 1f, true, true, true);
					this.FireProjectile("FinalBossPrayerBounceProjectile", 12, false, 180f, 1f, true, true, true);
					if (this.m_prayer_BounceLoopDelay > 0f)
					{
						yield return base.Wait(this.m_prayer_BounceLoopAdvancedDelay * 2f, false);
					}
				}
				else
				{
					if (this.m_prayer_BounceLoopDelay > 0f)
					{
						yield return base.Wait(this.m_prayer_BounceLoopAdvancedDelay * 2f, false);
					}
					this.FireProjectile("FinalBossPrayerBounceProjectile", 11, false, 0f, 1f, true, true, true);
					this.FireProjectile("FinalBossPrayerBounceProjectile", 11, false, 180f, 1f, true, true, true);
					if (this.m_prayer_BounceLoopDelay > 0f)
					{
						yield return base.Wait(this.m_prayer_BounceLoopAdvancedDelay, false);
					}
				}
			}
			else if (this.m_prayer_BounceLoopDelay > 0f)
			{
				yield return base.Wait(this.m_prayer_BounceLoopDelay, false);
			}
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_dark_prayer_launch_secondWave", base.EnemyController.Midpoint);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 9, false, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 13, false, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 13, false, 180f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 11, false, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 11, false, 180f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 9, false, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 9, false, 180f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 12, false, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossPrayerBounceProjectile", 12, false, 180f, 1f, true, true, true);
			if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
			{
				this.PerformForcedColourShift();
				if (this.m_prayer_BounceAdvancedVoidInitialDelay > 0f)
				{
					yield return base.Wait(this.m_prayer_BounceAdvancedVoidInitialDelay, false);
				}
				this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 0f, 1f, true, true, true);
				Projectile_RL projectile_RL2 = this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 180f, 1f, true, true, true);
				projectile_RL2.transform.SetLocalScaleX(-projectile_RL2.transform.localScale.x);
				if (this.m_prayer_VoidLoopAdvancedFollowUpDelay > 0f)
				{
					yield return base.Wait(this.m_prayer_VoidLoopAdvancedFollowUpDelay, false);
				}
				this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 0f, 1f, true, true, true);
				Projectile_RL projectile_RL3 = this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 180f, 1f, true, true, true);
				projectile_RL3.transform.SetLocalScaleX(-projectile_RL3.transform.localScale.x);
				if (this.m_prayer_VoidLoopAdvancedFollowUpDelay > 0f)
				{
					yield return base.Wait(this.m_prayer_VoidLoopAdvancedFollowUpDelay, false);
				}
				this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 0f, 1f, true, true, true);
				Projectile_RL projectile_RL4 = this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 180f, 1f, true, true, true);
				projectile_RL4.transform.SetLocalScaleX(-projectile_RL4.transform.localScale.x);
			}
		}
		else
		{
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_dark_prayer_launch_firstWave", base.EnemyController.Midpoint);
			this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 0f, 1f, true, true, true);
			Projectile_RL projectile_RL5 = this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 180f, 1f, true, true, true);
			projectile_RL5.transform.SetLocalScaleX(-projectile_RL5.transform.localScale.x);
			if (this.m_isPrime_Version)
			{
				if (this.m_prayer_VoidLoopAdvancedFollowUpDelay > 0f)
				{
					yield return base.Wait(this.m_prayer_VoidLoopAdvancedFollowUpDelay, false);
				}
				this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 0f, 1f, true, true, true);
				Projectile_RL projectile_RL6 = this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 180f, 1f, true, true, true);
				projectile_RL6.transform.SetLocalScaleX(-projectile_RL6.transform.localScale.x);
				if (this.m_prayer_VoidLoopAdvancedFollowUpDelay > 0f)
				{
					yield return base.Wait(this.m_prayer_VoidLoopAdvancedFollowUpDelay, false);
				}
				this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 0f, 1f, true, true, true);
				Projectile_RL projectile_RL7 = this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 180f, 1f, true, true, true);
				projectile_RL7.transform.SetLocalScaleX(-projectile_RL7.transform.localScale.x);
			}
			if (this.m_prayer_VoidLoopDelay > 0f)
			{
				yield return base.Wait(this.m_prayer_VoidLoopDelay, false);
			}
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_dark_prayer_launch_secondWave", base.EnemyController.Midpoint);
			this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 0f, 1f, true, true, true);
			Projectile_RL projectile_RL8 = this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 180f, 1f, true, true, true);
			projectile_RL8.transform.SetLocalScaleX(-projectile_RL8.transform.localScale.x);
			if (this.m_isPrime_Version)
			{
				if (this.m_prayer_VoidLoopAdvancedFollowUpDelay > 0f)
				{
					yield return base.Wait(this.m_prayer_VoidLoopAdvancedFollowUpDelay, false);
				}
				this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 0f, 1f, true, true, true);
				Projectile_RL projectile_RL9 = this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 180f, 1f, true, true, true);
				projectile_RL9.transform.SetLocalScaleX(-projectile_RL9.transform.localScale.x);
				if (this.m_prayer_VoidLoopAdvancedFollowUpDelay > 0f)
				{
					yield return base.Wait(this.m_prayer_VoidLoopAdvancedFollowUpDelay, false);
				}
				this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 0f, 1f, true, true, true);
				Projectile_RL projectile_RL10 = this.FireProjectile("FinalBossPrayerVoidProjectile", 0, false, 180f, 1f, true, true, true);
				projectile_RL10.transform.SetLocalScaleX(-projectile_RL10.transform.localScale.x);
			}
			if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
			{
				this.PerformForcedColourShift();
				if (this.m_prayer_VoidLoopAdvancedFollowUpDelay > 0f)
				{
					yield return base.Wait(this.m_prayer_VoidLoopAdvancedFollowUpDelay, false);
				}
				this.FireProjectile("FinalBossPrayerBounceProjectile", 9, false, 0f, 1f, true, true, true);
				this.FireProjectile("FinalBossPrayerBounceProjectile", 9, false, 180f, 1f, true, true, true);
				this.FireProjectile("FinalBossPrayerBounceProjectile", 10, false, 0f, 1f, true, true, true);
				this.FireProjectile("FinalBossPrayerBounceProjectile", 10, false, 180f, 1f, true, true, true);
				this.FireProjectile("FinalBossPrayerBounceProjectile", 11, false, 0f, 1f, true, true, true);
				this.FireProjectile("FinalBossPrayerBounceProjectile", 11, false, 180f, 1f, true, true, true);
				this.FireProjectile("FinalBossPrayerBounceProjectile", 12, false, 0f, 1f, true, true, true);
				this.FireProjectile("FinalBossPrayerBounceProjectile", 12, false, 180f, 1f, true, true, true);
			}
		}
		yield return this.Default_Animation(this.PRAYER_EXIT, this.m_prayer_Exit_AnimSpeed, this.m_prayer_Exit_Delay, true);
		this.m_regularWeaponHitbox.SetActive(true);
		this.m_regularBodyHitbox.SetActive(true);
		this.m_jumpingWeaponHitbox.SetActive(false);
		this.m_jumpingBodyHitbox.SetActive(false);
		this.m_prayingBodyHitbox.SetActive(false);
		if (this.m_prayer_AttackHold_ExitDelay > 0f)
		{
			yield return base.Wait(this.m_prayer_AttackHold_ExitDelay, false);
		}
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
		{
			yield return this.Default_Attack_Cooldown(this.m_Prime_Version_Final_Mode_Delay_Force_Idle_Add + this.m_prayer_Exit_IdleDuration, this.m_prayer_AttackCD);
		}
		else
		{
			yield return this.Default_Attack_Cooldown(this.m_prayer_Exit_IdleDuration, this.m_prayer_AttackCD);
		}
		this.PerformColourShiftCheck();
		yield break;
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06000568 RID: 1384 RVA: 0x00017919 File Offset: 0x00015B19
	protected string VERTICAL_BEAM_TELL_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "RaiseWeaponBlack_Tell_Intro";
			}
			return "RaiseWeaponWhite_Tell_Intro";
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06000569 RID: 1385 RVA: 0x0001792E File Offset: 0x00015B2E
	protected string VERTICAL_BEAM_TELL_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "RaiseWeaponBlack_Tell_Hold";
			}
			return "RaiseWeaponWhite_Tell_Hold";
		}
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x0600056A RID: 1386 RVA: 0x00017943 File Offset: 0x00015B43
	protected string VERTICAL_BEAM_ATTACK_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "RaiseWeaponBlack_Attack_Intro";
			}
			return "RaiseWeaponWhite_Attack_Intro";
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x0600056B RID: 1387 RVA: 0x00017958 File Offset: 0x00015B58
	protected string VERTICAL_BEAM_ATTACK_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "RaiseWeaponBlack_Attack_Hold";
			}
			return "RaiseWeaponWhite_Attack_Hold";
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x0600056C RID: 1388 RVA: 0x0001796D File Offset: 0x00015B6D
	protected string VERTICAL_BEAM_EXIT
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "RaiseWeaponBlack_Exit";
			}
			return "RaiseWeaponWhite_Exit";
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x0600056D RID: 1389 RVA: 0x00017982 File Offset: 0x00015B82
	protected virtual int m_numVerticalBeamProjectiles
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x0600056E RID: 1390 RVA: 0x00017986 File Offset: 0x00015B86
	protected virtual float m_numVerticalBeamAdvancedFollowUpDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x0600056F RID: 1391 RVA: 0x0001798D File Offset: 0x00015B8D
	protected virtual int m_numVerticalBeamAdvancedFollowUpProjectiles
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x00017990 File Offset: 0x00015B90
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator VerticalBeam()
	{
		this.ToDo("Vertical Beam");
		base.SetVelocityX(0f, false);
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop(this.VERTICAL_BEAM_TELL_INTRO, this.m_verticalBeam_TellIntro_AnimSpeed, this.VERTICAL_BEAM_TELL_HOLD, this.m_verticalBeam_TellHold_AnimSpeed, this.m_verticalBeam_TellIntroAndHold_Delay);
		yield return this.Default_Animation(this.VERTICAL_BEAM_ATTACK_INTRO, this.m_verticalBeam_AttackIntro_AnimSpeed, this.m_verticalBeam_AttackIntro_Delay, true);
		yield return this.Default_Animation(this.VERTICAL_BEAM_ATTACK_HOLD, this.m_verticalBeam_AttackHold_AnimSpeed, this.m_verticalBeam_AttackHold_Delay, false);
		if (this.m_isInWhiteMode)
		{
			float fireInterval = this.m_verticalBeam_AttackDuration / (float)this.m_numVerticalBeamProjectiles;
			float startingAngle = (float)UnityEngine.Random.Range(0, 360);
			int num = CDGHelper.RandomPlusMinus();
			float fireAngleInterval = (float)(this.m_verticalBeamFireballSpread / (this.m_numVerticalBeamProjectiles - 1) * num);
			int num2;
			for (int i = 0; i < this.m_numVerticalBeamProjectiles; i = num2 + 1)
			{
				float angle = startingAngle + fireAngleInterval * (float)i;
				this.FireProjectile("FinalBossWhiteVerticalProjectile", 2, false, angle, 1f, true, true, true);
				if (this.m_isPrime_Version && i % 6 == 5)
				{
					float angle2 = (float)UnityEngine.Random.Range(0, 360);
					this.FireProjectile("FinalBossVerticalCurseProjectile", 2, false, angle2, 1f, true, true, true);
				}
				yield return base.Wait(fireInterval, false);
				num2 = i;
			}
			if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
			{
				this.PerformForcedColourShift();
				Vector2 projPos = new Vector2(base.EnemyController.Room.BoundsRect.x, base.EnemyController.transform.localPosition.y);
				this.m_verticalBeamBlackWarningProjectile = this.FireProjectileAbsPos("FinalBossBlackVerticalWarningProjectile", projPos, false, 0f, 1f, true, true, true);
				if (this.m_verticalBeamBlackInitialDelay > 0f)
				{
					yield return base.Wait(this.m_verticalBeamBlackInitialDelay, false);
				}
				base.StopProjectile(ref this.m_verticalBeamBlackWarningProjectile);
				this.m_verticalBeamBlackProjectile = this.FireProjectileAbsPos("FinalBossBlackVerticalProjectile", projPos, false, 0f, 1f, true, true, true);
				float num3 = 1f;
				Vector2 pos = base.EnemyController.transform.position;
				if (base.EnemyController.IsFacingRight)
				{
					pos.x += num3;
				}
				else
				{
					pos.x -= num3;
				}
				this.m_verticalBeamBoltBlackProjectile = this.FireProjectileAbsPos("FinalBossBlackVerticalBoltProjectile", pos, false, 0f, 1f, true, true, true);
				Vector3 absoluteSpawnPositionAtIndex = base.GetAbsoluteSpawnPositionAtIndex(2, false);
				float angle3 = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - absoluteSpawnPositionAtIndex);
				if (CDGHelper.RandomPlusMinus() > 0)
				{
					this.FireProjectile("FinalBossBounceOrbVerticalProjectile", 2, false, angle3, 1f, true, true, true);
				}
				else
				{
					this.FireProjectile("FinalBossVerticalVoidSmallProjectile", 2, false, angle3, 1f, true, true, true);
				}
				projPos = default(Vector2);
			}
		}
		else
		{
			Vector2 projPos = new Vector2(base.EnemyController.Room.BoundsRect.x, base.EnemyController.transform.localPosition.y);
			this.m_verticalBeamBlackWarningProjectile = this.FireProjectileAbsPos("FinalBossBlackVerticalWarningProjectile", projPos, false, 0f, 1f, true, true, true);
			if (this.m_delayVerticalBeamBlackCoroutine != null)
			{
				base.StopCoroutine(this.m_delayVerticalBeamBlackCoroutine);
			}
			this.m_delayVerticalBeamBlackCoroutine = base.StartCoroutine(this.PlayDelayedVerticalBeamBlackSFX(1f));
			if (this.m_verticalBeamBlackInitialDelay > 0f)
			{
				yield return base.Wait(this.m_verticalBeamBlackInitialDelay, false);
			}
			base.StopProjectile(ref this.m_verticalBeamBlackWarningProjectile);
			this.m_verticalBeamBlackProjectile = this.FireProjectileAbsPos("FinalBossBlackVerticalProjectile", projPos, false, 0f, 1f, true, true, true);
			float num4 = 1f;
			Vector2 pos2 = base.EnemyController.transform.position;
			if (base.EnemyController.IsFacingRight)
			{
				pos2.x += num4;
			}
			else
			{
				pos2.x -= num4;
			}
			this.m_verticalBeamBoltBlackProjectile = this.FireProjectileAbsPos("FinalBossBlackVerticalBoltProjectile", pos2, false, 0f, 1f, true, true, true);
			Vector3 vertBeamPos = base.GetAbsoluteSpawnPositionAtIndex(2, false);
			float angle4 = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - vertBeamPos);
			this.FireProjectile("FinalBossBounceOrbVerticalProjectile", 2, false, angle4, 1f, true, true, true);
			yield return base.Wait(this.m_verticalBeamBlackLoopDelay, false);
			if (this.m_isPrime_Version)
			{
				if (CDGHelper.RandomPlusMinus() > 0)
				{
					angle4 = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - vertBeamPos);
					this.FireProjectile("FinalBossVerticalVoidSmallProjectile", 2, false, angle4, 1f, true, true, true);
					yield return base.Wait(this.m_verticalBeamBlackLoopDelay, false);
					angle4 = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - vertBeamPos);
					this.FireProjectile("FinalBossBounceOrbVerticalProjectile", 2, false, angle4, 1f, true, true, true);
					yield return base.Wait(this.m_verticalBeamBlackLoopDelay, false);
				}
				else
				{
					angle4 = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - vertBeamPos);
					this.FireProjectile("FinalBossBounceOrbVerticalProjectile", 2, false, angle4, 1f, true, true, true);
					yield return base.Wait(this.m_verticalBeamBlackLoopDelay, false);
					angle4 = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - vertBeamPos);
					this.FireProjectile("FinalBossVerticalVoidSmallProjectile", 2, false, angle4, 1f, true, true, true);
					yield return base.Wait(this.m_verticalBeamBlackLoopDelay, false);
				}
			}
			else
			{
				angle4 = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - vertBeamPos);
				this.FireProjectile("FinalBossBounceOrbVerticalProjectile", 2, false, angle4, 1f, true, true, true);
				yield return base.Wait(this.m_verticalBeamBlackLoopDelay, false);
				angle4 = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - vertBeamPos);
				this.FireProjectile("FinalBossBounceOrbVerticalProjectile", 2, false, angle4, 1f, true, true, true);
				yield return base.Wait(this.m_verticalBeamBlackLoopDelay, false);
			}
			if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
			{
				this.PerformForcedColourShift();
				yield return base.Wait(this.m_numVerticalBeamAdvancedFollowUpDelay, false);
				this.FireProjectile("FinalBossWhiteVerticalProjectile", 2, false, 65f, 1f, true, true, true);
				this.FireProjectile("FinalBossWhiteVerticalProjectile", 2, false, 130f, 1f, true, true, true);
				this.FireProjectile("FinalBossWhiteVerticalProjectile", 2, false, 195f, 1f, true, true, true);
				this.FireProjectile("FinalBossWhiteVerticalProjectile", 2, false, 260f, 1f, true, true, true);
				this.FireProjectile("FinalBossWhiteVerticalProjectile", 2, false, 325f, 1f, true, true, true);
			}
			projPos = default(Vector2);
			vertBeamPos = default(Vector3);
		}
		if (this.m_verticalBeam_AttackHold_ExitDelay > 0f)
		{
			yield return base.Wait(this.m_verticalBeam_AttackHold_ExitDelay, false);
		}
		base.StopProjectile(ref this.m_verticalBeamBlackProjectile);
		base.StopProjectile(ref this.m_verticalBeamBoltBlackProjectile);
		if (!this.m_isInWhiteMode)
		{
			if (this.m_verticalBeamBlackEventInstance.isValid())
			{
				AudioManager.Stop(this.m_verticalBeamBlackEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			if (this.m_verticalBeamBlackFloorLoopEventInstance.isValid())
			{
				AudioManager.Stop(this.m_verticalBeamBlackFloorLoopEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_dark_staff_attack_lightning_end", base.EnemyController.Midpoint);
		}
		yield return this.Default_Animation(this.VERTICAL_BEAM_EXIT, this.m_verticalBeam_Exit_AnimSpeed, this.m_verticalBeam_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
		{
			yield return this.Default_Attack_Cooldown(this.m_Prime_Version_Final_Mode_Delay_Force_Idle_Add + this.m_prayer_Exit_IdleDuration, this.m_verticalBeam_AttackCD);
		}
		else
		{
			yield return this.Default_Attack_Cooldown(this.m_verticalBeam_Exit_IdleDuration, this.m_verticalBeam_AttackCD);
		}
		this.PerformColourShiftCheck();
		yield break;
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0001799F File Offset: 0x00015B9F
	private IEnumerator PlayDelayedVerticalBeamBlackSFX(float delay)
	{
		delay += Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		if (this.m_verticalBeamBlackEventInstance.isValid())
		{
			AudioManager.Play(null, this.m_verticalBeamBlackEventInstance);
		}
		if (this.m_verticalBeamBlackFloorLoopEventInstance.isValid())
		{
			AudioManager.Play(null, this.m_verticalBeamBlackFloorLoopEventInstance);
		}
		this.m_delayVerticalBeamBlackCoroutine = null;
		yield break;
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000572 RID: 1394 RVA: 0x000179B5 File Offset: 0x00015BB5
	protected string SPINNING_JUMP_TELL_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "JumpAttackBlack_Tell_Intro";
			}
			return "JumpAttackWhite_Tell_Intro";
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06000573 RID: 1395 RVA: 0x000179CA File Offset: 0x00015BCA
	protected string SPINNING_JUMP_TELL_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "JumpAttackBlack_Tell_Hold";
			}
			return "JumpAttackWhite_Tell_Hold";
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06000574 RID: 1396 RVA: 0x000179DF File Offset: 0x00015BDF
	protected string SPINNING_JUMP_ATTACK_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "JumpAttackBlack_Attack_Intro";
			}
			return "JumpAttackWhite_Attack_Intro";
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06000575 RID: 1397 RVA: 0x000179F4 File Offset: 0x00015BF4
	protected string SPINNING_JUMP_ATTACK_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "JumpAttackBlack_Attack_Hold";
			}
			return "JumpAttackWhite_Attack_Hold";
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000576 RID: 1398 RVA: 0x00017A09 File Offset: 0x00015C09
	protected string SPINNING_JUMP_LAND
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "JumpAttackBlack_Land";
			}
			return "JumpAttackWhite_Land";
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000577 RID: 1399 RVA: 0x00017A1E File Offset: 0x00015C1E
	protected string SPINNING_JUMP_EXIT
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "JumpAttackBlack_Exit";
			}
			return "JumpAttackWhite_Exit";
		}
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00017A33 File Offset: 0x00015C33
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator SpinningJump()
	{
		this.ToDo("Spinning Jump Attack");
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop(this.SPINNING_JUMP_TELL_INTRO, this.m_spinningJump_TellIntro_AnimSpeed, this.SPINNING_JUMP_TELL_HOLD, this.m_spinningJump_TellHold_AnimSpeed, this.m_spinningJump_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		Vector2 spinningJump_AttackVelocity = this.m_spinningJump_AttackVelocity;
		if (!base.EnemyController.IsFacingRight)
		{
			spinningJump_AttackVelocity.x = -spinningJump_AttackVelocity.x;
		}
		base.SetVelocity(spinningJump_AttackVelocity, false);
		this.m_regularWeaponHitbox.SetActive(false);
		this.m_regularBodyHitbox.SetActive(false);
		this.m_jumpingWeaponHitbox.SetActive(true);
		this.m_jumpingBodyHitbox.SetActive(true);
		this.m_prayingBodyHitbox.SetActive(false);
		yield return this.Default_Animation(this.SPINNING_JUMP_ATTACK_INTRO, this.m_spinningJump_AttackIntro_AnimSpeed, this.m_spinningJump_AttackIntro_Delay, true);
		yield return base.Wait(0.1f, false);
		yield return this.Default_Animation(this.SPINNING_JUMP_ATTACK_HOLD, this.m_spinningJump_AttackHold_AnimSpeed, this.m_spinningJump_AttackHold_Delay, false);
		if (this.m_spinningJump_Spawn_AxeSpin)
		{
			this.m_spinningJumpProjectile = this.FireProjectile("SpellSwordAxeSpinProjectile", 3, false, 0f, 1f, true, true, true);
		}
		while (base.EnemyController.Velocity.y > 0f)
		{
			yield return null;
		}
		if (this.m_isInWhiteMode)
		{
			AudioManager.PlayOneShotAttached(null, "event:/SFX/Enemies/Cain/sfx_cain_light_jump_preLand", base.EnemyController.gameObject);
		}
		else
		{
			AudioManager.PlayOneShotAttached(null, "event:/SFX/Enemies/Cain/sfx_cain_dark_jump_preLand", base.EnemyController.gameObject);
		}
		yield return base.WaitUntilIsGrounded();
		this.m_regularWeaponHitbox.SetActive(true);
		this.m_regularBodyHitbox.SetActive(true);
		this.m_jumpingWeaponHitbox.SetActive(false);
		this.m_jumpingBodyHitbox.SetActive(false);
		this.m_prayingBodyHitbox.SetActive(false);
		this.FireProjectile("FinalBossSwordSlashProjectile", 4, true, 0f, 1f, true, true, true);
		if (this.m_spinningJump_Spawn_AxeSpin)
		{
			base.StopProjectile(ref this.m_spinningJumpProjectile);
		}
		if (this.m_isInWhiteMode)
		{
			float num = UnityEngine.Random.Range(this.m_spinningJump_Land_ThrowPower.x, this.m_spinningJump_Land_ThrowPower.y);
			int num2 = UnityEngine.Random.Range((int)this.m_spinningJump_Land_ThrowAngle.x, (int)this.m_spinningJump_Land_ThrowAngle.y);
			int num3 = UnityEngine.Random.Range((int)this.m_spinningJump_Land_ThrowAngle2.x, (int)this.m_spinningJump_Land_ThrowAngle2.y);
			int num4 = UnityEngine.Random.Range((int)this.m_spinningJump_Land_ThrowAngle3.x, (int)this.m_spinningJump_Land_ThrowAngle3.y);
			int num5 = UnityEngine.Random.Range((int)this.m_spinningJump_Land_ThrowAngle4.x, (int)this.m_spinningJump_Land_ThrowAngle4.y);
			this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)num2, num, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(180 - num2), num, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)num3, num, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(180 - num3), num, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 6, true, (float)num4, num, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 5, true, (float)(180 - num4), num, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 6, true, (float)num5, num, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 5, true, (float)(180 - num5), num, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(num2 + 3), num * this.m_spinningJump_Land_ThrowPowerHighMod, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(180 - num2 - 3), num * this.m_spinningJump_Land_ThrowPowerHighMod, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(num3 + 7), num * this.m_spinningJump_Land_ThrowPowerHighMod, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(180 - num3 - 7), num * this.m_spinningJump_Land_ThrowPowerHighMod, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 6, true, (float)(num4 + 8), num * this.m_spinningJump_Land_ThrowPowerHighMod, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 5, true, (float)(180 - num4 - 8), num * this.m_spinningJump_Land_ThrowPowerHighMod, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 6, true, (float)(num5 + 12), num * this.m_spinningJump_Land_ThrowPowerHighMod, true, true, true);
			this.FireProjectile("FinalBossMagmaProjectile", 5, true, (float)(180 - num5 - 12), num * this.m_spinningJump_Land_ThrowPowerHighMod, true, true, true);
			if (this.m_isPrime_Version)
			{
				this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(num2 + 3), num * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(180 - num2 - 3), num * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(num3 + 7), num * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(180 - num3 - 7), num * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 6, true, (float)(num4 + 8), num * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 5, true, (float)(180 - num4 - 8), num * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 6, true, (float)(num5 + 12), num * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 5, true, (float)(180 - num5 - 12), num * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
			}
			if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
			{
				this.FireProjectile("FinalBossRollingProjectile", 0, false, 0f, 1f, true, true, true).Flip();
				this.FireProjectile("FinalBossRollingProjectile", 0, false, 180f, 1f, true, true, true);
			}
		}
		else
		{
			Projectile_RL projectile_RL = this.FireProjectile("FinalBossRollingProjectile", 0, false, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossRollingProjectile", 0, false, 0f, this.m_spinningJump_Land_RollingPowerMod, true, true, true);
			if (this.m_isPrime_Version)
			{
				this.FireProjectile("FinalBossRollingProjectile", 0, false, 0f, this.m_spinningJump_Land_RollingPowerModAdvanced, true, true, true);
			}
			projectile_RL.Flip();
			this.FireProjectile("FinalBossRollingProjectile", 0, false, 180f, 1f, true, true, true);
			this.FireProjectile("FinalBossRollingProjectile", 0, false, 180f, this.m_spinningJump_Land_RollingPowerMod, true, true, true);
			if (this.m_isPrime_Version)
			{
				this.FireProjectile("FinalBossRollingProjectile", 0, false, 180f, this.m_spinningJump_Land_RollingPowerModAdvanced, true, true, true);
			}
			if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
			{
				this.PerformForcedColourShift();
				float num6 = UnityEngine.Random.Range(this.m_spinningJump_Land_ThrowPower.x, this.m_spinningJump_Land_ThrowPower.y);
				int num7 = UnityEngine.Random.Range((int)this.m_spinningJump_Land_ThrowAngle.x, (int)this.m_spinningJump_Land_ThrowAngle.y);
				int num8 = UnityEngine.Random.Range((int)this.m_spinningJump_Land_ThrowAngle2.x, (int)this.m_spinningJump_Land_ThrowAngle2.y);
				int num9 = UnityEngine.Random.Range((int)this.m_spinningJump_Land_ThrowAngle3.x, (int)this.m_spinningJump_Land_ThrowAngle3.y);
				int num10 = UnityEngine.Random.Range((int)this.m_spinningJump_Land_ThrowAngle4.x, (int)this.m_spinningJump_Land_ThrowAngle4.y);
				this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(num7 + 3), num6 * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(180 - num7 - 3), num6 * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(num8 + 7), num6 * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 0, true, (float)(180 - num8 - 7), num6 * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 6, true, (float)(num9 + 8), num6 * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 5, true, (float)(180 - num9 - 8), num6 * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 6, true, (float)(num10 + 12), num6 * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
				this.FireProjectile("FinalBossMagmaProjectile", 5, true, (float)(180 - num10 - 12), num6 * this.m_spinningJump_Land_ThrowPowerHighModAdvanced, true, true, true);
			}
		}
		base.SetVelocityX(0f, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation(this.SPINNING_JUMP_LAND, this.m_spinningJump_Exit_AnimSpeed, this.m_spinningJump_Exit_Delay, true);
		if (this.m_spinningJump_ExitHold_Delay > 0f)
		{
			yield return base.Wait(this.m_spinningJump_ExitHold_Delay, false);
		}
		yield return this.Default_Animation(this.SPINNING_JUMP_EXIT, this.m_spinningJump_Exit_AnimSpeed, this.m_spinningJump_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
		{
			yield return this.Default_Attack_Cooldown(this.m_Prime_Version_Final_Mode_Delay_Force_Idle_Add + this.m_prayer_Exit_IdleDuration, this.m_spinningJump_AttackCD);
		}
		else
		{
			yield return this.Default_Attack_Cooldown(this.m_spinningJump_Exit_IdleDuration, this.m_spinningJump_AttackCD);
		}
		this.PerformColourShiftCheck();
		yield break;
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06000579 RID: 1401 RVA: 0x00017A42 File Offset: 0x00015C42
	protected string SWORD_TELL_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "SwordAttackBlack_Tell_Intro";
			}
			return "SwordAttackWhite_Tell_Intro";
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x0600057A RID: 1402 RVA: 0x00017A57 File Offset: 0x00015C57
	protected string SWORD_TELL_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "SwordAttackBlack_Tell_Hold";
			}
			return "SwordAttackWhite_Tell_Hold";
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x0600057B RID: 1403 RVA: 0x00017A6C File Offset: 0x00015C6C
	protected string SWORD_ATTACK_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "SwordAttackBlack_Attack_Intro";
			}
			return "SwordAttackWhite_Attack_Intro";
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x0600057C RID: 1404 RVA: 0x00017A81 File Offset: 0x00015C81
	protected string SWORD_ATTACK_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "SwordAttackBlack_Attack_Hold";
			}
			return "SwordAttackWhite_Attack_Hold";
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x0600057D RID: 1405 RVA: 0x00017A96 File Offset: 0x00015C96
	protected string SWORD_EXIT
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "SwordAttackBlack_Exit";
			}
			return "SwordAttackWhite_Exit";
		}
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x00017AAB File Offset: 0x00015CAB
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Sword_Attack()
	{
		this.ToDo("Sword Attack");
		base.SetVelocityX(0f, false);
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		float dashSpeed = this.m_sword_AttackSpeed;
		if (!base.EnemyController.IsTargetToMyRight)
		{
			dashSpeed = -dashSpeed;
		}
		yield return this.Default_TellIntroAndLoop(this.SWORD_TELL_INTRO, this.m_sword_TellIntro_AnimSpeed, this.SWORD_TELL_HOLD, this.m_sword_TellHold_AnimSpeed, this.m_sword_TellIntroAndHold_Delay);
		yield return this.Default_Animation(this.SWORD_ATTACK_INTRO, this.m_sword_AttackIntro_AnimSpeed, this.m_sword_AttackIntro_Delay, true);
		yield return this.Default_Animation(this.SWORD_ATTACK_HOLD, this.m_sword_AttackHold_AnimSpeed, this.m_sword_AttackHold_Delay, false);
		this.FireProjectile("FinalBossSwordSlashProjectile", 17, true, 0f, 1f, true, true, true);
		if (this.m_isInWhiteMode)
		{
			Projectile_RL projectile_RL = this.FireProjectile("FinalBossBoneBigProjectile", 7, true, 0f, 1f, true, true, true);
			projectile_RL.RotationSpeed = projectile_RL.InitialRotationSpeed;
			Projectile_RL projectile_RL2 = this.FireProjectile("FinalBossBoneBigHorizontalProjectile", 7, true, 0f, 1f, true, true, true);
			projectile_RL2.RotationSpeed = projectile_RL2.InitialRotationSpeed;
			if (this.m_isPrime_Version)
			{
				Projectile_RL projectile_RL3 = this.FireProjectile("FinalBossBoneBigDiagonalProjectile", 7, true, 0f, 1f, true, true, true);
				projectile_RL3.RotationSpeed = projectile_RL3.InitialRotationSpeed;
			}
			this.FireProjectile("FinalBossBoneSmallProjectile", 8, true, 60f, 1f, true, true, true);
			this.FireProjectile("FinalBossBoneSmallProjectile", 8, true, 55f, 1f, true, true, true);
			this.FireProjectile("FinalBossBoneSmallProjectile", 8, true, 50f, 1f, true, true, true);
			if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
			{
				this.PerformForcedColourShift();
				this.FireProjectile("FinalBossPotionProjectile", 8, true, 60f, 1f, true, true, true);
			}
		}
		else
		{
			this.FireProjectile("FinalBossWrappedBounceBigProjectile", 15, true, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossWrappedVoidSmallProjectile", 15, true, 0f, 1f, true, true, true);
			this.FireProjectile("FinalBossWrappedBounceBigProjectile", 16, true, 180f, 1f, true, true, true);
			this.FireProjectile("FinalBossWrappedVoidSmallProjectile", 16, true, 180f, 1f, true, true, true);
			if (this.m_isPrime_Version)
			{
				this.FireProjectile("FinalBossPotionProjectile", 8, true, 60f, 1f, true, true, true);
			}
			if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
			{
				this.PerformForcedColourShift();
				this.FireProjectile("FinalBossBoneSmallProjectile", 8, true, 60f, 1f, true, true, true);
				this.FireProjectile("FinalBossBoneSmallProjectile", 8, true, 55f, 1f, true, true, true);
				this.FireProjectile("FinalBossBoneSmallProjectile", 8, true, 50f, 1f, true, true, true);
			}
		}
		base.SetVelocityX(dashSpeed, false);
		if (this.m_sword_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_sword_AttackDuration, false);
		}
		base.SetVelocityX(0f, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.SetVelocityX(0f, false);
		yield return this.Default_Animation(this.SWORD_EXIT, this.m_sword_Exit_AnimSpeed, this.m_sword_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
		{
			yield return this.Default_Attack_Cooldown(this.m_Prime_Version_Final_Mode_Delay_Force_Idle_Add + this.m_prayer_Exit_IdleDuration, this.m_sword_AttackCD);
		}
		else
		{
			yield return this.Default_Attack_Cooldown(this.m_sword_Exit_IdleDuration, this.m_sword_AttackCD);
		}
		this.PerformColourShiftCheck();
		yield break;
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00017ABA File Offset: 0x00015CBA
	private IEnumerator SwordAttackBlackSecondProjectileCoroutine()
	{
		float delay = Time.time + this.m_sword_BlackProjectile_Delay;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.FireProjectile("FinalBossWrappedVoidBigProjectile", 15, true, 0f, 1f, true, true, true);
		yield break;
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06000580 RID: 1408 RVA: 0x00017AC9 File Offset: 0x00015CC9
	protected string PORTAL_TELL_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PortalAttackBlack_Tell_Intro";
			}
			return "PortalAttackWhite_Tell_Intro";
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06000581 RID: 1409 RVA: 0x00017ADE File Offset: 0x00015CDE
	protected string PORTAL_TELL_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PortalAttackBlack_Tell_Hold";
			}
			return "PortalAttackWhite_Tell_Hold";
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06000582 RID: 1410 RVA: 0x00017AF3 File Offset: 0x00015CF3
	protected string PORTAL_ATTACK_INTRO
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PortalAttackBlack_Attack_Intro";
			}
			return "PortalAttackWhite_Attack_Intro";
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06000583 RID: 1411 RVA: 0x00017B08 File Offset: 0x00015D08
	protected string PORTAL_ATTACK_HOLD
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PortalAttackBlack_Attack_Hold";
			}
			return "PortalAttackWhite_Attack_Hold";
		}
	}

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06000584 RID: 1412 RVA: 0x00017B1D File Offset: 0x00015D1D
	protected string PORTAL_EXIT
	{
		get
		{
			if (!this.m_isInWhiteMode)
			{
				return "PortalAttackBlack_Exit";
			}
			return "PortalAttackWhite_Exit";
		}
	}

	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06000585 RID: 1413 RVA: 0x00017B32 File Offset: 0x00015D32
	protected virtual int Portal_Outward_Explosion_Amount
	{
		get
		{
			return 12;
		}
	}

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06000586 RID: 1414 RVA: 0x00017B36 File Offset: 0x00015D36
	protected virtual int Portal_Outward_Explosion_Angle
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06000587 RID: 1415 RVA: 0x00017B3A File Offset: 0x00015D3A
	protected virtual int Portal_Explosion_Bolt_Amount
	{
		get
		{
			return 15;
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06000588 RID: 1416 RVA: 0x00017B3E File Offset: 0x00015D3E
	protected virtual int Portal_Explosion_Bolt_Angle
	{
		get
		{
			return 24;
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06000589 RID: 1417 RVA: 0x00017B42 File Offset: 0x00015D42
	protected virtual float Portal_Explosion_Bolt_Delay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x0600058A RID: 1418 RVA: 0x00017B49 File Offset: 0x00015D49
	protected virtual float Portal_Advanced_Black_To_White_FollowUp_Delay
	{
		get
		{
			return 1.55f;
		}
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x0600058B RID: 1419 RVA: 0x00017B50 File Offset: 0x00015D50
	protected virtual float Portal_Advanced_White_To_Black_FollowUp_Delay
	{
		get
		{
			return 1.55f;
		}
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00017B57 File Offset: 0x00015D57
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Portal_Attack()
	{
		this.ToDo("Portal Attack");
		base.SetVelocityX(0f, false);
		base.EnemyController.LockFlip = true;
		Vector2 projSpawnPos = base.EnemyController.TargetController.Midpoint;
		if (this.m_isInWhiteMode)
		{
			this.m_portalWarningProj = this.FireProjectileAbsPos("FinalBossPortalInverseWarningProjectile", projSpawnPos, false, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_portalWarningProj = this.FireProjectileAbsPos("FinalBossPortalWarningProjectile", projSpawnPos, false, 0f, 1f, true, true, true);
		}
		yield return this.Default_TellIntroAndLoop(this.PORTAL_TELL_INTRO, this.m_portal_TellIntro_AnimSpeed, this.PORTAL_TELL_HOLD, this.m_portal_TellHold_AnimSpeed, this.m_portal_TellIntroAndHold_Delay);
		yield return this.Default_Animation(this.PORTAL_ATTACK_INTRO, this.m_portal_AttackIntro_AnimSpeed, this.m_portal_AttackIntro_Delay, true);
		yield return this.Default_Animation(this.PORTAL_ATTACK_HOLD, this.m_portal_AttackHold_AnimSpeed, this.m_portal_AttackHold_Delay, false);
		base.StopProjectile(ref this.m_portalWarningProj);
		this.StopPortalWhitePersistentCoroutine();
		if (this.m_isInWhiteMode)
		{
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_light_voidWave_aoe_explode", projSpawnPos);
			for (int i = 0; i <= this.Portal_Outward_Explosion_Amount; i++)
			{
				Vector2 b = CDGHelper.RotatedPoint(new Vector2(8.75f, 0f), Vector2.zero, (float)(i * this.Portal_Outward_Explosion_Angle));
				this.FireProjectileAbsPos("FinalBossPortalLineProjectile", projSpawnPos + b, false, (float)(i * this.Portal_Outward_Explosion_Angle), 1f, true, true, true);
			}
			if (this.m_isPrime_Version)
			{
				yield return base.Wait(0.25f, false);
				for (int j = 0; j <= this.Portal_Outward_Explosion_Amount; j++)
				{
					Vector2 b2 = CDGHelper.RotatedPoint(new Vector2(7.5f, 0f), Vector2.zero, (float)(j * this.Portal_Outward_Explosion_Angle));
					this.FireProjectileAbsPos("FinalBossPortalLineProjectile", projSpawnPos + b2, false, (float)(j * this.Portal_Outward_Explosion_Angle), 1f, true, true, true);
				}
			}
			if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
			{
				this.PerformForcedColourShift();
				this.m_portalWarningProj = this.FireProjectileAbsPos("FinalBossPortalWarningProjectile", projSpawnPos, false, 0f, 1f, true, true, true);
				if (this.Portal_Advanced_White_To_Black_FollowUp_Delay != 0f)
				{
					yield return base.Wait(this.Portal_Advanced_White_To_Black_FollowUp_Delay, false);
				}
				base.StopProjectile(ref this.m_portalWarningProj);
				this.FireProjectileAbsPos("FinalBossPortalExplosionProjectile", projSpawnPos, false, 0f, 1f, true, true, true);
				int portal_Outward_Explosion_Angle = this.Portal_Outward_Explosion_Angle;
				int portal_Outward_Explosion_Amount = this.Portal_Outward_Explosion_Amount;
				for (int k = 0; k <= this.Portal_Explosion_Bolt_Amount; k++)
				{
					this.FireProjectileAbsPos("FinalBossPortalBoltProjectile", projSpawnPos, false, (float)(k * this.Portal_Explosion_Bolt_Angle), 1f, true, true, true);
				}
			}
		}
		else
		{
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_dark_voidWave_aoe_explode_first", projSpawnPos);
			this.FireProjectileAbsPos("FinalBossPortalExplosionProjectile", projSpawnPos, false, 0f, 1f, true, true, true);
			int portal_Outward_Explosion_Angle2 = this.Portal_Outward_Explosion_Angle;
			int portal_Outward_Explosion_Amount2 = this.Portal_Outward_Explosion_Amount;
			for (int l = 0; l <= this.Portal_Explosion_Bolt_Amount; l++)
			{
				this.FireProjectileAbsPos("FinalBossPortalBoltProjectile", projSpawnPos, false, (float)(l * this.Portal_Explosion_Bolt_Angle), 1f, true, true, true);
			}
			if (this.Portal_Explosion_Bolt_Delay != 0f)
			{
				yield return base.Wait(this.Portal_Explosion_Bolt_Delay, false);
			}
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_dark_voidWave_aoe_explode_second", projSpawnPos);
			for (int m = 0; m <= this.Portal_Explosion_Bolt_Amount; m++)
			{
				this.FireProjectileAbsPos("FinalBossPortalBoltProjectile", projSpawnPos, false, (float)(m * this.Portal_Explosion_Bolt_Angle + this.Portal_Explosion_Bolt_Angle / 2), 1f, true, true, true);
			}
			if (this.m_isPrime_Version)
			{
				if (this.Portal_Explosion_Bolt_Delay != 0f)
				{
					yield return base.Wait(this.Portal_Explosion_Bolt_Delay, false);
				}
				for (int n = 0; n <= this.Portal_Explosion_Bolt_Amount; n++)
				{
					this.FireProjectileAbsPos("FinalBossPortalBoltProjectile", projSpawnPos, false, (float)(n * this.Portal_Explosion_Bolt_Angle), 1f, true, true, true);
				}
			}
			if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
			{
				this.PerformForcedColourShift();
				this.m_portalWarningProj = this.FireProjectileAbsPos("FinalBossPortalInverseWarningProjectile", projSpawnPos, false, 0f, 1f, true, true, true);
				if (this.Portal_Advanced_Black_To_White_FollowUp_Delay != 0f)
				{
					yield return base.Wait(this.Portal_Advanced_Black_To_White_FollowUp_Delay, false);
				}
				base.StopProjectile(ref this.m_portalWarningProj);
				for (int num = 0; num <= this.Portal_Outward_Explosion_Amount; num++)
				{
					Vector2 b3 = CDGHelper.RotatedPoint(new Vector2(7.5f, 0f), Vector2.zero, (float)(num * this.Portal_Outward_Explosion_Angle));
					this.FireProjectileAbsPos("FinalBossPortalLineProjectile", projSpawnPos + b3, false, (float)(num * this.Portal_Outward_Explosion_Angle), 1f, true, true, true);
				}
			}
		}
		yield return this.Default_Animation(this.PORTAL_EXIT, this.m_portal_Exit_AnimSpeed, this.m_portal_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		if (this.m_isPrime_Version && this.m_modeShiftIndex >= FinalBoss_Basic_AIScript.MODESHIFT_ARRAY.Length)
		{
			yield return this.Default_Attack_Cooldown(this.m_Prime_Version_Final_Mode_Delay_Force_Idle_Add + this.m_prayer_Exit_IdleDuration, this.m_portal_AttackCD);
		}
		else
		{
			yield return this.Default_Attack_Cooldown(this.m_portal_Exit_IdleDuration, this.m_portal_AttackCD);
		}
		this.PerformColourShiftCheck();
		yield break;
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x00017B66 File Offset: 0x00015D66
	private IEnumerator PortalWhiteAttackPersistentCoroutine(Vector2 spawnPos)
	{
		int flipper = CDGHelper.RandomPlusMinus();
		int SpinningFireballCycles = this.Portal_Outward_Explosion_Angle * this.Portal_Outward_Explosion_Amount;
		for (int i = 0; i <= SpinningFireballCycles; i += this.Portal_Outward_Explosion_Angle)
		{
			this.FireProjectileAbsPos("FinalBossPortalLineProjectile", spawnPos, false, (float)(i * flipper), 1f, true, true, true);
			this.FireProjectileAbsPos("FinalBossPortalLineProjectile", spawnPos, false, (float)(180 + i * flipper), 1f, true, true, true);
			yield return base.Wait(0.175f, false);
		}
		yield break;
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x00017B7C File Offset: 0x00015D7C
	private void StopPortalWhitePersistentCoroutine()
	{
		base.StopProjectile(ref this.m_portalProjectile);
		this.StopPersistentCoroutine(this.m_portalWhiteAttackPersistentCoroutine);
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x00017B96 File Offset: 0x00015D96
	public override IEnumerator DeathAnim()
	{
		FinalBossRoomController finalRoom = base.EnemyController.Room.GetComponent<FinalBossRoomController>();
		if (finalRoom)
		{
			finalRoom.ColourShiftToDefault();
		}
		yield return base.DeathAnim();
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_death_start", CameraController.GameCamera.transform.position);
		yield return this.Default_Animation("Death_Intro", this.m_death_Intro_AnimSpeed, this.m_death_Intro_Delay, true);
		yield return base.Wait(0.5f, true);
		AudioManager.PlayOneShot(this, "event:/SFX/Enemies/Cain/vo_cain_monster_greet", base.transform.position);
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_CAIN_NAME_1", "LOC_ID_CAIN_ENDING_VENGEANCE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		if (!WindowManager.GetIsWindowLoaded(WindowID.Dialogue))
		{
			WindowManager.LoadWindow(WindowID.Dialogue);
		}
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		yield return base.Wait(0.25f, true);
		if (this.m_rumbleEventInstance.isValid())
		{
			AudioManager.Play(null, this.m_rumbleEventInstance);
		}
		if (this.m_collapseSpiritsEventInstance.isValid())
		{
			AudioManager.Play(null, this.m_collapseSpiritsEventInstance);
		}
		yield return this.Default_Animation("Death", this.m_death_Hold_AnimSpeed, this.m_death_Hold_Delay - 1.8f, false);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_boss_cain_death_screenFlash", CameraController.GameCamera.transform.position);
		EffectManager.PlayEffect(base.gameObject, null, "ScreenFlashEffect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		yield return base.Wait(1.5f, true);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_boss_cain_death_screenFlash", CameraController.GameCamera.transform.position);
		EffectManager.PlayEffect(base.gameObject, null, "ScreenFlashEffect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		yield return base.Wait(0.3f, true);
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 0.25f);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_boss_cain_death_screenFlash", CameraController.GameCamera.transform.position);
		EffectManager.PlayEffect(base.gameObject, null, "ScreenFlashEffect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_death_finalGlow_flash_turnToStone", base.EnemyController.Midpoint);
		if (this.m_collapseSpiritsEventInstance.isValid())
		{
			AudioManager.Stop(this.m_collapseSpiritsEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_death_spirits_end", base.EnemyController.Midpoint);
		this.m_statue.gameObject.SetActive(true);
		this.m_statue.gameObject.SetLayerRecursively(0, false);
		this.m_model.SetActive(false);
		if (this.m_rumbleEventInstance.isValid())
		{
			AudioManager.Stop(this.m_rumbleEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		finalRoom.SmashWindows();
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/vo_cain_death", base.EnemyController.Midpoint);
		foreach (BaseEffect baseEffect in EffectManager.GetEffectList("SpellswordBoss_DeathScream_Effect"))
		{
			baseEffect.Stop(EffectStopType.Gracefully);
		}
		foreach (BaseEffect baseEffect2 in EffectManager.GetEffectList("CameraShakeSmall_Effect"))
		{
			baseEffect2.Stop(EffectStopType.Gracefully);
		}
		yield return base.Wait(0.5f, false);
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		yield break;
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x00017BA5 File Offset: 0x00015DA5
	private IEnumerator SoulsEscapeSFXCoroutine()
	{
		if (this.m_collapseSpiritsEventInstance.isValid())
		{
			AudioManager.Play(null, this.m_collapseSpiritsEventInstance);
		}
		BaseEffect deathScreamEffect = EffectManager.GetEffectList("FinalBoss_DeathScream_Effect")[0];
		while (deathScreamEffect && deathScreamEffect.IsPlaying)
		{
			yield return null;
		}
		if (this.m_collapseSpiritsEventInstance.isValid())
		{
			AudioManager.Stop(this.m_collapseSpiritsEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_death_spirits_end", base.EnemyController.Midpoint);
		yield break;
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00017BB4 File Offset: 0x00015DB4
	public override IEnumerator SpawnAnim()
	{
		if (!SaveManager.PlayerSaveData.SpokenToFinalBoss)
		{
			yield return this.ChangeAnimationState("Transform");
			yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
			MusicManager.PlayMusic(SongID.FinalBoss_ASITP, false, false);
			yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		}
		else
		{
			MusicManager.PlayMusic(SongID.FinalBoss_ASITP, false, false);
			yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		}
		yield break;
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00017BC4 File Offset: 0x00015DC4
	protected override void OnDisable()
	{
		if (this.m_rumbleEventInstance.isValid())
		{
			AudioManager.Stop(this.m_rumbleEventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
		if (this.m_collapseSpiritsEventInstance.isValid())
		{
			AudioManager.Stop(this.m_collapseSpiritsEventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
		if (this.m_verticalBeamBlackEventInstance.isValid())
		{
			AudioManager.Stop(this.m_verticalBeamBlackEventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
		if (this.m_verticalBeamBlackFloorLoopEventInstance.isValid())
		{
			AudioManager.Stop(this.m_verticalBeamBlackFloorLoopEventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
		base.OnDisable();
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x00017C3C File Offset: 0x00015E3C
	private void OnDestroy()
	{
		if (base.EnemyController)
		{
			base.EnemyController.HealthChangeRelay.RemoveListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit));
		}
		if (this.m_rumbleEventInstance.isValid())
		{
			this.m_rumbleEventInstance.release();
		}
		if (this.m_collapseSpiritsEventInstance.isValid())
		{
			this.m_collapseSpiritsEventInstance.release();
		}
		if (this.m_verticalBeamBlackEventInstance.isValid())
		{
			this.m_verticalBeamBlackEventInstance.release();
		}
		if (this.m_verticalBeamBlackFloorLoopEventInstance.isValid())
		{
			this.m_verticalBeamBlackFloorLoopEventInstance.release();
		}
	}

	// Token: 0x04000908 RID: 2312
	public const bool START_IN_WHITE_MODE = true;

	// Token: 0x04000909 RID: 2313
	protected const string VERTICAL_BEAM_BLACK_STOP_SFX_NAME = "event:/SFX/Enemies/Cain/sfx_cain_dark_staff_attack_lightning_end";

	// Token: 0x0400090A RID: 2314
	protected const string VERTICAL_BEAM_BLACK_LOOP_SFX_NAME = "event:/SFX/Enemies/Cain/sfx_cain_dark_staff_attack_lightning_loop";

	// Token: 0x0400090B RID: 2315
	protected const string VERTICAL_BEAM_BLACK_FLOOR_LOOP_SFX_NAME = "event:/SFX/Enemies/Cain/sfx_cain_dark_staff_attack_floor_loop";

	// Token: 0x0400090C RID: 2316
	private Relay<bool, float> m_colourShiftRelay = new Relay<bool, float>();

	// Token: 0x0400090D RID: 2317
	private static float[] MODESHIFT_ARRAY = new float[]
	{
		0.75f,
		0.5f
	};

	// Token: 0x0400090E RID: 2318
	private static Vector2Int COLOURSHIFT_MINMAX_COUNT = new Vector2Int(1, 2);

	// Token: 0x0400090F RID: 2319
	private EventInstance m_rumbleEventInstance;

	// Token: 0x04000910 RID: 2320
	private EventInstance m_collapseSpiritsEventInstance;

	// Token: 0x04000911 RID: 2321
	private EventInstance m_verticalBeamBlackEventInstance;

	// Token: 0x04000912 RID: 2322
	private EventInstance m_verticalBeamBlackFloorLoopEventInstance;

	// Token: 0x04000913 RID: 2323
	private bool m_isInWhiteMode;

	// Token: 0x04000914 RID: 2324
	private bool m_isModeShifting;

	// Token: 0x04000915 RID: 2325
	protected int m_modeShiftIndex;

	// Token: 0x04000916 RID: 2326
	private int m_currentColourShiftCount;

	// Token: 0x04000917 RID: 2327
	private int m_requiredColourShiftCount;

	// Token: 0x04000918 RID: 2328
	private GameObject m_regularWeaponHitbox;

	// Token: 0x04000919 RID: 2329
	private GameObject m_jumpingWeaponHitbox;

	// Token: 0x0400091A RID: 2330
	private GameObject m_regularBodyHitbox;

	// Token: 0x0400091B RID: 2331
	private GameObject m_jumpingBodyHitbox;

	// Token: 0x0400091C RID: 2332
	private GameObject m_prayingBodyHitbox;

	// Token: 0x0400091D RID: 2333
	private FinalBossColourShiftController m_colourShiftController;

	// Token: 0x0400091E RID: 2334
	private GameObject m_statue;

	// Token: 0x0400091F RID: 2335
	private GameObject m_model;

	// Token: 0x04000920 RID: 2336
	protected const string MODESHIFT_DOWNED = "ModeShift_Intro";

	// Token: 0x04000921 RID: 2337
	protected const string MODESHIFT_GETUP = "ModeShift_GetUp";

	// Token: 0x04000922 RID: 2338
	protected const string MODESHIFT_TELL_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000923 RID: 2339
	protected const string MODESHIFT_TELL_HOLD = "ModeShift_Tell_Loop";

	// Token: 0x04000924 RID: 2340
	protected const string MODESHIFT_ATTACK_INTRO = "ModeShift_Scream_Hold";

	// Token: 0x04000925 RID: 2341
	protected const string MODESHIFT_ATTACK_HOLD = "ModeShift_Action_Loop";

	// Token: 0x04000926 RID: 2342
	protected const string MODESHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000927 RID: 2343
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x04000928 RID: 2344
	protected float m_modeShift_Downed_AnimSpeed = 1f;

	// Token: 0x04000929 RID: 2345
	protected float m_modeShift_Downed_Delay;

	// Token: 0x0400092A RID: 2346
	protected float m_modeShift_GetUp_AnimSpeed = 1f;

	// Token: 0x0400092B RID: 2347
	protected float m_modeShift_TellIntro_AnimSpeed = 1.25f;

	// Token: 0x0400092C RID: 2348
	protected float m_modeShift_TellHold_AnimSpeed = 1.25f;

	// Token: 0x0400092D RID: 2349
	protected float m_modeShift_TellIntroAndHold_Delay = 2.5f;

	// Token: 0x0400092E RID: 2350
	protected float m_modeShift_AttackIntro_AnimSpeed = 1.25f;

	// Token: 0x0400092F RID: 2351
	protected float m_modeShift_AttackHold_AnimSpeed = 1.25f;

	// Token: 0x04000930 RID: 2352
	protected float m_modeShift_AttackHold_Delay = 1.5f;

	// Token: 0x04000931 RID: 2353
	protected float m_modeShift_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000932 RID: 2354
	protected float m_modeShift_Exit_Delay;

	// Token: 0x04000933 RID: 2355
	protected float m_modeShift_Exit_IdleDuration = 0.1f;

	// Token: 0x04000934 RID: 2356
	protected float m_modeShift_AttackCD = 99f;

	// Token: 0x04000935 RID: 2357
	protected string MODESHIFT_BOLT_PROJECTILE = "FinalBossModeShiftBoltProjectile";

	// Token: 0x04000936 RID: 2358
	protected string MODESHIFT_SHOUT_PROJECTILE = "FinalBossModeShiftShoutProjectile";

	// Token: 0x04000937 RID: 2359
	protected string MODESHIFT_SHOUT_WARNING_PROJECTILE = "FinalBossModeShiftShoutWarningProjectile";

	// Token: 0x04000938 RID: 2360
	protected int MODESHIFT_SHOUT_POSITION_INDEX = 1;

	// Token: 0x04000939 RID: 2361
	protected int MODESHIFT_POSITION_INDEX = 14;

	// Token: 0x0400093A RID: 2362
	protected Vector2 m_modeShift_ThrowAngle = new Vector2(30f, 85f);

	// Token: 0x0400093B RID: 2363
	protected Vector2 m_modeShift_ThrowPower = new Vector2(1f, 2.65f);

	// Token: 0x0400093C RID: 2364
	protected float m_modeShift_LoopDelay = 0.225f;

	// Token: 0x0400093D RID: 2365
	protected int m_modeShift_Loops = 15;

	// Token: 0x0400093E RID: 2366
	private Projectile_RL m_modeShiftShoutWarningProj;

	// Token: 0x0400093F RID: 2367
	private Projectile_RL m_modeShiftShoutProj;

	// Token: 0x04000940 RID: 2368
	private Coroutine m_modeShiftProjectileCoroutine;

	// Token: 0x04000941 RID: 2369
	protected const string SUMMON_TREE_EXPLOSION_PROJECTILE = "FinalBossTreeExplosionProjectile";

	// Token: 0x04000942 RID: 2370
	protected const string SUMMON_TREE_WARNING_PROJECTILE = "FinalBossTreeWarningProjectile";

	// Token: 0x04000943 RID: 2371
	protected const string SUMMON_TREE_PROJECTILE_WHITE = "FinalBossWhiteTreeProjectile";

	// Token: 0x04000944 RID: 2372
	protected const string SUMMON_TREE_PROJECTILE_BLACK = "FinalBossBlackTreeProjectile";

	// Token: 0x04000945 RID: 2373
	protected const string TREE_CHASE_PROJECTILE = "FinalBossWhiteTreeBoltProjectile";

	// Token: 0x04000946 RID: 2374
	protected const string TREE_VOID_PROJECTILE = "FinalBossBlackTreeBoltProjectile";

	// Token: 0x04000947 RID: 2375
	protected float m_summonTree_AttackDuration = 2f;

	// Token: 0x04000948 RID: 2376
	protected float m_summonTree_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000949 RID: 2377
	protected float m_summonTree_TellHold_AnimSpeed = 1.2f;

	// Token: 0x0400094A RID: 2378
	protected float m_summonTree_TellIntroAndHold_Delay = 0.95f;

	// Token: 0x0400094B RID: 2379
	protected float m_summonTree_Warning_Delay = 1f;

	// Token: 0x0400094C RID: 2380
	protected float m_summonTree_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x0400094D RID: 2381
	protected float m_summonTree_AttackIntro_Delay;

	// Token: 0x0400094E RID: 2382
	protected float m_summonTree_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x0400094F RID: 2383
	protected float m_summonTree_AttackHold_Delay;

	// Token: 0x04000950 RID: 2384
	protected float m_summonTree_AttackHold_ExitDelay = 0.75f;

	// Token: 0x04000951 RID: 2385
	protected float m_summonTree_Exit_AnimSpeed = 1.2f;

	// Token: 0x04000952 RID: 2386
	protected float m_summonTree_Exit_Delay;

	// Token: 0x04000953 RID: 2387
	protected float m_summonTree_Exit_IdleDuration = 0.1f;

	// Token: 0x04000954 RID: 2388
	protected float m_summonTree_AttackCD = 12f;

	// Token: 0x04000955 RID: 2389
	protected float m_treeProjectile_InitialDelay = 2.5f;

	// Token: 0x04000956 RID: 2390
	protected float m_whiteTreeProjectile_Delay = 8f;

	// Token: 0x04000957 RID: 2391
	protected float m_blackTreeProjectile_Delay = 8f;

	// Token: 0x04000958 RID: 2392
	protected int m_numTreeVoidProjectiles = 8;

	// Token: 0x04000959 RID: 2393
	protected float m_treeVoidSpeedMod = 0.65f;

	// Token: 0x0400095A RID: 2394
	private Projectile_RL m_summonTreeProjectile;

	// Token: 0x0400095B RID: 2395
	private Projectile_RL m_summonTreeWarningProjectile;

	// Token: 0x0400095C RID: 2396
	private Coroutine m_treePersistentCoroutine;

	// Token: 0x0400095D RID: 2397
	private bool m_summonedWhiteTree;

	// Token: 0x0400095E RID: 2398
	protected const string PRAYER_EXPLOSION_PROJECTILE = "FinalBossTreeExplosionProjectile";

	// Token: 0x0400095F RID: 2399
	protected const string PRAYER_WARNING_PROJECTILE = "FinalBossTreeWarningProjectile";

	// Token: 0x04000960 RID: 2400
	protected const string PRAYER_VOID_PROJECTILE = "FinalBossPrayerVoidProjectile";

	// Token: 0x04000961 RID: 2401
	protected const string PRAYER_BOUNCE_PROJECTILE = "FinalBossPrayerBounceProjectile";

	// Token: 0x04000962 RID: 2402
	protected const int PRAYER_VOID_POS = 0;

	// Token: 0x04000963 RID: 2403
	protected const int PRAYER_BOUNCE_POS_1 = 9;

	// Token: 0x04000964 RID: 2404
	protected const int PRAYER_BOUNCE_POS_2 = 10;

	// Token: 0x04000965 RID: 2405
	protected const int PRAYER_BOUNCE_POS_3 = 11;

	// Token: 0x04000966 RID: 2406
	protected const int PRAYER_BOUNCE_POS_4 = 12;

	// Token: 0x04000967 RID: 2407
	protected const int PRAYER_BOUNCE_POS_5 = 13;

	// Token: 0x04000968 RID: 2408
	protected float m_prayer_AttackDuration = 2f;

	// Token: 0x04000969 RID: 2409
	protected float m_prayer_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x0400096A RID: 2410
	protected float m_prayer_TellHold_AnimSpeed = 1.2f;

	// Token: 0x0400096B RID: 2411
	protected float m_prayer_TellIntroAndHold_Delay = 1.25f;

	// Token: 0x0400096C RID: 2412
	protected float m_prayer_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x0400096D RID: 2413
	protected float m_prayer_AttackIntro_Delay;

	// Token: 0x0400096E RID: 2414
	protected float m_prayer_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x0400096F RID: 2415
	protected float m_prayer_AttackHold_Delay;

	// Token: 0x04000970 RID: 2416
	protected float m_prayer_AttackHold_ExitDelay;

	// Token: 0x04000971 RID: 2417
	protected float m_prayer_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000972 RID: 2418
	protected float m_prayer_Exit_Delay;

	// Token: 0x04000973 RID: 2419
	protected float m_prayer_Exit_IdleDuration = 0.1f;

	// Token: 0x04000974 RID: 2420
	protected float m_prayer_AttackCD = 12f;

	// Token: 0x04000975 RID: 2421
	protected float m_prayer_VoidLoopDelay = 0.75f;

	// Token: 0x04000976 RID: 2422
	protected float m_prayer_VoidLoopAdvancedFollowUpDelay = 0.25f;

	// Token: 0x04000977 RID: 2423
	protected float m_prayer_BounceLoopDelay = 1f;

	// Token: 0x04000978 RID: 2424
	protected float m_prayer_BounceLoopAdvancedDelay = 0.35f;

	// Token: 0x04000979 RID: 2425
	protected float m_prayer_BounceAdvancedVoidInitialDelay = 0.85f;

	// Token: 0x0400097A RID: 2426
	protected const string VERTICAL_BEAM_PROJECTILE_WHITE = "FinalBossWhiteVerticalProjectile";

	// Token: 0x0400097B RID: 2427
	protected const string VERTICAL_BEAM_PROJECTILE_BLACK = "FinalBossBlackVerticalProjectile";

	// Token: 0x0400097C RID: 2428
	protected const string VERTICAL_BEAM_VERTICAL_BOLT_PROJECTILE_BLACK = "FinalBossBlackVerticalBoltProjectile";

	// Token: 0x0400097D RID: 2429
	protected const string VERTICAL_BEAM_BLACK_WARNING_PROJECTILE = "FinalBossBlackVerticalWarningProjectile";

	// Token: 0x0400097E RID: 2430
	protected const string VERTICAL_BEAM_BLACK_BOUNCE = "FinalBossBounceOrbVerticalProjectile";

	// Token: 0x0400097F RID: 2431
	protected const string VERTICAL_BEAM_BLACK_VOID = "FinalBossVerticalVoidSmallProjectile";

	// Token: 0x04000980 RID: 2432
	protected const string VERTICAL_CURSE_PROJECTILE = "FinalBossVerticalCurseProjectile";

	// Token: 0x04000981 RID: 2433
	protected const string VERTICAL_CURSE_BLUE_PROJECTILE = "FinalBossVerticalCurseBlueProjectile";

	// Token: 0x04000982 RID: 2434
	protected const int VERTICAL_BEAM_WHITE_POS_INDEX = 2;

	// Token: 0x04000983 RID: 2435
	protected float m_verticalBeam_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000984 RID: 2436
	protected float m_verticalBeam_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000985 RID: 2437
	protected float m_verticalBeam_TellIntroAndHold_Delay = 1.1f;

	// Token: 0x04000986 RID: 2438
	protected float m_verticalBeam_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000987 RID: 2439
	protected float m_verticalBeam_AttackIntro_Delay;

	// Token: 0x04000988 RID: 2440
	protected float m_verticalBeam_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000989 RID: 2441
	protected float m_verticalBeam_AttackHold_Delay;

	// Token: 0x0400098A RID: 2442
	protected float m_verticalBeam_AttackHold_ExitDelay = 0.45f;

	// Token: 0x0400098B RID: 2443
	protected float m_verticalBeam_Exit_AnimSpeed = 1.2f;

	// Token: 0x0400098C RID: 2444
	protected float m_verticalBeam_Exit_Delay;

	// Token: 0x0400098D RID: 2445
	protected float m_verticalBeam_Exit_IdleDuration = 0.1f;

	// Token: 0x0400098E RID: 2446
	protected float m_verticalBeam_AttackCD = 12f;

	// Token: 0x0400098F RID: 2447
	protected float m_verticalBeam_AttackDuration = 2f;

	// Token: 0x04000990 RID: 2448
	protected int m_verticalBeamFireballSpread = 720;

	// Token: 0x04000991 RID: 2449
	protected float m_verticalBeamBlackInitialDelay = 1.35f;

	// Token: 0x04000992 RID: 2450
	protected float m_verticalBeamBlackLoopDelay = 0.6f;

	// Token: 0x04000993 RID: 2451
	protected Projectile_RL m_verticalBeamBlackWarningProjectile;

	// Token: 0x04000994 RID: 2452
	protected Projectile_RL m_verticalBeamBlackProjectile;

	// Token: 0x04000995 RID: 2453
	protected Projectile_RL m_verticalBeamBoltBlackProjectile;

	// Token: 0x04000996 RID: 2454
	private Coroutine m_delayVerticalBeamBlackCoroutine;

	// Token: 0x04000997 RID: 2455
	protected const string SPINNING_JUMP_AXE_SPIN_PROJECTILE = "SpellSwordAxeSpinProjectile";

	// Token: 0x04000998 RID: 2456
	protected const string SPINNING_JUMP_SWORD_SLASH_PROJECTILE = "FinalBossSwordSlashProjectile";

	// Token: 0x04000999 RID: 2457
	protected const string SPINNING_JUMP_LAND_PROJECTILE_MAGMA = "FinalBossMagmaProjectile";

	// Token: 0x0400099A RID: 2458
	protected const string SPINNING_JUMP_LAND_PROJECTILE_ROLLING = "FinalBossRollingProjectile";

	// Token: 0x0400099B RID: 2459
	protected const int SPINNING_JUMP_SPIN_PROJ_INDEX = 3;

	// Token: 0x0400099C RID: 2460
	protected const int SPINNING_JUMP_LAND_SWORD_PROJ_INDEX = 4;

	// Token: 0x0400099D RID: 2461
	protected const int SPINNING_JUMP_LAND_CENTRE_PROJ_INDEX = 0;

	// Token: 0x0400099E RID: 2462
	protected const int SPINNING_JUMP_LAND_BACK_PROJ_INDEX = 5;

	// Token: 0x0400099F RID: 2463
	protected const int SPINNING_JUMP_LAND_FRONT_PROJ_INDEX = 6;

	// Token: 0x040009A0 RID: 2464
	protected float m_jump_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x040009A1 RID: 2465
	protected float m_jump_TellIntroAndHold_Delay = 0.65f;

	// Token: 0x040009A2 RID: 2466
	protected float m_spinningJump_TellIntro_AnimSpeed = 1.5f;

	// Token: 0x040009A3 RID: 2467
	protected float m_spinningJump_TellHold_AnimSpeed = 1.5f;

	// Token: 0x040009A4 RID: 2468
	protected float m_spinningJump_TellIntroAndHold_Delay = 0.85f;

	// Token: 0x040009A5 RID: 2469
	protected float m_spinningJump_AttackIntro_AnimSpeed = 1.5f;

	// Token: 0x040009A6 RID: 2470
	protected float m_spinningJump_AttackIntro_Delay;

	// Token: 0x040009A7 RID: 2471
	protected float m_spinningJump_AttackHold_AnimSpeed = 1.5f;

	// Token: 0x040009A8 RID: 2472
	protected float m_spinningJump_AttackHold_Delay;

	// Token: 0x040009A9 RID: 2473
	protected float m_spinningJump_AttackHold_ExitDelay;

	// Token: 0x040009AA RID: 2474
	protected float m_spinningJump_ExitHold_Delay = 0.375f;

	// Token: 0x040009AB RID: 2475
	protected float m_spinningJump_ExitHold_AnimSpeed = 1.2f;

	// Token: 0x040009AC RID: 2476
	protected float m_spinningJump_Exit_AnimSpeed = 1.2f;

	// Token: 0x040009AD RID: 2477
	protected float m_spinningJump_Exit_Delay;

	// Token: 0x040009AE RID: 2478
	protected float m_spinningJump_Exit_IdleDuration = 0.1f;

	// Token: 0x040009AF RID: 2479
	protected float m_spinningJump_AttackCD = 12f;

	// Token: 0x040009B0 RID: 2480
	protected Vector2 m_spinningJump_Land_ThrowAngle = new Vector2(85f, 85f);

	// Token: 0x040009B1 RID: 2481
	protected Vector2 m_spinningJump_Land_ThrowAngle2 = new Vector2(76f, 76f);

	// Token: 0x040009B2 RID: 2482
	protected Vector2 m_spinningJump_Land_ThrowAngle3 = new Vector2(71f, 71f);

	// Token: 0x040009B3 RID: 2483
	protected Vector2 m_spinningJump_Land_ThrowAngle4 = new Vector2(55f, 55f);

	// Token: 0x040009B4 RID: 2484
	protected Vector2 m_spinningJump_Land_ThrowAngle5 = new Vector2(82f, 82f);

	// Token: 0x040009B5 RID: 2485
	protected Vector2 m_spinningJump_Land_ThrowAngle6 = new Vector2(74f, 74f);

	// Token: 0x040009B6 RID: 2486
	protected Vector2 m_spinningJump_Land_ThrowPower = new Vector2(1f, 1f);

	// Token: 0x040009B7 RID: 2487
	protected float m_spinningJump_Land_ThrowPowerHighMod = 1.2f;

	// Token: 0x040009B8 RID: 2488
	protected float m_spinningJump_Land_ThrowPowerHighModAdvanced = 1.4f;

	// Token: 0x040009B9 RID: 2489
	protected float m_spinningJump_Land_RollingPowerMod = 0.3f;

	// Token: 0x040009BA RID: 2490
	protected float m_spinningJump_Land_RollingPowerModAdvanced = 0.15f;

	// Token: 0x040009BB RID: 2491
	protected bool m_spinningJump_Spawn_AxeSpin;

	// Token: 0x040009BC RID: 2492
	protected Projectile_RL m_spinningJumpProjectile;

	// Token: 0x040009BD RID: 2493
	protected Vector2 m_spinningJump_AttackVelocity = new Vector2(11f, 36f);

	// Token: 0x040009BE RID: 2494
	protected const string SWORD_PROJECTILE = "SpellSwordSlashDownProjectile";

	// Token: 0x040009BF RID: 2495
	protected const string SWORD_BONE_BIG_PROJECTILE = "FinalBossBoneBigProjectile";

	// Token: 0x040009C0 RID: 2496
	protected const string SWORD_BONE_BIG_HORIZONTAL_PROJECTILE = "FinalBossBoneBigHorizontalProjectile";

	// Token: 0x040009C1 RID: 2497
	protected const string SWORD_BONE_BIG_DIAGONAL_PROJECTILE = "FinalBossBoneBigDiagonalProjectile";

	// Token: 0x040009C2 RID: 2498
	protected const string SWORD_BONE_SMALL_PROJECTILE = "FinalBossBoneSmallProjectile";

	// Token: 0x040009C3 RID: 2499
	protected const string SWORD_WRAPPED_BOUNCE_SMALL_PROJECTILE = "FinalBossWrappedBounceSmallProjectile";

	// Token: 0x040009C4 RID: 2500
	protected const string SWORD_WRAPPED_VOID_SMALL_PROJECTILE = "FinalBossWrappedVoidSmallProjectile";

	// Token: 0x040009C5 RID: 2501
	protected const string SWORD_WRAPPED_BOUNCE_BIG_PROJECTILE = "FinalBossWrappedBounceBigProjectile";

	// Token: 0x040009C6 RID: 2502
	protected const string SWORD_WRAPPED_VOID_BIG_PROJECTILE = "FinalBossWrappedVoidBigProjectile";

	// Token: 0x040009C7 RID: 2503
	protected const string SWORD_POTION_PROJECTILE = "FinalBossPotionProjectile";

	// Token: 0x040009C8 RID: 2504
	protected const string SWORD_POTION_EXPLOSION_PROJECTILE = "FinalBossPotionExplosionProjectile";

	// Token: 0x040009C9 RID: 2505
	protected const int SWORD_WRAPPED_POS_INDEX = 15;

	// Token: 0x040009CA RID: 2506
	protected const int SWORD_WRAPPED_POS_INDEX2 = 16;

	// Token: 0x040009CB RID: 2507
	protected const int SWORD_BIG_ROTATING_POS_INDEX = 7;

	// Token: 0x040009CC RID: 2508
	protected const int SWORD_SMALL_ROTATING_POS_INDEX = 8;

	// Token: 0x040009CD RID: 2509
	protected const int SWORD_SWING_POS_INDEX = 17;

	// Token: 0x040009CE RID: 2510
	protected float m_sword_AttackSpeed = 16f;

	// Token: 0x040009CF RID: 2511
	protected float m_sword_AttackDuration = 0.185f;

	// Token: 0x040009D0 RID: 2512
	protected float m_sword_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x040009D1 RID: 2513
	protected float m_sword_TellHold_AnimSpeed = 1.2f;

	// Token: 0x040009D2 RID: 2514
	protected float m_sword_TellIntroAndHold_Delay = 0.95f;

	// Token: 0x040009D3 RID: 2515
	protected float m_sword_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x040009D4 RID: 2516
	protected float m_sword_AttackIntro_Delay;

	// Token: 0x040009D5 RID: 2517
	protected float m_sword_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x040009D6 RID: 2518
	protected float m_sword_AttackHold_Delay;

	// Token: 0x040009D7 RID: 2519
	protected float m_sword_Exit_AnimSpeed = 0.65f;

	// Token: 0x040009D8 RID: 2520
	protected float m_sword_Exit_Delay;

	// Token: 0x040009D9 RID: 2521
	protected float m_sword_Exit_IdleDuration = 0.1f;

	// Token: 0x040009DA RID: 2522
	protected float m_sword_AttackCD = 12f;

	// Token: 0x040009DB RID: 2523
	protected float m_sword_BlackProjectile_Delay = 0.65f;

	// Token: 0x040009DC RID: 2524
	private Coroutine m_swordAttackBlackSecondProjectileCoroutine;

	// Token: 0x040009DD RID: 2525
	protected const string PORTAL_PROJECTILE_LINE = "FinalBossPortalLineProjectile";

	// Token: 0x040009DE RID: 2526
	protected const string PORTAL_PROJECTILE_BOLT = "FinalBossPortalBoltProjectile";

	// Token: 0x040009DF RID: 2527
	protected const string PORTAL_PROJECTILE_EXPLOSION = "FinalBossPortalExplosionProjectile";

	// Token: 0x040009E0 RID: 2528
	protected const string PORTAL_WARNING_PROJECTILE = "FinalBossPortalWarningProjectile";

	// Token: 0x040009E1 RID: 2529
	protected const string PORTAL_WARNING_INVERSE_PROJECTILE = "FinalBossPortalInverseWarningProjectile";

	// Token: 0x040009E2 RID: 2530
	protected float m_portal_AttackSpeed = 16f;

	// Token: 0x040009E3 RID: 2531
	protected float m_portal_AttackDuration = 0.185f;

	// Token: 0x040009E4 RID: 2532
	protected float m_portal_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x040009E5 RID: 2533
	protected float m_portal_TellHold_AnimSpeed = 1.2f;

	// Token: 0x040009E6 RID: 2534
	protected float m_portal_TellIntroAndHold_Delay = 1.25f;

	// Token: 0x040009E7 RID: 2535
	protected float m_portal_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x040009E8 RID: 2536
	protected float m_portal_AttackIntro_Delay;

	// Token: 0x040009E9 RID: 2537
	protected float m_portal_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x040009EA RID: 2538
	protected float m_portal_AttackHold_Delay;

	// Token: 0x040009EB RID: 2539
	protected float m_portal_Exit_AnimSpeed = 0.65f;

	// Token: 0x040009EC RID: 2540
	protected float m_portal_Exit_Delay;

	// Token: 0x040009ED RID: 2541
	protected float m_portal_Exit_IdleDuration = 0.1f;

	// Token: 0x040009EE RID: 2542
	protected float m_portal_AttackCD = 12f;

	// Token: 0x040009EF RID: 2543
	protected Projectile_RL m_portalWarningProj;

	// Token: 0x040009F0 RID: 2544
	protected Projectile_RL m_portalProjectile;

	// Token: 0x040009F1 RID: 2545
	protected Coroutine m_portalWhiteAttackPersistentCoroutine;

	// Token: 0x040009F2 RID: 2546
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x040009F3 RID: 2547
	protected const string DEATH_HOLD = "Death";

	// Token: 0x040009F4 RID: 2548
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x040009F5 RID: 2549
	protected float m_death_Intro_Delay;

	// Token: 0x040009F6 RID: 2550
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x040009F7 RID: 2551
	protected float m_death_Hold_Delay = 5.5f;

	// Token: 0x040009F8 RID: 2552
	protected const string SPAWN_INTRO = "Transform";
}
