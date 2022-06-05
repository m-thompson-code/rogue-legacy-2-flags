using System;
using System.Collections;
using RLAudio;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200012A RID: 298
public class SpellswordBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000938 RID: 2360 RVA: 0x0001E04C File Offset: 0x0001C24C
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

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06000939 RID: 2361 RVA: 0x0001E108 File Offset: 0x0001C308
	protected virtual float m_modeShiftHealthMod
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x0600093A RID: 2362 RVA: 0x0001E10F File Offset: 0x0001C30F
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.75f, 1.25f);
		}
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x0600093B RID: 2363 RVA: 0x0001E120 File Offset: 0x0001C320
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1f, 1.5f);
		}
	}

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x0600093C RID: 2364 RVA: 0x0001E131 File Offset: 0x0001C331
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1f, 1.5f);
		}
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x0600093D RID: 2365 RVA: 0x0001E142 File Offset: 0x0001C342
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(1f, 6f);
		}
	}

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x0600093E RID: 2366 RVA: 0x0001E153 File Offset: 0x0001C353
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 11f);
		}
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x0600093F RID: 2367 RVA: 0x0001E164 File Offset: 0x0001C364
	protected virtual bool UseVariant
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0001E168 File Offset: 0x0001C368
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.LogicController.DisableLogicActivationByDistance = true;
		base.EnemyController.HealthChangeRelay.AddListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit), false);
		this.m_modeShiftController = base.EnemyController.GetComponent<BossModeShiftController>();
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0001E1B7 File Offset: 0x0001C3B7
	public override void ResetScript()
	{
		base.ResetScript();
		this.m_inSecondMode = false;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		base.LogicController.DisableLogicActivationByDistance = true;
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0001E1E3 File Offset: 0x0001C3E3
	private void OnDestroy()
	{
		if (base.EnemyController)
		{
			base.EnemyController.HealthChangeRelay.RemoveListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit));
		}
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0001E210 File Offset: 0x0001C410
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

	// Token: 0x06000944 RID: 2372 RVA: 0x0001E2B8 File Offset: 0x0001C4B8
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

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06000945 RID: 2373 RVA: 0x0001E2C7 File Offset: 0x0001C4C7
	protected virtual int m_numStaffFireballs
	{
		get
		{
			return 25;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06000946 RID: 2374 RVA: 0x0001E2CB File Offset: 0x0001C4CB
	protected virtual int m_numStaffFireballs_AddSecondMode
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x0001E2CF File Offset: 0x0001C4CF
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

	// Token: 0x06000948 RID: 2376 RVA: 0x0001E2DE File Offset: 0x0001C4DE
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

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06000949 RID: 2377 RVA: 0x0001E2ED File Offset: 0x0001C4ED
	protected virtual int m_numDaggerThrowPirhouettes
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x0600094A RID: 2378 RVA: 0x0001E2F0 File Offset: 0x0001C4F0
	protected virtual int m_numDaggerThrowDaggers_SecondModeAdd
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0001E2F3 File Offset: 0x0001C4F3
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

	// Token: 0x0600094C RID: 2380 RVA: 0x0001E302 File Offset: 0x0001C502
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

	// Token: 0x0600094D RID: 2381 RVA: 0x0001E311 File Offset: 0x0001C511
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

	// Token: 0x0600094E RID: 2382 RVA: 0x0001E320 File Offset: 0x0001C520
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

	// Token: 0x0600094F RID: 2383 RVA: 0x0001E32F File Offset: 0x0001C52F
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

	// Token: 0x06000950 RID: 2384 RVA: 0x0001E33E File Offset: 0x0001C53E
	public override IEnumerator SpawnAnim()
	{
		yield return this.Default_Animation("Intro_Idle", this.m_spawn_Idle_AnimSpeed, this.m_spawn_Idle_Delay, true);
		MusicManager.PlayMusic(SongID.CastleBossBGM_ASTIP_Boss1_Phase1, false, false);
		yield return this.Default_Animation("Intro", this.m_spawn_Intro_AnimSpeed, this.m_spawn_Intro_Delay, true);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		yield break;
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0001E350 File Offset: 0x0001C550
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

	// Token: 0x06000952 RID: 2386 RVA: 0x0001E3D4 File Offset: 0x0001C5D4
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

	// Token: 0x06000953 RID: 2387 RVA: 0x0001E514 File Offset: 0x0001C714
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

	// Token: 0x04000CCE RID: 3278
	public UnityEvent ModeShiftEvent;

	// Token: 0x04000CCF RID: 3279
	protected const int SPAWN_POS_INDEX = 0;

	// Token: 0x04000CD0 RID: 3280
	protected const int MID_POS_INDEX = 1;

	// Token: 0x04000CD1 RID: 3281
	protected const int TOP_POS_INDEX = 2;

	// Token: 0x04000CD2 RID: 3282
	protected const int FRONT_POS_INDEX = 3;

	// Token: 0x04000CD3 RID: 3283
	protected const int MID_FRONT_POS_INDEX = 4;

	// Token: 0x04000CD4 RID: 3284
	protected const int FOOT_FRONT_POS_INDEX = 5;

	// Token: 0x04000CD5 RID: 3285
	protected const int FOOT_BACK_POS_INDEX = 6;

	// Token: 0x04000CD6 RID: 3286
	protected const int STAFF_TOP_POS_INDEX = 7;

	// Token: 0x04000CD7 RID: 3287
	protected const int STAFF_FRONT_POS_INDEX = 8;

	// Token: 0x04000CD8 RID: 3288
	protected const int SHOUT_MOUTH_POS_INDEX = 9;

	// Token: 0x04000CD9 RID: 3289
	protected const int FAR_FRONT_POS_INDEX = 10;

	// Token: 0x04000CDA RID: 3290
	protected const int VERY_FAR_POS_INDEX = 11;

	// Token: 0x04000CDB RID: 3291
	protected const int SUPER_FAR_POS_INDEX = 12;

	// Token: 0x04000CDC RID: 3292
	protected const int LOW_GROUND_POS_INDEX = 13;

	// Token: 0x04000CDD RID: 3293
	private bool m_inSecondMode;

	// Token: 0x04000CDE RID: 3294
	private BossModeShiftController m_modeShiftController;

	// Token: 0x04000CDF RID: 3295
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x04000CE0 RID: 3296
	protected const string THRUST_TELL_INTRO = "SpearThrust_Tell_Intro";

	// Token: 0x04000CE1 RID: 3297
	protected const string THRUST_TELL_HOLD = "SpearThrust_Tell_Hold";

	// Token: 0x04000CE2 RID: 3298
	protected const string THRUST_ATTACK_INTRO = "SpearThrust_Attack_Intro";

	// Token: 0x04000CE3 RID: 3299
	protected const string THRUST_ATTACK_HOLD = "SpearThrust_Attack_Hold";

	// Token: 0x04000CE4 RID: 3300
	protected const string THRUST_EXIT = "SpearThrust_Exit";

	// Token: 0x04000CE5 RID: 3301
	protected const string THRUST_SPEAR_PROJECTILE = "SpellSwordSpearBounceBoltProjectile";

	// Token: 0x04000CE6 RID: 3302
	protected float m_thrust_AttackSpeed = 35f;

	// Token: 0x04000CE7 RID: 3303
	protected float m_thrust_AttackDuration = 0.65f;

	// Token: 0x04000CE8 RID: 3304
	protected float m_thrust_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000CE9 RID: 3305
	protected float m_thrust_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000CEA RID: 3306
	protected float m_thrust_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04000CEB RID: 3307
	protected float m_thrust_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000CEC RID: 3308
	protected float m_thrust_AttackIntro_Delay;

	// Token: 0x04000CED RID: 3309
	protected float m_thrust_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000CEE RID: 3310
	protected float m_thrust_AttackHold_Delay;

	// Token: 0x04000CEF RID: 3311
	protected float m_thrust_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000CF0 RID: 3312
	protected float m_thrust_Exit_Delay = 0.45f;

	// Token: 0x04000CF1 RID: 3313
	protected float m_thrust_Exit_IdleDuration = 0.15f;

	// Token: 0x04000CF2 RID: 3314
	protected float m_thrust_AttackCD = 12f;

	// Token: 0x04000CF3 RID: 3315
	protected int m_numThrustDaggers = 8;

	// Token: 0x04000CF4 RID: 3316
	protected int m_thrustDaggerSpread = 90;

	// Token: 0x04000CF5 RID: 3317
	protected const string STAFF_TELL_INTRO = "StaffCircle_Tell_Intro";

	// Token: 0x04000CF6 RID: 3318
	protected const string STAFF_TELL_HOLD = "StaffCircle_Tell_Hold";

	// Token: 0x04000CF7 RID: 3319
	protected const string STAFF_ATTACK_INTRO = "StaffCircle_Attack_Intro";

	// Token: 0x04000CF8 RID: 3320
	protected const string STAFF_ATTACK_HOLD = "StaffCircle_Attack_Hold";

	// Token: 0x04000CF9 RID: 3321
	protected const string STAFF_EXIT = "StaffCircle_Exit";

	// Token: 0x04000CFA RID: 3322
	protected const string STAFF_CIRCLE_PROJECTILE = "SpellSwordStaffCircleBoltProjectile";

	// Token: 0x04000CFB RID: 3323
	protected const string STAFF_CIRCLEHOMING_PROJECTILE = "SpellSwordStaffCircleHomingBoltProjectile";

	// Token: 0x04000CFC RID: 3324
	protected float m_staff_AttackDuration = 2f;

	// Token: 0x04000CFD RID: 3325
	protected float m_staff_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000CFE RID: 3326
	protected float m_staff_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000CFF RID: 3327
	protected float m_staff_TellIntroAndHold_Delay = 0.95f;

	// Token: 0x04000D00 RID: 3328
	protected float m_staff_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000D01 RID: 3329
	protected float m_staff_AttackIntro_Delay;

	// Token: 0x04000D02 RID: 3330
	protected float m_staff_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000D03 RID: 3331
	protected float m_staff_AttackHold_Delay;

	// Token: 0x04000D04 RID: 3332
	protected float m_staff_AttackHold_ExitDelay = 0.75f;

	// Token: 0x04000D05 RID: 3333
	protected float m_staff_Exit_AnimSpeed = 1.2f;

	// Token: 0x04000D06 RID: 3334
	protected float m_staff_Exit_Delay;

	// Token: 0x04000D07 RID: 3335
	protected float m_staff_Exit_IdleDuration = 0.15f;

	// Token: 0x04000D08 RID: 3336
	protected float m_staff_AttackCD = 12f;

	// Token: 0x04000D09 RID: 3337
	private Projectile_RL m_staffShieldProjectile;

	// Token: 0x04000D0A RID: 3338
	protected int m_numStaffFireballs_Variant = 15;

	// Token: 0x04000D0B RID: 3339
	protected int m_staffFireballSpread = 720;

	// Token: 0x04000D0C RID: 3340
	protected const string SWORD1_TELL_INTRO = "Sword1_Tell_Intro";

	// Token: 0x04000D0D RID: 3341
	protected const string SWORD1_TELL_HOLD = "Sword1_Tell_Hold";

	// Token: 0x04000D0E RID: 3342
	protected const string SWORD1_ATTACK_INTRO = "Sword1_Attack_Intro";

	// Token: 0x04000D0F RID: 3343
	protected const string SWORD1_ATTACK_HOLD = "Sword1_Attack_Hold";

	// Token: 0x04000D10 RID: 3344
	protected const string SWORD2_TELL_INTRO = "Sword2_Tell_Intro";

	// Token: 0x04000D11 RID: 3345
	protected const string SWORD2_TELL_HOLD = "Sword2_Tell_Hold";

	// Token: 0x04000D12 RID: 3346
	protected const string SWORD2_ATTACK_INTRO = "Sword2_Attack_Intro";

	// Token: 0x04000D13 RID: 3347
	protected const string SWORD2_ATTACK_HOLD = "Sword2_Attack_Hold";

	// Token: 0x04000D14 RID: 3348
	protected const string SWORD2_EXIT = "Sword2_Exit";

	// Token: 0x04000D15 RID: 3349
	protected const string SWORD_FLAMEGOUT_PROJECTILE = "SpellSwordFlameGoutProjectile";

	// Token: 0x04000D16 RID: 3350
	protected const string SWORD1_PROJECTILE = "SpellSwordSlashDownProjectile";

	// Token: 0x04000D17 RID: 3351
	protected const string SWORD2_PROJECTILE = "SpellSwordSlashUpProjectile";

	// Token: 0x04000D18 RID: 3352
	protected float m_sword_AttackSpeed = 16f;

	// Token: 0x04000D19 RID: 3353
	protected float m_sword_AttackDuration = 0.185f;

	// Token: 0x04000D1A RID: 3354
	protected float m_sword_PostAttack_Delay = 0.1f;

	// Token: 0x04000D1B RID: 3355
	protected float m_sword2_PostAttack_Delay = 0.75f;

	// Token: 0x04000D1C RID: 3356
	protected float m_sword2_AttackSpeed = 15f;

	// Token: 0x04000D1D RID: 3357
	protected float m_sword2_AttackDuration = 0.175f;

	// Token: 0x04000D1E RID: 3358
	protected float m_sword_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000D1F RID: 3359
	protected float m_sword_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000D20 RID: 3360
	protected float m_sword_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04000D21 RID: 3361
	protected float m_sword_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000D22 RID: 3362
	protected float m_sword_AttackIntro_Delay;

	// Token: 0x04000D23 RID: 3363
	protected float m_sword_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000D24 RID: 3364
	protected float m_sword_AttackHold_Delay;

	// Token: 0x04000D25 RID: 3365
	protected float m_sword2_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000D26 RID: 3366
	protected float m_sword2_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000D27 RID: 3367
	protected float m_sword2_TellIntroAndHold_Delay = 0.35f;

	// Token: 0x04000D28 RID: 3368
	protected float m_sword2_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000D29 RID: 3369
	protected float m_sword2_AttackIntro_Delay;

	// Token: 0x04000D2A RID: 3370
	protected float m_sword2_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000D2B RID: 3371
	protected float m_sword2_AttackHold_Delay;

	// Token: 0x04000D2C RID: 3372
	protected float m_sword_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000D2D RID: 3373
	protected float m_sword_Exit_Delay;

	// Token: 0x04000D2E RID: 3374
	protected float m_sword_Exit_IdleDuration = 0.15f;

	// Token: 0x04000D2F RID: 3375
	protected float m_sword_AttackCD = 8f;

	// Token: 0x04000D30 RID: 3376
	private Projectile_RL m_swordFlameProjectile;

	// Token: 0x04000D31 RID: 3377
	private Projectile_RL m_swordFlameProjectile2;

	// Token: 0x04000D32 RID: 3378
	private Projectile_RL m_swordFlameProjectile3;

	// Token: 0x04000D33 RID: 3379
	private Projectile_RL m_swordFlameProjectile4;

	// Token: 0x04000D34 RID: 3380
	protected const string DAGGERTHROW_TELL_INTRO = "DaggerThrow_Tell_Intro";

	// Token: 0x04000D35 RID: 3381
	protected const string DAGGERTHROW_TELL_HOLD = "DaggerThrow_Tell_Hold";

	// Token: 0x04000D36 RID: 3382
	protected const string DAGGERTHROW_ATTACK_INTRO = "DaggerThrow_Attack_Intro";

	// Token: 0x04000D37 RID: 3383
	protected const string DAGGERTHROW_ATTACK_HOLD = "DaggerThrow_Attack_Hold";

	// Token: 0x04000D38 RID: 3384
	protected const string DAGGERTHROW_EXIT = "DaggerThrow_Exit";

	// Token: 0x04000D39 RID: 3385
	protected const string DAGGERTHROW_PROJECTILE = "SpellSwordDaggerBoltBlueProjectile";

	// Token: 0x04000D3A RID: 3386
	protected float m_daggerThrowPirhouetteDelay = 0.15f;

	// Token: 0x04000D3B RID: 3387
	protected int m_numDaggerThrowDaggers = 3;

	// Token: 0x04000D3C RID: 3388
	protected int m_numDaggerThrowDaggers_Variant = 3;

	// Token: 0x04000D3D RID: 3389
	protected float m_daggerThrow_AttackSpeed = 28f;

	// Token: 0x04000D3E RID: 3390
	protected float m_daggerThrow_AttackDuration = 0.25f;

	// Token: 0x04000D3F RID: 3391
	protected float m_daggerThrow_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000D40 RID: 3392
	protected float m_daggerThrow_TellHold_AnimSpeed = 1f;

	// Token: 0x04000D41 RID: 3393
	protected float m_daggerThrow_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04000D42 RID: 3394
	protected float m_daggerThrow_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000D43 RID: 3395
	protected float m_daggerThrow_AttackIntro_Delay;

	// Token: 0x04000D44 RID: 3396
	protected float m_daggerThrow_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000D45 RID: 3397
	protected float m_daggerThrow_AttackHold_Delay;

	// Token: 0x04000D46 RID: 3398
	protected float m_daggerThrow_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000D47 RID: 3399
	protected float m_daggerThrow_Exit_Delay;

	// Token: 0x04000D48 RID: 3400
	protected float m_daggerThrow_Exit_IdleDuration = 0.15f;

	// Token: 0x04000D49 RID: 3401
	protected float m_daggerThrow_AttackCD = 12f;

	// Token: 0x04000D4A RID: 3402
	protected const string AXE_TELL_INTRO = "AxeJump_Tell_Intro";

	// Token: 0x04000D4B RID: 3403
	protected const string AXE_TELL_HOLD = "AxeJump_Tell_Hold";

	// Token: 0x04000D4C RID: 3404
	protected const string AXE_ATTACK_INTRO = "AxeJump_Attack_Intro";

	// Token: 0x04000D4D RID: 3405
	protected const string AXE_ATTACK_HOLD = "AxeJump_Attack_Hold";

	// Token: 0x04000D4E RID: 3406
	protected const string AXE_EXIT_BLADE_OUT = "AxeJump_Land";

	// Token: 0x04000D4F RID: 3407
	protected const string AXE_EXIT_RETRACT = "AxeJump_Exit";

	// Token: 0x04000D50 RID: 3408
	protected const string AXE_SPIN_PROJECTILE = "SpellSwordAxeSpinProjectile";

	// Token: 0x04000D51 RID: 3409
	protected const string AXE_LAND_PROJECTILE = "SpellSwordGroundRubbleProjectile";

	// Token: 0x04000D52 RID: 3410
	protected const string AXE_LAND_ADVANCED_PROJECTILE = "SpellSwordVoidProjectile";

	// Token: 0x04000D53 RID: 3411
	private Projectile_RL m_axeSpinProjectile;

	// Token: 0x04000D54 RID: 3412
	protected Vector2 m_axeAttackVelocity = new Vector2(12f, 32f);

	// Token: 0x04000D55 RID: 3413
	protected Vector2 m_axeAttack_Land_ThrowAngle = new Vector2(85f, 85f);

	// Token: 0x04000D56 RID: 3414
	protected Vector2 m_axeAttack_Land_ThrowAngle2 = new Vector2(78f, 78f);

	// Token: 0x04000D57 RID: 3415
	protected Vector2 m_axeAttack_Land_ThrowAngle3 = new Vector2(71f, 71f);

	// Token: 0x04000D58 RID: 3416
	protected Vector2 m_axeAttack_Land_ThrowAngle4 = new Vector2(55f, 55f);

	// Token: 0x04000D59 RID: 3417
	protected Vector2 m_axeAttack_Land_ThrowAngle5 = new Vector2(82f, 82f);

	// Token: 0x04000D5A RID: 3418
	protected Vector2 m_axeAttack_Land_ThrowAngle6 = new Vector2(74f, 74f);

	// Token: 0x04000D5B RID: 3419
	protected Vector2 m_axeAttack_Land_ThrowPower = new Vector2(1f, 1f);

	// Token: 0x04000D5C RID: 3420
	protected float m_jump_TellIntro_AnimSpeed = 1.2f;

	// Token: 0x04000D5D RID: 3421
	protected float m_jump_TellHold_AnimSpeed = 1.2f;

	// Token: 0x04000D5E RID: 3422
	protected float m_jump_TellIntroAndHold_Delay = 0.65f;

	// Token: 0x04000D5F RID: 3423
	protected float m_axeAttack_TellIntro_AnimSpeed = 1.5f;

	// Token: 0x04000D60 RID: 3424
	protected float m_axeAttack_TellHold_AnimSpeed = 1.5f;

	// Token: 0x04000D61 RID: 3425
	protected float m_axeAttack_TellIntroAndHold_Delay;

	// Token: 0x04000D62 RID: 3426
	protected float m_axeAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000D63 RID: 3427
	protected float m_axeAttack_AttackIntro_Delay;

	// Token: 0x04000D64 RID: 3428
	protected float m_axeAttack_AttackHold_AnimSpeed = 1.5f;

	// Token: 0x04000D65 RID: 3429
	protected float m_axeAttack_AttackHold_Delay;

	// Token: 0x04000D66 RID: 3430
	protected float m_axeAttack_ExitHold_Delay = 0.375f;

	// Token: 0x04000D67 RID: 3431
	protected float m_axeAttack_ExitHold_AnimSpeed = 1.2f;

	// Token: 0x04000D68 RID: 3432
	protected float m_axeAttack_Exit_AnimSpeed = 1.2f;

	// Token: 0x04000D69 RID: 3433
	protected float m_axeAttack_Exit_Delay;

	// Token: 0x04000D6A RID: 3434
	protected float m_axeAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x04000D6B RID: 3435
	protected float m_axeAttack_AttackCD = 5f;

	// Token: 0x04000D6C RID: 3436
	protected const string STAFFTHROW_TELL_INTRO = "StaffForward_Tell_Intro";

	// Token: 0x04000D6D RID: 3437
	protected const string STAFFTHROW_TELL_HOLD = "StaffForward_Tell_Hold";

	// Token: 0x04000D6E RID: 3438
	protected const string STAFFTHROW_ATTACK_INTRO = "StaffForward_Attack_Intro";

	// Token: 0x04000D6F RID: 3439
	protected const string STAFFTHROW_ATTACK_HOLD = "StaffForward_Attack_Hold";

	// Token: 0x04000D70 RID: 3440
	protected const string STAFFTHROW_EXIT = "StaffForward_Attack_Exit";

	// Token: 0x04000D71 RID: 3441
	protected const string STAFFTHROW_PROJECTILE = "SpellSwordStaffForwardBoltProjectile";

	// Token: 0x04000D72 RID: 3442
	protected const string STAFFTHROW_BEAM_PROJECTILE = "SpellSwordStaffForwardBeamProjectile";

	// Token: 0x04000D73 RID: 3443
	protected const string STAFFTHROW_BEAM_WARNING_PROJECTILE = "SpellSwordStaffWarningForwardBeamProjectile";

	// Token: 0x04000D74 RID: 3444
	protected const string STAFFTHROW_BEAM_ADVANCED_PROJECTILE = "SpellSwordStaffForwardBeamAdvancedProjectile";

	// Token: 0x04000D75 RID: 3445
	protected const string STAFFTHROW_BEAM_WARNING_ADVANCED_PROJECTILE = "SpellSwordStaffWarningForwardBeamAdvancedProjectile";

	// Token: 0x04000D76 RID: 3446
	protected float m_staffThrow_BeamAttack_Duration = 0.75f;

	// Token: 0x04000D77 RID: 3447
	protected float m_staffThrow_BeamAttack_Variant_Duration = 1f;

	// Token: 0x04000D78 RID: 3448
	protected float m_staffThrow_AttackSpeed = 28f;

	// Token: 0x04000D79 RID: 3449
	protected float m_staffThrow_AttackDuration_EndDelay = 1f;

	// Token: 0x04000D7A RID: 3450
	protected float m_staffThrow_TellIntro_AnimSpeed = 1.5f;

	// Token: 0x04000D7B RID: 3451
	protected float m_staffThrow_TellHold_AnimSpeed = 1f;

	// Token: 0x04000D7C RID: 3452
	protected float m_staffThrow_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04000D7D RID: 3453
	protected float m_staffThrow_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000D7E RID: 3454
	protected float m_staffThrow_AttackIntro_Delay;

	// Token: 0x04000D7F RID: 3455
	protected float m_staffThrow_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000D80 RID: 3456
	protected float m_staffThrow_AttackHold_Delay;

	// Token: 0x04000D81 RID: 3457
	protected float m_staffThrow_Exit_AnimSpeed = 1f;

	// Token: 0x04000D82 RID: 3458
	protected float m_staffThrow_Exit_Delay;

	// Token: 0x04000D83 RID: 3459
	protected float m_staffThrow_Exit_IdleDuration = 0.15f;

	// Token: 0x04000D84 RID: 3460
	protected float m_staffThrow_AttackCD = 12f;

	// Token: 0x04000D85 RID: 3461
	private Projectile_RL m_staffBeamProjectile;

	// Token: 0x04000D86 RID: 3462
	private Projectile_RL m_staffBeamWarningProjectile;

	// Token: 0x04000D87 RID: 3463
	protected const string MODESHIFT_DOWNED = "ModeShift_Intro";

	// Token: 0x04000D88 RID: 3464
	protected const string MODESHIFT_GETUP = "ModeShift_GetUp";

	// Token: 0x04000D89 RID: 3465
	protected const string MODESHIFT_ATTACK_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000D8A RID: 3466
	protected const string MODESHIFT_ATTACK_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x04000D8B RID: 3467
	protected const string MODESHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000D8C RID: 3468
	protected const string MODESHIFT_PROJECTILE_WARNING = "SpellSwordShoutWarningProjectile";

	// Token: 0x04000D8D RID: 3469
	protected const string MODESHIFT_PROJECTILE_ATTACK = "SpellSwordShoutAttackProjectile";

	// Token: 0x04000D8E RID: 3470
	protected const string MODESHIFT_PROJECTILE_HOMING = "SpellSwordShoutHomingBoltProjectile";

	// Token: 0x04000D8F RID: 3471
	private Projectile_RL m_modeShiftProjectile;

	// Token: 0x04000D90 RID: 3472
	private Projectile_RL m_modeShiftProjectile2;

	// Token: 0x04000D91 RID: 3473
	protected float m_modeShift_AttackSpeed = 28f;

	// Token: 0x04000D92 RID: 3474
	protected float m_modeShift_AttackDuration_Initial_Delay = 0.5f;

	// Token: 0x04000D93 RID: 3475
	protected float m_modeShift_AttackDuration_Fire_Duration = 0.05f;

	// Token: 0x04000D94 RID: 3476
	protected float m_modeShift_AttackDuration_Exit_Delay = 0.5f;

	// Token: 0x04000D95 RID: 3477
	protected float m_modeShift_AttackDuration_SecondShot_Delay = 1.5f;

	// Token: 0x04000D96 RID: 3478
	protected float m_modeShift_ProjectilesThrown_Variant = 5f;

	// Token: 0x04000D97 RID: 3479
	protected float m_modeShift_Damage_Mod = 0.1f;

	// Token: 0x04000D98 RID: 3480
	protected Vector2 m_modeShift_Shout_ThrowAngle = new Vector2(-40f, 40f);

	// Token: 0x04000D99 RID: 3481
	protected Vector2 m_modeShift_Shout_ThrowPower = new Vector2(0.525f, 0.625f);

	// Token: 0x04000D9A RID: 3482
	protected float m_modeShift_Downed_AnimSpeed = 1f;

	// Token: 0x04000D9B RID: 3483
	protected float m_modeShift_Downed_Delay = 1.3f;

	// Token: 0x04000D9C RID: 3484
	protected float m_modeShift_GetUp_AnimSpeed = 1f;

	// Token: 0x04000D9D RID: 3485
	protected float m_modeShift_GetUp_Delay = 1f;

	// Token: 0x04000D9E RID: 3486
	protected float m_modeShift_AttackIntro_AnimSpeed = 1.25f;

	// Token: 0x04000D9F RID: 3487
	protected float m_modeShift_AttackIntro_Delay;

	// Token: 0x04000DA0 RID: 3488
	protected float m_modeShift_AttackHold_AnimSpeed = 1.25f;

	// Token: 0x04000DA1 RID: 3489
	protected float m_modeShift_AttackHold_Delay;

	// Token: 0x04000DA2 RID: 3490
	protected float m_modeShift_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000DA3 RID: 3491
	protected float m_modeShift_Exit_Delay;

	// Token: 0x04000DA4 RID: 3492
	protected float m_modeShift_Exit_IdleDuration = 0.15f;

	// Token: 0x04000DA5 RID: 3493
	protected float m_modeShift_AttackCD = 99f;

	// Token: 0x04000DA6 RID: 3494
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x04000DA7 RID: 3495
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x04000DA8 RID: 3496
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04000DA9 RID: 3497
	protected float m_death_Intro_Delay;

	// Token: 0x04000DAA RID: 3498
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04000DAB RID: 3499
	protected float m_death_Hold_Delay = 4.5f;

	// Token: 0x04000DAC RID: 3500
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x04000DAD RID: 3501
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x04000DAE RID: 3502
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04000DAF RID: 3503
	protected float m_spawn_Idle_Delay = 2f;

	// Token: 0x04000DB0 RID: 3504
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04000DB1 RID: 3505
	protected float m_spawn_Intro_Delay;

	// Token: 0x04000DB2 RID: 3506
	protected bool m_isAxeSpinning;

	// Token: 0x04000DB3 RID: 3507
	protected bool m_isStaffBeaming;
}
