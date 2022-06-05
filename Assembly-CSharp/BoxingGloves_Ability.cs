using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020002E8 RID: 744
public class BoxingGloves_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x060016A8 RID: 5800 RVA: 0x0000B59D File Offset: 0x0000979D
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_projectileName2,
			this.m_projectileName3,
			this.m_downAttackProjectileName,
			this.m_upAttackProjectileName
		};
	}

	// Token: 0x17000AA9 RID: 2729
	// (get) Token: 0x060016A9 RID: 5801 RVA: 0x0000B5D8 File Offset: 0x000097D8
	protected virtual int NumComboAttacks
	{
		get
		{
			return 999999;
		}
	}

	// Token: 0x17000AAA RID: 2730
	// (get) Token: 0x060016AA RID: 5802 RVA: 0x00004536 File Offset: 0x00002736
	protected float TellIntroAnimSpeedNormalAttack
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000AAB RID: 2731
	// (get) Token: 0x060016AB RID: 5803 RVA: 0x000052A9 File Offset: 0x000034A9
	protected float TellIntroAnimSpeedDownAttack
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000AAC RID: 2732
	// (get) Token: 0x060016AC RID: 5804 RVA: 0x000052A9 File Offset: 0x000034A9
	protected float TellIntroAnimSpeedUpAttack
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000AAD RID: 2733
	// (get) Token: 0x060016AD RID: 5805 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayNormalAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AAE RID: 2734
	// (get) Token: 0x060016AE RID: 5806 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayDownAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AAF RID: 2735
	// (get) Token: 0x060016AF RID: 5807 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AB0 RID: 2736
	// (get) Token: 0x060016B0 RID: 5808 RVA: 0x00004536 File Offset: 0x00002736
	protected float TellAnimSpeedNormalAttack
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000AB1 RID: 2737
	// (get) Token: 0x060016B1 RID: 5809 RVA: 0x0000B5DF File Offset: 0x000097DF
	protected float TellAnimSpeedDownAttack
	{
		get
		{
			return this.TellAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000AB2 RID: 2738
	// (get) Token: 0x060016B2 RID: 5810 RVA: 0x0000B5DF File Offset: 0x000097DF
	protected float TellAnimSpeedUpAttack
	{
		get
		{
			return this.TellAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000AB3 RID: 2739
	// (get) Token: 0x060016B3 RID: 5811 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellAnimExitDelayNormalAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AB4 RID: 2740
	// (get) Token: 0x060016B4 RID: 5812 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellAnimExitDelayDownAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AB5 RID: 2741
	// (get) Token: 0x060016B5 RID: 5813 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AB6 RID: 2742
	// (get) Token: 0x060016B6 RID: 5814 RVA: 0x00004536 File Offset: 0x00002736
	protected float AttackIntroAnimSpeedNormalAttack
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000AB7 RID: 2743
	// (get) Token: 0x060016B7 RID: 5815 RVA: 0x0000B5E7 File Offset: 0x000097E7
	protected float AttackIntroAnimSpeedDownAttack
	{
		get
		{
			return this.AttackIntroAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000AB8 RID: 2744
	// (get) Token: 0x060016B8 RID: 5816 RVA: 0x0000B5E7 File Offset: 0x000097E7
	protected float AttackIntroAnimSpeedUpAttack
	{
		get
		{
			return this.AttackIntroAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000AB9 RID: 2745
	// (get) Token: 0x060016B9 RID: 5817 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayNormalAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000ABA RID: 2746
	// (get) Token: 0x060016BA RID: 5818 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayDownAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000ABB RID: 2747
	// (get) Token: 0x060016BB RID: 5819 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000ABC RID: 2748
	// (get) Token: 0x060016BC RID: 5820 RVA: 0x00004536 File Offset: 0x00002736
	protected float AttackAnimSpeedNormalAttack
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000ABD RID: 2749
	// (get) Token: 0x060016BD RID: 5821 RVA: 0x0000B5EF File Offset: 0x000097EF
	protected float AttackAnimSpeedDownAttack
	{
		get
		{
			return this.AttackAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000ABE RID: 2750
	// (get) Token: 0x060016BE RID: 5822 RVA: 0x00004FFB File Offset: 0x000031FB
	protected float AttackAnimSpeedUpAttack
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000ABF RID: 2751
	// (get) Token: 0x060016BF RID: 5823 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackAnimExitDelayNormalAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AC0 RID: 2752
	// (get) Token: 0x060016C0 RID: 5824 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackAnimExitDelayDownAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AC1 RID: 2753
	// (get) Token: 0x060016C1 RID: 5825 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AC2 RID: 2754
	// (get) Token: 0x060016C2 RID: 5826 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected float ExitAnimSpeedNormalAttack
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000AC3 RID: 2755
	// (get) Token: 0x060016C3 RID: 5827 RVA: 0x0000B5F7 File Offset: 0x000097F7
	protected float ExitAnimSpeedDownAttack
	{
		get
		{
			return this.ExitAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000AC4 RID: 2756
	// (get) Token: 0x060016C4 RID: 5828 RVA: 0x00004C67 File Offset: 0x00002E67
	protected float ExitAnimSpeedUpAttack
	{
		get
		{
			return 1.2f;
		}
	}

	// Token: 0x17000AC5 RID: 2757
	// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00006772 File Offset: 0x00004972
	protected float ExitAnimExitDelayNormalAttack
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x17000AC6 RID: 2758
	// (get) Token: 0x060016C6 RID: 5830 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float ExitAnimExitDelayDownAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AC7 RID: 2759
	// (get) Token: 0x060016C7 RID: 5831 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float ExitAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AC8 RID: 2760
	// (get) Token: 0x060016C8 RID: 5832 RVA: 0x0000B5FF File Offset: 0x000097FF
	public override Vector2 ProjectileOffset
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackProjectileOffset;
			}
			if (this.PerformDownAttack)
			{
				return this.m_downAttackProjectileOffset;
			}
			return base.ProjectileOffset;
		}
	}

	// Token: 0x17000AC9 RID: 2761
	// (get) Token: 0x060016C9 RID: 5833 RVA: 0x0000B625 File Offset: 0x00009825
	protected override float TellIntroAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellIntroAnimSpeedUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.TellIntroAnimSpeedDownAttack;
			}
			return this.TellIntroAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000ACA RID: 2762
	// (get) Token: 0x060016CA RID: 5834 RVA: 0x0000B64B File Offset: 0x0000984B
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellIntroAnimExitDelayUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.TellIntroAnimExitDelayDownAttack;
			}
			return this.TellIntroAnimExitDelayNormalAttack;
		}
	}

	// Token: 0x17000ACB RID: 2763
	// (get) Token: 0x060016CB RID: 5835 RVA: 0x0000B671 File Offset: 0x00009871
	protected override float TellAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellAnimSpeedUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.TellAnimSpeedDownAttack;
			}
			return this.TellAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000ACC RID: 2764
	// (get) Token: 0x060016CC RID: 5836 RVA: 0x0000B697 File Offset: 0x00009897
	protected override float TellAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellAnimExitDelayUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.TellAnimExitDelayDownAttack;
			}
			return this.TellAnimExitDelayNormalAttack;
		}
	}

	// Token: 0x17000ACD RID: 2765
	// (get) Token: 0x060016CD RID: 5837 RVA: 0x0000B6BD File Offset: 0x000098BD
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackIntroAnimSpeedUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.AttackIntroAnimSpeedDownAttack;
			}
			return this.AttackIntroAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000ACE RID: 2766
	// (get) Token: 0x060016CE RID: 5838 RVA: 0x0000B6E3 File Offset: 0x000098E3
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackIntroAnimExitDelayUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.AttackIntroAnimExitDelayDownAttack;
			}
			return this.AttackIntroAnimExitDelayNormalAttack;
		}
	}

	// Token: 0x17000ACF RID: 2767
	// (get) Token: 0x060016CF RID: 5839 RVA: 0x0000B709 File Offset: 0x00009909
	protected override float AttackAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackAnimSpeedUpAttack;
			}
			if (this.IsGrounded)
			{
				return this.AttackAnimSpeedDownAttack;
			}
			return this.AttackAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000AD0 RID: 2768
	// (get) Token: 0x060016D0 RID: 5840 RVA: 0x0000B72F File Offset: 0x0000992F
	protected override float AttackAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackAnimExitDelayUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.AttackAnimExitDelayDownAttack;
			}
			return this.AttackAnimExitDelayNormalAttack;
		}
	}

	// Token: 0x17000AD1 RID: 2769
	// (get) Token: 0x060016D1 RID: 5841 RVA: 0x0000B755 File Offset: 0x00009955
	protected override float ExitAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.ExitAnimSpeedUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.ExitAnimSpeedDownAttack;
			}
			return this.ExitAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000AD2 RID: 2770
	// (get) Token: 0x060016D2 RID: 5842 RVA: 0x0000B77B File Offset: 0x0000997B
	protected override float ExitAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.ExitAnimExitDelayUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.ExitAnimExitDelayDownAttack;
			}
			return this.ExitAnimExitDelayNormalAttack;
		}
	}

	// Token: 0x17000AD3 RID: 2771
	// (get) Token: 0x060016D3 RID: 5843 RVA: 0x0008B654 File Offset: 0x00089854
	public override string ProjectileName
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackProjectileName;
			}
			if (this.PerformDownAttack)
			{
				return this.m_downAttackProjectileName;
			}
			switch (this.m_punchIndex)
			{
			case 1:
				return base.ProjectileName;
			case 2:
				return this.m_projectileName2;
			case 3:
				return this.m_projectileName3;
			default:
				return base.ProjectileName;
			}
		}
	}

	// Token: 0x17000AD4 RID: 2772
	// (get) Token: 0x060016D4 RID: 5844 RVA: 0x0000B7A1 File Offset: 0x000099A1
	public override AbilityData AbilityData
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackAbilityData;
			}
			if (this.PerformDownAttack)
			{
				return this.m_downAttackAbilityData;
			}
			return base.AbilityData;
		}
	}

	// Token: 0x17000AD5 RID: 2773
	// (get) Token: 0x060016D5 RID: 5845 RVA: 0x0000B7C7 File Offset: 0x000099C7
	public override string AbilityTellIntroName
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackTellIntroName;
			}
			if (this.PerformDownAttack)
			{
				return this.m_downAttackTellIntroName;
			}
			return base.AbilityTellIntroName.Replace('1', this.m_punchIndex.ToString()[0]);
		}
	}

	// Token: 0x17000AD6 RID: 2774
	// (get) Token: 0x060016D6 RID: 5846 RVA: 0x0000B805 File Offset: 0x00009A05
	protected bool PerformUpAttack
	{
		get
		{
			return this.m_isUpAttacking;
		}
	}

	// Token: 0x17000AD7 RID: 2775
	// (get) Token: 0x060016D7 RID: 5847 RVA: 0x0000B80D File Offset: 0x00009A0D
	protected bool PerformDownAttack
	{
		get
		{
			return this.m_isDownAttacking;
		}
	}

	// Token: 0x17000AD8 RID: 2776
	// (get) Token: 0x060016D8 RID: 5848 RVA: 0x0000B815 File Offset: 0x00009A15
	protected bool IsGrounded
	{
		get
		{
			return !this.m_abilityController || this.m_abilityController.PlayerController.IsGrounded;
		}
	}

	// Token: 0x17000AD9 RID: 2777
	// (get) Token: 0x060016D9 RID: 5849 RVA: 0x0000B839 File Offset: 0x00009A39
	public override float MovementMod
	{
		get
		{
			if (!this.m_abilityController.PlayerController.IsGrounded)
			{
				return 1f;
			}
			return base.MovementMod;
		}
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x0000B859 File Offset: 0x00009A59
	protected override void Awake()
	{
		base.Awake();
		this.m_continueComboEvent = new Action<Projectile_RL, GameObject>(this.ContinueComboEvent);
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x0008B6B8 File Offset: 0x000898B8
	public override void PreCastAbility()
	{
		this.m_isUpAttacking = false;
		this.m_isDownAttacking = false;
		if (Rewired_RL.Player.GetButton("MoveVertical"))
		{
			this.m_isUpAttacking = true;
		}
		this.m_punchIndex = 1;
		this.m_canAttackAgainCounter = 0f;
		this.m_comboNumber = 0;
		base.PreCastAbility();
	}

	// Token: 0x060016DC RID: 5852 RVA: 0x0000B873 File Offset: 0x00009A73
	protected override void OnEnterTellLogic()
	{
		this.m_hasAttacked = false;
		this.m_comboNumber++;
		base.OnEnterTellLogic();
	}

	// Token: 0x060016DD RID: 5853 RVA: 0x0000B890 File Offset: 0x00009A90
	protected override void OnEnterAttackLogic()
	{
		base.OnEnterAttackLogic();
		this.m_canAttackAgainCounter = 0f;
	}

	// Token: 0x060016DE RID: 5854 RVA: 0x0000B8A3 File Offset: 0x00009AA3
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.AddListener(this.m_continueComboEvent, false);
		}
	}

	// Token: 0x060016DF RID: 5855 RVA: 0x0008B70C File Offset: 0x0008990C
	private void ContinueComboEvent(Projectile_RL projectile, GameObject colliderObj)
	{
		if (projectile)
		{
			projectile.OnCollisionRelay.RemoveListener(this.m_continueComboEvent);
		}
		if (!this)
		{
			return;
		}
		if (this.m_abilityController.PlayerController.ConditionState == CharacterStates.CharacterConditions.Stunned)
		{
			return;
		}
		if (this.PerformDownAttack || this.PerformUpAttack)
		{
			return;
		}
		if (this.m_abilityController.PlayerController.CharacterDash.IsDashing)
		{
			this.m_abilityController.PlayerController.CharacterDash.StopDash();
		}
		this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
		this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(false);
		if (this.m_reenableGravityCoroutine != null)
		{
			base.StopCoroutine(this.m_reenableGravityCoroutine);
		}
		this.m_reenableGravityCoroutine = base.StartCoroutine(this.ReenableGravityCoroutine());
	}

	// Token: 0x060016E0 RID: 5856 RVA: 0x0000B8D0 File Offset: 0x00009AD0
	private IEnumerator ReenableGravityCoroutine()
	{
		float delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		yield break;
	}

	// Token: 0x060016E1 RID: 5857 RVA: 0x0008B80C File Offset: 0x00089A0C
	protected void LateUpdate()
	{
		if (!base.AbilityActive)
		{
			return;
		}
		if (base.CurrentAbilityAnimState >= AbilityAnimState.Attack && !this.m_isUpAttacking)
		{
			if (this.m_canAttackAgainCounter >= 0.085f)
			{
				if (this.m_comboNumber < this.NumComboAttacks && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
				{
					this.m_hasAttacked = true;
					base.CancelChangeAnimCoroutine();
					this.m_isUpAttacking = false;
					this.m_isDownAttacking = false;
					base.IsAnimationComplete = false;
					this.m_punchIndex++;
					if (this.m_punchIndex > 3)
					{
						this.m_punchIndex = 1;
					}
					if (Rewired_RL.Player.GetButton("MoveVertical"))
					{
						this.m_isUpAttacking = true;
						if (base.AbilityActive)
						{
							this.m_animator.Play(this.AbilityTellIntroName);
							return;
						}
						this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
						return;
					}
					else
					{
						if (base.AbilityActive)
						{
							this.m_animator.Play(this.AbilityTellIntroName);
							return;
						}
						this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
						return;
					}
				}
			}
			else
			{
				this.m_canAttackAgainCounter += Time.deltaTime;
			}
		}
	}

	// Token: 0x060016E2 RID: 5858 RVA: 0x0000B8DF File Offset: 0x00009ADF
	protected override void OnExitExitLogic()
	{
		if (this.m_hasAttacked)
		{
			return;
		}
		base.OnExitExitLogic();
	}

	// Token: 0x060016E3 RID: 5859 RVA: 0x0008B940 File Offset: 0x00089B40
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_continueComboEvent);
		}
		if (this.m_reenableGravityCoroutine != null)
		{
			base.StopCoroutine(this.m_reenableGravityCoroutine);
		}
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x040016D9 RID: 5849
	[SerializeField]
	protected string m_projectileName2;

	// Token: 0x040016DA RID: 5850
	[SerializeField]
	protected string m_projectileName3;

	// Token: 0x040016DB RID: 5851
	[Header("Downward Attack Values")]
	[SerializeField]
	protected string m_downAttackTellIntroName;

	// Token: 0x040016DC RID: 5852
	[SerializeField]
	protected AbilityData m_downAttackAbilityData;

	// Token: 0x040016DD RID: 5853
	[SerializeField]
	protected string m_downAttackProjectileName;

	// Token: 0x040016DE RID: 5854
	[SerializeField]
	protected Vector2 m_downAttackProjectileOffset;

	// Token: 0x040016DF RID: 5855
	[Header("Upward Attack Values")]
	[SerializeField]
	protected string m_upAttackTellIntroName;

	// Token: 0x040016E0 RID: 5856
	[SerializeField]
	protected AbilityData m_upAttackAbilityData;

	// Token: 0x040016E1 RID: 5857
	[SerializeField]
	protected string m_upAttackProjectileName;

	// Token: 0x040016E2 RID: 5858
	[SerializeField]
	protected Vector2 m_upAttackProjectileOffset;

	// Token: 0x040016E3 RID: 5859
	private bool m_isUpAttacking;

	// Token: 0x040016E4 RID: 5860
	private bool m_isDownAttacking;

	// Token: 0x040016E5 RID: 5861
	private int m_comboNumber = 1;

	// Token: 0x040016E6 RID: 5862
	private float m_canAttackAgainCounter;

	// Token: 0x040016E7 RID: 5863
	private Coroutine m_pushForwardCoroutine;

	// Token: 0x040016E8 RID: 5864
	private int m_punchIndex;

	// Token: 0x040016E9 RID: 5865
	private Coroutine m_reenableGravityCoroutine;

	// Token: 0x040016EA RID: 5866
	private bool m_hasAttacked;

	// Token: 0x040016EB RID: 5867
	private Action<Projectile_RL, GameObject> m_continueComboEvent;

	// Token: 0x040016EC RID: 5868
	private const float CanAttackAgainDelay = 0.085f;

	// Token: 0x040016ED RID: 5869
	private const float GRAVITY_DISABLE_DURATION = 0.25f;
}
