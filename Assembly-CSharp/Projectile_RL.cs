using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020007C3 RID: 1987
public class Projectile_RL : MonoBehaviour, IDamageObj, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnStayHitResponse, IWeaponOnEnterHitResponse, IWeaponOnStayHitResponse, IBodyOnEnterHitResponse, IBodyOnStayHitResponse, IRootObj, IMidpointObj, IHeading, IEffectVelocity, IEffectTriggerEvent_OnDeath, IEffectTriggerEvent_OnSpawn, IEffectTriggerEvent_OnTimeout, IGenericPoolObj, IPreOnDisable, IPlayHitEffect, IPivotObj, IOffscreenObj
{
	// Token: 0x06003C50 RID: 15440 RVA: 0x00021554 File Offset: 0x0001F754
	public static bool OwnsProjectile(GameObject go, Projectile_RL projectile)
	{
		return projectile && !projectile.m_flagToDestroy && projectile.isActiveAndEnabled && projectile.Owner == go;
	}

	// Token: 0x06003C51 RID: 15441 RVA: 0x0002157C File Offset: 0x0001F77C
	public static bool CollisionFlagAllowed(Projectile_RL proj1, Projectile_RL proj2)
	{
		return (proj1.CanCollideWithFlags & proj2.CollisionFlags) != ProjectileCollisionFlag.None || (proj2.CanCollideWithFlags & proj1.CollisionFlags) > ProjectileCollisionFlag.None;
	}

	// Token: 0x17001609 RID: 5641
	// (get) Token: 0x06003C52 RID: 15442 RVA: 0x0002159F File Offset: 0x0001F79F
	public bool DisableOffscreenWarnings
	{
		get
		{
			return this.m_disableOffscreenWarnings;
		}
	}

	// Token: 0x1700160A RID: 5642
	// (get) Token: 0x06003C53 RID: 15443 RVA: 0x000215A7 File Offset: 0x0001F7A7
	// (set) Token: 0x06003C54 RID: 15444 RVA: 0x000215AF File Offset: 0x0001F7AF
	public bool MatchFacing { get; set; }

	// Token: 0x1700160B RID: 5643
	// (get) Token: 0x06003C55 RID: 15445 RVA: 0x000215B8 File Offset: 0x0001F7B8
	public float InitialRotationSpeed
	{
		get
		{
			return this.m_initialRotationSpeed;
		}
	}

	// Token: 0x1700160C RID: 5644
	// (get) Token: 0x06003C56 RID: 15446 RVA: 0x000215C0 File Offset: 0x0001F7C0
	// (set) Token: 0x06003C57 RID: 15447 RVA: 0x000215C8 File Offset: 0x0001F7C8
	public bool IsPersistentProjectile { get; set; }

	// Token: 0x1700160D RID: 5645
	// (get) Token: 0x06003C58 RID: 15448 RVA: 0x000215D1 File Offset: 0x0001F7D1
	public bool HasIntro
	{
		get
		{
			return this.m_hasIntro;
		}
	}

	// Token: 0x1700160E RID: 5646
	// (get) Token: 0x06003C59 RID: 15449 RVA: 0x000215D9 File Offset: 0x0001F7D9
	// (set) Token: 0x06003C5A RID: 15450 RVA: 0x000215E1 File Offset: 0x0001F7E1
	public float CDReducDelay { get; set; }

	// Token: 0x1700160F RID: 5647
	// (get) Token: 0x06003C5B RID: 15451 RVA: 0x000215EA File Offset: 0x0001F7EA
	public bool HasCDReductionDelay
	{
		get
		{
			return this.m_hasCDReductionDelay;
		}
	}

	// Token: 0x17001610 RID: 5648
	// (get) Token: 0x06003C5C RID: 15452 RVA: 0x000215F2 File Offset: 0x0001F7F2
	// (set) Token: 0x06003C5D RID: 15453 RVA: 0x000215FA File Offset: 0x0001F7FA
	public Vector2 CollisionPointOffset { get; set; } = Vector2.zero;

	// Token: 0x17001611 RID: 5649
	// (get) Token: 0x06003C5E RID: 15454 RVA: 0x00021603 File Offset: 0x0001F803
	// (set) Token: 0x06003C5F RID: 15455 RVA: 0x0002160B File Offset: 0x0001F80B
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001612 RID: 5650
	// (get) Token: 0x06003C60 RID: 15456 RVA: 0x00021614 File Offset: 0x0001F814
	public ProjectileCollisionFlag CanCollideWithFlags
	{
		get
		{
			return this.m_canCollideWithFlag;
		}
	}

	// Token: 0x17001613 RID: 5651
	// (get) Token: 0x06003C61 RID: 15457 RVA: 0x0002161C File Offset: 0x0001F81C
	public ProjectileCollisionFlag CollisionFlags
	{
		get
		{
			return this.m_collisionFlag;
		}
	}

	// Token: 0x17001614 RID: 5652
	// (get) Token: 0x06003C62 RID: 15458 RVA: 0x00021624 File Offset: 0x0001F824
	// (set) Token: 0x06003C63 RID: 15459 RVA: 0x0002162C File Offset: 0x0001F82C
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17001615 RID: 5653
	// (get) Token: 0x06003C64 RID: 15460 RVA: 0x00021635 File Offset: 0x0001F835
	public ProjectileEventEmitter AudioEventEmitter
	{
		get
		{
			if (!this.m_audioEventEmitter && !this.m_hasSearchedForAudioEventEmitter)
			{
				this.m_audioEventEmitter = base.GetComponent<ProjectileEventEmitter>();
				this.m_hasSearchedForAudioEventEmitter = true;
			}
			return this.m_audioEventEmitter;
		}
	}

	// Token: 0x17001616 RID: 5654
	// (get) Token: 0x06003C65 RID: 15461 RVA: 0x00021665 File Offset: 0x0001F865
	// (set) Token: 0x06003C66 RID: 15462 RVA: 0x0002166D File Offset: 0x0001F86D
	public CastAbilityType CastAbilityType { get; set; }

	// Token: 0x17001617 RID: 5655
	// (get) Token: 0x06003C67 RID: 15463 RVA: 0x00021676 File Offset: 0x0001F876
	public bool DestroyOnRoomChange
	{
		get
		{
			return this.m_destroyOnRoomChange;
		}
	}

	// Token: 0x17001618 RID: 5656
	// (get) Token: 0x06003C68 RID: 15464 RVA: 0x0002167E File Offset: 0x0001F87E
	public float[] StatusEffectDurations
	{
		get
		{
			return this.m_statusEffectDurationArray;
		}
	}

	// Token: 0x17001619 RID: 5657
	// (get) Token: 0x06003C69 RID: 15465 RVA: 0x00021686 File Offset: 0x0001F886
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return this.m_statusEffectTypeArray;
		}
	}

	// Token: 0x1700161A RID: 5658
	// (get) Token: 0x06003C6A RID: 15466 RVA: 0x0002168E File Offset: 0x0001F88E
	public bool IsDotDamage
	{
		get
		{
			return this.m_isDotDamage;
		}
	}

	// Token: 0x1700161B RID: 5659
	// (get) Token: 0x06003C6B RID: 15467 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700161C RID: 5660
	// (get) Token: 0x06003C6C RID: 15468 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool PlayDirectionalHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700161D RID: 5661
	// (get) Token: 0x06003C6D RID: 15469 RVA: 0x0000F49B File Offset: 0x0000D69B
	public string EffectNameOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1700161E RID: 5662
	// (get) Token: 0x06003C6E RID: 15470 RVA: 0x00021696 File Offset: 0x0001F896
	public IRelayLink<Projectile_RL, GameObject> OnCollisionRelay
	{
		get
		{
			return this.m_onCollisionRelay.link;
		}
	}

	// Token: 0x06003C6F RID: 15471 RVA: 0x000216A3 File Offset: 0x0001F8A3
	public void ForceDispatchOnCollisionRelay(GameObject otherObj)
	{
		this.m_onCollisionRelay.Dispatch(this, otherObj);
	}

	// Token: 0x1700161F RID: 5663
	// (get) Token: 0x06003C70 RID: 15472 RVA: 0x000216B2 File Offset: 0x0001F8B2
	public IRelayLink<Projectile_RL, GameObject> OnSpawnRelay
	{
		get
		{
			return this.m_onSpawnRelay.link;
		}
	}

	// Token: 0x17001620 RID: 5664
	// (get) Token: 0x06003C71 RID: 15473 RVA: 0x000216BF File Offset: 0x0001F8BF
	public IRelayLink<Projectile_RL, GameObject> OnDeathRelay
	{
		get
		{
			return this.m_onDeathRelay.link;
		}
	}

	// Token: 0x17001621 RID: 5665
	// (get) Token: 0x06003C72 RID: 15474 RVA: 0x000216CC File Offset: 0x0001F8CC
	public IRelayLink<GameObject> OnDeathEffectTriggerRelay
	{
		get
		{
			return this.m_onDeathEffectTriggerRelay.link;
		}
	}

	// Token: 0x17001622 RID: 5666
	// (get) Token: 0x06003C73 RID: 15475 RVA: 0x000216D9 File Offset: 0x0001F8D9
	public IRelayLink<GameObject> OnSpawnEffectTriggerRelay
	{
		get
		{
			return this.m_onSpawnEffectTriggerRelay.link;
		}
	}

	// Token: 0x17001623 RID: 5667
	// (get) Token: 0x06003C74 RID: 15476 RVA: 0x000216E6 File Offset: 0x0001F8E6
	public IRelayLink<GameObject> OnTimeoutEffectTriggerRelay
	{
		get
		{
			return this.m_onTimeoutEffectTriggerRelay.link;
		}
	}

	// Token: 0x17001624 RID: 5668
	// (get) Token: 0x06003C75 RID: 15477 RVA: 0x000216F3 File Offset: 0x0001F8F3
	public IRelayLink<IPreOnDisable> PreOnDisableRelay
	{
		get
		{
			return this.m_preOnDisableRelay.link;
		}
	}

	// Token: 0x17001625 RID: 5669
	// (get) Token: 0x06003C76 RID: 15478 RVA: 0x00021700 File Offset: 0x0001F900
	public IHitboxController HitboxController
	{
		get
		{
			return this.m_hbController;
		}
	}

	// Token: 0x17001626 RID: 5670
	// (get) Token: 0x06003C77 RID: 15479 RVA: 0x00021708 File Offset: 0x0001F908
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17001627 RID: 5671
	// (get) Token: 0x06003C78 RID: 15480 RVA: 0x00021710 File Offset: 0x0001F910
	public float GravityKickInDelay
	{
		get
		{
			return this.m_gravityKickInDelay;
		}
	}

	// Token: 0x17001628 RID: 5672
	// (get) Token: 0x06003C79 RID: 15481 RVA: 0x00021718 File Offset: 0x0001F918
	public float FallMultiplierOverride
	{
		get
		{
			return this.m_fallMultiplierOverride;
		}
	}

	// Token: 0x17001629 RID: 5673
	// (get) Token: 0x06003C7A RID: 15482 RVA: 0x00021720 File Offset: 0x0001F920
	public ProjectileData ProjectileData
	{
		get
		{
			return this.m_projectileData;
		}
	}

	// Token: 0x06003C7B RID: 15483 RVA: 0x00021728 File Offset: 0x0001F928
	public void SetProjectileData(ProjectileData projData)
	{
		this.m_projectileData = projData;
	}

	// Token: 0x1700162A RID: 5674
	// (get) Token: 0x06003C7C RID: 15484 RVA: 0x00021731 File Offset: 0x0001F931
	// (set) Token: 0x06003C7D RID: 15485 RVA: 0x00021739 File Offset: 0x0001F939
	public float LifespanTimer
	{
		get
		{
			return this.m_lifeSpanTimer;
		}
		set
		{
			this.m_lifeSpanTimer = value;
		}
	}

	// Token: 0x1700162B RID: 5675
	// (get) Token: 0x06003C7E RID: 15486 RVA: 0x00021742 File Offset: 0x0001F942
	public StrikeType StrikeType
	{
		get
		{
			return this.m_strikeType;
		}
	}

	// Token: 0x1700162C RID: 5676
	// (get) Token: 0x06003C7F RID: 15487 RVA: 0x0002174A File Offset: 0x0001F94A
	public Vector2 Velocity
	{
		get
		{
			if (!this.m_corgiController)
			{
				return this.m_movement;
			}
			return this.m_corgiController.Velocity;
		}
	}

	// Token: 0x1700162D RID: 5677
	// (get) Token: 0x06003C80 RID: 15488 RVA: 0x000F6524 File Offset: 0x000F4724
	public Vector2 EffectVelocity
	{
		get
		{
			Vector2 vector = Vector2.zero;
			if (this.SnapToOwner && this.m_useOwnerVelocityForEffects)
			{
				if (this.OwnerController)
				{
					vector = this.OwnerController.Velocity;
				}
			}
			else
			{
				vector = this.Heading * this.Speed;
			}
			if (vector.x == 0f)
			{
				if (!this.IsFlipped)
				{
					vector.x = 1f;
				}
				else
				{
					vector.x = -1f;
				}
			}
			return vector;
		}
	}

	// Token: 0x1700162E RID: 5678
	// (get) Token: 0x06003C81 RID: 15489 RVA: 0x0002176B File Offset: 0x0001F96B
	public Vector3 Midpoint
	{
		get
		{
			if (this.m_ownerController && this.UseOwnerCollisionPoint)
			{
				return this.m_ownerController.Midpoint;
			}
			return base.transform.position;
		}
	}

	// Token: 0x1700162F RID: 5679
	// (get) Token: 0x06003C82 RID: 15490 RVA: 0x00021799 File Offset: 0x0001F999
	// (set) Token: 0x06003C83 RID: 15491 RVA: 0x000217A1 File Offset: 0x0001F9A1
	public bool ScaleWithOwner
	{
		get
		{
			return this.m_scaleWithOwner;
		}
		set
		{
			this.m_scaleWithOwner = value;
		}
	}

	// Token: 0x17001630 RID: 5680
	// (get) Token: 0x06003C84 RID: 15492 RVA: 0x000217AA File Offset: 0x0001F9AA
	// (set) Token: 0x06003C85 RID: 15493 RVA: 0x000217B2 File Offset: 0x0001F9B2
	public bool CanBeRicocheted
	{
		get
		{
			return this.m_canBeRicocheted;
		}
		set
		{
			this.m_canBeRicocheted = value;
		}
	}

	// Token: 0x17001631 RID: 5681
	// (get) Token: 0x06003C86 RID: 15494 RVA: 0x000217BB File Offset: 0x0001F9BB
	// (set) Token: 0x06003C87 RID: 15495 RVA: 0x000217C3 File Offset: 0x0001F9C3
	public bool RicochetsOwnerWhenHits
	{
		get
		{
			return this.m_ricochetsOwnerWhenHits;
		}
		set
		{
			this.m_ricochetsOwnerWhenHits = value;
		}
	}

	// Token: 0x17001632 RID: 5682
	// (get) Token: 0x06003C88 RID: 15496 RVA: 0x000217CC File Offset: 0x0001F9CC
	public GameObject Pivot
	{
		get
		{
			return this.m_pivot;
		}
	}

	// Token: 0x17001633 RID: 5683
	// (get) Token: 0x06003C89 RID: 15497 RVA: 0x000217D4 File Offset: 0x0001F9D4
	// (set) Token: 0x06003C8A RID: 15498 RVA: 0x000217DC File Offset: 0x0001F9DC
	public bool PivotFollowsOrientation
	{
		get
		{
			return this.m_pivotFollowsOrientation;
		}
		set
		{
			this.m_pivotFollowsOrientation = value;
		}
	}

	// Token: 0x17001634 RID: 5684
	// (get) Token: 0x06003C8B RID: 15499 RVA: 0x000217E5 File Offset: 0x0001F9E5
	// (set) Token: 0x06003C8C RID: 15500 RVA: 0x000217ED File Offset: 0x0001F9ED
	public virtual bool IgnoreDamageScale
	{
		get
		{
			return this.m_ignoreDamageScale;
		}
		set
		{
			this.m_ignoreDamageScale = value;
		}
	}

	// Token: 0x17001635 RID: 5685
	// (get) Token: 0x06003C8D RID: 15501 RVA: 0x000217F6 File Offset: 0x0001F9F6
	public virtual DamageType DamageType
	{
		get
		{
			return this.m_projectileData.DamageType;
		}
	}

	// Token: 0x17001636 RID: 5686
	// (get) Token: 0x06003C8E RID: 15502 RVA: 0x00021803 File Offset: 0x0001FA03
	// (set) Token: 0x06003C8F RID: 15503 RVA: 0x0002180B File Offset: 0x0001FA0B
	public virtual float StrengthScale { get; set; }

	// Token: 0x17001637 RID: 5687
	// (get) Token: 0x06003C90 RID: 15504 RVA: 0x00021814 File Offset: 0x0001FA14
	// (set) Token: 0x06003C91 RID: 15505 RVA: 0x0002181C File Offset: 0x0001FA1C
	public virtual float MagicScale { get; set; }

	// Token: 0x17001638 RID: 5688
	// (get) Token: 0x06003C92 RID: 15506 RVA: 0x00021825 File Offset: 0x0001FA25
	public virtual float CooldownReductionPerHit
	{
		get
		{
			return this.m_projectileData.CooldownReductionPerHit;
		}
	}

	// Token: 0x17001639 RID: 5689
	// (get) Token: 0x06003C93 RID: 15507 RVA: 0x00021832 File Offset: 0x0001FA32
	public virtual float ManaGainPerHit
	{
		get
		{
			return this.m_projectileData.ManaGainPerHit;
		}
	}

	// Token: 0x1700163A RID: 5690
	// (get) Token: 0x06003C94 RID: 15508 RVA: 0x0002183F File Offset: 0x0001FA3F
	// (set) Token: 0x06003C95 RID: 15509 RVA: 0x00021847 File Offset: 0x0001FA47
	public virtual float BaseStunStrength { get; set; }

	// Token: 0x1700163B RID: 5691
	// (get) Token: 0x06003C96 RID: 15510 RVA: 0x00021850 File Offset: 0x0001FA50
	public virtual float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x1700163C RID: 5692
	// (get) Token: 0x06003C97 RID: 15511 RVA: 0x00021858 File Offset: 0x0001FA58
	// (set) Token: 0x06003C98 RID: 15512 RVA: 0x00021860 File Offset: 0x0001FA60
	public virtual float BaseKnockbackStrength { get; set; }

	// Token: 0x1700163D RID: 5693
	// (get) Token: 0x06003C99 RID: 15513 RVA: 0x00021869 File Offset: 0x0001FA69
	public virtual float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x1700163E RID: 5694
	// (get) Token: 0x06003C9A RID: 15514 RVA: 0x00021871 File Offset: 0x0001FA71
	// (set) Token: 0x06003C9B RID: 15515 RVA: 0x00021879 File Offset: 0x0001FA79
	public virtual Vector2 ExternalKnockbackMod { get; set; }

	// Token: 0x1700163F RID: 5695
	// (get) Token: 0x06003C9C RID: 15516 RVA: 0x00021882 File Offset: 0x0001FA82
	// (set) Token: 0x06003C9D RID: 15517 RVA: 0x0002188A File Offset: 0x0001FA8A
	public virtual float Speed { get; set; }

	// Token: 0x17001640 RID: 5696
	// (get) Token: 0x06003C9E RID: 15518 RVA: 0x00021893 File Offset: 0x0001FA93
	// (set) Token: 0x06003C9F RID: 15519 RVA: 0x0002189B File Offset: 0x0001FA9B
	public virtual float TurnSpeed { get; set; }

	// Token: 0x17001641 RID: 5697
	// (get) Token: 0x06003CA0 RID: 15520 RVA: 0x000218A4 File Offset: 0x0001FAA4
	// (set) Token: 0x06003CA1 RID: 15521 RVA: 0x000218AC File Offset: 0x0001FAAC
	public virtual float RotationSpeed
	{
		get
		{
			return this.m_visualRotationSpeed;
		}
		set
		{
			this.m_visualRotationSpeed = value;
		}
	}

	// Token: 0x17001642 RID: 5698
	// (get) Token: 0x06003CA2 RID: 15522 RVA: 0x000218B5 File Offset: 0x0001FAB5
	// (set) Token: 0x06003CA3 RID: 15523 RVA: 0x000218BD File Offset: 0x0001FABD
	public virtual bool IsWeighted
	{
		get
		{
			return this.m_isWeighted;
		}
		set
		{
			this.m_isWeighted = value;
		}
	}

	// Token: 0x17001643 RID: 5699
	// (get) Token: 0x06003CA4 RID: 15524 RVA: 0x000218C6 File Offset: 0x0001FAC6
	// (set) Token: 0x06003CA5 RID: 15525 RVA: 0x000218CE File Offset: 0x0001FACE
	public virtual bool SnapToOwner
	{
		get
		{
			return this.m_snapToOwner;
		}
		set
		{
			this.m_snapToOwner = value;
		}
	}

	// Token: 0x17001644 RID: 5700
	// (get) Token: 0x06003CA6 RID: 15526 RVA: 0x000218D7 File Offset: 0x0001FAD7
	// (set) Token: 0x06003CA7 RID: 15527 RVA: 0x000218DF File Offset: 0x0001FADF
	public virtual bool CanHitOwner
	{
		get
		{
			return this.m_canHitOwner;
		}
		set
		{
			this.m_canHitOwner = value;
		}
	}

	// Token: 0x17001645 RID: 5701
	// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x000218E8 File Offset: 0x0001FAE8
	// (set) Token: 0x06003CA9 RID: 15529 RVA: 0x000218F0 File Offset: 0x0001FAF0
	public virtual bool CanHitWall
	{
		get
		{
			return this.m_canHitWall;
		}
		set
		{
			this.m_canHitWall = value;
			this.SetCanHitWall(value);
		}
	}

	// Token: 0x17001646 RID: 5702
	// (get) Token: 0x06003CAA RID: 15530 RVA: 0x00021900 File Offset: 0x0001FB00
	// (set) Token: 0x06003CAB RID: 15531 RVA: 0x00021908 File Offset: 0x0001FB08
	public bool DieOnRoomBoundsCollision
	{
		get
		{
			return this.m_dieOnRoomBoundsCollision;
		}
		set
		{
			this.m_dieOnRoomBoundsCollision = value;
		}
	}

	// Token: 0x17001647 RID: 5703
	// (get) Token: 0x06003CAC RID: 15532 RVA: 0x00021911 File Offset: 0x0001FB11
	// (set) Token: 0x06003CAD RID: 15533 RVA: 0x00021919 File Offset: 0x0001FB19
	public virtual bool CanHitBreakables
	{
		get
		{
			return this.m_canHitBreakables;
		}
		set
		{
			this.m_canHitBreakables = value;
		}
	}

	// Token: 0x17001648 RID: 5704
	// (get) Token: 0x06003CAE RID: 15534 RVA: 0x00021922 File Offset: 0x0001FB22
	// (set) Token: 0x06003CAF RID: 15535 RVA: 0x0002192A File Offset: 0x0001FB2A
	public virtual bool HitsFlimsyBreakablesOnly
	{
		get
		{
			return this.m_hitsFlimsyBreakablesOnly;
		}
		set
		{
			this.m_hitsFlimsyBreakablesOnly = value;
		}
	}

	// Token: 0x17001649 RID: 5705
	// (get) Token: 0x06003CB0 RID: 15536 RVA: 0x00021933 File Offset: 0x0001FB33
	// (set) Token: 0x06003CB1 RID: 15537 RVA: 0x00021945 File Offset: 0x0001FB45
	public virtual bool DieOnWallCollision
	{
		get
		{
			return this.m_dieOnWallCollision && this.m_canHitWall;
		}
		set
		{
			this.m_dieOnWallCollision = value;
		}
	}

	// Token: 0x1700164A RID: 5706
	// (get) Token: 0x06003CB2 RID: 15538 RVA: 0x0002194E File Offset: 0x0001FB4E
	// (set) Token: 0x06003CB3 RID: 15539 RVA: 0x00021956 File Offset: 0x0001FB56
	public virtual bool DieOnCharacterCollision
	{
		get
		{
			return this.m_dieOnCharacterCollision;
		}
		set
		{
			this.m_dieOnCharacterCollision = value;
		}
	}

	// Token: 0x1700164B RID: 5707
	// (get) Token: 0x06003CB4 RID: 15540 RVA: 0x0002195F File Offset: 0x0001FB5F
	public virtual float RepeatHitDuration
	{
		get
		{
			return this.m_projectileData.RepeatHitCheckTimer;
		}
	}

	// Token: 0x1700164C RID: 5708
	// (get) Token: 0x06003CB5 RID: 15541 RVA: 0x0002196C File Offset: 0x0001FB6C
	public virtual float Lifespan
	{
		get
		{
			return this.m_projectileData.LifeSpan;
		}
	}

	// Token: 0x1700164D RID: 5709
	// (get) Token: 0x06003CB6 RID: 15542 RVA: 0x00021979 File Offset: 0x0001FB79
	// (set) Token: 0x06003CB7 RID: 15543 RVA: 0x00021981 File Offset: 0x0001FB81
	public bool UseOwnerCollisionPoint
	{
		get
		{
			return this.m_useOwnerCollisionPoint;
		}
		set
		{
			this.m_useOwnerCollisionPoint = value;
		}
	}

	// Token: 0x1700164E RID: 5710
	// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x000F65A4 File Offset: 0x000F47A4
	public bool IsFlipped
	{
		get
		{
			if (!this.SnapToOwner || !this.Owner)
			{
				return base.transform.localScale.x < 0f;
			}
			if (this.OwnerController)
			{
				return !this.OwnerController.IsFacingRight;
			}
			return this.Owner.transform.localScale.x < 0f;
		}
	}

	// Token: 0x1700164F RID: 5711
	// (get) Token: 0x06003CB9 RID: 15545 RVA: 0x0002198A File Offset: 0x0001FB8A
	// (set) Token: 0x06003CBA RID: 15546 RVA: 0x00021992 File Offset: 0x0001FB92
	public Vector2 Acceleration
	{
		get
		{
			return this.m_acceleration;
		}
		set
		{
			this.m_acceleration = value;
		}
	}

	// Token: 0x17001650 RID: 5712
	// (get) Token: 0x06003CBB RID: 15547 RVA: 0x0002199B File Offset: 0x0001FB9B
	// (set) Token: 0x06003CBC RID: 15548 RVA: 0x000219A8 File Offset: 0x0001FBA8
	public float AccelerationX
	{
		get
		{
			return this.m_acceleration.x;
		}
		set
		{
			this.m_acceleration.x = value;
		}
	}

	// Token: 0x17001651 RID: 5713
	// (get) Token: 0x06003CBD RID: 15549 RVA: 0x000219B6 File Offset: 0x0001FBB6
	// (set) Token: 0x06003CBE RID: 15550 RVA: 0x000219C3 File Offset: 0x0001FBC3
	public float AccelerationY
	{
		get
		{
			return this.m_acceleration.y;
		}
		set
		{
			this.m_acceleration.y = value;
		}
	}

	// Token: 0x17001652 RID: 5714
	// (get) Token: 0x06003CBF RID: 15551 RVA: 0x000219D1 File Offset: 0x0001FBD1
	// (set) Token: 0x06003CC0 RID: 15552 RVA: 0x000F6618 File Offset: 0x000F4818
	public Vector2 Heading
	{
		get
		{
			return this.m_heading;
		}
		set
		{
			if (this.m_heading != value)
			{
				this.m_heading = value;
				this.m_heading.Normalize();
				this.m_orientation = Mathf.Atan2(this.m_heading.y, this.m_heading.x);
			}
		}
	}

	// Token: 0x17001653 RID: 5715
	// (get) Token: 0x06003CC1 RID: 15553 RVA: 0x000219D9 File Offset: 0x0001FBD9
	// (set) Token: 0x06003CC2 RID: 15554 RVA: 0x000F6668 File Offset: 0x000F4868
	public float HeadingX
	{
		get
		{
			return this.m_heading.x;
		}
		set
		{
			if (this.m_heading.x != value)
			{
				this.m_heading.x = value;
				this.m_heading.Normalize();
				this.m_orientation = Mathf.Atan2(this.m_heading.y, this.m_heading.x);
			}
		}
	}

	// Token: 0x17001654 RID: 5716
	// (get) Token: 0x06003CC3 RID: 15555 RVA: 0x000219E6 File Offset: 0x0001FBE6
	// (set) Token: 0x06003CC4 RID: 15556 RVA: 0x000F66BC File Offset: 0x000F48BC
	public float HeadingY
	{
		get
		{
			return this.m_heading.y;
		}
		set
		{
			if (this.m_heading.y != value)
			{
				this.m_heading.y = value;
				this.m_heading.Normalize();
				this.m_orientation = Mathf.Atan2(this.m_heading.y, this.m_heading.x);
			}
		}
	}

	// Token: 0x17001655 RID: 5717
	// (get) Token: 0x06003CC5 RID: 15557 RVA: 0x000219F3 File Offset: 0x0001FBF3
	// (set) Token: 0x06003CC6 RID: 15558 RVA: 0x000219FB File Offset: 0x0001FBFB
	public float Orientation
	{
		get
		{
			return this.m_orientation;
		}
		set
		{
			if (this.m_orientation != value)
			{
				this.m_orientation = value;
				this.m_heading.x = Mathf.Cos(this.m_orientation);
				this.m_heading.y = Mathf.Sin(this.m_orientation);
			}
		}
	}

	// Token: 0x17001656 RID: 5718
	// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x00021A39 File Offset: 0x0001FC39
	// (set) Token: 0x06003CC8 RID: 15560 RVA: 0x00021A41 File Offset: 0x0001FC41
	public GameObject Owner
	{
		get
		{
			return this.m_owner;
		}
		set
		{
			this.m_owner = value;
			if (this.m_owner)
			{
				this.m_ownerController = this.m_owner.GetComponent<BaseCharacterController>();
				return;
			}
			this.m_ownerController = null;
		}
	}

	// Token: 0x17001657 RID: 5719
	// (get) Token: 0x06003CC9 RID: 15561 RVA: 0x00021A70 File Offset: 0x0001FC70
	public BaseCharacterController OwnerController
	{
		get
		{
			return this.m_ownerController;
		}
	}

	// Token: 0x17001658 RID: 5720
	// (get) Token: 0x06003CCA RID: 15562 RVA: 0x00021A78 File Offset: 0x0001FC78
	public float BaseDamage
	{
		get
		{
			if (this.IgnoreDamageScale)
			{
				return this.Strength + this.Magic;
			}
			return this.Strength * this.StrengthScale + this.Magic * this.MagicScale;
		}
	}

	// Token: 0x17001659 RID: 5721
	// (get) Token: 0x06003CCB RID: 15563 RVA: 0x00021AAB File Offset: 0x0001FCAB
	public virtual float ActualDamage
	{
		get
		{
			if (this.m_dealsHazardDamage)
			{
				return Hazard_EV.GetDamageAmount(PlayerManager.GetCurrentPlayerRoom()) * this.ProjectileData.StrengthScale;
			}
			return this.BaseDamage;
		}
	}

	// Token: 0x1700165A RID: 5722
	// (get) Token: 0x06003CCC RID: 15564 RVA: 0x00021AD2 File Offset: 0x0001FCD2
	// (set) Token: 0x06003CCD RID: 15565 RVA: 0x00021ADA File Offset: 0x0001FCDA
	public virtual float DamageMod { get; set; }

	// Token: 0x1700165B RID: 5723
	// (get) Token: 0x06003CCE RID: 15566 RVA: 0x00021AE3 File Offset: 0x0001FCE3
	// (set) Token: 0x06003CCF RID: 15567 RVA: 0x00021AEB File Offset: 0x0001FCEB
	public virtual float Magic { get; set; }

	// Token: 0x1700165C RID: 5724
	// (get) Token: 0x06003CD0 RID: 15568 RVA: 0x00021AF4 File Offset: 0x0001FCF4
	// (set) Token: 0x06003CD1 RID: 15569 RVA: 0x00021AFC File Offset: 0x0001FCFC
	public virtual float Strength { get; set; }

	// Token: 0x1700165D RID: 5725
	// (get) Token: 0x06003CD2 RID: 15570 RVA: 0x00021B05 File Offset: 0x0001FD05
	// (set) Token: 0x06003CD3 RID: 15571 RVA: 0x00021B0D File Offset: 0x0001FD0D
	public virtual float ActualCritChance { get; set; }

	// Token: 0x1700165E RID: 5726
	// (get) Token: 0x06003CD4 RID: 15572 RVA: 0x00021B16 File Offset: 0x0001FD16
	// (set) Token: 0x06003CD5 RID: 15573 RVA: 0x00021B1E File Offset: 0x0001FD1E
	public virtual float ActualCritDamage { get; set; }

	// Token: 0x1700165F RID: 5727
	// (get) Token: 0x06003CD6 RID: 15574 RVA: 0x00021B27 File Offset: 0x0001FD27
	// (set) Token: 0x06003CD7 RID: 15575 RVA: 0x00021B2F File Offset: 0x0001FD2F
	public bool DontFireDeathRelay { get; set; }

	// Token: 0x17001660 RID: 5728
	// (get) Token: 0x06003CD8 RID: 15576 RVA: 0x00021B38 File Offset: 0x0001FD38
	// (set) Token: 0x06003CD9 RID: 15577 RVA: 0x00021B40 File Offset: 0x0001FD40
	public string RelicDamageTypeString { get; set; }

	// Token: 0x06003CDA RID: 15578 RVA: 0x00021B49 File Offset: 0x0001FD49
	private void OnEnable()
	{
		ProjectileManager.ActiveProjectileCount++;
	}

	// Token: 0x06003CDB RID: 15579 RVA: 0x000F6710 File Offset: 0x000F4910
	private void OnDisable()
	{
		foreach (BaseProjectileLogic baseProjectileLogic in this.m_projectileLogicArray)
		{
			baseProjectileLogic.enabled = false;
			baseProjectileLogic.StopAllCoroutines();
		}
		ProjectileManager.ActiveProjectileCount--;
		if (this.m_wasSpawned)
		{
			DisablePooledObjectManager.DisablePooledObject(this, false);
		}
		if (this.Animator)
		{
			this.Animator.WriteDefaultValues();
		}
		this.StopChangeCollisionPointCoroutine();
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x000F677C File Offset: 0x000F497C
	private void Awake()
	{
		if (Application.isPlaying)
		{
			this.Initialize();
			PointLightController componentInChildren = base.GetComponentInChildren<PointLightController>();
			if (componentInChildren)
			{
				componentInChildren.SetLocation(PointLightLocation.Front);
				componentInChildren.UpdateLocation(CameraLayer.Foreground_ORTHO);
			}
			this.IsAwakeCalled = true;
		}
		this.m_detachOnDestruction = (this.m_hasAnimStates && !string.IsNullOrEmpty(this.m_outtroTriggerName));
	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x000F67DC File Offset: 0x000F49DC
	private void Start()
	{
		this.m_player = PlayerManager.GetPlayerController();
		bool disableAllCollisions = this.m_hbController.DisableAllCollisions;
		bool hitboxActiveState = this.m_hbController.GetHitboxActiveState(HitboxType.Weapon);
		bool hitboxActiveState2 = this.m_hbController.GetHitboxActiveState(HitboxType.Body);
		bool hitboxActiveState3 = this.m_hbController.GetHitboxActiveState(HitboxType.Terrain);
		bool hitboxActiveState4 = this.m_hbController.GetHitboxActiveState(HitboxType.Platform);
		CameraLayerController component = base.GetComponent<CameraLayerController>();
		component.SetCameraLayer(CameraLayer.Foreground_ORTHO);
		int subLayer = ProjectileSubLayerManager.GetSubLayer(base.name);
		component.SetSubLayer(subLayer, false);
		this.m_hbController.DisableAllCollisions = disableAllCollisions;
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, hitboxActiveState);
		this.m_hbController.SetHitboxActiveState(HitboxType.Body, hitboxActiveState2);
		this.m_hbController.SetHitboxActiveState(HitboxType.Terrain, hitboxActiveState3);
		this.m_hbController.SetHitboxActiveState(HitboxType.Platform, hitboxActiveState4);
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x000F689C File Offset: 0x000F4A9C
	private void Initialize()
	{
		if (!this.m_ignoreTrailRendererYScaleMatching)
		{
			TrailRenderer[] componentsInChildren = base.GetComponentsInChildren<TrailRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].widthMultiplier = base.transform.localScale.y;
			}
		}
		this.m_projectileLogicArray = base.GetComponents<BaseProjectileLogic>();
		BaseProjectileLogic[] projectileLogicArray = this.m_projectileLogicArray;
		for (int i = 0; i < projectileLogicArray.Length; i++)
		{
			projectileLogicArray[i].enabled = false;
		}
		this.m_initialScale = base.transform.localScale;
		this.m_initialScale.z = Mathf.Clamp(this.m_initialScale.z, 0f, 1f);
		this.m_initialRotationSpeed = this.RotationSpeed;
		if (this.Pivot)
		{
			this.m_initialRotation = this.Pivot.transform.localEulerAngles;
		}
		else
		{
			this.m_initialRotation = base.transform.localEulerAngles;
		}
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
		this.m_hbController.RepeatHitDuration = this.RepeatHitDuration;
		if (!this.m_hbController.IsInitialized)
		{
			this.m_hbController.Initialize();
		}
		this.InitializeCollisionSettings();
		this.m_animator = base.GetComponentInChildren<Animator>();
		if (this.m_animator)
		{
			this.m_hasVelocityParams = (global::AnimatorUtility.HasParameter(this.m_animator, "xVelocity") && global::AnimatorUtility.HasParameter(this.m_animator, "yVelocity"));
		}
		this.m_corgiController = base.GetComponent<CorgiController_RL>();
		if (this.m_corgiController)
		{
			this.m_corgiController.GravityActive(false);
		}
		this.ResetValues();
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x000F6A2C File Offset: 0x000F4C2C
	private void InitializeCollisionSettings()
	{
		this.SetCanHitWall(this.CanHitWall);
		CollisionType collisionType = this.m_hbController.WeaponCollidesWithType;
		if (this.CanHitBreakables)
		{
			if (!this.HitsFlimsyBreakablesOnly)
			{
				collisionType |= CollisionType.Breakable;
			}
			collisionType |= CollisionType.FlimsyBreakable;
		}
		else
		{
			collisionType &= ~CollisionType.Breakable;
			collisionType &= ~CollisionType.FlimsyBreakable;
		}
		if (!base.CompareTag("PlayerProjectile"))
		{
			collisionType |= CollisionType.Player_Dodging;
			this.m_voidDissolve = base.GetComponent<VoidDissolveComponent>();
		}
		this.m_hbController.ChangeCanCollideWith(HitboxType.Weapon, collisionType);
		if (this.CanBeRicocheted && this.CanHitWall)
		{
			if (this.m_ricochetPlayer == null)
			{
				this.m_ricochetPlayer = new Action<Projectile_RL, GameObject>(this.RicochetPlayer);
			}
			this.OnCollisionRelay.AddListener(this.m_ricochetPlayer, false);
		}
	}

	// Token: 0x06003CE0 RID: 15584 RVA: 0x000F6AE8 File Offset: 0x000F4CE8
	private void SetCanHitWall(bool value)
	{
		if (this.m_hbController != null)
		{
			Collider2D collider = this.m_hbController.GetCollider(HitboxType.Terrain);
			if (collider)
			{
				if (value)
				{
					collider.gameObject.layer = 18;
					return;
				}
				collider.gameObject.layer = 13;
			}
		}
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x000F6B30 File Offset: 0x000F4D30
	private void OnDestroy()
	{
		foreach (BaseProjectileLogic baseProjectileLogic in this.m_projectileLogicArray)
		{
			baseProjectileLogic.enabled = false;
			baseProjectileLogic.StopAllCoroutines();
		}
		this.m_preOnDisableRelay.Dispatch(this);
		this.OnCollisionRelay.RemoveListener(this.m_ricochetPlayer);
	}

	// Token: 0x06003CE2 RID: 15586 RVA: 0x000F6B80 File Offset: 0x000F4D80
	protected virtual void FixedUpdate()
	{
		if (!this.m_isDying)
		{
			this.UpdateDeathState();
		}
		if (this.m_lifeSpanTimer > 0f && !this.m_isDying)
		{
			this.m_lifeSpanTimer -= Time.deltaTime;
			if (this.m_lifeSpanTimer <= 0f)
			{
				this.Death(null);
			}
		}
		this.UpdateMovement();
		if (this.CanHitWall && this.DieOnWallCollision && this.DieOnRoomBoundsCollision && !PlayerManager.GetCurrentPlayerRoom().Bounds.Contains(base.transform.position))
		{
			this.FlagForDestruction(null);
		}
		if (this.m_hasVelocityParams)
		{
			Vector2 vector = this.Heading * this.Speed;
			if (!this.IsFlipped)
			{
				this.m_animator.SetFloat("xVelocity", vector.x);
			}
			else
			{
				this.m_animator.SetFloat("xVelocity", -vector.x);
			}
			this.m_animator.SetFloat("yVelocity", vector.y);
		}
	}

	// Token: 0x06003CE3 RID: 15587 RVA: 0x00021B57 File Offset: 0x0001FD57
	protected virtual bool UpdateDeathState()
	{
		if (this.m_flagToDestroy)
		{
			this.m_flagToDestroy = false;
			this.Death(this.m_collidedObj);
			return true;
		}
		return false;
	}

	// Token: 0x06003CE4 RID: 15588 RVA: 0x00021B77 File Offset: 0x0001FD77
	public virtual void SetCorgiVelocity(Vector2 velocity)
	{
		if (this.m_corgiController)
		{
			this.m_corgiController.SetForce(velocity);
		}
	}

	// Token: 0x06003CE5 RID: 15589 RVA: 0x00021B92 File Offset: 0x0001FD92
	public virtual void SetCorgiVelocity(float x, float y)
	{
		if (this.m_corgiController)
		{
			this.m_corgiController.SetHorizontalForce(x);
			this.m_corgiController.SetHorizontalForce(y);
		}
	}

	// Token: 0x06003CE6 RID: 15590 RVA: 0x000F6C84 File Offset: 0x000F4E84
	public virtual void UpdateMovement()
	{
		if (this.TurnSpeed > 0f && this.m_player)
		{
			this.Orientation = CDGHelper.TurnToFaceRadians(this.Midpoint, this.m_player.Midpoint, CDGHelper.ToRadians(this.TurnSpeed), this.Orientation, Time.deltaTime, false);
		}
		float radians = this.Orientation;
		if (this.m_corgiController)
		{
			if (this.m_corgiController.IsInitialized)
			{
				if (this.m_gravityKickInTimer <= 0f && !this.m_corgiController.IsGravityActive && this.IsWeighted)
				{
					this.m_corgiController.GravityActive(true);
				}
				if (this.m_corgiController.Velocity.x != 0f && this.m_corgiController.State.IsGrounded && !this.m_disableFriction)
				{
					this.m_corgiController.SetHorizontalForce(0f);
				}
				this.m_movement = this.m_corgiController.Velocity;
			}
		}
		else
		{
			this.m_movement = this.Heading * (this.Speed * Time.deltaTime);
			if (this.m_gravityKickInTimer <= 0f && this.IsWeighted)
			{
				float num = -50f;
				if (-this.AccelerationY * Time.deltaTime < this.m_movement.y)
				{
					num *= 1f * this.m_ascentMultiplierOverride;
				}
				else
				{
					num = num * 1f * this.m_fallMultiplierOverride;
				}
				float terminalVelocity = Global_EV.TerminalVelocity;
				this.AccelerationY += num * Time.deltaTime;
				this.m_movement.y = this.m_movement.y + this.AccelerationY * Time.deltaTime;
				if (this.m_movement.y < terminalVelocity)
				{
					this.m_movement.y = terminalVelocity;
				}
				radians = Mathf.Atan2(this.m_movement.y, this.m_movement.x);
			}
			base.transform.Translate(this.m_movement, Space.Self);
		}
		if (this.m_gravityKickInTimer > 0f)
		{
			this.m_gravityKickInTimer -= Time.deltaTime;
		}
		if (this.Pivot)
		{
			if (!this.PivotFollowsOrientation && this.RotationSpeed != 0f)
			{
				Vector3 localEulerAngles = this.Pivot.transform.localEulerAngles;
				localEulerAngles.z += this.RotationSpeed * Time.deltaTime;
				this.Pivot.transform.localEulerAngles = localEulerAngles;
				return;
			}
			if (this.PivotFollowsOrientation)
			{
				Vector3 localEulerAngles2 = this.Pivot.transform.localEulerAngles;
				float num2 = CDGHelper.ToDegrees(CDGHelper.WrapAngleRadians(radians, false));
				if (this.MatchFacing)
				{
					if (num2 > 90f && num2 < 270f)
					{
						if (!this.IsFlipped)
						{
							this.Flip();
						}
						num2 = (num2 - 180f) * -1f;
					}
					else if (this.IsFlipped)
					{
						this.Flip();
					}
				}
				localEulerAngles2.z = num2;
				this.Pivot.transform.localEulerAngles = localEulerAngles2;
			}
		}
	}

	// Token: 0x06003CE7 RID: 15591 RVA: 0x000F6F9C File Offset: 0x000F519C
	public virtual void Flip()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.x = -localScale.x;
		base.transform.localScale = localScale;
	}

	// Token: 0x06003CE8 RID: 15592 RVA: 0x000F6FD0 File Offset: 0x000F51D0
	public virtual void ResetValues()
	{
		this.Owner = null;
		this.RelicDamageTypeString = null;
		this.Strength = 1f;
		this.Magic = 1f;
		this.ActualCritChance = 0f;
		this.ActualCritDamage = 0f;
		this.DamageMod = 0f;
		this.m_wasSpawned = false;
		this.m_isDying = false;
		this.MatchFacing = false;
		this.BaseKnockbackStrength = this.m_projectileData.KnockbackStrength;
		this.BaseStunStrength = this.m_projectileData.StunStrength;
		this.Speed = this.m_projectileData.Speed;
		this.TurnSpeed = this.m_projectileData.TurnSpeed;
		this.ExternalKnockbackMod = new Vector2(this.m_projectileData.KnockbackModX, this.m_projectileData.KnockbackModY);
		this.m_flagToDestroy = false;
		this.m_collidedObj = null;
		this.m_destructionType = Projectile_RL.ProjectileDestructionType.None;
		base.transform.localScale = this.m_initialScale;
		this.m_lifeSpanTimer = this.Lifespan;
		this.m_gravityKickInTimer = this.m_gravityKickInDelay;
		this.Acceleration = Vector2.zero;
		this.Heading = Vector2.zero;
		this.RotationSpeed = this.m_initialRotationSpeed;
		if (this.Pivot)
		{
			this.Pivot.transform.localEulerAngles = this.m_initialRotation;
			if (this.SnapToOwner)
			{
				base.transform.localEulerAngles = Vector3.zero;
			}
		}
		else
		{
			base.transform.localEulerAngles = this.m_initialRotation;
		}
		base.StopAllCoroutines();
		this.IgnoreDamageScale = false;
		if (this.m_hbController != null)
		{
			this.m_hbController.DisableAllCollisions = false;
		}
		if (this.m_corgiController && this.m_corgiController.IsInitialized)
		{
			this.m_corgiController.ResetState();
		}
		this.RemoveAllStatusEffects();
		if (this.m_corgiController)
		{
			this.m_corgiController.State.IsCollidingBelow = false;
		}
		this.ResetSpriteRendererColor();
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x000F71BC File Offset: 0x000F53BC
	public void RemoveAllStatusEffects()
	{
		if (this.m_statusEffectTypeArray != null && this.m_statusEffectDurationArray != null)
		{
			for (int i = 0; i < 5; i++)
			{
				this.m_statusEffectTypeArray[i] = StatusEffectType.None;
				this.m_statusEffectDurationArray[i] = 0f;
			}
		}
	}

	// Token: 0x06003CEA RID: 15594 RVA: 0x000F71FC File Offset: 0x000F53FC
	public void Spawn()
	{
		this.m_wasSpawned = true;
		this.m_onSpawnRelay.Dispatch(this, null);
		this.m_onSpawnEffectTriggerRelay.Dispatch(base.gameObject);
		BaseProjectileLogic[] projectileLogicArray = this.m_projectileLogicArray;
		for (int i = 0; i < projectileLogicArray.Length; i++)
		{
			projectileLogicArray[i].enabled = true;
		}
		if (!base.CompareTag("PlayerProjectile") && !this.m_disableOffscreenWarnings)
		{
			ProjectileManager.AttachOffscreenIcon(this, false);
		}
		base.StartCoroutine(this.SpawnProjectileCoroutine());
		this.StopChangeCollisionPointCoroutine();
		if (!this.UseOwnerCollisionPoint)
		{
			this.m_changeCollPointCoroutine = base.StartCoroutine(this.ChangeCollisionPointCoroutine());
		}
	}

	// Token: 0x06003CEB RID: 15595 RVA: 0x00021BB9 File Offset: 0x0001FDB9
	private IEnumerator SpawnProjectileCoroutine()
	{
		if (this.m_hasAnimStates && this.m_hasIntro)
		{
			this.m_hbController.DisableAllCollisions = true;
			int currentAnimatorHash = this.m_animator.GetCurrentAnimatorStateInfo(this.m_animLayer).shortNameHash;
			while (this.m_animator.GetCurrentAnimatorStateInfo(this.m_animLayer).shortNameHash == currentAnimatorHash)
			{
				yield return null;
			}
			this.m_hbController.DisableAllCollisions = false;
		}
		yield break;
	}

	// Token: 0x06003CEC RID: 15596 RVA: 0x00021BC8 File Offset: 0x0001FDC8
	private IEnumerator ChangeCollisionPointCoroutine()
	{
		this.m_collisionPointChanged = true;
		this.UseOwnerCollisionPoint = true;
		yield return this.m_fixedUpdateYield;
		this.m_collisionPointChanged = false;
		this.UseOwnerCollisionPoint = false;
		yield break;
	}

	// Token: 0x06003CED RID: 15597 RVA: 0x00021BD7 File Offset: 0x0001FDD7
	public void StopChangeCollisionPointCoroutine()
	{
		if (this.m_collisionPointChanged)
		{
			if (this.m_changeCollPointCoroutine != null)
			{
				base.StopCoroutine(this.m_changeCollPointCoroutine);
			}
			this.m_collisionPointChanged = false;
			this.UseOwnerCollisionPoint = false;
		}
	}

	// Token: 0x06003CEE RID: 15598 RVA: 0x000F7298 File Offset: 0x000F5498
	protected virtual void Death(GameObject attacker)
	{
		this.m_isDying = true;
		if (!this.DontFireDeathRelay)
		{
			this.m_onDeathRelay.Dispatch(this, attacker);
		}
		if (this.m_lifeSpanTimer <= 0f)
		{
			this.m_onTimeoutEffectTriggerRelay.Dispatch(base.gameObject);
		}
		else if (attacker)
		{
			this.m_onDeathEffectTriggerRelay.Dispatch(attacker);
		}
		else
		{
			this.m_onDeathEffectTriggerRelay.Dispatch(base.gameObject);
		}
		if (this.m_hbController != null)
		{
			this.m_hbController.DisableAllCollisions = true;
		}
		base.StartCoroutine(this.DestroyProjectileCoroutine(true));
	}

	// Token: 0x06003CEF RID: 15599 RVA: 0x00021C03 File Offset: 0x0001FE03
	private IEnumerator CanHitSelfCoroutine()
	{
		yield break;
		CollisionType collisionType = this.m_hbController.WeaponCollidesWithType;
		CollisionType collisionType2 = this.OwnerController.HitboxController.CollisionType;
		collisionType |= collisionType2;
		this.m_hbController.ChangeCanCollideWith(HitboxType.Weapon, collisionType);
		yield break;
	}

	// Token: 0x06003CF0 RID: 15600 RVA: 0x00021C12 File Offset: 0x0001FE12
	protected virtual IEnumerator DestroyProjectileCoroutine(bool animate = false)
	{
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.SpellsDamageCloud).Level > 0 && base.CompareTag("PlayerProjectile") && this.CastAbilityType == CastAbilityType.Spell)
		{
			ProjectileManager.FireProjectile(this.Owner, "SuperFartProjectile", base.transform.position, false, 0f, 1f, true, true, true, true);
		}
		if (this.m_destructionType != Projectile_RL.ProjectileDestructionType.VoidDashCollision || (this.m_destructionType == Projectile_RL.ProjectileDestructionType.VoidDashCollision && !this.m_keepMovingWhenDashedThrough))
		{
			this.SetCorgiVelocity(Vector2.zero);
			this.Speed = 0f;
		}
		if (this.m_voidDissolve && ((this.CollisionFlags & ProjectileCollisionFlag.VoidDashable) != ProjectileCollisionFlag.None || (this.CollisionFlags & ProjectileCollisionFlag.WeaponStrikeable) != ProjectileCollisionFlag.None))
		{
			yield return this.m_voidDissolve.DissolveCoroutine(this.m_destructionType == Projectile_RL.ProjectileDestructionType.VoidDashCollision);
		}
		if (this.m_hasAnimStates && !string.IsNullOrEmpty(this.m_outtroTriggerName) && animate && (!this.m_playOuttroOnTimeoutOnly || (this.m_playOuttroOnTimeoutOnly && this.m_lifeSpanTimer <= 0f)))
		{
			if (base.gameObject.transform.parent)
			{
				base.gameObject.transform.SetParent(null);
			}
			int currentAnimatorHash = this.m_animator.GetCurrentAnimatorStateInfo(this.m_animLayer).shortNameHash;
			this.m_animator.SetTrigger(this.m_outtroTriggerName);
			while (this.m_animator.GetCurrentAnimatorStateInfo(this.m_animLayer).shortNameHash == currentAnimatorHash)
			{
				yield return null;
			}
			currentAnimatorHash = this.m_animator.GetCurrentAnimatorStateInfo(this.m_animLayer).shortNameHash;
			for (;;)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(this.m_animLayer);
				if (currentAnimatorStateInfo.shortNameHash != currentAnimatorHash || currentAnimatorStateInfo.normalizedTime >= 1f)
				{
					break;
				}
				yield return null;
			}
		}
		this.m_preOnDisableRelay.Dispatch(this);
		base.gameObject.SetActive(false);
		yield return null;
		yield break;
	}

	// Token: 0x06003CF1 RID: 15601 RVA: 0x00021C28 File Offset: 0x0001FE28
	public void WeaponOnStayHitResponse(IHitboxController otherHBController)
	{
		this.WeaponOnEnterHitResponse(otherHBController);
	}

	// Token: 0x06003CF2 RID: 15602 RVA: 0x000F732C File Offset: 0x000F552C
	public virtual void WeaponOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (CollisionType_RL.IsProjectile(otherHBController.RootGameObject))
		{
			Projectile_RL component = otherHBController.RootGameObject.GetComponent<Projectile_RL>();
			if (!component || !Projectile_RL.CollisionFlagAllowed(this, component))
			{
				return;
			}
		}
		if (this.Owner == otherHBController.RootGameObject && !this.CanHitOwner)
		{
			return;
		}
		this.m_onCollisionRelay.Dispatch(this, otherHBController.RootGameObject);
		if (this.DieOnCharacterCollision)
		{
			bool flag = true;
			if (otherHBController.CollisionType == CollisionType.FlimsyBreakable)
			{
				flag = false;
			}
			if (flag)
			{
				this.m_collidedObj = otherHBController.RootGameObject;
				this.FlagForDestruction(null);
				this.m_destructionType = Projectile_RL.ProjectileDestructionType.CharacterCollision;
				if (otherHBController.RootGameObject.CompareTag("Player") && PlayerManager.GetPlayerController().CharacterDash.IsDashing && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockVoidDash) > 0)
				{
					this.m_destructionType = Projectile_RL.ProjectileDestructionType.VoidDashCollision;
				}
			}
		}
		if (this.Owner && PlayerManager.GetPlayerController())
		{
			GameObject rootGameObject = otherHBController.RootGameObject;
			EnemyController component2 = rootGameObject.GetComponent<EnemyController>();
			if (component2 && (component2.RicochetsAttackerOnHit || this.RicochetsOwnerWhenHits) && this.CanBeRicocheted)
			{
				this.RicochetPlayer(null, rootGameObject);
			}
		}
		this.PerformHitEffectCheck(otherHBController);
	}

	// Token: 0x06003CF3 RID: 15603 RVA: 0x000F7458 File Offset: 0x000F5658
	protected void RicochetPlayer(Projectile_RL projectile, GameObject colliderObj)
	{
		if (!colliderObj.CompareTag("Enemy") && !colliderObj.CompareTag("Platform"))
		{
			return;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CharacterMove.DisableMovement(this.m_ricochetLockoutDuration);
		playerController.KnockedIntoAir = true;
		playerController.StopActiveAbilities(false);
		playerController.MovementState = CharacterStates.MovementStates.Jumping;
		playerController.CharacterJump.ResetBrakeForce();
		playerController.Animator.SetTrigger("AirJump");
		if (colliderObj.CompareTag("Platform"))
		{
			Vector3 vector = this.Midpoint + this.CollisionPointOffset;
			if (this.HitboxController.LastCollidedWith)
			{
				vector = this.HitboxController.LastCollidedWith.ClosestPoint(vector);
			}
			if (vector.x < playerController.Midpoint.x)
			{
				playerController.SetVelocity(this.m_ricochetKnockbackDistance.x, this.m_ricochetKnockbackDistance.y, false);
				return;
			}
			playerController.SetVelocity(-this.m_ricochetKnockbackDistance.x, this.m_ricochetKnockbackDistance.y, false);
			return;
		}
		else
		{
			if (colliderObj.transform.position.x < playerController.Midpoint.x)
			{
				playerController.SetVelocity(this.m_ricochetKnockbackDistance.x, this.m_ricochetKnockbackDistance.y, false);
				return;
			}
			playerController.SetVelocity(-this.m_ricochetKnockbackDistance.x, this.m_ricochetKnockbackDistance.y, false);
			return;
		}
	}

	// Token: 0x06003CF4 RID: 15604 RVA: 0x00021C31 File Offset: 0x0001FE31
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnEnterHitResponse(otherHBController);
	}

	// Token: 0x06003CF5 RID: 15605 RVA: 0x000F75C8 File Offset: 0x000F57C8
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (!this.CanHitWall)
		{
			return;
		}
		if (otherHBController == null)
		{
			this.m_onCollisionRelay.Dispatch(this, this.HitboxController.LastCollidedWith.gameObject);
			this.m_collidedObj = this.HitboxController.LastCollidedWith.gameObject;
		}
		else
		{
			this.m_onCollisionRelay.Dispatch(this, otherHBController.RootGameObject);
			this.m_collidedObj = otherHBController.RootGameObject;
		}
		if (this.DieOnWallCollision)
		{
			this.m_destructionType = Projectile_RL.ProjectileDestructionType.TerrainCollision;
			this.FlagForDestruction(null);
		}
		this.PerformHitEffectCheck(otherHBController);
	}

	// Token: 0x06003CF6 RID: 15606 RVA: 0x00021C3A File Offset: 0x0001FE3A
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x06003CF7 RID: 15607 RVA: 0x000F7650 File Offset: 0x000F5850
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		Projectile_RL projectile_RL = null;
		if (CollisionType_RL.IsProjectile(otherHBController.RootGameObject))
		{
			projectile_RL = otherHBController.RootGameObject.GetComponent<Projectile_RL>();
			if (!projectile_RL || !Projectile_RL.CollisionFlagAllowed(this, projectile_RL))
			{
				return;
			}
		}
		this.m_onCollisionRelay.Dispatch(this, otherHBController.RootGameObject);
		if (this.DieOnCharacterCollision || (projectile_RL && (projectile_RL.CanCollideWithFlags & (ProjectileCollisionFlag.ReflectWeak | ProjectileCollisionFlag.ReflectStrong)) != ProjectileCollisionFlag.None && !base.CompareTag("PlayerProjectile")))
		{
			this.FlagForDestruction(null);
			if (otherHBController != null)
			{
				this.m_collidedObj = otherHBController.RootGameObject;
			}
			else
			{
				this.m_collidedObj = this.HitboxController.LastCollidedWith.gameObject;
			}
			otherHBController.RemoveFromRepeatHitChecks(base.gameObject);
		}
	}

	// Token: 0x06003CF8 RID: 15608 RVA: 0x000F7700 File Offset: 0x000F5900
	public void PerformHitEffectCheck(IHitboxController otherHBController)
	{
		Vector3 vector = this.Midpoint + this.CollisionPointOffset;
		if (this.HitboxController.LastCollidedWith)
		{
			vector = this.HitboxController.LastCollidedWith.ClosestPoint(vector);
		}
		IPlayHitEffect playHitEffect = (otherHBController != null) ? otherHBController.RootGameObject.GetComponent<IPlayHitEffect>() : null;
		bool flag = playHitEffect.IsNativeNull();
		if (otherHBController == null || (!flag && playHitEffect.PlayHitEffect))
		{
			if (!flag)
			{
				EffectManager.PlayHitEffect(this, vector, playHitEffect.EffectNameOverride, this.StrikeType, false);
			}
			else
			{
				EffectManager.PlayHitEffect(this, vector, null, this.StrikeType, false);
			}
		}
		if (!flag && playHitEffect.PlayDirectionalHitEffect)
		{
			EffectManager.PlayDirectionalHitEffect(this, this.HitboxController.RootGameObject, vector);
		}
	}

	// Token: 0x06003CF9 RID: 15609 RVA: 0x000F77C4 File Offset: 0x000F59C4
	public void FlagForDestruction(GameObject collidedObj = null)
	{
		if (base.gameObject.activeSelf)
		{
			this.m_collidedObj = collidedObj;
			this.m_flagToDestroy = true;
			if (this.m_detachOnDestruction && (!this.m_playOuttroOnTimeoutOnly || (this.m_playOuttroOnTimeoutOnly && this.m_lifeSpanTimer <= 0f)) && base.gameObject.transform.parent)
			{
				base.gameObject.transform.SetParent(null);
			}
		}
	}

	// Token: 0x06003CFA RID: 15610 RVA: 0x000F783C File Offset: 0x000F5A3C
	public void AttachStatusEffect(StatusEffectType type, float duration = 0f)
	{
		if (this.m_statusEffectTypeArray == null)
		{
			this.m_statusEffectTypeArray = new StatusEffectType[5];
			this.m_statusEffectDurationArray = new float[5];
			for (int i = 0; i < 5; i++)
			{
				this.m_statusEffectTypeArray[i] = StatusEffectType.None;
				this.m_statusEffectDurationArray[i] = 0f;
			}
		}
		int num = this.m_statusEffectTypeArray.IndexOf(type);
		if (num != -1)
		{
			if (this.m_statusEffectDurationArray[num] < duration)
			{
				this.m_statusEffectDurationArray[num] = duration;
			}
			return;
		}
		int num2 = this.m_statusEffectTypeArray.IndexOf(StatusEffectType.None);
		if (num2 != -1)
		{
			this.m_statusEffectTypeArray[num2] = type;
			this.m_statusEffectDurationArray[num2] = duration;
			return;
		}
		Debug.Log(string.Concat(new string[]
		{
			"<color=yellow>WARNING: Cannot add status effect: ",
			type.ToString(),
			" to projectile. Projectile has reached max limit of ",
			5.ToString(),
			"</color>"
		}));
	}

	// Token: 0x06003CFB RID: 15611 RVA: 0x000F791C File Offset: 0x000F5B1C
	public void RemoveStatusEffect(StatusEffectType statusEffect)
	{
		if (this.m_statusEffectTypeArray == null)
		{
			return;
		}
		int num = this.m_statusEffectTypeArray.IndexOf(statusEffect);
		if (num != -1)
		{
			this.m_statusEffectTypeArray[num] = StatusEffectType.None;
			this.m_statusEffectDurationArray[num] = 0f;
		}
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x000F795C File Offset: 0x000F5B5C
	public void ChangeSpriteRendererColor(Color newColor)
	{
		if (!this.m_checkedForRenderer)
		{
			this.m_checkedForRenderer = true;
			this.m_spriteRenderer = base.GetComponentInChildren<SpriteRenderer>();
			if (this.m_spriteRenderer)
			{
				this.m_startingColor = this.m_spriteRenderer.color;
			}
		}
		if (this.m_spriteRenderer)
		{
			this.m_spriteRenderer.color = newColor;
		}
	}

	// Token: 0x06003CFD RID: 15613 RVA: 0x00021C43 File Offset: 0x0001FE43
	public void ResetSpriteRendererColor()
	{
		if (this.m_spriteRenderer)
		{
			this.m_spriteRenderer.color = this.m_startingColor;
		}
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06003D01 RID: 15617 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IEffectVelocity.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06003D02 RID: 15618 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06003D03 RID: 15619 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IPreOnDisable.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06003D04 RID: 15620 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IOffscreenObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002FD6 RID: 12246
	private const int REPEAT_HIT_ARRAY_SIZE = 5;

	// Token: 0x04002FD7 RID: 12247
	[Header("General Settings")]
	[Tooltip("The Pivot GO on the projectile. Once set, this usually doesn't need to be changed.")]
	[SerializeField]
	protected GameObject m_pivot;

	// Token: 0x04002FD8 RID: 12248
	[Tooltip("The XML data for the projectile.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	protected ProjectileData m_projectileData;

	// Token: 0x04002FD9 RID: 12249
	[Tooltip("Determines whether the projectile grows or shrinks based off the Owner's scale. NOTE: If the Owner is a BaseCharacterController, it scales off the BaseScaleToOffsetWith.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_scaleWithOwner;

	// Token: 0x04002FDA RID: 12250
	[Tooltip("Parents the Owner to the projectile, so that the projectile follows wherever the Owner goes. If the Owner flips (scale.x == -1) so does the projectile. NOTE: BaseAbility can override the flip by changing its 'Projectile Match Facing' property.")]
	[SerializeField]
	private bool m_snapToOwner;

	// Token: 0x04002FDB RID: 12251
	[Tooltip("Determines if the projectile gets destroyed when the player exits the room.")]
	[SerializeField]
	private bool m_destroyOnRoomChange = true;

	// Token: 0x04002FDC RID: 12252
	[Header("Visual Settings")]
	[Tooltip("Determines whether the Visual pivot of the projectile follows the heading it's going in (i.e. does the object visually rotate to match the direction it's going in). NOTE: This field overrides Visual Rotation Speed.")]
	[SerializeField]
	private bool m_pivotFollowsOrientation;

	// Token: 0x04002FDD RID: 12253
	[Tooltip("How quickly (in degrees per second) the projectile visually spins in the air. Only the Visual pivot spins, so the projectile does not actually spin. Useful for visuals like a spinning bone projectile. NOTE: If you want to rotate the projectile, use the projectile's Rotation transform.")]
	[SerializeField]
	[ConditionalHide("m_pivotFollowsOrientation", true, true)]
	private float m_visualRotationSpeed;

	// Token: 0x04002FDE RID: 12254
	[Tooltip("We use velocity to determine how effects should occur when two objects interact (for ex. walking past curtains). When a projectile is fired, it uses its own velocity for the effect.  This field uses the Owner's velocity instead.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_useOwnerVelocityForEffects;

	// Token: 0x04002FDF RID: 12255
	[Tooltip("Determines what type of strike effect should play when the projectile hits something.")]
	[SerializeField]
	private StrikeType m_strikeType = StrikeType.Blunt;

	// Token: 0x04002FE0 RID: 12256
	[Tooltip("Determines if Offscreen warnings appear for this projectile.")]
	[SerializeField]
	private bool m_disableOffscreenWarnings;

	// Token: 0x04002FE1 RID: 12257
	[Header("Animation Settings")]
	[SerializeField]
	[Tooltip("Setting this to true means the projectile will not be collidable until its intro has played")]
	private bool m_hasAnimStates;

	// Token: 0x04002FE2 RID: 12258
	[SerializeField]
	[ConditionalHide("m_hasAnimStates", true)]
	private bool m_hasIntro;

	// Token: 0x04002FE3 RID: 12259
	[SerializeField]
	[ConditionalHide("m_hasAnimStates", true)]
	private bool m_playOuttroOnTimeoutOnly;

	// Token: 0x04002FE4 RID: 12260
	[SerializeField]
	[ConditionalHide("m_hasAnimStates", true)]
	private string m_outtroTriggerName;

	// Token: 0x04002FE5 RID: 12261
	[SerializeField]
	[ConditionalHide("m_hasAnimStates", true)]
	private int m_animLayer;

	// Token: 0x04002FE6 RID: 12262
	[SerializeField]
	[Tooltip("Determines whether the projectile keeps moving during its death anim when dashed through")]
	private bool m_keepMovingWhenDashedThrough;

	// Token: 0x04002FE7 RID: 12263
	[Header("Collision Settings")]
	[Tooltip("Does this projectile deal dot damage. I.e. does it deal damage but not trigger invincibility.")]
	[SerializeField]
	private bool m_isDotDamage;

	// Token: 0x04002FE8 RID: 12264
	[Tooltip("Does this projectile deal hazard damage. This will override the projectile data's strength with Hazard_EV.GetDamageAmount().")]
	[SerializeField]
	private bool m_dealsHazardDamage;

	// Token: 0x04002FE9 RID: 12265
	[Tooltip("Used to determine whether we should use the Owner or the projectile as the point of collision. Mostly used for positioning the strike effect and knockback. For ex. if an enemy is knocked back, does he push away from the projectile or the Owner. NOTE: SnapToOwner might be able to replace this field entirely.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_useOwnerCollisionPoint = true;

	// Token: 0x04002FEA RID: 12266
	[Tooltip("Determines whether a projectile can hit its owner or not. 99.9% of the time this is false.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_canHitOwner;

	// Token: 0x04002FEB RID: 12267
	[Tooltip("Determines if the projectile can hit breakables.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_canHitBreakables;

	// Token: 0x04002FEC RID: 12268
	[Tooltip("Determines if the projectile only hits flimsy breakables.")]
	[SerializeField]
	[ConditionalHide("m_canHitBreakables", true)]
	[ReadOnlyOnPlay]
	private bool m_hitsFlimsyBreakablesOnly;

	// Token: 0x04002FED RID: 12269
	[Tooltip("Determines if the projectile is allowed to collide with walls.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_canHitWall;

	// Token: 0x04002FEE RID: 12270
	[Tooltip("Determines if the projectile is destroyed when colliding with walls.")]
	[SerializeField]
	[ConditionalHide("m_canHitWall", true)]
	private bool m_dieOnWallCollision;

	// Token: 0x04002FEF RID: 12271
	[Tooltip("Determines if the projectile is allowed to collide with walls.")]
	[SerializeField]
	[ConditionalHide("m_dieOnWallCollision", true)]
	private bool m_dieOnRoomBoundsCollision;

	// Token: 0x04002FF0 RID: 12272
	[Tooltip("Determines if the projectile is destroyed when colliding with characters.  This includes breakables.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_dieOnCharacterCollision;

	// Token: 0x04002FF1 RID: 12273
	[Header("Projectile-on-Projectile Collisions")]
	[Tooltip("The type of projectile I am. This must match the CanCollideWithFlag of the oncoming projectile for a collision to occur.")]
	[SerializeField]
	private ProjectileCollisionFlag m_collisionFlag;

	// Token: 0x04002FF2 RID: 12274
	[Tooltip("Which projectiles I can kill. This must match the CollisionFlag of the oncoming projectile for a collision to occur. Right now only player projectiles can kill other projectiles.")]
	[SerializeField]
	private ProjectileCollisionFlag m_canCollideWithFlag;

	// Token: 0x04002FF3 RID: 12275
	[Tooltip("Determines if the on-hit CD logic has a delay. Only applies to AxeSpinner")]
	[SerializeField]
	private bool m_hasCDReductionDelay;

	// Token: 0x04002FF4 RID: 12276
	[Header("Physics Settings")]
	[Tooltip("Determines if the projectile falls due to gravity or not.")]
	[SerializeField]
	private bool m_isWeighted;

	// Token: 0x04002FF5 RID: 12277
	[Tooltip("The amount of time before gravity kicks in for a projectile. NOTE: This setting only works if 'Is Weighted' == true.")]
	[SerializeField]
	[ConditionalHide("m_isWeighted", true)]
	private float m_gravityKickInDelay;

	// Token: 0x04002FF6 RID: 12278
	[Tooltip("A modifier on how much gravity pushes down on the projectile when it is going up. NOTE: This setting only works if 'Is Weighted' == true.")]
	[SerializeField]
	[ConditionalHide("m_isWeighted", true)]
	private float m_ascentMultiplierOverride = 1f;

	// Token: 0x04002FF7 RID: 12279
	[Tooltip("A modifier on how much gravity pushes down on the projectile when it is going down. NOTE: This setting only works if 'Is Weighted' == true.")]
	[SerializeField]
	[ConditionalHide("m_isWeighted", true)]
	private float m_fallMultiplierOverride = 1f;

	// Token: 0x04002FF8 RID: 12280
	[Tooltip("Determines whether the projectile should keep sliding when it hits the ground or if it should stop. NOTE: This only works if the projectile has a CorgiController attached to control its physics (necessary for projectiles that 'bounce' off walls) and 'Is Weighted' == true.")]
	[SerializeField]
	[ConditionalHide("m_isWeighted", true)]
	private bool m_disableFriction;

	// Token: 0x04002FF9 RID: 12281
	[Header("Ricochet Settings")]
	[Tooltip("Certain enemies knock back the player when the Player's projectile hits the enemy. Set this to false to disable the ricochet logic (For ex. the bow's arrow projectile). NOTE: Affects Player only.")]
	[SerializeField]
	private bool m_canBeRicocheted;

	// Token: 0x04002FFA RID: 12282
	[Tooltip("If this is on, the projectile will knock back the Player regardless of whether the enemy is set to ricochet the Player or not. Useful for a trait. NOTE: Only works if 'Can Be Ricocheted' == true and affects Player only.")]
	[SerializeField]
	[ConditionalHide("m_canBeRicocheted", true)]
	private bool m_ricochetsOwnerWhenHits;

	// Token: 0x04002FFB RID: 12283
	[Tooltip("The knockback distance of the ricochet.")]
	[SerializeField]
	[ConditionalHide("m_canBeRicocheted", true)]
	private Vector2 m_ricochetKnockbackDistance = Player_EV.PLAYER_BASE_KNOCKBACK_DISTANCE;

	// Token: 0x04002FFC RID: 12284
	[Tooltip("The control lockout duration of the ricochet.")]
	[SerializeField]
	[ConditionalHide("m_canBeRicocheted", true)]
	private float m_ricochetLockoutDuration = 0.45f;

	// Token: 0x04002FFD RID: 12285
	[Header("Misc Settings - Programmers only")]
	[Tooltip("This is purely for artists.  Do not touch.")]
	[SerializeField]
	private bool m_ignoreTrailRendererYScaleMatching;

	// Token: 0x04002FFE RID: 12286
	protected float m_lifeSpanTimer;

	// Token: 0x04002FFF RID: 12287
	protected GameObject m_owner;

	// Token: 0x04003000 RID: 12288
	protected BaseCharacterController m_ownerController;

	// Token: 0x04003001 RID: 12289
	protected IHitboxController m_hbController;

	// Token: 0x04003002 RID: 12290
	private Animator m_animator;

	// Token: 0x04003003 RID: 12291
	protected GameObject m_collidedObj;

	// Token: 0x04003004 RID: 12292
	private PlayerController m_player;

	// Token: 0x04003005 RID: 12293
	private CorgiController_RL m_corgiController;

	// Token: 0x04003006 RID: 12294
	private SpriteRenderer m_spriteRenderer;

	// Token: 0x04003007 RID: 12295
	private Color m_startingColor;

	// Token: 0x04003008 RID: 12296
	private bool m_wasSpawned;

	// Token: 0x04003009 RID: 12297
	protected Vector2 m_movement;

	// Token: 0x0400300A RID: 12298
	protected Vector2 m_heading = new Vector2(1f, 0f);

	// Token: 0x0400300B RID: 12299
	protected float m_orientation;

	// Token: 0x0400300C RID: 12300
	protected Vector2 m_acceleration;

	// Token: 0x0400300D RID: 12301
	protected float m_gravityKickInTimer;

	// Token: 0x0400300E RID: 12302
	private Vector3 m_initialScale;

	// Token: 0x0400300F RID: 12303
	private Vector3 m_initialRotation;

	// Token: 0x04003010 RID: 12304
	private float m_initialRotationSpeed;

	// Token: 0x04003011 RID: 12305
	private bool m_flagToDestroy;

	// Token: 0x04003012 RID: 12306
	private bool m_ignoreDamageScale;

	// Token: 0x04003013 RID: 12307
	private bool m_isDying;

	// Token: 0x04003014 RID: 12308
	private bool m_hasVelocityParams;

	// Token: 0x04003015 RID: 12309
	private bool m_collisionPointChanged;

	// Token: 0x04003016 RID: 12310
	private Action<Projectile_RL, GameObject> m_ricochetPlayer;

	// Token: 0x04003017 RID: 12311
	private bool m_detachOnDestruction;

	// Token: 0x04003018 RID: 12312
	private BaseProjectileLogic[] m_projectileLogicArray;

	// Token: 0x04003019 RID: 12313
	private StatusEffectType[] m_statusEffectTypeArray;

	// Token: 0x0400301A RID: 12314
	private float[] m_statusEffectDurationArray;

	// Token: 0x0400301B RID: 12315
	private VoidDissolveComponent m_voidDissolve;

	// Token: 0x0400301C RID: 12316
	protected Projectile_RL.ProjectileDestructionType m_destructionType;

	// Token: 0x0400301D RID: 12317
	private bool m_hasSearchedForAudioEventEmitter;

	// Token: 0x0400301E RID: 12318
	private ProjectileEventEmitter m_audioEventEmitter;

	// Token: 0x04003026 RID: 12326
	protected Relay<Projectile_RL, GameObject> m_onCollisionRelay = new Relay<Projectile_RL, GameObject>();

	// Token: 0x04003027 RID: 12327
	protected Relay<Projectile_RL, GameObject> m_onSpawnRelay = new Relay<Projectile_RL, GameObject>();

	// Token: 0x04003028 RID: 12328
	protected Relay<Projectile_RL, GameObject> m_onDeathRelay = new Relay<Projectile_RL, GameObject>();

	// Token: 0x04003029 RID: 12329
	protected Relay<GameObject> m_onDeathEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x0400302A RID: 12330
	protected Relay<GameObject> m_onSpawnEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x0400302B RID: 12331
	protected Relay<GameObject> m_onTimeoutEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x0400302C RID: 12332
	private Relay<IPreOnDisable> m_preOnDisableRelay = new Relay<IPreOnDisable>();

	// Token: 0x0400303B RID: 12347
	private WaitForFixedUpdate m_fixedUpdateYield = new WaitForFixedUpdate();

	// Token: 0x0400303C RID: 12348
	private Coroutine m_changeCollPointCoroutine;

	// Token: 0x0400303D RID: 12349
	private WaitRL_Yield m_canHitOwnerWaitYield;

	// Token: 0x0400303E RID: 12350
	private bool m_checkedForRenderer;

	// Token: 0x020007C4 RID: 1988
	protected enum ProjectileDestructionType
	{
		// Token: 0x04003040 RID: 12352
		None,
		// Token: 0x04003041 RID: 12353
		TerrainCollision,
		// Token: 0x04003042 RID: 12354
		CharacterCollision,
		// Token: 0x04003043 RID: 12355
		VoidDashCollision
	}
}
