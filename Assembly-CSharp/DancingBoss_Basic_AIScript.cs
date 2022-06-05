using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class DancingBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x17000110 RID: 272
	// (get) Token: 0x060003ED RID: 1005 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_advancedAttacks
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x00004A12 File Offset: 0x00002C12
	private void Awake()
	{
		this.m_onBossHit = new Action<object, HealthChangeEventArgs>(this.OnBossHit);
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x060003EF RID: 1007 RVA: 0x00004A26 File Offset: 0x00002C26
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.5f, 1f);
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00004A37 File Offset: 0x00002C37
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1.75f, 2.5f);
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x060003F1 RID: 1009 RVA: 0x00004A37 File Offset: 0x00002C37
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1.75f, 2.5f);
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00004A48 File Offset: 0x00002C48
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00004A48 File Offset: 0x00002C48
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x00054820 File Offset: 0x00052A20
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"DancingBossWaveBoltProjectile",
			"DancingBossWaveBounceProjectile",
			"DancingBossWavePassBoltProjectile",
			"DancingBossBoltProjectile",
			"DancingBossBounceBoltProjectile",
			"DancingBossSummonProjectile",
			"SpellSwordShoutWarningProjectile",
			"DancingBossBounceRotateProjectile",
			"DancingBossStandingBoltProjectile",
			"DancingBossVoidProjectile"
		};
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_verticalWave_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_verticalWave_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_verticalWave_TellIntroAndHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_verticalWave_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_verticalWave_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x060003FA RID: 1018 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_verticalWave_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_verticalWave_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x060003FC RID: 1020 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_verticalWave_AttackHoldExit_Delay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_verticalWave_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x060003FE RID: 1022 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_verticalWave_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x060003FF RID: 1023 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_verticalWave_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000400 RID: 1024 RVA: 0x00003C70 File Offset: 0x00001E70
	protected virtual float m_verticalWave_AttackExit_CooldownDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000401 RID: 1025 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_wave_SummoningCircleDelay
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000402 RID: 1026 RVA: 0x00004A59 File Offset: 0x00002C59
	protected virtual int m_verticalWave_ProjectileCount
	{
		get
		{
			return 17;
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000403 RID: 1027 RVA: 0x00004A59 File Offset: 0x00002C59
	protected virtual int m_verticalWave_BounceProjectileCount
	{
		get
		{
			return 17;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000404 RID: 1028 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_verticalWave_ProjectileSpeedMod
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000405 RID: 1029 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_verticalWave_Pause_While_Attacking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x00004A5D File Offset: 0x00002C5D
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator VerticalBulletWave()
	{
		this.ToDo("*** V Bullet Wave ***");
		if (this.m_verticalWave_Pause_While_Attacking)
		{
			this.StopAndFaceTarget();
		}
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("SummonBulletsVertical_Tell_Intro", this.m_verticalWave_TellIntro_AnimSpeed, "SummonBulletsVertical_Tell_Hold", this.m_verticalWave_TellHold_AnimSpeed, this.m_verticalWave_TellIntroAndHold_Duration);
		yield return this.Default_Animation("SummonBulletsVertical_Attack_Intro", this.m_verticalWave_AttackIntro_AnimSpeed, this.m_verticalWave_AttackIntro_Delay, true);
		yield return this.Default_Animation("SummonBulletsVertical_Attack_Hold", this.m_verticalWave_AttackHold_AnimSpeed, this.m_verticalWave_AttackHold_Delay, false);
		RoomSide randomSide = DancingBoss_Basic_AIScript.GetRandomSide(new RoomSide[]
		{
			RoomSide.Bottom,
			RoomSide.Left,
			RoomSide.Right
		});
		bool isVerticalWave = false;
		if (randomSide == RoomSide.Bottom)
		{
			isVerticalWave = true;
		}
		this.FireWave(randomSide, this.m_verticalWave_ProjectileCount, this.m_verticalWave_BounceProjectileCount, (float)this.m_verticalWave_ProjectileSpeedMod, false, isVerticalWave);
		if (this.m_verticalWave_AttackHoldExit_Delay > 0f)
		{
			yield return base.Wait(this.m_verticalWave_AttackHoldExit_Delay, false);
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("SummonBulletsVertical_Exit", this.m_verticalWave_AttackExit_AnimSpeed, this.m_verticalWave_AttackExit_Delay, true);
		float idleDuration = this.m_verticalWave_AttackExit_IdleDuration;
		if (!this.m_verticalWave_Pause_While_Attacking)
		{
			idleDuration = 0f;
		}
		yield return this.Default_Attack_Cooldown(idleDuration, this.m_verticalWave_AttackExit_CooldownDuration);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spread_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000408 RID: 1032 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spread_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000409 RID: 1033 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spread_TellIntroAndHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x0600040A RID: 1034 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spread_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x0600040B RID: 1035 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spread_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x0600040C RID: 1036 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spread_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x0600040D RID: 1037 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spread_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x0600040E RID: 1038 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_spread_AttackHoldExit_Delay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x0600040F RID: 1039 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spread_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000410 RID: 1040 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spread_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000411 RID: 1041 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spread_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000412 RID: 1042 RVA: 0x00003C70 File Offset: 0x00001E70
	protected virtual float m_spread_AttackExit_CooldownDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06000413 RID: 1043 RVA: 0x00004A59 File Offset: 0x00002C59
	protected virtual int m_spread_BasicProjectileCount
	{
		get
		{
			return 17;
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000414 RID: 1044 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int m_spread_BounceProjectileCount
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000415 RID: 1045 RVA: 0x00004A6C File Offset: 0x00002C6C
	protected virtual float m_spread_ProjectileSpeedMod
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000416 RID: 1046 RVA: 0x00004520 File Offset: 0x00002720
	protected virtual float m_spread_TimeBetweenAttacks
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000417 RID: 1047 RVA: 0x00004A07 File Offset: 0x00002C07
	protected virtual int m_spread_NumberOfAttacks
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000418 RID: 1048 RVA: 0x00004A73 File Offset: 0x00002C73
	protected virtual float m_spread_SpaceBetweenAttacks
	{
		get
		{
			return 6.25f;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000419 RID: 1049 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_spread_summonProjectilesAtPlayerPosition
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x0600041A RID: 1050 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_spread_Pause_While_Attacking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00004A7A File Offset: 0x00002C7A
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator SpreadAttack()
	{
		this.ToDo("*** Spread Attack ***");
		if (this.m_spread_Pause_While_Attacking)
		{
			this.StopAndFaceTarget();
		}
		base.EnemyController.LockFlip = true;
		RoomSide side = DancingBoss_Basic_AIScript.GetRandomSide(new RoomSide[]
		{
			RoomSide.Bottom,
			RoomSide.Left
		});
		this.SpreadIntroAnimStartRelay.Dispatch();
		yield return this.Default_TellIntroAndLoop("CircularBurst_Tell_Intro", this.m_spread_TellIntro_AnimSpeed, "CircularBurst_Tell_Hold", this.m_spread_TellHold_AnimSpeed, this.m_spread_TellIntroAndHold_Duration);
		this.SpreadIntroAnimCompleteRelay.Dispatch();
		this.SpreadAttackAnimStartRelay.Dispatch();
		yield return this.Default_Animation("CircularBurst_Attack_Intro", this.m_spread_AttackIntro_AnimSpeed, this.m_spread_AttackIntro_Delay, true);
		yield return this.Default_Animation("CircularBurst_Attack_Hold", this.m_spread_AttackHold_AnimSpeed, this.m_spread_AttackHold_Delay, false);
		this.SpreadAttackAnimCompleteRelay.Dispatch();
		Vector3 originPosition = Vector3.zero;
		if (this.m_spread_summonProjectilesAtPlayerPosition)
		{
			originPosition = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		}
		Vector3 vector = originPosition;
		this.SpreadAttackStartRelay.Dispatch(base.EnemyController.Midpoint + vector);
		if (side == RoomSide.Bottom)
		{
			this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 90f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
			this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 270f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
			if (this.m_advancedAttacks)
			{
			}
		}
		else
		{
			this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 0f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
			this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 180f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
			bool advancedAttacks = this.m_advancedAttacks;
		}
		yield return base.Wait(this.m_spread_TimeBetweenAttacks, false);
		int num;
		for (int i = 1; i < this.m_spread_NumberOfAttacks; i = num + 1)
		{
			if (side == RoomSide.Bottom)
			{
				vector = originPosition;
				vector.x = originPosition.x - (float)i * this.m_spread_SpaceBetweenAttacks;
				this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 90f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
				this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 270f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
				vector.x = originPosition.x + (float)i * this.m_spread_SpaceBetweenAttacks;
				this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 90f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
				this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 270f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
				if (this.m_advancedAttacks && i == 4)
				{
					vector = Vector3.zero;
					this.SummonProjectile("DancingBossSummonProjectile", vector, false, 90f, this.m_bomb_ProjectileSpeedMod, true, true, true, true);
				}
			}
			else
			{
				vector = originPosition;
				vector.y = originPosition.y - (float)i * this.m_spread_SpaceBetweenAttacks;
				this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 0f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
				this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 180f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
				vector.y = originPosition.y + (float)i * this.m_spread_SpaceBetweenAttacks;
				this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 0f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
				this.SummonProjectile("DancingBossWaveBoltProjectile", vector, false, 180f, this.m_spread_ProjectileSpeedMod, false, true, true, true);
				if (this.m_advancedAttacks && i == 4)
				{
					vector = Vector3.zero;
					this.SummonProjectile("DancingBossSummonProjectile", vector, false, 90f, this.m_bomb_ProjectileSpeedMod, true, true, true, true);
				}
			}
			yield return base.Wait(this.m_spread_TimeBetweenAttacks, false);
			num = i;
		}
		this.SpreadAttackCompleteRelay.Dispatch();
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("CircularBurst_Exit", this.m_spread_AttackExit_AnimSpeed, this.m_spread_AttackExit_Delay, true);
		float idleDuration = this.m_verticalWave_AttackExit_IdleDuration;
		if (!this.m_spread_Pause_While_Attacking)
		{
			idleDuration = 0f;
		}
		yield return this.Default_Attack_Cooldown(idleDuration, this.m_spread_AttackExit_CooldownDuration);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		this.ResetSummoningPointPositions(side);
		yield break;
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x0600041C RID: 1052 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_horizontalWave_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x0600041D RID: 1053 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_horizontalWave_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x0600041E RID: 1054 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_horizontalWave_TellIntroAndHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x0600041F RID: 1055 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_horizontalWave_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000420 RID: 1056 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_horizontalWave_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06000421 RID: 1057 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_horizontalWave_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000422 RID: 1058 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_horizontalWave_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000423 RID: 1059 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_horizontalWave_AttackHoldExit_Delay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000424 RID: 1060 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_horizontalWave_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000425 RID: 1061 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_horizontalWave_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000426 RID: 1062 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_horizontalWave_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000427 RID: 1063 RVA: 0x00003C70 File Offset: 0x00001E70
	protected virtual float m_horizontalWave_AttackExit_CooldownDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000428 RID: 1064 RVA: 0x00004A89 File Offset: 0x00002C89
	protected virtual int m_horizontalWave_ProjectileCount
	{
		get
		{
			return 11;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000429 RID: 1065 RVA: 0x00004A89 File Offset: 0x00002C89
	protected virtual int m_horizontalWave_BounceProjectileCount
	{
		get
		{
			return 11;
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x0600042A RID: 1066 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_horizontalWave_ProjectileSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x0600042B RID: 1067 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_horizontalWave_Pause_While_Attacking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x0600042C RID: 1068 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_bomb_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x0600042D RID: 1069 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_bomb_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x0600042E RID: 1070 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_bomb_TellIntroAndHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x0600042F RID: 1071 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_bomb_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000430 RID: 1072 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_bomb_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000431 RID: 1073 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_bomb_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000432 RID: 1074 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_bomb_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000433 RID: 1075 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_bomb_AttackHoldExit_Delay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000434 RID: 1076 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_bomb_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000435 RID: 1077 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_bomb_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000436 RID: 1078 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_bomb_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000437 RID: 1079 RVA: 0x00003C70 File Offset: 0x00001E70
	protected virtual float m_bomb_AttackExit_CooldownDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000438 RID: 1080 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_bomb_summonProjectilesAtPlayerPosition
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000439 RID: 1081 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected virtual int m_bomb_projectileAmount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x0600043A RID: 1082 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_bomb_projectileDelay
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x0600043B RID: 1083 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_bomb_ProjectileSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x0600043C RID: 1084 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_bomb_Pause_While_Attacking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00004A97 File Offset: 0x00002C97
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator BombAttack()
	{
		this.ToDo("*** Bomb Attack ***");
		if (this.m_bomb_Pause_While_Attacking)
		{
			this.StopAndFaceTarget();
		}
		base.EnemyController.LockFlip = true;
		DancingBoss_Basic_AIScript.GetRandomSide(new RoomSide[]
		{
			RoomSide.Left,
			RoomSide.Right
		});
		yield return this.Default_TellIntroAndLoop("SummonIce_Tell_Intro", this.m_bomb_TellIntro_AnimSpeed, "SummonIce_Tell_Hold", this.m_bomb_TellHold_AnimSpeed, this.m_bomb_TellIntroAndHold_Duration);
		yield return this.Default_Animation("SummonIce_Attack_Intro", this.m_bomb_AttackIntro_AnimSpeed, this.m_bomb_AttackIntro_Delay, true);
		yield return this.Default_Animation("SummonIce_Attack_Hold", this.m_bomb_AttackHold_AnimSpeed, this.m_bomb_AttackHold_Delay, false);
		Vector3 vector = Vector3.zero;
		if (this.m_bomb_summonProjectilesAtPlayerPosition)
		{
			vector = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		}
		Vector3 summonPosition = vector;
		int num;
		for (int i = 0; i < this.m_bomb_projectileAmount; i = num + 1)
		{
			this.SummonProjectile("DancingBossSummonProjectile", summonPosition, false, 0f, this.m_bomb_ProjectileSpeedMod, true, true, true, true);
			if (this.m_bomb_projectileDelay > 0f)
			{
				yield return base.Wait(this.m_bomb_projectileDelay, false);
			}
			num = i;
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("SummonIce_Exit", this.m_bomb_AttackExit_AnimSpeed, this.m_bomb_AttackExit_Delay, true);
		float bomb_AttackExit_IdleDuration = this.m_bomb_AttackExit_IdleDuration;
		bool bomb_Pause_While_Attacking = this.m_bomb_Pause_While_Attacking;
		yield return this.Default_Attack_Cooldown(this.m_bomb_AttackExit_IdleDuration, this.m_bomb_AttackExit_CooldownDuration);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x0600043E RID: 1086 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_summonShots_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x0600043F RID: 1087 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_summonShots_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000440 RID: 1088 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_summonShots_TellIntroAndHold_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000441 RID: 1089 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_summonShots_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000442 RID: 1090 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_summonShots_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000443 RID: 1091 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_summonShots_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000444 RID: 1092 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_summonShots_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000445 RID: 1093 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_summonShots_AttackHoldExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000446 RID: 1094 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_summonShots_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000447 RID: 1095 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_summonShots_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000448 RID: 1096 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_summonShots_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000449 RID: 1097 RVA: 0x00003C70 File Offset: 0x00001E70
	protected virtual float m_summonShots_AttackExit_CooldownDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x0600044A RID: 1098 RVA: 0x00004A07 File Offset: 0x00002C07
	protected virtual int m_summonShots_ProjectileCount
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x0600044B RID: 1099 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_summonShots_ProjectileSpeedMod
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x00004AA6 File Offset: 0x00002CA6
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator SummonShots()
	{
		this.ToDo("*** Summon Shots ***");
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		this.SummonShotsTellStartRelay.Dispatch();
		yield return this.Default_TellIntroAndLoop("CircularBurst_Tell_Intro", this.m_summonShots_TellIntro_AnimSpeed, "CircularBurst_Tell_Hold", this.m_summonShots_TellHold_AnimSpeed, this.m_summonShots_TellIntroAndHold_Duration);
		this.SummonShotsTellCompleteRelay.Dispatch();
		this.SummonShotsAttackStartRelay.Dispatch();
		yield return this.Default_Animation("CircularBurst_Attack_Intro", this.m_summonShots_AttackIntro_AnimSpeed, this.m_summonShots_AttackIntro_Delay, true);
		yield return this.Default_Animation("CircularBurst_Attack_Hold", this.m_summonShots_AttackHold_AnimSpeed, this.m_summonShots_AttackHold_Delay, false);
		for (int i = 0; i < this.m_summonShots_ProjectileCount; i++)
		{
			float x = UnityEngine.Random.Range(base.EnemyController.Room.Bounds.min.x, base.EnemyController.Room.Bounds.max.x);
			float y = UnityEngine.Random.Range(base.EnemyController.Room.Bounds.min.y, base.EnemyController.Room.Bounds.max.y);
			Vector2 v = new Vector2(x, y);
			this.m_summonProjectileSummonPoints[i].transform.position = v;
		}
		Vector3 vector = Vector3.zero;
		vector = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		float num = 3f;
		Vector3 v2 = vector;
		v2.y = vector.y + num;
		this.SummonProjectile("DancingBossStandingBoltProjectile", v2, false, 90f, 0f, false, true, true, true);
		v2 = vector;
		v2.y = vector.y + num * 2f;
		v2.x = vector.x - num;
		this.SummonProjectile("DancingBossStandingBoltProjectile", v2, false, 90f, 0f, false, true, true, true);
		v2 = vector;
		v2.y = vector.y + num * 2f;
		v2.x = vector.x + num;
		this.SummonProjectile("DancingBossStandingBoltProjectile", v2, false, 90f, 0f, false, true, true, true);
		v2 = vector;
		v2.x = vector.x - num * 2f;
		v2.y = vector.y + num;
		this.SummonProjectile("DancingBossStandingBoltProjectile", v2, false, 180f, 0f, false, true, true, true);
		v2.x = vector.x - num * 2f;
		v2.y = vector.y + 0f;
		this.SummonProjectile("DancingBossStandingBoltProjectile", v2, false, 180f, 0f, false, true, true, true);
		v2 = vector;
		v2.x = vector.x + num * 2f;
		v2.y = vector.y + num;
		this.SummonProjectile("DancingBossStandingBoltProjectile", v2, false, 180f, 0f, false, true, true, true);
		v2.x = vector.x + num * 2f;
		v2.y = vector.y + 0f;
		this.SummonProjectile("DancingBossStandingBoltProjectile", v2, false, 180f, 0f, false, true, true, true);
		v2.x = vector.x - num;
		v2.y = vector.y - num;
		this.SummonProjectile("DancingBossStandingBoltProjectile", v2, false, 180f, 0f, false, true, true, true);
		v2.x = vector.x + num;
		v2.y = vector.y - num;
		this.SummonProjectile("DancingBossStandingBoltProjectile", v2, false, 180f, 0f, false, true, true, true);
		v2 = vector;
		v2.y = vector.y - num * 2f;
		this.SummonProjectile("DancingBossStandingBoltProjectile", v2, false, 90f, 0f, false, true, true, true);
		if (this.m_summonShots_AttackHoldExit_Delay > 0f)
		{
			yield return base.Wait(this.m_summonShots_AttackHoldExit_Delay, false);
		}
		this.SummonShotsAttackCompleteRelay.Dispatch();
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("CircularBurst_Exit", this.m_summonShots_AttackExit_AnimSpeed, this.m_summonShots_AttackExit_Delay, true);
		yield return this.Default_Attack_Cooldown(this.m_summonShots_AttackExit_IdleDuration, this.m_summonShots_AttackExit_CooldownDuration);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x0600044D RID: 1101 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x0600044E RID: 1102 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x0600044F RID: 1103 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_TellIntroAndHold_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000450 RID: 1104 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected virtual float m_dash_AttackIntro_AnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06000451 RID: 1105 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000452 RID: 1106 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06000453 RID: 1107 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06000454 RID: 1108 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06000455 RID: 1109 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06000456 RID: 1110 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06000457 RID: 1111 RVA: 0x00003CC4 File Offset: 0x00001EC4
	protected virtual float m_dash_AttackExit_CooldownDuration
	{
		get
		{
			return 12f;
		}
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06000458 RID: 1112 RVA: 0x00003D9A File Offset: 0x00001F9A
	protected virtual float m_dash_Attack_ForwardSpeed
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000459 RID: 1113 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x0600045A RID: 1114 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_dash_Attack_Pause_While_Attacking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00004ABC File Offset: 0x00002CBC
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		this.ToDo("*** Dash ***");
		if (this.m_dash_Attack_Pause_While_Attacking)
		{
			this.StopAndFaceTarget();
		}
		yield return this.Default_TellIntroAndLoop("Dash_Tell_Intro", this.m_dash_TellIntro_AnimSpeed, "Dash_Tell_Hold", this.m_dash_TellHold_AnimSpeed, this.m_dash_TellIntroAndHold_Duration);
		base.SetAttackingWithContactDamage(true, 0.1f);
		base.EnemyController.Heading = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		if (this.m_dash_Attack_Pause_While_Attacking)
		{
			base.EnemyController.LockFlip = true;
			base.EnemyController.FollowTarget = false;
		}
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dash_AttackIntro_AnimSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dash_AttackHold_AnimSpeed, this.m_dash_AttackHold_Delay, false);
		if (!this.m_dash_Attack_Pause_While_Attacking)
		{
			base.EnemyController.LockFlip = true;
			base.EnemyController.FollowTarget = false;
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
		Vector2 vector = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		Vector2 vector2 = this.m_dash_Attack_ForwardSpeed * vector.normalized;
		base.EnemyController.SetVelocity(vector2.x, vector2.y, false);
		if (this.m_dash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_Duration, false);
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Animation("Dash_Exit", this.m_dash_AttackExit_AnimSpeed, this.m_dash_AttackExit_Delay, true);
		if (!this.m_advancedAttacks)
		{
			this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 0f, 1.1f, true, true, true);
			this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 45f, 1.1f, true, true, true);
			this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 90f, 1.1f, true, true, true);
			this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 135f, 1.1f, true, true, true);
			this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 180f, 1.1f, true, true, true);
			this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 225f, 1.1f, true, true, true);
			this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 270f, 1.1f, true, true, true);
			this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 315f, 1.1f, true, true, true);
		}
		else
		{
			float speedmod = 0.35f;
			for (int i = 0; i < 3; i++)
			{
				this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 0f, 1.1f + (float)i * speedmod, true, true, true);
				this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 45f, 1.1f + (float)i * speedmod, true, true, true);
				this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 90f, 1.1f + (float)i * speedmod, true, true, true);
				this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 135f, 1.1f + (float)i * speedmod, true, true, true);
				this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 180f, 1.1f + (float)i * speedmod, true, true, true);
				this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 225f, 1.1f + (float)i * speedmod, true, true, true);
				this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 270f, 1.1f + (float)i * speedmod, true, true, true);
				this.FireProjectile("DancingBossWaveBoltProjectile", 0, false, 315f, 1.1f + (float)i * speedmod, true, true, true);
			}
			yield return base.Wait(0.5f, false);
			this.FireProjectile("DancingBossVoidProjectile", 0, false, 22.5f, 1.1f + 4f * speedmod, true, true, true);
			this.FireProjectile("DancingBossVoidProjectile", 0, false, 67.5f, 1.1f + 4f * speedmod, true, true, true);
			this.FireProjectile("DancingBossVoidProjectile", 0, false, 112.5f, 1.1f + 4f * speedmod, true, true, true);
			this.FireProjectile("DancingBossVoidProjectile", 0, false, 157.5f, 1.1f + 4f * speedmod, true, true, true);
			this.FireProjectile("DancingBossVoidProjectile", 0, false, 202.5f, 1.1f + 4f * speedmod, true, true, true);
			this.FireProjectile("DancingBossVoidProjectile", 0, false, 247.5f, 1.1f + 4f * speedmod, true, true, true);
			this.FireProjectile("DancingBossVoidProjectile", 0, false, 292.5f, 1.1f + 4f * speedmod, true, true, true);
			this.FireProjectile("DancingBossVoidProjectile", 0, false, 337.5f, 1.1f + 4f * speedmod, true, true, true);
		}
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		float dash_AttackExit_IdleDuration = this.m_dash_AttackExit_IdleDuration;
		yield return this.Default_Attack_Cooldown(dash_AttackExit_IdleDuration, this.m_dash_AttackExit_CooldownDuration);
		yield break;
	}

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x0600045C RID: 1116 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_modeShift01_Downed_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x0600045D RID: 1117 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_modeShift01_Downed_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x0600045E RID: 1118 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_modeShift01_GetUp_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x0600045F RID: 1119 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_modeShift01_GetUp_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000460 RID: 1120 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_modeShift01_Warning_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000461 RID: 1121 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_modeShift01_Exit_AnimSpeed
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000462 RID: 1122 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_modeShift01_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06000463 RID: 1123 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_modeShift01_Exit_IdleDuration
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06000464 RID: 1124 RVA: 0x00004ACB File Offset: 0x00002CCB
	protected virtual float m_modeShift01_AttackCD
	{
		get
		{
			return 999999f;
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06000465 RID: 1125 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float m_modeShift01_Damage_Mod
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06000466 RID: 1126 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_firstModeShiftHealthMod
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x00004AD2 File Offset: 0x00002CD2
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[WanderLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Mode_Shift_01()
	{
		base.RemoveStatusEffects(false);
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = true;
		yield return base.DeathAnim();
		this.ToDo("*** MODE SHIFT 01 ***");
		MusicManager.SetBossEncounterParam(0.51f);
		base.EnemyController.LockFlip = true;
		base.SetVelocity(0f, 0f, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift01_Damage_Mod;
		this.ToDo("*** DOWNED ***");
		yield return this.Default_Animation("ModeShift_Intro", this.m_modeShift01_Downed_AnimSpeed, this.m_modeShift01_Downed_Delay, true);
		this.ToDo("*** GETTING UP ***");
		yield return this.ChangeAnimationState("ModeShift_Scream_Intro");
		yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		this.ToDo("*** WARNING ***");
		yield return this.ChangeAnimationState("ModeShift_Scream_Hold");
		yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		if (this.m_modeShift01_Warning_Delay > 0f)
		{
			yield return base.Wait(this.m_modeShift01_Warning_Delay, false);
		}
		this.ToDo("*** SUMMON SENTRYS ***");
		yield return this.SummonSentries();
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.ChangeAnimationState("ModeShift_Scream_Exit");
		yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_modeShift01_Exit_IdleDuration, this.m_modeShift01_AttackCD);
		yield break;
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06000468 RID: 1128 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_modeShift02_Downed_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06000469 RID: 1129 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_modeShift02_Downed_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x0600046A RID: 1130 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_modeShift02_GetUp_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x0600046B RID: 1131 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_modeShift02_GetUp_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x0600046C RID: 1132 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_modeShift02_Warning_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x0600046D RID: 1133 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_modeShift02_Exit_AnimSpeed
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x0600046E RID: 1134 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_modeShift02_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x0600046F RID: 1135 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_modeShift02_Exit_IdleDuration
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x06000470 RID: 1136 RVA: 0x00004AE1 File Offset: 0x00002CE1
	protected virtual float m_modeShift02_AttackCD
	{
		get
		{
			return 99f;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x06000471 RID: 1137 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_finalDance_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06000472 RID: 1138 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_finalDance_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06000473 RID: 1139 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_finalDance_TellIntroAndHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06000474 RID: 1140 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_finalDance_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x06000475 RID: 1141 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_finalDance_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06000476 RID: 1142 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_finalDance_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06000477 RID: 1143 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_finalDance_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06000478 RID: 1144 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_finalDance_AttackHoldExit_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000479 RID: 1145 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_finalDance_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x0600047A RID: 1146 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_finalDance_AttackExit_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x0600047B RID: 1147 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_finalDance_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x0600047C RID: 1148 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_finalDance_AttackExit_CooldownDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x0600047D RID: 1149 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float m_modeShift02_Damage_Mod
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x0600047E RID: 1150 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_secondModeShiftHealthMod
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x0600047F RID: 1151 RVA: 0x00003C70 File Offset: 0x00001E70
	protected virtual float m_sentryRestDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000480 RID: 1152 RVA: 0x00004AE8 File Offset: 0x00002CE8
	public List<Sentry_Hazard> SentryHazards
	{
		get
		{
			return this.m_sentryHazards;
		}
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00004AF0 File Offset: 0x00002CF0
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[WanderLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Mode_Shift_02()
	{
		this.ToDo("*** MODE SHIFT 02 ***");
		base.EnemyController.LockFlip = true;
		base.SetVelocity(0f, 0f, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift01_Damage_Mod;
		this.ToDo("*** DOWNED ***");
		yield return this.Default_Animation("Stunned", this.m_modeShift02_Downed_AnimSpeed, this.m_modeShift02_Downed_Delay, true);
		this.ToDo("*** GETTING UP ***");
		yield return this.Default_Animation("Neutral", this.m_modeShift02_GetUp_AnimSpeed, this.m_modeShift02_GetUp_Delay, true);
		this.ToDo("*** FLY TO CENTRE OF ROOM ***");
		base.EnemyController.Target = this.m_centerOfRoom;
		base.EnemyController.FollowTarget = true;
		base.EnemyController.FollowOffset = Vector3.zero;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "WalkTowards";
		float sqrMagnitudeToCentreOfRoom = (base.EnemyController.Target.transform.position - base.EnemyController.transform.position).sqrMagnitude;
		while (sqrMagnitudeToCentreOfRoom > 0.5f)
		{
			sqrMagnitudeToCentreOfRoom = (base.EnemyController.Target.transform.position - base.EnemyController.transform.position).sqrMagnitude;
			yield return null;
		}
		base.EnemyController.Target = PlayerManager.GetPlayerController().gameObject;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.SetVelocity(0f, 0f, false);
		this.ToDo("*** PERFORM FINAL DANCE ***");
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_finalDance_TellIntro_AnimSpeed, "Shoot_Tell_Hold", this.m_finalDance_TellHold_AnimSpeed, this.m_finalDance_TellIntroAndHold_Duration);
		base.EnemyController.LockFlip = true;
		base.EnemyController.FollowTarget = false;
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_finalDance_AttackIntro_AnimSpeed, this.m_finalDance_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_finalDance_AttackHold_AnimSpeed, this.m_finalDance_AttackHold_Delay, false);
		if (this.m_finalDance_AttackHoldExit_Delay > 0f)
		{
			yield return base.Wait(this.m_finalDance_AttackHoldExit_Delay, false);
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Animation("Neutral", this.m_finalDance_AttackExit_AnimSpeed, this.m_finalDance_AttackExit_Delay, true);
		this.ToDo("*** WARNING ***");
		this.m_modeShift02WarningProjectile = this.FireProjectile("SpellSwordShoutWarningProjectile", 0, true, 0f, 1f, true, true, true);
		if (this.m_modeShift02_Warning_Delay > 0f)
		{
			yield return base.Wait(this.m_modeShift02_Warning_Delay, false);
		}
		base.StopProjectile(ref this.m_modeShift02WarningProjectile);
		this.ToDo("*** SUMMON SENTRYS ***");
		yield return this.SummonSentries();
		this.ToDo("*** FIRE PROJECTILES ***");
		yield return this.FireProjectilesInRandomDirections();
		yield return base.Wait(9999999f, false);
		yield break;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00004AFF File Offset: 0x00002CFF
	private IEnumerator FireProjectilesInRandomDirections()
	{
		yield break;
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x00004B07 File Offset: 0x00002D07
	private IEnumerator SummonSentries()
	{
		this.SummonSentriesStartRelay.Dispatch();
		GameObject[] points = (from entry in this.m_sentryHazards
		select entry.gameObject).ToArray<GameObject>();
		yield return this.PlaySummoningEffect(points);
		for (int i = 0; i < this.m_sentryHazards.Count; i++)
		{
			this.m_sentryHazards[i].gameObject.SetActive(true);
			this.m_sentryHazards[i].StartInRestState(0f);
		}
		this.SummonSentriesCompleteRelay.Dispatch();
		yield break;
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00004B16 File Offset: 0x00002D16
	public override IEnumerator SpawnAnim()
	{
		yield return this.Default_Animation("Intro_Idle", this.m_spawn_Idle_AnimSpeed, this.m_spawn_Idle_Delay, true);
		MusicManager.PlayMusic(SongID.ForestBossBGM_Tettix_Naamah_180_BPM, false, false);
		yield return this.Default_Animation("Intro", this.m_spawn_Intro_AnimSpeed, this.m_spawn_Intro_Delay, true);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		yield break;
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00004B25 File Offset: 0x00002D25
	public override IEnumerator DeathAnim()
	{
		yield return base.DeathAnim();
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 0.5f);
		yield return this.Default_Animation("Death_Intro", this.m_death_Intro_AnimSpeed, this.m_death_Intro_Delay, true);
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		for (int i = 0; i < this.m_sentryHazards.Count; i++)
		{
			if (this.m_sentryHazards[i].isActiveAndEnabled)
			{
				this.m_sentryHazards[i].ActivateRestState();
			}
		}
		yield return base.Wait(0.5f, true);
		for (int j = 0; j < this.m_sentryHazards.Count; j++)
		{
			this.m_sentryHazards[j].gameObject.SetActive(false);
		}
		base.EnemyController.GetComponent<BossModeShiftController>().DissolveMaterials(this.m_death_Hold_Delay);
		yield return this.Default_Animation("Death_Loop", this.m_death_Hold_AnimSpeed, this.m_death_Hold_Delay, true);
		yield break;
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x00004B34 File Offset: 0x00002D34
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_modeShift01WarningProjectile);
		base.StopProjectile(ref this.m_modeShift02WarningProjectile);
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00004B54 File Offset: 0x00002D54
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.LogicController.DisableLogicActivationByDistance = true;
		base.EnemyController.Target = PlayerManager.GetPlayerController().gameObject;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0005488C File Offset: 0x00052A8C
	private void InitializeSentryHazards()
	{
		if (this.m_sentryHazards == null)
		{
			this.m_sentryHazards = new List<Sentry_Hazard>();
		}
		else
		{
			this.m_sentryHazards.Clear();
		}
		ISpawnController[] spawnControllers = base.EnemyController.Room.SpawnControllerManager.SpawnControllers;
		for (int i = 0; i < spawnControllers.Length; i++)
		{
			IHazardSpawnController hazardSpawnController = spawnControllers[i] as IHazardSpawnController;
			if (!hazardSpawnController.IsNativeNull() && hazardSpawnController.Type == HazardType.Sentry)
			{
				Sentry_Hazard sentry_Hazard = hazardSpawnController.Hazard as Sentry_Hazard;
				if (sentry_Hazard)
				{
					sentry_Hazard.SetRestDurationOverride(this.m_sentryRestDuration);
					sentry_Hazard.gameObject.SetActive(false);
					this.m_sentryHazards.Add(sentry_Hazard);
				}
			}
		}
		if (!this.m_centerOfRoom)
		{
			this.m_centerOfRoom = new GameObject("Center of Room");
		}
		this.m_centerOfRoom.transform.SetParent(base.EnemyController.Room.gameObject.transform);
		this.m_centerOfRoom.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x00004B8A File Offset: 0x00002D8A
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		base.EnemyController.HealthChangeRelay.AddListener(this.m_onBossHit, false);
		this.CreateSummoningPoints();
		this.InitializeSentryHazards();
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x00004BB6 File Offset: 0x00002DB6
	protected override void OnDisable()
	{
		base.OnDisable();
		if (base.EnemyController && base.EnemyController.HealthChangeRelay != null)
		{
			base.EnemyController.HealthChangeRelay.RemoveListener(this.m_onBossHit);
		}
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00004BEF File Offset: 0x00002DEF
	private void FireWave(RoomSide side, int basicProjectileAmount, int bounceProjectileAmount, float projectileSpeedMod, bool addSummoningDelay = true, bool isVerticalWave = false)
	{
		base.StartCoroutine(this.FireWaveCoroutine(side, basicProjectileAmount, bounceProjectileAmount, projectileSpeedMod, addSummoningDelay, isVerticalWave));
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00004C07 File Offset: 0x00002E07
	private IEnumerator FireWaveCoroutine(RoomSide side, int BasicProjectileAmount, int BounceProjectileAmount, float ProjectileSpeedMod, bool addSummoningDelay, bool isVerticalWave)
	{
		float projectileAngle = this.GetProjectileAngle(side);
		GameObject[] summoningPoints = this.GetSummoningPoints(side);
		int num = UnityEngine.Random.Range(0, BasicProjectileAmount - BounceProjectileAmount + 1);
		int num2 = 0;
		this.DispatchWaveRelay(side, summoningPoints, ProjectileSpeedMod);
		for (int i = 0; i < summoningPoints.Length; i++)
		{
			string projectileName = "DancingBossWaveBounceProjectile";
			if (i >= num && num2 < BounceProjectileAmount)
			{
				projectileName = "DancingBossWaveBounceProjectile";
				num2++;
			}
			GameObject summonPoint = summoningPoints[i];
			this.SummonProjectile(summonPoint, projectileName, false, projectileAngle, ProjectileSpeedMod, false, false, false, true);
			if (this.m_advancedAttacks && !isVerticalWave)
			{
				this.SummonProjectile(summonPoint, "DancingBossWavePassBoltProjectile", false, projectileAngle, ProjectileSpeedMod, false, false, false, true);
			}
		}
		yield break;
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x0005498C File Offset: 0x00052B8C
	private void DispatchWaveRelay(RoomSide side, GameObject[] summonPoints, float speedMod)
	{
		Vector2 zero = Vector2.zero;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			zero = new Vector2(summonPoints[0].transform.position.x, base.EnemyController.Room.Bounds.center.y);
		}
		else if (side == RoomSide.Top || side == RoomSide.Bottom)
		{
			zero = new Vector2(base.EnemyController.Room.Bounds.center.x, summonPoints[0].transform.position.y);
		}
		Projectile_RL projectile = ProjectileLibrary.GetProjectile("DancingBossWaveBoltProjectile");
		float speed = projectile.ProjectileData.Speed;
		float speed2 = (float)this.m_verticalWave_ProjectileSpeedMod * speed;
		float lifespan = projectile.Lifespan;
		if (this.m_waveEventArgs == null)
		{
			this.m_waveEventArgs = new DancingBossWaveFiredEventArgs(side, zero, speed2, lifespan);
		}
		else
		{
			this.m_waveEventArgs.Initialize(side, zero, speed2, lifespan);
		}
		this.WaveFiredRelay.Dispatch(this.m_waveEventArgs);
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00004C3B File Offset: 0x00002E3B
	private IEnumerator FireMultiWaveSpreadCoroutine(RoomSide side, int BasicProjectileAmount, int BounceProjectileAmount, float ProjectileSpeedMod, bool addSummoningDelay)
	{
		float angle = this.GetProjectileAngle(side);
		GameObject[] summonPoints = this.GetSummoningPoints(side);
		string projectileName = "DancingBossWaveBoltProjectile";
		int setBounceProjectileLocation = UnityEngine.Random.Range(0, BasicProjectileAmount - BounceProjectileAmount + 1);
		int bounceProjectilesSpawnedCounter = 0;
		int midPoint = Mathf.FloorToInt((float)summonPoints.Length / 2f);
		this.SummonProjectile(summonPoints[midPoint], projectileName, false, angle, ProjectileSpeedMod, true, true, true, true);
		yield return base.Wait(this.m_spread_TimeBetweenAttacks, false);
		int num;
		for (int i = 1; i < midPoint; i = num + 1)
		{
			projectileName = "DancingBossWaveBoltProjectile";
			if (i >= setBounceProjectileLocation && bounceProjectilesSpawnedCounter < BounceProjectileAmount)
			{
				projectileName = "DancingBossWaveBounceProjectile";
				num = bounceProjectilesSpawnedCounter;
				bounceProjectilesSpawnedCounter = num + 1;
			}
			GameObject summonPoint = summonPoints[summonPoints.Length / 2 - i];
			GameObject summonPoint2 = summonPoints[summonPoints.Length / 2 + i];
			this.SummonProjectile(summonPoint, projectileName, false, angle, ProjectileSpeedMod, true, true, true, true);
			this.SummonProjectile(summonPoint2, projectileName, false, angle, ProjectileSpeedMod, true, true, true, true);
			yield return base.Wait(this.m_spread_TimeBetweenAttacks, false);
			num = i;
		}
		yield break;
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00054A80 File Offset: 0x00052C80
	private void SummonProjectile(GameObject summonPoint, string projectileName, bool matchFacing, float angle, float speedMod, bool playSummonAudio = true, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		Vector2 offset = summonPoint.transform.position - base.EnemyController.transform.position;
		this.SummonProjectile(projectileName, offset, matchFacing, angle, speedMod, playSummonAudio, playSpawnAudio, playLifetimeAudio, playDeathAudio);
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00054AC8 File Offset: 0x00052CC8
	private void OnBossHit(object sender, HealthChangeEventArgs args)
	{
		if (this.m_currentMode >= 1 || base.EnemyController.IsDead || args.PrevHealthValue <= args.NewHealthValue)
		{
			return;
		}
		if (base.EnemyController.CurrentHealth <= (float)base.EnemyController.ActualMaxHealth * this.m_firstModeShiftHealthMod)
		{
			this.m_currentMode = 1;
			string forceExecuteLogicBlockName_OnceOnly = "Mode_Shift_01";
			base.LogicController.StopAllLogic(true);
			base.LogicController.ForceExecuteLogicBlockName_OnceOnly = forceExecuteLogicBlockName_OnceOnly;
			if (this.m_modeShiftEventArgs == null)
			{
				this.m_modeShiftEventArgs = new EnemyModeShiftEventArgs(base.EnemyController);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EnemyModeShift, this, this.m_modeShiftEventArgs);
			base.EnemyController.HealthChangeRelay.RemoveListener(new Action<object, HealthChangeEventArgs>(this.OnBossHit));
		}
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00054B84 File Offset: 0x00052D84
	private static RoomSide GetRandomSide(params RoomSide[] sides)
	{
		int num = UnityEngine.Random.Range(0, sides.Length);
		return sides[num];
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00054BA0 File Offset: 0x00052DA0
	private float GetProjectileAngle(RoomSide side)
	{
		float result;
		if (side == RoomSide.Left)
		{
			result = 0f;
		}
		else if (side == RoomSide.Right)
		{
			result = 180f;
		}
		else if (side == RoomSide.Top)
		{
			result = 270f;
		}
		else
		{
			result = 90f;
		}
		return result;
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00054BE0 File Offset: 0x00052DE0
	private GameObject[] GetSummoningPoints(RoomSide side)
	{
		GameObject[] result = null;
		switch (side)
		{
		case RoomSide.Top:
			result = this.m_topVerticalProjectileSummonPoints;
			break;
		case RoomSide.Bottom:
			result = this.m_bottomVerticalProjectileSummonPoints;
			break;
		case RoomSide.Left:
			result = this.m_leftHorizontalProjectileSummonPoints;
			break;
		case RoomSide.Right:
			result = this.m_rightHorizontalProjectileSummonPoints;
			break;
		}
		return result;
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00004C67 File Offset: 0x00002E67
	private float GetSummonEffectDuration()
	{
		return 1.2f;
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x00004C6E File Offset: 0x00002E6E
	protected IEnumerator PlaySummoningEffect(GameObject[] points)
	{
		float summonEffectDuration = this.GetSummonEffectDuration();
		for (int i = 0; i < points.Length; i++)
		{
			Vector3 position = points[i].transform.position;
			EffectManager.PlayEffect(null, null, "SpellCircle_Effect", position, summonEffectDuration, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
		yield return base.Wait(summonEffectDuration, false);
		yield break;
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00054C2C File Offset: 0x00052E2C
	private void CreateSummoningPoints()
	{
		this.m_areSummoningPointsInitialized = true;
		Vector2Int unitDimensions = (base.EnemyController.Room as Room).UnitDimensions;
		float distanceBetweenProjectiles = this.GetDistanceBetweenProjectiles(RoomSide.Left);
		this.m_leftHorizontalProjectileSummonPoints = new GameObject[this.m_horizontalWave_ProjectileCount];
		this.m_rightHorizontalProjectileSummonPoints = new GameObject[this.m_horizontalWave_ProjectileCount];
		Vector2 initialPosition = this.GetInitialPosition(RoomSide.Left);
		Vector2 initialPosition2 = this.GetInitialPosition(RoomSide.Right);
		for (int i = 0; i < this.m_horizontalWave_ProjectileCount; i++)
		{
			Vector2 position = initialPosition + (float)i * distanceBetweenProjectiles * Vector2.up;
			GameObject gameObject = this.CreateProjectileSummonPoint(RoomSide.Left, position);
			this.m_leftHorizontalProjectileSummonPoints[i] = gameObject;
			Vector2 position2 = initialPosition2 + (float)i * distanceBetweenProjectiles * Vector2.up;
			GameObject gameObject2 = this.CreateProjectileSummonPoint(RoomSide.Right, position2);
			this.m_rightHorizontalProjectileSummonPoints[i] = gameObject2;
		}
		float distanceBetweenProjectiles2 = this.GetDistanceBetweenProjectiles(RoomSide.Top);
		this.m_topVerticalProjectileSummonPoints = new GameObject[this.m_verticalWave_ProjectileCount];
		this.m_bottomVerticalProjectileSummonPoints = new GameObject[this.m_verticalWave_ProjectileCount];
		Vector2 initialPosition3 = this.GetInitialPosition(RoomSide.Top);
		Vector2 initialPosition4 = this.GetInitialPosition(RoomSide.Bottom);
		for (int j = 0; j < this.m_verticalWave_ProjectileCount; j++)
		{
			Vector2 position3 = initialPosition3 + (float)j * distanceBetweenProjectiles2 * Vector2.right;
			GameObject gameObject3 = this.CreateProjectileSummonPoint(RoomSide.Top, position3);
			this.m_topVerticalProjectileSummonPoints[j] = gameObject3;
			Vector2 position4 = initialPosition4 + (float)j * distanceBetweenProjectiles2 * Vector2.right;
			GameObject gameObject4 = this.CreateProjectileSummonPoint(RoomSide.Bottom, position4);
			this.m_bottomVerticalProjectileSummonPoints[j] = gameObject4;
		}
		this.m_summonProjectileSummonPoints = new GameObject[this.m_summonShots_ProjectileCount];
		for (int k = 0; k < this.m_summonShots_ProjectileCount; k++)
		{
			GameObject gameObject5 = this.CreateProjectileSummonPoint(RoomSide.None, Vector2.zero);
			this.m_summonProjectileSummonPoints[k] = gameObject5;
		}
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00054DEC File Offset: 0x00052FEC
	private void ResetSummoningPointPositions(RoomSide side)
	{
		float distanceBetweenProjectiles = this.GetDistanceBetweenProjectiles(side);
		Vector2 initialPosition = this.GetInitialPosition(side);
		GameObject[] summoningPoints = this.GetSummoningPoints(side);
		for (int i = 0; i < summoningPoints.Length; i++)
		{
			Vector2 v;
			if (side == RoomSide.Left || side == RoomSide.Right)
			{
				v = initialPosition + (float)i * distanceBetweenProjectiles * Vector2.up;
			}
			else
			{
				v = initialPosition + (float)i * distanceBetweenProjectiles * Vector2.right;
			}
			summoningPoints[i].transform.position = v;
		}
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00054E6C File Offset: 0x0005306C
	private Vector2 GetInitialPosition(RoomSide side)
	{
		Vector2 result;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			float num = base.EnemyController.Room.Bounds.min.x;
			int num2 = 1;
			if (side == RoomSide.Right)
			{
				num = base.EnemyController.Room.Bounds.max.x;
				num2 = -1;
			}
			num += (float)num2 * 10f;
			float y = base.EnemyController.Room.Bounds.center.y - 0.5f * this.GetProjectileSpread(side);
			result = new Vector2(num, y);
		}
		else
		{
			float x = base.EnemyController.Room.Bounds.center.x - 0.5f * this.GetProjectileSpread(side);
			float num3 = base.EnemyController.Room.Bounds.min.y;
			int num4 = 1;
			if (side == RoomSide.Top)
			{
				num3 = base.EnemyController.Room.Bounds.max.y;
				num4 = -1;
			}
			num3 += (float)num4 * 0f;
			result = new Vector2(x, num3);
		}
		return result;
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00054FA4 File Offset: 0x000531A4
	private float GetDistanceBetweenProjectiles(RoomSide side)
	{
		int num;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			num = this.m_horizontalWave_ProjectileCount;
		}
		else
		{
			num = this.m_verticalWave_ProjectileCount;
		}
		return this.GetProjectileSpread(side) / (float)(num - 1);
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00054FD8 File Offset: 0x000531D8
	private float GetProjectileSpread(RoomSide side)
	{
		Vector2Int unitDimensions = (base.EnemyController.Room as Room).UnitDimensions;
		float result;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			result = (float)unitDimensions.y - 0f;
		}
		else
		{
			result = (float)unitDimensions.x - 20f;
		}
		return result;
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00055024 File Offset: 0x00053224
	private GameObject CreateProjectileSummonPoint(RoomSide side, Vector2 position)
	{
		if (!this.m_summonPointStorageLocation)
		{
			GameObject gameObject = new GameObject("Summoning Points");
			gameObject.transform.SetParent(base.EnemyController.Room.gameObject.transform);
			gameObject.transform.localPosition = Vector3.zero;
			this.m_summonPointStorageLocation = gameObject.transform;
		}
		GameObject gameObject2 = new GameObject("Projectile Summoning Point - " + side.ToString());
		gameObject2.transform.SetParent(this.m_summonPointStorageLocation);
		gameObject2.transform.position = position;
		return gameObject2;
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x00004C84 File Offset: 0x00002E84
	public override void ResetScript()
	{
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		this.m_currentMode = 0;
		if (this.m_summonPointStorageLocation)
		{
			UnityEngine.Object.Destroy(this.m_summonPointStorageLocation.gameObject);
		}
		base.ResetScript();
	}

	// Token: 0x04000845 RID: 2117
	public Relay SummonSentriesStartRelay = new Relay();

	// Token: 0x04000846 RID: 2118
	public Relay SummonSentriesCompleteRelay = new Relay();

	// Token: 0x04000847 RID: 2119
	public Relay<DancingBossWaveFiredEventArgs> WaveFiredRelay = new Relay<DancingBossWaveFiredEventArgs>();

	// Token: 0x04000848 RID: 2120
	public Relay SpreadIntroAnimStartRelay = new Relay();

	// Token: 0x04000849 RID: 2121
	public Relay SpreadIntroAnimCompleteRelay = new Relay();

	// Token: 0x0400084A RID: 2122
	public Relay SpreadAttackAnimStartRelay = new Relay();

	// Token: 0x0400084B RID: 2123
	public Relay SpreadAttackAnimCompleteRelay = new Relay();

	// Token: 0x0400084C RID: 2124
	public Relay<Vector3> SpreadAttackStartRelay = new Relay<Vector3>();

	// Token: 0x0400084D RID: 2125
	public Relay SpreadAttackCompleteRelay = new Relay();

	// Token: 0x0400084E RID: 2126
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x0400084F RID: 2127
	private int m_currentMode;

	// Token: 0x04000850 RID: 2128
	private Transform m_summonPointStorageLocation;

	// Token: 0x04000851 RID: 2129
	private Action<object, HealthChangeEventArgs> m_onBossHit;

	// Token: 0x04000852 RID: 2130
	private const float HORIZONTAL_INSET = 10f;

	// Token: 0x04000853 RID: 2131
	private const float VERTICAL_INSET = 0f;

	// Token: 0x04000854 RID: 2132
	private const string BASIC_WAVE_PROJECTILE = "DancingBossWaveBoltProjectile";

	// Token: 0x04000855 RID: 2133
	private const string BOUNCE_WAVE_PROJECTILE = "DancingBossWaveBounceProjectile";

	// Token: 0x04000856 RID: 2134
	private const string PASS_WAVE_PROJECTILE = "DancingBossWavePassBoltProjectile";

	// Token: 0x04000857 RID: 2135
	private const string BASIC_PROJECTILE = "DancingBossBoltProjectile";

	// Token: 0x04000858 RID: 2136
	private const string BOUNCE_PROJECTILE = "DancingBossBounceBoltProjectile";

	// Token: 0x04000859 RID: 2137
	private const string SUMMON_PROJECTILE = "DancingBossSummonProjectile";

	// Token: 0x0400085A RID: 2138
	private const string MODE_SHIFT_WARNING_PROJECTILE = "SpellSwordShoutWarningProjectile";

	// Token: 0x0400085B RID: 2139
	protected const string ORBIT_SHOT_PROJECTILE = "DancingBossBounceRotateProjectile";

	// Token: 0x0400085C RID: 2140
	protected const string STANDING_PROJECTILE = "DancingBossStandingBoltProjectile";

	// Token: 0x0400085D RID: 2141
	protected const string VOID_PROJECTILE = "DancingBossVoidProjectile";

	// Token: 0x0400085E RID: 2142
	private const string VERTICAL_TELL_INTRO = "SummonBulletsVertical_Tell_Intro";

	// Token: 0x0400085F RID: 2143
	private const string VERTICAL_TELL_HOLD = "SummonBulletsVertical_Tell_Hold";

	// Token: 0x04000860 RID: 2144
	private const string VERTICAL_ATTACK_INTRO = "SummonBulletsVertical_Attack_Intro";

	// Token: 0x04000861 RID: 2145
	private const string VERTICAL_ATTACK_HOLD = "SummonBulletsVertical_Attack_Hold";

	// Token: 0x04000862 RID: 2146
	private const string VERTICAL_ATTACK_EXIT = "SummonBulletsVertical_Exit";

	// Token: 0x04000863 RID: 2147
	private const string SPREAD_VERTICAL_TELL_INTRO = "CircularBurst_Tell_Intro";

	// Token: 0x04000864 RID: 2148
	private const string SPREAD_TELL_HOLD = "CircularBurst_Tell_Hold";

	// Token: 0x04000865 RID: 2149
	private const string SPREAD_VERTICAL_ATTACK_INTRO = "CircularBurst_Attack_Intro";

	// Token: 0x04000866 RID: 2150
	private const string SPREAD_VERTICAL_ATTACK_HOLD = "CircularBurst_Attack_Hold";

	// Token: 0x04000867 RID: 2151
	private const string SPREAD_VERTICAL_ATTACK_EXIT = "CircularBurst_Exit";

	// Token: 0x04000868 RID: 2152
	private const string HORIZONTAL_TELL_INTRO = "SummonBullets_Tell_Intro";

	// Token: 0x04000869 RID: 2153
	private const string HORIZONTAL_TELL_HOLD = "SummonBullets_Tell_Hold";

	// Token: 0x0400086A RID: 2154
	private const string HORIZONTAL_ATTACK_INTRO = "SummonBullets_Attack_Intro";

	// Token: 0x0400086B RID: 2155
	private const string HORIZONTAL_ATTACK_HOLD = "SummonBullets_Attack_Hold";

	// Token: 0x0400086C RID: 2156
	private const string HORIZONTAL_ATTACK_EXIT = "SummonBullets_Exit";

	// Token: 0x0400086D RID: 2157
	private const string BOMB_TELL_INTRO = "SummonIce_Tell_Intro";

	// Token: 0x0400086E RID: 2158
	private const string BOMB_TELL_HOLD = "SummonIce_Tell_Hold";

	// Token: 0x0400086F RID: 2159
	private const string BOMB_ATTACK_INTRO = "SummonIce_Attack_Intro";

	// Token: 0x04000870 RID: 2160
	private const string BOMB_ATTACK_HOLD = "SummonIce_Attack_Hold";

	// Token: 0x04000871 RID: 2161
	private const string BOMB_ATTACK_EXIT = "SummonIce_Exit";

	// Token: 0x04000872 RID: 2162
	private const string SUMMON_SHOTS_TELL_INTRO = "CircularBurst_Tell_Intro";

	// Token: 0x04000873 RID: 2163
	private const string SUMMON_SHOTS_TELL_HOLD = "CircularBurst_Tell_Hold";

	// Token: 0x04000874 RID: 2164
	private const string SUMMON_SHOTS_ATTACK_INTRO = "CircularBurst_Attack_Intro";

	// Token: 0x04000875 RID: 2165
	private const string SUMMON_SHOTS_ATTACK_HOLD = "CircularBurst_Attack_Hold";

	// Token: 0x04000876 RID: 2166
	private const string SUMMON_SHOTS_ATTACK_EXIT = "CircularBurst_Exit";

	// Token: 0x04000877 RID: 2167
	public Relay SummonShotsTellStartRelay = new Relay();

	// Token: 0x04000878 RID: 2168
	public Relay SummonShotsTellCompleteRelay = new Relay();

	// Token: 0x04000879 RID: 2169
	public Relay SummonShotsAttackStartRelay = new Relay();

	// Token: 0x0400087A RID: 2170
	public Relay SummonShotsAttackCompleteRelay = new Relay();

	// Token: 0x0400087B RID: 2171
	public Relay<Vector2> SummonShotsPortalOpenedRelay = new Relay<Vector2>();

	// Token: 0x0400087C RID: 2172
	private const string DASH_TELL_INTRO = "Dash_Tell_Intro";

	// Token: 0x0400087D RID: 2173
	private const string DASH_TELL_HOLD = "Dash_Tell_Hold";

	// Token: 0x0400087E RID: 2174
	private const string DASH_ATTACK_INTRO = "Dash_Attack_Intro";

	// Token: 0x0400087F RID: 2175
	private const string DASH_ATTACK_HOLD = "Dash_Attack_Hold";

	// Token: 0x04000880 RID: 2176
	private const string DASH_ATTACK_EXIT = "Dash_Exit";

	// Token: 0x04000881 RID: 2177
	protected const string MODE_SHIFT_01_DOWNED = "ModeShift_Intro";

	// Token: 0x04000882 RID: 2178
	protected const string MODE_SHIFT_01_ATTACK_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000883 RID: 2179
	protected const string MODE_SHIFT_01_ATTACK_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x04000884 RID: 2180
	protected const string MODE_SHIFT_01_ATTACK_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000885 RID: 2181
	private Projectile_RL m_modeShift01WarningProjectile;

	// Token: 0x04000886 RID: 2182
	protected const int MODE_SHIFT_01_WARNING_PROJECTILE_POS_INDEX = 0;

	// Token: 0x04000887 RID: 2183
	protected const string MODE_SHIFT_02_DOWNED = "Stunned";

	// Token: 0x04000888 RID: 2184
	protected const string MODE_SHIFT_02_GETUP = "Neutral";

	// Token: 0x04000889 RID: 2185
	protected const string MODE_SHIFT_02_EXIT = "Neutral";

	// Token: 0x0400088A RID: 2186
	protected const string MODE_SHIFT_02_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x0400088B RID: 2187
	protected const string MODE_SHIFT_02_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x0400088C RID: 2188
	protected const string MODE_SHIFT_02_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x0400088D RID: 2189
	protected const string MODE_SHIFT_02_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x0400088E RID: 2190
	protected const string MODE_SHIFT_02_ATTACK_EXIT = "Shoot_Exit";

	// Token: 0x0400088F RID: 2191
	protected const string FINAL_DANCE_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000890 RID: 2192
	protected const string FINAL_DANCE_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000891 RID: 2193
	protected const string FINAL_DANCE_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000892 RID: 2194
	protected const string FINAL_DANCE_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000893 RID: 2195
	protected const string FINAL_DANCE_ATTACK_EXIT = "Neutral";

	// Token: 0x04000894 RID: 2196
	private Projectile_RL m_modeShift02WarningProjectile;

	// Token: 0x04000895 RID: 2197
	protected const int MODE_SHIFT_02_WARNING_PROJECTILE_POS_INDEX = 0;

	// Token: 0x04000896 RID: 2198
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x04000897 RID: 2199
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x04000898 RID: 2200
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04000899 RID: 2201
	protected float m_spawn_Idle_Delay = 2f;

	// Token: 0x0400089A RID: 2202
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x0400089B RID: 2203
	protected float m_spawn_Intro_Delay;

	// Token: 0x0400089C RID: 2204
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x0400089D RID: 2205
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x0400089E RID: 2206
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x0400089F RID: 2207
	protected float m_death_Intro_Delay;

	// Token: 0x040008A0 RID: 2208
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x040008A1 RID: 2209
	protected float m_death_Hold_Delay = 4.5f;

	// Token: 0x040008A2 RID: 2210
	private List<Sentry_Hazard> m_sentryHazards;

	// Token: 0x040008A3 RID: 2211
	private GameObject m_centerOfRoom;

	// Token: 0x040008A4 RID: 2212
	private DancingBossWaveFiredEventArgs m_waveEventArgs;

	// Token: 0x040008A5 RID: 2213
	private bool m_areSummoningPointsInitialized;

	// Token: 0x040008A6 RID: 2214
	private GameObject[] m_leftHorizontalProjectileSummonPoints;

	// Token: 0x040008A7 RID: 2215
	private GameObject[] m_rightHorizontalProjectileSummonPoints;

	// Token: 0x040008A8 RID: 2216
	private GameObject[] m_topVerticalProjectileSummonPoints;

	// Token: 0x040008A9 RID: 2217
	private GameObject[] m_bottomVerticalProjectileSummonPoints;

	// Token: 0x040008AA RID: 2218
	private GameObject[] m_summonProjectileSummonPoints;
}
