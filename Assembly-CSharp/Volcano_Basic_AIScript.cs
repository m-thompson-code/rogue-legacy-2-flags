using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class Volcano_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000AFA RID: 2810 RVA: 0x000221F0 File Offset: 0x000203F0
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"VolcanoLineBoltProjectile",
			"VolcanoWhiteBoltMinibossProjectile",
			"VolcanoWhiteBoltProjectile",
			"VolcanoHomingBoltProjectile",
			"VolcanoWarningProjectile"
		};
	}

	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x06000AFB RID: 2811 RVA: 0x00022226 File Offset: 0x00020426
	protected virtual float m_flameGout_Exit_ForceIdle
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x06000AFC RID: 2812 RVA: 0x0002222D File Offset: 0x0002042D
	protected virtual float m_flameGout_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x06000AFD RID: 2813 RVA: 0x00022234 File Offset: 0x00020434
	protected virtual bool m_flameGout_hasMultipleFlames
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x06000AFE RID: 2814 RVA: 0x00022237 File Offset: 0x00020437
	protected virtual float m_flameGout_AttackDuration
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0002223E File Offset: 0x0002043E
	protected virtual int m_flameGout_FireProjectileLoopAmount
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00022241 File Offset: 0x00020441
	protected virtual int m_flameGout_FireProjectileBulletAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00022244 File Offset: 0x00020444
	protected virtual Vector2 m_flameGout_FireProjectileAngle
	{
		get
		{
			return new Vector2(35f, 145f);
		}
	}

	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x06000B02 RID: 2818 RVA: 0x00022255 File Offset: 0x00020455
	protected virtual Vector2 m_flameGout_FireProjectileSpeedMod
	{
		get
		{
			return new Vector2(1f, 1.25f);
		}
	}

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00022266 File Offset: 0x00020466
	protected virtual float m_flameGout_AttackMoveSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0002226D File Offset: 0x0002046D
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator CastFlameGout()
	{
		this.StopAndFaceTarget();
		bool isFacingRight = base.EnemyController.IsFacingRight;
		base.EnemyController.LockFlip = true;
		this.m_flameTellProjectile = this.FireProjectile("VolcanoWarningProjectile", 1, true, 90f, 1f, true, true, true);
		yield return this.Default_TellIntroAndLoop("Flame_Tell_Intro", this.m_flameGout_TellIntro_AnimationSpeed, "Flame_Tell_Hold", this.m_flameGout_TellHold_AnimationSpeed, this.m_flameGout_Tell_Delay);
		base.StopProjectile(ref this.m_flameTellProjectile);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Flame_Attack_Intro", this.m_flameGout_AttackIntro_AnimationSpeed, this.m_flameGout_AttackIntro_Delay, true);
		yield return this.Default_Animation("Flame_Attack_Hold", this.m_flameGout_AttackHold_AnimationSpeed, this.m_flameGout_AttackHold_Delay, false);
		this.m_projectile1 = this.FireProjectile("VolcanoLineBoltProjectile", 1, true, 90f, 1f, true, true, true);
		if (this.m_flameGout_hasMultipleFlames)
		{
			this.m_projectile2 = this.FireProjectile("VolcanoLineBoltProjectile", 1, true, 80f, 1f, true, true, true);
			this.m_projectile3 = this.FireProjectile("VolcanoLineBoltProjectile", 1, true, 100f, 1f, true, true, true);
		}
		base.StartCoroutine(this.DelayProjectileWeaponHitbox(this.m_projectile1, 0.25f));
		base.StartCoroutine(this.DelayProjectileWeaponHitbox(this.m_projectile2, 0.25f));
		base.StartCoroutine(this.DelayProjectileWeaponHitbox(this.m_projectile3, 0.25f));
		if (isFacingRight)
		{
			base.SetVelocityX(this.m_flameGout_AttackMoveSpeed, false);
		}
		else
		{
			base.SetVelocityX(-this.m_flameGout_AttackMoveSpeed, false);
		}
		float delayBetweenProjectileAttacks = this.m_flameGout_AttackDuration / (float)this.m_flameGout_FireProjectileLoopAmount;
		int num2;
		for (int i = 0; i < this.m_flameGout_FireProjectileLoopAmount; i = num2 + 1)
		{
			for (int j = 0; j < this.m_flameGout_FireProjectileBulletAmount; j++)
			{
				int num = (int)UnityEngine.Random.Range(this.m_flameGout_FireProjectileAngle.x, this.m_flameGout_FireProjectileAngle.y);
				float speedMod = UnityEngine.Random.Range(this.m_flameGout_FireProjectileSpeedMod.x, this.m_flameGout_FireProjectileSpeedMod.y);
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.FireProjectile("VolcanoWhiteBoltMinibossProjectile", 2, true, (float)num, speedMod, true, true, true);
				}
				else
				{
					this.FireProjectile("VolcanoWhiteBoltProjectile", 2, true, (float)num, speedMod, true, true, true);
				}
			}
			if (delayBetweenProjectileAttacks > 0f)
			{
				yield return base.Wait(delayBetweenProjectileAttacks, false);
			}
			num2 = i;
		}
		base.SetVelocityX(0f, false);
		base.StopProjectile(ref this.m_projectile1);
		base.StopProjectile(ref this.m_projectile2);
		base.StopProjectile(ref this.m_projectile3);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Flame_Exit", this.m_flameGout_Exit_AnimationSpeed, this.m_flameGout_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_flameGout_Exit_ForceIdle, this.m_flameGout_AttackCD);
		yield break;
	}

	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0002227C File Offset: 0x0002047C
	protected virtual float m_alternateGout_Exit_ForceIdle
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00022283 File Offset: 0x00020483
	protected virtual float m_alternateGout_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0002228A File Offset: 0x0002048A
	protected virtual float m_alternateGout_AttackDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00022291 File Offset: 0x00020491
	protected virtual int m_alternateGout_FireProjectileLoopAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x06000B09 RID: 2825 RVA: 0x00022294 File Offset: 0x00020494
	protected virtual int m_alternateGout_FireProjectileBulletAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x00022297 File Offset: 0x00020497
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator CastAlternateGout()
	{
		this.StopAndFaceTarget();
		bool isFacingRight = base.EnemyController.IsFacingRight;
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("HomingShot_Tell_Intro", this.m_alternateGout_TellIntro_AnimationSpeed, "HomingShot_Tell_Hold", this.m_alternateGout_TellHold_AnimationSpeed, this.m_alternateGout_Tell_Delay);
		base.SetAttackingWithContactDamage(true, 0f);
		yield return this.Default_Animation("HomingShot_Attack_Intro", this.m_alternateGout_AttackIntro_AnimationSpeed, this.m_alternateGout_AttackIntro_Delay, true);
		yield return this.Default_Animation("HomingShot_Attack_Hold", this.m_alternateGout_AttackHold_AnimationSpeed, this.m_alternateGout_AttackHold_Delay, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		float delayBetweenProjectileAttacks = this.m_alternateGout_AttackDuration / (float)this.m_alternateGout_FireProjectileLoopAmount;
		int num;
		for (int i = 0; i < this.m_alternateGout_FireProjectileLoopAmount; i = num + 1)
		{
			for (int j = 0; j < this.m_alternateGout_FireProjectileBulletAmount; j++)
			{
				this.FireProjectile("VolcanoHomingBoltProjectile", 1, true, 75f, 1f, true, true, true);
				this.FireProjectile("VolcanoHomingBoltProjectile", 1, true, 105f, 1f, true, true, true);
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
				{
					this.FireProjectile("VolcanoHomingBoltProjectile", 1, true, 60f, 1f, true, true, true);
					this.FireProjectile("VolcanoHomingBoltProjectile", 1, true, 120f, 1f, true, true, true);
				}
			}
			if (delayBetweenProjectileAttacks > 0f)
			{
				yield return base.Wait(delayBetweenProjectileAttacks, false);
			}
			num = i;
		}
		base.SetVelocityX(0f, false);
		yield return this.Default_Animation("HomingShot_Exit", this.m_alternateGout_Exit_AnimationSpeed, this.m_alternateGout_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_alternateGout_Exit_ForceIdle, this.m_alternateGout_AttackCD);
		yield break;
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x000222A8 File Offset: 0x000204A8
	public override void Pause()
	{
		base.Pause();
		if (this.m_projectile1 && !this.m_projectile1.IsFreePoolObj && this.m_projectile1.Owner == base.EnemyController.gameObject)
		{
			this.m_wasFrozenWhileFiring = true;
			base.StopProjectile(ref this.m_projectile1);
		}
		if (this.m_projectile2 && !this.m_projectile2.IsFreePoolObj && this.m_projectile2.Owner == base.EnemyController.gameObject)
		{
			this.m_wasFrozenWhileFiring = true;
			base.StopProjectile(ref this.m_projectile2);
		}
		if (this.m_projectile3 && !this.m_projectile3.IsFreePoolObj && this.m_projectile3.Owner == base.EnemyController.gameObject)
		{
			this.m_wasFrozenWhileFiring = true;
			base.StopProjectile(ref this.m_projectile3);
		}
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0002239C File Offset: 0x0002059C
	public override void Unpause()
	{
		base.Unpause();
		if (this.m_wasFrozenWhileFiring)
		{
			this.m_projectile1 = this.FireProjectile("VolcanoLineBoltProjectile", 1, true, 90f, 1f, true, true, true);
			if (this.m_flameGout_hasMultipleFlames)
			{
				this.m_projectile2 = this.FireProjectile("VolcanoLineBoltProjectile", 1, true, 80f, 1f, true, true, true);
				this.m_projectile3 = this.FireProjectile("VolcanoLineBoltProjectile", 1, true, 100f, 1f, true, true, true);
			}
			this.m_wasFrozenWhileFiring = false;
		}
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x00022426 File Offset: 0x00020626
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_projectile1);
		base.StopProjectile(ref this.m_projectile2);
		base.StopProjectile(ref this.m_projectile3);
		base.StopProjectile(ref this.m_flameTellProjectile);
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0002245E File Offset: 0x0002065E
	public override void ResetScript()
	{
		this.m_wasFrozenWhileFiring = false;
		base.ResetScript();
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0002246D File Offset: 0x0002066D
	protected IEnumerator DelayProjectileWeaponHitbox(Projectile_RL projectile, float delay)
	{
		if (delay > 0f)
		{
			if (projectile && !projectile.IsFreePoolObj && projectile.Owner == base.EnemyController.gameObject)
			{
				projectile.HitboxController.SetHitboxActiveState(HitboxType.Weapon, false);
			}
			yield return base.Wait(delay, false);
			if (projectile && !projectile.IsFreePoolObj && projectile.Owner == base.EnemyController.gameObject)
			{
				projectile.HitboxController.SetHitboxActiveState(HitboxType.Weapon, true);
			}
		}
		yield break;
	}

	// Token: 0x04000FA0 RID: 4000
	protected const string FLAME_TELL_PROJECTILE = "VolcanoWarningProjectile";

	// Token: 0x04000FA1 RID: 4001
	protected Projectile_RL m_flameTellProjectile;

	// Token: 0x04000FA2 RID: 4002
	protected Projectile_RL m_projectile1;

	// Token: 0x04000FA3 RID: 4003
	protected Projectile_RL m_projectile2;

	// Token: 0x04000FA4 RID: 4004
	protected Projectile_RL m_projectile3;

	// Token: 0x04000FA5 RID: 4005
	protected bool m_wasFrozenWhileFiring;

	// Token: 0x04000FA6 RID: 4006
	protected const string FLAMEGOUT_TELL_INTRO = "Flame_Tell_Intro";

	// Token: 0x04000FA7 RID: 4007
	protected const string FLAMEGOUT_TELL_HOLD = "Flame_Tell_Hold";

	// Token: 0x04000FA8 RID: 4008
	protected const string FLAMEGOUT_ATTACK_INTRO = "Flame_Attack_Intro";

	// Token: 0x04000FA9 RID: 4009
	protected const string FLAMEGOUT_ATTACK_HOLD = "Flame_Attack_Hold";

	// Token: 0x04000FAA RID: 4010
	protected const string FLAMEGOUT_EXIT = "Flame_Exit";

	// Token: 0x04000FAB RID: 4011
	protected const string FLAMEGOUT_LINEBOLT_PROJECTILE = "VolcanoLineBoltProjectile";

	// Token: 0x04000FAC RID: 4012
	protected const string FLAMEGOUT_WHITEBOLT_PROJECTILE = "VolcanoWhiteBoltProjectile";

	// Token: 0x04000FAD RID: 4013
	protected const string FLAMEGOUT_WHITEBOLT_MINIBOSS_PROJECTILE = "VolcanoWhiteBoltMinibossProjectile";

	// Token: 0x04000FAE RID: 4014
	protected float m_flameGout_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000FAF RID: 4015
	protected float m_flameGout_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000FB0 RID: 4016
	protected float m_flameGout_Tell_Delay = 1.15f;

	// Token: 0x04000FB1 RID: 4017
	protected float m_flameGout_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000FB2 RID: 4018
	protected float m_flameGout_AttackIntro_Delay;

	// Token: 0x04000FB3 RID: 4019
	protected float m_flameGout_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000FB4 RID: 4020
	protected float m_flameGout_AttackHold_Delay;

	// Token: 0x04000FB5 RID: 4021
	protected float m_flameGout_Exit_AnimationSpeed = 1f;

	// Token: 0x04000FB6 RID: 4022
	protected float m_flameGout_Exit_Delay;

	// Token: 0x04000FB7 RID: 4023
	protected const string HOMINGSHOT_TELL_INTRO = "HomingShot_Tell_Intro";

	// Token: 0x04000FB8 RID: 4024
	protected const string HOMINGSHOT_TELL_HOLD = "HomingShot_Tell_Hold";

	// Token: 0x04000FB9 RID: 4025
	protected const string HOMINGSHOT_ATTACK_INTRO = "HomingShot_Attack_Intro";

	// Token: 0x04000FBA RID: 4026
	protected const string HOMINGSHOT_ATTACK_HOLD = "HomingShot_Attack_Hold";

	// Token: 0x04000FBB RID: 4027
	protected const string HOMINGSHOT_EXIT = "HomingShot_Exit";

	// Token: 0x04000FBC RID: 4028
	private const string FLAMEGOUT_HOMINGBOLT_PROJECTILE = "VolcanoHomingBoltProjectile";

	// Token: 0x04000FBD RID: 4029
	protected float m_alternateGout_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000FBE RID: 4030
	protected float m_alternateGout_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000FBF RID: 4031
	protected float m_alternateGout_Tell_Delay = 1.45f;

	// Token: 0x04000FC0 RID: 4032
	protected float m_alternateGout_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000FC1 RID: 4033
	protected float m_alternateGout_AttackIntro_Delay;

	// Token: 0x04000FC2 RID: 4034
	protected float m_alternateGout_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000FC3 RID: 4035
	protected float m_alternateGout_AttackHold_Delay;

	// Token: 0x04000FC4 RID: 4036
	protected float m_alternateGout_Exit_AnimationSpeed = 1f;

	// Token: 0x04000FC5 RID: 4037
	protected float m_alternateGout_Exit_Delay;
}
