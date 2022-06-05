using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class DancingBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x060002D1 RID: 721 RVA: 0x00014061 File Offset: 0x00012261
	protected virtual bool m_advancedAttacks
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00014064 File Offset: 0x00012264
	private void Awake()
	{
		this.m_onBossHit = new Action<object, HealthChangeEventArgs>(this.OnBossHit);
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x060002D3 RID: 723 RVA: 0x00014078 File Offset: 0x00012278
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.5f, 1f);
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x060002D4 RID: 724 RVA: 0x00014089 File Offset: 0x00012289
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1.75f, 2.5f);
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x060002D5 RID: 725 RVA: 0x0001409A File Offset: 0x0001229A
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1.75f, 2.5f);
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x060002D6 RID: 726 RVA: 0x000140AB File Offset: 0x000122AB
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x060002D7 RID: 727 RVA: 0x000140BC File Offset: 0x000122BC
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x000140D0 File Offset: 0x000122D0
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

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x060002D9 RID: 729 RVA: 0x0001413B File Offset: 0x0001233B
	protected virtual float m_verticalWave_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060002DA RID: 730 RVA: 0x00014142 File Offset: 0x00012342
	protected virtual float m_verticalWave_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060002DB RID: 731 RVA: 0x00014149 File Offset: 0x00012349
	protected virtual float m_verticalWave_TellIntroAndHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060002DC RID: 732 RVA: 0x00014150 File Offset: 0x00012350
	protected virtual float m_verticalWave_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060002DD RID: 733 RVA: 0x00014157 File Offset: 0x00012357
	protected virtual float m_verticalWave_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060002DE RID: 734 RVA: 0x0001415E File Offset: 0x0001235E
	protected virtual float m_verticalWave_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x060002DF RID: 735 RVA: 0x00014165 File Offset: 0x00012365
	protected virtual float m_verticalWave_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x060002E0 RID: 736 RVA: 0x0001416C File Offset: 0x0001236C
	protected virtual float m_verticalWave_AttackHoldExit_Delay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x060002E1 RID: 737 RVA: 0x00014173 File Offset: 0x00012373
	protected virtual float m_verticalWave_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060002E2 RID: 738 RVA: 0x0001417A File Offset: 0x0001237A
	protected virtual float m_verticalWave_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x060002E3 RID: 739 RVA: 0x00014181 File Offset: 0x00012381
	protected virtual float m_verticalWave_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060002E4 RID: 740 RVA: 0x00014188 File Offset: 0x00012388
	protected virtual float m_verticalWave_AttackExit_CooldownDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060002E5 RID: 741 RVA: 0x0001418F File Offset: 0x0001238F
	protected virtual float m_wave_SummoningCircleDelay
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x060002E6 RID: 742 RVA: 0x00014196 File Offset: 0x00012396
	protected virtual int m_verticalWave_ProjectileCount
	{
		get
		{
			return 17;
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x060002E7 RID: 743 RVA: 0x0001419A File Offset: 0x0001239A
	protected virtual int m_verticalWave_BounceProjectileCount
	{
		get
		{
			return 17;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x060002E8 RID: 744 RVA: 0x0001419E File Offset: 0x0001239E
	protected virtual int m_verticalWave_ProjectileSpeedMod
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x060002E9 RID: 745 RVA: 0x000141A1 File Offset: 0x000123A1
	protected virtual bool m_verticalWave_Pause_While_Attacking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060002EA RID: 746 RVA: 0x000141A4 File Offset: 0x000123A4
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

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x060002EB RID: 747 RVA: 0x000141B3 File Offset: 0x000123B3
	protected virtual float m_spread_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x060002EC RID: 748 RVA: 0x000141BA File Offset: 0x000123BA
	protected virtual float m_spread_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x060002ED RID: 749 RVA: 0x000141C1 File Offset: 0x000123C1
	protected virtual float m_spread_TellIntroAndHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x060002EE RID: 750 RVA: 0x000141C8 File Offset: 0x000123C8
	protected virtual float m_spread_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x060002EF RID: 751 RVA: 0x000141CF File Offset: 0x000123CF
	protected virtual float m_spread_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060002F0 RID: 752 RVA: 0x000141D6 File Offset: 0x000123D6
	protected virtual float m_spread_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060002F1 RID: 753 RVA: 0x000141DD File Offset: 0x000123DD
	protected virtual float m_spread_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060002F2 RID: 754 RVA: 0x000141E4 File Offset: 0x000123E4
	protected virtual float m_spread_AttackHoldExit_Delay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060002F3 RID: 755 RVA: 0x000141EB File Offset: 0x000123EB
	protected virtual float m_spread_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060002F4 RID: 756 RVA: 0x000141F2 File Offset: 0x000123F2
	protected virtual float m_spread_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060002F5 RID: 757 RVA: 0x000141F9 File Offset: 0x000123F9
	protected virtual float m_spread_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060002F6 RID: 758 RVA: 0x00014200 File Offset: 0x00012400
	protected virtual float m_spread_AttackExit_CooldownDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060002F7 RID: 759 RVA: 0x00014207 File Offset: 0x00012407
	protected virtual int m_spread_BasicProjectileCount
	{
		get
		{
			return 17;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060002F8 RID: 760 RVA: 0x0001420B File Offset: 0x0001240B
	protected virtual int m_spread_BounceProjectileCount
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060002F9 RID: 761 RVA: 0x0001420E File Offset: 0x0001240E
	protected virtual float m_spread_ProjectileSpeedMod
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060002FA RID: 762 RVA: 0x00014215 File Offset: 0x00012415
	protected virtual float m_spread_TimeBetweenAttacks
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060002FB RID: 763 RVA: 0x0001421C File Offset: 0x0001241C
	protected virtual int m_spread_NumberOfAttacks
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x060002FC RID: 764 RVA: 0x0001421F File Offset: 0x0001241F
	protected virtual float m_spread_SpaceBetweenAttacks
	{
		get
		{
			return 6.25f;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x060002FD RID: 765 RVA: 0x00014226 File Offset: 0x00012426
	protected virtual bool m_spread_summonProjectilesAtPlayerPosition
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x060002FE RID: 766 RVA: 0x00014229 File Offset: 0x00012429
	protected virtual bool m_spread_Pause_While_Attacking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0001422C File Offset: 0x0001242C
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

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000300 RID: 768 RVA: 0x0001423B File Offset: 0x0001243B
	protected virtual float m_horizontalWave_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000301 RID: 769 RVA: 0x00014242 File Offset: 0x00012442
	protected virtual float m_horizontalWave_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000302 RID: 770 RVA: 0x00014249 File Offset: 0x00012449
	protected virtual float m_horizontalWave_TellIntroAndHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000303 RID: 771 RVA: 0x00014250 File Offset: 0x00012450
	protected virtual float m_horizontalWave_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000304 RID: 772 RVA: 0x00014257 File Offset: 0x00012457
	protected virtual float m_horizontalWave_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000305 RID: 773 RVA: 0x0001425E File Offset: 0x0001245E
	protected virtual float m_horizontalWave_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000306 RID: 774 RVA: 0x00014265 File Offset: 0x00012465
	protected virtual float m_horizontalWave_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000307 RID: 775 RVA: 0x0001426C File Offset: 0x0001246C
	protected virtual float m_horizontalWave_AttackHoldExit_Delay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000308 RID: 776 RVA: 0x00014273 File Offset: 0x00012473
	protected virtual float m_horizontalWave_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000309 RID: 777 RVA: 0x0001427A File Offset: 0x0001247A
	protected virtual float m_horizontalWave_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x0600030A RID: 778 RVA: 0x00014281 File Offset: 0x00012481
	protected virtual float m_horizontalWave_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x0600030B RID: 779 RVA: 0x00014288 File Offset: 0x00012488
	protected virtual float m_horizontalWave_AttackExit_CooldownDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x0600030C RID: 780 RVA: 0x0001428F File Offset: 0x0001248F
	protected virtual int m_horizontalWave_ProjectileCount
	{
		get
		{
			return 11;
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x0600030D RID: 781 RVA: 0x00014293 File Offset: 0x00012493
	protected virtual int m_horizontalWave_BounceProjectileCount
	{
		get
		{
			return 11;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x0600030E RID: 782 RVA: 0x00014297 File Offset: 0x00012497
	protected virtual float m_horizontalWave_ProjectileSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x0600030F RID: 783 RVA: 0x0001429E File Offset: 0x0001249E
	protected virtual bool m_horizontalWave_Pause_While_Attacking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000310 RID: 784 RVA: 0x000142A1 File Offset: 0x000124A1
	protected virtual float m_bomb_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000311 RID: 785 RVA: 0x000142A8 File Offset: 0x000124A8
	protected virtual float m_bomb_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000312 RID: 786 RVA: 0x000142AF File Offset: 0x000124AF
	protected virtual float m_bomb_TellIntroAndHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000313 RID: 787 RVA: 0x000142B6 File Offset: 0x000124B6
	protected virtual float m_bomb_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000314 RID: 788 RVA: 0x000142BD File Offset: 0x000124BD
	protected virtual float m_bomb_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000315 RID: 789 RVA: 0x000142C4 File Offset: 0x000124C4
	protected virtual float m_bomb_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000316 RID: 790 RVA: 0x000142CB File Offset: 0x000124CB
	protected virtual float m_bomb_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000317 RID: 791 RVA: 0x000142D2 File Offset: 0x000124D2
	protected virtual float m_bomb_AttackHoldExit_Delay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000318 RID: 792 RVA: 0x000142D9 File Offset: 0x000124D9
	protected virtual float m_bomb_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000319 RID: 793 RVA: 0x000142E0 File Offset: 0x000124E0
	protected virtual float m_bomb_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x0600031A RID: 794 RVA: 0x000142E7 File Offset: 0x000124E7
	protected virtual float m_bomb_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x0600031B RID: 795 RVA: 0x000142EE File Offset: 0x000124EE
	protected virtual float m_bomb_AttackExit_CooldownDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x0600031C RID: 796 RVA: 0x000142F5 File Offset: 0x000124F5
	protected virtual bool m_bomb_summonProjectilesAtPlayerPosition
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x0600031D RID: 797 RVA: 0x000142F8 File Offset: 0x000124F8
	protected virtual int m_bomb_projectileAmount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x0600031E RID: 798 RVA: 0x000142FB File Offset: 0x000124FB
	protected virtual float m_bomb_projectileDelay
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x0600031F RID: 799 RVA: 0x00014302 File Offset: 0x00012502
	protected virtual float m_bomb_ProjectileSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000320 RID: 800 RVA: 0x00014309 File Offset: 0x00012509
	protected virtual bool m_bomb_Pause_While_Attacking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000321 RID: 801 RVA: 0x0001430C File Offset: 0x0001250C
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

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000322 RID: 802 RVA: 0x0001431B File Offset: 0x0001251B
	protected virtual float m_summonShots_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000323 RID: 803 RVA: 0x00014322 File Offset: 0x00012522
	protected virtual float m_summonShots_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000324 RID: 804 RVA: 0x00014329 File Offset: 0x00012529
	protected virtual float m_summonShots_TellIntroAndHold_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000325 RID: 805 RVA: 0x00014330 File Offset: 0x00012530
	protected virtual float m_summonShots_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000326 RID: 806 RVA: 0x00014337 File Offset: 0x00012537
	protected virtual float m_summonShots_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000327 RID: 807 RVA: 0x0001433E File Offset: 0x0001253E
	protected virtual float m_summonShots_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06000328 RID: 808 RVA: 0x00014345 File Offset: 0x00012545
	protected virtual float m_summonShots_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000329 RID: 809 RVA: 0x0001434C File Offset: 0x0001254C
	protected virtual float m_summonShots_AttackHoldExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x0600032A RID: 810 RVA: 0x00014353 File Offset: 0x00012553
	protected virtual float m_summonShots_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x0600032B RID: 811 RVA: 0x0001435A File Offset: 0x0001255A
	protected virtual float m_summonShots_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x0600032C RID: 812 RVA: 0x00014361 File Offset: 0x00012561
	protected virtual float m_summonShots_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x0600032D RID: 813 RVA: 0x00014368 File Offset: 0x00012568
	protected virtual float m_summonShots_AttackExit_CooldownDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x0600032E RID: 814 RVA: 0x0001436F File Offset: 0x0001256F
	protected virtual int m_summonShots_ProjectileCount
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x0600032F RID: 815 RVA: 0x00014372 File Offset: 0x00012572
	protected virtual int m_summonShots_ProjectileSpeedMod
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00014375 File Offset: 0x00012575
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

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000331 RID: 817 RVA: 0x00014384 File Offset: 0x00012584
	protected virtual float m_dash_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000332 RID: 818 RVA: 0x0001438B File Offset: 0x0001258B
	protected virtual float m_dash_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000333 RID: 819 RVA: 0x00014392 File Offset: 0x00012592
	protected virtual float m_dash_TellIntroAndHold_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000334 RID: 820 RVA: 0x00014399 File Offset: 0x00012599
	protected virtual float m_dash_AttackIntro_AnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06000335 RID: 821 RVA: 0x000143A0 File Offset: 0x000125A0
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000336 RID: 822 RVA: 0x000143A7 File Offset: 0x000125A7
	protected virtual float m_dash_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000337 RID: 823 RVA: 0x000143AE File Offset: 0x000125AE
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06000338 RID: 824 RVA: 0x000143B5 File Offset: 0x000125B5
	protected virtual float m_dash_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000339 RID: 825 RVA: 0x000143BC File Offset: 0x000125BC
	protected virtual float m_dash_AttackExit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x0600033A RID: 826 RVA: 0x000143C3 File Offset: 0x000125C3
	protected virtual float m_dash_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x0600033B RID: 827 RVA: 0x000143CA File Offset: 0x000125CA
	protected virtual float m_dash_AttackExit_CooldownDuration
	{
		get
		{
			return 12f;
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x0600033C RID: 828 RVA: 0x000143D1 File Offset: 0x000125D1
	protected virtual float m_dash_Attack_ForwardSpeed
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x0600033D RID: 829 RVA: 0x000143D8 File Offset: 0x000125D8
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x0600033E RID: 830 RVA: 0x000143DF File Offset: 0x000125DF
	protected virtual bool m_dash_Attack_Pause_While_Attacking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600033F RID: 831 RVA: 0x000143E2 File Offset: 0x000125E2
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

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000340 RID: 832 RVA: 0x000143F1 File Offset: 0x000125F1
	protected virtual float m_modeShift01_Downed_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000341 RID: 833 RVA: 0x000143F8 File Offset: 0x000125F8
	protected virtual float m_modeShift01_Downed_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000342 RID: 834 RVA: 0x000143FF File Offset: 0x000125FF
	protected virtual float m_modeShift01_GetUp_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000343 RID: 835 RVA: 0x00014406 File Offset: 0x00012606
	protected virtual float m_modeShift01_GetUp_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000344 RID: 836 RVA: 0x0001440D File Offset: 0x0001260D
	protected virtual float m_modeShift01_Warning_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000345 RID: 837 RVA: 0x00014414 File Offset: 0x00012614
	protected virtual float m_modeShift01_Exit_AnimSpeed
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000346 RID: 838 RVA: 0x0001441B File Offset: 0x0001261B
	protected virtual float m_modeShift01_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000347 RID: 839 RVA: 0x00014422 File Offset: 0x00012622
	protected virtual float m_modeShift01_Exit_IdleDuration
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000348 RID: 840 RVA: 0x00014429 File Offset: 0x00012629
	protected virtual float m_modeShift01_AttackCD
	{
		get
		{
			return 999999f;
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000349 RID: 841 RVA: 0x00014430 File Offset: 0x00012630
	protected virtual float m_modeShift01_Damage_Mod
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x0600034A RID: 842 RVA: 0x00014437 File Offset: 0x00012637
	protected virtual float m_firstModeShiftHealthMod
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x0600034B RID: 843 RVA: 0x0001443E File Offset: 0x0001263E
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

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x0600034C RID: 844 RVA: 0x0001444D File Offset: 0x0001264D
	protected virtual float m_modeShift02_Downed_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x0600034D RID: 845 RVA: 0x00014454 File Offset: 0x00012654
	protected virtual float m_modeShift02_Downed_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x0600034E RID: 846 RVA: 0x0001445B File Offset: 0x0001265B
	protected virtual float m_modeShift02_GetUp_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x0600034F RID: 847 RVA: 0x00014462 File Offset: 0x00012662
	protected virtual float m_modeShift02_GetUp_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000350 RID: 848 RVA: 0x00014469 File Offset: 0x00012669
	protected virtual float m_modeShift02_Warning_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000351 RID: 849 RVA: 0x00014470 File Offset: 0x00012670
	protected virtual float m_modeShift02_Exit_AnimSpeed
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000352 RID: 850 RVA: 0x00014477 File Offset: 0x00012677
	protected virtual float m_modeShift02_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000353 RID: 851 RVA: 0x0001447E File Offset: 0x0001267E
	protected virtual float m_modeShift02_Exit_IdleDuration
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000354 RID: 852 RVA: 0x00014485 File Offset: 0x00012685
	protected virtual float m_modeShift02_AttackCD
	{
		get
		{
			return 99f;
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000355 RID: 853 RVA: 0x0001448C File Offset: 0x0001268C
	protected virtual float m_finalDance_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000356 RID: 854 RVA: 0x00014493 File Offset: 0x00012693
	protected virtual float m_finalDance_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000357 RID: 855 RVA: 0x0001449A File Offset: 0x0001269A
	protected virtual float m_finalDance_TellIntroAndHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000358 RID: 856 RVA: 0x000144A1 File Offset: 0x000126A1
	protected virtual float m_finalDance_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000359 RID: 857 RVA: 0x000144A8 File Offset: 0x000126A8
	protected virtual float m_finalDance_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x0600035A RID: 858 RVA: 0x000144AF File Offset: 0x000126AF
	protected virtual float m_finalDance_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x0600035B RID: 859 RVA: 0x000144B6 File Offset: 0x000126B6
	protected virtual float m_finalDance_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x0600035C RID: 860 RVA: 0x000144BD File Offset: 0x000126BD
	protected virtual float m_finalDance_AttackHoldExit_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x0600035D RID: 861 RVA: 0x000144C4 File Offset: 0x000126C4
	protected virtual float m_finalDance_AttackExit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x0600035E RID: 862 RVA: 0x000144CB File Offset: 0x000126CB
	protected virtual float m_finalDance_AttackExit_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x0600035F RID: 863 RVA: 0x000144D2 File Offset: 0x000126D2
	protected virtual float m_finalDance_AttackExit_IdleDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000360 RID: 864 RVA: 0x000144D9 File Offset: 0x000126D9
	protected virtual float m_finalDance_AttackExit_CooldownDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000361 RID: 865 RVA: 0x000144E0 File Offset: 0x000126E0
	protected virtual float m_modeShift02_Damage_Mod
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000362 RID: 866 RVA: 0x000144E7 File Offset: 0x000126E7
	protected virtual float m_secondModeShiftHealthMod
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000363 RID: 867 RVA: 0x000144EE File Offset: 0x000126EE
	protected virtual float m_sentryRestDuration
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000364 RID: 868 RVA: 0x000144F5 File Offset: 0x000126F5
	public List<Sentry_Hazard> SentryHazards
	{
		get
		{
			return this.m_sentryHazards;
		}
	}

	// Token: 0x06000365 RID: 869 RVA: 0x000144FD File Offset: 0x000126FD
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

	// Token: 0x06000366 RID: 870 RVA: 0x0001450C File Offset: 0x0001270C
	private IEnumerator FireProjectilesInRandomDirections()
	{
		yield break;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x00014514 File Offset: 0x00012714
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

	// Token: 0x06000368 RID: 872 RVA: 0x00014523 File Offset: 0x00012723
	public override IEnumerator SpawnAnim()
	{
		yield return this.Default_Animation("Intro_Idle", this.m_spawn_Idle_AnimSpeed, this.m_spawn_Idle_Delay, true);
		MusicManager.PlayMusic(SongID.ForestBossBGM_Tettix_Naamah_180_BPM, false, false);
		yield return this.Default_Animation("Intro", this.m_spawn_Intro_AnimSpeed, this.m_spawn_Intro_Delay, true);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		yield break;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00014532 File Offset: 0x00012732
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

	// Token: 0x0600036A RID: 874 RVA: 0x00014541 File Offset: 0x00012741
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_modeShift01WarningProjectile);
		base.StopProjectile(ref this.m_modeShift02WarningProjectile);
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00014561 File Offset: 0x00012761
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.LogicController.DisableLogicActivationByDistance = true;
		base.EnemyController.Target = PlayerManager.GetPlayerController().gameObject;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00014598 File Offset: 0x00012798
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

	// Token: 0x0600036D RID: 877 RVA: 0x00014696 File Offset: 0x00012896
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		base.EnemyController.HealthChangeRelay.AddListener(this.m_onBossHit, false);
		this.CreateSummoningPoints();
		this.InitializeSentryHazards();
	}

	// Token: 0x0600036E RID: 878 RVA: 0x000146C2 File Offset: 0x000128C2
	protected override void OnDisable()
	{
		base.OnDisable();
		if (base.EnemyController && base.EnemyController.HealthChangeRelay != null)
		{
			base.EnemyController.HealthChangeRelay.RemoveListener(this.m_onBossHit);
		}
	}

	// Token: 0x0600036F RID: 879 RVA: 0x000146FB File Offset: 0x000128FB
	private void FireWave(RoomSide side, int basicProjectileAmount, int bounceProjectileAmount, float projectileSpeedMod, bool addSummoningDelay = true, bool isVerticalWave = false)
	{
		base.StartCoroutine(this.FireWaveCoroutine(side, basicProjectileAmount, bounceProjectileAmount, projectileSpeedMod, addSummoningDelay, isVerticalWave));
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00014713 File Offset: 0x00012913
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

	// Token: 0x06000371 RID: 881 RVA: 0x00014748 File Offset: 0x00012948
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

	// Token: 0x06000372 RID: 882 RVA: 0x00014839 File Offset: 0x00012A39
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

	// Token: 0x06000373 RID: 883 RVA: 0x00014868 File Offset: 0x00012A68
	private void SummonProjectile(GameObject summonPoint, string projectileName, bool matchFacing, float angle, float speedMod, bool playSummonAudio = true, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		Vector2 offset = summonPoint.transform.position - base.EnemyController.transform.position;
		this.SummonProjectile(projectileName, offset, matchFacing, angle, speedMod, playSummonAudio, playSpawnAudio, playLifetimeAudio, playDeathAudio);
	}

	// Token: 0x06000374 RID: 884 RVA: 0x000148B0 File Offset: 0x00012AB0
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

	// Token: 0x06000375 RID: 885 RVA: 0x0001496C File Offset: 0x00012B6C
	private static RoomSide GetRandomSide(params RoomSide[] sides)
	{
		int num = UnityEngine.Random.Range(0, sides.Length);
		return sides[num];
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00014988 File Offset: 0x00012B88
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

	// Token: 0x06000377 RID: 887 RVA: 0x000149C8 File Offset: 0x00012BC8
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

	// Token: 0x06000378 RID: 888 RVA: 0x00014A12 File Offset: 0x00012C12
	private float GetSummonEffectDuration()
	{
		return 1.2f;
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00014A19 File Offset: 0x00012C19
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

	// Token: 0x0600037A RID: 890 RVA: 0x00014A30 File Offset: 0x00012C30
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

	// Token: 0x0600037B RID: 891 RVA: 0x00014BF0 File Offset: 0x00012DF0
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

	// Token: 0x0600037C RID: 892 RVA: 0x00014C70 File Offset: 0x00012E70
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

	// Token: 0x0600037D RID: 893 RVA: 0x00014DA8 File Offset: 0x00012FA8
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

	// Token: 0x0600037E RID: 894 RVA: 0x00014DDC File Offset: 0x00012FDC
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

	// Token: 0x0600037F RID: 895 RVA: 0x00014E28 File Offset: 0x00013028
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

	// Token: 0x06000380 RID: 896 RVA: 0x00014EC7 File Offset: 0x000130C7
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

	// Token: 0x04000764 RID: 1892
	public Relay SummonSentriesStartRelay = new Relay();

	// Token: 0x04000765 RID: 1893
	public Relay SummonSentriesCompleteRelay = new Relay();

	// Token: 0x04000766 RID: 1894
	public Relay<DancingBossWaveFiredEventArgs> WaveFiredRelay = new Relay<DancingBossWaveFiredEventArgs>();

	// Token: 0x04000767 RID: 1895
	public Relay SpreadIntroAnimStartRelay = new Relay();

	// Token: 0x04000768 RID: 1896
	public Relay SpreadIntroAnimCompleteRelay = new Relay();

	// Token: 0x04000769 RID: 1897
	public Relay SpreadAttackAnimStartRelay = new Relay();

	// Token: 0x0400076A RID: 1898
	public Relay SpreadAttackAnimCompleteRelay = new Relay();

	// Token: 0x0400076B RID: 1899
	public Relay<Vector3> SpreadAttackStartRelay = new Relay<Vector3>();

	// Token: 0x0400076C RID: 1900
	public Relay SpreadAttackCompleteRelay = new Relay();

	// Token: 0x0400076D RID: 1901
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x0400076E RID: 1902
	private int m_currentMode;

	// Token: 0x0400076F RID: 1903
	private Transform m_summonPointStorageLocation;

	// Token: 0x04000770 RID: 1904
	private Action<object, HealthChangeEventArgs> m_onBossHit;

	// Token: 0x04000771 RID: 1905
	private const float HORIZONTAL_INSET = 10f;

	// Token: 0x04000772 RID: 1906
	private const float VERTICAL_INSET = 0f;

	// Token: 0x04000773 RID: 1907
	private const string BASIC_WAVE_PROJECTILE = "DancingBossWaveBoltProjectile";

	// Token: 0x04000774 RID: 1908
	private const string BOUNCE_WAVE_PROJECTILE = "DancingBossWaveBounceProjectile";

	// Token: 0x04000775 RID: 1909
	private const string PASS_WAVE_PROJECTILE = "DancingBossWavePassBoltProjectile";

	// Token: 0x04000776 RID: 1910
	private const string BASIC_PROJECTILE = "DancingBossBoltProjectile";

	// Token: 0x04000777 RID: 1911
	private const string BOUNCE_PROJECTILE = "DancingBossBounceBoltProjectile";

	// Token: 0x04000778 RID: 1912
	private const string SUMMON_PROJECTILE = "DancingBossSummonProjectile";

	// Token: 0x04000779 RID: 1913
	private const string MODE_SHIFT_WARNING_PROJECTILE = "SpellSwordShoutWarningProjectile";

	// Token: 0x0400077A RID: 1914
	protected const string ORBIT_SHOT_PROJECTILE = "DancingBossBounceRotateProjectile";

	// Token: 0x0400077B RID: 1915
	protected const string STANDING_PROJECTILE = "DancingBossStandingBoltProjectile";

	// Token: 0x0400077C RID: 1916
	protected const string VOID_PROJECTILE = "DancingBossVoidProjectile";

	// Token: 0x0400077D RID: 1917
	private const string VERTICAL_TELL_INTRO = "SummonBulletsVertical_Tell_Intro";

	// Token: 0x0400077E RID: 1918
	private const string VERTICAL_TELL_HOLD = "SummonBulletsVertical_Tell_Hold";

	// Token: 0x0400077F RID: 1919
	private const string VERTICAL_ATTACK_INTRO = "SummonBulletsVertical_Attack_Intro";

	// Token: 0x04000780 RID: 1920
	private const string VERTICAL_ATTACK_HOLD = "SummonBulletsVertical_Attack_Hold";

	// Token: 0x04000781 RID: 1921
	private const string VERTICAL_ATTACK_EXIT = "SummonBulletsVertical_Exit";

	// Token: 0x04000782 RID: 1922
	private const string SPREAD_VERTICAL_TELL_INTRO = "CircularBurst_Tell_Intro";

	// Token: 0x04000783 RID: 1923
	private const string SPREAD_TELL_HOLD = "CircularBurst_Tell_Hold";

	// Token: 0x04000784 RID: 1924
	private const string SPREAD_VERTICAL_ATTACK_INTRO = "CircularBurst_Attack_Intro";

	// Token: 0x04000785 RID: 1925
	private const string SPREAD_VERTICAL_ATTACK_HOLD = "CircularBurst_Attack_Hold";

	// Token: 0x04000786 RID: 1926
	private const string SPREAD_VERTICAL_ATTACK_EXIT = "CircularBurst_Exit";

	// Token: 0x04000787 RID: 1927
	private const string HORIZONTAL_TELL_INTRO = "SummonBullets_Tell_Intro";

	// Token: 0x04000788 RID: 1928
	private const string HORIZONTAL_TELL_HOLD = "SummonBullets_Tell_Hold";

	// Token: 0x04000789 RID: 1929
	private const string HORIZONTAL_ATTACK_INTRO = "SummonBullets_Attack_Intro";

	// Token: 0x0400078A RID: 1930
	private const string HORIZONTAL_ATTACK_HOLD = "SummonBullets_Attack_Hold";

	// Token: 0x0400078B RID: 1931
	private const string HORIZONTAL_ATTACK_EXIT = "SummonBullets_Exit";

	// Token: 0x0400078C RID: 1932
	private const string BOMB_TELL_INTRO = "SummonIce_Tell_Intro";

	// Token: 0x0400078D RID: 1933
	private const string BOMB_TELL_HOLD = "SummonIce_Tell_Hold";

	// Token: 0x0400078E RID: 1934
	private const string BOMB_ATTACK_INTRO = "SummonIce_Attack_Intro";

	// Token: 0x0400078F RID: 1935
	private const string BOMB_ATTACK_HOLD = "SummonIce_Attack_Hold";

	// Token: 0x04000790 RID: 1936
	private const string BOMB_ATTACK_EXIT = "SummonIce_Exit";

	// Token: 0x04000791 RID: 1937
	private const string SUMMON_SHOTS_TELL_INTRO = "CircularBurst_Tell_Intro";

	// Token: 0x04000792 RID: 1938
	private const string SUMMON_SHOTS_TELL_HOLD = "CircularBurst_Tell_Hold";

	// Token: 0x04000793 RID: 1939
	private const string SUMMON_SHOTS_ATTACK_INTRO = "CircularBurst_Attack_Intro";

	// Token: 0x04000794 RID: 1940
	private const string SUMMON_SHOTS_ATTACK_HOLD = "CircularBurst_Attack_Hold";

	// Token: 0x04000795 RID: 1941
	private const string SUMMON_SHOTS_ATTACK_EXIT = "CircularBurst_Exit";

	// Token: 0x04000796 RID: 1942
	public Relay SummonShotsTellStartRelay = new Relay();

	// Token: 0x04000797 RID: 1943
	public Relay SummonShotsTellCompleteRelay = new Relay();

	// Token: 0x04000798 RID: 1944
	public Relay SummonShotsAttackStartRelay = new Relay();

	// Token: 0x04000799 RID: 1945
	public Relay SummonShotsAttackCompleteRelay = new Relay();

	// Token: 0x0400079A RID: 1946
	public Relay<Vector2> SummonShotsPortalOpenedRelay = new Relay<Vector2>();

	// Token: 0x0400079B RID: 1947
	private const string DASH_TELL_INTRO = "Dash_Tell_Intro";

	// Token: 0x0400079C RID: 1948
	private const string DASH_TELL_HOLD = "Dash_Tell_Hold";

	// Token: 0x0400079D RID: 1949
	private const string DASH_ATTACK_INTRO = "Dash_Attack_Intro";

	// Token: 0x0400079E RID: 1950
	private const string DASH_ATTACK_HOLD = "Dash_Attack_Hold";

	// Token: 0x0400079F RID: 1951
	private const string DASH_ATTACK_EXIT = "Dash_Exit";

	// Token: 0x040007A0 RID: 1952
	protected const string MODE_SHIFT_01_DOWNED = "ModeShift_Intro";

	// Token: 0x040007A1 RID: 1953
	protected const string MODE_SHIFT_01_ATTACK_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x040007A2 RID: 1954
	protected const string MODE_SHIFT_01_ATTACK_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x040007A3 RID: 1955
	protected const string MODE_SHIFT_01_ATTACK_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x040007A4 RID: 1956
	private Projectile_RL m_modeShift01WarningProjectile;

	// Token: 0x040007A5 RID: 1957
	protected const int MODE_SHIFT_01_WARNING_PROJECTILE_POS_INDEX = 0;

	// Token: 0x040007A6 RID: 1958
	protected const string MODE_SHIFT_02_DOWNED = "Stunned";

	// Token: 0x040007A7 RID: 1959
	protected const string MODE_SHIFT_02_GETUP = "Neutral";

	// Token: 0x040007A8 RID: 1960
	protected const string MODE_SHIFT_02_EXIT = "Neutral";

	// Token: 0x040007A9 RID: 1961
	protected const string MODE_SHIFT_02_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x040007AA RID: 1962
	protected const string MODE_SHIFT_02_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x040007AB RID: 1963
	protected const string MODE_SHIFT_02_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x040007AC RID: 1964
	protected const string MODE_SHIFT_02_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x040007AD RID: 1965
	protected const string MODE_SHIFT_02_ATTACK_EXIT = "Shoot_Exit";

	// Token: 0x040007AE RID: 1966
	protected const string FINAL_DANCE_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x040007AF RID: 1967
	protected const string FINAL_DANCE_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x040007B0 RID: 1968
	protected const string FINAL_DANCE_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x040007B1 RID: 1969
	protected const string FINAL_DANCE_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x040007B2 RID: 1970
	protected const string FINAL_DANCE_ATTACK_EXIT = "Neutral";

	// Token: 0x040007B3 RID: 1971
	private Projectile_RL m_modeShift02WarningProjectile;

	// Token: 0x040007B4 RID: 1972
	protected const int MODE_SHIFT_02_WARNING_PROJECTILE_POS_INDEX = 0;

	// Token: 0x040007B5 RID: 1973
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x040007B6 RID: 1974
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x040007B7 RID: 1975
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x040007B8 RID: 1976
	protected float m_spawn_Idle_Delay = 2f;

	// Token: 0x040007B9 RID: 1977
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x040007BA RID: 1978
	protected float m_spawn_Intro_Delay;

	// Token: 0x040007BB RID: 1979
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x040007BC RID: 1980
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x040007BD RID: 1981
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x040007BE RID: 1982
	protected float m_death_Intro_Delay;

	// Token: 0x040007BF RID: 1983
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x040007C0 RID: 1984
	protected float m_death_Hold_Delay = 4.5f;

	// Token: 0x040007C1 RID: 1985
	private List<Sentry_Hazard> m_sentryHazards;

	// Token: 0x040007C2 RID: 1986
	private GameObject m_centerOfRoom;

	// Token: 0x040007C3 RID: 1987
	private DancingBossWaveFiredEventArgs m_waveEventArgs;

	// Token: 0x040007C4 RID: 1988
	private bool m_areSummoningPointsInitialized;

	// Token: 0x040007C5 RID: 1989
	private GameObject[] m_leftHorizontalProjectileSummonPoints;

	// Token: 0x040007C6 RID: 1990
	private GameObject[] m_rightHorizontalProjectileSummonPoints;

	// Token: 0x040007C7 RID: 1991
	private GameObject[] m_topVerticalProjectileSummonPoints;

	// Token: 0x040007C8 RID: 1992
	private GameObject[] m_bottomVerticalProjectileSummonPoints;

	// Token: 0x040007C9 RID: 1993
	private GameObject[] m_summonProjectileSummonPoints;
}
