using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using RL_Windows;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020001F2 RID: 498
public class EnemyController : BaseCharacterController, IRoomConsumer, ILevelConsumer, ISummoner, IHealth, IDamageObj, IGenericPoolObj, IOffscreenObj
{
	// Token: 0x17000A82 RID: 2690
	// (get) Token: 0x0600146C RID: 5228 RVA: 0x0003DF1E File Offset: 0x0003C11E
	// (set) Token: 0x0600146D RID: 5229 RVA: 0x0003DF26 File Offset: 0x0003C126
	public bool DisableOffscreenWarnings { get; set; } = true;

	// Token: 0x17000A83 RID: 2691
	// (get) Token: 0x0600146E RID: 5230 RVA: 0x0003DF2F File Offset: 0x0003C12F
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000A84 RID: 2692
	// (get) Token: 0x0600146F RID: 5231 RVA: 0x0003DF32 File Offset: 0x0003C132
	// (set) Token: 0x06001470 RID: 5232 RVA: 0x0003DF3A File Offset: 0x0003C13A
	public EnemyManager.EnemyPreSummonedState PreSummonedState { get; set; }

	// Token: 0x17000A85 RID: 2693
	// (get) Token: 0x06001471 RID: 5233 RVA: 0x0003DF43 File Offset: 0x0003C143
	// (set) Token: 0x06001472 RID: 5234 RVA: 0x0003DF4B File Offset: 0x0003C14B
	public bool IsCulled { get; set; }

	// Token: 0x17000A86 RID: 2694
	// (get) Token: 0x06001473 RID: 5235 RVA: 0x0003DF54 File Offset: 0x0003C154
	// (set) Token: 0x06001474 RID: 5236 RVA: 0x0003DF5C File Offset: 0x0003C15C
	public bool KilledByKnockout { get; set; }

	// Token: 0x17000A87 RID: 2695
	// (get) Token: 0x06001475 RID: 5237 RVA: 0x0003DF65 File Offset: 0x0003C165
	// (set) Token: 0x06001476 RID: 5238 RVA: 0x0003DF6D File Offset: 0x0003C16D
	public bool DisableDistanceThresholdCheck { get; set; }

	// Token: 0x17000A88 RID: 2696
	// (get) Token: 0x06001477 RID: 5239 RVA: 0x0003DF76 File Offset: 0x0003C176
	// (set) Token: 0x06001478 RID: 5240 RVA: 0x0003DF7E File Offset: 0x0003C17E
	public bool IsCommander { get; set; }

	// Token: 0x17000A89 RID: 2697
	// (get) Token: 0x06001479 RID: 5241 RVA: 0x0003DF87 File Offset: 0x0003C187
	// (set) Token: 0x0600147A RID: 5242 RVA: 0x0003DF8F File Offset: 0x0003C18F
	public int LastDamageHitCount { get; set; }

	// Token: 0x17000A8A RID: 2698
	// (get) Token: 0x0600147B RID: 5243 RVA: 0x0003DF98 File Offset: 0x0003C198
	// (set) Token: 0x0600147C RID: 5244 RVA: 0x0003DFA0 File Offset: 0x0003C1A0
	public DamageType LastDamageTakenType
	{
		get
		{
			return this.m_lastDamageTypeTaken;
		}
		set
		{
			if (this.m_lastDamageTypeTaken != value)
			{
				this.LastDamageHitCount = 0;
			}
			this.m_lastDamageTypeTaken = value;
		}
	}

	// Token: 0x17000A8B RID: 2699
	// (get) Token: 0x0600147D RID: 5245 RVA: 0x0003DFB9 File Offset: 0x0003C1B9
	public PreventPlatformDrop PreventPlatformDropObj
	{
		get
		{
			return this.m_preventPlatformDropObj;
		}
	}

	// Token: 0x17000A8C RID: 2700
	// (get) Token: 0x0600147E RID: 5246 RVA: 0x0003DFC1 File Offset: 0x0003C1C1
	// (set) Token: 0x0600147F RID: 5247 RVA: 0x0003DFD4 File Offset: 0x0003C1D4
	public bool FallLedge
	{
		get
		{
			return this.IsFlying || this.m_fallLedge;
		}
		set
		{
			if (this.IsFlying)
			{
				value = true;
			}
			if (this.m_preventPlatformDropObj)
			{
				this.m_fallLedge = value;
				if (this.m_preventPlatformDropObj.enabled != !value)
				{
					this.m_preventPlatformDropObj.enabled = !value;
				}
			}
		}
	}

	// Token: 0x17000A8D RID: 2701
	// (get) Token: 0x06001480 RID: 5248 RVA: 0x0003E020 File Offset: 0x0003C220
	// (set) Token: 0x06001481 RID: 5249 RVA: 0x0003E028 File Offset: 0x0003C228
	public bool IsBeingSummoned { get; set; }

	// Token: 0x17000A8E RID: 2702
	// (get) Token: 0x06001482 RID: 5250 RVA: 0x0003E031 File Offset: 0x0003C231
	// (set) Token: 0x06001483 RID: 5251 RVA: 0x0003E039 File Offset: 0x0003C239
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17000A8F RID: 2703
	// (get) Token: 0x06001484 RID: 5252 RVA: 0x0003E042 File Offset: 0x0003C242
	// (set) Token: 0x06001485 RID: 5253 RVA: 0x0003E04A File Offset: 0x0003C24A
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17000A90 RID: 2704
	// (get) Token: 0x06001486 RID: 5254 RVA: 0x0003E053 File Offset: 0x0003C253
	// (set) Token: 0x06001487 RID: 5255 RVA: 0x0003E05B File Offset: 0x0003C25B
	public bool DisableXPBonuses
	{
		get
		{
			return this.m_disableXPBonuses;
		}
		set
		{
			this.m_disableXPBonuses = value;
		}
	}

	// Token: 0x17000A91 RID: 2705
	// (get) Token: 0x06001488 RID: 5256 RVA: 0x0003E064 File Offset: 0x0003C264
	// (set) Token: 0x06001489 RID: 5257 RVA: 0x0003E06C File Offset: 0x0003C26C
	public bool DisableHPMPBonuses
	{
		get
		{
			return this.m_disableHPMPBonuses;
		}
		set
		{
			this.m_disableHPMPBonuses = value;
		}
	}

	// Token: 0x17000A92 RID: 2706
	// (get) Token: 0x0600148A RID: 5258 RVA: 0x0003E075 File Offset: 0x0003C275
	public bool IsVulnerableToLifeSteal
	{
		get
		{
			return !this.m_disableHPMPBonuses;
		}
	}

	// Token: 0x17000A93 RID: 2707
	// (get) Token: 0x0600148B RID: 5259 RVA: 0x0003E080 File Offset: 0x0003C280
	// (set) Token: 0x0600148C RID: 5260 RVA: 0x0003E088 File Offset: 0x0003C288
	public bool ResetToNeutralWhenUnculling { get; set; } = true;

	// Token: 0x17000A94 RID: 2708
	// (get) Token: 0x0600148D RID: 5261 RVA: 0x0003E091 File Offset: 0x0003C291
	// (set) Token: 0x0600148E RID: 5262 RVA: 0x0003E099 File Offset: 0x0003C299
	public bool DisableCulling { get; set; }

	// Token: 0x17000A95 RID: 2709
	// (get) Token: 0x0600148F RID: 5263 RVA: 0x0003E0A2 File Offset: 0x0003C2A2
	public GameObject[] CulledObjectsArray
	{
		get
		{
			return this.m_culledObjectsArray;
		}
	}

	// Token: 0x17000A96 RID: 2710
	// (get) Token: 0x06001490 RID: 5264 RVA: 0x0003E0AA File Offset: 0x0003C2AA
	// (set) Token: 0x06001491 RID: 5265 RVA: 0x0003E0B2 File Offset: 0x0003C2B2
	public bool LeftSidePlatformDropPrevented { get; set; }

	// Token: 0x17000A97 RID: 2711
	// (get) Token: 0x06001492 RID: 5266 RVA: 0x0003E0BB File Offset: 0x0003C2BB
	// (set) Token: 0x06001493 RID: 5267 RVA: 0x0003E0C3 File Offset: 0x0003C2C3
	public bool RightSidePlatformDropPrevented { get; set; }

	// Token: 0x17000A98 RID: 2712
	// (get) Token: 0x06001494 RID: 5268 RVA: 0x0003E0CC File Offset: 0x0003C2CC
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000A99 RID: 2713
	// (get) Token: 0x06001495 RID: 5269 RVA: 0x0003E0CF File Offset: 0x0003C2CF
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000A9A RID: 2714
	// (get) Token: 0x06001496 RID: 5270 RVA: 0x0003E0D2 File Offset: 0x0003C2D2
	// (set) Token: 0x06001497 RID: 5271 RVA: 0x0003E0DA File Offset: 0x0003C2DA
	public float ModeshiftDamageMod { get; set; } = 1f;

	// Token: 0x17000A9B RID: 2715
	// (get) Token: 0x06001498 RID: 5272 RVA: 0x0003E0E3 File Offset: 0x0003C2E3
	public bool IsDotDamage
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000A9C RID: 2716
	// (get) Token: 0x06001499 RID: 5273 RVA: 0x0003E0E8 File Offset: 0x0003C2E8
	public override float ActualStrength
	{
		get
		{
			float num = 1f;
			num += this.GetInsightBossDamageMod() - 1f;
			float actualStrength = base.ActualStrength;
			int level = SaveManager.PlayerSaveData.GetRelic(RelicType.DamageReductionStatusEffect).Level;
			if (level > 0 && base.StatusEffectController.HasAnyActiveStatusEffect(false))
			{
				float num2 = 0.15f * (float)level - 1f;
				num += num2;
			}
			if (this.IsCommander)
			{
				num += 0f;
			}
			if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Size))
			{
				num += 0.5f;
			}
			if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Aggro))
			{
				num += (float)(base.StatusEffectController.GetStatusEffect(StatusEffectType.Enemy_Aggro).TimesStacked - 1) * 0.08f;
			}
			num = Mathf.Clamp(num, 0f, float.MaxValue);
			return actualStrength * num;
		}
	}

	// Token: 0x17000A9D RID: 2717
	// (get) Token: 0x0600149A RID: 5274 RVA: 0x0003E1BC File Offset: 0x0003C3BC
	public override int ActualMaxHealth
	{
		get
		{
			if (this.EnemyType == EnemyType.Target)
			{
				return 1;
			}
			float num = (float)(this.BaseMaxHealth + base.MaxHealthAdd + base.MaxHealthTemporaryAdd) * (1f + base.MaxHealthMod + base.MaxHealthTemporaryMod);
			if (SaveManager.PlayerSaveData.EnableHouseRules)
			{
				num *= SaveManager.PlayerSaveData.Assist_EnemyHealthMod;
			}
			return Mathf.Clamp(Mathf.FloorToInt(num), 1, int.MaxValue);
		}
	}

	// Token: 0x17000A9E RID: 2718
	// (get) Token: 0x0600149B RID: 5275 RVA: 0x0003E22C File Offset: 0x0003C42C
	// (set) Token: 0x0600149C RID: 5276 RVA: 0x0003E234 File Offset: 0x0003C434
	public EnemySpawnController EnemySpawnController { get; set; }

	// Token: 0x17000A9F RID: 2719
	// (get) Token: 0x0600149D RID: 5277 RVA: 0x0003E23D File Offset: 0x0003C43D
	// (set) Token: 0x0600149E RID: 5278 RVA: 0x0003E245 File Offset: 0x0003C445
	public ISummoner Summoner { get; set; }

	// Token: 0x17000AA0 RID: 2720
	// (get) Token: 0x0600149F RID: 5279 RVA: 0x0003E24E File Offset: 0x0003C44E
	public bool IsSummoned
	{
		get
		{
			return this.Summoner != null;
		}
	}

	// Token: 0x17000AA1 RID: 2721
	// (get) Token: 0x060014A0 RID: 5280 RVA: 0x0003E259 File Offset: 0x0003C459
	// (set) Token: 0x060014A1 RID: 5281 RVA: 0x0003E261 File Offset: 0x0003C461
	public float GroundHorizontalVelocity { get; set; }

	// Token: 0x17000AA2 RID: 2722
	// (get) Token: 0x060014A2 RID: 5282 RVA: 0x0003E26A File Offset: 0x0003C46A
	// (set) Token: 0x060014A3 RID: 5283 RVA: 0x0003E272 File Offset: 0x0003C472
	public float JumpHorizontalVelocity { get; set; }

	// Token: 0x17000AA3 RID: 2723
	// (get) Token: 0x060014A4 RID: 5284 RVA: 0x0003E27B File Offset: 0x0003C47B
	// (set) Token: 0x060014A5 RID: 5285 RVA: 0x0003E283 File Offset: 0x0003C483
	public bool IsBoss
	{
		get
		{
			return this.m_isBoss;
		}
		set
		{
			this.m_isBoss = value;
		}
	}

	// Token: 0x17000AA4 RID: 2724
	// (get) Token: 0x060014A6 RID: 5286 RVA: 0x0003E28C File Offset: 0x0003C48C
	public override float BaseScaleToOffsetWith
	{
		get
		{
			return this.m_baseScaleToOffsetWith;
		}
	}

	// Token: 0x17000AA5 RID: 2725
	// (get) Token: 0x060014A7 RID: 5287 RVA: 0x0003E294 File Offset: 0x0003C494
	public IRelayLink<object, EnemyActivationStateChangedEventArgs> OnActivatedRelay
	{
		get
		{
			return this.m_onActivatedRelay.link;
		}
	}

	// Token: 0x17000AA6 RID: 2726
	// (get) Token: 0x060014A8 RID: 5288 RVA: 0x0003E2A1 File Offset: 0x0003C4A1
	public IRelayLink<object, EnemyActivationStateChangedEventArgs> OnDeactivatedRelay
	{
		get
		{
			return this.m_onDeactivatedRelay.link;
		}
	}

	// Token: 0x17000AA7 RID: 2727
	// (get) Token: 0x060014A9 RID: 5289 RVA: 0x0003E2AE File Offset: 0x0003C4AE
	public IRelayLink<object, EnemyActivationStateChangedEventArgs> OnReactivationTimedOutRelay
	{
		get
		{
			return this.m_onReactivationTimedOutRelay.link;
		}
	}

	// Token: 0x17000AA8 RID: 2728
	// (get) Token: 0x060014AA RID: 5290 RVA: 0x0003E2BB File Offset: 0x0003C4BB
	public IRelayLink<object, EnemyDeathEventArgs> OnEnemyDeathRelay
	{
		get
		{
			return this.m_onEnemyDeathRelay.link;
		}
	}

	// Token: 0x17000AA9 RID: 2729
	// (get) Token: 0x060014AB RID: 5291 RVA: 0x0003E2C8 File Offset: 0x0003C4C8
	public IRelayLink<object, EventArgs> OnResetPositionRelay
	{
		get
		{
			return this.m_onResetPositionRelay.link;
		}
	}

	// Token: 0x17000AAA RID: 2730
	// (get) Token: 0x060014AC RID: 5292 RVA: 0x0003E2D5 File Offset: 0x0003C4D5
	public IRelayLink OnDisableRelay
	{
		get
		{
			return this.m_onDisableRelay.link;
		}
	}

	// Token: 0x17000AAB RID: 2731
	// (get) Token: 0x060014AD RID: 5293 RVA: 0x0003E2E2 File Offset: 0x0003C4E2
	public Relay<object, EventArgs> OnPositionedForSummoningRelay
	{
		get
		{
			return this.m_onPositionedForSummoningRelay;
		}
	}

	// Token: 0x17000AAC RID: 2732
	// (get) Token: 0x060014AE RID: 5294 RVA: 0x0003E2EA File Offset: 0x0003C4EA
	// (set) Token: 0x060014AF RID: 5295 RVA: 0x0003E2FC File Offset: 0x0003C4FC
	public override bool DisableFriction
	{
		get
		{
			return this.IsFlying || base.DisableFriction;
		}
		set
		{
			base.DisableFriction = value;
		}
	}

	// Token: 0x17000AAD RID: 2733
	// (get) Token: 0x060014B0 RID: 5296 RVA: 0x0003E305 File Offset: 0x0003C505
	// (set) Token: 0x060014B1 RID: 5297 RVA: 0x0003E30D File Offset: 0x0003C50D
	public bool ForceActivate
	{
		get
		{
			return this.m_forceActivate;
		}
		set
		{
			this.m_forceActivate = value;
		}
	}

	// Token: 0x17000AAE RID: 2734
	// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0003E316 File Offset: 0x0003C516
	// (set) Token: 0x060014B3 RID: 5299 RVA: 0x0003E31E File Offset: 0x0003C51E
	public StrikeType StrikeType
	{
		get
		{
			return this.m_strikeType;
		}
		set
		{
			this.m_strikeType = value;
		}
	}

	// Token: 0x17000AAF RID: 2735
	// (get) Token: 0x060014B4 RID: 5300 RVA: 0x0003E327 File Offset: 0x0003C527
	// (set) Token: 0x060014B5 RID: 5301 RVA: 0x0003E32F File Offset: 0x0003C52F
	public bool AlwaysDealsContactDamage
	{
		get
		{
			return this.m_alwaysDealsContactDamage;
		}
		set
		{
			this.m_alwaysDealsContactDamage = value;
		}
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x0003E338 File Offset: 0x0003C538
	public override void SetVelocity(float velocityX, float velocityY, bool additive)
	{
		this.m_previousAirVelocityX = velocityX;
		base.SetVelocity(velocityX, velocityY, additive);
	}

	// Token: 0x060014B7 RID: 5303 RVA: 0x0003E34A File Offset: 0x0003C54A
	public override void SetVelocityX(float velocity, bool additive)
	{
		this.m_previousAirVelocityX = velocity;
		base.SetVelocityX(velocity, additive);
	}

	// Token: 0x17000AB0 RID: 2736
	// (get) Token: 0x060014B8 RID: 5304 RVA: 0x0003E35B File Offset: 0x0003C55B
	// (set) Token: 0x060014B9 RID: 5305 RVA: 0x0003E363 File Offset: 0x0003C563
	public float FlightKnockbackDecelerationMod
	{
		get
		{
			return this.m_flightKnockbackDecelerationMod;
		}
		set
		{
			this.m_flightKnockbackDecelerationMod = value;
		}
	}

	// Token: 0x17000AB1 RID: 2737
	// (get) Token: 0x060014BA RID: 5306 RVA: 0x0003E36C File Offset: 0x0003C56C
	// (set) Token: 0x060014BB RID: 5307 RVA: 0x0003E374 File Offset: 0x0003C574
	public bool RicochetsAttackerOnHit
	{
		get
		{
			return this.m_knocksBackOnHit;
		}
		set
		{
			this.m_knocksBackOnHit = value;
		}
	}

	// Token: 0x17000AB2 RID: 2738
	// (get) Token: 0x060014BC RID: 5308 RVA: 0x0003E37D File Offset: 0x0003C57D
	// (set) Token: 0x060014BD RID: 5309 RVA: 0x0003E385 File Offset: 0x0003C585
	public EnemyData EnemyData
	{
		get
		{
			return this.m_enemyData;
		}
		set
		{
			this.m_enemyData = value;
			if (Application.isPlaying)
			{
				this.InitializeEnemyData();
			}
		}
	}

	// Token: 0x17000AB3 RID: 2739
	// (get) Token: 0x060014BE RID: 5310 RVA: 0x0003E39B File Offset: 0x0003C59B
	// (set) Token: 0x060014BF RID: 5311 RVA: 0x0003E3A3 File Offset: 0x0003C5A3
	public bool ConstrainToRoom
	{
		get
		{
			return this.m_constrainToRoom;
		}
		set
		{
			this.m_constrainToRoom = value;
		}
	}

	// Token: 0x17000AB4 RID: 2740
	// (get) Token: 0x060014C0 RID: 5312 RVA: 0x0003E3AC File Offset: 0x0003C5AC
	// (set) Token: 0x060014C1 RID: 5313 RVA: 0x0003E3B4 File Offset: 0x0003C5B4
	public LogicController LogicController
	{
		get
		{
			return this.m_logicController;
		}
		private set
		{
			this.m_logicController = value;
		}
	}

	// Token: 0x17000AB5 RID: 2741
	// (get) Token: 0x060014C2 RID: 5314 RVA: 0x0003E3BD File Offset: 0x0003C5BD
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x17000AB6 RID: 2742
	// (get) Token: 0x060014C3 RID: 5315 RVA: 0x0003E3C5 File Offset: 0x0003C5C5
	public bool IsTargetToMyRight
	{
		get
		{
			return this.Target && this.Target.transform.position.x > base.transform.position.x;
		}
	}

	// Token: 0x17000AB7 RID: 2743
	// (get) Token: 0x060014C4 RID: 5316 RVA: 0x0003E3FE File Offset: 0x0003C5FE
	// (set) Token: 0x060014C5 RID: 5317 RVA: 0x0003E406 File Offset: 0x0003C606
	public bool FollowTarget { get; set; }

	// Token: 0x17000AB8 RID: 2744
	// (get) Token: 0x060014C6 RID: 5318 RVA: 0x0003E40F File Offset: 0x0003C60F
	// (set) Token: 0x060014C7 RID: 5319 RVA: 0x0003E417 File Offset: 0x0003C617
	public Vector3 FollowOffset
	{
		get
		{
			return this.m_followOffset;
		}
		set
		{
			this.m_followOffset = value;
		}
	}

	// Token: 0x17000AB9 RID: 2745
	// (get) Token: 0x060014C8 RID: 5320 RVA: 0x0003E420 File Offset: 0x0003C620
	// (set) Token: 0x060014C9 RID: 5321 RVA: 0x0003E428 File Offset: 0x0003C628
	public FlyingMovementType FlyingMovementType { get; set; }

	// Token: 0x17000ABA RID: 2746
	// (get) Token: 0x060014CA RID: 5322 RVA: 0x0003E431 File Offset: 0x0003C631
	// (set) Token: 0x060014CB RID: 5323 RVA: 0x0003E439 File Offset: 0x0003C639
	public bool PivotFollowsOrientation
	{
		get
		{
			return this.m_pivotFollowsOrientation;
		}
		set
		{
			this.m_pivotFollowsOrientation = value;
			if (this.IsFlying && !this.AlwaysFacing && this.m_pivotFollowsOrientation && !base.IsFacingRight)
			{
				base.CharacterCorgi.Flip(false, true);
			}
		}
	}

	// Token: 0x17000ABB RID: 2747
	// (get) Token: 0x060014CC RID: 5324 RVA: 0x0003E46F File Offset: 0x0003C66F
	// (set) Token: 0x060014CD RID: 5325 RVA: 0x0003E477 File Offset: 0x0003C677
	public virtual bool AlwaysFacing
	{
		get
		{
			return this.m_alwaysFace;
		}
		set
		{
			this.m_alwaysFace = value;
		}
	}

	// Token: 0x17000ABC RID: 2748
	// (get) Token: 0x060014CE RID: 5326 RVA: 0x0003E480 File Offset: 0x0003C680
	// (set) Token: 0x060014CF RID: 5327 RVA: 0x0003E488 File Offset: 0x0003C688
	public GameObject Target
	{
		get
		{
			return this.m_target;
		}
		set
		{
			this.m_target = value;
			this.m_targetController = this.m_target.GetComponent<BaseCharacterController>();
		}
	}

	// Token: 0x17000ABD RID: 2749
	// (get) Token: 0x060014D0 RID: 5328 RVA: 0x0003E4A2 File Offset: 0x0003C6A2
	public BaseCharacterController TargetController
	{
		get
		{
			return this.m_targetController;
		}
	}

	// Token: 0x17000ABE RID: 2750
	// (get) Token: 0x060014D1 RID: 5329 RVA: 0x0003E4AA File Offset: 0x0003C6AA
	public virtual float ActualSummonValue
	{
		get
		{
			return Enemy_EV.GetSummonValue(this.EnemyRank);
		}
	}

	// Token: 0x17000ABF RID: 2751
	// (get) Token: 0x060014D2 RID: 5330 RVA: 0x0003E4B7 File Offset: 0x0003C6B7
	// (set) Token: 0x060014D3 RID: 5331 RVA: 0x0003E4BF File Offset: 0x0003C6BF
	public float BaseSpeed
	{
		get
		{
			return this.m_baseSpeed;
		}
		set
		{
			this.m_baseSpeed = value;
		}
	}

	// Token: 0x17000AC0 RID: 2752
	// (get) Token: 0x060014D4 RID: 5332 RVA: 0x0003E4C8 File Offset: 0x0003C6C8
	// (set) Token: 0x060014D5 RID: 5333 RVA: 0x0003E4D0 File Offset: 0x0003C6D0
	public float SpeedMod { get; protected set; }

	// Token: 0x17000AC1 RID: 2753
	// (get) Token: 0x060014D6 RID: 5334 RVA: 0x0003E4D9 File Offset: 0x0003C6D9
	// (set) Token: 0x060014D7 RID: 5335 RVA: 0x0003E4E1 File Offset: 0x0003C6E1
	public float SpeedAdd { get; protected set; }

	// Token: 0x17000AC2 RID: 2754
	// (get) Token: 0x060014D8 RID: 5336 RVA: 0x0003E4EA File Offset: 0x0003C6EA
	public virtual float ActualSpeed
	{
		get
		{
			return (this.BaseSpeed + this.SpeedAdd) * (1f + this.SpeedMod);
		}
	}

	// Token: 0x17000AC3 RID: 2755
	// (get) Token: 0x060014D9 RID: 5337 RVA: 0x0003E506 File Offset: 0x0003C706
	// (set) Token: 0x060014DA RID: 5338 RVA: 0x0003E50E File Offset: 0x0003C70E
	public float BaseTurnSpeed
	{
		get
		{
			return this.m_baseTurnSpeed;
		}
		set
		{
			this.m_baseTurnSpeed = value;
		}
	}

	// Token: 0x17000AC4 RID: 2756
	// (get) Token: 0x060014DB RID: 5339 RVA: 0x0003E517 File Offset: 0x0003C717
	public virtual float ActualTurnSpeed
	{
		get
		{
			return this.BaseTurnSpeed;
		}
	}

	// Token: 0x17000AC5 RID: 2757
	// (get) Token: 0x060014DC RID: 5340 RVA: 0x0003E51F File Offset: 0x0003C71F
	// (set) Token: 0x060014DD RID: 5341 RVA: 0x0003E527 File Offset: 0x0003C727
	public Vector2 BaseRestCooldown
	{
		get
		{
			return this.m_baseRestCooldown;
		}
		set
		{
			this.m_baseRestCooldown = value;
		}
	}

	// Token: 0x17000AC6 RID: 2758
	// (get) Token: 0x060014DE RID: 5342 RVA: 0x0003E530 File Offset: 0x0003C730
	public virtual Vector2 ActualRestCooldown
	{
		get
		{
			return this.BaseRestCooldown;
		}
	}

	// Token: 0x17000AC7 RID: 2759
	// (get) Token: 0x060014DF RID: 5343 RVA: 0x0003E538 File Offset: 0x0003C738
	// (set) Token: 0x060014E0 RID: 5344 RVA: 0x0003E540 File Offset: 0x0003C740
	public virtual bool IsFlying
	{
		get
		{
			return this.m_isFlying;
		}
		set
		{
			this.m_isFlying = value;
			if (this.PreventPlatformDropObj && this.PreventPlatformDropObj.enabled != !value && (value || (!value && !this.FallLedge)))
			{
				this.PreventPlatformDropObj.enabled = !value;
			}
			if (base.IsInitialized)
			{
				base.ControllerCorgi.GravityActive(!value);
				base.ControllerCorgi.StickWhenWalkingDownSlopes = !value;
				base.ControllerCorgi.DisableOneWayCollision = value;
			}
		}
	}

	// Token: 0x17000AC8 RID: 2760
	// (get) Token: 0x060014E1 RID: 5345 RVA: 0x0003E5C1 File Offset: 0x0003C7C1
	// (set) Token: 0x060014E2 RID: 5346 RVA: 0x0003E5C9 File Offset: 0x0003C7C9
	public virtual bool CollidesWithPlatforms
	{
		get
		{
			return this.m_collidesWithPlatforms;
		}
		set
		{
			this.m_collidesWithPlatforms = value;
			if (base.ControllerCorgi)
			{
				base.ControllerCorgi.DisablePlatformCollision = !value;
			}
		}
	}

	// Token: 0x17000AC9 RID: 2761
	// (get) Token: 0x060014E3 RID: 5347 RVA: 0x0003E5EE File Offset: 0x0003C7EE
	// (set) Token: 0x060014E4 RID: 5348 RVA: 0x0003E5F6 File Offset: 0x0003C7F6
	public float BaseStunStrength
	{
		get
		{
			return this.m_baseStunStrength;
		}
		set
		{
			this.m_baseStunStrength = value;
		}
	}

	// Token: 0x17000ACA RID: 2762
	// (get) Token: 0x060014E5 RID: 5349 RVA: 0x0003E5FF File Offset: 0x0003C7FF
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x17000ACB RID: 2763
	// (get) Token: 0x060014E6 RID: 5350 RVA: 0x0003E607 File Offset: 0x0003C807
	// (set) Token: 0x060014E7 RID: 5351 RVA: 0x0003E60F File Offset: 0x0003C80F
	public float BaseKnockbackStrength
	{
		get
		{
			return this.m_baseKnockbackStrength;
		}
		set
		{
			this.m_baseKnockbackStrength = value;
		}
	}

	// Token: 0x17000ACC RID: 2764
	// (get) Token: 0x060014E8 RID: 5352 RVA: 0x0003E618 File Offset: 0x0003C818
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x17000ACD RID: 2765
	// (get) Token: 0x060014E9 RID: 5353 RVA: 0x0003E620 File Offset: 0x0003C820
	// (set) Token: 0x060014EA RID: 5354 RVA: 0x0003E628 File Offset: 0x0003C828
	public float BaseDropOdds
	{
		get
		{
			return this.m_baseDropOdds;
		}
		set
		{
			this.m_baseDropOdds = value;
		}
	}

	// Token: 0x17000ACE RID: 2766
	// (get) Token: 0x060014EB RID: 5355 RVA: 0x0003E631 File Offset: 0x0003C831
	public virtual float ActualDropOdds
	{
		get
		{
			return this.BaseDropOdds;
		}
	}

	// Token: 0x17000ACF RID: 2767
	// (get) Token: 0x060014EC RID: 5356 RVA: 0x0003E639 File Offset: 0x0003C839
	// (set) Token: 0x060014ED RID: 5357 RVA: 0x0003E641 File Offset: 0x0003C841
	public virtual float CloseRangeRadius
	{
		get
		{
			return this.m_closeRadius;
		}
		set
		{
			this.m_closeRadius = value;
			this.m_closeRadius = Mathf.Clamp(this.m_closeRadius, 0f, this.m_mediumRadius);
		}
	}

	// Token: 0x17000AD0 RID: 2768
	// (get) Token: 0x060014EE RID: 5358 RVA: 0x0003E666 File Offset: 0x0003C866
	// (set) Token: 0x060014EF RID: 5359 RVA: 0x0003E66E File Offset: 0x0003C86E
	public virtual float MediumRangeRadius
	{
		get
		{
			return this.m_mediumRadius;
		}
		set
		{
			this.m_mediumRadius = value;
			this.m_mediumRadius = Mathf.Clamp(this.m_mediumRadius, this.m_closeRadius, this.m_farRadius);
		}
	}

	// Token: 0x17000AD1 RID: 2769
	// (get) Token: 0x060014F0 RID: 5360 RVA: 0x0003E694 File Offset: 0x0003C894
	// (set) Token: 0x060014F1 RID: 5361 RVA: 0x0003E6E2 File Offset: 0x0003C8E2
	public virtual float FarRangeRadius
	{
		get
		{
			if (CameraController.ZoomLevel == 1f)
			{
				return this.m_farRadius;
			}
			float num = (CameraController.ZoomLevel - 1f) * 0.5f + 1f;
			if (num < 1f)
			{
				num = 1f;
			}
			return num * this.m_farRadius;
		}
		set
		{
			this.m_farRadius = value;
			this.m_farRadius = Mathf.Clamp(this.m_farRadius, this.m_mediumRadius, float.MaxValue);
		}
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x0003E707 File Offset: 0x0003C907
	public override void SetHealth(float value, bool additive, bool runEvents)
	{
		base.SetHealth(value, additive, runEvents);
		if (runEvents)
		{
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EnemyHealthChange, this, this.m_healthChangeEventArgs);
		}
	}

	// Token: 0x17000AD2 RID: 2770
	// (get) Token: 0x060014F3 RID: 5363 RVA: 0x0003E722 File Offset: 0x0003C922
	public int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x060014F4 RID: 5364 RVA: 0x0003E72A File Offset: 0x0003C92A
	public void SetLevel(int value)
	{
		this.m_level = value;
		this.InjectLevel();
		if (base.IsInitialized)
		{
			this.InitializeLevelMods();
			this.ResetHealth();
		}
		this.m_level = Mathf.Clamp(this.m_level, 1, int.MaxValue);
	}

	// Token: 0x17000AD3 RID: 2771
	// (get) Token: 0x060014F5 RID: 5365 RVA: 0x0003E764 File Offset: 0x0003C964
	// (set) Token: 0x060014F6 RID: 5366 RVA: 0x0003E76C File Offset: 0x0003C96C
	public EnemyType EnemyType
	{
		get
		{
			return this.m_enemyType;
		}
		set
		{
			this.m_enemyType = value;
			LogicController component = base.GetComponent<LogicController>();
			if (component)
			{
				component.UpdateLogicReferences();
			}
			this.UpdateEnemyDataScale();
		}
	}

	// Token: 0x17000AD4 RID: 2772
	// (get) Token: 0x060014F7 RID: 5367 RVA: 0x0003E79B File Offset: 0x0003C99B
	// (set) Token: 0x060014F8 RID: 5368 RVA: 0x0003E7A4 File Offset: 0x0003C9A4
	public EnemyRank EnemyRank
	{
		get
		{
			return this.m_enemyRank;
		}
		set
		{
			this.m_enemyRank = value;
			LogicController component = base.GetComponent<LogicController>();
			if (component)
			{
				component.UpdateLogicReferences();
			}
			this.UpdateEnemyDataScale();
		}
	}

	// Token: 0x17000AD5 RID: 2773
	// (get) Token: 0x060014F9 RID: 5369 RVA: 0x0003E7D3 File Offset: 0x0003C9D3
	// (set) Token: 0x060014FA RID: 5370 RVA: 0x0003E7DB File Offset: 0x0003C9DB
	public BaseRoom Room { get; private set; }

	// Token: 0x17000AD6 RID: 2774
	// (get) Token: 0x060014FB RID: 5371 RVA: 0x0003E7E4 File Offset: 0x0003C9E4
	// (set) Token: 0x060014FC RID: 5372 RVA: 0x0003E7EC File Offset: 0x0003C9EC
	public int EnemyIndex
	{
		get
		{
			return this.m_enemyIndex;
		}
		private set
		{
			this.m_enemyIndex = value;
		}
	}

	// Token: 0x17000AD7 RID: 2775
	// (get) Token: 0x060014FD RID: 5373 RVA: 0x0003E7F5 File Offset: 0x0003C9F5
	public bool TouchingLeftRoomEdge
	{
		get
		{
			return this.m_touchingLeftRoomEdge;
		}
	}

	// Token: 0x17000AD8 RID: 2776
	// (get) Token: 0x060014FE RID: 5374 RVA: 0x0003E7FD File Offset: 0x0003C9FD
	public bool TouchingRightRoomEdge
	{
		get
		{
			return this.m_touchingRightRoomEdge;
		}
	}

	// Token: 0x17000AD9 RID: 2777
	// (get) Token: 0x060014FF RID: 5375 RVA: 0x0003E805 File Offset: 0x0003CA05
	public bool TouchingTopRoomEdge
	{
		get
		{
			return this.m_touchingTopRoomEdge;
		}
	}

	// Token: 0x17000ADA RID: 2778
	// (get) Token: 0x06001500 RID: 5376 RVA: 0x0003E80D File Offset: 0x0003CA0D
	public bool TouchingBottomRoomEdge
	{
		get
		{
			return this.m_touchingBottomRoomEdge;
		}
	}

	// Token: 0x17000ADB RID: 2779
	// (get) Token: 0x06001501 RID: 5377 RVA: 0x0003E815 File Offset: 0x0003CA15
	// (set) Token: 0x06001502 RID: 5378 RVA: 0x0003E81D File Offset: 0x0003CA1D
	public bool ActivatedByFairyRoomTrigger { get; set; }

	// Token: 0x17000ADC RID: 2780
	// (get) Token: 0x06001503 RID: 5379 RVA: 0x0003E826 File Offset: 0x0003CA26
	// (set) Token: 0x06001504 RID: 5380 RVA: 0x0003E82E File Offset: 0x0003CA2E
	public bool PreserveRotationWhenDeactivated { get; set; }

	// Token: 0x17000ADD RID: 2781
	// (get) Token: 0x06001505 RID: 5381 RVA: 0x0003E837 File Offset: 0x0003CA37
	// (set) Token: 0x06001506 RID: 5382 RVA: 0x0003E83F File Offset: 0x0003CA3F
	public bool ForceDisableSummonOffset { get; set; }

	// Token: 0x17000ADE RID: 2782
	// (get) Token: 0x06001507 RID: 5383 RVA: 0x0003E848 File Offset: 0x0003CA48
	// (set) Token: 0x06001508 RID: 5384 RVA: 0x0003E850 File Offset: 0x0003CA50
	public bool InvisibleDuringSummonAnim { get; set; }

	// Token: 0x06001509 RID: 5385 RVA: 0x0003E85C File Offset: 0x0003CA5C
	protected override void Awake()
	{
		base.Awake();
		this.m_preventPlatformDropObj = base.GetComponent<PreventPlatformDrop>();
		if (EnemyController.m_enemyDeathEventArgs_STATIC == null)
		{
			EnemyController.m_enemyDeathEventArgs_STATIC = new EnemyDeathEventArgs(null, null);
			EnemyController.m_enemyActivationStateChangedEventArgs_STATIC = new EnemyActivationStateChangedEventArgs(null);
		}
		EnemyClassData enemyClassData = EnemyClassLibrary.GetEnemyClassData(this.EnemyType);
		EnemyData enemyData = enemyClassData ? enemyClassData.GetEnemyData(this.EnemyRank) : null;
		if (this.m_controllerCorgi && (!enemyData || (enemyData && !enemyData.CollidesWithPlatforms)))
		{
			this.m_controllerCorgi.DisablePlatformCollision = true;
		}
		CameraLayerController component = base.GetComponent<CameraLayerController>();
		component.SetCameraLayer(CameraLayer.Game);
		int subLayer = EnemySubLayerManager.GetSubLayer(new EnemyTypeAndRank(this.EnemyType, this.EnemyRank));
		component.SetSubLayer(subLayer, false);
		this.m_logicController = base.GetComponent<LogicController>();
		if (!this.m_logicController)
		{
			throw new MissingComponentException("LogicController");
		}
		this.IsAwakeCalled = true;
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x0003E944 File Offset: 0x0003CB44
	protected override IEnumerator Start()
	{
		this.m_enemyInitializing = true;
		while (!this.m_controllerCorgi.IsInitialized || !this.m_characterCorgi.IsInitialized)
		{
			yield return null;
		}
		this.InitializeEnemyData();
		this.m_controllerCorgi.SetRaysParameters();
		if (!this.IsFlying)
		{
			float num = this.m_controllerCorgi.BoundsWidth / 2f;
			if (Mathf.Abs(this.m_controllerCorgi.BoxCollider.offset.x) + num > 1f)
			{
				this.m_controllerCorgi.CastRaysOnBothSides = true;
			}
		}
		if (Application.isPlaying)
		{
			base.ControllerCorgi.GravityActive(!this.IsFlying);
			base.ControllerCorgi.StickWhenWalkingDownSlopes = !this.IsFlying;
			base.ControllerCorgi.DisableOneWayCollision = this.IsFlying;
			if (this.IsFlying)
			{
				this.SetVelocity(0f, 0f, false);
			}
		}
		this.m_target = PlayerManager.GetPlayer();
		this.m_targetController = PlayerManager.GetPlayerController();
		yield return base.Start();
		this.m_enemyInitializing = false;
		yield break;
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x0003E953 File Offset: 0x0003CB53
	public void ForceFaceTarget()
	{
		if (this.Target)
		{
			base.Heading = this.TargetController.Midpoint - base.Midpoint;
		}
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x0003E984 File Offset: 0x0003CB84
	public void InitializeEnemyData()
	{
		if (this.EnemyType == EnemyType.None)
		{
			return;
		}
		EnemyClassData enemyClassData = EnemyClassLibrary.GetEnemyClassData(this.EnemyType);
		if (enemyClassData)
		{
			EnemyData enemyData = enemyClassData.GetEnemyData(this.EnemyRank);
			if (enemyData)
			{
				EnemyData enemyData2 = null;
				EnemyTypeAndRank key = new EnemyTypeAndRank(this.EnemyType, this.EnemyRank);
				if (!EnemyController.EnemyData_StaticInstanceDict.TryGetValue(key, out enemyData2))
				{
					enemyData2 = UnityEngine.Object.Instantiate<EnemyData>(enemyData);
					EnemyController.EnemyData_StaticInstanceDict.Add(key, enemyData2);
				}
				this.m_enemyData = enemyData2;
			}
		}
		if (this.m_enemyData)
		{
			this.ResetBaseValues();
			this.ResetHealth();
		}
	}

	// Token: 0x0600150D RID: 5389 RVA: 0x0003EA1A File Offset: 0x0003CC1A
	private void OnEnable()
	{
		if (!base.IsInitialized && this.m_enemyInitializing)
		{
			base.StartCoroutine(this.Start());
		}
		this.DisableEffectsOnSpawn();
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x0003EA3F File Offset: 0x0003CC3F
	private void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
		this.FlightKnockbackAcceleration = Vector2.zero;
		if (!GameManager.IsApplicationClosing)
		{
			this.m_onDisableRelay.Dispatch();
		}
	}

	// Token: 0x0600150F RID: 5391 RVA: 0x0003EA68 File Offset: 0x0003CC68
	public void UpdateEnemyDataScale()
	{
		if (this.m_enemyData)
		{
			base.transform.localScale = new Vector3(this.m_enemyData.Scale, this.m_enemyData.Scale, Mathf.Clamp(this.m_enemyData.Scale, 0f, 1f));
		}
		else
		{
			EnemyClassData enemyClassData = EnemyClassLibrary.GetEnemyClassData(this.EnemyType);
			if (enemyClassData)
			{
				EnemyData enemyData = enemyClassData.GetEnemyData(this.EnemyRank);
				if (enemyData)
				{
					base.transform.localScale = new Vector3(enemyData.Scale, enemyData.Scale, Mathf.Clamp(enemyData.Scale, 0f, 1f));
				}
			}
		}
		if (Application.isPlaying)
		{
			if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Size))
			{
				base.transform.localScale *= 1.25f;
			}
			this.UpdateBounds();
			base.StatusBarController.ResetPositionAndScale(this);
		}
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x0003EB64 File Offset: 0x0003CD64
	public void ForceStandingOn(Collider2D collider)
	{
		if (this.m_controllerCorgi)
		{
			this.m_controllerCorgi.ForceStandingOn(collider);
		}
	}

	// Token: 0x06001511 RID: 5393 RVA: 0x0003EB7F File Offset: 0x0003CD7F
	public void DisableEffectsOnSpawn()
	{
		base.StartCoroutine(this.DisableEffectsOnSpawnCoroutine(0.1f));
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x0003EB93 File Offset: 0x0003CD93
	private IEnumerator DisableEffectsOnSpawnCoroutine(float duration)
	{
		if (this.m_animator)
		{
			EffectManager.AddAnimatorToDisableList(this.m_animator);
			float delay = Time.time + duration;
			while (!base.IsInitialized)
			{
				yield return null;
			}
			if (global::AnimatorUtility.HasState(this.m_animator, EnemyController.LAND_ANIMATION_STATE_NAME))
			{
				this.m_animator.Play(EnemyController.LAND_ANIMATION_STATE_NAME, global::AnimatorUtility.GetLayerIndex(this.m_animator, EnemyController.LAND_ANIMATION_STATE_NAME), 1f);
				this.m_animator.Update(1f);
			}
			while (Time.time < delay)
			{
				yield return null;
			}
			EffectManager.RemoveAnimatorFromDisableList(this.m_animator);
		}
		yield break;
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x0003EBA9 File Offset: 0x0003CDA9
	public void ResetCollisionState()
	{
		if (this.m_controllerCorgi)
		{
			this.m_controllerCorgi.ResetState();
		}
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x0003EBC3 File Offset: 0x0003CDC3
	public void UpdateBounds()
	{
		if (this.m_controllerCorgi)
		{
			this.m_controllerCorgi.SetRaysParameters();
		}
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x0003EBE0 File Offset: 0x0003CDE0
	public override void ResetBaseValues()
	{
		if (GameManager.IsApplicationClosing)
		{
			return;
		}
		base.ResetBaseValues();
		if (this.m_enemyData == null)
		{
			return;
		}
		this.BaseMaxHealth = this.m_enemyData.Health;
		base.BaseStrength = (float)this.m_enemyData.WeaponDamage;
		base.BaseMagic = (float)this.m_enemyData.MagicDamage;
		this.BaseSpeed = this.m_enemyData.Speed;
		this.BaseTurnSpeed = this.m_enemyData.TurnSpeed;
		this.BaseRestCooldown = new Vector2(this.m_enemyData.RestMinCooldown, this.m_enemyData.RestMaxCooldown);
		bool isGravityActive = base.ControllerCorgi.IsGravityActive;
		this.IsFlying = this.m_enemyData.IsFlying;
		base.ControllerCorgi.GravityActive(isGravityActive);
		this.AlwaysFacing = this.m_enemyData.AlwaysFace;
		this.FallLedge = this.m_enemyData.FallLedge;
		this.CollidesWithPlatforms = this.m_enemyData.CollidesWithPlatforms;
		this.BaseStunDefense = (float)this.m_enemyData.StunDefence;
		this.BaseKnockbackDefense = (float)this.m_enemyData.KnockbackDefence;
		base.StunDuration = this.m_enemyData.StunDuration;
		this.InternalKnockbackMod = new Vector2(this.m_enemyData.KnockbackModX, this.m_enemyData.KnockbackModY);
		base.BaseScale = this.m_enemyData.Scale;
		this.m_baseScaleToOffsetWith = EnemyClassLibrary.GetEnemyClassData(this.EnemyType).GetEnemyData(EnemyRank.Basic).Scale;
		this.BaseDropOdds = this.m_enemyData.DropOdds;
		this.FarRangeRadius = this.m_enemyData.FarRadius;
		this.MediumRangeRadius = this.m_enemyData.ProjectileRadius;
		this.CloseRangeRadius = this.m_enemyData.MeleeRadius;
		this.ModeshiftDamageMod = 1f;
		this.UpdateEnemyDataScale();
		this.JumpHorizontalVelocity = 0f;
		this.GroundHorizontalVelocity = 0f;
		this.InitializeLevelMods();
		float num = 1f;
		if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Speed))
		{
			num += 1.25f;
		}
		if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Aggro))
		{
			num += (float)(base.StatusEffectController.GetStatusEffect(StatusEffectType.Enemy_Aggro).TimesStacked - 1) * 0.12f;
		}
		this.BaseSpeed *= num;
		this.BaseTurnSpeed *= num;
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x0003EE44 File Offset: 0x0003D044
	public void InitializeLevelMods()
	{
		int num = this.Level;
		if (num < 1)
		{
			Debug.LogFormat("<color=red>| {0} | Enemy Level should be greater than 0, but isn't</color>", new object[]
			{
				this
			});
			num = 0;
		}
		base.MaxHealthAdd = Mathf.RoundToInt((float)Mathf.Clamp(num - 1, 0, int.MaxValue) * 0.0625f * (float)this.BaseMaxHealth);
		if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Size))
		{
			base.MaxHealthAdd += Mathf.RoundToInt(1f * (float)this.BaseMaxHealth);
		}
		base.StrengthAdd = Mathf.RoundToInt((float)Mathf.Clamp(num - 1, 0, int.MaxValue) * 0.0575f * base.BaseStrength);
		base.MaxHealthMod = 0f;
		base.StrengthMod = 0f;
		this.SpeedAdd = 0f;
		float burdenStatGain = BurdenManager.GetBurdenStatGain(BurdenType.EnemyHealth);
		base.MaxHealthMod += burdenStatGain;
		float num2 = 0.01f * (float)SaveManager.PlayerSaveData.NewGamePlusLevel;
		base.MaxHealthMod += num2;
		if (this.IsBoss)
		{
			base.MaxHealthMod += 0.14999998f;
		}
		if (this.IsCommander)
		{
			base.MaxHealthMod += 0.75f;
		}
		base.MaxHealthMod += EnemyHPMod_BiomeRule.MaxHealthMod - 1f;
		float burdenStatGain2 = BurdenManager.GetBurdenStatGain(BurdenType.EnemyDamage);
		base.StrengthMod += burdenStatGain2;
		float num3 = 0.007f * (float)SaveManager.PlayerSaveData.NewGamePlusLevel;
		base.StrengthMod += num3;
		base.StrengthMod += EnemyDamageMod_BiomeRule.StrengthMod - 1f;
		if (this.EnemyType != EnemyType.Eggplant)
		{
			float burdenStatGain3 = BurdenManager.GetBurdenStatGain(BurdenType.EnemySpeed);
			if (this.BaseSpeed > 0f)
			{
				this.SpeedAdd += burdenStatGain3;
			}
		}
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x0003F014 File Offset: 0x0003D214
	public void AddCommanderStatusEffect(StatusEffectType statusEffect)
	{
		int num = StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.IndexOf(statusEffect);
		if (num != -1)
		{
			this.m_commanderStatusEffectFlags |= 1 << num;
		}
	}

	// Token: 0x06001518 RID: 5400 RVA: 0x0003F044 File Offset: 0x0003D244
	public void RemoveCommanderStatusEffect(StatusEffectType statusEffect)
	{
		int num = StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.IndexOf(statusEffect);
		if (num != -1)
		{
			this.m_commanderStatusEffectFlags &= ~(1 << num);
		}
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x0003F078 File Offset: 0x0003D278
	public bool HasCommanderStatusEffect(StatusEffectType statusEffect)
	{
		int num = StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.IndexOf(statusEffect);
		return num != -1 && (this.m_commanderStatusEffectFlags & 1 << num) != 0;
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x0003F0A7 File Offset: 0x0003D2A7
	public void RemoveAllCommanderStatusEffects()
	{
		this.m_commanderStatusEffectFlags = 0;
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x0003F0B0 File Offset: 0x0003D2B0
	public void AddRandomCommanderStatusEffect()
	{
		int num = RNGManager.GetRandomNumber(RngID.SpecialProps_RoomSeed, "EnemyController.CreateRandomCommanderStatusEffect", 0, StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.Length);
		int num2 = 0;
		while ((this.m_commanderStatusEffectFlags & 1 << num) != 0 && num2 < StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.Length)
		{
			num++;
			num2++;
			if (num >= StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.Length)
			{
				num = 0;
			}
		}
		if (num2 < StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.Length)
		{
			this.AddCommanderStatusEffect(StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY[num]);
		}
	}

	// Token: 0x0600151C RID: 5404 RVA: 0x0003F120 File Offset: 0x0003D320
	public void InitializeCommanderStatusEffects()
	{
		this.RemoveAllCommanderStatusEffects();
		if (this.EnemyType == EnemyType.BouncySpike || this.EnemyType == EnemyType.Wisp || this.EnemyType == EnemyType.Slug || this.EnemyType == EnemyType.Dummy || this.EnemyType == EnemyType.Target || this.EnemyType == EnemyType.Eggplant)
		{
			this.IsCommander = false;
			return;
		}
		if (this.IsCommander)
		{
			this.AddRandomCommanderStatusEffect();
			int num = Mathf.RoundToInt(BurdenManager.GetBurdenStatGain(BurdenType.CommanderTraits));
			for (int i = 0; i < num; i++)
			{
				this.AddRandomCommanderStatusEffect();
			}
		}
		if (this.IsBoss)
		{
			int num2 = Mathf.RoundToInt(BurdenManager.GetBurdenStatGain(BurdenType.BossPower));
			for (int j = 0; j < num2; j++)
			{
				this.AddRandomCommanderStatusEffect();
			}
		}
		if (SceneLoader_RL.CurrentScene != SceneLoadingUtility.GetSceneName(SceneID.Tutorial) && !this.IsCommander && !this.IsBoss && this.EnemyRank != EnemyRank.Miniboss && !this.IsSummoned && this.EnemyType != EnemyType.Eggplant)
		{
			float burdenStatGain = BurdenManager.GetBurdenStatGain(BurdenType.EnemyAdapt);
			float randomNumber = RNGManager.GetRandomNumber(RngID.SpecialProps_RoomSeed, "EnemyController.EnemyAdapt Burden", 0f, 1f);
			if (randomNumber <= burdenStatGain && randomNumber > 0f)
			{
				this.AddRandomCommanderStatusEffect();
			}
		}
		base.StartCoroutine(this.ApplyCommanderStatusEffects());
		this.ResetHealth();
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x0003F261 File Offset: 0x0003D461
	private IEnumerator ApplyCommanderStatusEffects()
	{
		while (!base.IsInitialized)
		{
			yield return null;
		}
		if (this.IsCommander && !base.StatusBarController.HasActiveStatusBarEntry(StatusBarEntryType.Commander))
		{
			base.StatusBarController.ApplyUIEffect(StatusBarEntryType.Commander);
		}
		if (this.m_commanderStatusEffectFlags != 0)
		{
			for (int i = 0; i < StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.Length; i++)
			{
				if ((this.m_commanderStatusEffectFlags & 1 << i) != 0)
				{
					StatusEffectType statusEffectType = StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY[i];
					this.m_statusEffectController.StopStatusEffect(statusEffectType, false);
					this.m_statusEffectController.StartStatusEffect(statusEffectType, 0f, null);
				}
			}
		}
		if (this.EnemyType == EnemyType.Zombie || this.IsSummoned || (this.Room.SpecialRoomType == SpecialRoomType.Fairy && !this.Room.GetComponent<FairyRoomController>().IsHiddenFairyRoom))
		{
			this.m_statusEffectController.SetAllStatusEffectsHidden(true);
			this.DisableOffscreenWarnings = true;
		}
		yield break;
	}

	// Token: 0x0600151E RID: 5406 RVA: 0x0003F270 File Offset: 0x0003D470
	protected override void FixedUpdate()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (this.ConstrainToRoom)
		{
			this.ConstrainEnemyMovementToRoom();
		}
		base.FixedUpdate();
		if (!base.IsDead)
		{
			this.UpdateFacing();
			if (this.IsFlying)
			{
				if (this.LogicController && this.LogicController.LogicIsActivated)
				{
					this.UpdateFlightMovement();
					this.UpdateFlightAcceleration();
				}
			}
			else
			{
				if (!base.IsGrounded && this.JumpHorizontalVelocity != 0f && Mathf.Approximately(base.Velocity.x, 0f))
				{
					this.SetVelocityX(this.JumpHorizontalVelocity, false);
				}
				if (base.IsGrounded && !base.KnockedIntoAir && this.GroundHorizontalVelocity != 0f && base.Velocity.x != this.GroundHorizontalVelocity && ((this.GroundHorizontalVelocity > 0f && !this.RightSidePlatformDropPrevented) || (this.GroundHorizontalVelocity < 0f && !this.LeftSidePlatformDropPrevented)))
				{
					this.SetVelocityX(this.GroundHorizontalVelocity, false);
				}
			}
			if (this.LogicController && !this.LogicController.LogicIsActivated && !base.KnockedIntoAir && !base.ControllerCorgi.RetainVelocity)
			{
				if (base.Velocity.x != 0f)
				{
					this.SetVelocityX(0f, false);
				}
				if (this.IsFlying && base.Velocity.y != 0f)
				{
					this.SetVelocityY(0f, false);
				}
			}
		}
		if (this.LogicController && this.LogicController.enabled)
		{
			this.LogicController.InternalUpdate();
		}
	}

	// Token: 0x0600151F RID: 5407 RVA: 0x0003F41C File Offset: 0x0003D61C
	protected void UpdateFacing()
	{
		if (this.AlwaysFacing && this.Target && !base.LockFlip && ((base.IsFacingRight && base.transform.localPosition.x > this.Target.transform.localPosition.x) || (!base.IsFacingRight && base.transform.localPosition.x < this.Target.transform.localPosition.x)))
		{
			if (this.PivotFollowsOrientation)
			{
				base.CharacterCorgi.Flip(false, true);
				return;
			}
			base.CharacterCorgi.Flip(false, false);
		}
	}

	// Token: 0x06001520 RID: 5408 RVA: 0x0003F4D0 File Offset: 0x0003D6D0
	protected void UpdateJumpAcceleration()
	{
		if (!base.IsGrounded && this.m_previousAirVelocityX != 0f)
		{
			this.SetVelocityX(this.m_previousAirVelocityX, false);
		}
		this.m_previousAirVelocityX = base.Velocity.x;
		if (base.IsGrounded)
		{
			this.m_previousAirVelocityX = 0f;
		}
	}

	// Token: 0x06001521 RID: 5409 RVA: 0x0003F524 File Offset: 0x0003D724
	protected void UpdateFlightMovement()
	{
		if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Freeze))
		{
			return;
		}
		if (this.Target)
		{
			Vector3 vector = this.Target.transform.position;
			if (this.m_targetController)
			{
				vector = this.m_targetController.Midpoint;
			}
			Vector3 followOffset = this.FollowOffset;
			vector += followOffset;
			if (this.AlwaysFacing)
			{
				if (this.FollowTarget)
				{
					base.Orientation = CDGHelper.TurnToFaceRadians(base.Midpoint, vector, CDGHelper.ToRadians(this.ActualTurnSpeed), base.Orientation, Time.fixedDeltaTime, false);
				}
				float num = CDGHelper.DistanceBetweenPts(vector, base.Midpoint);
				if (this.FollowTarget && !this.DisableDistanceThresholdCheck && num <= 0.1f && !base.LockFlip)
				{
					this.SetVelocity(0f, 0f, false);
				}
				else if (this.FlightKnockbackAcceleration != Vector2.zero && this.FlyingMovementType == FlyingMovementType.Override)
				{
					this.SetVelocity(0f, 0f, false);
				}
				else if (this.FlyingMovementType == FlyingMovementType.Towards)
				{
					this.SetVelocity(base.HeadingX * this.ActualSpeed, base.HeadingY * this.ActualSpeed, false);
				}
				else if (this.FlyingMovementType == FlyingMovementType.Away)
				{
					this.SetVelocity(base.HeadingX * -this.ActualSpeed, base.HeadingY * -this.ActualSpeed, false);
				}
				else if (this.FlyingMovementType == FlyingMovementType.Stop)
				{
					this.SetVelocity(0f, 0f, false);
				}
			}
			else
			{
				if (this.FollowTarget)
				{
					if (this.FlyingMovementType == FlyingMovementType.Away)
					{
						vector = base.Midpoint - vector;
						vector *= 10f;
					}
					base.Orientation = CDGHelper.TurnToFaceRadians(base.Midpoint, vector, CDGHelper.ToRadians(this.ActualTurnSpeed), base.Orientation, Time.fixedDeltaTime, false);
				}
				if (this.FlyingMovementType == FlyingMovementType.Away || this.FlyingMovementType == FlyingMovementType.Towards)
				{
					this.SetVelocity(base.HeadingX * this.ActualSpeed, base.HeadingY * this.ActualSpeed, false);
				}
				else if (this.FlyingMovementType == FlyingMovementType.Stop)
				{
					this.SetVelocity(0f, 0f, false);
				}
			}
			if (base.Pivot && this.PivotFollowsOrientation)
			{
				if (this.AlwaysFacing)
				{
					float num2 = CDGHelper.ToDegrees(base.Orientation);
					num2 = CDGHelper.GetLockedAngle(num2, 40);
					if (base.IsFacingRight)
					{
						if (num2 > 270f)
						{
							num2 -= 360f;
						}
					}
					else
					{
						num2 -= 180f;
					}
					Vector3 localEulerAngles = base.Pivot.transform.localEulerAngles;
					localEulerAngles.z = num2;
					base.Pivot.transform.localEulerAngles = localEulerAngles;
					return;
				}
				float num3 = CDGHelper.ToDegrees(base.Orientation);
				num3 = CDGHelper.GetLockedAngle(num3, 40);
				Vector3 localEulerAngles2 = base.Pivot.transform.localEulerAngles;
				localEulerAngles2.z = num3;
				base.Pivot.transform.localEulerAngles = localEulerAngles2;
			}
		}
	}

	// Token: 0x06001522 RID: 5410 RVA: 0x0003F848 File Offset: 0x0003DA48
	protected void UpdateFlightAcceleration()
	{
		float num = 15f * Time.fixedDeltaTime * this.FlightKnockbackDecelerationMod;
		if (base.Velocity == Vector2.zero)
		{
			num *= 1.5f;
		}
		if (this.FlightKnockbackAcceleration.x > 0f)
		{
			this.FlightKnockbackAcceleration.x = this.FlightKnockbackAcceleration.x - num;
		}
		if (this.FlightKnockbackAcceleration.x < 0f)
		{
			this.FlightKnockbackAcceleration.x = this.FlightKnockbackAcceleration.x + num;
		}
		if (Mathf.Abs(this.FlightKnockbackAcceleration.x) < num)
		{
			this.FlightKnockbackAcceleration.x = 0f;
		}
		if (this.FlightKnockbackAcceleration.y > 0f)
		{
			this.FlightKnockbackAcceleration.y = this.FlightKnockbackAcceleration.y - num;
		}
		if (this.FlightKnockbackAcceleration.y < 0f)
		{
			this.FlightKnockbackAcceleration.y = this.FlightKnockbackAcceleration.y + num;
		}
		if (Mathf.Abs(this.FlightKnockbackAcceleration.y) < num)
		{
			this.FlightKnockbackAcceleration.y = 0f;
		}
		this.SetVelocity(this.FlightKnockbackAcceleration.x, this.FlightKnockbackAcceleration.y, true);
	}

	// Token: 0x06001523 RID: 5411 RVA: 0x0003F970 File Offset: 0x0003DB70
	public void OrientToTarget()
	{
		float x = this.Target.transform.position.x - base.Midpoint.x;
		float y = this.Target.transform.position.y - base.Midpoint.y;
		if (this.m_targetController != null)
		{
			x = this.m_targetController.Midpoint.x - base.Midpoint.x;
			y = this.m_targetController.Midpoint.y - base.Midpoint.y;
		}
		base.Heading = new Vector2(x, y);
	}

	// Token: 0x06001524 RID: 5412 RVA: 0x0003FA18 File Offset: 0x0003DC18
	public void GenerateRandomFollowOffset(Vector2 randomXOffsets, Vector2 randomYOffsets)
	{
		float x = UnityEngine.Random.Range(randomXOffsets.x, randomXOffsets.y);
		float y = UnityEngine.Random.Range(randomYOffsets.x, randomYOffsets.y);
		this.m_followOffset = new Vector3(x, y, 0f);
		if (this.Target != null)
		{
			Vector3 vector = this.Target.transform.position;
			if (this.m_targetController)
			{
				vector = this.m_targetController.Midpoint;
			}
			if (vector.x > base.Midpoint.x)
			{
				this.m_followOffset.x = -this.m_followOffset.x;
			}
		}
	}

	// Token: 0x06001525 RID: 5413 RVA: 0x0003FAC0 File Offset: 0x0003DCC0
	public void ConstrainEnemyMovementToRoom()
	{
		if (this.ConstrainToRoom)
		{
			Rect collisionBounds = base.CollisionBounds;
			if (collisionBounds.width == 0f || collisionBounds.height == 0f)
			{
				Debug.Log("<color=yellow>Cannot constrain enemy to room.  Bounds width or height is 0.</color>");
				return;
			}
			this.m_touchingLeftRoomEdge = false;
			this.m_touchingRightRoomEdge = false;
			this.m_touchingBottomRoomEdge = false;
			this.m_touchingTopRoomEdge = false;
			BaseRoom room = this.Room;
			if (room)
			{
				Rect boundsRect = room.BoundsRect;
				float num = 0f;
				float num2 = 0f;
				if (collisionBounds.xMin < boundsRect.xMin)
				{
					num = boundsRect.xMin - collisionBounds.xMin;
					this.m_touchingLeftRoomEdge = true;
				}
				else if (collisionBounds.xMax > boundsRect.xMax)
				{
					num = boundsRect.xMax - collisionBounds.xMax;
					this.m_touchingRightRoomEdge = true;
				}
				if (collisionBounds.yMin < boundsRect.yMin)
				{
					num2 = boundsRect.yMin - collisionBounds.yMin;
					this.m_touchingBottomRoomEdge = true;
				}
				else if (collisionBounds.yMax > boundsRect.yMax)
				{
					num2 = boundsRect.yMax - collisionBounds.yMax;
					this.m_touchingTopRoomEdge = true;
				}
				if (num != 0f || num2 != 0f)
				{
					base.gameObject.transform.position += new Vector3(num, num2);
					this.UpdateBounds();
				}
			}
		}
	}

	// Token: 0x06001526 RID: 5414 RVA: 0x0003FC24 File Offset: 0x0003DE24
	public override float CalculateDamageTaken(IDamageObj damageObj, out CriticalStrikeType critType, out float damageBlocked, float damageOverride = -1f, bool trueDamage = false, bool pureCalculation = true)
	{
		critType = CriticalStrikeType.None;
		if (PlayerManager.GetPlayerController().CurrentHealth <= 0f)
		{
			damageBlocked = 0f;
			return 0f;
		}
		damageBlocked = 0f;
		float num = 1f;
		if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Invuln))
		{
			return 0f;
		}
		if (base.TakesNoDamage)
		{
			return 0f;
		}
		if (base.StatusEffectController.HasInvulnStack)
		{
			return 0f;
		}
		bool flag = damageObj.gameObject.CompareTag("Hazard");
		if (flag && !this.IsBoss)
		{
			return (float)Mathf.RoundToInt((float)this.ActualMaxHealth * 0.2f);
		}
		if (!TraitManager.IsTraitActive(TraitType.CantAttack))
		{
			Projectile_RL projectile_RL = damageObj as Projectile_RL;
			float num2 = damageObj.ActualDamage;
			if (damageOverride != -1f)
			{
				num2 = damageOverride;
				if (trueDamage)
				{
					return num2;
				}
			}
			else
			{
				float num3 = damageObj.ActualCritChance;
				if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Freeze) && num3 < 100f)
				{
					num3 += 100f;
				}
				if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Vulnerable) && num3 < 100f)
				{
					num3 += 100f;
				}
				BaseStatusEffect statusEffect = base.StatusEffectController.GetStatusEffect(StatusEffectType.Enemy_Combo);
				if (statusEffect && statusEffect.IsPlaying && statusEffect.TimesStacked >= 15 && num3 < 100f)
				{
					num3 += 100f;
				}
				float num4 = UnityEngine.Random.Range(0f, 1f);
				if (num3 > 0f && num3 >= num4)
				{
					num2 += damageObj.ActualCritDamage;
					if (num3 >= 100f)
					{
						critType = CriticalStrikeType.Guaranteed;
					}
					else
					{
						critType = CriticalStrikeType.Regular;
					}
				}
				if (critType == CriticalStrikeType.Guaranteed && num3 >= 100f)
				{
					float num5 = num3 - 100f;
					num5 *= 0.5f;
					RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.SuperCritChanceUp);
					num5 += (float)relic.Level * 0.2f;
					if (PlayerManager.GetPlayerController().CharacterClass.ClassType == ClassType.DualBladesClass)
					{
						num5 += 0.1f;
					}
					num5 += RuneLogicHelper.GetSuperCritChanceAdd();
					float num6 = UnityEngine.Random.Range(0f, 1f);
					if (num5 > 0f && num5 >= num6)
					{
						num2 += num2 * (0.35f + RuneLogicHelper.GetSuperCritDamageAdd());
						critType = CriticalStrikeType.Super;
					}
				}
				if (projectile_RL)
				{
					num += projectile_RL.DamageMod;
				}
				if ((!projectile_RL || !(projectile_RL is DownstrikeProjectile_RL)) && TraitManager.IsTraitActive(TraitType.SkillCritsOnly) && critType != CriticalStrikeType.Guaranteed && critType != CriticalStrikeType.Super)
				{
					num2 = 0f;
				}
			}
			if (critType != CriticalStrikeType.None && projectile_RL)
			{
				if (projectile_RL.CastAbilityType == CastAbilityType.Weapon)
				{
					RuneObj rune = RuneManager.GetRune(RuneType.WeaponCritDot);
					if (rune.EquippedLevel > 0)
					{
						float currentStatModTotal_ = rune.CurrentStatModTotal_1;
						base.StatusEffectController.StartStatusEffect(StatusEffectType.Enemy_MagicBreak, currentStatModTotal_, projectile_RL);
					}
					if (critType != CriticalStrikeType.Regular)
					{
						RelicObj relic2 = SaveManager.PlayerSaveData.GetRelic(RelicType.SkillCritBonus);
						if (relic2.Level > 0)
						{
							float duration = 2.5f + 2.5f * (float)(relic2.Level - 1);
							base.StatusEffectController.StartStatusEffect(StatusEffectType.Enemy_MagicBreak, duration, projectile_RL);
						}
					}
				}
				else
				{
					RuneObj rune2 = RuneManager.GetRune(RuneType.MagicCritDot);
					if (rune2.EquippedLevel > 0)
					{
						float currentStatModTotal_2 = rune2.CurrentStatModTotal_1;
						base.StatusEffectController.StartStatusEffect(StatusEffectType.Enemy_ArmorBreak, currentStatModTotal_2, projectile_RL);
					}
				}
			}
			if (!flag)
			{
				if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_ArmorBreak) && projectile_RL && projectile_RL.CastAbilityType == CastAbilityType.Weapon)
				{
					num += 0.20000005f;
				}
				if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_MagicBreak) && (!projectile_RL || (projectile_RL && projectile_RL.CastAbilityType != CastAbilityType.Weapon)))
				{
					num += 0.20000005f;
				}
				if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Combo))
				{
					num += 0.02f * (float)base.StatusEffectController.GetStatusEffect(StatusEffectType.Enemy_Combo).TimesStacked;
				}
				RelicObj relic3 = SaveManager.PlayerSaveData.GetRelic(RelicType.BonusDamageOnNextHit);
				if (relic3.Level > 0 && relic3.IntValue >= 6)
				{
					relic3.SetIntValue(0, false, true);
					float num7 = 0.75f * (float)relic3.Level + 1f;
					BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_ENEMY_DAMAGE_HELPER.Add(RelicType.BonusDamageOnNextHit.ToString());
					num += num7 - 1f;
				}
				if (!this.IsBoss && this.EnemyRank != EnemyRank.Miniboss && !damageObj.gameObject.CompareTag("Player"))
				{
					RelicObj relic4 = SaveManager.PlayerSaveData.GetRelic(RelicType.FreeEnemyKill);
					if (relic4.Level > 0)
					{
						int num8 = 6;
						num8 -= relic4.Level - 1;
						if (relic4.IntValue >= num8)
						{
							num2 = this.CurrentHealth;
							BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_ENEMY_DAMAGE_HELPER.Add(RelicType.FreeEnemyKill.ToString());
							return num2;
						}
					}
				}
				RelicObj relic5 = SaveManager.PlayerSaveData.GetRelic(RelicType.DamageBuffStatusEffect);
				if (relic5.Level > 0 && base.StatusEffectController.HasAnyActiveStatusEffect(false) && !damageObj.gameObject.CompareTag("Player"))
				{
					float num9 = (float)relic5.Level * 0.2f;
					num += num9;
					BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_ENEMY_DAMAGE_HELPER.Add(RelicType.DamageBuffStatusEffect.ToString());
				}
				if (projectile_RL && projectile_RL.CastAbilityType == CastAbilityType.Weapon && !(projectile_RL is DownstrikeProjectile_RL))
				{
					bool flag2 = projectile_RL.StrengthScale > 0f || projectile_RL.MagicScale > 0f;
					RelicObj relic6 = SaveManager.PlayerSaveData.GetRelic(RelicType.NoAttackDamageBonus);
					if (relic6.Level > 0 && flag2 && relic6.IntValue >= Relic_EV.GetRelicMaxStack(RelicType.NoAttackDamageBonus, relic6.Level))
					{
						float num10 = 0.75f;
						num += num10;
						BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_ENEMY_DAMAGE_HELPER.Add(RelicType.NoAttackDamageBonus.ToString());
						relic6.SetIntValue(0, false, true);
						PlayerManager.GetPlayerController().StartNoAttackDamageBonusTimer();
					}
				}
				RelicObj relic7 = SaveManager.PlayerSaveData.GetRelic(RelicType.RangeDamageBonusCurse);
				if (SaveManager.PlayerSaveData.GetRelic(RelicType.RangeDamageBonusCurse).Level > 0)
				{
					if (CDGHelper.DistanceBetweenPts(base.Midpoint, PlayerManager.GetPlayerController().Midpoint) >= 8.5f)
					{
						BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_ENEMY_DAMAGE_HELPER.Add(RelicType.RangeDamageBonusCurse.ToString());
						num += 0.125f * (float)relic7.Level;
					}
					else
					{
						num += 0f;
					}
				}
				if (SaveManager.PlayerSaveData.GetRelic(RelicType.DamageNoHitChallenge).Level > 0)
				{
					BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_ENEMY_DAMAGE_HELPER.Add(RelicType.DamageNoHitChallenge.ToString());
				}
				if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Aggro))
				{
					num -= (float)(base.StatusEffectController.GetStatusEffect(StatusEffectType.Enemy_Aggro).TimesStacked - 1) * 0.15f;
				}
				float insightPlayerDamageMod = this.GetInsightPlayerDamageMod();
				num += insightPlayerDamageMod - 1f;
			}
			num = Mathf.Clamp(num, 0f, float.MaxValue);
			num2 *= num;
			num2 *= this.ModeshiftDamageMod;
			return Mathf.Floor(num2);
		}
		if (this.EnemyType == EnemyType.Target)
		{
			return 1f;
		}
		return 0f;
	}

	// Token: 0x06001527 RID: 5415 RVA: 0x0004034C File Offset: 0x0003E54C
	protected float GetInsightPlayerDamageMod()
	{
		EnemyType enemyType = this.EnemyType;
		if (enemyType <= EnemyType.MimicChestBoss)
		{
			if (enemyType <= EnemyType.DancingBoss)
			{
				if (enemyType != EnemyType.SpellswordBoss)
				{
					if (enemyType == EnemyType.DancingBoss)
					{
						if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.DancingBossCombatBonus) >= InsightState.ResolvedButNotViewed)
						{
							return 1.15f;
						}
					}
				}
				else if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.SpellSwordBossCombatBonus) >= InsightState.ResolvedButNotViewed)
				{
					return 1.15f;
				}
			}
			else if (enemyType - EnemyType.SkeletonBossA > 1)
			{
				if (enemyType - EnemyType.StudyBoss <= 1)
				{
					if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.StudyBossCombatBonus) >= InsightState.ResolvedButNotViewed)
					{
						return 1.15f;
					}
				}
			}
			else if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.SkeletonBossCombatBonus) >= InsightState.ResolvedButNotViewed)
			{
				return 1.15f;
			}
		}
		else if (enemyType <= EnemyType.CaveBoss)
		{
			if (enemyType - EnemyType.EyeballBoss_Left > 3)
			{
				if (enemyType == EnemyType.CaveBoss)
				{
					if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.CaveBossCombatBonus) >= InsightState.ResolvedButNotViewed)
					{
						return 1.15f;
					}
				}
			}
			else if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.TowerBossCombatBonus) >= InsightState.ResolvedButNotViewed)
			{
				return 1.15f;
			}
		}
		else
		{
			if (enemyType == EnemyType.TraitorBoss)
			{
				float num = 1f;
				if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.TraitorBoss_HPReduce_SpellSwordBoss) >= InsightState.ResolvedButNotViewed)
				{
					num += 0.05f;
				}
				if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.TraitorBoss_HPReduce_DancingBoss) >= InsightState.ResolvedButNotViewed)
				{
					num += 0.05f;
				}
				if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.TraitorBoss_HPReduce_StudyBoss) >= InsightState.ResolvedButNotViewed)
				{
					num += 0.05f;
				}
				return num;
			}
			if (enemyType == EnemyType.FinalBoss)
			{
				float num2 = 1f;
				if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.FinalBoss_HPReduce_SkeletonBoss) >= InsightState.ResolvedButNotViewed)
				{
					num2 += 0.05f;
				}
				if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.FinalBoss_HPReduce_TowerBoss) >= InsightState.ResolvedButNotViewed)
				{
					num2 += 0.05f;
				}
				if (ChallengeManager.IsInChallenge || SaveManager.PlayerSaveData.GetInsightState(InsightType.FinalBoss_HPReduce_CaveBoss) >= InsightState.ResolvedButNotViewed)
				{
					num2 += 0.05f;
				}
				return num2;
			}
		}
		return 1f;
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x00040590 File Offset: 0x0003E790
	protected float GetInsightBossDamageMod()
	{
		EnemyType enemyType = this.EnemyType;
		if (enemyType <= EnemyType.MimicChestBoss)
		{
			if (enemyType <= EnemyType.DancingBoss)
			{
				if (enemyType == EnemyType.SpellswordBoss)
				{
					SaveManager.PlayerSaveData.GetInsightState(InsightType.SpellSwordBossCombatBonus);
					return 1f;
				}
				if (enemyType == EnemyType.DancingBoss)
				{
					SaveManager.PlayerSaveData.GetInsightState(InsightType.DancingBossCombatBonus);
					return 1f;
				}
			}
			else
			{
				if (enemyType - EnemyType.SkeletonBossA <= 1)
				{
					SaveManager.PlayerSaveData.GetInsightState(InsightType.SkeletonBossCombatBonus);
					return 1f;
				}
				if (enemyType - EnemyType.StudyBoss <= 1)
				{
					SaveManager.PlayerSaveData.GetInsightState(InsightType.StudyBossCombatBonus);
					return 1f;
				}
			}
		}
		else if (enemyType <= EnemyType.CaveBoss)
		{
			if (enemyType - EnemyType.EyeballBoss_Left <= 3)
			{
				SaveManager.PlayerSaveData.GetInsightState(InsightType.TowerBossCombatBonus);
				return 1f;
			}
			if (enemyType == EnemyType.CaveBoss)
			{
				SaveManager.PlayerSaveData.GetInsightState(InsightType.CaveBossCombatBonus);
				return 1f;
			}
		}
		else
		{
			if (enemyType == EnemyType.TraitorBoss)
			{
				return 1f;
			}
			if (enemyType == EnemyType.FinalBoss)
			{
				return 1f;
			}
		}
		return 1f;
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x000406CC File Offset: 0x0003E8CC
	public override void KillCharacter(GameObject killer, bool broadcastEvent)
	{
		if (base.IsDead)
		{
			return;
		}
		if (this.DisableDeath)
		{
			this.SetHealth(1f, false, true);
			return;
		}
		if (this.EnemyType == EnemyType.Eggplant)
		{
			this.SetHealth((float)this.ActualMaxHealth, false, true);
			return;
		}
		if (this.EnemyType == EnemyType.Dummy)
		{
			this.SetHealth((float)this.ActualMaxHealth, false, true);
			if (this.EnemyRank == EnemyRank.Basic)
			{
				NPCController component = base.GetComponent<NPCController>();
				if (component)
				{
					DialogueManager.StartNewDialogue(component, NPCState.Idle);
					DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(component.NPCType), "LOC_ID_DUMMY_DIALOGUE_DEALING_MAX_DAMAGE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
					WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
				}
			}
			return;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_DeathDelay))
		{
			this.LogicController.StopAllLogic(true);
			base.IsDead = true;
			if (global::AnimatorUtility.HasParameter(base.Animator, "Stunned"))
			{
				base.Animator.SetBool("Stunned", true);
			}
			base.ConditionState = CharacterStates.CharacterConditions.Dead;
			return;
		}
		if (!ChallengeManager.IsInChallenge)
		{
			SaveManager.PlayerSaveData.EnemiesKilled++;
			SaveManager.ModeSaveData.SetEnemiesDefeated(GameModeType.Regular, this.EnemyType, this.EnemyRank, 1, true);
		}
		RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.ResolveCombatChallenge);
		if (relic.Level > 0 && !this.DisableHPMPBonuses)
		{
			if (this.IsBoss && this.EnemyRank != EnemyRank.Miniboss)
			{
				relic.SetIntValue(10, false, true);
			}
			else
			{
				relic.SetIntValue(1, true, true);
			}
			if (relic.IntValue >= 10)
			{
				relic.SetIntValue(0, false, true);
				relic.SetLevel(-1, true, true);
				SaveManager.PlayerSaveData.GetRelic(RelicType.ResolveCombatChallengeUsed).SetLevel(1, true, true);
				EnemyController.m_relicChangedEventArgs_STATIC.Initialize(RelicType.ResolveCombatChallengeUsed);
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RelicPurified, this, EnemyController.m_relicChangedEventArgs_STATIC);
				playerController.InitializeHealthMods();
			}
		}
		RelicObj relic2 = SaveManager.PlayerSaveData.GetRelic(RelicType.GoldCombatChallenge);
		if (relic2.Level > 0 && !this.DisableHPMPBonuses)
		{
			if (this.IsBoss && this.EnemyRank != EnemyRank.Miniboss)
			{
				relic2.SetIntValue(15, false, true);
			}
			else
			{
				relic2.SetIntValue(1, true, true);
			}
			if (relic2.IntValue >= 15)
			{
				relic2.SetIntValue(0, false, true);
				relic2.SetLevel(-1, true, true);
				SaveManager.PlayerSaveData.GetRelic(RelicType.GoldCombatChallengeUsed).SetLevel(1, true, true);
				EnemyController.m_relicChangedEventArgs_STATIC.Initialize(RelicType.GoldCombatChallengeUsed);
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RelicPurified, this, EnemyController.m_relicChangedEventArgs_STATIC);
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, null, null);
			}
		}
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.SporeburstKillAdd).Level > 0)
		{
			ProjectileManager.FireProjectile(PlayerManager.GetPlayerController().gameObject, "SporeBurstProjectile", base.Midpoint, false, 0f, 1f, true, true, true, true).CastAbilityType = CastAbilityType.Spell;
		}
		base.KillCharacter(killer, broadcastEvent);
		this.m_onPreDisableRelay.Dispatch(this);
		this.SetVelocity(0f, 0f, false);
		if (!killer.CompareTag("Player"))
		{
			killer.CompareTag("PlayerProjectile");
		}
		if (!this.IsBoss && TraitManager.IsTraitActive(TraitType.ExplosiveEnemies) && this.EnemyType != EnemyType.Target)
		{
			float angleInDeg = UnityEngine.Random.Range(Trait_EV.EXPLOSIVE_ENEMIES_PROJECTILE_ANGLE.x, Trait_EV.EXPLOSIVE_ENEMIES_PROJECTILE_ANGLE.y);
			ProjectileManager.FireProjectile(base.gameObject, "ExplosiveEnemiesPotionProjectile", base.Midpoint, false, angleInDeg, 1f, true, true, true, true);
		}
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.DeathCoroutine(killer, broadcastEvent));
		}
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x00040A52 File Offset: 0x0003EC52
	private IEnumerator DeathCoroutine(GameObject killer, bool broadcastEvent)
	{
		this.LogicController.TriggerDeath();
		if (this.IsBoss || this.LogicController.LogicScript.ForceDeathAnimation)
		{
			yield return this.LogicController.LogicScript.DeathAnim();
		}
		if (broadcastEvent)
		{
			EnemyController.m_enemyDeathEventArgs_STATIC.Initialize(this, killer);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EnemyDeath, this, EnemyController.m_enemyDeathEventArgs_STATIC);
			this.m_onEnemyDeathRelay.Dispatch(this, EnemyController.m_enemyDeathEventArgs_STATIC);
			if (this.EnemyType != EnemyType.TraitorBoss && this.EnemyType != EnemyType.FinalBoss)
			{
				this.m_onDeathEffectTriggerRelay.Dispatch(killer.gameObject);
			}
			this.DropReward();
		}
		if (this.EnemyType != EnemyType.TraitorBoss && this.EnemyType != EnemyType.FinalBoss)
		{
			base.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x0600152B RID: 5419 RVA: 0x00040A70 File Offset: 0x0003EC70
	private void DropReward()
	{
		if (this.IsBoss)
		{
			return;
		}
		if (ChallengeManager.IsInChallenge || !this.Room.AllowItemDrops)
		{
			return;
		}
		if (this.IsSummoned)
		{
			return;
		}
		if (this.Room.SpecialRoomType == SpecialRoomType.Fairy)
		{
			return;
		}
		Vector3 midpoint = base.Midpoint;
		midpoint.y += 1f;
		bool largeSpurt = TraitManager.IsTraitActive(TraitType.ItemsGoFlying);
		if (this.IsCommander)
		{
			int amount = Economy_EV.EXPERT_ENEMY_BASE_ORE_DROP + Mathf.FloorToInt((float)this.Level * Economy_EV.EXPERT_ENEMY_ORE_SCALING_PER_ENEMY_LEVEL);
			ItemDropManager.DropItem(ItemDropType.EquipmentOre, amount, midpoint, largeSpurt, this.IsBoss, false);
		}
		float actualDropOdds = this.ActualDropOdds;
		if (UnityEngine.Random.Range(0f, 1f) < actualDropOdds)
		{
			float num = (float)UnityEngine.Random.Range(Economy_EV.ENEMY_BASE_GOLD_DROP_AMOUNT.x, Economy_EV.ENEMY_BASE_GOLD_DROP_AMOUNT.x + 1);
			float num2 = UnityEngine.Random.Range(Economy_EV.ENEMY_GOLD_DROP_PER_LEVEL_ADD.x, Economy_EV.ENEMY_GOLD_DROP_PER_LEVEL_ADD.y);
			num2 *= (float)(this.Level - 1);
			int num3 = (int)this.EnemyRank;
			if (this.IsBoss)
			{
				num3 = 4;
			}
			float num4 = Economy_EV.ENEMY_TYPE_GOLD_MOD[num3];
			int newGamePlusLevel = SaveManager.PlayerSaveData.NewGamePlusLevel;
			ItemDropManager.DropGold((int)((num + num2 - (float)newGamePlusLevel) * num4) * Economy_EV.GetItemDropValue(ItemDropType.Coin, false), midpoint, largeSpurt, this.IsBoss, false);
		}
		float num5 = 0f;
		if (UnityEngine.Random.Range(0f, 1f) < num5)
		{
			if (CDGHelper.RandomPlusMinus() > 0)
			{
				ItemDropManager.DropItem(ItemDropType.HealthDrop, 1, midpoint, largeSpurt, this.IsBoss, false);
			}
			else
			{
				ItemDropManager.DropItem(ItemDropType.ManaDrop, 1, midpoint, largeSpurt, this.IsBoss, false);
			}
		}
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.EnemiesDropMeat).Level;
		float num6 = 0.08f * (float)level;
		if (num6 > 0f && UnityEngine.Random.Range(0f, 1f) < num6)
		{
			ItemDropManager.DropItem(ItemDropType.HealthDrop, 1, midpoint, largeSpurt, this.IsBoss, false);
		}
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x00040C48 File Offset: 0x0003EE48
	public void InjectLevel()
	{
		foreach (ILevelConsumer levelConsumer in base.GetComponentsInChildren<ILevelConsumer>())
		{
			if (levelConsumer != this)
			{
				levelConsumer.SetLevel(this.Level);
			}
		}
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x00040C7E File Offset: 0x0003EE7E
	public override void ResetStates()
	{
		base.ResetStates();
		this.LogicController.ResetLogic();
		this.FollowOffset = Vector2.zero;
		this.JumpHorizontalVelocity = 0f;
		this.LeftSidePlatformDropPrevented = false;
		this.RightSidePlatformDropPrevented = false;
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x00040CBA File Offset: 0x0003EEBA
	public void ResetPivotRotation()
	{
		if (base.Pivot)
		{
			base.Pivot.transform.eulerAngles = Vector3.zero;
		}
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x00040CDE File Offset: 0x0003EEDE
	public void ResetTurnTrigger()
	{
		if (base.Animator && global::AnimatorUtility.HasParameter(base.Animator, EnemyController.TURN_ANIMATION_TRIGGER_NAME))
		{
			base.Animator.ResetTrigger(EnemyController.TURN_ANIMATION_TRIGGER_NAME);
		}
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x00040D0F File Offset: 0x0003EF0F
	public override void ResetMods()
	{
		base.ResetMods();
		this.SpeedMod = 0f;
		this.SpeedAdd = 0f;
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x00040D30 File Offset: 0x0003EF30
	public void ResetPositionForSpawnController(Vector3 spawnPoint, Collider2D spawnCollider)
	{
		base.transform.localPosition = spawnPoint;
		if (base.IsInitialized)
		{
			this.UpdateBounds();
			this.ResetCollisionState();
		}
		if (spawnCollider)
		{
			this.ForceStandingOn(spawnCollider);
		}
		this.ForceFaceTarget();
		this.m_onResetPositionRelay.Dispatch(null, null);
	}

	// Token: 0x06001532 RID: 5426 RVA: 0x00040D7F File Offset: 0x0003EF7F
	public void ActivateEnemy()
	{
		this.LogicController.ResetLogic();
		EnemyController.m_enemyActivationStateChangedEventArgs_STATIC.Initialize(this);
		this.m_onActivatedRelay.Dispatch(this, EnemyController.m_enemyActivationStateChangedEventArgs_STATIC);
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x00040DA8 File Offset: 0x0003EFA8
	public void DeactivateEnemy()
	{
		this.ResetStates();
		if (!this.PreserveRotationWhenDeactivated)
		{
			this.ResetPivotRotation();
		}
		this.ResetTurnTrigger();
		EnemyController.m_enemyActivationStateChangedEventArgs_STATIC.Initialize(this);
		this.m_onDeactivatedRelay.Dispatch(this, EnemyController.m_enemyActivationStateChangedEventArgs_STATIC);
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x00040DE0 File Offset: 0x0003EFE0
	public void EnemyTimedOut()
	{
		EnemyController.m_enemyActivationStateChangedEventArgs_STATIC.Initialize(this);
		this.m_onReactivationTimedOutRelay.Dispatch(this, EnemyController.m_enemyActivationStateChangedEventArgs_STATIC);
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x00040DFE File Offset: 0x0003EFFE
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x00040E07 File Offset: 0x0003F007
	public void SetEnemyIndex(int index)
	{
		this.EnemyIndex = index;
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x00040E10 File Offset: 0x0003F010
	public void ResetValues()
	{
		this.ResetCharacter();
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x00040E18 File Offset: 0x0003F018
	public override void ResetHealth()
	{
		this.SetHealth((float)this.ActualMaxHealth, false, false);
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x00040E2C File Offset: 0x0003F02C
	public override void ResetCharacter()
	{
		base.ResetCharacter();
		base.ResetRendererArrayColor();
		base.BlinkPulseEffect.ResetAllBlackFills();
		this.InitializeLevelMods();
		this.ResetHealth();
		this.IsBeingSummoned = false;
		this.KilledByKnockout = false;
		if (base.IsInitialized)
		{
			this.DisableDeath = false;
		}
		this.DisableOffscreenWarnings = false;
		base.StatusBarController.StopUIEffect(StatusBarEntryType.Commander);
		this.LastDamageTakenType = DamageType.Strength;
		this.LastDamageHitCount = 0;
		if (!this.IsFlying && base.ControllerCorgi)
		{
			base.ControllerCorgi.GravityActive(true);
		}
		this.GroundHorizontalVelocity = 0f;
		this.JumpHorizontalVelocity = 0f;
		if (TraitManager.IsTraitActive(TraitType.EnemiesBlackFill))
		{
			base.BlinkPulseEffect.ActivateBlackFill(BlackFillType.EnemiesBlackFill_Trait, 0f);
		}
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x00040FE4 File Offset: 0x0003F1E4
	GameObject ISummoner.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x00040FEC File Offset: 0x0003F1EC
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x00040FF4 File Offset: 0x0003F1F4
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x00040FFC File Offset: 0x0003F1FC
	GameObject IOffscreenObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400142C RID: 5164
	private const float CHASE_DISTANCE_THRESHOLD = 0.1f;

	// Token: 0x0400142D RID: 5165
	private static int LAND_ANIMATION_STATE_NAME = Animator.StringToHash("Land");

	// Token: 0x0400142E RID: 5166
	private static int TURN_ANIMATION_TRIGGER_NAME = Animator.StringToHash("Turn");

	// Token: 0x0400142F RID: 5167
	private static RelicChangedEventArgs m_relicChangedEventArgs_STATIC = new RelicChangedEventArgs(RelicType.None);

	// Token: 0x04001430 RID: 5168
	[Header("Enemy Values")]
	[SerializeField]
	[HideInInspector]
	private EnemyType m_enemyType = EnemyType.Skeleton;

	// Token: 0x04001431 RID: 5169
	[SerializeField]
	[HideInInspector]
	private EnemyRank m_enemyRank;

	// Token: 0x04001432 RID: 5170
	[Space(10f)]
	[SerializeField]
	private float m_flightKnockbackDecelerationMod = 1f;

	// Token: 0x04001433 RID: 5171
	[SerializeField]
	private bool m_constrainToRoom = true;

	// Token: 0x04001434 RID: 5172
	[SerializeField]
	private bool m_knocksBackOnHit;

	// Token: 0x04001435 RID: 5173
	[SerializeField]
	private StrikeType m_strikeType = StrikeType.Blunt;

	// Token: 0x04001436 RID: 5174
	[SerializeField]
	private bool m_isBoss;

	// Token: 0x04001437 RID: 5175
	[SerializeField]
	private bool m_forceActivate;

	// Token: 0x04001438 RID: 5176
	[SerializeField]
	private bool m_disableXPBonuses;

	// Token: 0x04001439 RID: 5177
	[SerializeField]
	private bool m_disableHPMPBonuses;

	// Token: 0x0400143A RID: 5178
	[SerializeField]
	private bool m_alwaysDealsContactDamage;

	// Token: 0x0400143B RID: 5179
	[SerializeField]
	private GameObject[] m_culledObjectsArray;

	// Token: 0x0400143C RID: 5180
	private static Dictionary<EnemyTypeAndRank, EnemyData> EnemyData_StaticInstanceDict = new Dictionary<EnemyTypeAndRank, EnemyData>();

	// Token: 0x0400143D RID: 5181
	private float m_baseSpeed;

	// Token: 0x0400143E RID: 5182
	private float m_baseTurnSpeed;

	// Token: 0x0400143F RID: 5183
	private Vector2 m_baseRestCooldown;

	// Token: 0x04001440 RID: 5184
	private bool m_isFlying;

	// Token: 0x04001441 RID: 5185
	private bool m_alwaysFace;

	// Token: 0x04001442 RID: 5186
	private bool m_collidesWithPlatforms;

	// Token: 0x04001443 RID: 5187
	private float m_baseKnockbackStrength;

	// Token: 0x04001444 RID: 5188
	private float m_baseStunStrength;

	// Token: 0x04001445 RID: 5189
	private float m_baseDropOdds;

	// Token: 0x04001446 RID: 5190
	private float m_closeRadius;

	// Token: 0x04001447 RID: 5191
	private float m_mediumRadius;

	// Token: 0x04001448 RID: 5192
	private float m_farRadius;

	// Token: 0x04001449 RID: 5193
	protected GameObject m_target;

	// Token: 0x0400144A RID: 5194
	protected BaseCharacterController m_targetController;

	// Token: 0x0400144B RID: 5195
	private Vector3 m_followOffset;

	// Token: 0x0400144C RID: 5196
	private float m_baseScaleToOffsetWith = 1f;

	// Token: 0x0400144D RID: 5197
	private LogicController m_logicController;

	// Token: 0x0400144E RID: 5198
	private bool m_pivotFollowsOrientation;

	// Token: 0x0400144F RID: 5199
	private EnemyData m_enemyData;

	// Token: 0x04001450 RID: 5200
	private int m_level = 1;

	// Token: 0x04001451 RID: 5201
	private float m_previousAirVelocityX;

	// Token: 0x04001452 RID: 5202
	private PreventPlatformDrop m_preventPlatformDropObj;

	// Token: 0x04001453 RID: 5203
	private bool m_fallLedge;

	// Token: 0x04001454 RID: 5204
	[NonSerialized]
	public Vector2 FlightKnockbackAcceleration;

	// Token: 0x04001455 RID: 5205
	[NonSerialized]
	public bool CanIncrementRelicHitCounter = true;

	// Token: 0x04001456 RID: 5206
	[NonSerialized]
	public bool AttackingWithContactDamage;

	// Token: 0x04001457 RID: 5207
	[NonSerialized]
	public bool DisableDeath;

	// Token: 0x04001458 RID: 5208
	private static EnemyDeathEventArgs m_enemyDeathEventArgs_STATIC;

	// Token: 0x04001459 RID: 5209
	private static EnemyActivationStateChangedEventArgs m_enemyActivationStateChangedEventArgs_STATIC;

	// Token: 0x0400145A RID: 5210
	private int m_enemyIndex = -1;

	// Token: 0x0400145B RID: 5211
	private bool m_enemyInitializing;

	// Token: 0x0400145C RID: 5212
	private bool m_touchingLeftRoomEdge;

	// Token: 0x0400145D RID: 5213
	private bool m_touchingRightRoomEdge;

	// Token: 0x0400145E RID: 5214
	private bool m_touchingTopRoomEdge;

	// Token: 0x0400145F RID: 5215
	private bool m_touchingBottomRoomEdge;

	// Token: 0x04001460 RID: 5216
	private int m_commanderStatusEffectFlags;

	// Token: 0x04001468 RID: 5224
	private DamageType m_lastDamageTypeTaken;

	// Token: 0x04001475 RID: 5237
	private Relay<object, EnemyActivationStateChangedEventArgs> m_onActivatedRelay = new Relay<object, EnemyActivationStateChangedEventArgs>();

	// Token: 0x04001476 RID: 5238
	private Relay<object, EnemyActivationStateChangedEventArgs> m_onDeactivatedRelay = new Relay<object, EnemyActivationStateChangedEventArgs>();

	// Token: 0x04001477 RID: 5239
	private Relay<object, EnemyActivationStateChangedEventArgs> m_onReactivationTimedOutRelay = new Relay<object, EnemyActivationStateChangedEventArgs>();

	// Token: 0x04001478 RID: 5240
	private Relay<object, EnemyDeathEventArgs> m_onEnemyDeathRelay = new Relay<object, EnemyDeathEventArgs>();

	// Token: 0x04001479 RID: 5241
	private Relay<object, EventArgs> m_onResetPositionRelay = new Relay<object, EventArgs>();

	// Token: 0x0400147A RID: 5242
	private Relay<object, EventArgs> m_onPositionedForSummoningRelay = new Relay<object, EventArgs>();

	// Token: 0x0400147B RID: 5243
	private Relay m_onDisableRelay = new Relay();
}
