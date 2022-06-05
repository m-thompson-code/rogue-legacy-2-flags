using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class TraitorBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x06000A6B RID: 2667 RVA: 0x000208E4 File Offset: 0x0001EAE4
	protected float[] MODESHIFT_ARRAY
	{
		get
		{
			EnemyRank enemyRank = base.EnemyController.EnemyRank;
			if (enemyRank == EnemyRank.Expert)
			{
				return TraitorBoss_Basic_AIScript.m_expertModeshiftArray;
			}
			if (enemyRank != EnemyRank.Miniboss)
			{
				return TraitorBoss_Basic_AIScript.m_advancedModeshiftArray;
			}
			return TraitorBoss_Basic_AIScript.m_minibossModeshiftArray;
		}
	}

	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00020918 File Offset: 0x0001EB18
	public string GetAttackSFXName
	{
		get
		{
			if (base.EnemyController && base.EnemyController.EnemyRank == EnemyRank.Expert)
			{
				return "event:/SFX/Enemies/vo_jonah_female_attack";
			}
			return "event:/SFX/Enemies/Jonah/vo_jonah_attack";
		}
	}

	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00020940 File Offset: 0x0001EB40
	protected virtual bool is_Advanced
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x06000A6E RID: 2670 RVA: 0x00020943 File Offset: 0x0001EB43
	protected virtual bool is_Gregory
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x00020948 File Offset: 0x0001EB48
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"TraitorMagmaProjectile",
			"TraitorSpoonProjectile",
			"TraitorHomingBoltProjectile",
			"TraitorAxeSpinProjectile",
			"TraitorBoomerangProjectile",
			"TraitorLightningProjectile",
			"TraitorLightningWarningProjectile",
			"TraitorScytheProjectile",
			"TraitorScytheSecondProjectile",
			"TraitorArrowProjectile",
			"TraitorTelesliceProjectile",
			"TraitorRelicLandProjectile",
			"TraitorRelicDashVoidProjectile",
			"TraitorRelicDashVoidWarningProjectile",
			"TraitorRelicDashCurseProjectile",
			"TraitorRelicDamageAuraProjectile",
			"TraitorBossShoutExplosionProjectile",
			"TraitorBossShoutWarningProjectile",
			"TraitorForwardBeamProjectile",
			"TraitorWarningForwardBeamProjectile"
		};
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x00020A0D File Offset: 0x0001EC0D
	protected RelicType GetRelicType(TraitorBoss_Basic_AIScript.TraitorRelicType traitorRelicType)
	{
		switch (traitorRelicType)
		{
		case TraitorBoss_Basic_AIScript.TraitorRelicType.DamageZone:
			return RelicType.DamageAuraOnHit;
		case TraitorBoss_Basic_AIScript.TraitorRelicType.Jump:
			return RelicType.LandShockwave;
		case (TraitorBoss_Basic_AIScript.TraitorRelicType)3:
			break;
		case TraitorBoss_Basic_AIScript.TraitorRelicType.Dash:
			return RelicType.DashStrikeDamageUp;
		default:
			if (traitorRelicType == TraitorBoss_Basic_AIScript.TraitorRelicType.Block)
			{
				return RelicType.FreeHitRegenerate;
			}
			break;
		}
		return RelicType.None;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x00020A46 File Offset: 0x0001EC46
	protected string GetRelicLocID(TraitorBoss_Basic_AIScript.TraitorRelicType traitorRelicType)
	{
		switch (traitorRelicType)
		{
		case TraitorBoss_Basic_AIScript.TraitorRelicType.DamageZone:
			return "LOC_ID_TRAITOR_RELICS_DAMAGEZONE_1";
		case TraitorBoss_Basic_AIScript.TraitorRelicType.Jump:
			return "LOC_ID_TRAITOR_RELICS_JUMP_1";
		case (TraitorBoss_Basic_AIScript.TraitorRelicType)3:
			break;
		case TraitorBoss_Basic_AIScript.TraitorRelicType.Dash:
			return "LOC_ID_TRAITOR_RELICS_DASH_1";
		default:
			if (traitorRelicType == TraitorBoss_Basic_AIScript.TraitorRelicType.Block)
			{
				return "LOC_ID_TRAITOR_RELICS_BLOCK_1";
			}
			break;
		}
		return null;
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x00020A7F File Offset: 0x0001EC7F
	protected StatusBarEntryType GetStatusBarIcon(TraitorBoss_Basic_AIScript.TraitorRelicType traitorRelicType)
	{
		switch (traitorRelicType)
		{
		case TraitorBoss_Basic_AIScript.TraitorRelicType.DamageZone:
			return StatusBarEntryType.TraitorRelic_DamageZone;
		case TraitorBoss_Basic_AIScript.TraitorRelicType.Jump:
			return StatusBarEntryType.TraitorRelic_Jump;
		case (TraitorBoss_Basic_AIScript.TraitorRelicType)3:
			break;
		case TraitorBoss_Basic_AIScript.TraitorRelicType.Dash:
			return StatusBarEntryType.TraitorRelic_Dash;
		default:
			if (traitorRelicType == TraitorBoss_Basic_AIScript.TraitorRelicType.Block)
			{
				return StatusBarEntryType.TraitorRelic_Block;
			}
			break;
		}
		return StatusBarEntryType.None;
	}

	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x06000A73 RID: 2675 RVA: 0x00020AB8 File Offset: 0x0001ECB8
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06000A74 RID: 2676 RVA: 0x00020AC9 File Offset: 0x0001ECC9
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06000A75 RID: 2677 RVA: 0x00020ADA File Offset: 0x0001ECDA
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06000A76 RID: 2678 RVA: 0x00020AEB File Offset: 0x0001ECEB
	protected virtual int KNOCKBACK_DEFENSE_OVERRIDE
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06000A77 RID: 2679 RVA: 0x00020AEE File Offset: 0x0001ECEE
	protected virtual float DEFAULT_WEAPON_SWAP_DELAY
	{
		get
		{
			return 0.125f;
		}
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00020AF8 File Offset: 0x0001ECF8
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		if (TraitorBoss_Basic_AIScript.m_traitorRelicTypeArray == null)
		{
			TraitorBoss_Basic_AIScript.m_traitorRelicTypeArray = (Enum.GetValues(typeof(TraitorBoss_Basic_AIScript.TraitorRelicType)) as TraitorBoss_Basic_AIScript.TraitorRelicType[]);
		}
		base.EnemyController.HealthChangeRelay.AddListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit), false);
		this.m_weaponGeoController = enemyController.GetComponent<TraitorBossGeoController>();
		this.m_relicIndices = new int[4];
		this.DefaultAnimationLayer = 1;
		this.OnEnable();
		this.InitializeAimLine();
		if (!this.m_spoonThrowEventInstance.isValid())
		{
			this.m_spoonThrowEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/Jonah/sfx_jonah_spoon_throwing_loop", base.EnemyController.transform);
		}
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00020BA0 File Offset: 0x0001EDA0
	private void InitializeAimLine()
	{
		float num = this.m_teleslice_TeleportDistance / (float)this.m_aimLine.positionCount;
		for (int i = 0; i < this.m_aimLine.positionCount; i++)
		{
			this.m_aimLine.SetPosition(i, new Vector3(num * (float)i, 0f, 0f));
		}
		this.m_endAimIndicator.transform.localPosition = new Vector3(this.m_teleslice_TeleportDistance, 0f, 0f);
		this.m_aimLine.transform.localScale = new Vector3(1f / base.EnemyController.transform.lossyScale.x, 1f / base.EnemyController.transform.lossyScale.y, 1f);
		this.m_aimLine.transform.position = base.EnemyController.Midpoint;
		base.LogicController.DisableLogicActivationByDistance = true;
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x00020C94 File Offset: 0x0001EE94
	public override void ResetScript()
	{
		base.ResetScript();
		this.m_isWalking = false;
		this.m_isJumping = false;
		this.m_isAxeSpinning = false;
		this.m_isAimingSpread = false;
		this.m_isAimingTeleslice = false;
		this.m_relicMask = 0;
		base.Animator.SetBool("IsTraitorBoss", true);
		base.Animator.SetFloat("Walk_Anim_Speed", 0.8f);
		this.m_isModeShifting = false;
		this.m_modeShiftIndex = 0;
		base.EnemyController.StatusBarController.StopAllUIEffects();
		this.m_aimLine.gameObject.SetActive(false);
		this.m_prevIsGrounded = true;
		this.m_relicHitCount = 0;
		base.EnemyController.TakesNoDamage = false;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		this.HideAllWeaponGeos();
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x00020D58 File Offset: 0x0001EF58
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			this.m_isWalking = false;
			this.m_isJumping = false;
			this.m_isAxeSpinning = false;
			this.m_isAimingSpread = false;
			this.m_isAimingTeleslice = false;
			base.Animator.SetBool("IsTraitorBoss", true);
			base.Animator.SetFloat("Walk_Anim_Speed", 0.8f);
			this.m_aimLine.gameObject.SetActive(false);
			this.HideAllWeaponGeos();
			for (int i = 0; i < 4; i++)
			{
				this.m_relicIndices[i] = i;
			}
			CDGHelper.Shuffle<int>(this.m_relicIndices);
			this.m_relicMask = 0;
		}
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x00020DF8 File Offset: 0x0001EFF8
	private void OnDestroy()
	{
		if (base.EnemyController)
		{
			base.EnemyController.HealthChangeRelay.RemoveListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit));
		}
		if (this.m_spoonThrowEventInstance.isValid())
		{
			this.m_spoonThrowEventInstance.release();
		}
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x00020E48 File Offset: 0x0001F048
	private void OnBossHit(object sender, HealthChangeEventArgs args)
	{
		if (base.EnemyController.IsDead)
		{
			return;
		}
		if (this.HasRelic(TraitorBoss_Basic_AIScript.TraitorRelicType.Block))
		{
			this.m_relicHitCount++;
			if (this.m_relicHitCount > 3)
			{
				this.m_relicHitCount = 0;
				base.EnemyController.TakesNoDamage = false;
				AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_coesShell_damagePrevented", base.EnemyController.transform.position);
			}
			else if (this.m_relicHitCount == 3)
			{
				base.EnemyController.TakesNoDamage = true;
			}
			base.EnemyController.StatusBarController.ApplyUIEffect(StatusBarEntryType.TraitorRelic_Block, 3, this.m_relicHitCount);
		}
		if (args.PrevHealthValue <= args.NewHealthValue)
		{
			return;
		}
		if (this.m_isModeShifting)
		{
			return;
		}
		if (this.m_modeShiftIndex >= this.MODESHIFT_ARRAY.Length)
		{
			return;
		}
		if (base.EnemyController.EnemyRank >= EnemyRank.Advanced)
		{
			float num = this.MODESHIFT_ARRAY[this.m_modeShiftIndex] * (float)base.EnemyController.ActualMaxHealth;
			if (base.EnemyController.CurrentHealth <= num)
			{
				this.m_modeShiftIndex++;
				base.LogicController.StopAllLogic(true);
				base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "Relic_Mode_Shift";
			}
		}
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x00020F70 File Offset: 0x0001F170
	private void LateUpdate()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (base.EnemyController.CurrentHealth > 0f)
		{
			base.Animator.SetBool("Walking", this.m_isWalking && base.EnemyController.Velocity.x != 0f && base.EnemyController.IsGrounded);
			base.Animator.SetBool("Grounded", base.EnemyController.IsGrounded);
			base.Animator.SetFloat("ySpeed", base.EnemyController.Velocity.y);
			if (this.m_isJumping && base.EnemyController.IsGrounded)
			{
				this.m_isJumping = false;
				base.Animator.SetBool("Jumping", false);
			}
		}
		if (this.m_isAimingSpread)
		{
			Vector3 vector = new Vector3(base.EnemyController.Midpoint.x, base.EnemyController.Midpoint.y + this.ARROW_POS_OFFSET.y, base.EnemyController.Midpoint.z);
			Vector3 midpoint = base.EnemyController.TargetController.Midpoint;
			this.m_aimAngle = Mathf.Atan2(midpoint.y - vector.y, midpoint.x - vector.x);
			this.m_aimAngle = CDGHelper.ToDegrees(this.m_aimAngle);
			this.m_aimAngle = CDGHelper.WrapAngleDegrees(this.m_aimAngle, true);
			if (!base.EnemyController.IsFacingRight)
			{
				this.m_aimAngle = CDGHelper.WrapAngleDegrees(-180f - this.m_aimAngle, true);
			}
			float t = (this.m_aimAngle - -90f) / 180f;
			float value = Mathf.Lerp(4f, 0f, t);
			base.EnemyController.Animator.SetFloat("Attack_Direction", value);
		}
		if (this.m_isAimingTeleslice)
		{
			this.m_aimAngle = CDGHelper.AngleBetweenPts(base.EnemyController.Midpoint, base.EnemyController.TargetController.Midpoint);
			if (!this.m_aimLine.gameObject.activeSelf)
			{
				this.m_aimLine.gameObject.SetActive(true);
			}
			this.m_aimLine.transform.localEulerAngles = new Vector3(0f, 0f, this.m_aimAngle);
		}
		else if (this.m_aimLine.gameObject.activeSelf)
		{
			this.m_aimLine.gameObject.SetActive(false);
		}
		if (this.HasRelic(TraitorBoss_Basic_AIScript.TraitorRelicType.Jump) && !this.m_prevIsGrounded && base.EnemyController.IsGrounded && Time.time - this.m_prevIsGroundedStartTime > 0.65f)
		{
			this.FireProjectileAbsPos("TraitorRelicLandProjectile", base.EnemyController.transform.position, false, 0f, 1f, true, true, true);
			this.FireProjectile("TraitorMagmaProjectile", 0, true, 85f, 1f, true, true, true);
			this.FireProjectile("TraitorMagmaProjectile", 0, true, 95f, 1f, true, true, true);
		}
		this.m_prevIsGrounded = base.EnemyController.IsGrounded;
		if (this.m_prevIsGrounded)
		{
			this.m_prevIsGroundedStartTime = Time.time;
		}
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x000212AC File Offset: 0x0001F4AC
	public override void OnLBCompleteOrCancelled()
	{
		this.m_isWalking = false;
		this.m_isAimingSpread = false;
		this.m_isAimingTeleslice = false;
		base.EnemyController.DisableFriction = false;
		this.m_isAxeSpinning = false;
		base.StopProjectile(ref this.m_axeSpinProjectile);
		base.Animator.SetBool("Dashing", false);
		base.Animator.SetBool("Victory", false);
		this.Single_Enable_Gravity();
		this.Single_Disable_KnockbackDefense();
		base.Animator.Play("Empty", this.DefaultAnimationLayer);
		base.EnemyController.LockFlip = false;
		if (this.m_spoonThrowEventInstance.isValid())
		{
			AudioManager.Stop(this.m_spoonThrowEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0002135C File Offset: 0x0001F55C
	public override IEnumerator WalkTowards()
	{
		this.m_isWalking = true;
		yield return base.WalkTowards();
		yield break;
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0002136B File Offset: 0x0001F56B
	public override IEnumerator WalkAway()
	{
		this.m_isWalking = true;
		yield return base.WalkAway();
		yield break;
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0002137C File Offset: 0x0001F57C
	public override void Pause()
	{
		base.Pause();
		if (this.m_axeSpinProjectile && this.m_axeSpinProjectile.isActiveAndEnabled && this.m_axeSpinProjectile.OwnerController == base.EnemyController)
		{
			this.m_isAxeSpinning = true;
			base.StopProjectile(ref this.m_axeSpinProjectile);
		}
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x000213D4 File Offset: 0x0001F5D4
	public override void Unpause()
	{
		base.Unpause();
		if (this.m_isAxeSpinning)
		{
			this.m_isAxeSpinning = false;
			this.m_axeSpinProjectile = this.FireProjectile("TraitorAxeSpinProjectile", 0, false, 0f, 1f, true, true, true);
		}
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x00021416 File Offset: 0x0001F616
	protected IEnumerator SetWeaponGeo(GameObject geo, float holdDelay)
	{
		if (!geo.activeSelf)
		{
			this.HideAllWeaponGeos();
			geo.SetActive(true);
			base.Animator.SetBool("Victory", true);
			yield return null;
			yield return base.WaitUntilAnimComplete(0);
			yield return base.Wait(holdDelay, false);
			base.Animator.SetBool("Victory", false);
			yield return null;
			yield return base.WaitUntilAnimComplete(0);
		}
		yield break;
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x00021433 File Offset: 0x0001F633
	private IEnumerator SetWeaponGeos(GameObject geo1, GameObject geo2, float holdDelay)
	{
		if (!geo1.activeSelf)
		{
			this.HideAllWeaponGeos();
			geo1.SetActive(true);
			geo2.SetActive(true);
			base.Animator.SetBool("Victory", true);
			yield return null;
			yield return base.WaitUntilAnimComplete(0);
			yield return base.Wait(holdDelay, false);
			base.Animator.SetBool("Victory", false);
			yield return null;
			yield return base.WaitUntilAnimComplete(0);
		}
		yield break;
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x00021458 File Offset: 0x0001F658
	protected void HideAllWeaponGeos()
	{
		for (int i = 0; i < this.m_weaponGeoController.AllWeaponGeo.Length; i++)
		{
			if (this.m_weaponGeoController.AllWeaponGeo[i].activeSelf)
			{
				this.m_weaponGeoController.AllWeaponGeo[i].SetActive(false);
			}
		}
	}

	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x06000A87 RID: 2695 RVA: 0x000214A4 File Offset: 0x0001F6A4
	protected float m_magma_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x06000A88 RID: 2696 RVA: 0x000214AB File Offset: 0x0001F6AB
	protected float m_magma_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06000A89 RID: 2697 RVA: 0x000214B2 File Offset: 0x0001F6B2
	protected float m_magma_AttackHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06000A8A RID: 2698 RVA: 0x000214B9 File Offset: 0x0001F6B9
	protected float m_magma_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x06000A8B RID: 2699 RVA: 0x000214C0 File Offset: 0x0001F6C0
	protected float m_magma_Exit_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x06000A8C RID: 2700 RVA: 0x000214C7 File Offset: 0x0001F6C7
	protected float m_magma_Exit_Delay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x06000A8D RID: 2701 RVA: 0x000214CE File Offset: 0x0001F6CE
	protected virtual Vector2 m_magma_JumpDelay
	{
		get
		{
			return new Vector2(0.15f, 0.35f);
		}
	}

	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x06000A8E RID: 2702 RVA: 0x000214DF File Offset: 0x0001F6DF
	protected virtual int m_magma_ProjectilesFired
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x06000A8F RID: 2703 RVA: 0x000214E2 File Offset: 0x0001F6E2
	protected virtual float m_magma_ProjectileDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06000A90 RID: 2704 RVA: 0x000214E9 File Offset: 0x0001F6E9
	protected virtual float m_magma_Exit_ForceIdle
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x06000A91 RID: 2705 RVA: 0x000214F0 File Offset: 0x0001F6F0
	protected virtual float m_magma_Exit_AttackCD
	{
		get
		{
			return 8.5f;
		}
	}

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x06000A92 RID: 2706 RVA: 0x000214F7 File Offset: 0x0001F6F7
	protected virtual int m_magma_angleAdder
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x000214FB File Offset: 0x0001F6FB
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator MagmaCombo()
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_spoon_equip", base.EnemyController.transform.position);
		yield return this.SetWeaponGeo(this.m_weaponGeoController.Ladle, this.DEFAULT_WEAPON_SWAP_DELAY);
		CDGHelper.RandomPlusMinus();
		yield return this.Single_Jump(true, false, false);
		float seconds = UnityEngine.Random.Range(this.m_magma_JumpDelay.x, this.m_magma_JumpDelay.y);
		yield return base.Wait(seconds, false);
		if (this.m_spoonThrowEventInstance.isValid())
		{
			AudioManager.PlayAttached(null, this.m_spoonThrowEventInstance, base.EnemyController.gameObject);
		}
		yield return this.Default_Animation("SpellCast_Attack_Intro", this.m_magma_AttackIntro_AnimationSpeed, this.m_magma_AttackIntro_Delay, true);
		yield return this.Default_Animation("SpellCast_Attack_Hold", this.m_magma_AttackHold_AnimationSpeed, this.m_magma_AttackHold_Delay, false);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/vo_jonah_attack", base.EnemyController.transform.position);
		int num2;
		for (int i = 0; i < this.m_magma_ProjectilesFired; i = num2 + 1)
		{
			string projectileName = "TraitorSpoonProjectile";
			int spawnPosIndex = 0;
			bool matchFacing = true;
			float num = (float)90;
			int magma_angleAdder = this.m_magma_angleAdder;
			this.FireProjectile(projectileName, spawnPosIndex, matchFacing, num + (float)0, 1f, true, true, true);
			this.FireProjectile("TraitorSpoonProjectile", 0, true, (float)(90 + this.m_magma_angleAdder), 1f, true, true, true);
			this.FireProjectile("TraitorSpoonProjectile", 0, true, (float)(90 + this.m_magma_angleAdder * -1), 1f, true, true, true);
			this.FireProjectile("TraitorSpoonProjectile", 0, true, (float)(90 + this.m_magma_angleAdder * 2), 1f, true, true, true);
			this.FireProjectile("TraitorSpoonProjectile", 0, true, (float)(90 + this.m_magma_angleAdder * -2), 1f, true, true, true);
			if (this.is_Advanced)
			{
				this.FireProjectile("TraitorSpoonProjectile", 0, true, (float)(90 + this.m_magma_angleAdder * 3), 1f, true, true, true);
				this.FireProjectile("TraitorSpoonProjectile", 0, true, (float)(90 + this.m_magma_angleAdder * -3), 1f, true, true, true);
			}
			if (this.m_magma_ProjectileDelay != 0f)
			{
				yield return base.Wait(this.m_magma_ProjectileDelay, false);
			}
			num2 = i;
		}
		if (this.m_spoonThrowEventInstance.isValid())
		{
			AudioManager.Stop(this.m_spoonThrowEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		yield return this.Default_Animation("SpellCast_Exit", this.m_magma_Exit_AnimationSpeed, this.m_magma_Exit_Delay, true);
		base.Animator.SetTrigger("Change_Ability_Anim");
		base.EnemyController.LockFlip = false;
		yield return this.Single_WaitUntilGrounded();
		this.HideAllWeaponGeos();
		yield return this.Default_Attack_Cooldown(this.m_magma_Exit_ForceIdle, this.m_magma_Exit_AttackCD);
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0002150A File Offset: 0x0001F70A
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator AxeCombo()
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		this.Single_Enable_KnockbackDefense();
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_axe_equip", base.EnemyController.transform.position);
		yield return this.SetWeaponGeo(this.m_weaponGeoController.Axe, this.DEFAULT_WEAPON_SWAP_DELAY);
		yield return this.Default_TellIntroAndLoop("AxeGrounded_Tell_Intro", this.m_axe_TellIntro_AnimSpeed, "AxeGrounded_Tell_Hold", this.m_axe_TellHold_AnimSpeed, this.m_axe_TellIntroAndHold_Delay);
		yield return this.Default_Animation("AxeAirborne_Attack_Hold", this.m_axe_AttackHold_AnimSpeed, this.m_axe_AttackHold_Delay, false);
		this.m_axeSpinProjectile = this.FireProjectile("TraitorAxeSpinProjectile", 1, false, 0f, 1f, true, true, true);
		AudioManager.PlayOneShot(null, this.GetAttackSFXName, base.EnemyController.transform.position);
		yield return this.Single_Jump(true, false, false);
		yield return this.Single_WaitUntilGrounded();
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_axe_bounce", base.EnemyController.transform.position);
		base.EnemyController.LockFlip = false;
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		yield return this.Single_Jump(false, true, false);
		yield return this.Single_WaitUntilGrounded();
		if (this.is_Advanced)
		{
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_axe_bounce", base.EnemyController.transform.position);
			base.EnemyController.LockFlip = false;
			this.StopAndFaceTarget();
			base.EnemyController.LockFlip = true;
			yield return this.Single_Jump(false, true, false);
			yield return this.Single_WaitUntilGrounded();
		}
		base.StopProjectile(ref this.m_axeSpinProjectile);
		this.Single_Disable_KnockbackDefense();
		yield return this.Default_Animation("AxeAirborne_Exit", this.m_axe_Exit_AnimSpeed, this.m_axe_Exit_Delay, true);
		base.Animator.SetTrigger("Change_Ability_Anim");
		base.EnemyController.LockFlip = false;
		this.HideAllWeaponGeos();
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_axe_Exit_IdleDuration, this.m_axe_AttackCD);
		yield break;
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x06000A95 RID: 2709 RVA: 0x00021519 File Offset: 0x0001F719
	protected float m_teleslice_DelayBetweenSlices
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x06000A96 RID: 2710 RVA: 0x00021520 File Offset: 0x0001F720
	protected float m_teleslice_TeleportDistance
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x00021527 File Offset: 0x0001F727
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator TeleSliceCombo()
	{
		this.StopAndFaceTarget();
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_sword_equip", base.EnemyController.transform.position);
		yield return this.SetWeaponGeo(this.m_weaponGeoController.Katana, this.DEFAULT_WEAPON_SWAP_DELAY);
		this.m_isAimingTeleslice = true;
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_sword_prep", base.EnemyController.transform.position);
		yield return this.Default_TellIntroAndLoop("Teleport_Tell_Intro", this.m_teleslice_TellIntro_AnimSpeed, "Teleport_Tell_Hold", this.m_teleslice_TellHold_AnimSpeed, this.m_teleslice_TellIntroAndHold_Delay);
		this.m_isAimingTeleslice = false;
		base.EnemyController.LockFlip = true;
		this.Single_TeleSlice();
		yield return this.Default_Animation("Teleport_Attack_Hold", this.m_teleslice_AttackHold_AnimSpeed, this.m_teleslice_AttackHold_Delay, true);
		base.EnemyController.LockFlip = false;
		this.m_isAimingTeleslice = true;
		yield return this.Default_TellIntroAndLoop("Teleport_Tell_Intro", this.m_teleslice_TellIntro_AnimSpeed, "Teleport_Tell_Hold", this.m_teleslice_TellHold_AnimSpeed, this.m_teleslice_TellIntroAndHold_Delay);
		this.m_isAimingTeleslice = false;
		base.EnemyController.LockFlip = true;
		this.Single_TeleSlice();
		yield return this.Default_Animation("Teleport_Attack_Hold", this.m_teleslice_AttackHold_AnimSpeed, this.m_teleslice_AttackHold_Delay, true);
		if (this.is_Advanced)
		{
			this.m_isAimingTeleslice = true;
			yield return this.Default_TellIntroAndLoop("Teleport_Tell_Intro", this.m_teleslice_TellIntro_AnimSpeed, "Teleport_Tell_Hold", this.m_teleslice_TellHold_AnimSpeed, this.m_teleslice_TellIntroAndHold_Delay);
			this.m_isAimingTeleslice = false;
			base.EnemyController.LockFlip = true;
			this.Single_TeleSlice();
			yield return this.Default_Animation("Teleport_Attack_Hold", this.m_teleslice_AttackHold_AnimSpeed, this.m_teleslice_AttackHold_Delay, true);
		}
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_sword_flourish", base.EnemyController.transform.position);
		yield return this.Default_Animation("Teleport_Exit", this.m_teleslice_Exit_AnimSpeed, this.m_teleslice_Exit_Delay, true);
		base.Animator.SetTrigger("Change_Ability_Anim");
		this.HideAllWeaponGeos();
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_teleslice_Exit_IdleDuration, this.m_teleslice_AttackCD);
		yield break;
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x00021538 File Offset: 0x0001F738
	private void Single_TeleSlice()
	{
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_sword_dash", base.EnemyController.transform.position);
		AudioManager.PlayOneShot(null, this.GetAttackSFXName, base.EnemyController.transform.position);
		Vector2 vector = CDGHelper.AngleToVector(this.m_aimAngle);
		LayerMask layerMask = base.EnemyController.ControllerCorgi.SavedPlatformMask & ~base.EnemyController.ControllerCorgi.OneWayPlatformMask;
		ContactFilter2D contactFilter = default(ContactFilter2D);
		contactFilter.NoFilter();
		contactFilter.SetLayerMask(layerMask);
		this.m_teleslicePlatformHitList.Clear();
		Physics2D.Raycast(base.EnemyController.Midpoint, vector, contactFilter, this.m_teleslicePlatformHitList, this.m_teleslice_TeleportDistance);
		RaycastHit2D hit = default(RaycastHit2D);
		foreach (RaycastHit2D raycastHit2D in this.m_teleslicePlatformHitList)
		{
			if (raycastHit2D)
			{
				hit = raycastHit2D;
				break;
			}
		}
		Vector3 localPosition = base.EnemyController.transform.localPosition;
		if (hit)
		{
			Vector2 vector2 = -vector;
			Vector3 vector3 = base.EnemyController.Midpoint - base.EnemyController.transform.localPosition;
			if (vector.x >= 0f)
			{
				localPosition.x = Mathf.Max(hit.point.x - vector3.x + vector2.x, localPosition.x);
			}
			else
			{
				localPosition.x = Mathf.Min(hit.point.x - vector3.x + vector2.x, localPosition.x);
			}
			if (vector.y >= 0f)
			{
				localPosition.y = Mathf.Max(hit.point.y - vector3.y + vector2.y, localPosition.y);
			}
			else
			{
				localPosition.y = Mathf.Min(hit.point.y - vector3.y + vector2.y, localPosition.y);
			}
		}
		else
		{
			localPosition.x += vector.x * this.m_teleslice_TeleportDistance;
			localPosition.y += vector.y * this.m_teleslice_TeleportDistance;
		}
		Rect boundsRect = PlayerManager.GetCurrentPlayerRoom().BoundsRect;
		float num = 0f;
		float num2 = 0f;
		if (localPosition.x < boundsRect.xMin)
		{
			num = boundsRect.xMin - localPosition.x;
		}
		else if (localPosition.x > boundsRect.xMax)
		{
			num = boundsRect.xMax - localPosition.x;
		}
		if (localPosition.y < boundsRect.yMin)
		{
			num2 = boundsRect.yMin - localPosition.y;
		}
		else if (localPosition.y > boundsRect.yMax)
		{
			num2 = boundsRect.yMax - localPosition.y;
		}
		localPosition.x += num;
		localPosition.y += num2;
		if (num != 0f || num2 != 0f)
		{
			Vector2 vector4 = CDGHelper.AngleToVector(CDGHelper.AngleBetweenPts(base.EnemyController.Midpoint, localPosition));
			hit = Physics2D.Raycast(base.EnemyController.Midpoint, vector4, this.m_teleslice_TeleportDistance, layerMask);
			if (hit)
			{
				Vector2 vector5 = -vector4;
				Vector3 vector6 = base.EnemyController.Midpoint - base.EnemyController.transform.localPosition;
				localPosition.x = hit.point.x - vector6.x + vector5.x;
				localPosition.y = hit.point.y - vector6.y + vector5.y;
			}
		}
		base.EnemyController.transform.localPosition = localPosition;
		base.EnemyController.ControllerCorgi.SetRaysParameters();
		base.EnemyController.SetVelocity(0f, 0f, false);
		Vector2 offset = CDGHelper.RotatedPoint(new Vector2(this.TELESLICE_PROJECTILE_OFFSET.x, 0f), Vector2.zero, this.m_aimAngle);
		offset.y += this.TELESLICE_PROJECTILE_OFFSET.y;
		this.FireProjectile("TraitorTelesliceProjectile", offset, false, this.m_aimAngle, 1f, true, true, true);
	}

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x06000A99 RID: 2713 RVA: 0x000219D0 File Offset: 0x0001FBD0
	protected virtual Vector2 m_boomerang_JumpDelay
	{
		get
		{
			return new Vector2(0.3f, 0.65f);
		}
	}

	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x06000A9A RID: 2714 RVA: 0x000219E1 File Offset: 0x0001FBE1
	protected virtual int m_boomerang_ThrowLoopCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x000219E4 File Offset: 0x0001FBE4
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator BoomerangCombo()
	{
		base.SetVelocityX(0f, false);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_pizza_equip", base.EnemyController.transform.position);
		yield return this.SetWeaponGeo(this.m_weaponGeoController.Pizza, this.DEFAULT_WEAPON_SWAP_DELAY);
		float dieRollJump = (float)CDGHelper.RandomPlusMinus();
		if (dieRollJump > 0f)
		{
			yield return this.Single_Jump(true, false, false);
			float seconds = UnityEngine.Random.Range(this.m_boomerang_JumpDelay.x, this.m_boomerang_JumpDelay.y);
			yield return base.Wait(seconds, false);
		}
		this.Single_Disable_Gravity();
		this.Single_Enable_KnockbackDefense();
		base.EnemyController.LockFlip = false;
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_pizza_prep", base.EnemyController.transform.position);
		yield return this.Default_TellIntroAndLoop("Chakram_Tell_Intro", this.m_boomerang_TellIntro_AnimSpeed, "Chakram_Tell_Hold", this.m_boomerang_TellHold_AnimSpeed, this.m_boomerang_TellIntroAndHold_Delay);
		int num;
		for (int i = 0; i < this.m_boomerang_ThrowLoopCount; i = num + 1)
		{
			if (i > 0)
			{
				base.EnemyController.LockFlip = false;
				this.StopAndFaceTarget();
				base.EnemyController.LockFlip = true;
			}
			yield return this.Default_Animation("Chakram_Attack_Intro", this.m_boomerang_AttackIntro_AnimSpeed, this.m_boomerang_AttackIntro_Delay, true);
			yield return this.Default_Animation("Chakram_Attack_Hold", this.m_boomerang_AttackHold_AnimSpeed, this.m_boomerang_AttackHold_Delay, false);
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_pizza_throw", base.EnemyController.transform.position);
			AudioManager.PlayOneShot(null, this.GetAttackSFXName, base.EnemyController.transform.position);
			if (!base.EnemyController.IsGrounded)
			{
				if (this.is_Gregory)
				{
					this.FireProjectile("TraitorBoomerangProjectile", 0, false, 0f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
					this.FireProjectile("TraitorBoomerangProjectile", 0, false, 180f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
				}
				else if (this.is_Advanced)
				{
					this.FireProjectile("TraitorBoomerangProjectile", 0, false, 0f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
					this.FireProjectile("TraitorBoomerangProjectile", 0, false, 45f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
					this.FireProjectile("TraitorBoomerangProjectile", 0, false, 180f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
					this.FireProjectile("TraitorBoomerangProjectile", 0, false, 135f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
				}
				else
				{
					this.FireProjectile("TraitorBoomerangProjectile", 0, false, 0f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
					this.FireProjectile("TraitorBoomerangProjectile", 0, false, 90f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
					this.FireProjectile("TraitorBoomerangProjectile", 0, false, 180f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
				}
			}
			else if (this.is_Gregory)
			{
				this.FireProjectile("TraitorBoomerangProjectile", 0, false, 25f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
				this.FireProjectile("TraitorBoomerangProjectile", 0, false, 155f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
			}
			else if (this.is_Advanced)
			{
				this.FireProjectile("TraitorBoomerangProjectile", 0, false, 25f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
				this.FireProjectile("TraitorBoomerangProjectile", 0, false, 55f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
				this.FireProjectile("TraitorBoomerangProjectile", 0, false, 125f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
				this.FireProjectile("TraitorBoomerangProjectile", 0, false, 155f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
			}
			else
			{
				this.FireProjectile("TraitorBoomerangProjectile", 0, false, 25f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
				this.FireProjectile("TraitorBoomerangProjectile", 0, false, 90f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
				this.FireProjectile("TraitorBoomerangProjectile", 0, false, 155f, 1f + (float)i * this.m_boomerang_ThrowPowerScaling, true, true, true);
			}
			if (this.m_boomerang_LoopDelay > 0f)
			{
				yield return base.Wait(this.m_boomerang_LoopDelay, false);
			}
			num = i;
		}
		yield return this.Default_Animation("Chakram_Exit", this.m_boomerang_Exit_AnimSpeed, this.m_boomerang_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.Animator.SetTrigger("Change_Ability_Anim");
		base.EnemyController.Animator.ResetTrigger("Turn");
		this.Single_Enable_Gravity();
		this.Single_Disable_KnockbackDefense();
		if (dieRollJump > 0f)
		{
			yield return this.Single_WaitUntilGrounded();
		}
		this.HideAllWeaponGeos();
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_boomerang_Exit_IdleDuration, this.m_boomerang_AttackCD);
		yield break;
	}

	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x06000A9C RID: 2716 RVA: 0x000219F3 File Offset: 0x0001FBF3
	protected virtual Vector2 m_spread_JumpDelay
	{
		get
		{
			return new Vector2(0.3f, 0.45f);
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00021A04 File Offset: 0x0001FC04
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator SpreadCombo()
	{
		yield return this.Single_Dash(false, 0.25f);
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		yield return base.Wait(0.2f, false);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_longbow_equip", base.EnemyController.transform.position);
		yield return this.SetWeaponGeos(this.m_weaponGeoController.Bow, this.m_weaponGeoController.Arrow, this.DEFAULT_WEAPON_SWAP_DELAY);
		float dieRollJump = (float)CDGHelper.RandomPlusMinus();
		if (dieRollJump > 0f)
		{
			yield return this.Single_Jump(true, false, false);
			float seconds = UnityEngine.Random.Range(this.m_spread_JumpDelay.x, this.m_spread_JumpDelay.y);
			yield return base.Wait(seconds, false);
		}
		base.EnemyController.LockFlip = false;
		if (this.m_spread_AimAtPlayer)
		{
			this.m_isAimingSpread = true;
		}
		else
		{
			if (base.EnemyController.IsGrounded)
			{
				this.m_aimAngle = (float)UnityEngine.Random.Range(0, 90);
			}
			else
			{
				this.m_aimAngle = (float)UnityEngine.Random.Range(-90, 90);
			}
			float t = (this.m_aimAngle - -90f) / 180f;
			float value = Mathf.Lerp(4f, 0f, t);
			base.EnemyController.Animator.SetFloat("Attack_Direction", value);
		}
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_longbow_prep", base.EnemyController.transform.position);
		yield return this.Default_TellIntroAndLoop("Bow_Forward_Tell_Intro", this.m_spread_TellIntro_AnimSpeed, "Bow_Forward_Tell_Hold", this.m_spread_TellHold_AnimSpeed, this.m_spread_TellIntroAndHold_Delay);
		yield return this.Default_Animation("Bow_Forward_Attack_Intro", this.m_spread_AttackIntro_AnimSpeed, this.m_spread_AttackIntro_Delay, true);
		yield return this.Default_Animation("Bow_Forward_Attack_Hold", this.m_spread_AttackHold_AnimSpeed, 0f, false);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_longbow_shoot", base.EnemyController.transform.position);
		AudioManager.PlayOneShot(null, this.GetAttackSFXName, base.EnemyController.transform.position);
		Vector2 posOffset = this.ARROW_POS_OFFSET;
		posOffset = CDGHelper.RotatedPoint(posOffset, Vector2.zero, this.m_aimAngle);
		posOffset += base.EnemyController.Midpoint - base.EnemyController.transform.localPosition;
		if (!this.is_Gregory)
		{
			this.FireProjectile("TraitorArrowProjectile", posOffset, true, this.m_aimAngle, 1f, true, true, true);
			this.FireProjectile("TraitorArrowProjectile", posOffset, true, this.m_aimAngle + 25f, 1f, true, true, true);
			this.FireProjectile("TraitorArrowProjectile", posOffset, true, this.m_aimAngle - 25f, 1f, true, true, true);
			if (this.is_Advanced)
			{
				this.FireProjectile("TraitorArrowProjectile", posOffset, true, this.m_aimAngle + 50f, 1f, true, true, true);
				this.FireProjectile("TraitorArrowProjectile", posOffset, true, this.m_aimAngle - 50f, 1f, true, true, true);
			}
		}
		else
		{
			int num;
			for (int i = 0; i < 2; i = num + 1)
			{
				this.FireProjectile("TraitorArrowProjectile", posOffset, true, this.m_aimAngle, 1.2f, true, true, true);
				this.FireProjectile("TraitorArrowProjectile", posOffset, true, this.m_aimAngle + 25f, 1.2f, true, true, true);
				this.FireProjectile("TraitorArrowProjectile", posOffset, true, this.m_aimAngle - 25f, 1.2f, true, true, true);
				yield return base.Wait(0.15f, false);
				if (i != 0)
				{
					yield return this.Default_Animation("Bow_Forward_Attack_Intro", this.m_spread_AttackIntro_AnimSpeed, this.m_spread_AttackIntro_Delay, true);
					yield return this.Default_Animation("Bow_Forward_Attack_Hold", this.m_spread_AttackHold_AnimSpeed, 0f, false);
				}
				num = i;
			}
		}
		if (this.m_spread_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_spread_AttackHold_Delay, false);
		}
		yield return this.Default_Animation("Bow_Forward_Exit", this.m_spread_Exit_AnimSpeed, this.m_spread_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.Animator.SetTrigger("Change_Ability_Anim");
		this.m_isAimingSpread = false;
		if (dieRollJump > 0f)
		{
			yield return this.Single_WaitUntilGrounded();
		}
		this.HideAllWeaponGeos();
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_spread_Exit_IdleDuration, this.m_spread_AttackCD);
		yield break;
	}

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x06000A9E RID: 2718 RVA: 0x00021A13 File Offset: 0x0001FC13
	protected virtual float m_lightning_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x06000A9F RID: 2719 RVA: 0x00021A1A File Offset: 0x0001FC1A
	protected virtual float m_lightning_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x00021A21 File Offset: 0x0001FC21
	protected virtual float m_lightning_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005CA RID: 1482
	// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x00021A28 File Offset: 0x0001FC28
	protected virtual float m_lightning_TellHold_Delay
	{
		get
		{
			return 0.45f;
		}
	}

	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x00021A2F File Offset: 0x0001FC2F
	protected virtual float m_lightning_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x00021A36 File Offset: 0x0001FC36
	protected virtual float m_lightning_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x00021A3D File Offset: 0x0001FC3D
	protected virtual float m_lightning_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x00021A44 File Offset: 0x0001FC44
	protected virtual float m_lightning_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x00021A4B File Offset: 0x0001FC4B
	protected virtual float m_lightning_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x00021A52 File Offset: 0x0001FC52
	protected virtual float m_lightning_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00021A59 File Offset: 0x0001FC59
	protected virtual float m_lightning_Exit_ForceIdle
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x00021A60 File Offset: 0x0001FC60
	protected virtual float m_lightning_Exit_AttackCD
	{
		get
		{
			return 8.5f;
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x06000AAA RID: 2730 RVA: 0x00021A67 File Offset: 0x0001FC67
	protected virtual float m_lightning_Attack_DelayBetweenStrikesFirstPause
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x06000AAB RID: 2731 RVA: 0x00021A6E File Offset: 0x0001FC6E
	protected virtual float m_lightning_Attack_DelayBetweenStrikesSubsequent
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x06000AAC RID: 2732 RVA: 0x00021A75 File Offset: 0x0001FC75
	protected virtual float m_lightning_Attack_Duration
	{
		get
		{
			return 0.6f;
		}
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x06000AAD RID: 2733 RVA: 0x00021A7C File Offset: 0x0001FC7C
	protected virtual int m_lightning_NumStrikes
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00021A80 File Offset: 0x0001FC80
	protected virtual float m_lightning_StrikeOffsetX
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x00021A87 File Offset: 0x0001FC87
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator LightningAttack()
	{
		this.StopAndFaceTarget();
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_lightning_prepCharge_fullScreen", CameraController.GameCamera.transform.position);
		Vector2 lightningOriginPos = base.EnemyController.transform.position;
		if (!this.is_Advanced)
		{
			for (int j = 0; j < this.m_lightning_NumStrikes; j++)
			{
				if (j == 0)
				{
					this.FireProjectile("TraitorLightningWarningProjectile", Vector2.zero, false, 90f, 1f, true, true, true);
				}
				else
				{
					this.FireProjectile("TraitorLightningWarningProjectile", new Vector2(-this.m_lightning_StrikeOffsetX * (float)j, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectile("TraitorLightningWarningProjectile", new Vector2(this.m_lightning_StrikeOffsetX * (float)j, 0f), false, 90f, 1f, true, true, true);
				}
			}
		}
		else
		{
			for (int k = 0; k < this.m_lightning_NumStrikes; k++)
			{
				if (k == 0)
				{
					this.FireProjectile("TraitorLightningWarningProjectile", Vector2.zero, false, 90f, 1f, true, true, true);
					this.FireProjectile("TraitorLightningWarningProjectile", new Vector2(-this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectile("TraitorLightningWarningProjectile", new Vector2(this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
				}
				else
				{
					float num = (float)k * this.m_lightning_StrikeOffsetX * 2f;
					this.FireProjectile("TraitorLightningWarningProjectile", new Vector2(-num, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectile("TraitorLightningWarningProjectile", new Vector2(-num - this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectile("TraitorLightningWarningProjectile", new Vector2(-num + this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectile("TraitorLightningWarningProjectile", new Vector2(num, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectile("TraitorLightningWarningProjectile", new Vector2(num - this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectile("TraitorLightningWarningProjectile", new Vector2(num + this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
				}
			}
		}
		yield return this.Default_Animation("OmniSpellCast_Tell_Intro", this.m_lightning_TellIntro_AnimSpeed, this.m_lightning_TellIntro_Delay, true);
		yield return this.Default_Animation("OmniSpellCast_Tell_Hold", this.m_lightning_TellHold_AnimSpeed, this.m_lightning_TellHold_Delay, true);
		base.EnemyController.LockFlip = true;
		yield return this.Default_Animation("OmniSpellCast_Attack_Intro", this.m_lightning_AttackIntro_AnimSpeed, this.m_lightning_AttackIntro_Delay, true);
		yield return this.Default_Animation("OmniSpellCast_Attack_Hold", this.m_lightning_AttackHold_AnimSpeed, 0f, false);
		AudioManager.PlayOneShot(null, this.GetAttackSFXName, base.EnemyController.transform.position);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_lightning_attack_start_fullScreen", CameraController.GameCamera.transform.position);
		if (!this.is_Advanced)
		{
			for (int i = 0; i < this.m_lightning_NumStrikes; i++)
			{
				if (i == 0)
				{
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos, false, 90f, 1f, true, true, true);
					yield return base.Wait(this.m_lightning_Attack_DelayBetweenStrikesFirstPause, false);
				}
				else
				{
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos + new Vector2(-this.m_lightning_StrikeOffsetX * (float)i, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos + new Vector2(this.m_lightning_StrikeOffsetX * (float)i, 0f), false, 90f, 1f, true, true, true);
					yield return base.Wait(this.m_lightning_Attack_DelayBetweenStrikesSubsequent, false);
				}
			}
		}
		else
		{
			for (int i = 0; i < this.m_lightning_NumStrikes; i++)
			{
				if (i == 0)
				{
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos, false, 90f, 1f, true, true, true);
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos + new Vector2(-this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos + new Vector2(this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
					yield return base.Wait(this.m_lightning_Attack_DelayBetweenStrikesFirstPause, false);
				}
				else
				{
					float num2 = (float)i * this.m_lightning_StrikeOffsetX * 2f;
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos + new Vector2(-num2, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos + new Vector2(-num2 - this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos + new Vector2(-num2 + this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos + new Vector2(num2, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos + new Vector2(num2 - this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
					this.FireProjectileAbsPos("TraitorLightningProjectile", lightningOriginPos + new Vector2(num2 + this.m_lightning_StrikeOffsetX / 2f, 0f), false, 90f, 1f, true, true, true);
					yield return base.Wait(this.m_lightning_Attack_DelayBetweenStrikesSubsequent, false);
				}
			}
		}
		if (this.m_lightning_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_lightning_Attack_Duration, false);
		}
		yield return this.Default_Animation("OmniSpellCast_Exit", this.m_lightning_Exit_AnimSpeed, this.m_lightning_Exit_Delay, true);
		base.Animator.SetTrigger("Change_Ability_Anim");
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_lightning_Exit_ForceIdle, this.m_lightning_Exit_AttackCD);
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00021A96 File Offset: 0x0001FC96
	protected virtual float m_scythe_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x00021A9D File Offset: 0x0001FC9D
	protected virtual float m_scythe_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x00021AA4 File Offset: 0x0001FCA4
	protected virtual float m_scythe_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x00021AAB File Offset: 0x0001FCAB
	protected virtual float m_scythe_TellHold_Delay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x00021AB2 File Offset: 0x0001FCB2
	protected virtual float m_scythe_TellHold_Delay2
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x00021AB9 File Offset: 0x0001FCB9
	protected virtual float m_scythe_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00021AC0 File Offset: 0x0001FCC0
	protected virtual float m_scythe_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x00021AC7 File Offset: 0x0001FCC7
	protected virtual float m_scythe_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00021ACE File Offset: 0x0001FCCE
	protected virtual float m_scythe_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00021AD5 File Offset: 0x0001FCD5
	protected virtual float m_scythe_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00021ADC File Offset: 0x0001FCDC
	protected virtual float m_scythe_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00021AE3 File Offset: 0x0001FCE3
	protected virtual float m_scythe_Exit_ForceIdle
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x06000ABC RID: 2748 RVA: 0x00021AEA File Offset: 0x0001FCEA
	protected virtual float m_scythe_Exit_AttackCD
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x06000ABD RID: 2749 RVA: 0x00021AF1 File Offset: 0x0001FCF1
	protected virtual float m_scythe_Attack_ForwardSpeedOverride
	{
		get
		{
			return 35.5f;
		}
	}

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x06000ABE RID: 2750 RVA: 0x00021AF8 File Offset: 0x0001FCF8
	protected virtual float m_scythe_Attack_Duration
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00021AFF File Offset: 0x0001FCFF
	protected virtual bool m_scythe_RaiseKnockbackDefenseWhileAttacking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x00021B02 File Offset: 0x0001FD02
	protected virtual int m_scythe_KnockbackDefenseBoostOverride
	{
		get
		{
			return 99;
		}
	}

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x00021B06 File Offset: 0x0001FD06
	protected virtual float m_scythe_AttackDuration
	{
		get
		{
			return 0.375f;
		}
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x00021B0D File Offset: 0x0001FD0D
	protected virtual float m_scythe_TimeBetweenAttacks
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00021B14 File Offset: 0x0001FD14
	protected virtual Vector2 m_scythe_JumpDelay
	{
		get
		{
			return new Vector2(0.2f, 0.35f);
		}
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x00021B25 File Offset: 0x0001FD25
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ScytheAttack()
	{
		this.StopAndFaceTarget();
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_scythe_equip", base.EnemyController.transform.position);
		yield return this.SetWeaponGeo(this.m_weaponGeoController.Scythe, this.DEFAULT_WEAPON_SWAP_DELAY);
		float dieRollJump = (float)CDGHelper.RandomPlusMinus();
		if (dieRollJump > 0f)
		{
			yield return this.Single_Jump(true, false, true);
			float seconds = UnityEngine.Random.Range(this.m_scythe_JumpDelay.x, this.m_scythe_JumpDelay.y);
			yield return base.Wait(seconds, false);
		}
		float scytheSpeed = this.m_scythe_Attack_ForwardSpeedOverride;
		if (!base.EnemyController.IsTargetToMyRight)
		{
			scytheSpeed = -scytheSpeed;
		}
		this.FaceTarget();
		base.EnemyController.LockFlip = true;
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_scythe_prep", base.EnemyController.transform.position);
		yield return this.Default_Animation("Scythe_Tell_Intro", this.m_scythe_TellIntro_AnimSpeed, this.m_scythe_TellIntro_Delay, true);
		yield return this.Default_Animation("Scythe_Tell_Hold", this.m_scythe_TellHold_AnimSpeed, this.m_scythe_TellHold_Delay, true);
		this.Single_Disable_Gravity();
		yield return this.Default_Animation("Scythe_Attack_Intro", this.m_scythe_AttackIntro_AnimSpeed, this.m_scythe_AttackIntro_Delay, true);
		yield return this.Default_Animation("Scythe_Attack_Hold", this.m_scythe_AttackHold_AnimSpeed, this.m_scythe_AttackHold_Delay, false);
		AudioManager.PlayOneShot(null, this.GetAttackSFXName, base.EnemyController.transform.position);
		AudioManager.PlayOneShotAttached(null, "event:/SFX/Enemies/Jonah/sfx_jonah_scythe_dash", base.EnemyController.gameObject);
		if (this.m_scythe_RaiseKnockbackDefenseWhileAttacking && base.EnemyController.BaseKnockbackDefense < (float)this.m_scythe_KnockbackDefenseBoostOverride)
		{
			base.EnemyController.BaseKnockbackDefense = (float)this.m_scythe_KnockbackDefenseBoostOverride;
		}
		base.SetVelocityX(scytheSpeed, false);
		base.EnemyController.GroundHorizontalVelocity = scytheSpeed;
		base.EnemyController.DisableFriction = true;
		this.FireProjectile("TraitorScytheProjectile", 0, true, 0f, 1f, true, true, true);
		if (this.is_Advanced)
		{
			this.FireProjectile("TraitorRelicDashCurseProjectile", 0, true, 70f, 1f, true, true, true);
		}
		yield return base.Wait(this.m_scythe_TimeBetweenAttacks, false);
		yield return this.Default_Animation("Scythe_Tell_Intro_2", this.m_scythe_TellIntro_AnimSpeed, this.m_scythe_TellIntro_Delay, true);
		yield return this.Default_Animation("Scythe_Tell_Hold_2", this.m_scythe_TellHold_AnimSpeed, this.m_scythe_TellHold_Delay2, true);
		yield return this.Default_Animation("Scythe_Attack_Intro_2", this.m_scythe_AttackIntro_AnimSpeed, this.m_scythe_AttackIntro_Delay, true);
		yield return this.Default_Animation("Scythe_Attack_Hold_2", this.m_scythe_AttackHold_AnimSpeed, this.m_scythe_AttackHold_Delay, false);
		this.FireProjectile("TraitorScytheSecondProjectile", 0, true, 0f, 1f, true, true, true);
		if (this.is_Advanced)
		{
			this.FireProjectile("TraitorRelicDashCurseProjectile", 0, true, 110f, 1f, true, true, true);
		}
		if (this.m_scythe_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_scythe_AttackDuration, false);
		}
		base.SetVelocityX(0f, false);
		base.EnemyController.GroundHorizontalVelocity = 0f;
		base.EnemyController.DisableFriction = false;
		this.Single_Enable_Gravity();
		if (this.m_scythe_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_scythe_Attack_Duration, false);
		}
		yield return this.Default_Animation("Scythe_Exit", this.m_scythe_Exit_AnimSpeed, this.m_scythe_Exit_Delay, true);
		base.Animator.SetTrigger("Change_Ability_Anim");
		base.EnemyController.LockFlip = false;
		if (dieRollJump > 0f)
		{
			yield return this.Single_WaitUntilGrounded();
		}
		this.HideAllWeaponGeos();
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_scythe_Exit_ForceIdle, this.m_scythe_Exit_AttackCD);
		yield break;
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x00021B34 File Offset: 0x0001FD34
	protected virtual float m_dashMove_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x00021B3B File Offset: 0x0001FD3B
	protected virtual float m_dashMove_Exit_AttackCD
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x00021B42 File Offset: 0x0001FD42
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashMove()
	{
		if (CDGHelper.RandomPlusMinus() > 0)
		{
			yield return this.Single_Dash(false, 0.5f);
		}
		else
		{
			yield return this.Single_Dash(true, 0.5f);
		}
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_dashMove_Exit_ForceIdle, this.m_dashMove_Exit_AttackCD);
		yield break;
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x00021B51 File Offset: 0x0001FD51
	protected virtual float m_jump_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00021B58 File Offset: 0x0001FD58
	protected virtual float m_jump_PowerX
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x06000ACA RID: 2762 RVA: 0x00021B5F File Offset: 0x0001FD5F
	protected virtual float m_jump_PowerY
	{
		get
		{
			return 33f;
		}
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x06000ACB RID: 2763 RVA: 0x00021B66 File Offset: 0x0001FD66
	protected virtual float m_lowjump_PowerX
	{
		get
		{
			return 26f;
		}
	}

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x06000ACC RID: 2764 RVA: 0x00021B6D File Offset: 0x0001FD6D
	protected virtual float m_lowjump_PowerY
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x06000ACD RID: 2765 RVA: 0x00021B74 File Offset: 0x0001FD74
	protected virtual float m_straightJump_PowerX
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x06000ACE RID: 2766 RVA: 0x00021B7B File Offset: 0x0001FD7B
	protected virtual float m_straightJump_PowerY
	{
		get
		{
			return 28f;
		}
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x00021B82 File Offset: 0x0001FD82
	public IEnumerator Single_Jump(bool randomizeJump = true, bool lowJump = false, bool straightJump = false)
	{
		float num = this.m_jump_PowerX;
		float num2 = this.m_jump_PowerY;
		if (lowJump)
		{
			num = this.m_lowjump_PowerX;
			num2 = this.m_lowjump_PowerY;
		}
		if (straightJump)
		{
			num = this.m_lowjump_PowerX;
			num2 = this.m_lowjump_PowerY;
		}
		if (randomizeJump)
		{
			num += UnityEngine.Random.Range(this.m_jump_Random_AddX.x, this.m_jump_Random_AddX.y);
			num2 += UnityEngine.Random.Range(this.m_jump_Random_AddY.x, this.m_jump_Random_AddY.y);
		}
		if (base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(num, true);
		}
		else
		{
			base.SetVelocityX(-num, true);
		}
		base.SetVelocityY(num2, false);
		AudioManager.PlayOneShot(null, this.GetAttackSFXName, base.EnemyController.transform.position);
		base.Animator.SetBool("Jumping", true);
		yield return base.Wait(0.05f, false);
		yield break;
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x00021BA6 File Offset: 0x0001FDA6
	protected IEnumerator Single_WaitUntilGrounded()
	{
		yield return base.WaitUntilIsGrounded();
		base.SetVelocityX(0f, false);
		if (this.m_jump_Exit_Delay > 0f)
		{
			yield return base.Wait(this.m_jump_Exit_Delay, false);
		}
		yield break;
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x00021BB5 File Offset: 0x0001FDB5
	private void Single_Disable_Gravity()
	{
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.FallMultiplierOverride = 0f;
		base.EnemyController.ControllerCorgi.StickWhenWalkingDownSlopes = false;
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x00021BEE File Offset: 0x0001FDEE
	private void Single_Enable_Gravity()
	{
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.FallMultiplierOverride = 1f;
		base.EnemyController.ControllerCorgi.StickWhenWalkingDownSlopes = true;
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x00021C27 File Offset: 0x0001FE27
	public void Single_Enable_KnockbackDefense()
	{
		base.EnemyController.BaseKnockbackDefense = (float)this.KNOCKBACK_DEFENSE_OVERRIDE;
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x00021C3B File Offset: 0x0001FE3B
	protected void Single_Disable_KnockbackDefense()
	{
		base.EnemyController.BaseKnockbackDefense = (float)base.EnemyController.EnemyData.KnockbackDefence;
	}

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x00021C59 File Offset: 0x0001FE59
	protected virtual float m_dash_ForwardSpeedOverride
	{
		get
		{
			return 27.5f;
		}
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00021C60 File Offset: 0x0001FE60
	protected virtual float m_dash_BackwardSpeedOverride
	{
		get
		{
			return -27.5f;
		}
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x00021C67 File Offset: 0x0001FE67
	protected virtual float m_dashRelicWarningDuration
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x00021C6E File Offset: 0x0001FE6E
	private IEnumerator Single_Dash(bool dashForwards, float duration)
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		AudioManager.PlayOneShotAttached(null, "event:/SFX/Enemies/Jonah/sfx_jonah_dash", base.EnemyController.gameObject);
		if (this.HasRelic(TraitorBoss_Basic_AIScript.TraitorRelicType.Dash))
		{
			this.RunPersistentCoroutine(this.SpawnDashProjectileCoroutine(this.m_dashRelicWarningDuration));
		}
		float num = this.m_dash_ForwardSpeedOverride;
		if (!dashForwards)
		{
			num = this.m_dash_BackwardSpeedOverride;
		}
		if (!base.EnemyController.IsTargetToMyRight)
		{
			num = -num;
		}
		base.Animator.SetBool("Dashing", true);
		this.Single_Disable_Gravity();
		base.SetVelocity(new Vector2(num, 0f), false);
		base.EnemyController.GroundHorizontalVelocity = num;
		base.EnemyController.DisableFriction = true;
		yield return base.Wait(duration, false);
		base.Animator.SetBool("Dashing", false);
		this.Single_Enable_Gravity();
		base.EnemyController.GroundHorizontalVelocity = 0f;
		base.EnemyController.LockFlip = false;
		yield break;
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x00021C8B File Offset: 0x0001FE8B
	private IEnumerator SpawnDashProjectileCoroutine(float duration)
	{
		Vector3 warningSpawnPos = Vector3.zero;
		this.m_dashRelicWarningProjectile = this.FireProjectileAbsPos("TraitorRelicDashVoidWarningProjectile", base.EnemyController.transform.position, false, 0f, 1f, true, true, true);
		warningSpawnPos = this.m_dashRelicWarningProjectile.transform.position;
		float delay = duration + Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.FireProjectileAbsPos("TraitorRelicDashVoidProjectile", warningSpawnPos, false, 0f, 1f, true, true, true);
		base.StopProjectile(ref this.m_dashRelicWarningProjectile);
		yield break;
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x00021CA1 File Offset: 0x0001FEA1
	public IEnumerator Mode_Shift()
	{
		int num = 0;
		if (false)
		{
			this.m_relicMask |= 1 << 16 + this.m_relicIndices[num];
		}
		else
		{
			this.m_relicMask |= 1 << this.m_relicIndices[num];
		}
		yield break;
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x00021CB0 File Offset: 0x0001FEB0
	private bool HasRelic(TraitorBoss_Basic_AIScript.TraitorRelicType relicType)
	{
		return (relicType & (TraitorBoss_Basic_AIScript.TraitorRelicType)this.m_relicMask) > (TraitorBoss_Basic_AIScript.TraitorRelicType)0;
	}

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x06000ADC RID: 2780 RVA: 0x00021CBD File Offset: 0x0001FEBD
	protected virtual float m_relicModeShift_TellIntro_AnimSpeed
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00021CC4 File Offset: 0x0001FEC4
	protected virtual float m_relicModeShift_TellIntro_Delay
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x06000ADE RID: 2782 RVA: 0x00021CCB File Offset: 0x0001FECB
	protected virtual float m_relicModeShift_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00021CD2 File Offset: 0x0001FED2
	protected virtual float m_relicModeShift_TellHold_Delay
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x00021CD9 File Offset: 0x0001FED9
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Relic_Mode_Shift()
	{
		int num = UnityEngine.Random.Range(0, 4);
		TraitorBoss_Basic_AIScript.TraitorRelicType chosenRelic = TraitorBoss_Basic_AIScript.m_traitorRelicTypeArray[num];
		int num2 = 0;
		while (num2 < 4 && this.HasRelic(chosenRelic))
		{
			num++;
			if (num >= 4)
			{
				num = 0;
			}
			chosenRelic = TraitorBoss_Basic_AIScript.m_traitorRelicTypeArray[num];
			num2++;
		}
		if (num2 >= 4)
		{
			yield break;
		}
		this.ToDo("Relic ModeShift");
		RelicType actualRelic = this.GetRelicType(chosenRelic);
		this.m_isModeShifting = true;
		yield return base.DeathAnim();
		if (!base.EnemyController.IsGrounded)
		{
			yield return base.WaitUntilIsGrounded();
		}
		base.SetVelocityX(0f, false);
		base.RemoveStatusEffects(false);
		base.EnemyController.LockFlip = true;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = true;
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_Damage_Mod;
		base.EnemyController.BaseKnockbackDefense = 99999f;
		this.HideAllWeaponGeos();
		this.SetAnimationSpeedMultiplier(this.m_relicModeShift_TellIntro_AnimSpeed);
		base.EnemyController.Animator.SetBool("RetirePose", true);
		base.EnemyController.Animator.Play("RetirePose", 0);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_death_retire", base.EnemyController.transform.position);
		if (this.m_relicModeShift_TellIntro_Delay > 0f)
		{
			yield return base.Wait(this.m_relicModeShift_TellIntro_Delay, false);
		}
		base.EnemyController.Animator.SetBool("RetirePose", false);
		this.SetAnimationSpeedMultiplier(this.m_relicModeShift_TellHold_AnimSpeed);
		base.EnemyController.Animator.SetBool("Victory", true);
		base.EnemyController.Animator.Play("Victory", 0);
		AudioManager.PlayOneShot(null, this.GetAttackSFXName, base.EnemyController.transform.position);
		this.m_relicMask |= (int)chosenRelic;
		yield return base.Wait(0.25f, false);
		if (!RelicLibrary.GetRelicData(actualRelic))
		{
			Debug.Log("ActualRelic: " + actualRelic.ToString() + ", ChosenRelic: " + chosenRelic.ToString());
		}
		Vector2 position = new Vector2(base.EnemyController.transform.position.x, base.EnemyController.VisualBounds.max.y - 1f);
		(TextPopupManager.DisplayLocIDText(TextPopupType.TraitorRelic, this.GetRelicLocID(chosenRelic), StringGenderType.Male, position, 0f) as TraitorRelic_TextPopup).RelicType = actualRelic;
		TraitorBoss_Basic_AIScript.TraitorRelicType traitorRelicType = chosenRelic;
		switch (traitorRelicType)
		{
		case TraitorBoss_Basic_AIScript.TraitorRelicType.DamageZone:
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_voltaicCrown_equip", base.EnemyController.transform.position);
			break;
		case TraitorBoss_Basic_AIScript.TraitorRelicType.Jump:
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_marble_equip", base.EnemyController.transform.position);
			break;
		case (TraitorBoss_Basic_AIScript.TraitorRelicType)3:
			break;
		case TraitorBoss_Basic_AIScript.TraitorRelicType.Dash:
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_vanguard_equip", base.EnemyController.transform.position);
			break;
		default:
			if (traitorRelicType == TraitorBoss_Basic_AIScript.TraitorRelicType.Block)
			{
				AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_coesShell_equip", base.EnemyController.transform.position);
			}
			break;
		}
		if (this.m_relicModeShift_TellHold_Delay > 0f)
		{
			yield return base.Wait(this.m_relicModeShift_TellHold_Delay, false);
		}
		base.EnemyController.Animator.SetBool("Victory", false);
		this.m_isModeShifting = false;
		traitorRelicType = chosenRelic;
		if (traitorRelicType != TraitorBoss_Basic_AIScript.TraitorRelicType.DamageZone)
		{
			if (traitorRelicType == TraitorBoss_Basic_AIScript.TraitorRelicType.Block)
			{
				base.EnemyController.StatusBarController.ApplyUIEffect(StatusBarEntryType.TraitorRelic_Block, 3, 0);
			}
			else
			{
				base.EnemyController.StatusBarController.ApplyUIEffect(this.GetStatusBarIcon(chosenRelic));
			}
		}
		else
		{
			this.StopDamageZoneCoroutine();
			this.m_damageZoneCoroutine = this.RunPersistentCoroutine(this.DamageZoneRelicCoroutine());
		}
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_relicModeShift_Exit_IdleDuration, this.m_relicModeShift_AttackCD);
		yield break;
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x00021CE8 File Offset: 0x0001FEE8
	private IEnumerator DamageZoneRelicCoroutine()
	{
		for (;;)
		{
			base.EnemyController.StatusBarController.ApplyUIEffect(StatusBarEntryType.TraitorRelic_DamageZone, 10f);
			float timer = Time.time + 10f;
			while (Time.time < timer)
			{
				yield return null;
			}
			base.StopProjectile(ref this.m_damageZoneProjectile);
			this.m_damageZoneProjectile = this.FireProjectile("TraitorRelicDamageAuraProjectile", 1, false, 0f, 1f, true, true, true);
			timer = Time.time + 2f;
			while (Time.time < timer)
			{
				yield return null;
			}
			base.StopProjectile(ref this.m_damageZoneProjectile);
		}
		yield break;
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x00021CF7 File Offset: 0x0001FEF7
	private void StopDamageZoneCoroutine()
	{
		this.StopPersistentCoroutine(this.m_damageZoneCoroutine);
		this.m_damageZoneCoroutine = null;
		base.StopProjectile(ref this.m_damageZoneProjectile);
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x00021D18 File Offset: 0x0001FF18
	public override IEnumerator DeathAnim()
	{
		if (!ChallengeManager.IsInChallenge)
		{
			SaveManager.PlayerSaveData.TimesBeatenTraitor++;
		}
		base.EnemyController.StatusBarController.StopAllUIEffects();
		this.StopDamageZoneCoroutine();
		base.Animator.SetBool("Walking", false);
		base.Animator.SetFloat("ySpeed", 0f);
		base.Animator.SetBool("Stunned", true);
		base.Animator.Play("Empty", this.DefaultAnimationLayer);
		this.DefaultAnimationLayer = 0;
		this.ChangeAnimationState("Stunned");
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_death_hit", base.EnemyController.transform.position);
		yield return base.DeathAnim();
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 0.5f);
		if (this.m_death_Intro_Delay > 0f)
		{
			yield return base.Wait(this.m_death_Intro_Delay, true);
		}
		while (!base.EnemyController.ControllerCorgi.State.IsGrounded)
		{
			yield return null;
		}
		base.Animator.SetBool("Stunned", false);
		if (MusicManager.CurrentMusicInstance.isValid())
		{
			RuntimeManager.StudioSystem.setParameterByName("bossEncounterProgress_cain", 1f, false);
		}
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		int num = UnityEngine.Random.Range(0, 12);
		while (num == 9 || num == 8)
		{
			num = UnityEngine.Random.Range(0, 12);
		}
		yield return this.ChangeAnimationState("Death_" + num.ToString());
		yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		this.DefaultAnimationLayer = 1;
		if (this.m_death_Hold_Delay > 0f)
		{
			yield return base.Wait(this.m_death_Hold_Delay, true);
		}
		yield break;
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x00021D27 File Offset: 0x0001FF27
	public override IEnumerator SpawnAnim()
	{
		int storedAnimLayer = this.DefaultAnimationLayer;
		this.DefaultAnimationLayer = 0;
		yield return this.Default_Animation("Idle", this.m_spawn_Idle_AnimSpeed, this.m_spawn_Idle_Delay, true);
		base.EnemyController.Animator.SetTrigger("Bow");
		yield return base.Wait(3f, false);
		yield return this.Default_Animation("Idle", 1f, 0f, false);
		this.DefaultAnimationLayer = storedAnimLayer;
		yield break;
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x00021D36 File Offset: 0x0001FF36
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_spoonThrowEventInstance.isValid())
		{
			AudioManager.Stop(this.m_spoonThrowEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x04000ED4 RID: 3796
	[SerializeField]
	private LineRenderer m_aimLine;

	// Token: 0x04000ED5 RID: 3797
	[SerializeField]
	private GameObject m_endAimIndicator;

	// Token: 0x04000ED6 RID: 3798
	private static readonly float[] m_advancedModeshiftArray = new float[]
	{
		0.75f,
		0.5f,
		0.25f
	};

	// Token: 0x04000ED7 RID: 3799
	private static readonly float[] m_expertModeshiftArray = new float[]
	{
		0.66f,
		0.33f
	};

	// Token: 0x04000ED8 RID: 3800
	private static readonly float[] m_minibossModeshiftArray = new float[]
	{
		0.66f,
		0.33f
	};

	// Token: 0x04000ED9 RID: 3801
	private const int RELIC_BLOCK_HIT_COUNT = 3;

	// Token: 0x04000EDA RID: 3802
	private const float RELIC_DAMAGE_ZONE_TRIGGER_INTERVAL = 10f;

	// Token: 0x04000EDB RID: 3803
	private const float RELIC_DAMAGE_ZONE_DAMAGE_DURATION = 2f;

	// Token: 0x04000EDC RID: 3804
	private const float RELIC_LAND_SHOCKWAVE_FALL_DURATION = 0.65f;

	// Token: 0x04000EDD RID: 3805
	private const string ATTACK_SFX_NAME_MALE = "event:/SFX/Enemies/Jonah/vo_jonah_attack";

	// Token: 0x04000EDE RID: 3806
	private const string ATTACK_SFX_NAME_FEMALE = "event:/SFX/Enemies/vo_jonah_female_attack";

	// Token: 0x04000EDF RID: 3807
	protected const string MAGMA_PROJECTILE = "TraitorMagmaProjectile";

	// Token: 0x04000EE0 RID: 3808
	protected const string SPOON_PROJECTILE = "TraitorSpoonProjectile";

	// Token: 0x04000EE1 RID: 3809
	protected const string HOMING_PROJECTILE = "TraitorHomingBoltProjectile";

	// Token: 0x04000EE2 RID: 3810
	protected const string AXE_SPIN_PROJECTILE = "TraitorAxeSpinProjectile";

	// Token: 0x04000EE3 RID: 3811
	protected const string BOOMERANG_PROJECTILE = "TraitorBoomerangProjectile";

	// Token: 0x04000EE4 RID: 3812
	protected const string LIGHTNING_PROJECTILE = "TraitorLightningProjectile";

	// Token: 0x04000EE5 RID: 3813
	protected const string LIGHTNING_WARNING_PROJECTILE = "TraitorLightningWarningProjectile";

	// Token: 0x04000EE6 RID: 3814
	protected const string SCYTHE_PROJECTILE = "TraitorScytheProjectile";

	// Token: 0x04000EE7 RID: 3815
	protected const string SCYTHE_SECOND_PROJECTILE = "TraitorScytheSecondProjectile";

	// Token: 0x04000EE8 RID: 3816
	protected const string ARROW_PROJECTILE = "TraitorArrowProjectile";

	// Token: 0x04000EE9 RID: 3817
	protected const string TELESLICE_PROJECTILE = "TraitorTelesliceProjectile";

	// Token: 0x04000EEA RID: 3818
	protected const string RELIC_LAND_PROJECTILE = "TraitorRelicLandProjectile";

	// Token: 0x04000EEB RID: 3819
	protected const string RELIC_DASH_VOID_PROJECTILE = "TraitorRelicDashVoidProjectile";

	// Token: 0x04000EEC RID: 3820
	protected const string RELIC_DASH_WARNING_PROJECTILE = "TraitorRelicDashVoidWarningProjectile";

	// Token: 0x04000EED RID: 3821
	protected const string RELIC_DAMAGE_ZONE_PROJECTILE = "TraitorRelicDamageAuraProjectile";

	// Token: 0x04000EEE RID: 3822
	protected const string DASH_CURSE_PROJECTILE = "TraitorRelicDashCurseProjectile";

	// Token: 0x04000EEF RID: 3823
	protected const string SHOUT_ATTACK_EXPLOSION_PROJECTILE_NAME = "TraitorBossShoutExplosionProjectile";

	// Token: 0x04000EF0 RID: 3824
	protected const string SHOUT_ATTACK_WARNING_PROJECTILE_NAME = "TraitorBossShoutWarningProjectile";

	// Token: 0x04000EF1 RID: 3825
	protected const string STAFFTHROW_BEAM_PROJECTILE = "TraitorForwardBeamProjectile";

	// Token: 0x04000EF2 RID: 3826
	protected const string STAFFTHROW_BEAM_WARNING_PROJECTILE = "TraitorWarningForwardBeamProjectile";

	// Token: 0x04000EF3 RID: 3827
	protected const int MID_POS_INDEX = 1;

	// Token: 0x04000EF4 RID: 3828
	private static TraitorBoss_Basic_AIScript.TraitorRelicType[] m_traitorRelicTypeArray;

	// Token: 0x04000EF5 RID: 3829
	protected const int NUM_RELIC_TYPES = 4;

	// Token: 0x04000EF6 RID: 3830
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x04000EF7 RID: 3831
	private bool m_isWalking;

	// Token: 0x04000EF8 RID: 3832
	private bool m_isJumping;

	// Token: 0x04000EF9 RID: 3833
	private bool m_isAxeSpinning;

	// Token: 0x04000EFA RID: 3834
	private bool m_isAimingSpread;

	// Token: 0x04000EFB RID: 3835
	private bool m_isAimingTeleslice;

	// Token: 0x04000EFC RID: 3836
	private bool m_isModeShifting;

	// Token: 0x04000EFD RID: 3837
	private bool m_prevIsGrounded = true;

	// Token: 0x04000EFE RID: 3838
	private float m_prevIsGroundedStartTime;

	// Token: 0x04000EFF RID: 3839
	protected int m_modeShiftIndex;

	// Token: 0x04000F00 RID: 3840
	private int m_relicHitCount;

	// Token: 0x04000F01 RID: 3841
	protected TraitorBossGeoController m_weaponGeoController;

	// Token: 0x04000F02 RID: 3842
	private float m_aimAngle;

	// Token: 0x04000F03 RID: 3843
	private List<RaycastHit2D> m_teleslicePlatformHitList = new List<RaycastHit2D>(5);

	// Token: 0x04000F04 RID: 3844
	private int m_relicMask;

	// Token: 0x04000F05 RID: 3845
	private int[] m_relicIndices;

	// Token: 0x04000F06 RID: 3846
	protected const string MAGMA_ATTACK_INTRO = "SpellCast_Attack_Intro";

	// Token: 0x04000F07 RID: 3847
	protected const string MAGMA_ATTACK_HOLD = "SpellCast_Attack_Hold";

	// Token: 0x04000F08 RID: 3848
	protected const string MAGMA_EXIT = "SpellCast_Exit";

	// Token: 0x04000F09 RID: 3849
	protected Vector2 m_magma_AngleRandomizerAdd = new Vector2(-30f, 30f);

	// Token: 0x04000F0A RID: 3850
	protected Vector2 m_magma_PowerRandomizerMod = new Vector2(0.8f, 1.2f);

	// Token: 0x04000F0B RID: 3851
	protected int m_magma_InitialAngle = 90;

	// Token: 0x04000F0C RID: 3852
	private EventInstance m_spoonThrowEventInstance;

	// Token: 0x04000F0D RID: 3853
	protected const string AXE_TELL_INTRO = "AxeGrounded_Tell_Intro";

	// Token: 0x04000F0E RID: 3854
	protected const string AXE_TELL_HOLD = "AxeGrounded_Tell_Hold";

	// Token: 0x04000F0F RID: 3855
	protected const string AXE_ATTACK_INTRO = "AxeAirborne_Attack_Intro";

	// Token: 0x04000F10 RID: 3856
	protected const string AXE_ATTACK_HOLD = "AxeAirborne_Attack_Hold";

	// Token: 0x04000F11 RID: 3857
	protected const string AXE_EXIT = "AxeAirborne_Exit";

	// Token: 0x04000F12 RID: 3858
	protected float m_axe_TellIntro_AnimSpeed = 1.25f;

	// Token: 0x04000F13 RID: 3859
	protected float m_axe_TellHold_AnimSpeed = 1.25f;

	// Token: 0x04000F14 RID: 3860
	protected float m_axe_TellIntroAndHold_Delay = 0.35f;

	// Token: 0x04000F15 RID: 3861
	protected float m_axe_AttackHold_AnimSpeed = 2f;

	// Token: 0x04000F16 RID: 3862
	protected float m_axe_AttackHold_Delay;

	// Token: 0x04000F17 RID: 3863
	protected float m_axe_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000F18 RID: 3864
	protected float m_axe_Exit_Delay;

	// Token: 0x04000F19 RID: 3865
	protected float m_axe_Exit_IdleDuration = 0.1f;

	// Token: 0x04000F1A RID: 3866
	protected float m_axe_AttackCD = 1f;

	// Token: 0x04000F1B RID: 3867
	protected Projectile_RL m_axeSpinProjectile;

	// Token: 0x04000F1C RID: 3868
	protected const string TELESLICE_TELL_INTRO = "Teleport_Tell_Intro";

	// Token: 0x04000F1D RID: 3869
	protected const string TELESLICE_TELL_HOLD = "Teleport_Tell_Hold";

	// Token: 0x04000F1E RID: 3870
	protected const string TELESLICE_ATTACK_INTRO = "Teleport_Attack_Intro";

	// Token: 0x04000F1F RID: 3871
	protected const string TELESLICE_ATTACK_HOLD = "Teleport_Attack_Hold";

	// Token: 0x04000F20 RID: 3872
	protected const string TELESLICE_EXIT = "Teleport_Exit";

	// Token: 0x04000F21 RID: 3873
	protected float m_teleslice_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000F22 RID: 3874
	protected float m_teleslice_TellHold_AnimSpeed = 1f;

	// Token: 0x04000F23 RID: 3875
	protected float m_teleslice_TellIntroAndHold_Delay = 0.8f;

	// Token: 0x04000F24 RID: 3876
	protected float m_teleslice_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000F25 RID: 3877
	protected float m_teleslice_AttackIntro_Delay;

	// Token: 0x04000F26 RID: 3878
	protected float m_teleslice_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000F27 RID: 3879
	protected float m_teleslice_AttackHold_Delay = 0.1f;

	// Token: 0x04000F28 RID: 3880
	protected float m_teleslice_Exit_AnimSpeed = 1f;

	// Token: 0x04000F29 RID: 3881
	protected float m_teleslice_Exit_Delay;

	// Token: 0x04000F2A RID: 3882
	protected float m_teleslice_Exit_IdleDuration;

	// Token: 0x04000F2B RID: 3883
	protected float m_teleslice_AttackCD = 1f;

	// Token: 0x04000F2C RID: 3884
	protected Vector2 TELESLICE_PROJECTILE_OFFSET = new Vector2(0f, 1f);

	// Token: 0x04000F2D RID: 3885
	protected const string BOOMERANG_TELL_INTRO = "Chakram_Tell_Intro";

	// Token: 0x04000F2E RID: 3886
	protected const string BOOMERANG_TELL_HOLD = "Chakram_Tell_Hold";

	// Token: 0x04000F2F RID: 3887
	protected const string BOOMERANG_ATTACK_INTRO = "Chakram_Attack_Intro";

	// Token: 0x04000F30 RID: 3888
	protected const string BOOMERANG_ATTACK_HOLD = "Chakram_Attack_Hold";

	// Token: 0x04000F31 RID: 3889
	protected const string BOOMERANG_EXIT = "Chakram_Exit";

	// Token: 0x04000F32 RID: 3890
	protected float m_boomerang_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000F33 RID: 3891
	protected float m_boomerang_TellHold_AnimSpeed = 1f;

	// Token: 0x04000F34 RID: 3892
	protected float m_boomerang_TellIntroAndHold_Delay = 0.25f;

	// Token: 0x04000F35 RID: 3893
	protected float m_boomerang_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000F36 RID: 3894
	protected float m_boomerang_AttackIntro_Delay;

	// Token: 0x04000F37 RID: 3895
	protected float m_boomerang_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000F38 RID: 3896
	protected float m_boomerang_AttackHold_Delay;

	// Token: 0x04000F39 RID: 3897
	protected float m_boomerang_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000F3A RID: 3898
	protected float m_boomerang_Exit_Delay;

	// Token: 0x04000F3B RID: 3899
	protected float m_boomerang_Exit_IdleDuration = 0.1f;

	// Token: 0x04000F3C RID: 3900
	protected float m_boomerang_AttackCD = 10f;

	// Token: 0x04000F3D RID: 3901
	protected float m_boomerang_LoopDelay = 0.5f;

	// Token: 0x04000F3E RID: 3902
	protected float m_boomerang_ThrowPowerScaling = 0.5f;

	// Token: 0x04000F3F RID: 3903
	protected const string SPREAD_TELL_INTRO = "Bow_Forward_Tell_Intro";

	// Token: 0x04000F40 RID: 3904
	protected const string SPREAD_TELL_HOLD = "Bow_Forward_Tell_Hold";

	// Token: 0x04000F41 RID: 3905
	protected const string SPREAD_ATTACK_INTRO = "Bow_Forward_Attack_Intro";

	// Token: 0x04000F42 RID: 3906
	protected const string SPREAD_ATTACK_HOLD = "Bow_Forward_Attack_Hold";

	// Token: 0x04000F43 RID: 3907
	protected const string SPREAD_EXIT = "Bow_Forward_Exit";

	// Token: 0x04000F44 RID: 3908
	protected bool m_spread_AimAtPlayer = true;

	// Token: 0x04000F45 RID: 3909
	protected float m_spread_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000F46 RID: 3910
	protected float m_spread_TellHold_AnimSpeed = 1f;

	// Token: 0x04000F47 RID: 3911
	protected float m_spread_TellIntroAndHold_Delay;

	// Token: 0x04000F48 RID: 3912
	protected float m_spread_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000F49 RID: 3913
	protected float m_spread_AttackIntro_Delay;

	// Token: 0x04000F4A RID: 3914
	protected float m_spread_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000F4B RID: 3915
	protected float m_spread_AttackHold_Delay;

	// Token: 0x04000F4C RID: 3916
	protected float m_spread_Exit_AnimSpeed = 1f;

	// Token: 0x04000F4D RID: 3917
	protected float m_spread_Exit_Delay;

	// Token: 0x04000F4E RID: 3918
	protected float m_spread_Exit_IdleDuration;

	// Token: 0x04000F4F RID: 3919
	protected float m_spread_AttackCD = 1f;

	// Token: 0x04000F50 RID: 3920
	protected Vector2 ARROW_POS_OFFSET = new Vector2(1f, 0.25f);

	// Token: 0x04000F51 RID: 3921
	protected const string LIGHTNING_TELL_INTRO = "OmniSpellCast_Tell_Intro";

	// Token: 0x04000F52 RID: 3922
	protected const string LIGHTNING_TELL_HOLD = "OmniSpellCast_Tell_Hold";

	// Token: 0x04000F53 RID: 3923
	protected const string LIGHTNING_ATTACK_INTRO = "OmniSpellCast_Attack_Intro";

	// Token: 0x04000F54 RID: 3924
	protected const string LIGHTNING_ATTACK_HOLD = "OmniSpellCast_Attack_Hold";

	// Token: 0x04000F55 RID: 3925
	protected const string LIGHTNING_EXIT = "OmniSpellCast_Exit";

	// Token: 0x04000F56 RID: 3926
	protected const string SCYTHE_TELL_INTRO = "Scythe_Tell_Intro";

	// Token: 0x04000F57 RID: 3927
	protected const string SCYTHE_TELL_HOLD = "Scythe_Tell_Hold";

	// Token: 0x04000F58 RID: 3928
	protected const string SCYTHE_ATTACK_INTRO = "Scythe_Attack_Intro";

	// Token: 0x04000F59 RID: 3929
	protected const string SCYTHE_ATTACK_HOLD = "Scythe_Attack_Hold";

	// Token: 0x04000F5A RID: 3930
	protected const string SCYTHE_TELL_INTRO2 = "Scythe_Tell_Intro_2";

	// Token: 0x04000F5B RID: 3931
	protected const string SCYTHE_TELL_HOLD2 = "Scythe_Tell_Hold_2";

	// Token: 0x04000F5C RID: 3932
	protected const string SCYTHE_ATTACK_INTRO2 = "Scythe_Attack_Intro_2";

	// Token: 0x04000F5D RID: 3933
	protected const string SCYTHE_ATTACK_HOLD2 = "Scythe_Attack_Hold_2";

	// Token: 0x04000F5E RID: 3934
	protected const string SCYTHE_EXIT = "Scythe_Exit";

	// Token: 0x04000F5F RID: 3935
	protected Vector2 m_jump_Random_AddX = new Vector2(-1f, 10f);

	// Token: 0x04000F60 RID: 3936
	protected Vector2 m_jump_Random_AddY = new Vector2(-3f, 3f);

	// Token: 0x04000F61 RID: 3937
	private Projectile_RL m_dashRelicWarningProjectile;

	// Token: 0x04000F62 RID: 3938
	protected const string RELIC_MODESHIFT_TELL_INTRO = "RetirePose";

	// Token: 0x04000F63 RID: 3939
	protected const string RELIC_MODESHIFT_TELL_HOLD = "Victory";

	// Token: 0x04000F64 RID: 3940
	protected float m_relicModeShift_Exit_IdleDuration = 0.1f;

	// Token: 0x04000F65 RID: 3941
	protected float m_relicModeShift_AttackCD = 99f;

	// Token: 0x04000F66 RID: 3942
	protected Coroutine m_damageZoneCoroutine;

	// Token: 0x04000F67 RID: 3943
	protected Projectile_RL m_damageZoneProjectile;

	// Token: 0x04000F68 RID: 3944
	protected const string DEATH_INTRO = "Stunned";

	// Token: 0x04000F69 RID: 3945
	protected const string DEATH_HOLD = "Death_";

	// Token: 0x04000F6A RID: 3946
	protected float m_death_Intro_Delay = 1f;

	// Token: 0x04000F6B RID: 3947
	protected float m_death_Hold_Delay = 1f;

	// Token: 0x04000F6C RID: 3948
	protected const string SPAWN_IDLE = "Idle";

	// Token: 0x04000F6D RID: 3949
	protected const string SPAWN_INTRO = "Bow";

	// Token: 0x04000F6E RID: 3950
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04000F6F RID: 3951
	protected float m_spawn_Idle_Delay;

	// Token: 0x04000F70 RID: 3952
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04000F71 RID: 3953
	protected float m_spawn_Intro_Delay;

	// Token: 0x02000A6C RID: 2668
	protected enum TraitorRelicType
	{
		// Token: 0x040048AF RID: 18607
		DamageZone = 1,
		// Token: 0x040048B0 RID: 18608
		Jump,
		// Token: 0x040048B1 RID: 18609
		Dash = 4,
		// Token: 0x040048B2 RID: 18610
		Block = 8,
		// Token: 0x040048B3 RID: 18611
		TwinDamageZone = 65536,
		// Token: 0x040048B4 RID: 18612
		TwinJump = 131072,
		// Token: 0x040048B5 RID: 18613
		TwinDash = 262144,
		// Token: 0x040048B6 RID: 18614
		TwinBlock = 524288
	}
}
