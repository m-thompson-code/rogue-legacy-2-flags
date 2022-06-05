using System;
using System.Collections;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200024E RID: 590
public class TraitorBoss_Expert_AIScript : TraitorBoss_Basic_AIScript
{
	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x060010F0 RID: 4336 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override int m_magma_ProjectilesFired
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170007FD RID: 2045
	// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected override float m_magma_ProjectileDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x00008E70 File Offset: 0x00007070
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

	// Token: 0x170007FE RID: 2046
	// (get) Token: 0x060010F3 RID: 4339 RVA: 0x00008269 File Offset: 0x00006469
	protected virtual int m_shout_fireballsAtEnd
	{
		get
		{
			return 18;
		}
	}

	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x060010F4 RID: 4340 RVA: 0x00008E7F File Offset: 0x0000707F
	public IRelayLink ShoutAttackWarningAppearedRelay
	{
		get
		{
			return this.m_shoutAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x060010F5 RID: 4341 RVA: 0x00008E8C File Offset: 0x0000708C
	public IRelayLink ShoutAttackExplodedRelay
	{
		get
		{
			return this.m_shoutAttackExplodedRelay.link;
		}
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x00008E99 File Offset: 0x00007099
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

	// Token: 0x060010F7 RID: 4343 RVA: 0x00008EA8 File Offset: 0x000070A8
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

	// Token: 0x060010F8 RID: 4344 RVA: 0x00008EB7 File Offset: 0x000070B7
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.StopProjectile(ref this.m_staffBeamWarningProjectile);
		base.StopProjectile(ref this.m_staffBeamProjectile);
		base.StopProjectile(ref this.m_shoutWarningProjectile);
	}

	// Token: 0x040013EE RID: 5102
	private const string SHOUT_TELL_INTRO = "OmniSpellCast_Tell_Intro";

	// Token: 0x040013EF RID: 5103
	private const string SHOUT_TELL_HOLD = "OmniSpellCast_Tell_Hold";

	// Token: 0x040013F0 RID: 5104
	private const string SHOUT_ATTACK_INTRO = "OmniSpellCast_Attack_Intro";

	// Token: 0x040013F1 RID: 5105
	private const string SHOUT_ATTACK_HOLD = "OmniSpellCast_Attack_Hold";

	// Token: 0x040013F2 RID: 5106
	private const string SHOUT_EXIT = "OmniSpellCast_Exit";

	// Token: 0x040013F3 RID: 5107
	private float m_shout_TellIntro_AnimSpeed = 1f;

	// Token: 0x040013F4 RID: 5108
	private float m_shout_TellHold_AnimSpeed = 1f;

	// Token: 0x040013F5 RID: 5109
	private float m_shout_TellIntroAndHold_Delay = 1.5f;

	// Token: 0x040013F6 RID: 5110
	private float m_shout_AttackIntro_AnimSpeed = 1f;

	// Token: 0x040013F7 RID: 5111
	private float m_shout_AttackIntro_Delay;

	// Token: 0x040013F8 RID: 5112
	private float m_shout_AttackHold_AnimSpeed = 1f;

	// Token: 0x040013F9 RID: 5113
	private float m_shout_AttackHold_Delay;

	// Token: 0x040013FA RID: 5114
	private float m_shout_Exit_AnimSpeed = 0.65f;

	// Token: 0x040013FB RID: 5115
	private float m_shout_Exit_Delay;

	// Token: 0x040013FC RID: 5116
	private float m_shout_Exit_IdleDuration = 0.35f;

	// Token: 0x040013FD RID: 5117
	private float m_shout_AttackCD = 12f;

	// Token: 0x040013FE RID: 5118
	private float m_shoutAttackTimeToExplosion;

	// Token: 0x040013FF RID: 5119
	private bool m_shout_stopMovingWhileAttacking = true;

	// Token: 0x04001400 RID: 5120
	private float m_shoutAttackDelaySecondExplosion = 0.65f;

	// Token: 0x04001401 RID: 5121
	private int m_shoutAttackDelaySecondExplosionAngleAdd = 10;

	// Token: 0x04001402 RID: 5122
	private Projectile_RL m_shoutWarningProjectile;

	// Token: 0x04001403 RID: 5123
	private Relay m_shoutAttackWarningAppearedRelay = new Relay();

	// Token: 0x04001404 RID: 5124
	private Relay m_shoutAttackExplodedRelay = new Relay();

	// Token: 0x04001405 RID: 5125
	protected const string STAFFTHROW_TELL_INTRO = "SpellCast_Attack_Intro";

	// Token: 0x04001406 RID: 5126
	protected const string STAFFTHROW_TELL_HOLD = "StaffForward_Tell_Hold";

	// Token: 0x04001407 RID: 5127
	protected const string STAFFTHROW_ATTACK_INTRO = "StaffForward_Attack_Intro";

	// Token: 0x04001408 RID: 5128
	protected const string STAFFTHROW_ATTACK_HOLD = "SpellCast_Attack_Hold";

	// Token: 0x04001409 RID: 5129
	protected const string STAFFTHROW_EXIT = "SpellCast_Exit";

	// Token: 0x0400140A RID: 5130
	protected float m_staffThrow_BeamAttack_Duration = 0.75f;

	// Token: 0x0400140B RID: 5131
	protected float m_staffThrow_BeamAttack_Variant_Duration = 1f;

	// Token: 0x0400140C RID: 5132
	protected float m_staffThrow_AttackSpeed = 28f;

	// Token: 0x0400140D RID: 5133
	protected float m_staffThrow_AttackDuration_EndDelay = 1f;

	// Token: 0x0400140E RID: 5134
	protected float m_staffThrow_TellIntro_AnimSpeed = 1.5f;

	// Token: 0x0400140F RID: 5135
	protected float m_staffThrow_TellHold_AnimSpeed = 1f;

	// Token: 0x04001410 RID: 5136
	protected float m_staffThrow_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04001411 RID: 5137
	protected float m_staffThrow_AttackIntro_AnimSpeed = 1.2f;

	// Token: 0x04001412 RID: 5138
	protected float m_staffThrow_AttackIntro_Delay;

	// Token: 0x04001413 RID: 5139
	protected float m_staffThrow_AttackHold_AnimSpeed = 1.2f;

	// Token: 0x04001414 RID: 5140
	protected float m_staffThrow_AttackHold_Delay;

	// Token: 0x04001415 RID: 5141
	protected float m_staffThrow_Exit_AnimSpeed = 1f;

	// Token: 0x04001416 RID: 5142
	protected float m_staffThrow_Exit_Delay;

	// Token: 0x04001417 RID: 5143
	protected float m_staffThrow_Exit_IdleDuration = 0.15f;

	// Token: 0x04001418 RID: 5144
	protected float m_staffThrow_AttackCD = 12f;

	// Token: 0x04001419 RID: 5145
	protected int STAFF_FRONT_POS_INDEX = 1;

	// Token: 0x0400141A RID: 5146
	private Projectile_RL m_staffBeamProjectile;

	// Token: 0x0400141B RID: 5147
	private Projectile_RL m_staffBeamWarningProjectile;
}
