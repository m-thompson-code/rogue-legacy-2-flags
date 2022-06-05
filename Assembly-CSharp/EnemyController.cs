using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using RL_Windows;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200038C RID: 908
public class EnemyController : BaseCharacterController, IRoomConsumer, ILevelConsumer, ISummoner, IHealth, IDamageObj, IGenericPoolObj, IOffscreenObj
{
	// Token: 0x17000D84 RID: 3460
	// (get) Token: 0x06001D8D RID: 7565 RVA: 0x0000F48A File Offset: 0x0000D68A
	// (set) Token: 0x06001D8E RID: 7566 RVA: 0x0000F492 File Offset: 0x0000D692
	public bool DisableOffscreenWarnings { get; set; } = true;

	// Token: 0x17000D85 RID: 3461
	// (get) Token: 0x06001D8F RID: 7567 RVA: 0x0000F49B File Offset: 0x0000D69B
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000D86 RID: 3462
	// (get) Token: 0x06001D90 RID: 7568 RVA: 0x0000F49E File Offset: 0x0000D69E
	// (set) Token: 0x06001D91 RID: 7569 RVA: 0x0000F4A6 File Offset: 0x0000D6A6
	public EnemyManager.EnemyPreSummonedState PreSummonedState { get; set; }

	// Token: 0x17000D87 RID: 3463
	// (get) Token: 0x06001D92 RID: 7570 RVA: 0x0000F4AF File Offset: 0x0000D6AF
	// (set) Token: 0x06001D93 RID: 7571 RVA: 0x0000F4B7 File Offset: 0x0000D6B7
	public bool IsCulled { get; set; }

	// Token: 0x17000D88 RID: 3464
	// (get) Token: 0x06001D94 RID: 7572 RVA: 0x0000F4C0 File Offset: 0x0000D6C0
	// (set) Token: 0x06001D95 RID: 7573 RVA: 0x0000F4C8 File Offset: 0x0000D6C8
	public bool KilledByKnockout { get; set; }

	// Token: 0x17000D89 RID: 3465
	// (get) Token: 0x06001D96 RID: 7574 RVA: 0x0000F4D1 File Offset: 0x0000D6D1
	// (set) Token: 0x06001D97 RID: 7575 RVA: 0x0000F4D9 File Offset: 0x0000D6D9
	public bool DisableDistanceThresholdCheck { get; set; }

	// Token: 0x17000D8A RID: 3466
	// (get) Token: 0x06001D98 RID: 7576 RVA: 0x0000F4E2 File Offset: 0x0000D6E2
	// (set) Token: 0x06001D99 RID: 7577 RVA: 0x0000F4EA File Offset: 0x0000D6EA
	public bool IsCommander { get; set; }

	// Token: 0x17000D8B RID: 3467
	// (get) Token: 0x06001D9A RID: 7578 RVA: 0x0000F4F3 File Offset: 0x0000D6F3
	// (set) Token: 0x06001D9B RID: 7579 RVA: 0x0000F4FB File Offset: 0x0000D6FB
	public int LastDamageHitCount { get; set; }

	// Token: 0x17000D8C RID: 3468
	// (get) Token: 0x06001D9C RID: 7580 RVA: 0x0000F504 File Offset: 0x0000D704
	// (set) Token: 0x06001D9D RID: 7581 RVA: 0x0000F50C File Offset: 0x0000D70C
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

	// Token: 0x17000D8D RID: 3469
	// (get) Token: 0x06001D9E RID: 7582 RVA: 0x0000F525 File Offset: 0x0000D725
	public PreventPlatformDrop PreventPlatformDropObj
	{
		get
		{
			return this.m_preventPlatformDropObj;
		}
	}

	// Token: 0x17000D8E RID: 3470
	// (get) Token: 0x06001D9F RID: 7583 RVA: 0x0000F52D File Offset: 0x0000D72D
	// (set) Token: 0x06001DA0 RID: 7584 RVA: 0x0009CA88 File Offset: 0x0009AC88
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

	// Token: 0x17000D8F RID: 3471
	// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x0000F53F File Offset: 0x0000D73F
	// (set) Token: 0x06001DA2 RID: 7586 RVA: 0x0000F547 File Offset: 0x0000D747
	public bool IsBeingSummoned { get; set; }

	// Token: 0x17000D90 RID: 3472
	// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x0000F550 File Offset: 0x0000D750
	// (set) Token: 0x06001DA4 RID: 7588 RVA: 0x0000F558 File Offset: 0x0000D758
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17000D91 RID: 3473
	// (get) Token: 0x06001DA5 RID: 7589 RVA: 0x0000F561 File Offset: 0x0000D761
	// (set) Token: 0x06001DA6 RID: 7590 RVA: 0x0000F569 File Offset: 0x0000D769
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17000D92 RID: 3474
	// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x0000F572 File Offset: 0x0000D772
	// (set) Token: 0x06001DA8 RID: 7592 RVA: 0x0000F57A File Offset: 0x0000D77A
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

	// Token: 0x17000D93 RID: 3475
	// (get) Token: 0x06001DA9 RID: 7593 RVA: 0x0000F583 File Offset: 0x0000D783
	// (set) Token: 0x06001DAA RID: 7594 RVA: 0x0000F58B File Offset: 0x0000D78B
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

	// Token: 0x17000D94 RID: 3476
	// (get) Token: 0x06001DAB RID: 7595 RVA: 0x0000F594 File Offset: 0x0000D794
	public bool IsVulnerableToLifeSteal
	{
		get
		{
			return !this.m_disableHPMPBonuses;
		}
	}

	// Token: 0x17000D95 RID: 3477
	// (get) Token: 0x06001DAC RID: 7596 RVA: 0x0000F59F File Offset: 0x0000D79F
	// (set) Token: 0x06001DAD RID: 7597 RVA: 0x0000F5A7 File Offset: 0x0000D7A7
	public bool ResetToNeutralWhenUnculling { get; set; } = true;

	// Token: 0x17000D96 RID: 3478
	// (get) Token: 0x06001DAE RID: 7598 RVA: 0x0000F5B0 File Offset: 0x0000D7B0
	// (set) Token: 0x06001DAF RID: 7599 RVA: 0x0000F5B8 File Offset: 0x0000D7B8
	public bool DisableCulling { get; set; }

	// Token: 0x17000D97 RID: 3479
	// (get) Token: 0x06001DB0 RID: 7600 RVA: 0x0000F5C1 File Offset: 0x0000D7C1
	public GameObject[] CulledObjectsArray
	{
		get
		{
			return this.m_culledObjectsArray;
		}
	}

	// Token: 0x17000D98 RID: 3480
	// (get) Token: 0x06001DB1 RID: 7601 RVA: 0x0000F5C9 File Offset: 0x0000D7C9
	// (set) Token: 0x06001DB2 RID: 7602 RVA: 0x0000F5D1 File Offset: 0x0000D7D1
	public bool LeftSidePlatformDropPrevented { get; set; }

	// Token: 0x17000D99 RID: 3481
	// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x0000F5DA File Offset: 0x0000D7DA
	// (set) Token: 0x06001DB4 RID: 7604 RVA: 0x0000F5E2 File Offset: 0x0000D7E2
	public bool RightSidePlatformDropPrevented { get; set; }

	// Token: 0x17000D9A RID: 3482
	// (get) Token: 0x06001DB5 RID: 7605 RVA: 0x0000F49B File Offset: 0x0000D69B
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000D9B RID: 3483
	// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x0000F49B File Offset: 0x0000D69B
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000D9C RID: 3484
	// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x0000F5EB File Offset: 0x0000D7EB
	// (set) Token: 0x06001DB8 RID: 7608 RVA: 0x0000F5F3 File Offset: 0x0000D7F3
	public float ModeshiftDamageMod { get; set; } = 1f;

	// Token: 0x17000D9D RID: 3485
	// (get) Token: 0x06001DB9 RID: 7609 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public bool IsDotDamage
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000D9E RID: 3486
	// (get) Token: 0x06001DBA RID: 7610 RVA: 0x0009CAD4 File Offset: 0x0009ACD4
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

	// Token: 0x17000D9F RID: 3487
	// (get) Token: 0x06001DBB RID: 7611 RVA: 0x0009CBA8 File Offset: 0x0009ADA8
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

	// Token: 0x17000DA0 RID: 3488
	// (get) Token: 0x06001DBC RID: 7612 RVA: 0x0000F5FC File Offset: 0x0000D7FC
	// (set) Token: 0x06001DBD RID: 7613 RVA: 0x0000F604 File Offset: 0x0000D804
	public EnemySpawnController EnemySpawnController { get; set; }

	// Token: 0x17000DA1 RID: 3489
	// (get) Token: 0x06001DBE RID: 7614 RVA: 0x0000F60D File Offset: 0x0000D80D
	// (set) Token: 0x06001DBF RID: 7615 RVA: 0x0000F615 File Offset: 0x0000D815
	public ISummoner Summoner { get; set; }

	// Token: 0x17000DA2 RID: 3490
	// (get) Token: 0x06001DC0 RID: 7616 RVA: 0x0000F61E File Offset: 0x0000D81E
	public bool IsSummoned
	{
		get
		{
			return this.Summoner != null;
		}
	}

	// Token: 0x17000DA3 RID: 3491
	// (get) Token: 0x06001DC1 RID: 7617 RVA: 0x0000F629 File Offset: 0x0000D829
	// (set) Token: 0x06001DC2 RID: 7618 RVA: 0x0000F631 File Offset: 0x0000D831
	public float GroundHorizontalVelocity { get; set; }

	// Token: 0x17000DA4 RID: 3492
	// (get) Token: 0x06001DC3 RID: 7619 RVA: 0x0000F63A File Offset: 0x0000D83A
	// (set) Token: 0x06001DC4 RID: 7620 RVA: 0x0000F642 File Offset: 0x0000D842
	public float JumpHorizontalVelocity { get; set; }

	// Token: 0x17000DA5 RID: 3493
	// (get) Token: 0x06001DC5 RID: 7621 RVA: 0x0000F64B File Offset: 0x0000D84B
	// (set) Token: 0x06001DC6 RID: 7622 RVA: 0x0000F653 File Offset: 0x0000D853
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

	// Token: 0x17000DA6 RID: 3494
	// (get) Token: 0x06001DC7 RID: 7623 RVA: 0x0000F65C File Offset: 0x0000D85C
	public override float BaseScaleToOffsetWith
	{
		get
		{
			return this.m_baseScaleToOffsetWith;
		}
	}

	// Token: 0x17000DA7 RID: 3495
	// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x0000F664 File Offset: 0x0000D864
	public IRelayLink<object, EnemyActivationStateChangedEventArgs> OnActivatedRelay
	{
		get
		{
			return this.m_onActivatedRelay.link;
		}
	}

	// Token: 0x17000DA8 RID: 3496
	// (get) Token: 0x06001DC9 RID: 7625 RVA: 0x0000F671 File Offset: 0x0000D871
	public IRelayLink<object, EnemyActivationStateChangedEventArgs> OnDeactivatedRelay
	{
		get
		{
			return this.m_onDeactivatedRelay.link;
		}
	}

	// Token: 0x17000DA9 RID: 3497
	// (get) Token: 0x06001DCA RID: 7626 RVA: 0x0000F67E File Offset: 0x0000D87E
	public IRelayLink<object, EnemyActivationStateChangedEventArgs> OnReactivationTimedOutRelay
	{
		get
		{
			return this.m_onReactivationTimedOutRelay.link;
		}
	}

	// Token: 0x17000DAA RID: 3498
	// (get) Token: 0x06001DCB RID: 7627 RVA: 0x0000F68B File Offset: 0x0000D88B
	public IRelayLink<object, EnemyDeathEventArgs> OnEnemyDeathRelay
	{
		get
		{
			return this.m_onEnemyDeathRelay.link;
		}
	}

	// Token: 0x17000DAB RID: 3499
	// (get) Token: 0x06001DCC RID: 7628 RVA: 0x0000F698 File Offset: 0x0000D898
	public IRelayLink<object, EventArgs> OnResetPositionRelay
	{
		get
		{
			return this.m_onResetPositionRelay.link;
		}
	}

	// Token: 0x17000DAC RID: 3500
	// (get) Token: 0x06001DCD RID: 7629 RVA: 0x0000F6A5 File Offset: 0x0000D8A5
	public IRelayLink OnDisableRelay
	{
		get
		{
			return this.m_onDisableRelay.link;
		}
	}

	// Token: 0x17000DAD RID: 3501
	// (get) Token: 0x06001DCE RID: 7630 RVA: 0x0000F6B2 File Offset: 0x0000D8B2
	public Relay<object, EventArgs> OnPositionedForSummoningRelay
	{
		get
		{
			return this.m_onPositionedForSummoningRelay;
		}
	}

	// Token: 0x17000DAE RID: 3502
	// (get) Token: 0x06001DCF RID: 7631 RVA: 0x0000F6BA File Offset: 0x0000D8BA
	// (set) Token: 0x06001DD0 RID: 7632 RVA: 0x0000F6CC File Offset: 0x0000D8CC
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

	// Token: 0x17000DAF RID: 3503
	// (get) Token: 0x06001DD1 RID: 7633 RVA: 0x0000F6D5 File Offset: 0x0000D8D5
	// (set) Token: 0x06001DD2 RID: 7634 RVA: 0x0000F6DD File Offset: 0x0000D8DD
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

	// Token: 0x17000DB0 RID: 3504
	// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x0000F6E6 File Offset: 0x0000D8E6
	// (set) Token: 0x06001DD4 RID: 7636 RVA: 0x0000F6EE File Offset: 0x0000D8EE
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

	// Token: 0x17000DB1 RID: 3505
	// (get) Token: 0x06001DD5 RID: 7637 RVA: 0x0000F6F7 File Offset: 0x0000D8F7
	// (set) Token: 0x06001DD6 RID: 7638 RVA: 0x0000F6FF File Offset: 0x0000D8FF
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

	// Token: 0x06001DD7 RID: 7639 RVA: 0x0000F708 File Offset: 0x0000D908
	public override void SetVelocity(float velocityX, float velocityY, bool additive)
	{
		this.m_previousAirVelocityX = velocityX;
		base.SetVelocity(velocityX, velocityY, additive);
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x0000F71A File Offset: 0x0000D91A
	public override void SetVelocityX(float velocity, bool additive)
	{
		this.m_previousAirVelocityX = velocity;
		base.SetVelocityX(velocity, additive);
	}

	// Token: 0x17000DB2 RID: 3506
	// (get) Token: 0x06001DD9 RID: 7641 RVA: 0x0000F72B File Offset: 0x0000D92B
	// (set) Token: 0x06001DDA RID: 7642 RVA: 0x0000F733 File Offset: 0x0000D933
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

	// Token: 0x17000DB3 RID: 3507
	// (get) Token: 0x06001DDB RID: 7643 RVA: 0x0000F73C File Offset: 0x0000D93C
	// (set) Token: 0x06001DDC RID: 7644 RVA: 0x0000F744 File Offset: 0x0000D944
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

	// Token: 0x17000DB4 RID: 3508
	// (get) Token: 0x06001DDD RID: 7645 RVA: 0x0000F74D File Offset: 0x0000D94D
	// (set) Token: 0x06001DDE RID: 7646 RVA: 0x0000F755 File Offset: 0x0000D955
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

	// Token: 0x17000DB5 RID: 3509
	// (get) Token: 0x06001DDF RID: 7647 RVA: 0x0000F76B File Offset: 0x0000D96B
	// (set) Token: 0x06001DE0 RID: 7648 RVA: 0x0000F773 File Offset: 0x0000D973
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

	// Token: 0x17000DB6 RID: 3510
	// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x0000F77C File Offset: 0x0000D97C
	// (set) Token: 0x06001DE2 RID: 7650 RVA: 0x0000F784 File Offset: 0x0000D984
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

	// Token: 0x17000DB7 RID: 3511
	// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x00003713 File Offset: 0x00001913
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x17000DB8 RID: 3512
	// (get) Token: 0x06001DE4 RID: 7652 RVA: 0x0000F78D File Offset: 0x0000D98D
	public bool IsTargetToMyRight
	{
		get
		{
			return this.Target && this.Target.transform.position.x > base.transform.position.x;
		}
	}

	// Token: 0x17000DB9 RID: 3513
	// (get) Token: 0x06001DE5 RID: 7653 RVA: 0x0000F7C6 File Offset: 0x0000D9C6
	// (set) Token: 0x06001DE6 RID: 7654 RVA: 0x0000F7CE File Offset: 0x0000D9CE
	public bool FollowTarget { get; set; }

	// Token: 0x17000DBA RID: 3514
	// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x0000F7D7 File Offset: 0x0000D9D7
	// (set) Token: 0x06001DE8 RID: 7656 RVA: 0x0000F7DF File Offset: 0x0000D9DF
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

	// Token: 0x17000DBB RID: 3515
	// (get) Token: 0x06001DE9 RID: 7657 RVA: 0x0000F7E8 File Offset: 0x0000D9E8
	// (set) Token: 0x06001DEA RID: 7658 RVA: 0x0000F7F0 File Offset: 0x0000D9F0
	public FlyingMovementType FlyingMovementType { get; set; }

	// Token: 0x17000DBC RID: 3516
	// (get) Token: 0x06001DEB RID: 7659 RVA: 0x0000F7F9 File Offset: 0x0000D9F9
	// (set) Token: 0x06001DEC RID: 7660 RVA: 0x0000F801 File Offset: 0x0000DA01
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

	// Token: 0x17000DBD RID: 3517
	// (get) Token: 0x06001DED RID: 7661 RVA: 0x0000F837 File Offset: 0x0000DA37
	// (set) Token: 0x06001DEE RID: 7662 RVA: 0x0000F83F File Offset: 0x0000DA3F
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

	// Token: 0x17000DBE RID: 3518
	// (get) Token: 0x06001DEF RID: 7663 RVA: 0x0000F848 File Offset: 0x0000DA48
	// (set) Token: 0x06001DF0 RID: 7664 RVA: 0x0000F850 File Offset: 0x0000DA50
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

	// Token: 0x17000DBF RID: 3519
	// (get) Token: 0x06001DF1 RID: 7665 RVA: 0x0000F86A File Offset: 0x0000DA6A
	public BaseCharacterController TargetController
	{
		get
		{
			return this.m_targetController;
		}
	}

	// Token: 0x17000DC0 RID: 3520
	// (get) Token: 0x06001DF2 RID: 7666 RVA: 0x0000F872 File Offset: 0x0000DA72
	public virtual float ActualSummonValue
	{
		get
		{
			return Enemy_EV.GetSummonValue(this.EnemyRank);
		}
	}

	// Token: 0x17000DC1 RID: 3521
	// (get) Token: 0x06001DF3 RID: 7667 RVA: 0x0000F87F File Offset: 0x0000DA7F
	// (set) Token: 0x06001DF4 RID: 7668 RVA: 0x0000F887 File Offset: 0x0000DA87
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

	// Token: 0x17000DC2 RID: 3522
	// (get) Token: 0x06001DF5 RID: 7669 RVA: 0x0000F890 File Offset: 0x0000DA90
	// (set) Token: 0x06001DF6 RID: 7670 RVA: 0x0000F898 File Offset: 0x0000DA98
	public float SpeedMod { get; protected set; }

	// Token: 0x17000DC3 RID: 3523
	// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x0000F8A1 File Offset: 0x0000DAA1
	// (set) Token: 0x06001DF8 RID: 7672 RVA: 0x0000F8A9 File Offset: 0x0000DAA9
	public float SpeedAdd { get; protected set; }

	// Token: 0x17000DC4 RID: 3524
	// (get) Token: 0x06001DF9 RID: 7673 RVA: 0x0000F8B2 File Offset: 0x0000DAB2
	public virtual float ActualSpeed
	{
		get
		{
			return (this.BaseSpeed + this.SpeedAdd) * (1f + this.SpeedMod);
		}
	}

	// Token: 0x17000DC5 RID: 3525
	// (get) Token: 0x06001DFA RID: 7674 RVA: 0x0000F8CE File Offset: 0x0000DACE
	// (set) Token: 0x06001DFB RID: 7675 RVA: 0x0000F8D6 File Offset: 0x0000DAD6
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

	// Token: 0x17000DC6 RID: 3526
	// (get) Token: 0x06001DFC RID: 7676 RVA: 0x0000F8DF File Offset: 0x0000DADF
	public virtual float ActualTurnSpeed
	{
		get
		{
			return this.BaseTurnSpeed;
		}
	}

	// Token: 0x17000DC7 RID: 3527
	// (get) Token: 0x06001DFD RID: 7677 RVA: 0x0000F8E7 File Offset: 0x0000DAE7
	// (set) Token: 0x06001DFE RID: 7678 RVA: 0x0000F8EF File Offset: 0x0000DAEF
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

	// Token: 0x17000DC8 RID: 3528
	// (get) Token: 0x06001DFF RID: 7679 RVA: 0x0000F8F8 File Offset: 0x0000DAF8
	public virtual Vector2 ActualRestCooldown
	{
		get
		{
			return this.BaseRestCooldown;
		}
	}

	// Token: 0x17000DC9 RID: 3529
	// (get) Token: 0x06001E00 RID: 7680 RVA: 0x0000F900 File Offset: 0x0000DB00
	// (set) Token: 0x06001E01 RID: 7681 RVA: 0x0009CC18 File Offset: 0x0009AE18
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

	// Token: 0x17000DCA RID: 3530
	// (get) Token: 0x06001E02 RID: 7682 RVA: 0x0000F908 File Offset: 0x0000DB08
	// (set) Token: 0x06001E03 RID: 7683 RVA: 0x0000F910 File Offset: 0x0000DB10
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

	// Token: 0x17000DCB RID: 3531
	// (get) Token: 0x06001E04 RID: 7684 RVA: 0x0000F935 File Offset: 0x0000DB35
	// (set) Token: 0x06001E05 RID: 7685 RVA: 0x0000F93D File Offset: 0x0000DB3D
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

	// Token: 0x17000DCC RID: 3532
	// (get) Token: 0x06001E06 RID: 7686 RVA: 0x0000F946 File Offset: 0x0000DB46
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x17000DCD RID: 3533
	// (get) Token: 0x06001E07 RID: 7687 RVA: 0x0000F94E File Offset: 0x0000DB4E
	// (set) Token: 0x06001E08 RID: 7688 RVA: 0x0000F956 File Offset: 0x0000DB56
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

	// Token: 0x17000DCE RID: 3534
	// (get) Token: 0x06001E09 RID: 7689 RVA: 0x0000F95F File Offset: 0x0000DB5F
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x17000DCF RID: 3535
	// (get) Token: 0x06001E0A RID: 7690 RVA: 0x0000F967 File Offset: 0x0000DB67
	// (set) Token: 0x06001E0B RID: 7691 RVA: 0x0000F96F File Offset: 0x0000DB6F
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

	// Token: 0x17000DD0 RID: 3536
	// (get) Token: 0x06001E0C RID: 7692 RVA: 0x0000F978 File Offset: 0x0000DB78
	public virtual float ActualDropOdds
	{
		get
		{
			return this.BaseDropOdds;
		}
	}

	// Token: 0x17000DD1 RID: 3537
	// (get) Token: 0x06001E0D RID: 7693 RVA: 0x0000F980 File Offset: 0x0000DB80
	// (set) Token: 0x06001E0E RID: 7694 RVA: 0x0000F988 File Offset: 0x0000DB88
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

	// Token: 0x17000DD2 RID: 3538
	// (get) Token: 0x06001E0F RID: 7695 RVA: 0x0000F9AD File Offset: 0x0000DBAD
	// (set) Token: 0x06001E10 RID: 7696 RVA: 0x0000F9B5 File Offset: 0x0000DBB5
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

	// Token: 0x17000DD3 RID: 3539
	// (get) Token: 0x06001E11 RID: 7697 RVA: 0x0009CC9C File Offset: 0x0009AE9C
	// (set) Token: 0x06001E12 RID: 7698 RVA: 0x0000F9DB File Offset: 0x0000DBDB
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

	// Token: 0x06001E13 RID: 7699 RVA: 0x0000FA00 File Offset: 0x0000DC00
	public override void SetHealth(float value, bool additive, bool runEvents)
	{
		base.SetHealth(value, additive, runEvents);
		if (runEvents)
		{
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EnemyHealthChange, this, this.m_healthChangeEventArgs);
		}
	}

	// Token: 0x17000DD4 RID: 3540
	// (get) Token: 0x06001E14 RID: 7700 RVA: 0x0000FA1B File Offset: 0x0000DC1B
	public int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x0000FA23 File Offset: 0x0000DC23
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

	// Token: 0x17000DD5 RID: 3541
	// (get) Token: 0x06001E16 RID: 7702 RVA: 0x0000FA5D File Offset: 0x0000DC5D
	// (set) Token: 0x06001E17 RID: 7703 RVA: 0x0009CCEC File Offset: 0x0009AEEC
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

	// Token: 0x17000DD6 RID: 3542
	// (get) Token: 0x06001E18 RID: 7704 RVA: 0x0000FA65 File Offset: 0x0000DC65
	// (set) Token: 0x06001E19 RID: 7705 RVA: 0x0009CD1C File Offset: 0x0009AF1C
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

	// Token: 0x17000DD7 RID: 3543
	// (get) Token: 0x06001E1A RID: 7706 RVA: 0x0000FA6D File Offset: 0x0000DC6D
	// (set) Token: 0x06001E1B RID: 7707 RVA: 0x0000FA75 File Offset: 0x0000DC75
	public BaseRoom Room { get; private set; }

	// Token: 0x17000DD8 RID: 3544
	// (get) Token: 0x06001E1C RID: 7708 RVA: 0x0000FA7E File Offset: 0x0000DC7E
	// (set) Token: 0x06001E1D RID: 7709 RVA: 0x0000FA86 File Offset: 0x0000DC86
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

	// Token: 0x17000DD9 RID: 3545
	// (get) Token: 0x06001E1E RID: 7710 RVA: 0x0000FA8F File Offset: 0x0000DC8F
	public bool TouchingLeftRoomEdge
	{
		get
		{
			return this.m_touchingLeftRoomEdge;
		}
	}

	// Token: 0x17000DDA RID: 3546
	// (get) Token: 0x06001E1F RID: 7711 RVA: 0x0000FA97 File Offset: 0x0000DC97
	public bool TouchingRightRoomEdge
	{
		get
		{
			return this.m_touchingRightRoomEdge;
		}
	}

	// Token: 0x17000DDB RID: 3547
	// (get) Token: 0x06001E20 RID: 7712 RVA: 0x0000FA9F File Offset: 0x0000DC9F
	public bool TouchingTopRoomEdge
	{
		get
		{
			return this.m_touchingTopRoomEdge;
		}
	}

	// Token: 0x17000DDC RID: 3548
	// (get) Token: 0x06001E21 RID: 7713 RVA: 0x0000FAA7 File Offset: 0x0000DCA7
	public bool TouchingBottomRoomEdge
	{
		get
		{
			return this.m_touchingBottomRoomEdge;
		}
	}

	// Token: 0x17000DDD RID: 3549
	// (get) Token: 0x06001E22 RID: 7714 RVA: 0x0000FAAF File Offset: 0x0000DCAF
	// (set) Token: 0x06001E23 RID: 7715 RVA: 0x0000FAB7 File Offset: 0x0000DCB7
	public bool ActivatedByFairyRoomTrigger { get; set; }

	// Token: 0x17000DDE RID: 3550
	// (get) Token: 0x06001E24 RID: 7716 RVA: 0x0000FAC0 File Offset: 0x0000DCC0
	// (set) Token: 0x06001E25 RID: 7717 RVA: 0x0000FAC8 File Offset: 0x0000DCC8
	public bool PreserveRotationWhenDeactivated { get; set; }

	// Token: 0x17000DDF RID: 3551
	// (get) Token: 0x06001E26 RID: 7718 RVA: 0x0000FAD1 File Offset: 0x0000DCD1
	// (set) Token: 0x06001E27 RID: 7719 RVA: 0x0000FAD9 File Offset: 0x0000DCD9
	public bool ForceDisableSummonOffset { get; set; }

	// Token: 0x17000DE0 RID: 3552
	// (get) Token: 0x06001E28 RID: 7720 RVA: 0x0000FAE2 File Offset: 0x0000DCE2
	// (set) Token: 0x06001E29 RID: 7721 RVA: 0x0000FAEA File Offset: 0x0000DCEA
	public bool InvisibleDuringSummonAnim { get; set; }

	// Token: 0x06001E2A RID: 7722 RVA: 0x0009CD4C File Offset: 0x0009AF4C
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

	// Token: 0x06001E2B RID: 7723 RVA: 0x0000FAF3 File Offset: 0x0000DCF3
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

	// Token: 0x06001E2C RID: 7724 RVA: 0x0000FB02 File Offset: 0x0000DD02
	public void ForceFaceTarget()
	{
		if (this.Target)
		{
			base.Heading = this.TargetController.Midpoint - base.Midpoint;
		}
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x0009CE34 File Offset: 0x0009B034
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

	// Token: 0x06001E2E RID: 7726 RVA: 0x0000FB32 File Offset: 0x0000DD32
	private void OnEnable()
	{
		if (!base.IsInitialized && this.m_enemyInitializing)
		{
			base.StartCoroutine(this.Start());
		}
		this.DisableEffectsOnSpawn();
	}

	// Token: 0x06001E2F RID: 7727 RVA: 0x0000FB57 File Offset: 0x0000DD57
	private void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
		this.FlightKnockbackAcceleration = Vector2.zero;
		if (!GameManager.IsApplicationClosing)
		{
			this.m_onDisableRelay.Dispatch();
		}
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x0009CECC File Offset: 0x0009B0CC
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

	// Token: 0x06001E31 RID: 7729 RVA: 0x0000FB7D File Offset: 0x0000DD7D
	public void ForceStandingOn(Collider2D collider)
	{
		if (this.m_controllerCorgi)
		{
			this.m_controllerCorgi.ForceStandingOn(collider);
		}
	}

	// Token: 0x06001E32 RID: 7730 RVA: 0x0000FB98 File Offset: 0x0000DD98
	public void DisableEffectsOnSpawn()
	{
		base.StartCoroutine(this.DisableEffectsOnSpawnCoroutine(0.1f));
	}

	// Token: 0x06001E33 RID: 7731 RVA: 0x0000FBAC File Offset: 0x0000DDAC
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

	// Token: 0x06001E34 RID: 7732 RVA: 0x0000FBC2 File Offset: 0x0000DDC2
	public void ResetCollisionState()
	{
		if (this.m_controllerCorgi)
		{
			this.m_controllerCorgi.ResetState();
		}
	}

	// Token: 0x06001E35 RID: 7733 RVA: 0x0000FBDC File Offset: 0x0000DDDC
	public void UpdateBounds()
	{
		if (this.m_controllerCorgi)
		{
			this.m_controllerCorgi.SetRaysParameters();
		}
	}

	// Token: 0x06001E36 RID: 7734 RVA: 0x0009CFC8 File Offset: 0x0009B1C8
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

	// Token: 0x06001E37 RID: 7735 RVA: 0x0009D22C File Offset: 0x0009B42C
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

	// Token: 0x06001E38 RID: 7736 RVA: 0x0009D3FC File Offset: 0x0009B5FC
	public void AddCommanderStatusEffect(StatusEffectType statusEffect)
	{
		int num = StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.IndexOf(statusEffect);
		if (num != -1)
		{
			this.m_commanderStatusEffectFlags |= 1 << num;
		}
	}

	// Token: 0x06001E39 RID: 7737 RVA: 0x0009D42C File Offset: 0x0009B62C
	public void RemoveCommanderStatusEffect(StatusEffectType statusEffect)
	{
		int num = StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.IndexOf(statusEffect);
		if (num != -1)
		{
			this.m_commanderStatusEffectFlags &= ~(1 << num);
		}
	}

	// Token: 0x06001E3A RID: 7738 RVA: 0x0009D460 File Offset: 0x0009B660
	public bool HasCommanderStatusEffect(StatusEffectType statusEffect)
	{
		int num = StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.IndexOf(statusEffect);
		return num != -1 && (this.m_commanderStatusEffectFlags & 1 << num) != 0;
	}

	// Token: 0x06001E3B RID: 7739 RVA: 0x0000FBF6 File Offset: 0x0000DDF6
	public void RemoveAllCommanderStatusEffects()
	{
		this.m_commanderStatusEffectFlags = 0;
	}

	// Token: 0x06001E3C RID: 7740 RVA: 0x0009D490 File Offset: 0x0009B690
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

	// Token: 0x06001E3D RID: 7741 RVA: 0x0009D500 File Offset: 0x0009B700
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

	// Token: 0x06001E3E RID: 7742 RVA: 0x0000FBFF File Offset: 0x0000DDFF
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

	// Token: 0x06001E3F RID: 7743 RVA: 0x0009D644 File Offset: 0x0009B844
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

	// Token: 0x06001E40 RID: 7744 RVA: 0x0009D7F0 File Offset: 0x0009B9F0
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

	// Token: 0x06001E41 RID: 7745 RVA: 0x0009D8A4 File Offset: 0x0009BAA4
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

	// Token: 0x06001E42 RID: 7746 RVA: 0x0009D8F8 File Offset: 0x0009BAF8
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

	// Token: 0x06001E43 RID: 7747 RVA: 0x0009DC1C File Offset: 0x0009BE1C
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

	// Token: 0x06001E44 RID: 7748 RVA: 0x0009DD44 File Offset: 0x0009BF44
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

	// Token: 0x06001E45 RID: 7749 RVA: 0x0009DDEC File Offset: 0x0009BFEC
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

	// Token: 0x06001E46 RID: 7750 RVA: 0x0009DE94 File Offset: 0x0009C094
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

	// Token: 0x06001E47 RID: 7751 RVA: 0x0009DFF8 File Offset: 0x0009C1F8
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

	// Token: 0x06001E48 RID: 7752 RVA: 0x0009E720 File Offset: 0x0009C920
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

	// Token: 0x06001E49 RID: 7753 RVA: 0x0009E964 File Offset: 0x0009CB64
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

	// Token: 0x06001E4A RID: 7754 RVA: 0x0009EAA0 File Offset: 0x0009CCA0
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

	// Token: 0x06001E4B RID: 7755 RVA: 0x0000FC0E File Offset: 0x0000DE0E
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

	// Token: 0x06001E4C RID: 7756 RVA: 0x0009EE28 File Offset: 0x0009D028
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

	// Token: 0x06001E4D RID: 7757 RVA: 0x0009F000 File Offset: 0x0009D200
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

	// Token: 0x06001E4E RID: 7758 RVA: 0x0000FC2B File Offset: 0x0000DE2B
	public override void ResetStates()
	{
		base.ResetStates();
		this.LogicController.ResetLogic();
		this.FollowOffset = Vector2.zero;
		this.JumpHorizontalVelocity = 0f;
		this.LeftSidePlatformDropPrevented = false;
		this.RightSidePlatformDropPrevented = false;
	}

	// Token: 0x06001E4F RID: 7759 RVA: 0x0000FC67 File Offset: 0x0000DE67
	public void ResetPivotRotation()
	{
		if (base.Pivot)
		{
			base.Pivot.transform.eulerAngles = Vector3.zero;
		}
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x0000FC8B File Offset: 0x0000DE8B
	public void ResetTurnTrigger()
	{
		if (base.Animator && global::AnimatorUtility.HasParameter(base.Animator, EnemyController.TURN_ANIMATION_TRIGGER_NAME))
		{
			base.Animator.ResetTrigger(EnemyController.TURN_ANIMATION_TRIGGER_NAME);
		}
	}

	// Token: 0x06001E51 RID: 7761 RVA: 0x0000FCBC File Offset: 0x0000DEBC
	public override void ResetMods()
	{
		base.ResetMods();
		this.SpeedMod = 0f;
		this.SpeedAdd = 0f;
	}

	// Token: 0x06001E52 RID: 7762 RVA: 0x0009F038 File Offset: 0x0009D238
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

	// Token: 0x06001E53 RID: 7763 RVA: 0x0000FCDA File Offset: 0x0000DEDA
	public void ActivateEnemy()
	{
		this.LogicController.ResetLogic();
		EnemyController.m_enemyActivationStateChangedEventArgs_STATIC.Initialize(this);
		this.m_onActivatedRelay.Dispatch(this, EnemyController.m_enemyActivationStateChangedEventArgs_STATIC);
	}

	// Token: 0x06001E54 RID: 7764 RVA: 0x0000FD03 File Offset: 0x0000DF03
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

	// Token: 0x06001E55 RID: 7765 RVA: 0x0000FD3B File Offset: 0x0000DF3B
	public void EnemyTimedOut()
	{
		EnemyController.m_enemyActivationStateChangedEventArgs_STATIC.Initialize(this);
		this.m_onReactivationTimedOutRelay.Dispatch(this, EnemyController.m_enemyActivationStateChangedEventArgs_STATIC);
	}

	// Token: 0x06001E56 RID: 7766 RVA: 0x0000FD59 File Offset: 0x0000DF59
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x0000FD62 File Offset: 0x0000DF62
	public void SetEnemyIndex(int index)
	{
		this.EnemyIndex = index;
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x0000FD6B File Offset: 0x0000DF6B
	public void ResetValues()
	{
		this.ResetCharacter();
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x0000FD73 File Offset: 0x0000DF73
	public override void ResetHealth()
	{
		this.SetHealth((float)this.ActualMaxHealth, false, false);
	}

	// Token: 0x06001E5A RID: 7770 RVA: 0x0009F088 File Offset: 0x0009D288
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

	// Token: 0x06001E5D RID: 7773 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISummoner.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06001E5E RID: 7774 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06001E5F RID: 7775 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06001E60 RID: 7776 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IOffscreenObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001AD3 RID: 6867
	private const float CHASE_DISTANCE_THRESHOLD = 0.1f;

	// Token: 0x04001AD4 RID: 6868
	private static int LAND_ANIMATION_STATE_NAME = Animator.StringToHash("Land");

	// Token: 0x04001AD5 RID: 6869
	private static int TURN_ANIMATION_TRIGGER_NAME = Animator.StringToHash("Turn");

	// Token: 0x04001AD6 RID: 6870
	private static RelicChangedEventArgs m_relicChangedEventArgs_STATIC = new RelicChangedEventArgs(RelicType.None);

	// Token: 0x04001AD7 RID: 6871
	[Header("Enemy Values")]
	[SerializeField]
	[HideInInspector]
	private EnemyType m_enemyType = EnemyType.Skeleton;

	// Token: 0x04001AD8 RID: 6872
	[SerializeField]
	[HideInInspector]
	private EnemyRank m_enemyRank;

	// Token: 0x04001AD9 RID: 6873
	[Space(10f)]
	[SerializeField]
	private float m_flightKnockbackDecelerationMod = 1f;

	// Token: 0x04001ADA RID: 6874
	[SerializeField]
	private bool m_constrainToRoom = true;

	// Token: 0x04001ADB RID: 6875
	[SerializeField]
	private bool m_knocksBackOnHit;

	// Token: 0x04001ADC RID: 6876
	[SerializeField]
	private StrikeType m_strikeType = StrikeType.Blunt;

	// Token: 0x04001ADD RID: 6877
	[SerializeField]
	private bool m_isBoss;

	// Token: 0x04001ADE RID: 6878
	[SerializeField]
	private bool m_forceActivate;

	// Token: 0x04001ADF RID: 6879
	[SerializeField]
	private bool m_disableXPBonuses;

	// Token: 0x04001AE0 RID: 6880
	[SerializeField]
	private bool m_disableHPMPBonuses;

	// Token: 0x04001AE1 RID: 6881
	[SerializeField]
	private bool m_alwaysDealsContactDamage;

	// Token: 0x04001AE2 RID: 6882
	[SerializeField]
	private GameObject[] m_culledObjectsArray;

	// Token: 0x04001AE3 RID: 6883
	private static Dictionary<EnemyTypeAndRank, EnemyData> EnemyData_StaticInstanceDict = new Dictionary<EnemyTypeAndRank, EnemyData>();

	// Token: 0x04001AE4 RID: 6884
	private float m_baseSpeed;

	// Token: 0x04001AE5 RID: 6885
	private float m_baseTurnSpeed;

	// Token: 0x04001AE6 RID: 6886
	private Vector2 m_baseRestCooldown;

	// Token: 0x04001AE7 RID: 6887
	private bool m_isFlying;

	// Token: 0x04001AE8 RID: 6888
	private bool m_alwaysFace;

	// Token: 0x04001AE9 RID: 6889
	private bool m_collidesWithPlatforms;

	// Token: 0x04001AEA RID: 6890
	private float m_baseKnockbackStrength;

	// Token: 0x04001AEB RID: 6891
	private float m_baseStunStrength;

	// Token: 0x04001AEC RID: 6892
	private float m_baseDropOdds;

	// Token: 0x04001AED RID: 6893
	private float m_closeRadius;

	// Token: 0x04001AEE RID: 6894
	private float m_mediumRadius;

	// Token: 0x04001AEF RID: 6895
	private float m_farRadius;

	// Token: 0x04001AF0 RID: 6896
	protected GameObject m_target;

	// Token: 0x04001AF1 RID: 6897
	protected BaseCharacterController m_targetController;

	// Token: 0x04001AF2 RID: 6898
	private Vector3 m_followOffset;

	// Token: 0x04001AF3 RID: 6899
	private float m_baseScaleToOffsetWith = 1f;

	// Token: 0x04001AF4 RID: 6900
	private LogicController m_logicController;

	// Token: 0x04001AF5 RID: 6901
	private bool m_pivotFollowsOrientation;

	// Token: 0x04001AF6 RID: 6902
	private EnemyData m_enemyData;

	// Token: 0x04001AF7 RID: 6903
	private int m_level = 1;

	// Token: 0x04001AF8 RID: 6904
	private float m_previousAirVelocityX;

	// Token: 0x04001AF9 RID: 6905
	private PreventPlatformDrop m_preventPlatformDropObj;

	// Token: 0x04001AFA RID: 6906
	private bool m_fallLedge;

	// Token: 0x04001AFB RID: 6907
	[NonSerialized]
	public Vector2 FlightKnockbackAcceleration;

	// Token: 0x04001AFC RID: 6908
	[NonSerialized]
	public bool CanIncrementRelicHitCounter = true;

	// Token: 0x04001AFD RID: 6909
	[NonSerialized]
	public bool AttackingWithContactDamage;

	// Token: 0x04001AFE RID: 6910
	[NonSerialized]
	public bool DisableDeath;

	// Token: 0x04001AFF RID: 6911
	private static EnemyDeathEventArgs m_enemyDeathEventArgs_STATIC;

	// Token: 0x04001B00 RID: 6912
	private static EnemyActivationStateChangedEventArgs m_enemyActivationStateChangedEventArgs_STATIC;

	// Token: 0x04001B01 RID: 6913
	private int m_enemyIndex = -1;

	// Token: 0x04001B02 RID: 6914
	private bool m_enemyInitializing;

	// Token: 0x04001B03 RID: 6915
	private bool m_touchingLeftRoomEdge;

	// Token: 0x04001B04 RID: 6916
	private bool m_touchingRightRoomEdge;

	// Token: 0x04001B05 RID: 6917
	private bool m_touchingTopRoomEdge;

	// Token: 0x04001B06 RID: 6918
	private bool m_touchingBottomRoomEdge;

	// Token: 0x04001B07 RID: 6919
	private int m_commanderStatusEffectFlags;

	// Token: 0x04001B0F RID: 6927
	private DamageType m_lastDamageTypeTaken;

	// Token: 0x04001B1C RID: 6940
	private Relay<object, EnemyActivationStateChangedEventArgs> m_onActivatedRelay = new Relay<object, EnemyActivationStateChangedEventArgs>();

	// Token: 0x04001B1D RID: 6941
	private Relay<object, EnemyActivationStateChangedEventArgs> m_onDeactivatedRelay = new Relay<object, EnemyActivationStateChangedEventArgs>();

	// Token: 0x04001B1E RID: 6942
	private Relay<object, EnemyActivationStateChangedEventArgs> m_onReactivationTimedOutRelay = new Relay<object, EnemyActivationStateChangedEventArgs>();

	// Token: 0x04001B1F RID: 6943
	private Relay<object, EnemyDeathEventArgs> m_onEnemyDeathRelay = new Relay<object, EnemyDeathEventArgs>();

	// Token: 0x04001B20 RID: 6944
	private Relay<object, EventArgs> m_onResetPositionRelay = new Relay<object, EventArgs>();

	// Token: 0x04001B21 RID: 6945
	private Relay<object, EventArgs> m_onPositionedForSummoningRelay = new Relay<object, EventArgs>();

	// Token: 0x04001B22 RID: 6946
	private Relay m_onDisableRelay = new Relay();
}
