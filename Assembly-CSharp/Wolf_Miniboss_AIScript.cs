using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200026B RID: 619
public class Wolf_Miniboss_AIScript : Wolf_Basic_AIScript
{
	// Token: 0x1700086A RID: 2154
	// (get) Token: 0x060011CC RID: 4556 RVA: 0x0000932F File Offset: 0x0000752F
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.25f);
		}
	}

	// Token: 0x1700086B RID: 2155
	// (get) Token: 0x060011CD RID: 4557 RVA: 0x00009340 File Offset: 0x00007540
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.6f, 1.15f);
		}
	}

	// Token: 0x1700086C RID: 2156
	// (get) Token: 0x060011CE RID: 4558 RVA: 0x00009340 File Offset: 0x00007540
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.6f, 1.15f);
		}
	}

	// Token: 0x1700086D RID: 2157
	// (get) Token: 0x060011CF RID: 4559 RVA: 0x00009351 File Offset: 0x00007551
	protected override Vector2 m_dash_AttackHold_ForwardPower
	{
		get
		{
			return new Vector2(28f, 18f);
		}
	}

	// Token: 0x1700086E RID: 2158
	// (get) Token: 0x060011D0 RID: 4560 RVA: 0x00009362 File Offset: 0x00007562
	protected override Vector2 m_jumpPower
	{
		get
		{
			return new Vector2(20f, 32f);
		}
	}

	// Token: 0x1700086F RID: 2159
	// (get) Token: 0x060011D1 RID: 4561 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool m_howl_Randomize_Howl
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000870 RID: 2160
	// (get) Token: 0x060011D2 RID: 4562 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_howl_Spawn_Projectile
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000871 RID: 2161
	// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool m_howl_At_Start
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x00009373 File Offset: 0x00007573
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

	// Token: 0x040014A3 RID: 5283
	protected const string STAFFTHROW_TELL_INTRO = "Laser_Tell_Intro";

	// Token: 0x040014A4 RID: 5284
	protected const string STAFFTHROW_TELL_HOLD = "Laser_Tell_Hold";

	// Token: 0x040014A5 RID: 5285
	protected const string STAFFTHROW_ATTACK_INTRO = "Laser_Attack_Intro";

	// Token: 0x040014A6 RID: 5286
	protected const string STAFFTHROW_ATTACK_HOLD = "Laser_Attack_Hold";

	// Token: 0x040014A7 RID: 5287
	protected const string STAFFTHROW_EXIT = "Laser_Attack_Exit";

	// Token: 0x040014A8 RID: 5288
	protected const string STAFFTHROW_PROJECTILE = "SpellSwordStaffForwardBoltProjectile";

	// Token: 0x040014A9 RID: 5289
	protected const string STAFFTHROW_BEAM_PROJECTILE = "WolfForwardBeamProjectile";

	// Token: 0x040014AA RID: 5290
	protected const string STAFFTHROW_BEAM_WARNING_PROJECTILE = "WolfWarningForwardBeamProjectile";

	// Token: 0x040014AB RID: 5291
	protected const int STAFF_FRONT_POS_INDEX = 2;

	// Token: 0x040014AC RID: 5292
	protected float m_staffThrow_BeamAttack_Duration = 1.2f;

	// Token: 0x040014AD RID: 5293
	protected float m_staffThrow_BeamAttack_Variant_Duration = 1f;

	// Token: 0x040014AE RID: 5294
	protected float m_staffThrow_AttackSpeed = 28f;

	// Token: 0x040014AF RID: 5295
	protected float m_staffThrow_AttackDuration_EndDelay = 1f;

	// Token: 0x040014B0 RID: 5296
	protected float m_staffThrow_TellIntro_AnimSpeed = 1.5f;

	// Token: 0x040014B1 RID: 5297
	protected float m_staffThrow_TellHold_AnimSpeed = 1f;

	// Token: 0x040014B2 RID: 5298
	protected float m_staffThrow_TellIntroAndHold_Delay = 1.1f;

	// Token: 0x040014B3 RID: 5299
	protected float m_staffThrow_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x040014B4 RID: 5300
	protected float m_staffThrow_AttackIntro_Delay;

	// Token: 0x040014B5 RID: 5301
	protected float m_staffThrow_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x040014B6 RID: 5302
	protected float m_staffThrow_AttackHold_Delay;

	// Token: 0x040014B7 RID: 5303
	protected float m_staffThrow_Exit_AnimSpeed = 1f;

	// Token: 0x040014B8 RID: 5304
	protected float m_staffThrow_Exit_Delay;

	// Token: 0x040014B9 RID: 5305
	protected float m_staffThrow_Exit_IdleDuration = 0.15f;

	// Token: 0x040014BA RID: 5306
	protected float m_staffThrow_AttackCD = 12f;

	// Token: 0x040014BB RID: 5307
	private Projectile_RL m_staffBeamProjectile;

	// Token: 0x040014BC RID: 5308
	private Projectile_RL m_staffBeamWarningProjectile;
}
