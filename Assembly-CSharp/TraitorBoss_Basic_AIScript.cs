using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x02000237 RID: 567
public class TraitorBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x17000783 RID: 1923
	// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00079880 File Offset: 0x00077A80
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

	// Token: 0x17000784 RID: 1924
	// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x000088C5 File Offset: 0x00006AC5
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

	// Token: 0x17000785 RID: 1925
	// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool is_Advanced
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000786 RID: 1926
	// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool is_Gregory
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000FF6 RID: 4086 RVA: 0x000798B4 File Offset: 0x00077AB4
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

	// Token: 0x06000FF7 RID: 4087 RVA: 0x000088ED File Offset: 0x00006AED
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

	// Token: 0x06000FF8 RID: 4088 RVA: 0x00008926 File Offset: 0x00006B26
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

	// Token: 0x06000FF9 RID: 4089 RVA: 0x0000895F File Offset: 0x00006B5F
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

	// Token: 0x17000787 RID: 1927
	// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000788 RID: 1928
	// (get) Token: 0x06000FFB RID: 4091 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000789 RID: 1929
	// (get) Token: 0x06000FFC RID: 4092 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700078A RID: 1930
	// (get) Token: 0x06000FFD RID: 4093 RVA: 0x00003E42 File Offset: 0x00002042
	protected virtual int KNOCKBACK_DEFENSE_OVERRIDE
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x06000FFE RID: 4094 RVA: 0x00003C5B File Offset: 0x00001E5B
	protected virtual float DEFAULT_WEAPON_SWAP_DELAY
	{
		get
		{
			return 0.125f;
		}
	}

	// Token: 0x06000FFF RID: 4095 RVA: 0x0007997C File Offset: 0x00077B7C
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

	// Token: 0x06001000 RID: 4096 RVA: 0x00079A24 File Offset: 0x00077C24
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

	// Token: 0x06001001 RID: 4097 RVA: 0x00079B18 File Offset: 0x00077D18
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

	// Token: 0x06001002 RID: 4098 RVA: 0x00079BDC File Offset: 0x00077DDC
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

	// Token: 0x06001003 RID: 4099 RVA: 0x00079C7C File Offset: 0x00077E7C
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

	// Token: 0x06001004 RID: 4100 RVA: 0x00079CCC File Offset: 0x00077ECC
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

	// Token: 0x06001005 RID: 4101 RVA: 0x00079DF4 File Offset: 0x00077FF4
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

	// Token: 0x06001006 RID: 4102 RVA: 0x0007A130 File Offset: 0x00078330
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

	// Token: 0x06001007 RID: 4103 RVA: 0x00008998 File Offset: 0x00006B98
	public override IEnumerator WalkTowards()
	{
		this.m_isWalking = true;
		yield return base.WalkTowards();
		yield break;
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x000089A7 File Offset: 0x00006BA7
	public override IEnumerator WalkAway()
	{
		this.m_isWalking = true;
		yield return base.WalkAway();
		yield break;
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x0007A1E0 File Offset: 0x000783E0
	public override void Pause()
	{
		base.Pause();
		if (this.m_axeSpinProjectile && this.m_axeSpinProjectile.isActiveAndEnabled && this.m_axeSpinProjectile.OwnerController == base.EnemyController)
		{
			this.m_isAxeSpinning = true;
			base.StopProjectile(ref this.m_axeSpinProjectile);
		}
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x0007A238 File Offset: 0x00078438
	public override void Unpause()
	{
		base.Unpause();
		if (this.m_isAxeSpinning)
		{
			this.m_isAxeSpinning = false;
			this.m_axeSpinProjectile = this.FireProjectile("TraitorAxeSpinProjectile", 0, false, 0f, 1f, true, true, true);
		}
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x000089B6 File Offset: 0x00006BB6
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

	// Token: 0x0600100C RID: 4108 RVA: 0x000089D3 File Offset: 0x00006BD3
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

	// Token: 0x0600100D RID: 4109 RVA: 0x0007A27C File Offset: 0x0007847C
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

	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x0600100E RID: 4110 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected float m_magma_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x0600100F RID: 4111 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float m_magma_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700078E RID: 1934
	// (get) Token: 0x06001010 RID: 4112 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected float m_magma_AttackHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700078F RID: 1935
	// (get) Token: 0x06001011 RID: 4113 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float m_magma_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000790 RID: 1936
	// (get) Token: 0x06001012 RID: 4114 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected float m_magma_Exit_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x06001013 RID: 4115 RVA: 0x0000452F File Offset: 0x0000272F
	protected float m_magma_Exit_Delay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000792 RID: 1938
	// (get) Token: 0x06001014 RID: 4116 RVA: 0x000089F7 File Offset: 0x00006BF7
	protected virtual Vector2 m_magma_JumpDelay
	{
		get
		{
			return new Vector2(0.15f, 0.35f);
		}
	}

	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x06001015 RID: 4117 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_magma_ProjectilesFired
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x06001016 RID: 4118 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float m_magma_ProjectileDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x06001017 RID: 4119 RVA: 0x0000456C File Offset: 0x0000276C
	protected virtual float m_magma_Exit_ForceIdle
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x06001018 RID: 4120 RVA: 0x000086B1 File Offset: 0x000068B1
	protected virtual float m_magma_Exit_AttackCD
	{
		get
		{
			return 8.5f;
		}
	}

	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x06001019 RID: 4121 RVA: 0x000054AD File Offset: 0x000036AD
	protected virtual int m_magma_angleAdder
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x0600101A RID: 4122 RVA: 0x00008A08 File Offset: 0x00006C08
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

	// Token: 0x0600101B RID: 4123 RVA: 0x00008A17 File Offset: 0x00006C17
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

	// Token: 0x17000798 RID: 1944
	// (get) Token: 0x0600101C RID: 4124 RVA: 0x0000457A File Offset: 0x0000277A
	protected float m_teleslice_DelayBetweenSlices
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x0600101D RID: 4125 RVA: 0x000052B0 File Offset: 0x000034B0
	protected float m_teleslice_TeleportDistance
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x00008A26 File Offset: 0x00006C26
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

	// Token: 0x0600101F RID: 4127 RVA: 0x0007A2C8 File Offset: 0x000784C8
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

	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x06001020 RID: 4128 RVA: 0x00008A35 File Offset: 0x00006C35
	protected virtual Vector2 m_boomerang_JumpDelay
	{
		get
		{
			return new Vector2(0.3f, 0.65f);
		}
	}

	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x06001021 RID: 4129 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_boomerang_ThrowLoopCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x00008A46 File Offset: 0x00006C46
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

	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x06001023 RID: 4131 RVA: 0x00008A55 File Offset: 0x00006C55
	protected virtual Vector2 m_spread_JumpDelay
	{
		get
		{
			return new Vector2(0.3f, 0.45f);
		}
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x00008A66 File Offset: 0x00006C66
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

	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x06001025 RID: 4133 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_lightning_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x06001026 RID: 4134 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_lightning_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x06001027 RID: 4135 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_lightning_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007A0 RID: 1952
	// (get) Token: 0x06001028 RID: 4136 RVA: 0x00008A75 File Offset: 0x00006C75
	protected virtual float m_lightning_TellHold_Delay
	{
		get
		{
			return 0.45f;
		}
	}

	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x06001029 RID: 4137 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_lightning_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x0600102A RID: 4138 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_lightning_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x0600102B RID: 4139 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_lightning_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x0600102C RID: 4140 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_lightning_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x0600102D RID: 4141 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_lightning_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x0600102E RID: 4142 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_lightning_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x0600102F RID: 4143 RVA: 0x0000456C File Offset: 0x0000276C
	protected virtual float m_lightning_Exit_ForceIdle
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x06001030 RID: 4144 RVA: 0x000086B1 File Offset: 0x000068B1
	protected virtual float m_lightning_Exit_AttackCD
	{
		get
		{
			return 8.5f;
		}
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x06001031 RID: 4145 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_lightning_Attack_DelayBetweenStrikesFirstPause
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x06001032 RID: 4146 RVA: 0x00003D8C File Offset: 0x00001F8C
	protected virtual float m_lightning_Attack_DelayBetweenStrikesSubsequent
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x06001033 RID: 4147 RVA: 0x00005FB8 File Offset: 0x000041B8
	protected virtual float m_lightning_Attack_Duration
	{
		get
		{
			return 0.6f;
		}
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x06001034 RID: 4148 RVA: 0x000046FA File Offset: 0x000028FA
	protected virtual int m_lightning_NumStrikes
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x06001035 RID: 4149 RVA: 0x00003D93 File Offset: 0x00001F93
	protected virtual float m_lightning_StrikeOffsetX
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x00008A7C File Offset: 0x00006C7C
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

	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x06001037 RID: 4151 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_scythe_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x06001038 RID: 4152 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_scythe_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x06001039 RID: 4153 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_scythe_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x0600103A RID: 4154 RVA: 0x000050CB File Offset: 0x000032CB
	protected virtual float m_scythe_TellHold_Delay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x0600103B RID: 4155 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_scythe_TellHold_Delay2
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007B3 RID: 1971
	// (get) Token: 0x0600103C RID: 4156 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_scythe_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007B4 RID: 1972
	// (get) Token: 0x0600103D RID: 4157 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_scythe_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x0600103E RID: 4158 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_scythe_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007B6 RID: 1974
	// (get) Token: 0x0600103F RID: 4159 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_scythe_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x06001040 RID: 4160 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_scythe_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007B8 RID: 1976
	// (get) Token: 0x06001041 RID: 4161 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_scythe_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007B9 RID: 1977
	// (get) Token: 0x06001042 RID: 4162 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float m_scythe_Exit_ForceIdle
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170007BA RID: 1978
	// (get) Token: 0x06001043 RID: 4163 RVA: 0x00005FB1 File Offset: 0x000041B1
	protected virtual float m_scythe_Exit_AttackCD
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x06001044 RID: 4164 RVA: 0x00008A8B File Offset: 0x00006C8B
	protected virtual float m_scythe_Attack_ForwardSpeedOverride
	{
		get
		{
			return 35.5f;
		}
	}

	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x06001045 RID: 4165 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_scythe_Attack_Duration
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x06001046 RID: 4166 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_scythe_RaiseKnockbackDefenseWhileAttacking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x06001047 RID: 4167 RVA: 0x00008A92 File Offset: 0x00006C92
	protected virtual int m_scythe_KnockbackDefenseBoostOverride
	{
		get
		{
			return 99;
		}
	}

	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x06001048 RID: 4168 RVA: 0x00008A96 File Offset: 0x00006C96
	protected virtual float m_scythe_AttackDuration
	{
		get
		{
			return 0.375f;
		}
	}

	// Token: 0x170007C0 RID: 1984
	// (get) Token: 0x06001049 RID: 4169 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float m_scythe_TimeBetweenAttacks
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170007C1 RID: 1985
	// (get) Token: 0x0600104A RID: 4170 RVA: 0x00008A9D File Offset: 0x00006C9D
	protected virtual Vector2 m_scythe_JumpDelay
	{
		get
		{
			return new Vector2(0.2f, 0.35f);
		}
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x00008AAE File Offset: 0x00006CAE
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

	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x0600104C RID: 4172 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dashMove_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x0600104D RID: 4173 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected virtual float m_dashMove_Exit_AttackCD
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x00008ABD File Offset: 0x00006CBD
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

	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x0600104F RID: 4175 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_jump_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x06001050 RID: 4176 RVA: 0x00003C70 File Offset: 0x00001E70
	protected virtual float m_jump_PowerX
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170007C6 RID: 1990
	// (get) Token: 0x06001051 RID: 4177 RVA: 0x00008ACC File Offset: 0x00006CCC
	protected virtual float m_jump_PowerY
	{
		get
		{
			return 33f;
		}
	}

	// Token: 0x170007C7 RID: 1991
	// (get) Token: 0x06001052 RID: 4178 RVA: 0x00008AD3 File Offset: 0x00006CD3
	protected virtual float m_lowjump_PowerX
	{
		get
		{
			return 26f;
		}
	}

	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x06001053 RID: 4179 RVA: 0x000052B0 File Offset: 0x000034B0
	protected virtual float m_lowjump_PowerY
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x06001054 RID: 4180 RVA: 0x00003C54 File Offset: 0x00001E54
	protected virtual float m_straightJump_PowerX
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170007CA RID: 1994
	// (get) Token: 0x06001055 RID: 4181 RVA: 0x0000530E File Offset: 0x0000350E
	protected virtual float m_straightJump_PowerY
	{
		get
		{
			return 28f;
		}
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x00008ADA File Offset: 0x00006CDA
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

	// Token: 0x06001057 RID: 4183 RVA: 0x00008AFE File Offset: 0x00006CFE
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

	// Token: 0x06001058 RID: 4184 RVA: 0x00008B0D File Offset: 0x00006D0D
	private void Single_Disable_Gravity()
	{
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.FallMultiplierOverride = 0f;
		base.EnemyController.ControllerCorgi.StickWhenWalkingDownSlopes = false;
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x00008B46 File Offset: 0x00006D46
	private void Single_Enable_Gravity()
	{
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.FallMultiplierOverride = 1f;
		base.EnemyController.ControllerCorgi.StickWhenWalkingDownSlopes = true;
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x00008B7F File Offset: 0x00006D7F
	public void Single_Enable_KnockbackDefense()
	{
		base.EnemyController.BaseKnockbackDefense = (float)this.KNOCKBACK_DEFENSE_OVERRIDE;
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x00008B93 File Offset: 0x00006D93
	protected void Single_Disable_KnockbackDefense()
	{
		base.EnemyController.BaseKnockbackDefense = (float)base.EnemyController.EnemyData.KnockbackDefence;
	}

	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x0600105C RID: 4188 RVA: 0x00004548 File Offset: 0x00002748
	protected virtual float m_dash_ForwardSpeedOverride
	{
		get
		{
			return 27.5f;
		}
	}

	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x0600105D RID: 4189 RVA: 0x00008BB1 File Offset: 0x00006DB1
	protected virtual float m_dash_BackwardSpeedOverride
	{
		get
		{
			return -27.5f;
		}
	}

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x0600105E RID: 4190 RVA: 0x00004FFB File Offset: 0x000031FB
	protected virtual float m_dashRelicWarningDuration
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x00008BB8 File Offset: 0x00006DB8
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

	// Token: 0x06001060 RID: 4192 RVA: 0x00008BD5 File Offset: 0x00006DD5
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

	// Token: 0x06001061 RID: 4193 RVA: 0x00008BEB File Offset: 0x00006DEB
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

	// Token: 0x06001062 RID: 4194 RVA: 0x00008BFA File Offset: 0x00006DFA
	private bool HasRelic(TraitorBoss_Basic_AIScript.TraitorRelicType relicType)
	{
		return (relicType & (TraitorBoss_Basic_AIScript.TraitorRelicType)this.m_relicMask) > (TraitorBoss_Basic_AIScript.TraitorRelicType)0;
	}

	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x06001063 RID: 4195 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_relicModeShift_TellIntro_AnimSpeed
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x06001064 RID: 4196 RVA: 0x00004A6C File Offset: 0x00002C6C
	protected virtual float m_relicModeShift_TellIntro_Delay
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x170007D0 RID: 2000
	// (get) Token: 0x06001065 RID: 4197 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_relicModeShift_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007D1 RID: 2001
	// (get) Token: 0x06001066 RID: 4198 RVA: 0x00004565 File Offset: 0x00002765
	protected virtual float m_relicModeShift_TellHold_Delay
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x00008C07 File Offset: 0x00006E07
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

	// Token: 0x06001068 RID: 4200 RVA: 0x00008C16 File Offset: 0x00006E16
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

	// Token: 0x06001069 RID: 4201 RVA: 0x00008C25 File Offset: 0x00006E25
	private void StopDamageZoneCoroutine()
	{
		this.StopPersistentCoroutine(this.m_damageZoneCoroutine);
		this.m_damageZoneCoroutine = null;
		base.StopProjectile(ref this.m_damageZoneProjectile);
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x00008C46 File Offset: 0x00006E46
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

	// Token: 0x0600106B RID: 4203 RVA: 0x00008C55 File Offset: 0x00006E55
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

	// Token: 0x0600106C RID: 4204 RVA: 0x00008C64 File Offset: 0x00006E64
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_spoonThrowEventInstance.isValid())
		{
			AudioManager.Stop(this.m_spoonThrowEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x040012ED RID: 4845
	[SerializeField]
	private LineRenderer m_aimLine;

	// Token: 0x040012EE RID: 4846
	[SerializeField]
	private GameObject m_endAimIndicator;

	// Token: 0x040012EF RID: 4847
	private static readonly float[] m_advancedModeshiftArray = new float[]
	{
		0.75f,
		0.5f,
		0.25f
	};

	// Token: 0x040012F0 RID: 4848
	private static readonly float[] m_expertModeshiftArray = new float[]
	{
		0.66f,
		0.33f
	};

	// Token: 0x040012F1 RID: 4849
	private static readonly float[] m_minibossModeshiftArray = new float[]
	{
		0.66f,
		0.33f
	};

	// Token: 0x040012F2 RID: 4850
	private const int RELIC_BLOCK_HIT_COUNT = 3;

	// Token: 0x040012F3 RID: 4851
	private const float RELIC_DAMAGE_ZONE_TRIGGER_INTERVAL = 10f;

	// Token: 0x040012F4 RID: 4852
	private const float RELIC_DAMAGE_ZONE_DAMAGE_DURATION = 2f;

	// Token: 0x040012F5 RID: 4853
	private const float RELIC_LAND_SHOCKWAVE_FALL_DURATION = 0.65f;

	// Token: 0x040012F6 RID: 4854
	private const string ATTACK_SFX_NAME_MALE = "event:/SFX/Enemies/Jonah/vo_jonah_attack";

	// Token: 0x040012F7 RID: 4855
	private const string ATTACK_SFX_NAME_FEMALE = "event:/SFX/Enemies/vo_jonah_female_attack";

	// Token: 0x040012F8 RID: 4856
	protected const string MAGMA_PROJECTILE = "TraitorMagmaProjectile";

	// Token: 0x040012F9 RID: 4857
	protected const string SPOON_PROJECTILE = "TraitorSpoonProjectile";

	// Token: 0x040012FA RID: 4858
	protected const string HOMING_PROJECTILE = "TraitorHomingBoltProjectile";

	// Token: 0x040012FB RID: 4859
	protected const string AXE_SPIN_PROJECTILE = "TraitorAxeSpinProjectile";

	// Token: 0x040012FC RID: 4860
	protected const string BOOMERANG_PROJECTILE = "TraitorBoomerangProjectile";

	// Token: 0x040012FD RID: 4861
	protected const string LIGHTNING_PROJECTILE = "TraitorLightningProjectile";

	// Token: 0x040012FE RID: 4862
	protected const string LIGHTNING_WARNING_PROJECTILE = "TraitorLightningWarningProjectile";

	// Token: 0x040012FF RID: 4863
	protected const string SCYTHE_PROJECTILE = "TraitorScytheProjectile";

	// Token: 0x04001300 RID: 4864
	protected const string SCYTHE_SECOND_PROJECTILE = "TraitorScytheSecondProjectile";

	// Token: 0x04001301 RID: 4865
	protected const string ARROW_PROJECTILE = "TraitorArrowProjectile";

	// Token: 0x04001302 RID: 4866
	protected const string TELESLICE_PROJECTILE = "TraitorTelesliceProjectile";

	// Token: 0x04001303 RID: 4867
	protected const string RELIC_LAND_PROJECTILE = "TraitorRelicLandProjectile";

	// Token: 0x04001304 RID: 4868
	protected const string RELIC_DASH_VOID_PROJECTILE = "TraitorRelicDashVoidProjectile";

	// Token: 0x04001305 RID: 4869
	protected const string RELIC_DASH_WARNING_PROJECTILE = "TraitorRelicDashVoidWarningProjectile";

	// Token: 0x04001306 RID: 4870
	protected const string RELIC_DAMAGE_ZONE_PROJECTILE = "TraitorRelicDamageAuraProjectile";

	// Token: 0x04001307 RID: 4871
	protected const string DASH_CURSE_PROJECTILE = "TraitorRelicDashCurseProjectile";

	// Token: 0x04001308 RID: 4872
	protected const string SHOUT_ATTACK_EXPLOSION_PROJECTILE_NAME = "TraitorBossShoutExplosionProjectile";

	// Token: 0x04001309 RID: 4873
	protected const string SHOUT_ATTACK_WARNING_PROJECTILE_NAME = "TraitorBossShoutWarningProjectile";

	// Token: 0x0400130A RID: 4874
	protected const string STAFFTHROW_BEAM_PROJECTILE = "TraitorForwardBeamProjectile";

	// Token: 0x0400130B RID: 4875
	protected const string STAFFTHROW_BEAM_WARNING_PROJECTILE = "TraitorWarningForwardBeamProjectile";

	// Token: 0x0400130C RID: 4876
	protected const int MID_POS_INDEX = 1;

	// Token: 0x0400130D RID: 4877
	private static TraitorBoss_Basic_AIScript.TraitorRelicType[] m_traitorRelicTypeArray;

	// Token: 0x0400130E RID: 4878
	protected const int NUM_RELIC_TYPES = 4;

	// Token: 0x0400130F RID: 4879
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x04001310 RID: 4880
	private bool m_isWalking;

	// Token: 0x04001311 RID: 4881
	private bool m_isJumping;

	// Token: 0x04001312 RID: 4882
	private bool m_isAxeSpinning;

	// Token: 0x04001313 RID: 4883
	private bool m_isAimingSpread;

	// Token: 0x04001314 RID: 4884
	private bool m_isAimingTeleslice;

	// Token: 0x04001315 RID: 4885
	private bool m_isModeShifting;

	// Token: 0x04001316 RID: 4886
	private bool m_prevIsGrounded = true;

	// Token: 0x04001317 RID: 4887
	private float m_prevIsGroundedStartTime;

	// Token: 0x04001318 RID: 4888
	protected int m_modeShiftIndex;

	// Token: 0x04001319 RID: 4889
	private int m_relicHitCount;

	// Token: 0x0400131A RID: 4890
	protected TraitorBossGeoController m_weaponGeoController;

	// Token: 0x0400131B RID: 4891
	private float m_aimAngle;

	// Token: 0x0400131C RID: 4892
	private List<RaycastHit2D> m_teleslicePlatformHitList = new List<RaycastHit2D>(5);

	// Token: 0x0400131D RID: 4893
	private int m_relicMask;

	// Token: 0x0400131E RID: 4894
	private int[] m_relicIndices;

	// Token: 0x0400131F RID: 4895
	protected const string MAGMA_ATTACK_INTRO = "SpellCast_Attack_Intro";

	// Token: 0x04001320 RID: 4896
	protected const string MAGMA_ATTACK_HOLD = "SpellCast_Attack_Hold";

	// Token: 0x04001321 RID: 4897
	protected const string MAGMA_EXIT = "SpellCast_Exit";

	// Token: 0x04001322 RID: 4898
	protected Vector2 m_magma_AngleRandomizerAdd = new Vector2(-30f, 30f);

	// Token: 0x04001323 RID: 4899
	protected Vector2 m_magma_PowerRandomizerMod = new Vector2(0.8f, 1.2f);

	// Token: 0x04001324 RID: 4900
	protected int m_magma_InitialAngle = 90;

	// Token: 0x04001325 RID: 4901
	private EventInstance m_spoonThrowEventInstance;

	// Token: 0x04001326 RID: 4902
	protected const string AXE_TELL_INTRO = "AxeGrounded_Tell_Intro";

	// Token: 0x04001327 RID: 4903
	protected const string AXE_TELL_HOLD = "AxeGrounded_Tell_Hold";

	// Token: 0x04001328 RID: 4904
	protected const string AXE_ATTACK_INTRO = "AxeAirborne_Attack_Intro";

	// Token: 0x04001329 RID: 4905
	protected const string AXE_ATTACK_HOLD = "AxeAirborne_Attack_Hold";

	// Token: 0x0400132A RID: 4906
	protected const string AXE_EXIT = "AxeAirborne_Exit";

	// Token: 0x0400132B RID: 4907
	protected float m_axe_TellIntro_AnimSpeed = 1.25f;

	// Token: 0x0400132C RID: 4908
	protected float m_axe_TellHold_AnimSpeed = 1.25f;

	// Token: 0x0400132D RID: 4909
	protected float m_axe_TellIntroAndHold_Delay = 0.35f;

	// Token: 0x0400132E RID: 4910
	protected float m_axe_AttackHold_AnimSpeed = 2f;

	// Token: 0x0400132F RID: 4911
	protected float m_axe_AttackHold_Delay;

	// Token: 0x04001330 RID: 4912
	protected float m_axe_Exit_AnimSpeed = 0.65f;

	// Token: 0x04001331 RID: 4913
	protected float m_axe_Exit_Delay;

	// Token: 0x04001332 RID: 4914
	protected float m_axe_Exit_IdleDuration = 0.1f;

	// Token: 0x04001333 RID: 4915
	protected float m_axe_AttackCD = 1f;

	// Token: 0x04001334 RID: 4916
	protected Projectile_RL m_axeSpinProjectile;

	// Token: 0x04001335 RID: 4917
	protected const string TELESLICE_TELL_INTRO = "Teleport_Tell_Intro";

	// Token: 0x04001336 RID: 4918
	protected const string TELESLICE_TELL_HOLD = "Teleport_Tell_Hold";

	// Token: 0x04001337 RID: 4919
	protected const string TELESLICE_ATTACK_INTRO = "Teleport_Attack_Intro";

	// Token: 0x04001338 RID: 4920
	protected const string TELESLICE_ATTACK_HOLD = "Teleport_Attack_Hold";

	// Token: 0x04001339 RID: 4921
	protected const string TELESLICE_EXIT = "Teleport_Exit";

	// Token: 0x0400133A RID: 4922
	protected float m_teleslice_TellIntro_AnimSpeed = 1f;

	// Token: 0x0400133B RID: 4923
	protected float m_teleslice_TellHold_AnimSpeed = 1f;

	// Token: 0x0400133C RID: 4924
	protected float m_teleslice_TellIntroAndHold_Delay = 0.8f;

	// Token: 0x0400133D RID: 4925
	protected float m_teleslice_AttackIntro_AnimSpeed = 1f;

	// Token: 0x0400133E RID: 4926
	protected float m_teleslice_AttackIntro_Delay;

	// Token: 0x0400133F RID: 4927
	protected float m_teleslice_AttackHold_AnimSpeed = 1f;

	// Token: 0x04001340 RID: 4928
	protected float m_teleslice_AttackHold_Delay = 0.1f;

	// Token: 0x04001341 RID: 4929
	protected float m_teleslice_Exit_AnimSpeed = 1f;

	// Token: 0x04001342 RID: 4930
	protected float m_teleslice_Exit_Delay;

	// Token: 0x04001343 RID: 4931
	protected float m_teleslice_Exit_IdleDuration;

	// Token: 0x04001344 RID: 4932
	protected float m_teleslice_AttackCD = 1f;

	// Token: 0x04001345 RID: 4933
	protected Vector2 TELESLICE_PROJECTILE_OFFSET = new Vector2(0f, 1f);

	// Token: 0x04001346 RID: 4934
	protected const string BOOMERANG_TELL_INTRO = "Chakram_Tell_Intro";

	// Token: 0x04001347 RID: 4935
	protected const string BOOMERANG_TELL_HOLD = "Chakram_Tell_Hold";

	// Token: 0x04001348 RID: 4936
	protected const string BOOMERANG_ATTACK_INTRO = "Chakram_Attack_Intro";

	// Token: 0x04001349 RID: 4937
	protected const string BOOMERANG_ATTACK_HOLD = "Chakram_Attack_Hold";

	// Token: 0x0400134A RID: 4938
	protected const string BOOMERANG_EXIT = "Chakram_Exit";

	// Token: 0x0400134B RID: 4939
	protected float m_boomerang_TellIntro_AnimSpeed = 1f;

	// Token: 0x0400134C RID: 4940
	protected float m_boomerang_TellHold_AnimSpeed = 1f;

	// Token: 0x0400134D RID: 4941
	protected float m_boomerang_TellIntroAndHold_Delay = 0.25f;

	// Token: 0x0400134E RID: 4942
	protected float m_boomerang_AttackIntro_AnimSpeed = 1f;

	// Token: 0x0400134F RID: 4943
	protected float m_boomerang_AttackIntro_Delay;

	// Token: 0x04001350 RID: 4944
	protected float m_boomerang_AttackHold_AnimSpeed = 1f;

	// Token: 0x04001351 RID: 4945
	protected float m_boomerang_AttackHold_Delay;

	// Token: 0x04001352 RID: 4946
	protected float m_boomerang_Exit_AnimSpeed = 0.65f;

	// Token: 0x04001353 RID: 4947
	protected float m_boomerang_Exit_Delay;

	// Token: 0x04001354 RID: 4948
	protected float m_boomerang_Exit_IdleDuration = 0.1f;

	// Token: 0x04001355 RID: 4949
	protected float m_boomerang_AttackCD = 10f;

	// Token: 0x04001356 RID: 4950
	protected float m_boomerang_LoopDelay = 0.5f;

	// Token: 0x04001357 RID: 4951
	protected float m_boomerang_ThrowPowerScaling = 0.5f;

	// Token: 0x04001358 RID: 4952
	protected const string SPREAD_TELL_INTRO = "Bow_Forward_Tell_Intro";

	// Token: 0x04001359 RID: 4953
	protected const string SPREAD_TELL_HOLD = "Bow_Forward_Tell_Hold";

	// Token: 0x0400135A RID: 4954
	protected const string SPREAD_ATTACK_INTRO = "Bow_Forward_Attack_Intro";

	// Token: 0x0400135B RID: 4955
	protected const string SPREAD_ATTACK_HOLD = "Bow_Forward_Attack_Hold";

	// Token: 0x0400135C RID: 4956
	protected const string SPREAD_EXIT = "Bow_Forward_Exit";

	// Token: 0x0400135D RID: 4957
	protected bool m_spread_AimAtPlayer = true;

	// Token: 0x0400135E RID: 4958
	protected float m_spread_TellIntro_AnimSpeed = 1f;

	// Token: 0x0400135F RID: 4959
	protected float m_spread_TellHold_AnimSpeed = 1f;

	// Token: 0x04001360 RID: 4960
	protected float m_spread_TellIntroAndHold_Delay;

	// Token: 0x04001361 RID: 4961
	protected float m_spread_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04001362 RID: 4962
	protected float m_spread_AttackIntro_Delay;

	// Token: 0x04001363 RID: 4963
	protected float m_spread_AttackHold_AnimSpeed = 1f;

	// Token: 0x04001364 RID: 4964
	protected float m_spread_AttackHold_Delay;

	// Token: 0x04001365 RID: 4965
	protected float m_spread_Exit_AnimSpeed = 1f;

	// Token: 0x04001366 RID: 4966
	protected float m_spread_Exit_Delay;

	// Token: 0x04001367 RID: 4967
	protected float m_spread_Exit_IdleDuration;

	// Token: 0x04001368 RID: 4968
	protected float m_spread_AttackCD = 1f;

	// Token: 0x04001369 RID: 4969
	protected Vector2 ARROW_POS_OFFSET = new Vector2(1f, 0.25f);

	// Token: 0x0400136A RID: 4970
	protected const string LIGHTNING_TELL_INTRO = "OmniSpellCast_Tell_Intro";

	// Token: 0x0400136B RID: 4971
	protected const string LIGHTNING_TELL_HOLD = "OmniSpellCast_Tell_Hold";

	// Token: 0x0400136C RID: 4972
	protected const string LIGHTNING_ATTACK_INTRO = "OmniSpellCast_Attack_Intro";

	// Token: 0x0400136D RID: 4973
	protected const string LIGHTNING_ATTACK_HOLD = "OmniSpellCast_Attack_Hold";

	// Token: 0x0400136E RID: 4974
	protected const string LIGHTNING_EXIT = "OmniSpellCast_Exit";

	// Token: 0x0400136F RID: 4975
	protected const string SCYTHE_TELL_INTRO = "Scythe_Tell_Intro";

	// Token: 0x04001370 RID: 4976
	protected const string SCYTHE_TELL_HOLD = "Scythe_Tell_Hold";

	// Token: 0x04001371 RID: 4977
	protected const string SCYTHE_ATTACK_INTRO = "Scythe_Attack_Intro";

	// Token: 0x04001372 RID: 4978
	protected const string SCYTHE_ATTACK_HOLD = "Scythe_Attack_Hold";

	// Token: 0x04001373 RID: 4979
	protected const string SCYTHE_TELL_INTRO2 = "Scythe_Tell_Intro_2";

	// Token: 0x04001374 RID: 4980
	protected const string SCYTHE_TELL_HOLD2 = "Scythe_Tell_Hold_2";

	// Token: 0x04001375 RID: 4981
	protected const string SCYTHE_ATTACK_INTRO2 = "Scythe_Attack_Intro_2";

	// Token: 0x04001376 RID: 4982
	protected const string SCYTHE_ATTACK_HOLD2 = "Scythe_Attack_Hold_2";

	// Token: 0x04001377 RID: 4983
	protected const string SCYTHE_EXIT = "Scythe_Exit";

	// Token: 0x04001378 RID: 4984
	protected Vector2 m_jump_Random_AddX = new Vector2(-1f, 10f);

	// Token: 0x04001379 RID: 4985
	protected Vector2 m_jump_Random_AddY = new Vector2(-3f, 3f);

	// Token: 0x0400137A RID: 4986
	private Projectile_RL m_dashRelicWarningProjectile;

	// Token: 0x0400137B RID: 4987
	protected const string RELIC_MODESHIFT_TELL_INTRO = "RetirePose";

	// Token: 0x0400137C RID: 4988
	protected const string RELIC_MODESHIFT_TELL_HOLD = "Victory";

	// Token: 0x0400137D RID: 4989
	protected float m_relicModeShift_Exit_IdleDuration = 0.1f;

	// Token: 0x0400137E RID: 4990
	protected float m_relicModeShift_AttackCD = 99f;

	// Token: 0x0400137F RID: 4991
	protected Coroutine m_damageZoneCoroutine;

	// Token: 0x04001380 RID: 4992
	protected Projectile_RL m_damageZoneProjectile;

	// Token: 0x04001381 RID: 4993
	protected const string DEATH_INTRO = "Stunned";

	// Token: 0x04001382 RID: 4994
	protected const string DEATH_HOLD = "Death_";

	// Token: 0x04001383 RID: 4995
	protected float m_death_Intro_Delay = 1f;

	// Token: 0x04001384 RID: 4996
	protected float m_death_Hold_Delay = 1f;

	// Token: 0x04001385 RID: 4997
	protected const string SPAWN_IDLE = "Idle";

	// Token: 0x04001386 RID: 4998
	protected const string SPAWN_INTRO = "Bow";

	// Token: 0x04001387 RID: 4999
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04001388 RID: 5000
	protected float m_spawn_Idle_Delay;

	// Token: 0x04001389 RID: 5001
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x0400138A RID: 5002
	protected float m_spawn_Intro_Delay;

	// Token: 0x02000238 RID: 568
	protected enum TraitorRelicType
	{
		// Token: 0x0400138C RID: 5004
		DamageZone = 1,
		// Token: 0x0400138D RID: 5005
		Jump,
		// Token: 0x0400138E RID: 5006
		Dash = 4,
		// Token: 0x0400138F RID: 5007
		Block = 8,
		// Token: 0x04001390 RID: 5008
		TwinDamageZone = 65536,
		// Token: 0x04001391 RID: 5009
		TwinJump = 131072,
		// Token: 0x04001392 RID: 5010
		TwinDash = 262144,
		// Token: 0x04001393 RID: 5011
		TwinBlock = 524288
	}
}
