using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class Wolf_Miniboss_AIScript : Wolf_Basic_AIScript
{
	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x06000B67 RID: 2919 RVA: 0x00022B03 File Offset: 0x00020D03
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.25f);
		}
	}

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x06000B68 RID: 2920 RVA: 0x00022B14 File Offset: 0x00020D14
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.6f, 1.15f);
		}
	}

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00022B25 File Offset: 0x00020D25
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.6f, 1.15f);
		}
	}

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00022B36 File Offset: 0x00020D36
	protected override Vector2 m_dash_AttackHold_ForwardPower
	{
		get
		{
			return new Vector2(28f, 18f);
		}
	}

	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x06000B6B RID: 2923 RVA: 0x00022B47 File Offset: 0x00020D47
	protected override Vector2 m_jumpPower
	{
		get
		{
			return new Vector2(20f, 32f);
		}
	}

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x06000B6C RID: 2924 RVA: 0x00022B58 File Offset: 0x00020D58
	protected override bool m_howl_Randomize_Howl
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x06000B6D RID: 2925 RVA: 0x00022B5B File Offset: 0x00020D5B
	protected override bool m_howl_Spawn_Projectile
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x06000B6E RID: 2926 RVA: 0x00022B5E File Offset: 0x00020D5E
	protected override bool m_howl_At_Start
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x00022B61 File Offset: 0x00020D61
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
		this.m_staffBeamWarningProjectile = this.FireProjectile("WolfWarningForwardBeamProjectile", 2, true, 0f, 1f, true, true, true);
		yield return this.Default_TellIntroAndLoop("Laser_Tell_Intro", this.m_staffThrow_TellIntro_AnimSpeed, "Laser_Tell_Hold", this.m_staffThrow_TellHold_AnimSpeed, this.m_staffThrow_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Laser_Attack_Intro", this.m_staffThrow_AttackIntro_AnimSpeed, this.m_staffThrow_AttackIntro_Delay, true);
		yield return this.Default_Animation("Laser_Attack_Hold", this.m_staffThrow_AttackHold_AnimSpeed, this.m_staffThrow_AttackHold_Delay, false);
		if (this.m_staffBeamWarningProjectile && this.m_staffBeamWarningProjectile.isActiveAndEnabled)
		{
			this.m_staffBeamWarningProjectile.FlagForDestruction(null);
			this.m_staffBeamWarningProjectile = null;
		}
		this.m_staffBeamProjectile = this.FireProjectile("WolfForwardBeamProjectile", 2, true, 0f, 1f, true, true, true);
		if (this.m_staffThrow_BeamAttack_Duration > 0f)
		{
			yield return base.Wait(this.m_staffThrow_BeamAttack_Duration, false);
		}
		if (this.m_staffBeamProjectile && this.m_staffBeamProjectile.isActiveAndEnabled)
		{
			this.m_staffBeamProjectile.FlagForDestruction(null);
			this.m_staffBeamProjectile = null;
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return base.Wait(this.m_staffThrow_AttackDuration_EndDelay, false);
		yield return this.Default_Animation("Laser_Attack_Exit", this.m_staffThrow_Exit_AnimSpeed, this.m_staffThrow_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_staffThrow_Exit_IdleDuration, this.m_staffThrow_AttackCD);
		yield break;
	}

	// Token: 0x04000FEA RID: 4074
	protected const string STAFFTHROW_TELL_INTRO = "Laser_Tell_Intro";

	// Token: 0x04000FEB RID: 4075
	protected const string STAFFTHROW_TELL_HOLD = "Laser_Tell_Hold";

	// Token: 0x04000FEC RID: 4076
	protected const string STAFFTHROW_ATTACK_INTRO = "Laser_Attack_Intro";

	// Token: 0x04000FED RID: 4077
	protected const string STAFFTHROW_ATTACK_HOLD = "Laser_Attack_Hold";

	// Token: 0x04000FEE RID: 4078
	protected const string STAFFTHROW_EXIT = "Laser_Attack_Exit";

	// Token: 0x04000FEF RID: 4079
	protected const string STAFFTHROW_PROJECTILE = "SpellSwordStaffForwardBoltProjectile";

	// Token: 0x04000FF0 RID: 4080
	protected const string STAFFTHROW_BEAM_PROJECTILE = "WolfForwardBeamProjectile";

	// Token: 0x04000FF1 RID: 4081
	protected const string STAFFTHROW_BEAM_WARNING_PROJECTILE = "WolfWarningForwardBeamProjectile";

	// Token: 0x04000FF2 RID: 4082
	protected const int STAFF_FRONT_POS_INDEX = 2;

	// Token: 0x04000FF3 RID: 4083
	protected float m_staffThrow_BeamAttack_Duration = 1.2f;

	// Token: 0x04000FF4 RID: 4084
	protected float m_staffThrow_BeamAttack_Variant_Duration = 1f;

	// Token: 0x04000FF5 RID: 4085
	protected float m_staffThrow_AttackSpeed = 28f;

	// Token: 0x04000FF6 RID: 4086
	protected float m_staffThrow_AttackDuration_EndDelay = 1f;

	// Token: 0x04000FF7 RID: 4087
	protected float m_staffThrow_TellIntro_AnimSpeed = 1.5f;

	// Token: 0x04000FF8 RID: 4088
	protected float m_staffThrow_TellHold_AnimSpeed = 1f;

	// Token: 0x04000FF9 RID: 4089
	protected float m_staffThrow_TellIntroAndHold_Delay = 1.1f;

	// Token: 0x04000FFA RID: 4090
	protected float m_staffThrow_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000FFB RID: 4091
	protected float m_staffThrow_AttackIntro_Delay;

	// Token: 0x04000FFC RID: 4092
	protected float m_staffThrow_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000FFD RID: 4093
	protected float m_staffThrow_AttackHold_Delay;

	// Token: 0x04000FFE RID: 4094
	protected float m_staffThrow_Exit_AnimSpeed = 1f;

	// Token: 0x04000FFF RID: 4095
	protected float m_staffThrow_Exit_Delay;

	// Token: 0x04001000 RID: 4096
	protected float m_staffThrow_Exit_IdleDuration = 0.15f;

	// Token: 0x04001001 RID: 4097
	protected float m_staffThrow_AttackCD = 12f;

	// Token: 0x04001002 RID: 4098
	private Projectile_RL m_staffBeamProjectile;

	// Token: 0x04001003 RID: 4099
	private Projectile_RL m_staffBeamWarningProjectile;
}
