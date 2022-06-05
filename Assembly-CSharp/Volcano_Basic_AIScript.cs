using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000255 RID: 597
public class Volcano_Basic_AIScript : BaseAIScript
{
	// Token: 0x06001117 RID: 4375 RVA: 0x00008F56 File Offset: 0x00007156
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

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x06001118 RID: 4376 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_flameGout_Exit_ForceIdle
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x06001119 RID: 4377 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_flameGout_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x0600111A RID: 4378 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_flameGout_hasMultipleFlames
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x0600111B RID: 4379 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_flameGout_AttackDuration
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x0600111C RID: 4380 RVA: 0x00004A07 File Offset: 0x00002C07
	protected virtual int m_flameGout_FireProjectileLoopAmount
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x17000810 RID: 2064
	// (get) Token: 0x0600111D RID: 4381 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_flameGout_FireProjectileBulletAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000811 RID: 2065
	// (get) Token: 0x0600111E RID: 4382 RVA: 0x00008F8C File Offset: 0x0000718C
	protected virtual Vector2 m_flameGout_FireProjectileAngle
	{
		get
		{
			return new Vector2(35f, 145f);
		}
	}

	// Token: 0x17000812 RID: 2066
	// (get) Token: 0x0600111F RID: 4383 RVA: 0x00008F9D File Offset: 0x0000719D
	protected virtual Vector2 m_flameGout_FireProjectileSpeedMod
	{
		get
		{
			return new Vector2(1f, 1.25f);
		}
	}

	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x06001120 RID: 4384 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_flameGout_AttackMoveSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x00008FAE File Offset: 0x000071AE
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

	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x06001122 RID: 4386 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_alternateGout_Exit_ForceIdle
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x06001123 RID: 4387 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_alternateGout_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x06001124 RID: 4388 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_alternateGout_AttackDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x06001125 RID: 4389 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_alternateGout_FireProjectileLoopAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x06001126 RID: 4390 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_alternateGout_FireProjectileBulletAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06001127 RID: 4391 RVA: 0x00008FBD File Offset: 0x000071BD
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

	// Token: 0x06001128 RID: 4392 RVA: 0x0007EAB4 File Offset: 0x0007CCB4
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

	// Token: 0x06001129 RID: 4393 RVA: 0x0007EBA8 File Offset: 0x0007CDA8
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

	// Token: 0x0600112A RID: 4394 RVA: 0x00008FCC File Offset: 0x000071CC
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_projectile1);
		base.StopProjectile(ref this.m_projectile2);
		base.StopProjectile(ref this.m_projectile3);
		base.StopProjectile(ref this.m_flameTellProjectile);
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x0600112B RID: 4395 RVA: 0x00009004 File Offset: 0x00007204
	public override void ResetScript()
	{
		this.m_wasFrozenWhileFiring = false;
		base.ResetScript();
	}

	// Token: 0x0600112C RID: 4396 RVA: 0x00009013 File Offset: 0x00007213
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

	// Token: 0x04001429 RID: 5161
	protected const string FLAME_TELL_PROJECTILE = "VolcanoWarningProjectile";

	// Token: 0x0400142A RID: 5162
	protected Projectile_RL m_flameTellProjectile;

	// Token: 0x0400142B RID: 5163
	protected Projectile_RL m_projectile1;

	// Token: 0x0400142C RID: 5164
	protected Projectile_RL m_projectile2;

	// Token: 0x0400142D RID: 5165
	protected Projectile_RL m_projectile3;

	// Token: 0x0400142E RID: 5166
	protected bool m_wasFrozenWhileFiring;

	// Token: 0x0400142F RID: 5167
	protected const string FLAMEGOUT_TELL_INTRO = "Flame_Tell_Intro";

	// Token: 0x04001430 RID: 5168
	protected const string FLAMEGOUT_TELL_HOLD = "Flame_Tell_Hold";

	// Token: 0x04001431 RID: 5169
	protected const string FLAMEGOUT_ATTACK_INTRO = "Flame_Attack_Intro";

	// Token: 0x04001432 RID: 5170
	protected const string FLAMEGOUT_ATTACK_HOLD = "Flame_Attack_Hold";

	// Token: 0x04001433 RID: 5171
	protected const string FLAMEGOUT_EXIT = "Flame_Exit";

	// Token: 0x04001434 RID: 5172
	protected const string FLAMEGOUT_LINEBOLT_PROJECTILE = "VolcanoLineBoltProjectile";

	// Token: 0x04001435 RID: 5173
	protected const string FLAMEGOUT_WHITEBOLT_PROJECTILE = "VolcanoWhiteBoltProjectile";

	// Token: 0x04001436 RID: 5174
	protected const string FLAMEGOUT_WHITEBOLT_MINIBOSS_PROJECTILE = "VolcanoWhiteBoltMinibossProjectile";

	// Token: 0x04001437 RID: 5175
	protected float m_flameGout_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04001438 RID: 5176
	protected float m_flameGout_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04001439 RID: 5177
	protected float m_flameGout_Tell_Delay = 1.15f;

	// Token: 0x0400143A RID: 5178
	protected float m_flameGout_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x0400143B RID: 5179
	protected float m_flameGout_AttackIntro_Delay;

	// Token: 0x0400143C RID: 5180
	protected float m_flameGout_AttackHold_AnimationSpeed = 1f;

	// Token: 0x0400143D RID: 5181
	protected float m_flameGout_AttackHold_Delay;

	// Token: 0x0400143E RID: 5182
	protected float m_flameGout_Exit_AnimationSpeed = 1f;

	// Token: 0x0400143F RID: 5183
	protected float m_flameGout_Exit_Delay;

	// Token: 0x04001440 RID: 5184
	protected const string HOMINGSHOT_TELL_INTRO = "HomingShot_Tell_Intro";

	// Token: 0x04001441 RID: 5185
	protected const string HOMINGSHOT_TELL_HOLD = "HomingShot_Tell_Hold";

	// Token: 0x04001442 RID: 5186
	protected const string HOMINGSHOT_ATTACK_INTRO = "HomingShot_Attack_Intro";

	// Token: 0x04001443 RID: 5187
	protected const string HOMINGSHOT_ATTACK_HOLD = "HomingShot_Attack_Hold";

	// Token: 0x04001444 RID: 5188
	protected const string HOMINGSHOT_EXIT = "HomingShot_Exit";

	// Token: 0x04001445 RID: 5189
	private const string FLAMEGOUT_HOMINGBOLT_PROJECTILE = "VolcanoHomingBoltProjectile";

	// Token: 0x04001446 RID: 5190
	protected float m_alternateGout_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04001447 RID: 5191
	protected float m_alternateGout_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04001448 RID: 5192
	protected float m_alternateGout_Tell_Delay = 1.45f;

	// Token: 0x04001449 RID: 5193
	protected float m_alternateGout_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x0400144A RID: 5194
	protected float m_alternateGout_AttackIntro_Delay;

	// Token: 0x0400144B RID: 5195
	protected float m_alternateGout_AttackHold_AnimationSpeed = 1f;

	// Token: 0x0400144C RID: 5196
	protected float m_alternateGout_AttackHold_Delay;

	// Token: 0x0400144D RID: 5197
	protected float m_alternateGout_Exit_AnimationSpeed = 1f;

	// Token: 0x0400144E RID: 5198
	protected float m_alternateGout_Exit_Delay;
}
