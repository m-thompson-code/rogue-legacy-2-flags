using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
public class Projectile_RL : MonoBehaviour, IDamageObj, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnStayHitResponse, IWeaponOnEnterHitResponse, IWeaponOnStayHitResponse, IBodyOnEnterHitResponse, IBodyOnStayHitResponse, IRootObj, IMidpointObj, IHeading, IEffectVelocity, IEffectTriggerEvent_OnDeath, IEffectTriggerEvent_OnSpawn, IEffectTriggerEvent_OnTimeout, IGenericPoolObj, IPreOnDisable, IPlayHitEffect, IPivotObj, IOffscreenObj
{
	// Token: 0x06002BBF RID: 11199 RVA: 0x00094D56 File Offset: 0x00092F56
	public static bool OwnsProjectile(GameObject go, Projectile_RL projectile)
	{
		return projectile && !projectile.m_flagToDestroy && projectile.isActiveAndEnabled && projectile.Owner == go;
	}

	// Token: 0x06002BC0 RID: 11200 RVA: 0x00094D7E File Offset: 0x00092F7E
	public static bool CollisionFlagAllowed(Projectile_RL proj1, Projectile_RL proj2)
	{
		return (proj1.CanCollideWithFlags & proj2.CollisionFlags) != ProjectileCollisionFlag.None || (proj2.CanCollideWithFlags & proj1.CollisionFlags) > ProjectileCollisionFlag.None;
	}

	// Token: 0x170010AA RID: 4266
	// (get) Token: 0x06002BC1 RID: 11201 RVA: 0x00094DA1 File Offset: 0x00092FA1
	public bool DisableOffscreenWarnings
	{
		get
		{
			return this.m_disableOffscreenWarnings;
		}
	}

	// Token: 0x170010AB RID: 4267
	// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x00094DA9 File Offset: 0x00092FA9
	// (set) Token: 0x06002BC3 RID: 11203 RVA: 0x00094DB1 File Offset: 0x00092FB1
	public bool MatchFacing { get; set; }

	// Token: 0x170010AC RID: 4268
	// (get) Token: 0x06002BC4 RID: 11204 RVA: 0x00094DBA File Offset: 0x00092FBA
	public float InitialRotationSpeed
	{
		get
		{
			return this.m_initialRotationSpeed;
		}
	}

	// Token: 0x170010AD RID: 4269
	// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x00094DC2 File Offset: 0x00092FC2
	// (set) Token: 0x06002BC6 RID: 11206 RVA: 0x00094DCA File Offset: 0x00092FCA
	public bool IsPersistentProjectile { get; set; }

	// Token: 0x170010AE RID: 4270
	// (get) Token: 0x06002BC7 RID: 11207 RVA: 0x00094DD3 File Offset: 0x00092FD3
	public bool HasIntro
	{
		get
		{
			return this.m_hasIntro;
		}
	}

	// Token: 0x170010AF RID: 4271
	// (get) Token: 0x06002BC8 RID: 11208 RVA: 0x00094DDB File Offset: 0x00092FDB
	// (set) Token: 0x06002BC9 RID: 11209 RVA: 0x00094DE3 File Offset: 0x00092FE3
	public float CDReducDelay { get; set; }

	// Token: 0x170010B0 RID: 4272
	// (get) Token: 0x06002BCA RID: 11210 RVA: 0x00094DEC File Offset: 0x00092FEC
	public bool HasCDReductionDelay
	{
		get
		{
			return this.m_hasCDReductionDelay;
		}
	}

	// Token: 0x170010B1 RID: 4273
	// (get) Token: 0x06002BCB RID: 11211 RVA: 0x00094DF4 File Offset: 0x00092FF4
	// (set) Token: 0x06002BCC RID: 11212 RVA: 0x00094DFC File Offset: 0x00092FFC
	public Vector2 CollisionPointOffset { get; set; } = Vector2.zero;

	// Token: 0x170010B2 RID: 4274
	// (get) Token: 0x06002BCD RID: 11213 RVA: 0x00094E05 File Offset: 0x00093005
	// (set) Token: 0x06002BCE RID: 11214 RVA: 0x00094E0D File Offset: 0x0009300D
	public bool IsFreePoolObj { get; set; }

	// Token: 0x170010B3 RID: 4275
	// (get) Token: 0x06002BCF RID: 11215 RVA: 0x00094E16 File Offset: 0x00093016
	public ProjectileCollisionFlag CanCollideWithFlags
	{
		get
		{
			return this.m_canCollideWithFlag;
		}
	}

	// Token: 0x170010B4 RID: 4276
	// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x00094E1E File Offset: 0x0009301E
	public ProjectileCollisionFlag CollisionFlags
	{
		get
		{
			return this.m_collisionFlag;
		}
	}

	// Token: 0x170010B5 RID: 4277
	// (get) Token: 0x06002BD1 RID: 11217 RVA: 0x00094E26 File Offset: 0x00093026
	// (set) Token: 0x06002BD2 RID: 11218 RVA: 0x00094E2E File Offset: 0x0009302E
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x170010B6 RID: 4278
	// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x00094E37 File Offset: 0x00093037
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

	// Token: 0x170010B7 RID: 4279
	// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x00094E67 File Offset: 0x00093067
	// (set) Token: 0x06002BD5 RID: 11221 RVA: 0x00094E6F File Offset: 0x0009306F
	public CastAbilityType CastAbilityType { get; set; }

	// Token: 0x170010B8 RID: 4280
	// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x00094E78 File Offset: 0x00093078
	public bool DestroyOnRoomChange
	{
		get
		{
			return this.m_destroyOnRoomChange;
		}
	}

	// Token: 0x170010B9 RID: 4281
	// (get) Token: 0x06002BD7 RID: 11223 RVA: 0x00094E80 File Offset: 0x00093080
	public float[] StatusEffectDurations
	{
		get
		{
			return this.m_statusEffectDurationArray;
		}
	}

	// Token: 0x170010BA RID: 4282
	// (get) Token: 0x06002BD8 RID: 11224 RVA: 0x00094E88 File Offset: 0x00093088
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return this.m_statusEffectTypeArray;
		}
	}

	// Token: 0x170010BB RID: 4283
	// (get) Token: 0x06002BD9 RID: 11225 RVA: 0x00094E90 File Offset: 0x00093090
	public bool IsDotDamage
	{
		get
		{
			return this.m_isDotDamage;
		}
	}

	// Token: 0x170010BC RID: 4284
	// (get) Token: 0x06002BDA RID: 11226 RVA: 0x00094E98 File Offset: 0x00093098
	public bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170010BD RID: 4285
	// (get) Token: 0x06002BDB RID: 11227 RVA: 0x00094E9B File Offset: 0x0009309B
	public bool PlayDirectionalHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170010BE RID: 4286
	// (get) Token: 0x06002BDC RID: 11228 RVA: 0x00094E9E File Offset: 0x0009309E
	public string EffectNameOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170010BF RID: 4287
	// (get) Token: 0x06002BDD RID: 11229 RVA: 0x00094EA1 File Offset: 0x000930A1
	public IRelayLink<Projectile_RL, GameObject> OnCollisionRelay
	{
		get
		{
			return this.m_onCollisionRelay.link;
		}
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x00094EAE File Offset: 0x000930AE
	public void ForceDispatchOnCollisionRelay(GameObject otherObj)
	{
		this.m_onCollisionRelay.Dispatch(this, otherObj);
	}

	// Token: 0x170010C0 RID: 4288
	// (get) Token: 0x06002BDF RID: 11231 RVA: 0x00094EBD File Offset: 0x000930BD
	public IRelayLink<Projectile_RL, GameObject> OnSpawnRelay
	{
		get
		{
			return this.m_onSpawnRelay.link;
		}
	}

	// Token: 0x170010C1 RID: 4289
	// (get) Token: 0x06002BE0 RID: 11232 RVA: 0x00094ECA File Offset: 0x000930CA
	public IRelayLink<Projectile_RL, GameObject> OnDeathRelay
	{
		get
		{
			return this.m_onDeathRelay.link;
		}
	}

	// Token: 0x170010C2 RID: 4290
	// (get) Token: 0x06002BE1 RID: 11233 RVA: 0x00094ED7 File Offset: 0x000930D7
	public IRelayLink<GameObject> OnDeathEffectTriggerRelay
	{
		get
		{
			return this.m_onDeathEffectTriggerRelay.link;
		}
	}

	// Token: 0x170010C3 RID: 4291
	// (get) Token: 0x06002BE2 RID: 11234 RVA: 0x00094EE4 File Offset: 0x000930E4
	public IRelayLink<GameObject> OnSpawnEffectTriggerRelay
	{
		get
		{
			return this.m_onSpawnEffectTriggerRelay.link;
		}
	}

	// Token: 0x170010C4 RID: 4292
	// (get) Token: 0x06002BE3 RID: 11235 RVA: 0x00094EF1 File Offset: 0x000930F1
	public IRelayLink<GameObject> OnTimeoutEffectTriggerRelay
	{
		get
		{
			return this.m_onTimeoutEffectTriggerRelay.link;
		}
	}

	// Token: 0x170010C5 RID: 4293
	// (get) Token: 0x06002BE4 RID: 11236 RVA: 0x00094EFE File Offset: 0x000930FE
	public IRelayLink<IPreOnDisable> PreOnDisableRelay
	{
		get
		{
			return this.m_preOnDisableRelay.link;
		}
	}

	// Token: 0x170010C6 RID: 4294
	// (get) Token: 0x06002BE5 RID: 11237 RVA: 0x00094F0B File Offset: 0x0009310B
	public IHitboxController HitboxController
	{
		get
		{
			return this.m_hbController;
		}
	}

	// Token: 0x170010C7 RID: 4295
	// (get) Token: 0x06002BE6 RID: 11238 RVA: 0x00094F13 File Offset: 0x00093113
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x170010C8 RID: 4296
	// (get) Token: 0x06002BE7 RID: 11239 RVA: 0x00094F1B File Offset: 0x0009311B
	public float GravityKickInDelay
	{
		get
		{
			return this.m_gravityKickInDelay;
		}
	}

	// Token: 0x170010C9 RID: 4297
	// (get) Token: 0x06002BE8 RID: 11240 RVA: 0x00094F23 File Offset: 0x00093123
	public float FallMultiplierOverride
	{
		get
		{
			return this.m_fallMultiplierOverride;
		}
	}

	// Token: 0x170010CA RID: 4298
	// (get) Token: 0x06002BE9 RID: 11241 RVA: 0x00094F2B File Offset: 0x0009312B
	public ProjectileData ProjectileData
	{
		get
		{
			return this.m_projectileData;
		}
	}

	// Token: 0x06002BEA RID: 11242 RVA: 0x00094F33 File Offset: 0x00093133
	public void SetProjectileData(ProjectileData projData)
	{
		this.m_projectileData = projData;
	}

	// Token: 0x170010CB RID: 4299
	// (get) Token: 0x06002BEB RID: 11243 RVA: 0x00094F3C File Offset: 0x0009313C
	// (set) Token: 0x06002BEC RID: 11244 RVA: 0x00094F44 File Offset: 0x00093144
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

	// Token: 0x170010CC RID: 4300
	// (get) Token: 0x06002BED RID: 11245 RVA: 0x00094F4D File Offset: 0x0009314D
	public StrikeType StrikeType
	{
		get
		{
			return this.m_strikeType;
		}
	}

	// Token: 0x170010CD RID: 4301
	// (get) Token: 0x06002BEE RID: 11246 RVA: 0x00094F55 File Offset: 0x00093155
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

	// Token: 0x170010CE RID: 4302
	// (get) Token: 0x06002BEF RID: 11247 RVA: 0x00094F78 File Offset: 0x00093178
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

	// Token: 0x170010CF RID: 4303
	// (get) Token: 0x06002BF0 RID: 11248 RVA: 0x00094FF8 File Offset: 0x000931F8
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

	// Token: 0x170010D0 RID: 4304
	// (get) Token: 0x06002BF1 RID: 11249 RVA: 0x00095026 File Offset: 0x00093226
	// (set) Token: 0x06002BF2 RID: 11250 RVA: 0x0009502E File Offset: 0x0009322E
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

	// Token: 0x170010D1 RID: 4305
	// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x00095037 File Offset: 0x00093237
	// (set) Token: 0x06002BF4 RID: 11252 RVA: 0x0009503F File Offset: 0x0009323F
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

	// Token: 0x170010D2 RID: 4306
	// (get) Token: 0x06002BF5 RID: 11253 RVA: 0x00095048 File Offset: 0x00093248
	// (set) Token: 0x06002BF6 RID: 11254 RVA: 0x00095050 File Offset: 0x00093250
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

	// Token: 0x170010D3 RID: 4307
	// (get) Token: 0x06002BF7 RID: 11255 RVA: 0x00095059 File Offset: 0x00093259
	public GameObject Pivot
	{
		get
		{
			return this.m_pivot;
		}
	}

	// Token: 0x170010D4 RID: 4308
	// (get) Token: 0x06002BF8 RID: 11256 RVA: 0x00095061 File Offset: 0x00093261
	// (set) Token: 0x06002BF9 RID: 11257 RVA: 0x00095069 File Offset: 0x00093269
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

	// Token: 0x170010D5 RID: 4309
	// (get) Token: 0x06002BFA RID: 11258 RVA: 0x00095072 File Offset: 0x00093272
	// (set) Token: 0x06002BFB RID: 11259 RVA: 0x0009507A File Offset: 0x0009327A
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

	// Token: 0x170010D6 RID: 4310
	// (get) Token: 0x06002BFC RID: 11260 RVA: 0x00095083 File Offset: 0x00093283
	public virtual DamageType DamageType
	{
		get
		{
			return this.m_projectileData.DamageType;
		}
	}

	// Token: 0x170010D7 RID: 4311
	// (get) Token: 0x06002BFD RID: 11261 RVA: 0x00095090 File Offset: 0x00093290
	// (set) Token: 0x06002BFE RID: 11262 RVA: 0x00095098 File Offset: 0x00093298
	public virtual float StrengthScale { get; set; }

	// Token: 0x170010D8 RID: 4312
	// (get) Token: 0x06002BFF RID: 11263 RVA: 0x000950A1 File Offset: 0x000932A1
	// (set) Token: 0x06002C00 RID: 11264 RVA: 0x000950A9 File Offset: 0x000932A9
	public virtual float MagicScale { get; set; }

	// Token: 0x170010D9 RID: 4313
	// (get) Token: 0x06002C01 RID: 11265 RVA: 0x000950B2 File Offset: 0x000932B2
	public virtual float CooldownReductionPerHit
	{
		get
		{
			return this.m_projectileData.CooldownReductionPerHit;
		}
	}

	// Token: 0x170010DA RID: 4314
	// (get) Token: 0x06002C02 RID: 11266 RVA: 0x000950BF File Offset: 0x000932BF
	public virtual float ManaGainPerHit
	{
		get
		{
			return this.m_projectileData.ManaGainPerHit;
		}
	}

	// Token: 0x170010DB RID: 4315
	// (get) Token: 0x06002C03 RID: 11267 RVA: 0x000950CC File Offset: 0x000932CC
	// (set) Token: 0x06002C04 RID: 11268 RVA: 0x000950D4 File Offset: 0x000932D4
	public virtual float BaseStunStrength { get; set; }

	// Token: 0x170010DC RID: 4316
	// (get) Token: 0x06002C05 RID: 11269 RVA: 0x000950DD File Offset: 0x000932DD
	public virtual float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x170010DD RID: 4317
	// (get) Token: 0x06002C06 RID: 11270 RVA: 0x000950E5 File Offset: 0x000932E5
	// (set) Token: 0x06002C07 RID: 11271 RVA: 0x000950ED File Offset: 0x000932ED
	public virtual float BaseKnockbackStrength { get; set; }

	// Token: 0x170010DE RID: 4318
	// (get) Token: 0x06002C08 RID: 11272 RVA: 0x000950F6 File Offset: 0x000932F6
	public virtual float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x170010DF RID: 4319
	// (get) Token: 0x06002C09 RID: 11273 RVA: 0x000950FE File Offset: 0x000932FE
	// (set) Token: 0x06002C0A RID: 11274 RVA: 0x00095106 File Offset: 0x00093306
	public virtual Vector2 ExternalKnockbackMod { get; set; }

	// Token: 0x170010E0 RID: 4320
	// (get) Token: 0x06002C0B RID: 11275 RVA: 0x0009510F File Offset: 0x0009330F
	// (set) Token: 0x06002C0C RID: 11276 RVA: 0x00095117 File Offset: 0x00093317
	public virtual float Speed { get; set; }

	// Token: 0x170010E1 RID: 4321
	// (get) Token: 0x06002C0D RID: 11277 RVA: 0x00095120 File Offset: 0x00093320
	// (set) Token: 0x06002C0E RID: 11278 RVA: 0x00095128 File Offset: 0x00093328
	public virtual float TurnSpeed { get; set; }

	// Token: 0x170010E2 RID: 4322
	// (get) Token: 0x06002C0F RID: 11279 RVA: 0x00095131 File Offset: 0x00093331
	// (set) Token: 0x06002C10 RID: 11280 RVA: 0x00095139 File Offset: 0x00093339
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

	// Token: 0x170010E3 RID: 4323
	// (get) Token: 0x06002C11 RID: 11281 RVA: 0x00095142 File Offset: 0x00093342
	// (set) Token: 0x06002C12 RID: 11282 RVA: 0x0009514A File Offset: 0x0009334A
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

	// Token: 0x170010E4 RID: 4324
	// (get) Token: 0x06002C13 RID: 11283 RVA: 0x00095153 File Offset: 0x00093353
	// (set) Token: 0x06002C14 RID: 11284 RVA: 0x0009515B File Offset: 0x0009335B
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

	// Token: 0x170010E5 RID: 4325
	// (get) Token: 0x06002C15 RID: 11285 RVA: 0x00095164 File Offset: 0x00093364
	// (set) Token: 0x06002C16 RID: 11286 RVA: 0x0009516C File Offset: 0x0009336C
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

	// Token: 0x170010E6 RID: 4326
	// (get) Token: 0x06002C17 RID: 11287 RVA: 0x00095175 File Offset: 0x00093375
	// (set) Token: 0x06002C18 RID: 11288 RVA: 0x0009517D File Offset: 0x0009337D
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

	// Token: 0x170010E7 RID: 4327
	// (get) Token: 0x06002C19 RID: 11289 RVA: 0x0009518D File Offset: 0x0009338D
	// (set) Token: 0x06002C1A RID: 11290 RVA: 0x00095195 File Offset: 0x00093395
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

	// Token: 0x170010E8 RID: 4328
	// (get) Token: 0x06002C1B RID: 11291 RVA: 0x0009519E File Offset: 0x0009339E
	// (set) Token: 0x06002C1C RID: 11292 RVA: 0x000951A6 File Offset: 0x000933A6
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

	// Token: 0x170010E9 RID: 4329
	// (get) Token: 0x06002C1D RID: 11293 RVA: 0x000951AF File Offset: 0x000933AF
	// (set) Token: 0x06002C1E RID: 11294 RVA: 0x000951B7 File Offset: 0x000933B7
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

	// Token: 0x170010EA RID: 4330
	// (get) Token: 0x06002C1F RID: 11295 RVA: 0x000951C0 File Offset: 0x000933C0
	// (set) Token: 0x06002C20 RID: 11296 RVA: 0x000951D2 File Offset: 0x000933D2
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

	// Token: 0x170010EB RID: 4331
	// (get) Token: 0x06002C21 RID: 11297 RVA: 0x000951DB File Offset: 0x000933DB
	// (set) Token: 0x06002C22 RID: 11298 RVA: 0x000951E3 File Offset: 0x000933E3
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

	// Token: 0x170010EC RID: 4332
	// (get) Token: 0x06002C23 RID: 11299 RVA: 0x000951EC File Offset: 0x000933EC
	public virtual float RepeatHitDuration
	{
		get
		{
			return this.m_projectileData.RepeatHitCheckTimer;
		}
	}

	// Token: 0x170010ED RID: 4333
	// (get) Token: 0x06002C24 RID: 11300 RVA: 0x000951F9 File Offset: 0x000933F9
	public virtual float Lifespan
	{
		get
		{
			return this.m_projectileData.LifeSpan;
		}
	}

	// Token: 0x170010EE RID: 4334
	// (get) Token: 0x06002C25 RID: 11301 RVA: 0x00095206 File Offset: 0x00093406
	// (set) Token: 0x06002C26 RID: 11302 RVA: 0x0009520E File Offset: 0x0009340E
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

	// Token: 0x170010EF RID: 4335
	// (get) Token: 0x06002C27 RID: 11303 RVA: 0x00095218 File Offset: 0x00093418
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

	// Token: 0x170010F0 RID: 4336
	// (get) Token: 0x06002C28 RID: 11304 RVA: 0x0009528A File Offset: 0x0009348A
	// (set) Token: 0x06002C29 RID: 11305 RVA: 0x00095292 File Offset: 0x00093492
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

	// Token: 0x170010F1 RID: 4337
	// (get) Token: 0x06002C2A RID: 11306 RVA: 0x0009529B File Offset: 0x0009349B
	// (set) Token: 0x06002C2B RID: 11307 RVA: 0x000952A8 File Offset: 0x000934A8
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

	// Token: 0x170010F2 RID: 4338
	// (get) Token: 0x06002C2C RID: 11308 RVA: 0x000952B6 File Offset: 0x000934B6
	// (set) Token: 0x06002C2D RID: 11309 RVA: 0x000952C3 File Offset: 0x000934C3
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

	// Token: 0x170010F3 RID: 4339
	// (get) Token: 0x06002C2E RID: 11310 RVA: 0x000952D1 File Offset: 0x000934D1
	// (set) Token: 0x06002C2F RID: 11311 RVA: 0x000952DC File Offset: 0x000934DC
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

	// Token: 0x170010F4 RID: 4340
	// (get) Token: 0x06002C30 RID: 11312 RVA: 0x0009532A File Offset: 0x0009352A
	// (set) Token: 0x06002C31 RID: 11313 RVA: 0x00095338 File Offset: 0x00093538
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

	// Token: 0x170010F5 RID: 4341
	// (get) Token: 0x06002C32 RID: 11314 RVA: 0x0009538B File Offset: 0x0009358B
	// (set) Token: 0x06002C33 RID: 11315 RVA: 0x00095398 File Offset: 0x00093598
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

	// Token: 0x170010F6 RID: 4342
	// (get) Token: 0x06002C34 RID: 11316 RVA: 0x000953EB File Offset: 0x000935EB
	// (set) Token: 0x06002C35 RID: 11317 RVA: 0x000953F3 File Offset: 0x000935F3
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

	// Token: 0x170010F7 RID: 4343
	// (get) Token: 0x06002C36 RID: 11318 RVA: 0x00095431 File Offset: 0x00093631
	// (set) Token: 0x06002C37 RID: 11319 RVA: 0x00095439 File Offset: 0x00093639
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

	// Token: 0x170010F8 RID: 4344
	// (get) Token: 0x06002C38 RID: 11320 RVA: 0x00095468 File Offset: 0x00093668
	public BaseCharacterController OwnerController
	{
		get
		{
			return this.m_ownerController;
		}
	}

	// Token: 0x170010F9 RID: 4345
	// (get) Token: 0x06002C39 RID: 11321 RVA: 0x00095470 File Offset: 0x00093670
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

	// Token: 0x170010FA RID: 4346
	// (get) Token: 0x06002C3A RID: 11322 RVA: 0x000954A3 File Offset: 0x000936A3
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

	// Token: 0x170010FB RID: 4347
	// (get) Token: 0x06002C3B RID: 11323 RVA: 0x000954CA File Offset: 0x000936CA
	// (set) Token: 0x06002C3C RID: 11324 RVA: 0x000954D2 File Offset: 0x000936D2
	public virtual float DamageMod { get; set; }

	// Token: 0x170010FC RID: 4348
	// (get) Token: 0x06002C3D RID: 11325 RVA: 0x000954DB File Offset: 0x000936DB
	// (set) Token: 0x06002C3E RID: 11326 RVA: 0x000954E3 File Offset: 0x000936E3
	public virtual float Magic { get; set; }

	// Token: 0x170010FD RID: 4349
	// (get) Token: 0x06002C3F RID: 11327 RVA: 0x000954EC File Offset: 0x000936EC
	// (set) Token: 0x06002C40 RID: 11328 RVA: 0x000954F4 File Offset: 0x000936F4
	public virtual float Strength { get; set; }

	// Token: 0x170010FE RID: 4350
	// (get) Token: 0x06002C41 RID: 11329 RVA: 0x000954FD File Offset: 0x000936FD
	// (set) Token: 0x06002C42 RID: 11330 RVA: 0x00095505 File Offset: 0x00093705
	public virtual float ActualCritChance { get; set; }

	// Token: 0x170010FF RID: 4351
	// (get) Token: 0x06002C43 RID: 11331 RVA: 0x0009550E File Offset: 0x0009370E
	// (set) Token: 0x06002C44 RID: 11332 RVA: 0x00095516 File Offset: 0x00093716
	public virtual float ActualCritDamage { get; set; }

	// Token: 0x17001100 RID: 4352
	// (get) Token: 0x06002C45 RID: 11333 RVA: 0x0009551F File Offset: 0x0009371F
	// (set) Token: 0x06002C46 RID: 11334 RVA: 0x00095527 File Offset: 0x00093727
	public bool DontFireDeathRelay { get; set; }

	// Token: 0x17001101 RID: 4353
	// (get) Token: 0x06002C47 RID: 11335 RVA: 0x00095530 File Offset: 0x00093730
	// (set) Token: 0x06002C48 RID: 11336 RVA: 0x00095538 File Offset: 0x00093738
	public string RelicDamageTypeString { get; set; }

	// Token: 0x06002C49 RID: 11337 RVA: 0x00095541 File Offset: 0x00093741
	private void OnEnable()
	{
		ProjectileManager.ActiveProjectileCount++;
	}

	// Token: 0x06002C4A RID: 11338 RVA: 0x00095550 File Offset: 0x00093750
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

	// Token: 0x06002C4B RID: 11339 RVA: 0x000955BC File Offset: 0x000937BC
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

	// Token: 0x06002C4C RID: 11340 RVA: 0x0009561C File Offset: 0x0009381C
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

	// Token: 0x06002C4D RID: 11341 RVA: 0x000956DC File Offset: 0x000938DC
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

	// Token: 0x06002C4E RID: 11342 RVA: 0x0009586C File Offset: 0x00093A6C
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

	// Token: 0x06002C4F RID: 11343 RVA: 0x00095928 File Offset: 0x00093B28
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

	// Token: 0x06002C50 RID: 11344 RVA: 0x00095970 File Offset: 0x00093B70
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

	// Token: 0x06002C51 RID: 11345 RVA: 0x000959C0 File Offset: 0x00093BC0
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

	// Token: 0x06002C52 RID: 11346 RVA: 0x00095AC2 File Offset: 0x00093CC2
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

	// Token: 0x06002C53 RID: 11347 RVA: 0x00095AE2 File Offset: 0x00093CE2
	public virtual void SetCorgiVelocity(Vector2 velocity)
	{
		if (this.m_corgiController)
		{
			this.m_corgiController.SetForce(velocity);
		}
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x00095AFD File Offset: 0x00093CFD
	public virtual void SetCorgiVelocity(float x, float y)
	{
		if (this.m_corgiController)
		{
			this.m_corgiController.SetHorizontalForce(x);
			this.m_corgiController.SetHorizontalForce(y);
		}
	}

	// Token: 0x06002C55 RID: 11349 RVA: 0x00095B24 File Offset: 0x00093D24
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

	// Token: 0x06002C56 RID: 11350 RVA: 0x00095E3C File Offset: 0x0009403C
	public virtual void Flip()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.x = -localScale.x;
		base.transform.localScale = localScale;
	}

	// Token: 0x06002C57 RID: 11351 RVA: 0x00095E70 File Offset: 0x00094070
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

	// Token: 0x06002C58 RID: 11352 RVA: 0x0009605C File Offset: 0x0009425C
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

	// Token: 0x06002C59 RID: 11353 RVA: 0x0009609C File Offset: 0x0009429C
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

	// Token: 0x06002C5A RID: 11354 RVA: 0x00096135 File Offset: 0x00094335
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

	// Token: 0x06002C5B RID: 11355 RVA: 0x00096144 File Offset: 0x00094344
	private IEnumerator ChangeCollisionPointCoroutine()
	{
		this.m_collisionPointChanged = true;
		this.UseOwnerCollisionPoint = true;
		yield return this.m_fixedUpdateYield;
		this.m_collisionPointChanged = false;
		this.UseOwnerCollisionPoint = false;
		yield break;
	}

	// Token: 0x06002C5C RID: 11356 RVA: 0x00096153 File Offset: 0x00094353
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

	// Token: 0x06002C5D RID: 11357 RVA: 0x00096180 File Offset: 0x00094380
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

	// Token: 0x06002C5E RID: 11358 RVA: 0x00096212 File Offset: 0x00094412
	private IEnumerator CanHitSelfCoroutine()
	{
		yield break;
		CollisionType collisionType = this.m_hbController.WeaponCollidesWithType;
		CollisionType collisionType2 = this.OwnerController.HitboxController.CollisionType;
		collisionType |= collisionType2;
		this.m_hbController.ChangeCanCollideWith(HitboxType.Weapon, collisionType);
		yield break;
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x00096221 File Offset: 0x00094421
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

	// Token: 0x06002C60 RID: 11360 RVA: 0x00096237 File Offset: 0x00094437
	public void WeaponOnStayHitResponse(IHitboxController otherHBController)
	{
		this.WeaponOnEnterHitResponse(otherHBController);
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x00096240 File Offset: 0x00094440
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

	// Token: 0x06002C62 RID: 11362 RVA: 0x0009636C File Offset: 0x0009456C
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

	// Token: 0x06002C63 RID: 11363 RVA: 0x000964D9 File Offset: 0x000946D9
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnEnterHitResponse(otherHBController);
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x000964E4 File Offset: 0x000946E4
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

	// Token: 0x06002C65 RID: 11365 RVA: 0x0009656C File Offset: 0x0009476C
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x06002C66 RID: 11366 RVA: 0x00096578 File Offset: 0x00094778
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

	// Token: 0x06002C67 RID: 11367 RVA: 0x00096628 File Offset: 0x00094828
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

	// Token: 0x06002C68 RID: 11368 RVA: 0x000966EC File Offset: 0x000948EC
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

	// Token: 0x06002C69 RID: 11369 RVA: 0x00096764 File Offset: 0x00094964
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

	// Token: 0x06002C6A RID: 11370 RVA: 0x00096844 File Offset: 0x00094A44
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

	// Token: 0x06002C6B RID: 11371 RVA: 0x00096884 File Offset: 0x00094A84
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

	// Token: 0x06002C6C RID: 11372 RVA: 0x000968E3 File Offset: 0x00094AE3
	public void ResetSpriteRendererColor()
	{
		if (this.m_spriteRenderer)
		{
			this.m_spriteRenderer.color = this.m_startingColor;
		}
	}

	// Token: 0x06002C6E RID: 11374 RVA: 0x000969D1 File Offset: 0x00094BD1
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06002C6F RID: 11375 RVA: 0x000969D9 File Offset: 0x00094BD9
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x000969E1 File Offset: 0x00094BE1
	GameObject IEffectVelocity.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x000969E9 File Offset: 0x00094BE9
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06002C72 RID: 11378 RVA: 0x000969F1 File Offset: 0x00094BF1
	GameObject IPreOnDisable.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06002C73 RID: 11379 RVA: 0x000969F9 File Offset: 0x00094BF9
	GameObject IOffscreenObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002389 RID: 9097
	private const int REPEAT_HIT_ARRAY_SIZE = 5;

	// Token: 0x0400238A RID: 9098
	[Header("General Settings")]
	[Tooltip("The Pivot GO on the projectile. Once set, this usually doesn't need to be changed.")]
	[SerializeField]
	protected GameObject m_pivot;

	// Token: 0x0400238B RID: 9099
	[Tooltip("The XML data for the projectile.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	protected ProjectileData m_projectileData;

	// Token: 0x0400238C RID: 9100
	[Tooltip("Determines whether the projectile grows or shrinks based off the Owner's scale. NOTE: If the Owner is a BaseCharacterController, it scales off the BaseScaleToOffsetWith.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_scaleWithOwner;

	// Token: 0x0400238D RID: 9101
	[Tooltip("Parents the Owner to the projectile, so that the projectile follows wherever the Owner goes. If the Owner flips (scale.x == -1) so does the projectile. NOTE: BaseAbility can override the flip by changing its 'Projectile Match Facing' property.")]
	[SerializeField]
	private bool m_snapToOwner;

	// Token: 0x0400238E RID: 9102
	[Tooltip("Determines if the projectile gets destroyed when the player exits the room.")]
	[SerializeField]
	private bool m_destroyOnRoomChange = true;

	// Token: 0x0400238F RID: 9103
	[Header("Visual Settings")]
	[Tooltip("Determines whether the Visual pivot of the projectile follows the heading it's going in (i.e. does the object visually rotate to match the direction it's going in). NOTE: This field overrides Visual Rotation Speed.")]
	[SerializeField]
	private bool m_pivotFollowsOrientation;

	// Token: 0x04002390 RID: 9104
	[Tooltip("How quickly (in degrees per second) the projectile visually spins in the air. Only the Visual pivot spins, so the projectile does not actually spin. Useful for visuals like a spinning bone projectile. NOTE: If you want to rotate the projectile, use the projectile's Rotation transform.")]
	[SerializeField]
	[ConditionalHide("m_pivotFollowsOrientation", true, true)]
	private float m_visualRotationSpeed;

	// Token: 0x04002391 RID: 9105
	[Tooltip("We use velocity to determine how effects should occur when two objects interact (for ex. walking past curtains). When a projectile is fired, it uses its own velocity for the effect.  This field uses the Owner's velocity instead.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_useOwnerVelocityForEffects;

	// Token: 0x04002392 RID: 9106
	[Tooltip("Determines what type of strike effect should play when the projectile hits something.")]
	[SerializeField]
	private StrikeType m_strikeType = StrikeType.Blunt;

	// Token: 0x04002393 RID: 9107
	[Tooltip("Determines if Offscreen warnings appear for this projectile.")]
	[SerializeField]
	private bool m_disableOffscreenWarnings;

	// Token: 0x04002394 RID: 9108
	[Header("Animation Settings")]
	[SerializeField]
	[Tooltip("Setting this to true means the projectile will not be collidable until its intro has played")]
	private bool m_hasAnimStates;

	// Token: 0x04002395 RID: 9109
	[SerializeField]
	[ConditionalHide("m_hasAnimStates", true)]
	private bool m_hasIntro;

	// Token: 0x04002396 RID: 9110
	[SerializeField]
	[ConditionalHide("m_hasAnimStates", true)]
	private bool m_playOuttroOnTimeoutOnly;

	// Token: 0x04002397 RID: 9111
	[SerializeField]
	[ConditionalHide("m_hasAnimStates", true)]
	private string m_outtroTriggerName;

	// Token: 0x04002398 RID: 9112
	[SerializeField]
	[ConditionalHide("m_hasAnimStates", true)]
	private int m_animLayer;

	// Token: 0x04002399 RID: 9113
	[SerializeField]
	[Tooltip("Determines whether the projectile keeps moving during its death anim when dashed through")]
	private bool m_keepMovingWhenDashedThrough;

	// Token: 0x0400239A RID: 9114
	[Header("Collision Settings")]
	[Tooltip("Does this projectile deal dot damage. I.e. does it deal damage but not trigger invincibility.")]
	[SerializeField]
	private bool m_isDotDamage;

	// Token: 0x0400239B RID: 9115
	[Tooltip("Does this projectile deal hazard damage. This will override the projectile data's strength with Hazard_EV.GetDamageAmount().")]
	[SerializeField]
	private bool m_dealsHazardDamage;

	// Token: 0x0400239C RID: 9116
	[Tooltip("Used to determine whether we should use the Owner or the projectile as the point of collision. Mostly used for positioning the strike effect and knockback. For ex. if an enemy is knocked back, does he push away from the projectile or the Owner. NOTE: SnapToOwner might be able to replace this field entirely.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_useOwnerCollisionPoint = true;

	// Token: 0x0400239D RID: 9117
	[Tooltip("Determines whether a projectile can hit its owner or not. 99.9% of the time this is false.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_canHitOwner;

	// Token: 0x0400239E RID: 9118
	[Tooltip("Determines if the projectile can hit breakables.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_canHitBreakables;

	// Token: 0x0400239F RID: 9119
	[Tooltip("Determines if the projectile only hits flimsy breakables.")]
	[SerializeField]
	[ConditionalHide("m_canHitBreakables", true)]
	[ReadOnlyOnPlay]
	private bool m_hitsFlimsyBreakablesOnly;

	// Token: 0x040023A0 RID: 9120
	[Tooltip("Determines if the projectile is allowed to collide with walls.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_canHitWall;

	// Token: 0x040023A1 RID: 9121
	[Tooltip("Determines if the projectile is destroyed when colliding with walls.")]
	[SerializeField]
	[ConditionalHide("m_canHitWall", true)]
	private bool m_dieOnWallCollision;

	// Token: 0x040023A2 RID: 9122
	[Tooltip("Determines if the projectile is allowed to collide with walls.")]
	[SerializeField]
	[ConditionalHide("m_dieOnWallCollision", true)]
	private bool m_dieOnRoomBoundsCollision;

	// Token: 0x040023A3 RID: 9123
	[Tooltip("Determines if the projectile is destroyed when colliding with characters.  This includes breakables.")]
	[SerializeField]
	[ReadOnlyOnPlay]
	private bool m_dieOnCharacterCollision;

	// Token: 0x040023A4 RID: 9124
	[Header("Projectile-on-Projectile Collisions")]
	[Tooltip("The type of projectile I am. This must match the CanCollideWithFlag of the oncoming projectile for a collision to occur.")]
	[SerializeField]
	private ProjectileCollisionFlag m_collisionFlag;

	// Token: 0x040023A5 RID: 9125
	[Tooltip("Which projectiles I can kill. This must match the CollisionFlag of the oncoming projectile for a collision to occur. Right now only player projectiles can kill other projectiles.")]
	[SerializeField]
	private ProjectileCollisionFlag m_canCollideWithFlag;

	// Token: 0x040023A6 RID: 9126
	[Tooltip("Determines if the on-hit CD logic has a delay. Only applies to AxeSpinner")]
	[SerializeField]
	private bool m_hasCDReductionDelay;

	// Token: 0x040023A7 RID: 9127
	[Header("Physics Settings")]
	[Tooltip("Determines if the projectile falls due to gravity or not.")]
	[SerializeField]
	private bool m_isWeighted;

	// Token: 0x040023A8 RID: 9128
	[Tooltip("The amount of time before gravity kicks in for a projectile. NOTE: This setting only works if 'Is Weighted' == true.")]
	[SerializeField]
	[ConditionalHide("m_isWeighted", true)]
	private float m_gravityKickInDelay;

	// Token: 0x040023A9 RID: 9129
	[Tooltip("A modifier on how much gravity pushes down on the projectile when it is going up. NOTE: This setting only works if 'Is Weighted' == true.")]
	[SerializeField]
	[ConditionalHide("m_isWeighted", true)]
	private float m_ascentMultiplierOverride = 1f;

	// Token: 0x040023AA RID: 9130
	[Tooltip("A modifier on how much gravity pushes down on the projectile when it is going down. NOTE: This setting only works if 'Is Weighted' == true.")]
	[SerializeField]
	[ConditionalHide("m_isWeighted", true)]
	private float m_fallMultiplierOverride = 1f;

	// Token: 0x040023AB RID: 9131
	[Tooltip("Determines whether the projectile should keep sliding when it hits the ground or if it should stop. NOTE: This only works if the projectile has a CorgiController attached to control its physics (necessary for projectiles that 'bounce' off walls) and 'Is Weighted' == true.")]
	[SerializeField]
	[ConditionalHide("m_isWeighted", true)]
	private bool m_disableFriction;

	// Token: 0x040023AC RID: 9132
	[Header("Ricochet Settings")]
	[Tooltip("Certain enemies knock back the player when the Player's projectile hits the enemy. Set this to false to disable the ricochet logic (For ex. the bow's arrow projectile). NOTE: Affects Player only.")]
	[SerializeField]
	private bool m_canBeRicocheted;

	// Token: 0x040023AD RID: 9133
	[Tooltip("If this is on, the projectile will knock back the Player regardless of whether the enemy is set to ricochet the Player or not. Useful for a trait. NOTE: Only works if 'Can Be Ricocheted' == true and affects Player only.")]
	[SerializeField]
	[ConditionalHide("m_canBeRicocheted", true)]
	private bool m_ricochetsOwnerWhenHits;

	// Token: 0x040023AE RID: 9134
	[Tooltip("The knockback distance of the ricochet.")]
	[SerializeField]
	[ConditionalHide("m_canBeRicocheted", true)]
	private Vector2 m_ricochetKnockbackDistance = Player_EV.PLAYER_BASE_KNOCKBACK_DISTANCE;

	// Token: 0x040023AF RID: 9135
	[Tooltip("The control lockout duration of the ricochet.")]
	[SerializeField]
	[ConditionalHide("m_canBeRicocheted", true)]
	private float m_ricochetLockoutDuration = 0.45f;

	// Token: 0x040023B0 RID: 9136
	[Header("Misc Settings - Programmers only")]
	[Tooltip("This is purely for artists.  Do not touch.")]
	[SerializeField]
	private bool m_ignoreTrailRendererYScaleMatching;

	// Token: 0x040023B1 RID: 9137
	protected float m_lifeSpanTimer;

	// Token: 0x040023B2 RID: 9138
	protected GameObject m_owner;

	// Token: 0x040023B3 RID: 9139
	protected BaseCharacterController m_ownerController;

	// Token: 0x040023B4 RID: 9140
	protected IHitboxController m_hbController;

	// Token: 0x040023B5 RID: 9141
	private Animator m_animator;

	// Token: 0x040023B6 RID: 9142
	protected GameObject m_collidedObj;

	// Token: 0x040023B7 RID: 9143
	private PlayerController m_player;

	// Token: 0x040023B8 RID: 9144
	private CorgiController_RL m_corgiController;

	// Token: 0x040023B9 RID: 9145
	private SpriteRenderer m_spriteRenderer;

	// Token: 0x040023BA RID: 9146
	private Color m_startingColor;

	// Token: 0x040023BB RID: 9147
	private bool m_wasSpawned;

	// Token: 0x040023BC RID: 9148
	protected Vector2 m_movement;

	// Token: 0x040023BD RID: 9149
	protected Vector2 m_heading = new Vector2(1f, 0f);

	// Token: 0x040023BE RID: 9150
	protected float m_orientation;

	// Token: 0x040023BF RID: 9151
	protected Vector2 m_acceleration;

	// Token: 0x040023C0 RID: 9152
	protected float m_gravityKickInTimer;

	// Token: 0x040023C1 RID: 9153
	private Vector3 m_initialScale;

	// Token: 0x040023C2 RID: 9154
	private Vector3 m_initialRotation;

	// Token: 0x040023C3 RID: 9155
	private float m_initialRotationSpeed;

	// Token: 0x040023C4 RID: 9156
	private bool m_flagToDestroy;

	// Token: 0x040023C5 RID: 9157
	private bool m_ignoreDamageScale;

	// Token: 0x040023C6 RID: 9158
	private bool m_isDying;

	// Token: 0x040023C7 RID: 9159
	private bool m_hasVelocityParams;

	// Token: 0x040023C8 RID: 9160
	private bool m_collisionPointChanged;

	// Token: 0x040023C9 RID: 9161
	private Action<Projectile_RL, GameObject> m_ricochetPlayer;

	// Token: 0x040023CA RID: 9162
	private bool m_detachOnDestruction;

	// Token: 0x040023CB RID: 9163
	private BaseProjectileLogic[] m_projectileLogicArray;

	// Token: 0x040023CC RID: 9164
	private StatusEffectType[] m_statusEffectTypeArray;

	// Token: 0x040023CD RID: 9165
	private float[] m_statusEffectDurationArray;

	// Token: 0x040023CE RID: 9166
	private VoidDissolveComponent m_voidDissolve;

	// Token: 0x040023CF RID: 9167
	protected Projectile_RL.ProjectileDestructionType m_destructionType;

	// Token: 0x040023D0 RID: 9168
	private bool m_hasSearchedForAudioEventEmitter;

	// Token: 0x040023D1 RID: 9169
	private ProjectileEventEmitter m_audioEventEmitter;

	// Token: 0x040023D9 RID: 9177
	protected Relay<Projectile_RL, GameObject> m_onCollisionRelay = new Relay<Projectile_RL, GameObject>();

	// Token: 0x040023DA RID: 9178
	protected Relay<Projectile_RL, GameObject> m_onSpawnRelay = new Relay<Projectile_RL, GameObject>();

	// Token: 0x040023DB RID: 9179
	protected Relay<Projectile_RL, GameObject> m_onDeathRelay = new Relay<Projectile_RL, GameObject>();

	// Token: 0x040023DC RID: 9180
	protected Relay<GameObject> m_onDeathEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x040023DD RID: 9181
	protected Relay<GameObject> m_onSpawnEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x040023DE RID: 9182
	protected Relay<GameObject> m_onTimeoutEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x040023DF RID: 9183
	private Relay<IPreOnDisable> m_preOnDisableRelay = new Relay<IPreOnDisable>();

	// Token: 0x040023EE RID: 9198
	private WaitForFixedUpdate m_fixedUpdateYield = new WaitForFixedUpdate();

	// Token: 0x040023EF RID: 9199
	private Coroutine m_changeCollPointCoroutine;

	// Token: 0x040023F0 RID: 9200
	private WaitRL_Yield m_canHitOwnerWaitYield;

	// Token: 0x040023F1 RID: 9201
	private bool m_checkedForRenderer;

	// Token: 0x02000C8B RID: 3211
	protected enum ProjectileDestructionType
	{
		// Token: 0x040050E2 RID: 20706
		None,
		// Token: 0x040050E3 RID: 20707
		TerrainCollision,
		// Token: 0x040050E4 RID: 20708
		CharacterCollision,
		// Token: 0x040050E5 RID: 20709
		VoidDashCollision
	}
}
