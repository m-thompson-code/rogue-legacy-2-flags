using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using RL_Windows;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class FinalBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x1700033B RID: 827
	// (get) Token: 0x0600078C RID: 1932 RVA: 0x00005A03 File Offset: 0x00003C03
	public IRelayLink<bool, float> ColourShiftRelay
	{
		get
		{
			return this.m_colourShiftRelay.link;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x0600078D RID: 1933 RVA: 0x00005A10 File Offset: 0x00003C10
	public bool IsInWhiteMode
	{
		get
		{
			return this.m_isInWhiteMode;
		}
	}

	// Token: 0x1700033D RID: 829
	// (get) Token: 0x0600078E RID: 1934 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_isPrime_Version
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700033E RID: 830
	// (get) Token: 0x0600078F RID: 1935 RVA: 0x00005693 File Offset: 0x00003893
	protected virtual float m_Prime_Version_Final_Mode_Delay_Force_Idle_Add
	{
		get
		{
			return 1.35f;
		}
	}

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06000790 RID: 1936 RVA: 0x00004706 File Offset: 0x00002906
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.75f);
		}
	}

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06000791 RID: 1937 RVA: 0x00004717 File Offset: 0x00002917
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.75f, 1.15f);
		}
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06000792 RID: 1938 RVA: 0x00004717 File Offset: 0x00002917
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.75f, 1.15f);
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06000793 RID: 1939 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float WalkAnimSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x0005DACC File Offset: 0x0005BCCC
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

	// Token: 0x06000795 RID: 1941 RVA: 0x0005DC40 File Offset: 0x0005BE40
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

	// Token: 0x06000796 RID: 1942 RVA: 0x0005DE54 File Offset: 0x0005C054
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

	// Token: 0x06000797 RID: 1943 RVA: 0x0005DEEC File Offset: 0x0005C0EC
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

	// Token: 0x06000798 RID: 1944 RVA: 0x0005DF94 File Offset: 0x0005C194
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

	// Token: 0x06000799 RID: 1945 RVA: 0x0005E0A4 File Offset: 0x0005C2A4
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

	// Token: 0x0600079A RID: 1946 RVA: 0x00002FCA File Offset: 0x000011CA
	private void PerformForcedColourShift()
	{
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x0005E11C File Offset: 0x0005C31C
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

	// Token: 0x0600079C RID: 1948 RVA: 0x00005A18 File Offset: 0x00003C18
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

	// Token: 0x0600079D RID: 1949 RVA: 0x00005A27 File Offset: 0x00003C27
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

	// Token: 0x0600079E RID: 1950 RVA: 0x0005E19C File Offset: 0x0005C39C
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

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x0600079F RID: 1951 RVA: 0x00005A36 File Offset: 0x00003C36
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

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00005A4B File Offset: 0x00003C4B
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

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00005A60 File Offset: 0x00003C60
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

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00005A75 File Offset: 0x00003C75
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

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00005A8A File Offset: 0x00003C8A
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

	// Token: 0x060007A4 RID: 1956 RVA: 0x00005A9F File Offset: 0x00003C9F
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

	// Token: 0x060007A5 RID: 1957 RVA: 0x00005AAE File Offset: 0x00003CAE
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

	// Token: 0x060007A6 RID: 1958 RVA: 0x00005ACB File Offset: 0x00003CCB
	private void StopTreePersistentCoroutine()
	{
		this.StopPersistentCoroutine(this.m_treePersistentCoroutine);
		base.StopProjectile(ref this.m_summonTreeProjectile);
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00005A36 File Offset: 0x00003C36
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

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00005A4B File Offset: 0x00003C4B
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

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00005A60 File Offset: 0x00003C60
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

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x060007AA RID: 1962 RVA: 0x00005A75 File Offset: 0x00003C75
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

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x060007AB RID: 1963 RVA: 0x00005AE5 File Offset: 0x00003CE5
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

	// Token: 0x060007AC RID: 1964 RVA: 0x00005AFA File Offset: 0x00003CFA
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

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x060007AD RID: 1965 RVA: 0x00005B09 File Offset: 0x00003D09
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

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x060007AE RID: 1966 RVA: 0x00005B1E File Offset: 0x00003D1E
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

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x060007AF RID: 1967 RVA: 0x00005B33 File Offset: 0x00003D33
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

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00005B48 File Offset: 0x00003D48
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

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00005B5D File Offset: 0x00003D5D
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

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x060007B2 RID: 1970 RVA: 0x00005315 File Offset: 0x00003515
	protected virtual int m_numVerticalBeamProjectiles
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x17000353 RID: 851
	// (get) Token: 0x060007B3 RID: 1971 RVA: 0x000050CB File Offset: 0x000032CB
	protected virtual float m_numVerticalBeamAdvancedFollowUpDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x17000354 RID: 852
	// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00004762 File Offset: 0x00002962
	protected virtual int m_numVerticalBeamAdvancedFollowUpProjectiles
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x00005B72 File Offset: 0x00003D72
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

	// Token: 0x060007B6 RID: 1974 RVA: 0x00005B81 File Offset: 0x00003D81
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

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00005B97 File Offset: 0x00003D97
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

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00005BAC File Offset: 0x00003DAC
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

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00005BC1 File Offset: 0x00003DC1
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

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x060007BA RID: 1978 RVA: 0x00005BD6 File Offset: 0x00003DD6
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

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x060007BB RID: 1979 RVA: 0x00005BEB File Offset: 0x00003DEB
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

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x060007BC RID: 1980 RVA: 0x00005C00 File Offset: 0x00003E00
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

	// Token: 0x060007BD RID: 1981 RVA: 0x00005C15 File Offset: 0x00003E15
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

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x060007BE RID: 1982 RVA: 0x00005C24 File Offset: 0x00003E24
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

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x060007BF RID: 1983 RVA: 0x00005C39 File Offset: 0x00003E39
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

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00005C4E File Offset: 0x00003E4E
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

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00005C63 File Offset: 0x00003E63
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

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00005C78 File Offset: 0x00003E78
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

	// Token: 0x060007C3 RID: 1987 RVA: 0x00005C8D File Offset: 0x00003E8D
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

	// Token: 0x060007C4 RID: 1988 RVA: 0x00005C9C File Offset: 0x00003E9C
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

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x060007C5 RID: 1989 RVA: 0x00005CAB File Offset: 0x00003EAB
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

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00005CC0 File Offset: 0x00003EC0
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

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00005CD5 File Offset: 0x00003ED5
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

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00005CEA File Offset: 0x00003EEA
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

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x060007C9 RID: 1993 RVA: 0x00005CFF File Offset: 0x00003EFF
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

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x060007CA RID: 1994 RVA: 0x00005303 File Offset: 0x00003503
	protected virtual int Portal_Outward_Explosion_Amount
	{
		get
		{
			return 12;
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x060007CB RID: 1995 RVA: 0x000054AD File Offset: 0x000036AD
	protected virtual int Portal_Outward_Explosion_Angle
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x060007CC RID: 1996 RVA: 0x000054B1 File Offset: 0x000036B1
	protected virtual int Portal_Explosion_Bolt_Amount
	{
		get
		{
			return 15;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x060007CD RID: 1997 RVA: 0x00005D14 File Offset: 0x00003F14
	protected virtual int Portal_Explosion_Bolt_Angle
	{
		get
		{
			return 24;
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x060007CE RID: 1998 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float Portal_Explosion_Bolt_Delay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x060007CF RID: 1999 RVA: 0x00005D18 File Offset: 0x00003F18
	protected virtual float Portal_Advanced_Black_To_White_FollowUp_Delay
	{
		get
		{
			return 1.55f;
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x060007D0 RID: 2000 RVA: 0x00005D18 File Offset: 0x00003F18
	protected virtual float Portal_Advanced_White_To_Black_FollowUp_Delay
	{
		get
		{
			return 1.55f;
		}
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00005D1F File Offset: 0x00003F1F
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

	// Token: 0x060007D2 RID: 2002 RVA: 0x00005D2E File Offset: 0x00003F2E
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

	// Token: 0x060007D3 RID: 2003 RVA: 0x00005D44 File Offset: 0x00003F44
	private void StopPortalWhitePersistentCoroutine()
	{
		base.StopProjectile(ref this.m_portalProjectile);
		this.StopPersistentCoroutine(this.m_portalWhiteAttackPersistentCoroutine);
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00005D5E File Offset: 0x00003F5E
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

	// Token: 0x060007D5 RID: 2005 RVA: 0x00005D6D File Offset: 0x00003F6D
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

	// Token: 0x060007D6 RID: 2006 RVA: 0x00005D7C File Offset: 0x00003F7C
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

	// Token: 0x060007D7 RID: 2007 RVA: 0x0005E280 File Offset: 0x0005C480
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

	// Token: 0x060007D8 RID: 2008 RVA: 0x0005E2F8 File Offset: 0x0005C4F8
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

	// Token: 0x04000AE3 RID: 2787
	public const bool START_IN_WHITE_MODE = true;

	// Token: 0x04000AE4 RID: 2788
	protected const string VERTICAL_BEAM_BLACK_STOP_SFX_NAME = "event:/SFX/Enemies/Cain/sfx_cain_dark_staff_attack_lightning_end";

	// Token: 0x04000AE5 RID: 2789
	protected const string VERTICAL_BEAM_BLACK_LOOP_SFX_NAME = "event:/SFX/Enemies/Cain/sfx_cain_dark_staff_attack_lightning_loop";

	// Token: 0x04000AE6 RID: 2790
	protected const string VERTICAL_BEAM_BLACK_FLOOR_LOOP_SFX_NAME = "event:/SFX/Enemies/Cain/sfx_cain_dark_staff_attack_floor_loop";

	// Token: 0x04000AE7 RID: 2791
	private Relay<bool, float> m_colourShiftRelay = new Relay<bool, float>();

	// Token: 0x04000AE8 RID: 2792
	private static float[] MODESHIFT_ARRAY = new float[]
	{
		0.75f,
		0.5f
	};

	// Token: 0x04000AE9 RID: 2793
	private static Vector2Int COLOURSHIFT_MINMAX_COUNT = new Vector2Int(1, 2);

	// Token: 0x04000AEA RID: 2794
	private EventInstance m_rumbleEventInstance;

	// Token: 0x04000AEB RID: 2795
	private EventInstance m_collapseSpiritsEventInstance;

	// Token: 0x04000AEC RID: 2796
	private EventInstance m_verticalBeamBlackEventInstance;

	// Token: 0x04000AED RID: 2797
	private EventInstance m_verticalBeamBlackFloorLoopEventInstance;

	// Token: 0x04000AEE RID: 2798
	private bool m_isInWhiteMode;

	// Token: 0x04000AEF RID: 2799
	private bool m_isModeShifting;

	// Token: 0x04000AF0 RID: 2800
	protected int m_modeShiftIndex;

	// Token: 0x04000AF1 RID: 2801
	private int m_currentColourShiftCount;

	// Token: 0x04000AF2 RID: 2802
	private int m_requiredColourShiftCount;

	// Token: 0x04000AF3 RID: 2803
	private GameObject m_regularWeaponHitbox;

	// Token: 0x04000AF4 RID: 2804
	private GameObject m_jumpingWeaponHitbox;

	// Token: 0x04000AF5 RID: 2805
	private GameObject m_regularBodyHitbox;

	// Token: 0x04000AF6 RID: 2806
	private GameObject m_jumpingBodyHitbox;

	// Token: 0x04000AF7 RID: 2807
	private GameObject m_prayingBodyHitbox;

	// Token: 0x04000AF8 RID: 2808
	private FinalBossColourShiftController m_colourShiftController;

	// Token: 0x04000AF9 RID: 2809
	private GameObject m_statue;

	// Token: 0x04000AFA RID: 2810
	private GameObject m_model;

	// Token: 0x04000AFB RID: 2811
	protected const string MODESHIFT_DOWNED = "ModeShift_Intro";

	// Token: 0x04000AFC RID: 2812
	protected const string MODESHIFT_GETUP = "ModeShift_GetUp";

	// Token: 0x04000AFD RID: 2813
	protected const string MODESHIFT_TELL_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000AFE RID: 2814
	protected const string MODESHIFT_TELL_HOLD = "ModeShift_Tell_Loop";

	// Token: 0x04000AFF RID: 2815
	protected const string MODESHIFT_ATTACK_INTRO = "ModeShift_Scream_Hold";

	// Token: 0x04000B00 RID: 2816
	protected const string MODESHIFT_ATTACK_HOLD = "ModeShift_Action_Loop";

	// Token: 0x04000B01 RID: 2817
	protected const string MODESHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000B02 RID: 2818
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x04000B03 RID: 2819
	protected float m_modeShift_Downed_AnimSpeed = 1f;

	// Token: 0x04000B04 RID: 2820
	protected float m_modeShift_Downed_Delay;

	// Token: 0x04000B05 RID: 2821
	protected float m_modeShift_GetUp_AnimSpeed = 1f;

	// Token: 0x04000B06 RID: 2822
	protected float m_modeShift_TellIntro_AnimSpeed = 1.25f;

	// Token: 0x04000B07 RID: 2823
	protected float m_modeShift_TellHold_AnimSpeed = 1.25f;

	// Token: 0x04000B08 RID: 2824
	protected float m_modeShift_TellIntroAndHold_Delay = 2.5f;

	// Token: 0x04000B09 RID: 2825
	protected float m_modeShift_AttackIntro_AnimSpeed = 1.25f;

	// Token: 0x04000B0A RID: 2826
	protected float m_modeShift_AttackHold_AnimSpeed = 1.25f;

	// Token: 0x04000B0B RID: 2827
	protected float m_modeShift_AttackHold_Delay = 1.5f;

	// Token: 0x04000B0C RID: 2828
	protected float m_modeShift_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000B0D RID: 2829
	protected float m_modeShift_Exit_Delay;

	// Token: 0x04000B0E RID: 2830
	protected float m_modeShift_Exit_IdleDuration = 0.1f;

	// Token: 0x04000B0F RID: 2831
	protected float m_modeShift_AttackCD = 99f;

	// Token: 0x04000B10 RID: 2832
	protected string MODESHIFT_BOLT_PROJECTILE = "FinalBossModeShiftBoltProjectile";

	// Token: 0x04000B11 RID: 2833
	protected string MODESHIFT_SHOUT_PROJECTILE = "FinalBossModeShiftShoutProjectile";

	// Token: 0x04000B12 RID: 2834
	protected string MODESHIFT_SHOUT_WARNING_PROJECTILE = "FinalBossModeShiftShoutWarningProjectile";

	// Token: 0x04000B13 RID: 2835
	protected int MODESHIFT_SHOUT_POSITION_INDEX = 1;

	// Token: 0x04000B14 RID: 2836
	protected int MODESHIFT_POSITION_INDEX = 14;

	// Token: 0x04000B15 RID: 2837
	protected Vector2 m_modeShift_ThrowAngle = new Vector2(30f, 85f);

	// Token: 0x04000B16 RID: 2838
	protected Vector2 m_modeShift_ThrowPower = new Vector2(1f, 2.65f);

	// Token: 0x04000B17 RID: 2839
	protected float m_modeShift_LoopDelay = 0.225f;

	// Token: 0x04000B18 RID: 2840
	protected int m_modeShift_Loops = 15;

	// Token: 0x04000B19 RID: 2841
	private Projectile_RL m_modeShiftShoutWarningProj;

	// Token: 0x04000B1A RID: 2842
	private Projectile_RL m_modeShiftShoutProj;

	// Token: 0x04000B1B RID: 2843
	private Coroutine m_modeShiftProjectileCoroutine;

	// Token: 0x04000B1C RID: 2844
	protected const string SUMMON_TREE_EXPLOSION_PROJECTILE = "FinalBossTreeExplosionProjectile";

	// Token: 0x04000B1D RID: 2845
	protected const string SUMMON_TREE_WARNING_PROJECTILE = "FinalBossTreeWarningProjectile";

	// Token: 0x04000B1E RID: 2846
	protected const string SUMMON_TREE_PROJECTILE_WHITE = "FinalBossWhiteTreeProjectile";

	// Token: 0x04000B1F RID: 2847
	protected const string SUMMON_TREE_PROJECTILE_BLACK = "FinalBossBlackTreeProjectile";

	// Token: 0x04000B20 RID: 2848
	protected const string TREE_CHASE_PROJECTILE = "FinalBossWhiteTreeBoltProjectile";

	// Token: 0x04000B21 RID: 2849
	protected const string TREE_VOID_PROJECTILE = "FinalBossBlackTreeBoltProjectile";

	// Token: 0x04000B22 RID: 2850
	protected float m_summonTree_AttackDuration = 2f;

	// Token: 0x04000B23 RID: 2851
	protected float m_summonTree_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000B24 RID: 2852
	protected float m_summonTree_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000B25 RID: 2853
	protected float m_summonTree_TellIntroAndHold_Delay = 0.95f;

	// Token: 0x04000B26 RID: 2854
	protected float m_summonTree_Warning_Delay = 1f;

	// Token: 0x04000B27 RID: 2855
	protected float m_summonTree_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000B28 RID: 2856
	protected float m_summonTree_AttackIntro_Delay;

	// Token: 0x04000B29 RID: 2857
	protected float m_summonTree_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000B2A RID: 2858
	protected float m_summonTree_AttackHold_Delay;

	// Token: 0x04000B2B RID: 2859
	protected float m_summonTree_AttackHold_ExitDelay = 0.75f;

	// Token: 0x04000B2C RID: 2860
	protected float m_summonTree_Exit_AnimSpeed = 1.2f;

	// Token: 0x04000B2D RID: 2861
	protected float m_summonTree_Exit_Delay;

	// Token: 0x04000B2E RID: 2862
	protected float m_summonTree_Exit_IdleDuration = 0.1f;

	// Token: 0x04000B2F RID: 2863
	protected float m_summonTree_AttackCD = 12f;

	// Token: 0x04000B30 RID: 2864
	protected float m_treeProjectile_InitialDelay = 2.5f;

	// Token: 0x04000B31 RID: 2865
	protected float m_whiteTreeProjectile_Delay = 8f;

	// Token: 0x04000B32 RID: 2866
	protected float m_blackTreeProjectile_Delay = 8f;

	// Token: 0x04000B33 RID: 2867
	protected int m_numTreeVoidProjectiles = 8;

	// Token: 0x04000B34 RID: 2868
	protected float m_treeVoidSpeedMod = 0.65f;

	// Token: 0x04000B35 RID: 2869
	private Projectile_RL m_summonTreeProjectile;

	// Token: 0x04000B36 RID: 2870
	private Projectile_RL m_summonTreeWarningProjectile;

	// Token: 0x04000B37 RID: 2871
	private Coroutine m_treePersistentCoroutine;

	// Token: 0x04000B38 RID: 2872
	private bool m_summonedWhiteTree;

	// Token: 0x04000B39 RID: 2873
	protected const string PRAYER_EXPLOSION_PROJECTILE = "FinalBossTreeExplosionProjectile";

	// Token: 0x04000B3A RID: 2874
	protected const string PRAYER_WARNING_PROJECTILE = "FinalBossTreeWarningProjectile";

	// Token: 0x04000B3B RID: 2875
	protected const string PRAYER_VOID_PROJECTILE = "FinalBossPrayerVoidProjectile";

	// Token: 0x04000B3C RID: 2876
	protected const string PRAYER_BOUNCE_PROJECTILE = "FinalBossPrayerBounceProjectile";

	// Token: 0x04000B3D RID: 2877
	protected const int PRAYER_VOID_POS = 0;

	// Token: 0x04000B3E RID: 2878
	protected const int PRAYER_BOUNCE_POS_1 = 9;

	// Token: 0x04000B3F RID: 2879
	protected const int PRAYER_BOUNCE_POS_2 = 10;

	// Token: 0x04000B40 RID: 2880
	protected const int PRAYER_BOUNCE_POS_3 = 11;

	// Token: 0x04000B41 RID: 2881
	protected const int PRAYER_BOUNCE_POS_4 = 12;

	// Token: 0x04000B42 RID: 2882
	protected const int PRAYER_BOUNCE_POS_5 = 13;

	// Token: 0x04000B43 RID: 2883
	protected float m_prayer_AttackDuration = 2f;

	// Token: 0x04000B44 RID: 2884
	protected float m_prayer_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000B45 RID: 2885
	protected float m_prayer_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000B46 RID: 2886
	protected float m_prayer_TellIntroAndHold_Delay = 1.25f;

	// Token: 0x04000B47 RID: 2887
	protected float m_prayer_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000B48 RID: 2888
	protected float m_prayer_AttackIntro_Delay;

	// Token: 0x04000B49 RID: 2889
	protected float m_prayer_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000B4A RID: 2890
	protected float m_prayer_AttackHold_Delay;

	// Token: 0x04000B4B RID: 2891
	protected float m_prayer_AttackHold_ExitDelay;

	// Token: 0x04000B4C RID: 2892
	protected float m_prayer_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000B4D RID: 2893
	protected float m_prayer_Exit_Delay;

	// Token: 0x04000B4E RID: 2894
	protected float m_prayer_Exit_IdleDuration = 0.1f;

	// Token: 0x04000B4F RID: 2895
	protected float m_prayer_AttackCD = 12f;

	// Token: 0x04000B50 RID: 2896
	protected float m_prayer_VoidLoopDelay = 0.75f;

	// Token: 0x04000B51 RID: 2897
	protected float m_prayer_VoidLoopAdvancedFollowUpDelay = 0.25f;

	// Token: 0x04000B52 RID: 2898
	protected float m_prayer_BounceLoopDelay = 1f;

	// Token: 0x04000B53 RID: 2899
	protected float m_prayer_BounceLoopAdvancedDelay = 0.35f;

	// Token: 0x04000B54 RID: 2900
	protected float m_prayer_BounceAdvancedVoidInitialDelay = 0.85f;

	// Token: 0x04000B55 RID: 2901
	protected const string VERTICAL_BEAM_PROJECTILE_WHITE = "FinalBossWhiteVerticalProjectile";

	// Token: 0x04000B56 RID: 2902
	protected const string VERTICAL_BEAM_PROJECTILE_BLACK = "FinalBossBlackVerticalProjectile";

	// Token: 0x04000B57 RID: 2903
	protected const string VERTICAL_BEAM_VERTICAL_BOLT_PROJECTILE_BLACK = "FinalBossBlackVerticalBoltProjectile";

	// Token: 0x04000B58 RID: 2904
	protected const string VERTICAL_BEAM_BLACK_WARNING_PROJECTILE = "FinalBossBlackVerticalWarningProjectile";

	// Token: 0x04000B59 RID: 2905
	protected const string VERTICAL_BEAM_BLACK_BOUNCE = "FinalBossBounceOrbVerticalProjectile";

	// Token: 0x04000B5A RID: 2906
	protected const string VERTICAL_BEAM_BLACK_VOID = "FinalBossVerticalVoidSmallProjectile";

	// Token: 0x04000B5B RID: 2907
	protected const string VERTICAL_CURSE_PROJECTILE = "FinalBossVerticalCurseProjectile";

	// Token: 0x04000B5C RID: 2908
	protected const string VERTICAL_CURSE_BLUE_PROJECTILE = "FinalBossVerticalCurseBlueProjectile";

	// Token: 0x04000B5D RID: 2909
	protected const int VERTICAL_BEAM_WHITE_POS_INDEX = 2;

	// Token: 0x04000B5E RID: 2910
	protected float m_verticalBeam_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000B5F RID: 2911
	protected float m_verticalBeam_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000B60 RID: 2912
	protected float m_verticalBeam_TellIntroAndHold_Delay = 1.1f;

	// Token: 0x04000B61 RID: 2913
	protected float m_verticalBeam_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000B62 RID: 2914
	protected float m_verticalBeam_AttackIntro_Delay;

	// Token: 0x04000B63 RID: 2915
	protected float m_verticalBeam_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000B64 RID: 2916
	protected float m_verticalBeam_AttackHold_Delay;

	// Token: 0x04000B65 RID: 2917
	protected float m_verticalBeam_AttackHold_ExitDelay = 0.45f;

	// Token: 0x04000B66 RID: 2918
	protected float m_verticalBeam_Exit_AnimSpeed = 1.2f;

	// Token: 0x04000B67 RID: 2919
	protected float m_verticalBeam_Exit_Delay;

	// Token: 0x04000B68 RID: 2920
	protected float m_verticalBeam_Exit_IdleDuration = 0.1f;

	// Token: 0x04000B69 RID: 2921
	protected float m_verticalBeam_AttackCD = 12f;

	// Token: 0x04000B6A RID: 2922
	protected float m_verticalBeam_AttackDuration = 2f;

	// Token: 0x04000B6B RID: 2923
	protected int m_verticalBeamFireballSpread = 720;

	// Token: 0x04000B6C RID: 2924
	protected float m_verticalBeamBlackInitialDelay = 1.35f;

	// Token: 0x04000B6D RID: 2925
	protected float m_verticalBeamBlackLoopDelay = 0.6f;

	// Token: 0x04000B6E RID: 2926
	protected Projectile_RL m_verticalBeamBlackWarningProjectile;

	// Token: 0x04000B6F RID: 2927
	protected Projectile_RL m_verticalBeamBlackProjectile;

	// Token: 0x04000B70 RID: 2928
	protected Projectile_RL m_verticalBeamBoltBlackProjectile;

	// Token: 0x04000B71 RID: 2929
	private Coroutine m_delayVerticalBeamBlackCoroutine;

	// Token: 0x04000B72 RID: 2930
	protected const string SPINNING_JUMP_AXE_SPIN_PROJECTILE = "SpellSwordAxeSpinProjectile";

	// Token: 0x04000B73 RID: 2931
	protected const string SPINNING_JUMP_SWORD_SLASH_PROJECTILE = "FinalBossSwordSlashProjectile";

	// Token: 0x04000B74 RID: 2932
	protected const string SPINNING_JUMP_LAND_PROJECTILE_MAGMA = "FinalBossMagmaProjectile";

	// Token: 0x04000B75 RID: 2933
	protected const string SPINNING_JUMP_LAND_PROJECTILE_ROLLING = "FinalBossRollingProjectile";

	// Token: 0x04000B76 RID: 2934
	protected const int SPINNING_JUMP_SPIN_PROJ_INDEX = 3;

	// Token: 0x04000B77 RID: 2935
	protected const int SPINNING_JUMP_LAND_SWORD_PROJ_INDEX = 4;

	// Token: 0x04000B78 RID: 2936
	protected const int SPINNING_JUMP_LAND_CENTRE_PROJ_INDEX = 0;

	// Token: 0x04000B79 RID: 2937
	protected const int SPINNING_JUMP_LAND_BACK_PROJ_INDEX = 5;

	// Token: 0x04000B7A RID: 2938
	protected const int SPINNING_JUMP_LAND_FRONT_PROJ_INDEX = 6;

	// Token: 0x04000B7B RID: 2939
	protected float m_jump_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000B7C RID: 2940
	protected float m_jump_TellIntroAndHold_Delay = 0.65f;

	// Token: 0x04000B7D RID: 2941
	protected float m_spinningJump_TellIntro_AnimSpeed = 1.5f;

	// Token: 0x04000B7E RID: 2942
	protected float m_spinningJump_TellHold_AnimSpeed = 1.5f;

	// Token: 0x04000B7F RID: 2943
	protected float m_spinningJump_TellIntroAndHold_Delay = 0.85f;

	// Token: 0x04000B80 RID: 2944
	protected float m_spinningJump_AttackIntro_AnimSpeed = 1.5f;

	// Token: 0x04000B81 RID: 2945
	protected float m_spinningJump_AttackIntro_Delay;

	// Token: 0x04000B82 RID: 2946
	protected float m_spinningJump_AttackHold_AnimSpeed = 1.5f;

	// Token: 0x04000B83 RID: 2947
	protected float m_spinningJump_AttackHold_Delay;

	// Token: 0x04000B84 RID: 2948
	protected float m_spinningJump_AttackHold_ExitDelay;

	// Token: 0x04000B85 RID: 2949
	protected float m_spinningJump_ExitHold_Delay = 0.375f;

	// Token: 0x04000B86 RID: 2950
	protected float m_spinningJump_ExitHold_AnimSpeed = 1.2f;

	// Token: 0x04000B87 RID: 2951
	protected float m_spinningJump_Exit_AnimSpeed = 1.2f;

	// Token: 0x04000B88 RID: 2952
	protected float m_spinningJump_Exit_Delay;

	// Token: 0x04000B89 RID: 2953
	protected float m_spinningJump_Exit_IdleDuration = 0.1f;

	// Token: 0x04000B8A RID: 2954
	protected float m_spinningJump_AttackCD = 12f;

	// Token: 0x04000B8B RID: 2955
	protected Vector2 m_spinningJump_Land_ThrowAngle = new Vector2(85f, 85f);

	// Token: 0x04000B8C RID: 2956
	protected Vector2 m_spinningJump_Land_ThrowAngle2 = new Vector2(76f, 76f);

	// Token: 0x04000B8D RID: 2957
	protected Vector2 m_spinningJump_Land_ThrowAngle3 = new Vector2(71f, 71f);

	// Token: 0x04000B8E RID: 2958
	protected Vector2 m_spinningJump_Land_ThrowAngle4 = new Vector2(55f, 55f);

	// Token: 0x04000B8F RID: 2959
	protected Vector2 m_spinningJump_Land_ThrowAngle5 = new Vector2(82f, 82f);

	// Token: 0x04000B90 RID: 2960
	protected Vector2 m_spinningJump_Land_ThrowAngle6 = new Vector2(74f, 74f);

	// Token: 0x04000B91 RID: 2961
	protected Vector2 m_spinningJump_Land_ThrowPower = new Vector2(1f, 1f);

	// Token: 0x04000B92 RID: 2962
	protected float m_spinningJump_Land_ThrowPowerHighMod = 1.2f;

	// Token: 0x04000B93 RID: 2963
	protected float m_spinningJump_Land_ThrowPowerHighModAdvanced = 1.4f;

	// Token: 0x04000B94 RID: 2964
	protected float m_spinningJump_Land_RollingPowerMod = 0.3f;

	// Token: 0x04000B95 RID: 2965
	protected float m_spinningJump_Land_RollingPowerModAdvanced = 0.15f;

	// Token: 0x04000B96 RID: 2966
	protected bool m_spinningJump_Spawn_AxeSpin;

	// Token: 0x04000B97 RID: 2967
	protected Projectile_RL m_spinningJumpProjectile;

	// Token: 0x04000B98 RID: 2968
	protected Vector2 m_spinningJump_AttackVelocity = new Vector2(11f, 36f);

	// Token: 0x04000B99 RID: 2969
	protected const string SWORD_PROJECTILE = "SpellSwordSlashDownProjectile";

	// Token: 0x04000B9A RID: 2970
	protected const string SWORD_BONE_BIG_PROJECTILE = "FinalBossBoneBigProjectile";

	// Token: 0x04000B9B RID: 2971
	protected const string SWORD_BONE_BIG_HORIZONTAL_PROJECTILE = "FinalBossBoneBigHorizontalProjectile";

	// Token: 0x04000B9C RID: 2972
	protected const string SWORD_BONE_BIG_DIAGONAL_PROJECTILE = "FinalBossBoneBigDiagonalProjectile";

	// Token: 0x04000B9D RID: 2973
	protected const string SWORD_BONE_SMALL_PROJECTILE = "FinalBossBoneSmallProjectile";

	// Token: 0x04000B9E RID: 2974
	protected const string SWORD_WRAPPED_BOUNCE_SMALL_PROJECTILE = "FinalBossWrappedBounceSmallProjectile";

	// Token: 0x04000B9F RID: 2975
	protected const string SWORD_WRAPPED_VOID_SMALL_PROJECTILE = "FinalBossWrappedVoidSmallProjectile";

	// Token: 0x04000BA0 RID: 2976
	protected const string SWORD_WRAPPED_BOUNCE_BIG_PROJECTILE = "FinalBossWrappedBounceBigProjectile";

	// Token: 0x04000BA1 RID: 2977
	protected const string SWORD_WRAPPED_VOID_BIG_PROJECTILE = "FinalBossWrappedVoidBigProjectile";

	// Token: 0x04000BA2 RID: 2978
	protected const string SWORD_POTION_PROJECTILE = "FinalBossPotionProjectile";

	// Token: 0x04000BA3 RID: 2979
	protected const string SWORD_POTION_EXPLOSION_PROJECTILE = "FinalBossPotionExplosionProjectile";

	// Token: 0x04000BA4 RID: 2980
	protected const int SWORD_WRAPPED_POS_INDEX = 15;

	// Token: 0x04000BA5 RID: 2981
	protected const int SWORD_WRAPPED_POS_INDEX2 = 16;

	// Token: 0x04000BA6 RID: 2982
	protected const int SWORD_BIG_ROTATING_POS_INDEX = 7;

	// Token: 0x04000BA7 RID: 2983
	protected const int SWORD_SMALL_ROTATING_POS_INDEX = 8;

	// Token: 0x04000BA8 RID: 2984
	protected const int SWORD_SWING_POS_INDEX = 17;

	// Token: 0x04000BA9 RID: 2985
	protected float m_sword_AttackSpeed = 16f;

	// Token: 0x04000BAA RID: 2986
	protected float m_sword_AttackDuration = 0.185f;

	// Token: 0x04000BAB RID: 2987
	protected float m_sword_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000BAC RID: 2988
	protected float m_sword_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000BAD RID: 2989
	protected float m_sword_TellIntroAndHold_Delay = 0.95f;

	// Token: 0x04000BAE RID: 2990
	protected float m_sword_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000BAF RID: 2991
	protected float m_sword_AttackIntro_Delay;

	// Token: 0x04000BB0 RID: 2992
	protected float m_sword_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000BB1 RID: 2993
	protected float m_sword_AttackHold_Delay;

	// Token: 0x04000BB2 RID: 2994
	protected float m_sword_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000BB3 RID: 2995
	protected float m_sword_Exit_Delay;

	// Token: 0x04000BB4 RID: 2996
	protected float m_sword_Exit_IdleDuration = 0.1f;

	// Token: 0x04000BB5 RID: 2997
	protected float m_sword_AttackCD = 12f;

	// Token: 0x04000BB6 RID: 2998
	protected float m_sword_BlackProjectile_Delay = 0.65f;

	// Token: 0x04000BB7 RID: 2999
	private Coroutine m_swordAttackBlackSecondProjectileCoroutine;

	// Token: 0x04000BB8 RID: 3000
	protected const string PORTAL_PROJECTILE_LINE = "FinalBossPortalLineProjectile";

	// Token: 0x04000BB9 RID: 3001
	protected const string PORTAL_PROJECTILE_BOLT = "FinalBossPortalBoltProjectile";

	// Token: 0x04000BBA RID: 3002
	protected const string PORTAL_PROJECTILE_EXPLOSION = "FinalBossPortalExplosionProjectile";

	// Token: 0x04000BBB RID: 3003
	protected const string PORTAL_WARNING_PROJECTILE = "FinalBossPortalWarningProjectile";

	// Token: 0x04000BBC RID: 3004
	protected const string PORTAL_WARNING_INVERSE_PROJECTILE = "FinalBossPortalInverseWarningProjectile";

	// Token: 0x04000BBD RID: 3005
	protected float m_portal_AttackSpeed = 16f;

	// Token: 0x04000BBE RID: 3006
	protected float m_portal_AttackDuration = 0.185f;

	// Token: 0x04000BBF RID: 3007
	protected float m_portal_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000BC0 RID: 3008
	protected float m_portal_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000BC1 RID: 3009
	protected float m_portal_TellIntroAndHold_Delay = 1.25f;

	// Token: 0x04000BC2 RID: 3010
	protected float m_portal_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000BC3 RID: 3011
	protected float m_portal_AttackIntro_Delay;

	// Token: 0x04000BC4 RID: 3012
	protected float m_portal_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000BC5 RID: 3013
	protected float m_portal_AttackHold_Delay;

	// Token: 0x04000BC6 RID: 3014
	protected float m_portal_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000BC7 RID: 3015
	protected float m_portal_Exit_Delay;

	// Token: 0x04000BC8 RID: 3016
	protected float m_portal_Exit_IdleDuration = 0.1f;

	// Token: 0x04000BC9 RID: 3017
	protected float m_portal_AttackCD = 12f;

	// Token: 0x04000BCA RID: 3018
	protected Projectile_RL m_portalWarningProj;

	// Token: 0x04000BCB RID: 3019
	protected Projectile_RL m_portalProjectile;

	// Token: 0x04000BCC RID: 3020
	protected Coroutine m_portalWhiteAttackPersistentCoroutine;

	// Token: 0x04000BCD RID: 3021
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x04000BCE RID: 3022
	protected const string DEATH_HOLD = "Death";

	// Token: 0x04000BCF RID: 3023
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04000BD0 RID: 3024
	protected float m_death_Intro_Delay;

	// Token: 0x04000BD1 RID: 3025
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04000BD2 RID: 3026
	protected float m_death_Hold_Delay = 5.5f;

	// Token: 0x04000BD3 RID: 3027
	protected const string SPAWN_INTRO = "Transform";
}
