using System;
using FMODUnity;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020002F4 RID: 756
public class Katana_Ability : Sword_Ability
{
	// Token: 0x0600176B RID: 5995 RVA: 0x0000BD5E File Offset: 0x00009F5E
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_airborneProjectileName
		};
	}

	// Token: 0x17000B2F RID: 2863
	// (get) Token: 0x0600176C RID: 5996 RVA: 0x00003DE8 File Offset: 0x00001FE8
	protected float TellIntroAnimSpeedGround
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x0600176D RID: 5997 RVA: 0x00003DE8 File Offset: 0x00001FE8
	protected float TellIntroAnimSpeedAir
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x17000B31 RID: 2865
	// (get) Token: 0x0600176E RID: 5998 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B32 RID: 2866
	// (get) Token: 0x0600176F RID: 5999 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B33 RID: 2867
	// (get) Token: 0x06001770 RID: 6000 RVA: 0x0000BD7E File Offset: 0x00009F7E
	protected float TellHoldAnimSpeedGround
	{
		get
		{
			return 2.35f;
		}
	}

	// Token: 0x17000B34 RID: 2868
	// (get) Token: 0x06001771 RID: 6001 RVA: 0x0000BD7E File Offset: 0x00009F7E
	protected float TellHoldAnimSpeedAir
	{
		get
		{
			return 2.35f;
		}
	}

	// Token: 0x17000B35 RID: 2869
	// (get) Token: 0x06001772 RID: 6002 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B36 RID: 2870
	// (get) Token: 0x06001773 RID: 6003 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B37 RID: 2871
	// (get) Token: 0x06001774 RID: 6004 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected float AttackIntroAnimSpeedGround
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B38 RID: 2872
	// (get) Token: 0x06001775 RID: 6005 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected float AttackIntroAnimSpeedAir
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B39 RID: 2873
	// (get) Token: 0x06001776 RID: 6006 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B3A RID: 2874
	// (get) Token: 0x06001777 RID: 6007 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B3B RID: 2875
	// (get) Token: 0x06001778 RID: 6008 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected float AttackHoldAnimSpeedGround
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B3C RID: 2876
	// (get) Token: 0x06001779 RID: 6009 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected float AttackHoldAnimSpeedAir
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B3D RID: 2877
	// (get) Token: 0x0600177A RID: 6010 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected float AttackHoldAnimDelayGround
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000B3E RID: 2878
	// (get) Token: 0x0600177B RID: 6011 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected float AttackHoldAnimDelayAir
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000B3F RID: 2879
	// (get) Token: 0x0600177C RID: 6012 RVA: 0x0000BD85 File Offset: 0x00009F85
	protected float ExitIntroAnimSpeedGround
	{
		get
		{
			return 2.2f;
		}
	}

	// Token: 0x17000B40 RID: 2880
	// (get) Token: 0x0600177D RID: 6013 RVA: 0x00003DE8 File Offset: 0x00001FE8
	protected float ExitIntroAnimSpeedAir
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x17000B41 RID: 2881
	// (get) Token: 0x0600177E RID: 6014 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float ExitHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B42 RID: 2882
	// (get) Token: 0x0600177F RID: 6015 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float ExitHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B43 RID: 2883
	// (get) Token: 0x06001780 RID: 6016 RVA: 0x0000BD8C File Offset: 0x00009F8C
	public override Vector2 ProjectileOffset
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.ProjectileOffset;
			}
			return this.m_airBorneProjectileOffset;
		}
	}

	// Token: 0x17000B44 RID: 2884
	// (get) Token: 0x06001781 RID: 6017 RVA: 0x0000BDA3 File Offset: 0x00009FA3
	protected override float TellIntroAnimSpeed
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.TellIntroAnimSpeedGround;
			}
			return this.TellIntroAnimSpeedAir;
		}
	}

	// Token: 0x17000B45 RID: 2885
	// (get) Token: 0x06001782 RID: 6018 RVA: 0x0000BDBA File Offset: 0x00009FBA
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.TellIntroAnimExitDelayGround;
			}
			return this.TellIntroAnimExitDelayAir;
		}
	}

	// Token: 0x17000B46 RID: 2886
	// (get) Token: 0x06001783 RID: 6019 RVA: 0x0000BDD1 File Offset: 0x00009FD1
	protected override float TellAnimSpeed
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.TellHoldAnimSpeedGround;
			}
			return this.TellHoldAnimSpeedAir;
		}
	}

	// Token: 0x17000B47 RID: 2887
	// (get) Token: 0x06001784 RID: 6020 RVA: 0x0000BDE8 File Offset: 0x00009FE8
	protected override float TellAnimExitDelay
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.TellHoldAnimDelayGround;
			}
			return this.TellHoldAnimDelayAir;
		}
	}

	// Token: 0x17000B48 RID: 2888
	// (get) Token: 0x06001785 RID: 6021 RVA: 0x0000BDFF File Offset: 0x00009FFF
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.AttackIntroAnimSpeedGround;
			}
			return this.AttackIntroAnimSpeedAir;
		}
	}

	// Token: 0x17000B49 RID: 2889
	// (get) Token: 0x06001786 RID: 6022 RVA: 0x0000BE16 File Offset: 0x0000A016
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.AttackIntroAnimExitDelayGround;
			}
			return this.AttackIntroAnimExitDelayAir;
		}
	}

	// Token: 0x17000B4A RID: 2890
	// (get) Token: 0x06001787 RID: 6023 RVA: 0x0000BE2D File Offset: 0x0000A02D
	protected override float AttackAnimSpeed
	{
		get
		{
			if (this.IsGrounded)
			{
				return this.AttackHoldAnimSpeedGround;
			}
			return this.AttackHoldAnimSpeedAir;
		}
	}

	// Token: 0x17000B4B RID: 2891
	// (get) Token: 0x06001788 RID: 6024 RVA: 0x0000BE44 File Offset: 0x0000A044
	protected override float AttackAnimExitDelay
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.AttackHoldAnimDelayGround;
			}
			return this.AttackHoldAnimDelayAir;
		}
	}

	// Token: 0x17000B4C RID: 2892
	// (get) Token: 0x06001789 RID: 6025 RVA: 0x0000BE5B File Offset: 0x0000A05B
	protected override float ExitAnimSpeed
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.ExitIntroAnimSpeedGround;
			}
			return this.ExitIntroAnimSpeedAir;
		}
	}

	// Token: 0x17000B4D RID: 2893
	// (get) Token: 0x0600178A RID: 6026 RVA: 0x0000BE72 File Offset: 0x0000A072
	protected override float ExitAnimExitDelay
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.ExitHoldAnimDelayGround;
			}
			return this.ExitHoldAnimDelayAir;
		}
	}

	// Token: 0x17000B4E RID: 2894
	// (get) Token: 0x0600178B RID: 6027 RVA: 0x0000BE89 File Offset: 0x0000A089
	public override bool HasAttackFlipCheck
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.HasAttackFlipCheck;
			}
			return this.m_hasAirborneAttackFlipCheck;
		}
	}

	// Token: 0x17000B4F RID: 2895
	// (get) Token: 0x0600178C RID: 6028 RVA: 0x0000BEA0 File Offset: 0x0000A0A0
	public override string ProjectileName
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.ProjectileName;
			}
			return this.m_airborneProjectileName;
		}
	}

	// Token: 0x17000B50 RID: 2896
	// (get) Token: 0x0600178D RID: 6029 RVA: 0x0000BEB7 File Offset: 0x0000A0B7
	public override AbilityData AbilityData
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.AbilityData;
			}
			return this.m_airborneAbilityData;
		}
	}

	// Token: 0x17000B51 RID: 2897
	// (get) Token: 0x0600178E RID: 6030 RVA: 0x0000BECE File Offset: 0x0000A0CE
	public override string AbilityTellIntroName
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.AbilityTellIntroName;
			}
			return this.m_abilityAirTellIntroName;
		}
	}

	// Token: 0x17000B52 RID: 2898
	// (get) Token: 0x0600178F RID: 6031 RVA: 0x0000BEE5 File Offset: 0x0000A0E5
	protected bool PerformGroundAttack
	{
		get
		{
			return (this.IsGrounded && !this.m_isAirAttacking) || this.m_isGroundAttacking;
		}
	}

	// Token: 0x17000B53 RID: 2899
	// (get) Token: 0x06001790 RID: 6032 RVA: 0x0000B815 File Offset: 0x00009A15
	protected bool IsGrounded
	{
		get
		{
			return !this.m_abilityController || this.m_abilityController.PlayerController.IsGrounded;
		}
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x0008C118 File Offset: 0x0008A318
	public override void PreCastAbility()
	{
		this.m_isGroundAttacking = false;
		this.m_isAirAttacking = false;
		if (this.IsGrounded)
		{
			this.m_isGroundAttacking = true;
		}
		else
		{
			this.m_isAirAttacking = true;
		}
		if (Rewired_RL.Player.GetButton("MoveVertical"))
		{
			this.m_attackDirection = Katana_Ability.KatanaAttackDirection.Up;
		}
		else if (Rewired_RL.Player.GetNegativeButton("MoveVertical"))
		{
			this.m_attackDirection = Katana_Ability.KatanaAttackDirection.Down;
		}
		else
		{
			this.m_attackDirection = Katana_Ability.KatanaAttackDirection.None;
		}
		this.m_animator.SetFloat("Attack_Direction", (float)this.m_attackDirection);
		this.m_prepEventEmitter.Play();
		base.PreCastAbility();
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x0008C1B0 File Offset: 0x0008A3B0
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.m_isGroundAttacking || !this.m_isGroundAttacking)
		{
			if (this.m_attackDirection == Katana_Ability.KatanaAttackDirection.Up)
			{
				Vector3 localEulerAngles = this.m_firedProjectile.transform.localEulerAngles;
				localEulerAngles.z = 35f;
				this.m_firedProjectile.transform.localEulerAngles = localEulerAngles;
				return;
			}
			if (this.m_attackDirection == Katana_Ability.KatanaAttackDirection.Down)
			{
				Vector3 localEulerAngles2 = this.m_firedProjectile.transform.localEulerAngles;
				localEulerAngles2.z = -35f;
				this.m_firedProjectile.transform.localEulerAngles = localEulerAngles2;
			}
		}
	}

	// Token: 0x06001793 RID: 6035 RVA: 0x0000BEFF File Offset: 0x0000A0FF
	protected override void OnEnterExitLogic()
	{
		this.m_retractBladeEventEmitter.Play();
		base.OnEnterExitLogic();
	}

	// Token: 0x06001794 RID: 6036 RVA: 0x0008C244 File Offset: 0x0008A444
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_isAirAttacking = false;
		this.m_isGroundAttacking = false;
		this.m_abilityController.PlayerController.DisableFriction = false;
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.gameObject.SetActive(false);
			this.m_firedProjectile = null;
		}
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x04001728 RID: 5928
	[Header("Airborne Values")]
	[SerializeField]
	protected string m_abilityAirTellIntroName;

	// Token: 0x04001729 RID: 5929
	[SerializeField]
	protected AbilityData m_airborneAbilityData;

	// Token: 0x0400172A RID: 5930
	[SerializeField]
	protected string m_airborneProjectileName;

	// Token: 0x0400172B RID: 5931
	[SerializeField]
	protected Vector2 m_airBorneProjectileOffset;

	// Token: 0x0400172C RID: 5932
	[SerializeField]
	protected bool m_hasAirborneAttackFlipCheck;

	// Token: 0x0400172D RID: 5933
	[Header("Audio Event Emitters")]
	[SerializeField]
	private StudioEventEmitter m_prepEventEmitter;

	// Token: 0x0400172E RID: 5934
	[SerializeField]
	private StudioEventEmitter m_retractBladeEventEmitter;

	// Token: 0x0400172F RID: 5935
	private bool m_isAirAttacking;

	// Token: 0x04001730 RID: 5936
	private bool m_isGroundAttacking;

	// Token: 0x04001731 RID: 5937
	private float TellIntroAnim = 1f;

	// Token: 0x04001732 RID: 5938
	private float TellHoldAnim = 2f;

	// Token: 0x04001733 RID: 5939
	private float AttackIntroAnim = 1f;

	// Token: 0x04001734 RID: 5940
	private float AttackHoldAnim = 1f;

	// Token: 0x04001735 RID: 5941
	private float ExitIntroAnim = 1f;

	// Token: 0x04001736 RID: 5942
	private float Attack_Ground_HoldDelay = 0.1f;

	// Token: 0x04001737 RID: 5943
	private float Attack_Air_HoldDelay = 0.1f;

	// Token: 0x04001738 RID: 5944
	private Katana_Ability.KatanaAttackDirection m_attackDirection;

	// Token: 0x020002F5 RID: 757
	private enum KatanaAttackDirection
	{
		// Token: 0x0400173A RID: 5946
		Up = 1,
		// Token: 0x0400173B RID: 5947
		None,
		// Token: 0x0400173C RID: 5948
		Down
	}
}
