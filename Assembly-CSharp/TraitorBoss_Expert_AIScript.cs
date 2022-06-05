using System;
using System.Collections;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class TraitorBoss_Expert_AIScript : TraitorBoss_Basic_AIScript
{
	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x06000AEB RID: 2795 RVA: 0x00022021 File Offset: 0x00020221
	protected override int m_magma_ProjectilesFired
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x06000AEC RID: 2796 RVA: 0x00022024 File Offset: 0x00020224
	protected override float m_magma_ProjectileDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0002202B File Offset: 0x0002022B
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator CustomMagmaCombo()
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		yield return base.SetWeaponGeo(this.m_weaponGeoController.Staff, this.DEFAULT_WEAPON_SWAP_DELAY);
		CDGHelper.RandomPlusMinus();
		yield return base.Single_Jump(true, false, false);
		float seconds = UnityEngine.Random.Range(this.m_magma_JumpDelay.x, this.m_magma_JumpDelay.y);
		yield return base.Wait(seconds, false);
		yield return this.Default_Animation("SpellCast_Attack_Intro", base.m_magma_AttackIntro_AnimationSpeed, base.m_magma_AttackIntro_Delay, true);
		yield return this.Default_Animation("SpellCast_Attack_Hold", base.m_magma_AttackHold_AnimationSpeed, base.m_magma_AttackHold_Delay, false);
		int num2;
		for (int i = 0; i < this.m_magma_ProjectilesFired; i = num2 + 1)
		{
			string projectileName = "TraitorMagmaProjectile";
			int spawnPosIndex = 0;
			bool matchFacing = true;
			float num = (float)90;
			int magma_angleAdder = this.m_magma_angleAdder;
			this.FireProjectile(projectileName, spawnPosIndex, matchFacing, num + (float)0, 1f, true, true, true);
			this.FireProjectile("TraitorMagmaProjectile", 0, true, (float)(90 + this.m_magma_angleAdder), 1f, true, true, true);
			this.FireProjectile("TraitorMagmaProjectile", 0, true, (float)(90 + this.m_magma_angleAdder * -1), 1f, true, true, true);
			this.FireProjectile("TraitorMagmaProjectile", 0, true, (float)(90 + this.m_magma_angleAdder * 2), 1f, true, true, true);
			this.FireProjectile("TraitorMagmaProjectile", 0, true, (float)(90 + this.m_magma_angleAdder * -2), 1f, true, true, true);
			if (this.m_magma_ProjectileDelay != 0f)
			{
				yield return base.Wait(this.m_magma_ProjectileDelay, false);
			}
			num2 = i;
		}
		yield return this.Default_Animation("SpellCast_Exit", base.m_magma_Exit_AnimationSpeed, base.m_magma_Exit_Delay, true);
		base.Animator.SetTrigger("Change_Ability_Anim");
		base.EnemyController.LockFlip = false;
		yield return base.Single_WaitUntilGrounded();
		base.HideAllWeaponGeos();
		yield return this.Default_Attack_Cooldown(this.m_magma_Exit_ForceIdle, this.m_magma_Exit_AttackCD);
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x06000AEE RID: 2798 RVA: 0x0002203A File Offset: 0x0002023A
	protected virtual int m_shout_fireballsAtEnd
	{
		get
		{
			return 18;
		}
	}

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0002203E File Offset: 0x0002023E
	public IRelayLink ShoutAttackWarningAppearedRelay
	{
		get
		{
			return this.m_shoutAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x0002204B File Offset: 0x0002024B
	public IRelayLink ShoutAttackExplodedRelay
	{
		get
		{
			return this.m_shoutAttackExplodedRelay.link;
		}
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x00022058 File Offset: 0x00020258
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShoutAttack()
	{
		this.ToDo("SHOUT ATTACK");
		if (this.m_shout_stopMovingWhileAttacking)
		{
			base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
			this.StopAndFaceTarget();
			base.EnemyController.LockFlip = true;
		}
		this.m_shoutWarningProjectile = this.FireProjectile("TraitorBossShoutWarningProjectile", 2, false, 0f, 1f, true, true, true);
		yield return this.Default_TellIntroAndLoop("OmniSpellCast_Tell_Intro", this.m_shout_TellIntro_AnimSpeed, "OmniSpellCast_Tell_Hold", this.m_shout_TellHold_AnimSpeed, this.m_shout_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("OmniSpellCast_Attack_Intro", this.m_shout_AttackIntro_AnimSpeed, this.m_shout_AttackIntro_Delay, true);
		this.m_shoutWarningProjectile.transform.position = base.EnemyController.Midpoint;
		this.m_shoutWarningProjectile.transform.SetParent(base.EnemyController.transform, true);
		this.m_shoutAttackWarningAppearedRelay.Dispatch();
		base.StopProjectile(ref this.m_shoutWarningProjectile);
		this.m_shoutAttackExplodedRelay.Dispatch();
		this.FireProjectile("TraitorBossShoutExplosionProjectile", 2, false, 0f, 1f, true, true, true);
		yield return this.Default_Animation("OmniSpellCast_Attack_Hold", this.m_shout_AttackHold_AnimSpeed, this.m_shout_AttackHold_Delay, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("OmniSpellCast_Exit", this.m_shout_Exit_AnimSpeed, this.m_shout_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Attack_Cooldown(this.m_shout_Exit_IdleDuration, this.m_shout_AttackCD);
		yield break;
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x00022067 File Offset: 0x00020267
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Staff_Throw()
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		this.m_staffBeamWarningProjectile = this.FireProjectile("TraitorWarningForwardBeamProjectile", this.STAFF_FRONT_POS_INDEX, true, 0f, 1f, true, true, true);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Spells/sfx_enemy_spell_gravityWell_prep", base.EnemyController.Midpoint);
		yield return this.Default_Animation("SpellCast_Attack_Intro", this.m_staffThrow_TellIntro_AnimSpeed, this.m_staffThrow_TellIntroAndHold_Delay, true);
		yield return this.Default_Animation("StaffForward_Attack_Intro", this.m_staffThrow_AttackIntro_AnimSpeed, this.m_staffThrow_AttackIntro_Delay, true);
		yield return this.Default_Animation("SpellCast_Attack_Hold", this.m_staffThrow_AttackHold_AnimSpeed, this.m_staffThrow_AttackHold_Delay, false);
		base.StopProjectile(ref this.m_staffBeamWarningProjectile);
		this.m_staffBeamProjectile = this.FireProjectile("TraitorForwardBeamProjectile", this.STAFF_FRONT_POS_INDEX, true, 0f, 1f, true, true, true);
		if (this.m_staffThrow_BeamAttack_Duration > 0f)
		{
			yield return base.Wait(this.m_staffThrow_BeamAttack_Duration, false);
		}
		base.StopProjectile(ref this.m_staffBeamProjectile);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return base.Wait(this.m_staffThrow_AttackDuration_EndDelay, false);
		yield return this.Default_Animation("SpellCast_Exit", this.m_staffThrow_Exit_AnimSpeed, this.m_staffThrow_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_staffThrow_Exit_IdleDuration, this.m_staffThrow_AttackCD);
		yield break;
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00022076 File Offset: 0x00020276
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.StopProjectile(ref this.m_staffBeamWarningProjectile);
		base.StopProjectile(ref this.m_staffBeamProjectile);
		base.StopProjectile(ref this.m_shoutWarningProjectile);
	}

	// Token: 0x04000F72 RID: 3954
	private const string SHOUT_TELL_INTRO = "OmniSpellCast_Tell_Intro";

	// Token: 0x04000F73 RID: 3955
	private const string SHOUT_TELL_HOLD = "OmniSpellCast_Tell_Hold";

	// Token: 0x04000F74 RID: 3956
	private const string SHOUT_ATTACK_INTRO = "OmniSpellCast_Attack_Intro";

	// Token: 0x04000F75 RID: 3957
	private const string SHOUT_ATTACK_HOLD = "OmniSpellCast_Attack_Hold";

	// Token: 0x04000F76 RID: 3958
	private const string SHOUT_EXIT = "OmniSpellCast_Exit";

	// Token: 0x04000F77 RID: 3959
	private float m_shout_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000F78 RID: 3960
	private float m_shout_TellHold_AnimSpeed = 1f;

	// Token: 0x04000F79 RID: 3961
	private float m_shout_TellIntroAndHold_Delay = 1.5f;

	// Token: 0x04000F7A RID: 3962
	private float m_shout_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000F7B RID: 3963
	private float m_shout_AttackIntro_Delay;

	// Token: 0x04000F7C RID: 3964
	private float m_shout_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000F7D RID: 3965
	private float m_shout_AttackHold_Delay;

	// Token: 0x04000F7E RID: 3966
	private float m_shout_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000F7F RID: 3967
	private float m_shout_Exit_Delay;

	// Token: 0x04000F80 RID: 3968
	private float m_shout_Exit_IdleDuration = 0.35f;

	// Token: 0x04000F81 RID: 3969
	private float m_shout_AttackCD = 12f;

	// Token: 0x04000F82 RID: 3970
	private float m_shoutAttackTimeToExplosion;

	// Token: 0x04000F83 RID: 3971
	private bool m_shout_stopMovingWhileAttacking = true;

	// Token: 0x04000F84 RID: 3972
	private float m_shoutAttackDelaySecondExplosion = 0.65f;

	// Token: 0x04000F85 RID: 3973
	private int m_shoutAttackDelaySecondExplosionAngleAdd = 10;

	// Token: 0x04000F86 RID: 3974
	private Projectile_RL m_shoutWarningProjectile;

	// Token: 0x04000F87 RID: 3975
	private Relay m_shoutAttackWarningAppearedRelay = new Relay();

	// Token: 0x04000F88 RID: 3976
	private Relay m_shoutAttackExplodedRelay = new Relay();

	// Token: 0x04000F89 RID: 3977
	protected const string STAFFTHROW_TELL_INTRO = "SpellCast_Attack_Intro";

	// Token: 0x04000F8A RID: 3978
	protected const string STAFFTHROW_TELL_HOLD = "StaffForward_Tell_Hold";

	// Token: 0x04000F8B RID: 3979
	protected const string STAFFTHROW_ATTACK_INTRO = "StaffForward_Attack_Intro";

	// Token: 0x04000F8C RID: 3980
	protected const string STAFFTHROW_ATTACK_HOLD = "SpellCast_Attack_Hold";

	// Token: 0x04000F8D RID: 3981
	protected const string STAFFTHROW_EXIT = "SpellCast_Exit";

	// Token: 0x04000F8E RID: 3982
	protected float m_staffThrow_BeamAttack_Duration = 0.75f;

	// Token: 0x04000F8F RID: 3983
	protected float m_staffThrow_BeamAttack_Variant_Duration = 1f;

	// Token: 0x04000F90 RID: 3984
	protected float m_staffThrow_AttackSpeed = 28f;

	// Token: 0x04000F91 RID: 3985
	protected float m_staffThrow_AttackDuration_EndDelay = 1f;

	// Token: 0x04000F92 RID: 3986
	protected float m_staffThrow_TellIntro_AnimSpeed = 1.5f;

	// Token: 0x04000F93 RID: 3987
	protected float m_staffThrow_TellHold_AnimSpeed = 1f;

	// Token: 0x04000F94 RID: 3988
	protected float m_staffThrow_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04000F95 RID: 3989
	protected float m_staffThrow_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04000F96 RID: 3990
	protected float m_staffThrow_AttackIntro_Delay;

	// Token: 0x04000F97 RID: 3991
	protected float m_staffThrow_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04000F98 RID: 3992
	protected float m_staffThrow_AttackHold_Delay;

	// Token: 0x04000F99 RID: 3993
	protected float m_staffThrow_Exit_AnimSpeed = 1f;

	// Token: 0x04000F9A RID: 3994
	protected float m_staffThrow_Exit_Delay;

	// Token: 0x04000F9B RID: 3995
	protected float m_staffThrow_Exit_IdleDuration = 0.15f;

	// Token: 0x04000F9C RID: 3996
	protected float m_staffThrow_AttackCD = 12f;

	// Token: 0x04000F9D RID: 3997
	protected int STAFF_FRONT_POS_INDEX = 1;

	// Token: 0x04000F9E RID: 3998
	private Projectile_RL m_staffBeamProjectile;

	// Token: 0x04000F9F RID: 3999
	private Projectile_RL m_staffBeamWarningProjectile;
}
