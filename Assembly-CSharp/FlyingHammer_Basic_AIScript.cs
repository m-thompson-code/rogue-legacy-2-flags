using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000DA RID: 218
public class FlyingHammer_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000651 RID: 1617 RVA: 0x00019184 File Offset: 0x00017384
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingHammerMagmaProjectile",
			"FlyingHammerWarningMinibossProjectile",
			"FlyingHammerWarningLargeProjectile",
			"FlyingHammerWarningProjectile",
			"FlyingHammerQuakeMinibossProjectile",
			"FlyingHammerQuakeLargeProjectile",
			"FlyingHammerQuakeProjectile",
			"FlyingHammerFireballMinibossProjectile",
			"FlyingHammerCurseBoltProjectile",
			"FlyingHammerWhirlwindProjectile",
			"FlyingHammerWarningWhirlwindProjectile"
		};
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06000652 RID: 1618 RVA: 0x000191F8 File Offset: 0x000173F8
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.7f, 1f);
		}
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06000653 RID: 1619 RVA: 0x00019209 File Offset: 0x00017409
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06000654 RID: 1620 RVA: 0x0001921A File Offset: 0x0001741A
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06000655 RID: 1621 RVA: 0x0001922B File Offset: 0x0001742B
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06000656 RID: 1622 RVA: 0x0001923C File Offset: 0x0001743C
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06000657 RID: 1623 RVA: 0x0001924D File Offset: 0x0001744D
	protected virtual float m_vertSpin_Attack_MovementSpeed
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06000658 RID: 1624 RVA: 0x00019254 File Offset: 0x00017454
	protected virtual Vector2 m_fireballSpeedMod
	{
		get
		{
			return new Vector2(0.5f, 1f);
		}
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06000659 RID: 1625 RVA: 0x00019265 File Offset: 0x00017465
	protected virtual Vector2 m_fireballAngle
	{
		get
		{
			return new Vector2(180f, 0f);
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x0600065A RID: 1626 RVA: 0x00019276 File Offset: 0x00017476
	protected virtual float m_initialProjectileDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x0600065B RID: 1627 RVA: 0x0001927D File Offset: 0x0001747D
	protected virtual float m_exitProjectileDelay
	{
		get
		{
			return 0.55f;
		}
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x0600065C RID: 1628 RVA: 0x00019284 File Offset: 0x00017484
	protected virtual int m_projectileSpawnAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x0600065D RID: 1629 RVA: 0x00019287 File Offset: 0x00017487
	protected virtual int m_projectileSpawnLoopCount
	{
		get
		{
			return 12;
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x0600065E RID: 1630 RVA: 0x0001928B File Offset: 0x0001748B
	protected virtual float m_projectileRateOfFire
	{
		get
		{
			return 0.125f;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x0600065F RID: 1631 RVA: 0x00019292 File Offset: 0x00017492
	protected virtual bool m_shockwave_IsLarge
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06000660 RID: 1632 RVA: 0x00019295 File Offset: 0x00017495
	protected virtual float m_shockwave_Attack_TurnSpeed
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001929C File Offset: 0x0001749C
	protected virtual float m_shockwave_Attack_MovementSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06000662 RID: 1634 RVA: 0x000192A3 File Offset: 0x000174A3
	protected virtual Vector2 m_shockwave_fireballSpeedMod
	{
		get
		{
			return new Vector2(0.5f, 1.25f);
		}
	}

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06000663 RID: 1635 RVA: 0x000192B4 File Offset: 0x000174B4
	protected virtual Vector2 m_shockwave_fireballAngle
	{
		get
		{
			return new Vector2(360f, 0f);
		}
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06000664 RID: 1636 RVA: 0x000192C5 File Offset: 0x000174C5
	protected virtual float m_shockwave_initialProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06000665 RID: 1637 RVA: 0x000192CC File Offset: 0x000174CC
	protected virtual float m_shockwave_exitProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06000666 RID: 1638 RVA: 0x000192D3 File Offset: 0x000174D3
	protected virtual int m_shockwave_projectileSpawnAmount
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06000667 RID: 1639 RVA: 0x000192D6 File Offset: 0x000174D6
	protected virtual int m_shockwave_projectileSpawnLoopCount
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06000668 RID: 1640 RVA: 0x000192D9 File Offset: 0x000174D9
	protected virtual bool m_causesRicochet
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06000669 RID: 1641 RVA: 0x000192DC File Offset: 0x000174DC
	protected virtual Vector2 m_ricochetKnockbackMod
	{
		get
		{
			return new Vector2(10.5f, 13.5f);
		}
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x000192ED File Offset: 0x000174ED
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Vertical_Spin_Attack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		yield return this.Default_TellIntroAndLoop("SideSpin_Tell_Intro", this.m_vertSpin_TellIntro_AnimationSpeed, "SideSpin_Tell_Hold", this.m_vertSpin_TellHold_AnimationSpeed, this.m_vertSpin_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("SideSpin_Attack_Intro", this.m_vertSpin_AttackIntro_AnimationSpeed, this.m_vertSpin_AttackIntro_Delay, true);
		yield return this.Default_Animation("SideSpin_Attack_Hold", 1f, 0f, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_vertSpin_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_vertSpin_Attack_TurnSpeed;
		if (this.m_initialProjectileDelay > 0f)
		{
			yield return base.Wait(this.m_initialProjectileDelay, false);
		}
		int num2;
		for (int i = 0; i < this.m_projectileSpawnLoopCount; i = num2 + 1)
		{
			for (int j = 0; j < this.m_projectileSpawnAmount; j++)
			{
				int num = (int)UnityEngine.Random.Range(this.m_fireballAngle.x, this.m_fireballAngle.y);
				float speedMod = UnityEngine.Random.Range(this.m_fireballSpeedMod.x, this.m_fireballSpeedMod.y);
				this.FireProjectile("FlyingHammerMagmaProjectile", new Vector2(0f, 0f), true, (float)num, speedMod, true, true, true);
			}
			if (this.m_projectileRateOfFire > 0f)
			{
				yield return base.Wait(this.m_projectileRateOfFire, false);
			}
			num2 = i;
		}
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("SideSpin_Exit", this.m_vertSpin_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Attack_Cooldown(0.5f, this.m_vertSpin_Exit_AttackCD);
		yield break;
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x000192FC File Offset: 0x000174FC
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Shockwave_Attack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.m_warningProjectile = this.FireProjectile("FlyingHammerWarningMinibossProjectile", 0, true, 0f, 1f, true, true, true);
		}
		else if (this.m_shockwave_IsLarge)
		{
			this.m_warningProjectile = this.FireProjectile("FlyingHammerWarningLargeProjectile", 0, true, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_warningProjectile = this.FireProjectile("FlyingHammerWarningProjectile", 0, true, 0f, 1f, true, true, true);
		}
		yield return this.Default_TellIntroAndLoop("WallHit_Tell_Intro", this.m_shockwave_TellIntro_AnimationSpeed, "WallHit_Tell_Hold", this.m_shockwave_TellHold_AnimationSpeed, this.m_shockwave_TellIntroAndHold_Delay);
		base.StopProjectile(ref this.m_warningProjectile);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("WallHit_Attack_Intro", this.m_shockwave_AttackIntro_AnimationSpeed, this.m_shockwave_AttackIntro_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_shockwave_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_shockwave_Attack_TurnSpeed;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.m_shockwaveProjectile = this.FireProjectile("FlyingHammerQuakeMinibossProjectile", 0, true, 0f, 1f, true, true, true);
		}
		else if (this.m_shockwave_IsLarge)
		{
			this.m_shockwaveProjectile = this.FireProjectile("FlyingHammerQuakeLargeProjectile", 0, true, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_shockwaveProjectile = this.FireProjectile("FlyingHammerQuakeProjectile", 0, true, 0f, 1f, true, true, true);
		}
		this.m_shockwaveProjectileSpawnPos = this.m_shockwaveProjectile.transform.position;
		yield return this.Default_Animation("WallHit_Attack_Hold", 1f, 0f, true);
		if (this.m_initialProjectileDelay > 0f)
		{
			yield return base.Wait(this.m_shockwave_initialProjectileDelay, false);
		}
		int num2;
		for (int i = 0; i < this.m_shockwave_projectileSpawnLoopCount + 1; i = num2 + 1)
		{
			for (int j = 0; j < this.m_shockwave_projectileSpawnAmount; j++)
			{
				int num = (int)UnityEngine.Random.Range(this.m_shockwave_fireballAngle.x, this.m_shockwave_fireballAngle.y);
				float speedMod = UnityEngine.Random.Range(this.m_shockwave_fireballSpeedMod.x, this.m_shockwave_fireballSpeedMod.y);
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.FireProjectile("FlyingHammerFireballMinibossProjectile", new Vector2(0f, 0f), false, (float)num, speedMod, true, true, true);
				}
				else
				{
					this.FireProjectile("FlyingHammerCurseBoltProjectile", new Vector2(0f, 0f), false, (float)num, speedMod, true, true, true);
				}
			}
			if (1f / (float)this.m_shockwave_projectileSpawnLoopCount > 0f)
			{
				yield return base.Wait(1f / (float)(this.m_shockwave_projectileSpawnLoopCount + 1), false);
			}
			num2 = i;
		}
		base.StopProjectile(ref this.m_shockwaveProjectile);
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("WallHit_Exit", this.m_shockwave_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Attack_Cooldown(1.25f, this.m_shockwave_Exit_AttackCD);
		yield break;
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x0001930B File Offset: 0x0001750B
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShockwaveAtPlayer_Attack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		base.EnemyController.LockFlip = true;
		Vector3 shockwaveSpawnPos = base.EnemyController.TargetController.Midpoint;
		this.m_isHammeringAtPlayer = true;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		else if (this.m_shockwave_IsLarge)
		{
			this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		yield return this.Default_TellIntroAndLoop("VerticalSpin_Tell_Intro", this.m_shockwaveAtPlayer_TellIntro_AnimationSpeed, "VerticalSpin_Tell_Hold", this.m_shockwaveAtPlayer_TellHold_AnimationSpeed, this.m_shockwaveAtPlayer_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		base.StopProjectile(ref this.m_warningProjectile);
		yield return this.Default_Animation("VerticalSpin_Attack_Intro", this.m_shockwaveAtPlayer_AttackIntro_AnimationSpeed, this.m_shockwaveAtPlayer_AttackIntro_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_shockwave_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_shockwave_Attack_TurnSpeed;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		else if (this.m_shockwave_IsLarge)
		{
			this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		this.m_shockwaveProjectileSpawnPos = this.m_shockwaveProjectile.transform.position;
		if (this.m_shockwave_IsLarge)
		{
			yield return this.Default_Animation("VerticalSpin_Attack_Hold", 1f, 0f, true);
		}
		else
		{
			yield return this.Default_Animation("VerticalSpin_Attack_Hold", 1f, 1.55f, true);
		}
		if (this.m_initialProjectileDelay > 0f)
		{
			yield return base.Wait(this.m_shockwave_initialProjectileDelay, false);
		}
		if (this.m_shockwave_IsLarge)
		{
			int num2;
			for (int i = 0; i < this.m_shockwave_projectileSpawnLoopCount + 1; i = num2 + 1)
			{
				for (int j = 0; j < this.m_shockwave_projectileSpawnAmount; j++)
				{
					int num = (int)UnityEngine.Random.Range(this.m_shockwave_fireballAngle.x, this.m_shockwave_fireballAngle.y);
					float speedMod = UnityEngine.Random.Range(this.m_shockwave_fireballSpeedMod.x, this.m_shockwave_fireballSpeedMod.y);
					if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
					{
						this.FireProjectileAbsPos("FlyingHammerFireballMinibossProjectile", shockwaveSpawnPos, false, (float)num, speedMod, true, true, true);
					}
					else
					{
						this.FireProjectileAbsPos("FlyingHammerCurseBoltProjectile", shockwaveSpawnPos, false, (float)num, speedMod, true, true, true);
					}
				}
				if (1f / (float)this.m_shockwave_projectileSpawnLoopCount > 0f)
				{
					yield return base.Wait(1f / (float)(this.m_shockwave_projectileSpawnLoopCount + 1), false);
				}
				num2 = i;
			}
		}
		base.StopProjectile(ref this.m_shockwaveProjectile);
		this.m_isHammeringAtPlayer = false;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("VerticalSpin_Exit", this.m_shockwaveAtPlayer_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		base.EnemyController.LockFlip = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Attack_Cooldown(1.25f, this.m_shockwaveAtPlayer_Exit_AttackCD);
		yield break;
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x0001931C File Offset: 0x0001751C
	public override void Pause()
	{
		base.Pause();
		if (this.m_shockwaveProjectile && !this.m_shockwaveProjectile.IsFreePoolObj && this.m_shockwaveProjectile.Owner == base.EnemyController.gameObject)
		{
			this.m_frozenWhileHammering = true;
			base.StopProjectile(ref this.m_shockwaveProjectile);
			if (!this.m_isHammeringAtPlayer)
			{
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningMinibossProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
					return;
				}
				if (this.m_shockwave_IsLarge)
				{
					this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningLargeProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
					return;
				}
				this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
				return;
			}
			else
			{
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
					return;
				}
				if (this.m_shockwave_IsLarge)
				{
					this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
					return;
				}
				this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
			}
		}
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x000194BC File Offset: 0x000176BC
	public override void Unpause()
	{
		base.Unpause();
		if (this.m_frozenWhileHammering)
		{
			if (!this.m_isHammeringAtPlayer)
			{
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerQuakeMinibossProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
				}
				else if (this.m_shockwave_IsLarge)
				{
					this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerQuakeLargeProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
				}
				else
				{
					this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerQuakeProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
				}
			}
			else if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
			{
				this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
			}
			else if (this.m_shockwave_IsLarge)
			{
				this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
			}
			else
			{
				this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
			}
			base.StopProjectile(ref this.m_warningProjectile);
			this.m_frozenWhileHammering = false;
		}
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00019633 File Offset: 0x00017833
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_warningProjectile);
		base.StopProjectile(ref this.m_shockwaveProjectile);
		this.m_frozenWhileHammering = false;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0001965A File Offset: 0x0001785A
	public override void ResetScript()
	{
		this.m_frozenWhileHammering = false;
		this.m_isHammeringAtPlayer = false;
		base.ResetScript();
	}

	// Token: 0x04000A89 RID: 2697
	private const string GENERIC_PROJECTILE_NAME = "FlyingHammerMagmaProjectile";

	// Token: 0x04000A8A RID: 2698
	private const string WARNING_MINIBOSS_PROJECTILE_NAME = "FlyingHammerWarningMinibossProjectile";

	// Token: 0x04000A8B RID: 2699
	private const string WARNING_LARGE_PROJECTILE_NAME = "FlyingHammerWarningLargeProjectile";

	// Token: 0x04000A8C RID: 2700
	private const string WARNING_PROJECTILE_NAME = "FlyingHammerWarningProjectile";

	// Token: 0x04000A8D RID: 2701
	private const string QUAKE_MINIBOSS_PROJECTILE_NAME = "FlyingHammerQuakeMinibossProjectile";

	// Token: 0x04000A8E RID: 2702
	private const string QUAKE_LARGE_PROJECTILE_NAME = "FlyingHammerQuakeLargeProjectile";

	// Token: 0x04000A8F RID: 2703
	private const string QUAKE_PROJECTILE_NAME = "FlyingHammerQuakeProjectile";

	// Token: 0x04000A90 RID: 2704
	private const string FIREBALL_MINIBOSS_PROJECTILE_NAME = "FlyingHammerFireballMinibossProjectile";

	// Token: 0x04000A91 RID: 2705
	private const string CURSEBOLT_PROJECTILE_NAME = "FlyingHammerCurseBoltProjectile";

	// Token: 0x04000A92 RID: 2706
	private bool m_frozenWhileHammering;

	// Token: 0x04000A93 RID: 2707
	private bool m_isHammeringAtPlayer;

	// Token: 0x04000A94 RID: 2708
	protected Projectile_RL m_warningProjectile;

	// Token: 0x04000A95 RID: 2709
	protected Projectile_RL m_shockwaveProjectile;

	// Token: 0x04000A96 RID: 2710
	protected Vector3 m_shockwaveProjectileSpawnPos;

	// Token: 0x04000A97 RID: 2711
	protected float m_vertSpin_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000A98 RID: 2712
	protected float m_vertSpin_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000A99 RID: 2713
	protected float m_vertSpin_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04000A9A RID: 2714
	protected float m_vertSpin_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000A9B RID: 2715
	protected float m_vertSpin_AttackIntro_Delay;

	// Token: 0x04000A9C RID: 2716
	protected const float m_vertSpin_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000A9D RID: 2717
	protected const float m_vertSpin_AttackHold_Delay = 0f;

	// Token: 0x04000A9E RID: 2718
	protected float m_vertSpin_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000A9F RID: 2719
	protected const float m_vertSpin_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000AA0 RID: 2720
	protected const float m_vertSpin_Exit_ForceIdle = 0.5f;

	// Token: 0x04000AA1 RID: 2721
	protected float m_vertSpin_Exit_AttackCD = 3f;

	// Token: 0x04000AA2 RID: 2722
	protected float m_vertSpin_Attack_TurnSpeed;

	// Token: 0x04000AA3 RID: 2723
	protected float m_shockwave_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000AA4 RID: 2724
	protected float m_shockwave_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000AA5 RID: 2725
	protected float m_shockwave_TellIntroAndHold_Delay = 1f;

	// Token: 0x04000AA6 RID: 2726
	protected float m_shockwave_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000AA7 RID: 2727
	protected float m_shockwave_AttackIntro_Delay;

	// Token: 0x04000AA8 RID: 2728
	protected const float m_shockwave_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000AA9 RID: 2729
	protected const float m_shockwave_AttackHold_Delay = 1f;

	// Token: 0x04000AAA RID: 2730
	protected float m_shockwave_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000AAB RID: 2731
	protected const float m_shockwave_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000AAC RID: 2732
	protected const float m_shockwave_Exit_ForceIdle = 1.25f;

	// Token: 0x04000AAD RID: 2733
	protected float m_shockwave_Exit_AttackCD;

	// Token: 0x04000AAE RID: 2734
	protected float m_shockwaveAtPlayer_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000AAF RID: 2735
	protected float m_shockwaveAtPlayer_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000AB0 RID: 2736
	protected float m_shockwaveAtPlayer_TellIntroAndHold_Delay = 1.45f;

	// Token: 0x04000AB1 RID: 2737
	protected float m_shockwaveAtPlayer_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000AB2 RID: 2738
	protected float m_shockwaveAtPlayer_AttackIntro_Delay;

	// Token: 0x04000AB3 RID: 2739
	protected const float m_shockwaveAtPlayer_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000AB4 RID: 2740
	protected const float m_shockwaveAtPlayer_AttackHold_Delay = 1.55f;

	// Token: 0x04000AB5 RID: 2741
	protected float m_shockwaveAtPlayer_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000AB6 RID: 2742
	protected const float m_shockwaveAtPlayer_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000AB7 RID: 2743
	protected const float m_shockwaveAtPlayer_Exit_ForceIdle = 1.25f;

	// Token: 0x04000AB8 RID: 2744
	protected float m_shockwaveAtPlayer_Exit_AttackCD = 4f;

	// Token: 0x04000AB9 RID: 2745
	protected const string SHOCKWAVE_AT_PLAYER_PROJECTILE = "FlyingHammerWhirlwindProjectile";

	// Token: 0x04000ABA RID: 2746
	protected const string SHOCKWAVE_WARNING_AT_PLAYER_PROJECTILE = "FlyingHammerWarningWhirlwindProjectile";
}
