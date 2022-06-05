using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020001BF RID: 447
public abstract class BaseCharacterController : MonoBehaviour, IHealth, IDefenseObj, IRootObj, IMidpointObj, IEffectVelocity, IPreOnDisable, IPivotObj, IEffectTriggerEvent_OnDeath, IHeading
{
	// Token: 0x17000990 RID: 2448
	// (get) Token: 0x0600115E RID: 4446 RVA: 0x0003258B File Offset: 0x0003078B
	public IRelayLink<GameObject> OnDeathEffectTriggerRelay
	{
		get
		{
			return this.m_onDeathEffectTriggerRelay.link;
		}
	}

	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x0600115F RID: 4447 RVA: 0x00032598 File Offset: 0x00030798
	public BlinkPulseEffect BlinkPulseEffect
	{
		get
		{
			return this.m_characterHitResponse.BlinkPulseEffect;
		}
	}

	// Token: 0x17000992 RID: 2450
	// (get) Token: 0x06001160 RID: 4448 RVA: 0x000325A5 File Offset: 0x000307A5
	// (set) Token: 0x06001161 RID: 4449 RVA: 0x000325AD File Offset: 0x000307AD
	public List<Renderer> RendererArray { get; protected set; }

	// Token: 0x17000993 RID: 2451
	// (get) Token: 0x06001162 RID: 4450 RVA: 0x000325B6 File Offset: 0x000307B6
	// (set) Token: 0x06001163 RID: 4451 RVA: 0x000325BE File Offset: 0x000307BE
	public List<RendererArrayEntry> RendererArrayDefaultTint { get; protected set; }

	// Token: 0x17000994 RID: 2452
	// (get) Token: 0x06001164 RID: 4452 RVA: 0x000325C7 File Offset: 0x000307C7
	// (set) Token: 0x06001165 RID: 4453 RVA: 0x000325CF File Offset: 0x000307CF
	public bool UseOverrideDefaultTint { get; set; }

	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x06001166 RID: 4454 RVA: 0x000325D8 File Offset: 0x000307D8
	// (set) Token: 0x06001167 RID: 4455 RVA: 0x000325E0 File Offset: 0x000307E0
	public Color DefaultTintOverrideColor { get; set; }

	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x06001168 RID: 4456 RVA: 0x000325E9 File Offset: 0x000307E9
	public IRelayLink<IPreOnDisable> PreOnDisableRelay
	{
		get
		{
			return this.m_onPreDisableRelay.link;
		}
	}

	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x06001169 RID: 4457 RVA: 0x000325F6 File Offset: 0x000307F6
	public Rect CollisionBounds
	{
		get
		{
			return this.m_controllerCorgi.AbsBounds;
		}
	}

	// Token: 0x17000998 RID: 2456
	// (get) Token: 0x0600116A RID: 4458 RVA: 0x00032603 File Offset: 0x00030803
	public Bounds VisualBounds
	{
		get
		{
			return this.VisualBoundsObj.Bounds;
		}
	}

	// Token: 0x17000999 RID: 2457
	// (get) Token: 0x0600116B RID: 4459 RVA: 0x00032610 File Offset: 0x00030810
	public Bounds_RL VisualBoundsObj
	{
		get
		{
			if (!this.m_useCachedVisualBounds)
			{
				this.m_useCachedVisualBounds = true;
				this.m_cachedVisualBounds = base.GetComponent<Bounds_RL>();
			}
			return this.m_cachedVisualBounds;
		}
	}

	// Token: 0x1700099A RID: 2458
	// (get) Token: 0x0600116C RID: 4460
	public abstract float BaseScaleToOffsetWith { get; }

	// Token: 0x1700099B RID: 2459
	// (get) Token: 0x0600116D RID: 4461 RVA: 0x00032633 File Offset: 0x00030833
	// (set) Token: 0x0600116E RID: 4462 RVA: 0x0003263B File Offset: 0x0003083B
	public bool TakesNoDamage
	{
		get
		{
			return this.m_takesNoDamage;
		}
		set
		{
			this.m_takesNoDamage = value;
		}
	}

	// Token: 0x1700099C RID: 2460
	// (get) Token: 0x0600116F RID: 4463 RVA: 0x00032644 File Offset: 0x00030844
	public IRelayLink<object, HealthChangeEventArgs> HealthChangeRelay
	{
		get
		{
			return this.m_healthChangeRelay.link;
		}
	}

	// Token: 0x1700099D RID: 2461
	// (get) Token: 0x06001170 RID: 4464 RVA: 0x00032651 File Offset: 0x00030851
	public IRelayLink<object, HealthChangeEventArgs> MaxHealthChangeRelay
	{
		get
		{
			return this.m_maxHealthChangeRelay.link;
		}
	}

	// Token: 0x1700099E RID: 2462
	// (get) Token: 0x06001171 RID: 4465 RVA: 0x0003265E File Offset: 0x0003085E
	public GameObject Pivot
	{
		get
		{
			return this.m_characterCorgi.CharacterModel;
		}
	}

	// Token: 0x1700099F RID: 2463
	// (get) Token: 0x06001172 RID: 4466 RVA: 0x0003266B File Offset: 0x0003086B
	public bool IsInvincible
	{
		get
		{
			return this.m_characterHitResponse.IsInvincible;
		}
	}

	// Token: 0x170009A0 RID: 2464
	// (get) Token: 0x06001173 RID: 4467 RVA: 0x00032678 File Offset: 0x00030878
	public float InvincibilityTimer
	{
		get
		{
			return this.m_characterHitResponse.InvincibleTimer;
		}
	}

	// Token: 0x170009A1 RID: 2465
	// (get) Token: 0x06001174 RID: 4468 RVA: 0x00032685 File Offset: 0x00030885
	// (set) Token: 0x06001175 RID: 4469 RVA: 0x0003268D File Offset: 0x0003088D
	public float BaseInvincibilityDuration { get; set; }

	// Token: 0x170009A2 RID: 2466
	// (get) Token: 0x06001176 RID: 4470 RVA: 0x00032696 File Offset: 0x00030896
	public float ActualInvincibilityDuration
	{
		get
		{
			return this.BaseInvincibilityDuration + this.InvincibilityDurationAdd;
		}
	}

	// Token: 0x170009A3 RID: 2467
	// (get) Token: 0x06001177 RID: 4471 RVA: 0x000326A5 File Offset: 0x000308A5
	// (set) Token: 0x06001178 RID: 4472 RVA: 0x000326AD File Offset: 0x000308AD
	public float InvincibilityDurationAdd { get; set; }

	// Token: 0x170009A4 RID: 2468
	// (get) Token: 0x06001179 RID: 4473 RVA: 0x000326B6 File Offset: 0x000308B6
	// (set) Token: 0x0600117A RID: 4474 RVA: 0x000326BE File Offset: 0x000308BE
	public float StunDuration { get; set; }

	// Token: 0x170009A5 RID: 2469
	// (get) Token: 0x0600117B RID: 4475 RVA: 0x000326C7 File Offset: 0x000308C7
	public Vector2 EffectVelocity
	{
		get
		{
			return this.Velocity;
		}
	}

	// Token: 0x170009A6 RID: 2470
	// (get) Token: 0x0600117C RID: 4476 RVA: 0x000326CF File Offset: 0x000308CF
	// (set) Token: 0x0600117D RID: 4477 RVA: 0x000326D7 File Offset: 0x000308D7
	public GameObject Visuals
	{
		get
		{
			return this.m_visuals;
		}
		set
		{
			this.m_visuals = value;
		}
	}

	// Token: 0x170009A7 RID: 2471
	// (get) Token: 0x0600117E RID: 4478 RVA: 0x000326E0 File Offset: 0x000308E0
	// (set) Token: 0x0600117F RID: 4479 RVA: 0x000326ED File Offset: 0x000308ED
	public bool LockFlip
	{
		get
		{
			return this.m_characterCorgi.LockFlip;
		}
		set
		{
			this.m_characterCorgi.LockFlip = value;
		}
	}

	// Token: 0x170009A8 RID: 2472
	// (get) Token: 0x06001180 RID: 4480 RVA: 0x000326FB File Offset: 0x000308FB
	// (set) Token: 0x06001181 RID: 4481 RVA: 0x00032703 File Offset: 0x00030903
	public float FallMultiplierOverride
	{
		get
		{
			return this.m_fallMultiplierOverride;
		}
		set
		{
			this.m_fallMultiplierOverride = value;
			if (this.m_controllerCorgi)
			{
				this.m_controllerCorgi.Parameters.FallMultiplier = value;
			}
		}
	}

	// Token: 0x170009A9 RID: 2473
	// (get) Token: 0x06001182 RID: 4482 RVA: 0x0003272A File Offset: 0x0003092A
	// (set) Token: 0x06001183 RID: 4483 RVA: 0x00032732 File Offset: 0x00030932
	public float AscentMultiplierOverride
	{
		get
		{
			return this.m_ascentMultiplierOverride;
		}
		set
		{
			this.m_ascentMultiplierOverride = value;
			if (this.m_controllerCorgi)
			{
				this.m_controllerCorgi.Parameters.AscentMultiplier = value;
			}
		}
	}

	// Token: 0x170009AA RID: 2474
	// (get) Token: 0x06001184 RID: 4484 RVA: 0x00032759 File Offset: 0x00030959
	public Vector3 Midpoint
	{
		get
		{
			if (this.IsInitialized)
			{
				return this.m_controllerCorgi.BoundsCenter;
			}
			return base.transform.position;
		}
	}

	// Token: 0x170009AB RID: 2475
	// (get) Token: 0x06001185 RID: 4485 RVA: 0x0003277A File Offset: 0x0003097A
	// (set) Token: 0x06001186 RID: 4486 RVA: 0x00032784 File Offset: 0x00030984
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

	// Token: 0x170009AC RID: 2476
	// (get) Token: 0x06001187 RID: 4487 RVA: 0x000327D2 File Offset: 0x000309D2
	// (set) Token: 0x06001188 RID: 4488 RVA: 0x000327E0 File Offset: 0x000309E0
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

	// Token: 0x170009AD RID: 2477
	// (get) Token: 0x06001189 RID: 4489 RVA: 0x00032833 File Offset: 0x00030A33
	// (set) Token: 0x0600118A RID: 4490 RVA: 0x00032840 File Offset: 0x00030A40
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

	// Token: 0x170009AE RID: 2478
	// (get) Token: 0x0600118B RID: 4491 RVA: 0x00032893 File Offset: 0x00030A93
	// (set) Token: 0x0600118C RID: 4492 RVA: 0x0003289B File Offset: 0x00030A9B
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

	// Token: 0x170009AF RID: 2479
	// (get) Token: 0x0600118D RID: 4493 RVA: 0x000328D9 File Offset: 0x00030AD9
	// (set) Token: 0x0600118E RID: 4494 RVA: 0x000328E1 File Offset: 0x00030AE1
	public virtual bool DisableFriction
	{
		get
		{
			return this.m_disableFriction;
		}
		set
		{
			this.m_disableFriction = value;
		}
	}

	// Token: 0x170009B0 RID: 2480
	// (get) Token: 0x0600118F RID: 4495 RVA: 0x000328EA File Offset: 0x00030AEA
	// (set) Token: 0x06001190 RID: 4496 RVA: 0x000328F2 File Offset: 0x00030AF2
	public float BaseScale
	{
		get
		{
			return this.m_baseScale;
		}
		set
		{
			this.m_baseScale = value;
		}
	}

	// Token: 0x170009B1 RID: 2481
	// (get) Token: 0x06001191 RID: 4497 RVA: 0x000328FB File Offset: 0x00030AFB
	public virtual float ActualScale
	{
		get
		{
			return this.BaseScale;
		}
	}

	// Token: 0x170009B2 RID: 2482
	// (get) Token: 0x06001192 RID: 4498 RVA: 0x00032903 File Offset: 0x00030B03
	// (set) Token: 0x06001193 RID: 4499 RVA: 0x0003290B File Offset: 0x00030B0B
	public virtual Vector2 InternalKnockbackMod
	{
		get
		{
			return this.m_internalKnockbackMod;
		}
		set
		{
			this.m_internalKnockbackMod.x = value.x;
			this.m_internalKnockbackMod.y = value.y;
		}
	}

	// Token: 0x170009B3 RID: 2483
	// (get) Token: 0x06001194 RID: 4500 RVA: 0x0003292F File Offset: 0x00030B2F
	// (set) Token: 0x06001195 RID: 4501 RVA: 0x00032937 File Offset: 0x00030B37
	public virtual Vector2 ExternalKnockbackMod
	{
		get
		{
			return this.m_externalKnockbackMod;
		}
		set
		{
			this.m_externalKnockbackMod.x = value.x;
			this.m_externalKnockbackMod.y = value.y;
		}
	}

	// Token: 0x170009B4 RID: 2484
	// (get) Token: 0x06001196 RID: 4502 RVA: 0x0003295B File Offset: 0x00030B5B
	// (set) Token: 0x06001197 RID: 4503 RVA: 0x00032963 File Offset: 0x00030B63
	public virtual float BaseKnockbackDefense
	{
		get
		{
			return this.m_baseKnockbackDefense;
		}
		set
		{
			this.m_baseKnockbackDefense = value;
		}
	}

	// Token: 0x170009B5 RID: 2485
	// (get) Token: 0x06001198 RID: 4504 RVA: 0x0003296C File Offset: 0x00030B6C
	public virtual float ActualKnockbackDefense
	{
		get
		{
			return this.BaseKnockbackDefense;
		}
	}

	// Token: 0x170009B6 RID: 2486
	// (get) Token: 0x06001199 RID: 4505 RVA: 0x00032974 File Offset: 0x00030B74
	// (set) Token: 0x0600119A RID: 4506 RVA: 0x0003297C File Offset: 0x00030B7C
	public virtual float BaseStunDefense
	{
		get
		{
			return this.m_baseStunDefense;
		}
		set
		{
			this.m_baseStunDefense = value;
		}
	}

	// Token: 0x170009B7 RID: 2487
	// (get) Token: 0x0600119B RID: 4507 RVA: 0x00032985 File Offset: 0x00030B85
	public virtual float ActualStunDefense
	{
		get
		{
			return this.BaseStunDefense;
		}
	}

	// Token: 0x170009B8 RID: 2488
	// (get) Token: 0x0600119C RID: 4508 RVA: 0x0003298D File Offset: 0x00030B8D
	public float BaseDamage
	{
		get
		{
			this.m_baseDamage = this.BaseStrength + this.BaseMagic;
			return this.m_baseDamage;
		}
	}

	// Token: 0x170009B9 RID: 2489
	// (get) Token: 0x0600119D RID: 4509 RVA: 0x000329A8 File Offset: 0x00030BA8
	public virtual float ActualDamage
	{
		get
		{
			return Mathf.Clamp(this.ActualStrength + this.ActualMagic, 0f, float.MaxValue);
		}
	}

	// Token: 0x170009BA RID: 2490
	// (get) Token: 0x0600119E RID: 4510 RVA: 0x000329C6 File Offset: 0x00030BC6
	public virtual float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009BB RID: 2491
	// (get) Token: 0x0600119F RID: 4511 RVA: 0x000329CD File Offset: 0x00030BCD
	public virtual float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009BC RID: 2492
	// (get) Token: 0x060011A0 RID: 4512 RVA: 0x000329D4 File Offset: 0x00030BD4
	// (set) Token: 0x060011A1 RID: 4513 RVA: 0x000329DC File Offset: 0x00030BDC
	public float BaseStrength
	{
		get
		{
			return this.m_baseStrength;
		}
		set
		{
			this.m_baseStrength = Mathf.Clamp(value, 0f, float.MaxValue);
		}
	}

	// Token: 0x170009BD RID: 2493
	// (get) Token: 0x060011A2 RID: 4514 RVA: 0x000329F4 File Offset: 0x00030BF4
	// (set) Token: 0x060011A3 RID: 4515 RVA: 0x000329FC File Offset: 0x00030BFC
	public float StrengthMod { get; protected set; }

	// Token: 0x170009BE RID: 2494
	// (get) Token: 0x060011A4 RID: 4516 RVA: 0x00032A05 File Offset: 0x00030C05
	// (set) Token: 0x060011A5 RID: 4517 RVA: 0x00032A0D File Offset: 0x00030C0D
	public float StrengthTemporaryMod { get; protected set; }

	// Token: 0x170009BF RID: 2495
	// (get) Token: 0x060011A6 RID: 4518 RVA: 0x00032A16 File Offset: 0x00030C16
	// (set) Token: 0x060011A7 RID: 4519 RVA: 0x00032A1E File Offset: 0x00030C1E
	public int StrengthAdd { get; protected set; }

	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x060011A8 RID: 4520 RVA: 0x00032A27 File Offset: 0x00030C27
	// (set) Token: 0x060011A9 RID: 4521 RVA: 0x00032A2F File Offset: 0x00030C2F
	public int StrengthTemporaryAdd { get; protected set; }

	// Token: 0x170009C1 RID: 2497
	// (get) Token: 0x060011AA RID: 4522 RVA: 0x00032A38 File Offset: 0x00030C38
	public virtual float ActualStrength
	{
		get
		{
			return Mathf.Clamp((this.BaseStrength + (float)this.StrengthAdd + (float)this.StrengthTemporaryAdd) * (1f + this.StrengthMod + this.StrengthTemporaryMod), 0f, float.MaxValue);
		}
	}

	// Token: 0x170009C2 RID: 2498
	// (get) Token: 0x060011AB RID: 4523 RVA: 0x00032A73 File Offset: 0x00030C73
	// (set) Token: 0x060011AC RID: 4524 RVA: 0x00032A7B File Offset: 0x00030C7B
	public float BaseMagic
	{
		get
		{
			return this.m_baseMagic;
		}
		set
		{
			this.m_baseMagic = Mathf.Clamp(value, 0f, float.MaxValue);
		}
	}

	// Token: 0x170009C3 RID: 2499
	// (get) Token: 0x060011AD RID: 4525 RVA: 0x00032A93 File Offset: 0x00030C93
	// (set) Token: 0x060011AE RID: 4526 RVA: 0x00032A9B File Offset: 0x00030C9B
	public float MagicMod { get; protected set; }

	// Token: 0x170009C4 RID: 2500
	// (get) Token: 0x060011AF RID: 4527 RVA: 0x00032AA4 File Offset: 0x00030CA4
	// (set) Token: 0x060011B0 RID: 4528 RVA: 0x00032AAC File Offset: 0x00030CAC
	public float MagicTemporaryMod { get; protected set; }

	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x060011B1 RID: 4529 RVA: 0x00032AB5 File Offset: 0x00030CB5
	// (set) Token: 0x060011B2 RID: 4530 RVA: 0x00032ABD File Offset: 0x00030CBD
	public int MagicAdd { get; protected set; }

	// Token: 0x170009C6 RID: 2502
	// (get) Token: 0x060011B3 RID: 4531 RVA: 0x00032AC6 File Offset: 0x00030CC6
	// (set) Token: 0x060011B4 RID: 4532 RVA: 0x00032ACE File Offset: 0x00030CCE
	public int MagicTemporaryAdd { get; protected set; }

	// Token: 0x170009C7 RID: 2503
	// (get) Token: 0x060011B5 RID: 4533 RVA: 0x00032AD7 File Offset: 0x00030CD7
	public virtual float ActualMagic
	{
		get
		{
			return Mathf.Clamp((this.BaseMagic + (float)this.MagicAdd + (float)this.MagicTemporaryAdd) * (1f + this.MagicMod + this.MagicTemporaryMod), 0f, float.MaxValue);
		}
	}

	// Token: 0x170009C8 RID: 2504
	// (get) Token: 0x060011B6 RID: 4534 RVA: 0x00032B12 File Offset: 0x00030D12
	// (set) Token: 0x060011B7 RID: 4535 RVA: 0x00032B1A File Offset: 0x00030D1A
	public virtual int BaseMaxHealth { get; protected set; }

	// Token: 0x170009C9 RID: 2505
	// (get) Token: 0x060011B8 RID: 4536 RVA: 0x00032B23 File Offset: 0x00030D23
	// (set) Token: 0x060011B9 RID: 4537 RVA: 0x00032B2B File Offset: 0x00030D2B
	public int MaxHealthAdd { get; protected set; }

	// Token: 0x170009CA RID: 2506
	// (get) Token: 0x060011BA RID: 4538 RVA: 0x00032B34 File Offset: 0x00030D34
	// (set) Token: 0x060011BB RID: 4539 RVA: 0x00032B3C File Offset: 0x00030D3C
	public int MaxHealthTemporaryAdd { get; protected set; }

	// Token: 0x170009CB RID: 2507
	// (get) Token: 0x060011BC RID: 4540 RVA: 0x00032B45 File Offset: 0x00030D45
	// (set) Token: 0x060011BD RID: 4541 RVA: 0x00032B4D File Offset: 0x00030D4D
	public float MaxHealthMod { get; protected set; }

	// Token: 0x170009CC RID: 2508
	// (get) Token: 0x060011BE RID: 4542 RVA: 0x00032B56 File Offset: 0x00030D56
	// (set) Token: 0x060011BF RID: 4543 RVA: 0x00032B5E File Offset: 0x00030D5E
	public float MaxHealthTemporaryMod { get; protected set; }

	// Token: 0x170009CD RID: 2509
	// (get) Token: 0x060011C0 RID: 4544
	public abstract int ActualMaxHealth { get; }

	// Token: 0x170009CE RID: 2510
	// (get) Token: 0x060011C1 RID: 4545 RVA: 0x00032B67 File Offset: 0x00030D67
	// (set) Token: 0x060011C2 RID: 4546 RVA: 0x00032B6F File Offset: 0x00030D6F
	public virtual float CurrentHealth { get; protected set; }

	// Token: 0x060011C3 RID: 4547 RVA: 0x00032B78 File Offset: 0x00030D78
	public virtual void SetHealth(float value, bool additive, bool runEvents)
	{
		if (this.CurrentHealth > (float)this.ActualMaxHealth)
		{
			this.CurrentHealth = (float)this.ActualMaxHealth;
		}
		float currentHealth = this.CurrentHealth;
		float num = value;
		if (additive)
		{
			num = this.CurrentHealth + value;
		}
		this.CurrentHealth = Mathf.Clamp(num, 0f, (float)this.ActualMaxHealth);
		if (runEvents)
		{
			if (this.m_healthChangeEventArgs != null)
			{
				this.m_healthChangeEventArgs.Initialise(this, num, currentHealth);
			}
			if (this.m_healthChangeRelay != null)
			{
				this.m_healthChangeRelay.Dispatch(this, this.m_healthChangeEventArgs);
			}
		}
	}

	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x060011C4 RID: 4548 RVA: 0x00032C01 File Offset: 0x00030E01
	public int CurrentHealthAsInt
	{
		get
		{
			return Mathf.CeilToInt(this.CurrentHealth);
		}
	}

	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x060011C5 RID: 4549 RVA: 0x00032C0E File Offset: 0x00030E0E
	// (set) Token: 0x060011C6 RID: 4550 RVA: 0x00032C16 File Offset: 0x00030E16
	public bool KnockedIntoAir
	{
		get
		{
			return this.m_knockedIntoAir;
		}
		set
		{
			this.m_knockedIntoAir = value;
			if (this.m_knockedIntoAir)
			{
				this.m_knockedIntoAirStartTime = Time.time;
			}
		}
	}

	// Token: 0x170009D1 RID: 2513
	// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00032C32 File Offset: 0x00030E32
	public Vector2 Velocity
	{
		get
		{
			return this.m_controllerCorgi.Velocity;
		}
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x00032C3F File Offset: 0x00030E3F
	public virtual void SetVelocity(float velocityX, float velocityY, bool additive)
	{
		this.m_speedHolder.x = velocityX;
		this.m_speedHolder.y = velocityY;
		if (additive)
		{
			this.m_controllerCorgi.AddForce(this.m_speedHolder);
			return;
		}
		this.m_controllerCorgi.SetForce(this.m_speedHolder);
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x00032C7F File Offset: 0x00030E7F
	public virtual void SetVelocityX(float velocity, bool additive)
	{
		this.m_speedHolder.x = velocity;
		if (additive)
		{
			this.m_controllerCorgi.AddHorizontalForce(velocity);
			return;
		}
		this.m_controllerCorgi.SetHorizontalForce(velocity);
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x00032CA9 File Offset: 0x00030EA9
	public virtual void SetVelocityY(float velocity, bool additive)
	{
		this.m_speedHolder.y = velocity;
		if (additive)
		{
			this.m_controllerCorgi.AddVerticalForce(velocity);
			return;
		}
		this.m_controllerCorgi.SetVerticalForce(velocity);
	}

	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x060011CB RID: 4555 RVA: 0x00032CD3 File Offset: 0x00030ED3
	public bool IsFalling
	{
		get
		{
			return this.m_controllerCorgi.State.IsFalling;
		}
	}

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x060011CC RID: 4556 RVA: 0x00032CE5 File Offset: 0x00030EE5
	public bool IsGrounded
	{
		get
		{
			return this.m_controllerCorgi.State.IsGrounded && this.CurrentHealth > 0f;
		}
	}

	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x060011CD RID: 4557 RVA: 0x00032D08 File Offset: 0x00030F08
	public bool IsFacingRight
	{
		get
		{
			return this.m_characterCorgi.IsFacingRight;
		}
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x00032D15 File Offset: 0x00030F15
	public void SetFacing(bool faceRight)
	{
		if (faceRight)
		{
			if (!this.IsFacingRight)
			{
				this.m_characterCorgi.Flip(false, false);
				return;
			}
		}
		else if (this.IsFacingRight)
		{
			this.m_characterCorgi.Flip(false, false);
		}
	}

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x060011CF RID: 4559 RVA: 0x00032D45 File Offset: 0x00030F45
	// (set) Token: 0x060011D0 RID: 4560 RVA: 0x00032D57 File Offset: 0x00030F57
	public CharacterStates.MovementStates MovementState
	{
		get
		{
			return this.m_characterCorgi.MovementState.CurrentState;
		}
		set
		{
			this.m_characterCorgi.MovementState.ChangeState(value);
		}
	}

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x060011D1 RID: 4561 RVA: 0x00032D6A File Offset: 0x00030F6A
	public CharacterStates.MovementStates PreviousMovementState
	{
		get
		{
			return this.m_characterCorgi.MovementState.PreviousState;
		}
	}

	// Token: 0x060011D2 RID: 4562 RVA: 0x00032D7C File Offset: 0x00030F7C
	[Obsolete("This is a SUPER dangerous call. Better to set the state to what you actually want")]
	public void RestorePreviousMovementState()
	{
		this.m_characterCorgi.MovementState.RestorePreviousState();
	}

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00032D8E File Offset: 0x00030F8E
	// (set) Token: 0x060011D4 RID: 4564 RVA: 0x00032DA0 File Offset: 0x00030FA0
	public CharacterStates.CharacterConditions ConditionState
	{
		get
		{
			return this.m_characterCorgi.ConditionState.CurrentState;
		}
		set
		{
			this.m_characterCorgi.ConditionState.ChangeState(value);
		}
	}

	// Token: 0x170009D8 RID: 2520
	// (get) Token: 0x060011D5 RID: 4565 RVA: 0x00032DB3 File Offset: 0x00030FB3
	[Obsolete("This is a SUPER dangerous call. Better to set the state to what you actually want")]
	public CharacterStates.CharacterConditions PreviousConditionState
	{
		get
		{
			return this.m_characterCorgi.ConditionState.PreviousState;
		}
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x00032DC5 File Offset: 0x00030FC5
	public void RestorePreviousCharacterCondition()
	{
		this.m_characterCorgi.ConditionState.RestorePreviousState();
	}

	// Token: 0x170009D9 RID: 2521
	// (get) Token: 0x060011D7 RID: 4567 RVA: 0x00032DD7 File Offset: 0x00030FD7
	public IHitboxController HitboxController
	{
		get
		{
			return this.m_hitboxController;
		}
	}

	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x060011D8 RID: 4568 RVA: 0x00032DDF File Offset: 0x00030FDF
	public SpawnPositionController SpawnPositionController
	{
		get
		{
			return this.m_spawnPositionController;
		}
	}

	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x060011D9 RID: 4569 RVA: 0x00032DE7 File Offset: 0x00030FE7
	public BaseCharacterHitResponse CharacterHitResponse
	{
		get
		{
			return this.m_characterHitResponse;
		}
	}

	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x060011DA RID: 4570 RVA: 0x00032DEF File Offset: 0x00030FEF
	public CorgiController_RL ControllerCorgi
	{
		get
		{
			return this.m_controllerCorgi;
		}
	}

	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x060011DB RID: 4571 RVA: 0x00032DF7 File Offset: 0x00030FF7
	public Character CharacterCorgi
	{
		get
		{
			return this.m_characterCorgi;
		}
	}

	// Token: 0x170009DE RID: 2526
	// (get) Token: 0x060011DC RID: 4572 RVA: 0x00032DFF File Offset: 0x00030FFF
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x170009DF RID: 2527
	// (get) Token: 0x060011DD RID: 4573 RVA: 0x00032E07 File Offset: 0x00031007
	public StatusEffectController StatusEffectController
	{
		get
		{
			return this.m_statusEffectController;
		}
	}

	// Token: 0x170009E0 RID: 2528
	// (get) Token: 0x060011DE RID: 4574 RVA: 0x00032E0F File Offset: 0x0003100F
	public StatusBarController StatusBarController
	{
		get
		{
			return this.m_statusBarController;
		}
	}

	// Token: 0x060011DF RID: 4575 RVA: 0x00032E17 File Offset: 0x00031017
	public void DisableGroundedState()
	{
		this.m_controllerCorgi.State.IsCollidingBelow = false;
		this.m_controllerCorgi.State.JustGotGrounded = false;
	}

	// Token: 0x170009E1 RID: 2529
	// (get) Token: 0x060011E0 RID: 4576 RVA: 0x00032E3B File Offset: 0x0003103B
	// (set) Token: 0x060011E1 RID: 4577 RVA: 0x00032E43 File Offset: 0x00031043
	public bool IsDead { get; protected set; }

	// Token: 0x170009E2 RID: 2530
	// (get) Token: 0x060011E2 RID: 4578 RVA: 0x00032E4C File Offset: 0x0003104C
	// (set) Token: 0x060011E3 RID: 4579 RVA: 0x00032E54 File Offset: 0x00031054
	public bool IsInitialized { get; protected set; }

	// Token: 0x060011E4 RID: 4580 RVA: 0x00032E60 File Offset: 0x00031060
	protected virtual void Awake()
	{
		if (BaseCharacterController.m_charMaterialPropertyBlock_STATIC == null)
		{
			BaseCharacterController.m_charMaterialPropertyBlock_STATIC = new MaterialPropertyBlock();
		}
		this.RendererArray = new List<Renderer>();
		this.RendererArrayDefaultTint = new List<RendererArrayEntry>();
		this.RecreateRendererArray();
		this.m_controllerCorgi = base.gameObject.GetComponent<CorgiController_RL>();
		this.m_characterCorgi = base.gameObject.GetComponent<Character>();
		this.m_characterHitResponse = base.gameObject.GetComponent<BaseCharacterHitResponse>();
		this.m_hitboxController = base.gameObject.GetComponentInChildren<IHitboxController>();
		this.m_animator = this.m_characterCorgi.CharacterAnimator;
		this.m_spawnPositionController = base.gameObject.GetComponent<SpawnPositionController>();
		this.m_statusEffectController = base.gameObject.GetComponentInChildren<StatusEffectController>();
		this.m_statusEffectController.Initialize(this);
		this.m_statusBarController = base.gameObject.GetComponentInChildren<StatusBarController>();
		this.m_healthChangeEventArgs = new HealthChangeEventArgs(this, 0f, 0f);
	}

	// Token: 0x060011E5 RID: 4581 RVA: 0x00032F44 File Offset: 0x00031144
	public void RecreateRendererArray()
	{
		if (BaseCharacterController.m_charMaterialPropertyBlock_STATIC == null)
		{
			BaseCharacterController.m_charMaterialPropertyBlock_STATIC = new MaterialPropertyBlock();
		}
		for (int i = 0; i < this.RendererArray.Count; i++)
		{
			if (this.RendererArray[i])
			{
				Renderer renderer = this.RendererArray[i];
				renderer.GetPropertyBlock(BaseCharacterController.m_charMaterialPropertyBlock_STATIC);
				RendererArrayEntry rendererArrayEntry = this.RendererArrayDefaultTint[i];
				if (rendererArrayEntry.HasAlphaColor)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetColor(ShaderID_RL._AlphaBlendColor, rendererArrayEntry.DefaultAlphaColor);
				}
				if (rendererArrayEntry.HasRimColor)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetColor(ShaderID_RL._RimLightColor, rendererArrayEntry.DefaultRimLightColor);
				}
				if (rendererArrayEntry.HasMultiplyColor)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetColor(ShaderID_RL._MultiplyColor, rendererArrayEntry.DefaultMultiplyColor);
				}
				if (rendererArrayEntry.HasAddColor)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetColor(ShaderID_RL._AddColor, rendererArrayEntry.DefaultAddColor);
				}
				if (rendererArrayEntry.HasRimBias)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimBias, rendererArrayEntry.DefaultRimBias);
				}
				if (rendererArrayEntry.HasRimScale)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimScale, rendererArrayEntry.DefaultRimScale);
				}
				renderer.SetPropertyBlock(BaseCharacterController.m_charMaterialPropertyBlock_STATIC);
			}
		}
		BaseCharacterController.m_charMaterialPropertyBlock_STATIC.Clear();
		this.RendererArray.Clear();
		this.RendererArrayDefaultTint.Clear();
		PlayerController playerController = this as PlayerController;
		if (playerController)
		{
			playerController.RangeBonusDamageCurseIndicatorGO.transform.SetParent(null);
		}
		this.Visuals.GetComponentsInChildren<Renderer>(true, this.RendererArray);
		if (playerController)
		{
			playerController.RangeBonusDamageCurseIndicatorGO.transform.SetParent(this.Visuals.transform);
			playerController.RangeBonusDamageCurseIndicatorGO.transform.localPosition = Vector3.zero;
		}
		for (int j = 0; j < this.RendererArray.Count; j++)
		{
			Renderer renderer2 = this.RendererArray[j];
			RendererArrayEntry rendererArrayEntry2 = default(RendererArrayEntry);
			Material sharedMaterial = renderer2.sharedMaterial;
			if (sharedMaterial)
			{
				rendererArrayEntry2.HasAlphaColor = sharedMaterial.HasProperty(ShaderID_RL._AlphaBlendColor);
				rendererArrayEntry2.HasRimColor = sharedMaterial.HasProperty(ShaderID_RL._RimLightColor);
				rendererArrayEntry2.HasMultiplyColor = sharedMaterial.HasProperty(ShaderID_RL._MultiplyColor);
				rendererArrayEntry2.HasAddColor = sharedMaterial.HasProperty(ShaderID_RL._AddColor);
				rendererArrayEntry2.HasRimBias = sharedMaterial.HasProperty(ShaderID_RL._RimBias);
				rendererArrayEntry2.HasRimScale = sharedMaterial.HasProperty(ShaderID_RL._RimScale);
				if (rendererArrayEntry2.HasAlphaColor)
				{
					rendererArrayEntry2.DefaultAlphaColor = sharedMaterial.GetColor(ShaderID_RL._AlphaBlendColor);
				}
				if (rendererArrayEntry2.HasRimColor)
				{
					rendererArrayEntry2.DefaultRimLightColor = sharedMaterial.GetColor(ShaderID_RL._RimLightColor);
				}
				if (rendererArrayEntry2.HasMultiplyColor)
				{
					rendererArrayEntry2.DefaultMultiplyColor = sharedMaterial.GetColor(ShaderID_RL._MultiplyColor);
				}
				if (rendererArrayEntry2.HasAddColor)
				{
					rendererArrayEntry2.DefaultAddColor = sharedMaterial.GetColor(ShaderID_RL._AddColor);
				}
				if (rendererArrayEntry2.HasRimBias)
				{
					rendererArrayEntry2.DefaultRimBias = sharedMaterial.GetFloat(ShaderID_RL._RimBias);
				}
				if (rendererArrayEntry2.HasRimScale)
				{
					rendererArrayEntry2.DefaultRimScale = sharedMaterial.GetFloat(ShaderID_RL._RimScale);
				}
			}
			this.RendererArrayDefaultTint.Add(rendererArrayEntry2);
		}
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x00033264 File Offset: 0x00031464
	public void ResetRendererArrayColor()
	{
		this.UseOverrideDefaultTint = false;
		if (this.UseOverrideDefaultTint)
		{
			for (int i = 0; i < this.RendererArray.Count; i++)
			{
				Renderer renderer = this.RendererArray[i];
				if (!renderer || BaseCharacterController.m_charMaterialPropertyBlock_STATIC == null)
				{
					this.PrintOutMatError(renderer, i);
				}
				if (renderer.sharedMaterial)
				{
					renderer.GetPropertyBlock(BaseCharacterController.m_charMaterialPropertyBlock_STATIC);
					RendererArrayEntry rendererArrayEntry = this.RendererArrayDefaultTint[i];
					if (rendererArrayEntry.HasAlphaColor)
					{
						BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetColor(ShaderID_RL._AlphaBlendColor, this.DefaultTintOverrideColor);
					}
					if (rendererArrayEntry.HasRimColor)
					{
						BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetColor(ShaderID_RL._RimLightColor, this.DefaultTintOverrideColor);
					}
					if (rendererArrayEntry.HasRimBias)
					{
						BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimBias, 0f);
					}
					if (rendererArrayEntry.HasRimScale)
					{
						BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimLightColor, 0f);
					}
					renderer.SetPropertyBlock(BaseCharacterController.m_charMaterialPropertyBlock_STATIC);
				}
			}
			return;
		}
		for (int j = 0; j < this.RendererArray.Count; j++)
		{
			Renderer renderer2 = this.RendererArray[j];
			if (!renderer2 || BaseCharacterController.m_charMaterialPropertyBlock_STATIC == null)
			{
				this.PrintOutMatError(renderer2, j);
			}
			if (renderer2.sharedMaterial)
			{
				renderer2.GetPropertyBlock(BaseCharacterController.m_charMaterialPropertyBlock_STATIC);
				RendererArrayEntry rendererArrayEntry2 = this.RendererArrayDefaultTint[j];
				if (rendererArrayEntry2.HasAlphaColor)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetColor(ShaderID_RL._AlphaBlendColor, rendererArrayEntry2.DefaultAlphaColor);
				}
				if (rendererArrayEntry2.HasRimColor)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetColor(ShaderID_RL._RimLightColor, rendererArrayEntry2.DefaultRimLightColor);
				}
				if (rendererArrayEntry2.HasMultiplyColor)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetColor(ShaderID_RL._MultiplyColor, rendererArrayEntry2.DefaultMultiplyColor);
				}
				if (rendererArrayEntry2.HasAddColor)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetColor(ShaderID_RL._AddColor, rendererArrayEntry2.DefaultAddColor);
				}
				if (rendererArrayEntry2.HasRimBias)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimBias, rendererArrayEntry2.DefaultRimBias);
				}
				if (rendererArrayEntry2.HasRimScale)
				{
					BaseCharacterController.m_charMaterialPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimScale, rendererArrayEntry2.DefaultRimScale);
				}
				renderer2.SetPropertyBlock(BaseCharacterController.m_charMaterialPropertyBlock_STATIC);
			}
		}
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x00033498 File Offset: 0x00031698
	private void PrintOutMatError(Renderer renderer, int rendererIndex)
	{
		Debug.Log("MAT ERROR!");
		Debug.Log("ObjectName: " + base.name);
		Debug.Log("RENDERER NULL? " + (!renderer).ToString());
		string str = "MAT BLOCK NULL? ";
		MaterialPropertyBlock charMaterialPropertyBlock_STATIC = BaseCharacterController.m_charMaterialPropertyBlock_STATIC;
		Debug.Log(str + ((charMaterialPropertyBlock_STATIC != null) ? charMaterialPropertyBlock_STATIC.ToString() : null) == null);
		if (!renderer)
		{
			Debug.Log("Renderer null on index: " + rendererIndex.ToString());
			Debug.Log("Renderer Array size: " + this.RendererArray.Count.ToString());
		}
		Debug.Log("Crashed on Player Info: ");
		Debug.Log("Class Type: " + SaveManager.PlayerSaveData.CurrentCharacter.ClassType.ToString());
		Debug.Log("Traits 1: " + SaveManager.PlayerSaveData.CurrentCharacter.TraitOne.ToString());
		Debug.Log("Traits 2: " + SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo.ToString());
		Debug.Log("Equipped Weapon: " + SaveManager.PlayerSaveData.CurrentCharacter.EdgeEquipmentType.ToString());
		Debug.Log("Equipped Chest: " + SaveManager.PlayerSaveData.CurrentCharacter.EdgeEquipmentType.ToString());
		Debug.Log("Equipped Helm: " + SaveManager.PlayerSaveData.CurrentCharacter.EdgeEquipmentType.ToString());
		Debug.Log("Equipped Cape: " + SaveManager.PlayerSaveData.CurrentCharacter.EdgeEquipmentType.ToString());
		Debug.Log("Equipped Trinket: " + SaveManager.PlayerSaveData.CurrentCharacter.EdgeEquipmentType.ToString());
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x0003369E File Offset: 0x0003189E
	protected virtual IEnumerator Start()
	{
		while (!this.m_controllerCorgi.IsInitialized || !this.m_characterCorgi.IsInitialized)
		{
			yield return null;
		}
		this.ResetCharacter();
		this.IsInitialized = true;
		yield break;
	}

	// Token: 0x060011E9 RID: 4585 RVA: 0x000336B0 File Offset: 0x000318B0
	protected virtual void FixedUpdate()
	{
		if (!this.IsInitialized)
		{
			return;
		}
		if (!this.m_controllerCorgi)
		{
			return;
		}
		bool flag = false;
		if (this.IsGrounded && this.KnockedIntoAir && this.m_knockedIntoAirStartTime + 0.1f < Time.time)
		{
			this.KnockedIntoAir = false;
			flag = true;
		}
		bool flag2 = this.IsGrounded && !this.m_wasGroundedLastFixedUpdate;
		if (flag2)
		{
			this.OnJustGrounded();
		}
		if ((flag2 || flag) && this.Velocity.x != 0f && (this.Velocity.y <= 0f || (this.Velocity.y > 0f && this.IsGrounded && this.m_controllerCorgi.State.BelowSlopeAngle != 0f)) && !this.DisableFriction && this.m_knockedIntoAirStartTime + 0.1f < Time.time)
		{
			this.SetVelocityX(0f, false);
		}
		this.m_wasGroundedLastFixedUpdate = this.IsGrounded;
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x000337AC File Offset: 0x000319AC
	protected virtual void OnJustGrounded()
	{
	}

	// Token: 0x060011EB RID: 4587
	public abstract float CalculateDamageTaken(IDamageObj damageObj, out CriticalStrikeType critType, out float damageBlocked, float damageOverride = -1f, bool trueDamage = false, bool pureCalculation = true);

	// Token: 0x060011EC RID: 4588 RVA: 0x000337AE File Offset: 0x000319AE
	public virtual void KillCharacter(GameObject killer, bool broadcastEvent)
	{
		this.IsDead = true;
		this.StatusEffectController.StopAllStatusEffects(false);
		this.ConditionState = CharacterStates.CharacterConditions.Dead;
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x000337CA File Offset: 0x000319CA
	public virtual void ResetBaseValues()
	{
		if (this.IsInitialized)
		{
			this.AscentMultiplierOverride = this.m_ascentMultiplierOverride;
			this.FallMultiplierOverride = this.m_fallMultiplierOverride;
		}
		this.ExternalKnockbackMod = Vector2.one;
		this.InternalKnockbackMod = Vector2.one;
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x00033804 File Offset: 0x00031A04
	public virtual void ResetStates()
	{
		if (this.m_characterCorgi && this.m_characterCorgi.IsInitialized)
		{
			this.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		this.LockFlip = false;
		if (this.m_characterHitResponse.IsStunned)
		{
			this.m_characterHitResponse.StopCharacterStunned();
		}
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x00033854 File Offset: 0x00031A54
	public virtual void ResetMods()
	{
		this.StrengthMod = 0f;
		this.StrengthAdd = 0;
		this.StrengthTemporaryAdd = 0;
		this.StrengthTemporaryMod = 0f;
		this.MagicMod = 0f;
		this.MagicAdd = 0;
		this.MagicTemporaryAdd = 0;
		this.MagicTemporaryMod = 0f;
		this.MaxHealthMod = 0f;
		this.MaxHealthAdd = 0;
		this.MaxHealthTemporaryAdd = 0;
		this.MaxHealthTemporaryMod = 0f;
	}

	// Token: 0x060011F0 RID: 4592 RVA: 0x000338CD File Offset: 0x00031ACD
	public virtual void ResetCharacter()
	{
		this.IsDead = false;
		this.ResetStates();
		this.ResetBaseValues();
		this.ResetMods();
		this.ResetHealth();
		this.StatusEffectController.StopAllStatusEffects(true);
		this.StatusBarController.ResetPositionAndScale(this);
		this.DisableFriction = false;
	}

	// Token: 0x060011F1 RID: 4593 RVA: 0x0003390D File Offset: 0x00031B0D
	public virtual void ResetHealth()
	{
		this.SetHealth((float)this.ActualMaxHealth, false, true);
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x0003391E File Offset: 0x00031B1E
	public void ResetIsDead()
	{
		this.IsDead = false;
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x000339A8 File Offset: 0x00031BA8
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060011F5 RID: 4597 RVA: 0x000339B0 File Offset: 0x00031BB0
	GameObject IEffectVelocity.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060011F6 RID: 4598 RVA: 0x000339B8 File Offset: 0x00031BB8
	GameObject IPreOnDisable.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400124D RID: 4685
	private float m_baseScale;

	// Token: 0x0400124E RID: 4686
	private float m_baseStrength;

	// Token: 0x0400124F RID: 4687
	private float m_baseKnockbackDefense;

	// Token: 0x04001250 RID: 4688
	private float m_baseStunDefense;

	// Token: 0x04001251 RID: 4689
	private float m_baseMagic;

	// Token: 0x04001252 RID: 4690
	private float m_baseDamage;

	// Token: 0x04001253 RID: 4691
	protected bool m_disableFriction;

	// Token: 0x04001254 RID: 4692
	[SerializeField]
	[HideInInspector]
	private float m_fallMultiplierOverride = 1f;

	// Token: 0x04001255 RID: 4693
	[SerializeField]
	[HideInInspector]
	private float m_ascentMultiplierOverride = 1f;

	// Token: 0x04001256 RID: 4694
	[SerializeField]
	private GameObject m_visuals;

	// Token: 0x04001257 RID: 4695
	[SerializeField]
	private bool m_takesNoDamage;

	// Token: 0x04001258 RID: 4696
	private static MaterialPropertyBlock m_charMaterialPropertyBlock_STATIC;

	// Token: 0x04001259 RID: 4697
	protected CorgiController_RL m_controllerCorgi;

	// Token: 0x0400125A RID: 4698
	protected BaseCharacterHitResponse m_characterHitResponse;

	// Token: 0x0400125B RID: 4699
	protected Character m_characterCorgi;

	// Token: 0x0400125C RID: 4700
	protected SpawnPositionController m_spawnPositionController;

	// Token: 0x0400125D RID: 4701
	protected StatusEffectController m_statusEffectController;

	// Token: 0x0400125E RID: 4702
	protected StatusBarController m_statusBarController;

	// Token: 0x0400125F RID: 4703
	protected IHitboxController m_hitboxController;

	// Token: 0x04001260 RID: 4704
	protected Animator m_animator;

	// Token: 0x04001261 RID: 4705
	private Vector2 m_heading = new Vector2(1f, 0f);

	// Token: 0x04001262 RID: 4706
	private float m_orientation;

	// Token: 0x04001263 RID: 4707
	private bool m_knockedIntoAir;

	// Token: 0x04001264 RID: 4708
	private float m_knockedIntoAirStartTime;

	// Token: 0x04001265 RID: 4709
	private bool m_wasGroundedLastFixedUpdate;

	// Token: 0x04001266 RID: 4710
	private Vector2 m_internalKnockbackMod = Vector2.one;

	// Token: 0x04001267 RID: 4711
	private Vector2 m_externalKnockbackMod = Vector2.one;

	// Token: 0x04001268 RID: 4712
	protected HealthChangeEventArgs m_healthChangeEventArgs;

	// Token: 0x04001269 RID: 4713
	private bool m_useCachedVisualBounds;

	// Token: 0x0400126A RID: 4714
	private Bounds_RL m_cachedVisualBounds;

	// Token: 0x0400126B RID: 4715
	protected Relay<GameObject> m_onDeathEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x04001270 RID: 4720
	protected Relay<IPreOnDisable> m_onPreDisableRelay = new Relay<IPreOnDisable>();

	// Token: 0x04001271 RID: 4721
	private Relay<object, HealthChangeEventArgs> m_healthChangeRelay = new Relay<object, HealthChangeEventArgs>();

	// Token: 0x04001272 RID: 4722
	private Relay<object, HealthChangeEventArgs> m_maxHealthChangeRelay = new Relay<object, HealthChangeEventArgs>();

	// Token: 0x04001284 RID: 4740
	protected Vector2 m_speedHolder;
}
