using System;
using System.Collections;
using RLAudio;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001FE RID: 510
public class SpellswordBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000E05 RID: 3589 RVA: 0x000724D4 File Offset: 0x000706D4
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SpellSwordSpearBounceBoltProjectile",
			"SpellSwordStaffCircleBoltProjectile",
			"SpellSwordStaffCircleHomingBoltProjectile",
			"SpellSwordFlameGoutProjectile",
			"SpellSwordSlashDownProjectile",
			"SpellSwordSlashUpProjectile",
			"SpellSwordDaggerBoltBlueProjectile",
			"SpellSwordAxeSpinProjectile",
			"SpellSwordGroundRubbleProjectile",
			"SpellSwordStaffForwardBoltProjectile",
			"SpellSwordStaffForwardBeamProjectile",
			"SpellSwordStaffWarningForwardBeamProjectile",
			"SpellSwordStaffForwardBeamAdvancedProjectile",
			"SpellSwordStaffWarningForwardBeamAdvancedProjectile",
			"SpellSwordShoutWarningProjectile",
			"SpellSwordShoutAttackProjectile",
			"SpellSwordShoutHomingBoltProjectile",
			"SpellSwordCurseBoltProjectile",
			"SpellSwordVoidProjectile"
		};
	}

	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x06000E06 RID: 3590 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_modeShiftHealthMod
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x06000E07 RID: 3591 RVA: 0x00007E0D File Offset: 0x0000600D
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.75f, 1.25f);
		}
	}

	// Token: 0x17000690 RID: 1680
	// (get) Token: 0x06000E08 RID: 3592 RVA: 0x00007E1E File Offset: 0x0000601E
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1f, 1.5f);
		}
	}

	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x06000E09 RID: 3593 RVA: 0x00007E1E File Offset: 0x0000601E
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1f, 1.5f);
		}
	}

	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x06000E0A RID: 3594 RVA: 0x00004F89 File Offset: 0x00003189
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(1f, 6f);
		}
	}

	// Token: 0x17000693 RID: 1683
	// (get) Token: 0x06000E0B RID: 3595 RVA: 0x00007E2F File Offset: 0x0000602F
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 11f);
		}
	}

	// Token: 0x17000694 RID: 1684
	// (get) Token: 0x06000E0C RID: 3596 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool UseVariant
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x00072590 File Offset: 0x00070790
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.LogicController.DisableLogicActivationByDistance = true;
		base.EnemyController.HealthChangeRelay.AddListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit), false);
		this.m_modeShiftController = base.EnemyController.GetComponent<BossModeShiftController>();
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x00007E40 File Offset: 0x00006040
	public override void ResetScript()
	{
		base.ResetScript();
		this.m_inSecondMode = false;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		base.LogicController.DisableLogicActivationByDistance = true;
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x00007E6C File Offset: 0x0000606C
	private void OnDestroy()
	{
		if (base.EnemyController)
		{
			base.EnemyController.HealthChangeRelay.RemoveListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit));
		}
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x000725E0 File Offset: 0x000707E0
	private void OnBossHit(object sender, HealthChangeEventArgs args)
	{
		if (this.m_inSecondMode)
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
		if (base.EnemyController.CurrentHealth <= (float)base.EnemyController.ActualMaxHealth * this.m_modeShiftHealthMod)
		{
			this.m_inSecondMode = true;
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

	// Token: 0x06000E11 RID: 3601 RVA: 0x00007E98 File Offset: 0x00006098
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Spear_Thrust()
	{
		this.StopAndFaceTarget();
		float dashSpeed = this.m_thrust_AttackSpeed;
		if (!base.EnemyController.IsTargetToMyRight)
		{
			dashSpeed = -dashSpeed;
		}
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("SpearThrust_Tell_Intro", this.m_thrust_TellIntro_AnimSpeed, "SpearThrust_Tell_Hold", this.m_thrust_TellHold_AnimSpeed, this.m_thrust_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("SpearThrust_Attack_Intro", this.m_thrust_AttackIntro_AnimSpeed, this.m_thrust_AttackIntro_Delay, true);
		yield return this.Default_Animation("SpearThrust_Attack_Hold", this.m_thrust_AttackHold_AnimSpeed, this.m_thrust_AttackHold_Delay, false);
		base.SetVelocityX(dashSpeed, false);
		if (this.UseVariant && this.m_thrust_AttackDuration > 0f)
		{
			float fireInterval = this.m_thrust_AttackDuration / (float)this.m_numThrustDaggers;
			int num = this.m_thrustDaggerSpread / (this.m_numThrustDaggers - 1);
			int num2;
			for (int i = 0; i < this.m_numThrustDaggers; i = num2 + 1)
			{
				float angle = 90f;
				if (!base.EnemyController.IsFacingRight)
				{
					angle = 90f;
				}
				this.FireProjectile("SpellSwordSpearBounceBoltProjectile", 13, false, angle, 1f, true, true, true);
				yield return base.Wait(fireInterval, false);
				num2 = i;
			}
		}
		else if (this.m_thrust_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_thrust_AttackDuration, false);
		}
		base.SetVelocityX(0f, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Animation("SpearThrust_Exit", this.m_thrust_Exit_AnimSpeed, this.m_thrust_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_thrust_Exit_IdleDuration, this.m_thrust_AttackCD);
		yield break;
	}

	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x06000E12 RID: 3602 RVA: 0x00007B8D File Offset: 0x00005D8D
	protected virtual int m_numStaffFireballs
	{
		get
		{
			return 25;
		}
	}

	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x06000E13 RID: 3603 RVA: 0x00005315 File Offset: 0x00003515
	protected virtual int m_numStaffFireballs_AddSecondMode
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x00007EA7 File Offset: 0x000060A7
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Staff_Attack()
	{
		base.SetVelocityX(0f, false);
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("StaffCircle_Tell_Intro", this.m_staff_TellIntro_AnimSpeed, "StaffCircle_Tell_Hold", this.m_staff_TellHold_AnimSpeed, this.m_staff_TellIntroAndHold_Delay);
		yield return this.Default_Animation("StaffCircle_Attack_Intro", this.m_staff_AttackIntro_AnimSpeed, this.m_staff_AttackIntro_Delay, true);
		yield return this.Default_Animation("StaffCircle_Attack_Hold", this.m_staff_AttackHold_AnimSpeed, this.m_staff_AttackHold_Delay, false);
		if (this.m_staff_AttackDuration > 0f)
		{
			int fireballAmount;
			if (this.UseVariant)
			{
				fireballAmount = this.m_numStaffFireballs_Variant;
			}
			else
			{
				fireballAmount = this.m_numStaffFireballs;
			}
			if (this.m_inSecondMode)
			{
				fireballAmount += this.m_numStaffFireballs_AddSecondMode;
			}
			float fireInterval = this.m_staff_AttackDuration / (float)fireballAmount;
			float startingAngle = (float)UnityEngine.Random.Range(0, 360);
			int num = CDGHelper.RandomPlusMinus();
			float fireAngleInterval = (float)(this.m_staffFireballSpread / (fireballAmount - 1) * num);
			int num2;
			for (int i = 0; i < fireballAmount; i = num2 + 1)
			{
				float angle = startingAngle + fireAngleInterval * (float)i;
				if (this.UseVariant)
				{
					this.FireProjectile("SpellSwordStaffCircleHomingBoltProjectile", 7, false, angle, 1f, true, true, true);
				}
				else
				{
					this.FireProjectile("SpellSwordStaffCircleBoltProjectile", 7, false, angle, 1f, true, true, true);
				}
				yield return base.Wait(fireInterval, false);
				num2 = i;
			}
		}
		else if (this.m_staff_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_staff_AttackDuration, false);
		}
		base.StopProjectile(ref this.m_staffShieldProjectile);
		if (this.m_staff_AttackHold_ExitDelay > 0f)
		{
			yield return base.Wait(this.m_staff_AttackHold_ExitDelay, false);
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("StaffCircle_Exit", this.m_staff_Exit_AnimSpeed, this.m_staff_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_staff_Exit_IdleDuration, this.m_staff_AttackCD);
		yield break;
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x00007EB6 File Offset: 0x000060B6
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Sword_Attack()
	{
		base.SetVelocityX(0f, false);
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		float dashSpeed = this.m_sword_AttackSpeed;
		if (!base.EnemyController.IsTargetToMyRight)
		{
			dashSpeed = -dashSpeed;
		}
		yield return this.Default_TellIntroAndLoop("Sword1_Tell_Intro", this.m_sword_TellIntro_AnimSpeed, "Sword1_Tell_Hold", this.m_sword_TellHold_AnimSpeed, this.m_sword_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Sword1_Attack_Intro", this.m_sword_AttackIntro_AnimSpeed, this.m_sword_AttackIntro_Delay, true);
		yield return this.Default_Animation("Sword1_Attack_Hold", this.m_sword_AttackHold_AnimSpeed, this.m_sword_AttackHold_Delay, false);
		this.FireProjectile("SpellSwordSlashDownProjectile", 0, true, 0f, 1f, true, true, true);
		base.SetVelocityX(dashSpeed, false);
		if (this.m_sword_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_sword_AttackDuration, false);
		}
		base.SetVelocityX(0f, false);
		if (this.m_sword_PostAttack_Delay > 0f)
		{
			yield return base.Wait(this.m_sword_PostAttack_Delay, false);
		}
		yield return this.Default_Animation("Sword2_Tell_Intro", this.m_sword2_TellIntro_AnimSpeed, 0f, true);
		base.EnemyController.LockFlip = false;
		this.StopAndFaceTarget();
		base.Animator.SetBool("Turn", false);
		base.EnemyController.LockFlip = true;
		dashSpeed = this.m_sword2_AttackSpeed;
		if (!base.EnemyController.IsFacingRight)
		{
			dashSpeed = -dashSpeed;
		}
		yield return this.Default_Animation("Sword2_Tell_Hold", this.m_sword2_TellHold_AnimSpeed, this.m_sword2_TellIntroAndHold_Delay, true);
		yield return this.Default_Animation("Sword2_Attack_Intro", this.m_sword2_AttackIntro_AnimSpeed, this.m_sword2_AttackIntro_Delay, true);
		yield return this.Default_Animation("Sword2_Attack_Hold", this.m_sword2_AttackHold_AnimSpeed, this.m_sword2_AttackHold_Delay, false);
		this.FireProjectile("SpellSwordSlashUpProjectile", 0, true, 0f, 1f, true, true, true);
		this.m_swordFlameProjectile = this.FireProjectile("SpellSwordFlameGoutProjectile", 3, true, 0f, 1f, true, true, true);
		this.m_swordFlameProjectile2 = this.FireProjectile("SpellSwordFlameGoutProjectile", 10, true, 0f, 1f, true, true, true);
		if ((this.UseVariant || this.m_inSecondMode) && base.LogicController.EnemyLogicType != EnemyLogicType.Expert)
		{
			this.m_swordFlameProjectile3 = this.FireProjectile("SpellSwordFlameGoutProjectile", 11, true, 0f, 1f, true, true, true);
		}
		if (this.UseVariant && this.m_inSecondMode)
		{
			this.m_swordFlameProjectile4 = this.FireProjectile("SpellSwordFlameGoutProjectile", 12, true, 0f, 1f, true, true, true);
		}
		base.SetVelocityX(dashSpeed, false);
		if (this.m_sword2_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_sword2_AttackDuration, false);
		}
		base.SetVelocityX(0f, false);
		if (this.m_sword2_PostAttack_Delay > 0f)
		{
			yield return base.Wait(this.m_sword2_PostAttack_Delay, false);
		}
		base.StopProjectile(ref this.m_swordFlameProjectile);
		base.StopProjectile(ref this.m_swordFlameProjectile2);
		base.StopProjectile(ref this.m_swordFlameProjectile3);
		base.StopProjectile(ref this.m_swordFlameProjectile4);
		yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		base.SetVelocityX(0f, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.SetVelocityX(0f, false);
		yield return this.Default_Animation("Sword2_Exit", this.m_sword_Exit_AnimSpeed, this.m_sword_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_sword_Exit_IdleDuration, this.m_sword_AttackCD);
		yield break;
	}

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x06000E16 RID: 3606 RVA: 0x000047A4 File Offset: 0x000029A4
	protected virtual int m_numDaggerThrowPirhouettes
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x06000E17 RID: 3607 RVA: 0x000047A7 File Offset: 0x000029A7
	protected virtual int m_numDaggerThrowDaggers_SecondModeAdd
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x00007EC5 File Offset: 0x000060C5
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Dagger_Throw()
	{
		base.EnemyController.LockFlip = false;
		base.EnemyController.SetVelocityX(0f, false);
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("DaggerThrow_Tell_Intro", this.m_daggerThrow_TellIntro_AnimSpeed, "DaggerThrow_Tell_Hold", this.m_daggerThrow_TellHold_AnimSpeed, this.m_daggerThrow_TellIntroAndHold_Delay);
		int num;
		for (int i = 0; i < this.m_numDaggerThrowPirhouettes; i = num + 1)
		{
			if (i > 0)
			{
				base.EnemyController.LockFlip = false;
				base.EnemyController.SetVelocityX(0f, false);
				this.StopAndFaceTarget();
				base.EnemyController.LockFlip = true;
			}
			yield return this.Default_Animation("DaggerThrow_Attack_Intro", this.m_daggerThrow_AttackIntro_AnimSpeed, this.m_daggerThrow_AttackIntro_Delay, true);
			yield return this.Default_Animation("DaggerThrow_Attack_Hold", this.m_daggerThrow_AttackHold_AnimSpeed, this.m_daggerThrow_AttackHold_Delay, false);
			int numDaggers = this.m_numDaggerThrowDaggers;
			if (this.UseVariant)
			{
				numDaggers = this.m_numDaggerThrowDaggers_Variant;
			}
			if (this.m_inSecondMode)
			{
				numDaggers += this.m_numDaggerThrowDaggers_SecondModeAdd;
			}
			float fireInterval = this.m_daggerThrow_AttackDuration / (float)numDaggers;
			float fireAngle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
			for (int j = 0; j < numDaggers; j = num + 1)
			{
				this.FireProjectile("SpellSwordDaggerBoltBlueProjectile", 1, false, fireAngle, 1f, true, true, true);
				if (this.UseVariant)
				{
					this.FireProjectile("SpellSwordDaggerBoltBlueProjectile", 1, false, fireAngle + 5f, 1f, true, true, true);
					this.FireProjectile("SpellSwordDaggerBoltBlueProjectile", 1, false, fireAngle - 5f, 1f, true, true, true);
				}
				yield return base.Wait(fireInterval, false);
				num = j;
			}
			if (this.m_daggerThrowPirhouetteDelay > 0f)
			{
				yield return base.Wait(this.m_daggerThrowPirhouetteDelay, false);
			}
			num = i;
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("DaggerThrow_Exit", this.m_daggerThrow_Exit_AnimSpeed, this.m_daggerThrow_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		base.EnemyController.Animator.ResetTrigger("Turn");
		yield return this.Default_Attack_Cooldown(this.m_daggerThrow_Exit_IdleDuration, this.m_daggerThrow_AttackCD);
		yield break;
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x00007ED4 File Offset: 0x000060D4
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Axe_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		yield return this.Default_Animation("Jump_Tell", this.m_jump_TellIntro_AnimSpeed, this.m_jump_TellIntroAndHold_Delay, true);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.ChangeAnimationState("Jump_Up_Intro");
		Vector2 axeAttackVelocity = this.m_axeAttackVelocity;
		if (!base.EnemyController.IsFacingRight)
		{
			axeAttackVelocity.x = -axeAttackVelocity.x;
		}
		base.SetVelocity(axeAttackVelocity, false);
		yield return base.Wait(0.1f, false);
		yield return this.Default_TellIntroAndLoop("AxeJump_Tell_Intro", this.m_axeAttack_TellIntro_AnimSpeed, "AxeJump_Tell_Hold", this.m_axeAttack_TellHold_AnimSpeed, this.m_axeAttack_TellIntroAndHold_Delay);
		yield return this.Default_Animation("AxeJump_Attack_Intro", this.m_axeAttack_AttackIntro_AnimSpeed, this.m_axeAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("AxeJump_Attack_Hold", this.m_axeAttack_AttackHold_AnimSpeed, this.m_axeAttack_AttackHold_Delay, false);
		this.m_axeSpinProjectile = this.FireProjectile("SpellSwordAxeSpinProjectile", 1, false, 0f, 1f, true, true, true);
		yield return base.WaitUntilIsGrounded();
		base.StopProjectile(ref this.m_axeSpinProjectile);
		float speedMod = UnityEngine.Random.Range(this.m_axeAttack_Land_ThrowPower.x, this.m_axeAttack_Land_ThrowPower.y);
		int num = UnityEngine.Random.Range((int)this.m_axeAttack_Land_ThrowAngle.x, (int)this.m_axeAttack_Land_ThrowAngle.y);
		int num2 = UnityEngine.Random.Range((int)this.m_axeAttack_Land_ThrowAngle2.x, (int)this.m_axeAttack_Land_ThrowAngle2.y);
		int num3 = UnityEngine.Random.Range((int)this.m_axeAttack_Land_ThrowAngle3.x, (int)this.m_axeAttack_Land_ThrowAngle3.y);
		int num4 = UnityEngine.Random.Range((int)this.m_axeAttack_Land_ThrowAngle4.x, (int)this.m_axeAttack_Land_ThrowAngle4.y);
		int num5 = UnityEngine.Random.Range((int)this.m_axeAttack_Land_ThrowAngle5.x, (int)this.m_axeAttack_Land_ThrowAngle5.y);
		int num6 = UnityEngine.Random.Range((int)this.m_axeAttack_Land_ThrowAngle6.x, (int)this.m_axeAttack_Land_ThrowAngle6.y);
		this.FireProjectile("SpellSwordGroundRubbleProjectile", 0, true, (float)num, speedMod, true, true, true);
		this.FireProjectile("SpellSwordGroundRubbleProjectile", 0, true, (float)(180 - num), speedMod, true, true, true);
		this.FireProjectile("SpellSwordGroundRubbleProjectile", 5, true, (float)num2, speedMod, true, true, true);
		this.FireProjectile("SpellSwordGroundRubbleProjectile", 5, true, (float)(180 - num2), speedMod, true, true, true);
		this.FireProjectile("SpellSwordGroundRubbleProjectile", 5, true, (float)num3, speedMod, true, true, true);
		this.FireProjectile("SpellSwordGroundRubbleProjectile", 5, true, (float)(180 - num3), speedMod, true, true, true);
		if (base.LogicController.EnemyLogicType != EnemyLogicType.Expert)
		{
			if (this.m_inSecondMode)
			{
				this.FireProjectile("SpellSwordGroundRubbleProjectile", 5, true, (float)num4, speedMod, true, true, true);
				this.FireProjectile("SpellSwordGroundRubbleProjectile", 5, true, (float)(180 - num4), speedMod, true, true, true);
			}
			if (this.UseVariant)
			{
				this.FireProjectile("SpellSwordGroundRubbleProjectile", 5, true, (float)num5, speedMod, true, true, true);
				this.FireProjectile("SpellSwordGroundRubbleProjectile", 5, true, (float)(180 - num5), speedMod, true, true, true);
				this.FireProjectile("SpellSwordVoidProjectile", 5, false, 0f, 1f, true, true, true);
				this.FireProjectile("SpellSwordVoidProjectile", 5, false, 180f, 1f, true, true, true).Flip();
			}
			if (this.UseVariant && this.m_inSecondMode)
			{
				this.FireProjectile("SpellSwordGroundRubbleProjectile", 5, true, (float)num6, speedMod, true, true, true);
				this.FireProjectile("SpellSwordGroundRubbleProjectile", 5, true, (float)(180 - num6), speedMod, true, true, true);
			}
		}
		base.SetVelocityX(0f, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("AxeJump_Land", this.m_axeAttack_Exit_AnimSpeed, this.m_axeAttack_Exit_Delay, true);
		if (this.m_axeAttack_ExitHold_Delay > 0f)
		{
			yield return base.Wait(this.m_axeAttack_ExitHold_Delay, false);
		}
		yield return this.Default_Animation("AxeJump_Exit", this.m_axeAttack_Exit_AnimSpeed, this.m_axeAttack_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_axeAttack_Exit_IdleDuration, this.m_axeAttack_AttackCD);
		yield break;
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x00007EE3 File Offset: 0x000060E3
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Staff_Throw()
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		if (this.UseVariant)
		{
			this.m_staffBeamWarningProjectile = this.FireProjectile("SpellSwordStaffWarningForwardBeamAdvancedProjectile", 8, true, 0f, 1f, true, true, true);
			yield return this.Default_TellIntroAndLoop("StaffForward_Tell_Intro", this.m_staffThrow_TellIntro_AnimSpeed, "StaffForward_Tell_Hold", this.m_staffThrow_TellHold_AnimSpeed, this.m_staffThrow_TellIntroAndHold_Delay);
		}
		else
		{
			this.m_staffBeamWarningProjectile = this.FireProjectile("SpellSwordStaffWarningForwardBeamProjectile", 8, true, 0f, 1f, true, true, true);
			yield return this.Default_TellIntroAndLoop("StaffForward_Tell_Intro", this.m_staffThrow_TellIntro_AnimSpeed, "StaffForward_Tell_Hold", this.m_staffThrow_TellHold_AnimSpeed, this.m_staffThrow_TellIntroAndHold_Delay);
		}
		yield return this.Default_Animation("StaffForward_Attack_Intro", this.m_staffThrow_AttackIntro_AnimSpeed, this.m_staffThrow_AttackIntro_Delay, true);
		yield return this.Default_Animation("StaffForward_Attack_Hold", this.m_staffThrow_AttackHold_AnimSpeed, this.m_staffThrow_AttackHold_Delay, false);
		base.StopProjectile(ref this.m_staffBeamWarningProjectile);
		if (this.UseVariant)
		{
			this.m_staffBeamProjectile = this.FireProjectile("SpellSwordStaffForwardBeamAdvancedProjectile", 8, true, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_staffBeamProjectile = this.FireProjectile("SpellSwordStaffForwardBeamProjectile", 8, true, 0f, 1f, true, true, true);
		}
		if (this.UseVariant)
		{
			if (this.m_staffThrow_BeamAttack_Variant_Duration > 0f)
			{
				yield return base.Wait(this.m_staffThrow_BeamAttack_Variant_Duration, false);
			}
		}
		else if (this.m_staffThrow_BeamAttack_Duration > 0f)
		{
			yield return base.Wait(this.m_staffThrow_BeamAttack_Duration, false);
		}
		base.StopProjectile(ref this.m_staffBeamProjectile);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return base.Wait(this.m_staffThrow_AttackDuration_EndDelay, false);
		yield return this.Default_Animation("StaffForward_Attack_Exit", this.m_staffThrow_Exit_AnimSpeed, this.m_staffThrow_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_staffThrow_Exit_IdleDuration, this.m_staffThrow_AttackCD);
		yield break;
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x00007EF2 File Offset: 0x000060F2
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
		yield return base.DeathAnim();
		base.EnemyController.LockFlip = true;
		base.SetVelocityX(0f, false);
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_Damage_Mod;
		yield return this.Default_Animation("ModeShift_Intro", this.m_modeShift_Downed_AnimSpeed, this.m_modeShift_Downed_Delay, true);
		yield return this.Default_Animation("ModeShift_GetUp", this.m_modeShift_GetUp_AnimSpeed, 0f, true);
		if (this.ModeShiftEvent != null)
		{
			this.ModeShiftEvent.Invoke();
		}
		base.LogicController.SetLogicBlockEnabled("Axe_Attack", true);
		base.LogicController.SetLogicBlockEnabled("Staff_Throw", true);
		this.m_modeShiftProjectile = this.FireProjectile("SpellSwordShoutWarningProjectile", 9, true, 0f, 1f, true, true, true);
		if (this.m_modeShift_GetUp_Delay > 0f)
		{
			yield return base.Wait(this.m_modeShift_GetUp_Delay, false);
		}
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("ModeShift_Scream_Intro", this.m_modeShift_AttackIntro_AnimSpeed, this.m_modeShift_AttackIntro_Delay, true);
		base.StopProjectile(ref this.m_modeShiftProjectile);
		yield return this.Default_Animation("ModeShift_Scream_Hold", this.m_modeShift_AttackHold_AnimSpeed, this.m_modeShift_AttackHold_Delay, false);
		this.m_modeShiftProjectile2 = this.FireProjectile("SpellSwordShoutAttackProjectile", 9, true, 0f, 1f, true, true, true);
		if (this.m_modeShift_AttackDuration_Initial_Delay > 0f)
		{
			yield return base.Wait(this.m_modeShift_AttackDuration_Initial_Delay, false);
		}
		CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
		int i;
		if (this.UseVariant)
		{
			i = 0;
			while ((float)i < this.m_modeShift_ProjectilesThrown_Variant)
			{
				float speedMod = UnityEngine.Random.Range(this.m_modeShift_Shout_ThrowPower.x, this.m_modeShift_Shout_ThrowPower.y);
				float angle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
				angle = (float)(90 + UnityEngine.Random.Range((int)this.m_modeShift_Shout_ThrowAngle.x, (int)this.m_modeShift_Shout_ThrowAngle.y));
				this.FireProjectile("SpellSwordShoutHomingBoltProjectile", 9, false, angle, speedMod, true, true, true);
				yield return base.Wait(this.m_modeShift_AttackDuration_Fire_Duration / this.m_modeShift_ProjectilesThrown_Variant, false);
				int num = i;
				i = num + 1;
			}
			if (this.m_modeShift_AttackDuration_SecondShot_Delay > 0f)
			{
				yield return base.Wait(this.m_modeShift_AttackDuration_SecondShot_Delay, false);
			}
		}
		i = 0;
		while ((float)i < this.m_modeShift_ProjectilesThrown_Variant)
		{
			float speedMod2 = UnityEngine.Random.Range(this.m_modeShift_Shout_ThrowPower.x, this.m_modeShift_Shout_ThrowPower.y);
			float angle2 = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
			angle2 = (float)(90 + UnityEngine.Random.Range((int)this.m_modeShift_Shout_ThrowAngle.x, (int)this.m_modeShift_Shout_ThrowAngle.y));
			this.FireProjectile("SpellSwordShoutHomingBoltProjectile", 9, false, angle2, speedMod2, true, true, true);
			yield return base.Wait(this.m_modeShift_AttackDuration_Fire_Duration / this.m_modeShift_ProjectilesThrown_Variant, false);
			int num = i;
			i = num + 1;
		}
		if (this.m_modeShift_AttackDuration_Exit_Delay > 0f)
		{
			yield return base.Wait(this.m_modeShift_AttackDuration_Exit_Delay, false);
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.StopProjectile(ref this.m_modeShiftProjectile2);
		yield return this.Default_Animation("ModeShift_Scream_Exit", this.m_modeShift_Exit_AnimSpeed, this.m_modeShift_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.ResetBaseValues();
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		yield return this.Default_Attack_Cooldown(this.m_modeShift_Exit_IdleDuration, this.m_modeShift_AttackCD);
		yield break;
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x00007F01 File Offset: 0x00006101
	public override IEnumerator DeathAnim()
	{
		yield return base.DeathAnim();
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 0.5f);
		yield return this.Default_Animation("Death_Intro", this.m_death_Intro_AnimSpeed, this.m_death_Intro_Delay, true);
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		base.EnemyController.GetComponent<BossModeShiftController>().DissolveMaterials(this.m_death_Hold_Delay);
		yield return this.Default_Animation("Death_Loop", this.m_death_Hold_AnimSpeed, this.m_death_Hold_Delay, true);
		yield break;
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x00007F10 File Offset: 0x00006110
	public override IEnumerator SpawnAnim()
	{
		yield return this.Default_Animation("Intro_Idle", this.m_spawn_Idle_AnimSpeed, this.m_spawn_Idle_Delay, true);
		MusicManager.PlayMusic(SongID.CastleBossBGM_ASTIP_Boss1_Phase1, false, false);
		yield return this.Default_Animation("Intro", this.m_spawn_Intro_AnimSpeed, this.m_spawn_Intro_Delay, true);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		yield break;
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x00072688 File Offset: 0x00070888
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_swordFlameProjectile);
		base.StopProjectile(ref this.m_swordFlameProjectile2);
		base.StopProjectile(ref this.m_swordFlameProjectile3);
		base.StopProjectile(ref this.m_staffBeamWarningProjectile);
		base.StopProjectile(ref this.m_staffBeamProjectile);
		this.m_isStaffBeaming = false;
		base.StopProjectile(ref this.m_modeShiftProjectile);
		base.StopProjectile(ref this.m_modeShiftProjectile2);
		base.StopProjectile(ref this.m_axeSpinProjectile);
		this.m_isAxeSpinning = false;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x0007270C File Offset: 0x0007090C
	public override void Pause()
	{
		base.Pause();
		if (this.m_axeSpinProjectile && this.m_axeSpinProjectile.isActiveAndEnabled && this.m_axeSpinProjectile.OwnerController == base.EnemyController)
		{
			this.m_isAxeSpinning = true;
			base.StopProjectile(ref this.m_axeSpinProjectile);
		}
		if (this.m_staffBeamProjectile && this.m_staffBeamProjectile.isActiveAndEnabled && this.m_staffBeamProjectile.OwnerController == base.EnemyController)
		{
			this.m_isStaffBeaming = true;
			base.StopProjectile(ref this.m_staffBeamProjectile);
			if (this.UseVariant)
			{
				this.m_staffBeamWarningProjectile = this.FireProjectile("SpellSwordStaffWarningForwardBeamAdvancedProjectile", 8, true, 0f, 1f, true, true, true);
			}
			else
			{
				this.m_staffBeamWarningProjectile = this.FireProjectile("SpellSwordStaffWarningForwardBeamProjectile", 8, true, 0f, 1f, true, true, true);
			}
		}
		if (this.m_swordFlameProjectile && this.m_swordFlameProjectile.isActiveAndEnabled && this.m_swordFlameProjectile.OwnerController == base.EnemyController)
		{
			base.StopProjectile(ref this.m_swordFlameProjectile);
			base.StopProjectile(ref this.m_swordFlameProjectile2);
			base.StopProjectile(ref this.m_swordFlameProjectile3);
		}
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x0007284C File Offset: 0x00070A4C
	public override void Unpause()
	{
		base.Unpause();
		if (this.m_isAxeSpinning)
		{
			this.m_isAxeSpinning = false;
			this.m_axeSpinProjectile = this.FireProjectile("SpellSwordAxeSpinProjectile", 1, false, 0f, 1f, true, true, true);
		}
		if (this.m_isStaffBeaming)
		{
			this.m_isStaffBeaming = false;
			if (this.UseVariant)
			{
				this.m_staffBeamProjectile = this.FireProjectile("SpellSwordStaffForwardBeamAdvancedProjectile", 8, true, 0f, 1f, true, true, true);
			}
			else
			{
				this.m_staffBeamProjectile = this.FireProjectile("SpellSwordStaffForwardBeamProjectile", 8, true, 0f, 1f, true, true, true);
			}
			base.StopProjectile(ref this.m_staffBeamWarningProjectile);
		}
	}

	// Token: 0x0400104F RID: 4175
	public UnityEvent ModeShiftEvent;

	// Token: 0x04001050 RID: 4176
	protected const int SPAWN_POS_INDEX = 0;

	// Token: 0x04001051 RID: 4177
	protected const int MID_POS_INDEX = 1;

	// Token: 0x04001052 RID: 4178
	protected const int TOP_POS_INDEX = 2;

	// Token: 0x04001053 RID: 4179
	protected const int FRONT_POS_INDEX = 3;

	// Token: 0x04001054 RID: 4180
	protected const int MID_FRONT_POS_INDEX = 4;

	// Token: 0x04001055 RID: 4181
	protected const int FOOT_FRONT_POS_INDEX = 5;

	// Token: 0x04001056 RID: 4182
	protected const int FOOT_BACK_POS_INDEX = 6;

	// Token: 0x04001057 RID: 4183
	protected const int STAFF_TOP_POS_INDEX = 7;

	// Token: 0x04001058 RID: 4184
	protected const int STAFF_FRONT_POS_INDEX = 8;

	// Token: 0x04001059 RID: 4185
	protected const int SHOUT_MOUTH_POS_INDEX = 9;

	// Token: 0x0400105A RID: 4186
	protected const int FAR_FRONT_POS_INDEX = 10;

	// Token: 0x0400105B RID: 4187
	protected const int VERY_FAR_POS_INDEX = 11;

	// Token: 0x0400105C RID: 4188
	protected const int SUPER_FAR_POS_INDEX = 12;

	// Token: 0x0400105D RID: 4189
	protected const int LOW_GROUND_POS_INDEX = 13;

	// Token: 0x0400105E RID: 4190
	private bool m_inSecondMode;

	// Token: 0x0400105F RID: 4191
	private BossModeShiftController m_modeShiftController;

	// Token: 0x04001060 RID: 4192
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x04001061 RID: 4193
	protected const string THRUST_TELL_INTRO = "SpearThrust_Tell_Intro";

	// Token: 0x04001062 RID: 4194
	protected const string THRUST_TELL_HOLD = "SpearThrust_Tell_Hold";

	// Token: 0x04001063 RID: 4195
	protected const string THRUST_ATTACK_INTRO = "SpearThrust_Attack_Intro";

	// Token: 0x04001064 RID: 4196
	protected const string THRUST_ATTACK_HOLD = "SpearThrust_Attack_Hold";

	// Token: 0x04001065 RID: 4197
	protected const string THRUST_EXIT = "SpearThrust_Exit";

	// Token: 0x04001066 RID: 4198
	protected const string THRUST_SPEAR_PROJECTILE = "SpellSwordSpearBounceBoltProjectile";

	// Token: 0x04001067 RID: 4199
	protected float m_thrust_AttackSpeed = 35f;

	// Token: 0x04001068 RID: 4200
	protected float m_thrust_AttackDuration = 0.65f;

	// Token: 0x04001069 RID: 4201
	protected float m_thrust_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x0400106A RID: 4202
	protected float m_thrust_TellHold_AnimSpeed = 1.2f;

	// Token: 0x0400106B RID: 4203
	protected float m_thrust_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x0400106C RID: 4204
	protected float m_thrust_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x0400106D RID: 4205
	protected float m_thrust_AttackIntro_Delay;

	// Token: 0x0400106E RID: 4206
	protected float m_thrust_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x0400106F RID: 4207
	protected float m_thrust_AttackHold_Delay;

	// Token: 0x04001070 RID: 4208
	protected float m_thrust_Exit_AnimSpeed = 0.65f;

	// Token: 0x04001071 RID: 4209
	protected float m_thrust_Exit_Delay = 0.45f;

	// Token: 0x04001072 RID: 4210
	protected float m_thrust_Exit_IdleDuration = 0.15f;

	// Token: 0x04001073 RID: 4211
	protected float m_thrust_AttackCD = 12f;

	// Token: 0x04001074 RID: 4212
	protected int m_numThrustDaggers = 8;

	// Token: 0x04001075 RID: 4213
	protected int m_thrustDaggerSpread = 90;

	// Token: 0x04001076 RID: 4214
	protected const string STAFF_TELL_INTRO = "StaffCircle_Tell_Intro";

	// Token: 0x04001077 RID: 4215
	protected const string STAFF_TELL_HOLD = "StaffCircle_Tell_Hold";

	// Token: 0x04001078 RID: 4216
	protected const string STAFF_ATTACK_INTRO = "StaffCircle_Attack_Intro";

	// Token: 0x04001079 RID: 4217
	protected const string STAFF_ATTACK_HOLD = "StaffCircle_Attack_Hold";

	// Token: 0x0400107A RID: 4218
	protected const string STAFF_EXIT = "StaffCircle_Exit";

	// Token: 0x0400107B RID: 4219
	protected const string STAFF_CIRCLE_PROJECTILE = "SpellSwordStaffCircleBoltProjectile";

	// Token: 0x0400107C RID: 4220
	protected const string STAFF_CIRCLEHOMING_PROJECTILE = "SpellSwordStaffCircleHomingBoltProjectile";

	// Token: 0x0400107D RID: 4221
	protected float m_staff_AttackDuration = 2f;

	// Token: 0x0400107E RID: 4222
	protected float m_staff_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x0400107F RID: 4223
	protected float m_staff_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04001080 RID: 4224
	protected float m_staff_TellIntroAndHold_Delay = 0.95f;

	// Token: 0x04001081 RID: 4225
	protected float m_staff_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04001082 RID: 4226
	protected float m_staff_AttackIntro_Delay;

	// Token: 0x04001083 RID: 4227
	protected float m_staff_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04001084 RID: 4228
	protected float m_staff_AttackHold_Delay;

	// Token: 0x04001085 RID: 4229
	protected float m_staff_AttackHold_ExitDelay = 0.75f;

	// Token: 0x04001086 RID: 4230
	protected float m_staff_Exit_AnimSpeed = 1.2f;

	// Token: 0x04001087 RID: 4231
	protected float m_staff_Exit_Delay;

	// Token: 0x04001088 RID: 4232
	protected float m_staff_Exit_IdleDuration = 0.15f;

	// Token: 0x04001089 RID: 4233
	protected float m_staff_AttackCD = 12f;

	// Token: 0x0400108A RID: 4234
	private Projectile_RL m_staffShieldProjectile;

	// Token: 0x0400108B RID: 4235
	protected int m_numStaffFireballs_Variant = 15;

	// Token: 0x0400108C RID: 4236
	protected int m_staffFireballSpread = 720;

	// Token: 0x0400108D RID: 4237
	protected const string SWORD1_TELL_INTRO = "Sword1_Tell_Intro";

	// Token: 0x0400108E RID: 4238
	protected const string SWORD1_TELL_HOLD = "Sword1_Tell_Hold";

	// Token: 0x0400108F RID: 4239
	protected const string SWORD1_ATTACK_INTRO = "Sword1_Attack_Intro";

	// Token: 0x04001090 RID: 4240
	protected const string SWORD1_ATTACK_HOLD = "Sword1_Attack_Hold";

	// Token: 0x04001091 RID: 4241
	protected const string SWORD2_TELL_INTRO = "Sword2_Tell_Intro";

	// Token: 0x04001092 RID: 4242
	protected const string SWORD2_TELL_HOLD = "Sword2_Tell_Hold";

	// Token: 0x04001093 RID: 4243
	protected const string SWORD2_ATTACK_INTRO = "Sword2_Attack_Intro";

	// Token: 0x04001094 RID: 4244
	protected const string SWORD2_ATTACK_HOLD = "Sword2_Attack_Hold";

	// Token: 0x04001095 RID: 4245
	protected const string SWORD2_EXIT = "Sword2_Exit";

	// Token: 0x04001096 RID: 4246
	protected const string SWORD_FLAMEGOUT_PROJECTILE = "SpellSwordFlameGoutProjectile";

	// Token: 0x04001097 RID: 4247
	protected const string SWORD1_PROJECTILE = "SpellSwordSlashDownProjectile";

	// Token: 0x04001098 RID: 4248
	protected const string SWORD2_PROJECTILE = "SpellSwordSlashUpProjectile";

	// Token: 0x04001099 RID: 4249
	protected float m_sword_AttackSpeed = 16f;

	// Token: 0x0400109A RID: 4250
	protected float m_sword_AttackDuration = 0.185f;

	// Token: 0x0400109B RID: 4251
	protected float m_sword_PostAttack_Delay = 0.1f;

	// Token: 0x0400109C RID: 4252
	protected float m_sword2_PostAttack_Delay = 0.75f;

	// Token: 0x0400109D RID: 4253
	protected float m_sword2_AttackSpeed = 15f;

	// Token: 0x0400109E RID: 4254
	protected float m_sword2_AttackDuration = 0.175f;

	// Token: 0x0400109F RID: 4255
	protected float m_sword_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x040010A0 RID: 4256
	protected float m_sword_TellHold_AnimSpeed = 1.2f;

	// Token: 0x040010A1 RID: 4257
	protected float m_sword_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x040010A2 RID: 4258
	protected float m_sword_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x040010A3 RID: 4259
	protected float m_sword_AttackIntro_Delay;

	// Token: 0x040010A4 RID: 4260
	protected float m_sword_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x040010A5 RID: 4261
	protected float m_sword_AttackHold_Delay;

	// Token: 0x040010A6 RID: 4262
	protected float m_sword2_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x040010A7 RID: 4263
	protected float m_sword2_TellHold_AnimSpeed = 1.2f;

	// Token: 0x040010A8 RID: 4264
	protected float m_sword2_TellIntroAndHold_Delay = 0.35f;

	// Token: 0x040010A9 RID: 4265
	protected float m_sword2_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x040010AA RID: 4266
	protected float m_sword2_AttackIntro_Delay;

	// Token: 0x040010AB RID: 4267
	protected float m_sword2_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x040010AC RID: 4268
	protected float m_sword2_AttackHold_Delay;

	// Token: 0x040010AD RID: 4269
	protected float m_sword_Exit_AnimSpeed = 0.65f;

	// Token: 0x040010AE RID: 4270
	protected float m_sword_Exit_Delay;

	// Token: 0x040010AF RID: 4271
	protected float m_sword_Exit_IdleDuration = 0.15f;

	// Token: 0x040010B0 RID: 4272
	protected float m_sword_AttackCD = 8f;

	// Token: 0x040010B1 RID: 4273
	private Projectile_RL m_swordFlameProjectile;

	// Token: 0x040010B2 RID: 4274
	private Projectile_RL m_swordFlameProjectile2;

	// Token: 0x040010B3 RID: 4275
	private Projectile_RL m_swordFlameProjectile3;

	// Token: 0x040010B4 RID: 4276
	private Projectile_RL m_swordFlameProjectile4;

	// Token: 0x040010B5 RID: 4277
	protected const string DAGGERTHROW_TELL_INTRO = "DaggerThrow_Tell_Intro";

	// Token: 0x040010B6 RID: 4278
	protected const string DAGGERTHROW_TELL_HOLD = "DaggerThrow_Tell_Hold";

	// Token: 0x040010B7 RID: 4279
	protected const string DAGGERTHROW_ATTACK_INTRO = "DaggerThrow_Attack_Intro";

	// Token: 0x040010B8 RID: 4280
	protected const string DAGGERTHROW_ATTACK_HOLD = "DaggerThrow_Attack_Hold";

	// Token: 0x040010B9 RID: 4281
	protected const string DAGGERTHROW_EXIT = "DaggerThrow_Exit";

	// Token: 0x040010BA RID: 4282
	protected const string DAGGERTHROW_PROJECTILE = "SpellSwordDaggerBoltBlueProjectile";

	// Token: 0x040010BB RID: 4283
	protected float m_daggerThrowPirhouetteDelay = 0.15f;

	// Token: 0x040010BC RID: 4284
	protected int m_numDaggerThrowDaggers = 3;

	// Token: 0x040010BD RID: 4285
	protected int m_numDaggerThrowDaggers_Variant = 3;

	// Token: 0x040010BE RID: 4286
	protected float m_daggerThrow_AttackSpeed = 28f;

	// Token: 0x040010BF RID: 4287
	protected float m_daggerThrow_AttackDuration = 0.25f;

	// Token: 0x040010C0 RID: 4288
	protected float m_daggerThrow_TellIntro_AnimSpeed = 1f;

	// Token: 0x040010C1 RID: 4289
	protected float m_daggerThrow_TellHold_AnimSpeed = 1f;

	// Token: 0x040010C2 RID: 4290
	protected float m_daggerThrow_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x040010C3 RID: 4291
	protected float m_daggerThrow_AttackIntro_AnimSpeed = 1f;

	// Token: 0x040010C4 RID: 4292
	protected float m_daggerThrow_AttackIntro_Delay;

	// Token: 0x040010C5 RID: 4293
	protected float m_daggerThrow_AttackHold_AnimSpeed = 1f;

	// Token: 0x040010C6 RID: 4294
	protected float m_daggerThrow_AttackHold_Delay;

	// Token: 0x040010C7 RID: 4295
	protected float m_daggerThrow_Exit_AnimSpeed = 0.65f;

	// Token: 0x040010C8 RID: 4296
	protected float m_daggerThrow_Exit_Delay;

	// Token: 0x040010C9 RID: 4297
	protected float m_daggerThrow_Exit_IdleDuration = 0.15f;

	// Token: 0x040010CA RID: 4298
	protected float m_daggerThrow_AttackCD = 12f;

	// Token: 0x040010CB RID: 4299
	protected const string AXE_TELL_INTRO = "AxeJump_Tell_Intro";

	// Token: 0x040010CC RID: 4300
	protected const string AXE_TELL_HOLD = "AxeJump_Tell_Hold";

	// Token: 0x040010CD RID: 4301
	protected const string AXE_ATTACK_INTRO = "AxeJump_Attack_Intro";

	// Token: 0x040010CE RID: 4302
	protected const string AXE_ATTACK_HOLD = "AxeJump_Attack_Hold";

	// Token: 0x040010CF RID: 4303
	protected const string AXE_EXIT_BLADE_OUT = "AxeJump_Land";

	// Token: 0x040010D0 RID: 4304
	protected const string AXE_EXIT_RETRACT = "AxeJump_Exit";

	// Token: 0x040010D1 RID: 4305
	protected const string AXE_SPIN_PROJECTILE = "SpellSwordAxeSpinProjectile";

	// Token: 0x040010D2 RID: 4306
	protected const string AXE_LAND_PROJECTILE = "SpellSwordGroundRubbleProjectile";

	// Token: 0x040010D3 RID: 4307
	protected const string AXE_LAND_ADVANCED_PROJECTILE = "SpellSwordVoidProjectile";

	// Token: 0x040010D4 RID: 4308
	private Projectile_RL m_axeSpinProjectile;

	// Token: 0x040010D5 RID: 4309
	protected Vector2 m_axeAttackVelocity = new Vector2(12f, 32f);

	// Token: 0x040010D6 RID: 4310
	protected Vector2 m_axeAttack_Land_ThrowAngle = new Vector2(85f, 85f);

	// Token: 0x040010D7 RID: 4311
	protected Vector2 m_axeAttack_Land_ThrowAngle2 = new Vector2(78f, 78f);

	// Token: 0x040010D8 RID: 4312
	protected Vector2 m_axeAttack_Land_ThrowAngle3 = new Vector2(71f, 71f);

	// Token: 0x040010D9 RID: 4313
	protected Vector2 m_axeAttack_Land_ThrowAngle4 = new Vector2(55f, 55f);

	// Token: 0x040010DA RID: 4314
	protected Vector2 m_axeAttack_Land_ThrowAngle5 = new Vector2(82f, 82f);

	// Token: 0x040010DB RID: 4315
	protected Vector2 m_axeAttack_Land_ThrowAngle6 = new Vector2(74f, 74f);

	// Token: 0x040010DC RID: 4316
	protected Vector2 m_axeAttack_Land_ThrowPower = new Vector2(1f, 1f);

	// Token: 0x040010DD RID: 4317
	protected float m_jump_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x040010DE RID: 4318
	protected float m_jump_TellHold_AnimSpeed = 1.2f;

	// Token: 0x040010DF RID: 4319
	protected float m_jump_TellIntroAndHold_Delay = 0.65f;

	// Token: 0x040010E0 RID: 4320
	protected float m_axeAttack_TellIntro_AnimSpeed = 1.5f;

	// Token: 0x040010E1 RID: 4321
	protected float m_axeAttack_TellHold_AnimSpeed = 1.5f;

	// Token: 0x040010E2 RID: 4322
	protected float m_axeAttack_TellIntroAndHold_Delay;

	// Token: 0x040010E3 RID: 4323
	protected float m_axeAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x040010E4 RID: 4324
	protected float m_axeAttack_AttackIntro_Delay;

	// Token: 0x040010E5 RID: 4325
	protected float m_axeAttack_AttackHold_AnimSpeed = 1.5f;

	// Token: 0x040010E6 RID: 4326
	protected float m_axeAttack_AttackHold_Delay;

	// Token: 0x040010E7 RID: 4327
	protected float m_axeAttack_ExitHold_Delay = 0.375f;

	// Token: 0x040010E8 RID: 4328
	protected float m_axeAttack_ExitHold_AnimSpeed = 1.2f;

	// Token: 0x040010E9 RID: 4329
	protected float m_axeAttack_Exit_AnimSpeed = 1.2f;

	// Token: 0x040010EA RID: 4330
	protected float m_axeAttack_Exit_Delay;

	// Token: 0x040010EB RID: 4331
	protected float m_axeAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x040010EC RID: 4332
	protected float m_axeAttack_AttackCD = 5f;

	// Token: 0x040010ED RID: 4333
	protected const string STAFFTHROW_TELL_INTRO = "StaffForward_Tell_Intro";

	// Token: 0x040010EE RID: 4334
	protected const string STAFFTHROW_TELL_HOLD = "StaffForward_Tell_Hold";

	// Token: 0x040010EF RID: 4335
	protected const string STAFFTHROW_ATTACK_INTRO = "StaffForward_Attack_Intro";

	// Token: 0x040010F0 RID: 4336
	protected const string STAFFTHROW_ATTACK_HOLD = "StaffForward_Attack_Hold";

	// Token: 0x040010F1 RID: 4337
	protected const string STAFFTHROW_EXIT = "StaffForward_Attack_Exit";

	// Token: 0x040010F2 RID: 4338
	protected const string STAFFTHROW_PROJECTILE = "SpellSwordStaffForwardBoltProjectile";

	// Token: 0x040010F3 RID: 4339
	protected const string STAFFTHROW_BEAM_PROJECTILE = "SpellSwordStaffForwardBeamProjectile";

	// Token: 0x040010F4 RID: 4340
	protected const string STAFFTHROW_BEAM_WARNING_PROJECTILE = "SpellSwordStaffWarningForwardBeamProjectile";

	// Token: 0x040010F5 RID: 4341
	protected const string STAFFTHROW_BEAM_ADVANCED_PROJECTILE = "SpellSwordStaffForwardBeamAdvancedProjectile";

	// Token: 0x040010F6 RID: 4342
	protected const string STAFFTHROW_BEAM_WARNING_ADVANCED_PROJECTILE = "SpellSwordStaffWarningForwardBeamAdvancedProjectile";

	// Token: 0x040010F7 RID: 4343
	protected float m_staffThrow_BeamAttack_Duration = 0.75f;

	// Token: 0x040010F8 RID: 4344
	protected float m_staffThrow_BeamAttack_Variant_Duration = 1f;

	// Token: 0x040010F9 RID: 4345
	protected float m_staffThrow_AttackSpeed = 28f;

	// Token: 0x040010FA RID: 4346
	protected float m_staffThrow_AttackDuration_EndDelay = 1f;

	// Token: 0x040010FB RID: 4347
	protected float m_staffThrow_TellIntro_AnimSpeed = 1.5f;

	// Token: 0x040010FC RID: 4348
	protected float m_staffThrow_TellHold_AnimSpeed = 1f;

	// Token: 0x040010FD RID: 4349
	protected float m_staffThrow_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x040010FE RID: 4350
	protected float m_staffThrow_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x040010FF RID: 4351
	protected float m_staffThrow_AttackIntro_Delay;

	// Token: 0x04001100 RID: 4352
	protected float m_staffThrow_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04001101 RID: 4353
	protected float m_staffThrow_AttackHold_Delay;

	// Token: 0x04001102 RID: 4354
	protected float m_staffThrow_Exit_AnimSpeed = 1f;

	// Token: 0x04001103 RID: 4355
	protected float m_staffThrow_Exit_Delay;

	// Token: 0x04001104 RID: 4356
	protected float m_staffThrow_Exit_IdleDuration = 0.15f;

	// Token: 0x04001105 RID: 4357
	protected float m_staffThrow_AttackCD = 12f;

	// Token: 0x04001106 RID: 4358
	private Projectile_RL m_staffBeamProjectile;

	// Token: 0x04001107 RID: 4359
	private Projectile_RL m_staffBeamWarningProjectile;

	// Token: 0x04001108 RID: 4360
	protected const string MODESHIFT_DOWNED = "ModeShift_Intro";

	// Token: 0x04001109 RID: 4361
	protected const string MODESHIFT_GETUP = "ModeShift_GetUp";

	// Token: 0x0400110A RID: 4362
	protected const string MODESHIFT_ATTACK_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x0400110B RID: 4363
	protected const string MODESHIFT_ATTACK_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x0400110C RID: 4364
	protected const string MODESHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x0400110D RID: 4365
	protected const string MODESHIFT_PROJECTILE_WARNING = "SpellSwordShoutWarningProjectile";

	// Token: 0x0400110E RID: 4366
	protected const string MODESHIFT_PROJECTILE_ATTACK = "SpellSwordShoutAttackProjectile";

	// Token: 0x0400110F RID: 4367
	protected const string MODESHIFT_PROJECTILE_HOMING = "SpellSwordShoutHomingBoltProjectile";

	// Token: 0x04001110 RID: 4368
	private Projectile_RL m_modeShiftProjectile;

	// Token: 0x04001111 RID: 4369
	private Projectile_RL m_modeShiftProjectile2;

	// Token: 0x04001112 RID: 4370
	protected float m_modeShift_AttackSpeed = 28f;

	// Token: 0x04001113 RID: 4371
	protected float m_modeShift_AttackDuration_Initial_Delay = 0.5f;

	// Token: 0x04001114 RID: 4372
	protected float m_modeShift_AttackDuration_Fire_Duration = 0.05f;

	// Token: 0x04001115 RID: 4373
	protected float m_modeShift_AttackDuration_Exit_Delay = 0.5f;

	// Token: 0x04001116 RID: 4374
	protected float m_modeShift_AttackDuration_SecondShot_Delay = 1.5f;

	// Token: 0x04001117 RID: 4375
	protected float m_modeShift_ProjectilesThrown_Variant = 5f;

	// Token: 0x04001118 RID: 4376
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x04001119 RID: 4377
	protected Vector2 m_modeShift_Shout_ThrowAngle = new Vector2(-40f, 40f);

	// Token: 0x0400111A RID: 4378
	protected Vector2 m_modeShift_Shout_ThrowPower = new Vector2(0.525f, 0.625f);

	// Token: 0x0400111B RID: 4379
	protected float m_modeShift_Downed_AnimSpeed = 1f;

	// Token: 0x0400111C RID: 4380
	protected float m_modeShift_Downed_Delay = 1.3f;

	// Token: 0x0400111D RID: 4381
	protected float m_modeShift_GetUp_AnimSpeed = 1f;

	// Token: 0x0400111E RID: 4382
	protected float m_modeShift_GetUp_Delay = 1f;

	// Token: 0x0400111F RID: 4383
	protected float m_modeShift_AttackIntro_AnimSpeed = 1.25f;

	// Token: 0x04001120 RID: 4384
	protected float m_modeShift_AttackIntro_Delay;

	// Token: 0x04001121 RID: 4385
	protected float m_modeShift_AttackHold_AnimSpeed = 1.25f;

	// Token: 0x04001122 RID: 4386
	protected float m_modeShift_AttackHold_Delay;

	// Token: 0x04001123 RID: 4387
	protected float m_modeShift_Exit_AnimSpeed = 0.65f;

	// Token: 0x04001124 RID: 4388
	protected float m_modeShift_Exit_Delay;

	// Token: 0x04001125 RID: 4389
	protected float m_modeShift_Exit_IdleDuration = 0.15f;

	// Token: 0x04001126 RID: 4390
	protected float m_modeShift_AttackCD = 99f;

	// Token: 0x04001127 RID: 4391
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x04001128 RID: 4392
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x04001129 RID: 4393
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x0400112A RID: 4394
	protected float m_death_Intro_Delay;

	// Token: 0x0400112B RID: 4395
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x0400112C RID: 4396
	protected float m_death_Hold_Delay = 4.5f;

	// Token: 0x0400112D RID: 4397
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x0400112E RID: 4398
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x0400112F RID: 4399
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04001130 RID: 4400
	protected float m_spawn_Idle_Delay = 2f;

	// Token: 0x04001131 RID: 4401
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04001132 RID: 4402
	protected float m_spawn_Intro_Delay;

	// Token: 0x04001133 RID: 4403
	protected bool m_isAxeSpinning;

	// Token: 0x04001134 RID: 4404
	protected bool m_isStaffBeaming;
}
