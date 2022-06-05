using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000330 RID: 816
public abstract class BaseCharacterController : MonoBehaviour, IHealth, IDefenseObj, IRootObj, IMidpointObj, IEffectVelocity, IPreOnDisable, IPivotObj, IEffectTriggerEvent_OnDeath, IHeading
{
	// Token: 0x17000C52 RID: 3154
	// (get) Token: 0x060019B0 RID: 6576 RVA: 0x0000D041 File Offset: 0x0000B241
	public IRelayLink<GameObject> OnDeathEffectTriggerRelay
	{
		get
		{
			return this.m_onDeathEffectTriggerRelay.link;
		}
	}

	// Token: 0x17000C53 RID: 3155
	// (get) Token: 0x060019B1 RID: 6577 RVA: 0x0000D04E File Offset: 0x0000B24E
	public BlinkPulseEffect BlinkPulseEffect
	{
		get
		{
			return this.m_characterHitResponse.BlinkPulseEffect;
		}
	}

	// Token: 0x17000C54 RID: 3156
	// (get) Token: 0x060019B2 RID: 6578 RVA: 0x0000D05B File Offset: 0x0000B25B
	// (set) Token: 0x060019B3 RID: 6579 RVA: 0x0000D063 File Offset: 0x0000B263
	public List<Renderer> RendererArray { get; protected set; }

	// Token: 0x17000C55 RID: 3157
	// (get) Token: 0x060019B4 RID: 6580 RVA: 0x0000D06C File Offset: 0x0000B26C
	// (set) Token: 0x060019B5 RID: 6581 RVA: 0x0000D074 File Offset: 0x0000B274
	public List<RendererArrayEntry> RendererArrayDefaultTint { get; protected set; }

	// Token: 0x17000C56 RID: 3158
	// (get) Token: 0x060019B6 RID: 6582 RVA: 0x0000D07D File Offset: 0x0000B27D
	// (set) Token: 0x060019B7 RID: 6583 RVA: 0x0000D085 File Offset: 0x0000B285
	public bool UseOverrideDefaultTint { get; set; }

	// Token: 0x17000C57 RID: 3159
	// (get) Token: 0x060019B8 RID: 6584 RVA: 0x0000D08E File Offset: 0x0000B28E
	// (set) Token: 0x060019B9 RID: 6585 RVA: 0x0000D096 File Offset: 0x0000B296
	public Color DefaultTintOverrideColor { get; set; }

	// Token: 0x17000C58 RID: 3160
	// (get) Token: 0x060019BA RID: 6586 RVA: 0x0000D09F File Offset: 0x0000B29F
	public IRelayLink<IPreOnDisable> PreOnDisableRelay
	{
		get
		{
			return this.m_onPreDisableRelay.link;
		}
	}

	// Token: 0x17000C59 RID: 3161
	// (get) Token: 0x060019BB RID: 6587 RVA: 0x0000D0AC File Offset: 0x0000B2AC
	public Rect CollisionBounds
	{
		get
		{
			return this.m_controllerCorgi.AbsBounds;
		}
	}

	// Token: 0x17000C5A RID: 3162
	// (get) Token: 0x060019BC RID: 6588 RVA: 0x0000D0B9 File Offset: 0x0000B2B9
	public Bounds VisualBounds
	{
		get
		{
			return this.VisualBoundsObj.Bounds;
		}
	}

	// Token: 0x17000C5B RID: 3163
	// (get) Token: 0x060019BD RID: 6589 RVA: 0x0000D0C6 File Offset: 0x0000B2C6
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

	// Token: 0x17000C5C RID: 3164
	// (get) Token: 0x060019BE RID: 6590
	public abstract float BaseScaleToOffsetWith { get; }

	// Token: 0x17000C5D RID: 3165
	// (get) Token: 0x060019BF RID: 6591 RVA: 0x0000D0E9 File Offset: 0x0000B2E9
	// (set) Token: 0x060019C0 RID: 6592 RVA: 0x0000D0F1 File Offset: 0x0000B2F1
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

	// Token: 0x17000C5E RID: 3166
	// (get) Token: 0x060019C1 RID: 6593 RVA: 0x0000D0FA File Offset: 0x0000B2FA
	public IRelayLink<object, HealthChangeEventArgs> HealthChangeRelay
	{
		get
		{
			return this.m_healthChangeRelay.link;
		}
	}

	// Token: 0x17000C5F RID: 3167
	// (get) Token: 0x060019C2 RID: 6594 RVA: 0x0000D107 File Offset: 0x0000B307
	public IRelayLink<object, HealthChangeEventArgs> MaxHealthChangeRelay
	{
		get
		{
			return this.m_maxHealthChangeRelay.link;
		}
	}

	// Token: 0x17000C60 RID: 3168
	// (get) Token: 0x060019C3 RID: 6595 RVA: 0x0000D114 File Offset: 0x0000B314
	public GameObject Pivot
	{
		get
		{
			return this.m_characterCorgi.CharacterModel;
		}
	}

	// Token: 0x17000C61 RID: 3169
	// (get) Token: 0x060019C4 RID: 6596 RVA: 0x0000D121 File Offset: 0x0000B321
	public bool IsInvincible
	{
		get
		{
			return this.m_characterHitResponse.IsInvincible;
		}
	}

	// Token: 0x17000C62 RID: 3170
	// (get) Token: 0x060019C5 RID: 6597 RVA: 0x0000D12E File Offset: 0x0000B32E
	public float InvincibilityTimer
	{
		get
		{
			return this.m_characterHitResponse.InvincibleTimer;
		}
	}

	// Token: 0x17000C63 RID: 3171
	// (get) Token: 0x060019C6 RID: 6598 RVA: 0x0000D13B File Offset: 0x0000B33B
	// (set) Token: 0x060019C7 RID: 6599 RVA: 0x0000D143 File Offset: 0x0000B343
	public float BaseInvincibilityDuration { get; set; }

	// Token: 0x17000C64 RID: 3172
	// (get) Token: 0x060019C8 RID: 6600 RVA: 0x0000D14C File Offset: 0x0000B34C
	public float ActualInvincibilityDuration
	{
		get
		{
			return this.BaseInvincibilityDuration + this.InvincibilityDurationAdd;
		}
	}

	// Token: 0x17000C65 RID: 3173
	// (get) Token: 0x060019C9 RID: 6601 RVA: 0x0000D15B File Offset: 0x0000B35B
	// (set) Token: 0x060019CA RID: 6602 RVA: 0x0000D163 File Offset: 0x0000B363
	public float InvincibilityDurationAdd { get; set; }

	// Token: 0x17000C66 RID: 3174
	// (get) Token: 0x060019CB RID: 6603 RVA: 0x0000D16C File Offset: 0x0000B36C
	// (set) Token: 0x060019CC RID: 6604 RVA: 0x0000D174 File Offset: 0x0000B374
	public float StunDuration { get; set; }

	// Token: 0x17000C67 RID: 3175
	// (get) Token: 0x060019CD RID: 6605 RVA: 0x0000D17D File Offset: 0x0000B37D
	public Vector2 EffectVelocity
	{
		get
		{
			return this.Velocity;
		}
	}

	// Token: 0x17000C68 RID: 3176
	// (get) Token: 0x060019CE RID: 6606 RVA: 0x0000D185 File Offset: 0x0000B385
	// (set) Token: 0x060019CF RID: 6607 RVA: 0x0000D18D File Offset: 0x0000B38D
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

	// Token: 0x17000C69 RID: 3177
	// (get) Token: 0x060019D0 RID: 6608 RVA: 0x0000D196 File Offset: 0x0000B396
	// (set) Token: 0x060019D1 RID: 6609 RVA: 0x0000D1A3 File Offset: 0x0000B3A3
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

	// Token: 0x17000C6A RID: 3178
	// (get) Token: 0x060019D2 RID: 6610 RVA: 0x0000D1B1 File Offset: 0x0000B3B1
	// (set) Token: 0x060019D3 RID: 6611 RVA: 0x0000D1B9 File Offset: 0x0000B3B9
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

	// Token: 0x17000C6B RID: 3179
	// (get) Token: 0x060019D4 RID: 6612 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
	// (set) Token: 0x060019D5 RID: 6613 RVA: 0x0000D1E8 File Offset: 0x0000B3E8
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

	// Token: 0x17000C6C RID: 3180
	// (get) Token: 0x060019D6 RID: 6614 RVA: 0x0000D20F File Offset: 0x0000B40F
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

	// Token: 0x17000C6D RID: 3181
	// (get) Token: 0x060019D7 RID: 6615 RVA: 0x0000D230 File Offset: 0x0000B430
	// (set) Token: 0x060019D8 RID: 6616 RVA: 0x000907C8 File Offset: 0x0008E9C8
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

	// Token: 0x17000C6E RID: 3182
	// (get) Token: 0x060019D9 RID: 6617 RVA: 0x0000D238 File Offset: 0x0000B438
	// (set) Token: 0x060019DA RID: 6618 RVA: 0x00090818 File Offset: 0x0008EA18
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

	// Token: 0x17000C6F RID: 3183
	// (get) Token: 0x060019DB RID: 6619 RVA: 0x0000D245 File Offset: 0x0000B445
	// (set) Token: 0x060019DC RID: 6620 RVA: 0x0009086C File Offset: 0x0008EA6C
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

	// Token: 0x17000C70 RID: 3184
	// (get) Token: 0x060019DD RID: 6621 RVA: 0x0000D252 File Offset: 0x0000B452
	// (set) Token: 0x060019DE RID: 6622 RVA: 0x0000D25A File Offset: 0x0000B45A
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

	// Token: 0x17000C71 RID: 3185
	// (get) Token: 0x060019DF RID: 6623 RVA: 0x0000D298 File Offset: 0x0000B498
	// (set) Token: 0x060019E0 RID: 6624 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
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

	// Token: 0x17000C72 RID: 3186
	// (get) Token: 0x060019E1 RID: 6625 RVA: 0x0000D2A9 File Offset: 0x0000B4A9
	// (set) Token: 0x060019E2 RID: 6626 RVA: 0x0000D2B1 File Offset: 0x0000B4B1
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

	// Token: 0x17000C73 RID: 3187
	// (get) Token: 0x060019E3 RID: 6627 RVA: 0x0000D2BA File Offset: 0x0000B4BA
	public virtual float ActualScale
	{
		get
		{
			return this.BaseScale;
		}
	}

	// Token: 0x17000C74 RID: 3188
	// (get) Token: 0x060019E4 RID: 6628 RVA: 0x0000D2C2 File Offset: 0x0000B4C2
	// (set) Token: 0x060019E5 RID: 6629 RVA: 0x0000D2CA File Offset: 0x0000B4CA
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

	// Token: 0x17000C75 RID: 3189
	// (get) Token: 0x060019E6 RID: 6630 RVA: 0x0000D2EE File Offset: 0x0000B4EE
	// (set) Token: 0x060019E7 RID: 6631 RVA: 0x0000D2F6 File Offset: 0x0000B4F6
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

	// Token: 0x17000C76 RID: 3190
	// (get) Token: 0x060019E8 RID: 6632 RVA: 0x0000D31A File Offset: 0x0000B51A
	// (set) Token: 0x060019E9 RID: 6633 RVA: 0x0000D322 File Offset: 0x0000B522
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

	// Token: 0x17000C77 RID: 3191
	// (get) Token: 0x060019EA RID: 6634 RVA: 0x0000D32B File Offset: 0x0000B52B
	public virtual float ActualKnockbackDefense
	{
		get
		{
			return this.BaseKnockbackDefense;
		}
	}

	// Token: 0x17000C78 RID: 3192
	// (get) Token: 0x060019EB RID: 6635 RVA: 0x0000D333 File Offset: 0x0000B533
	// (set) Token: 0x060019EC RID: 6636 RVA: 0x0000D33B File Offset: 0x0000B53B
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

	// Token: 0x17000C79 RID: 3193
	// (get) Token: 0x060019ED RID: 6637 RVA: 0x0000D344 File Offset: 0x0000B544
	public virtual float ActualStunDefense
	{
		get
		{
			return this.BaseStunDefense;
		}
	}

	// Token: 0x17000C7A RID: 3194
	// (get) Token: 0x060019EE RID: 6638 RVA: 0x0000D34C File Offset: 0x0000B54C
	public float BaseDamage
	{
		get
		{
			this.m_baseDamage = this.BaseStrength + this.BaseMagic;
			return this.m_baseDamage;
		}
	}

	// Token: 0x17000C7B RID: 3195
	// (get) Token: 0x060019EF RID: 6639 RVA: 0x0000D367 File Offset: 0x0000B567
	public virtual float ActualDamage
	{
		get
		{
			return Mathf.Clamp(this.ActualStrength + this.ActualMagic, 0f, float.MaxValue);
		}
	}

	// Token: 0x17000C7C RID: 3196
	// (get) Token: 0x060019F0 RID: 6640 RVA: 0x00003CCB File Offset: 0x00001ECB
	public virtual float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C7D RID: 3197
	// (get) Token: 0x060019F1 RID: 6641 RVA: 0x00003CCB File Offset: 0x00001ECB
	public virtual float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C7E RID: 3198
	// (get) Token: 0x060019F2 RID: 6642 RVA: 0x0000D385 File Offset: 0x0000B585
	// (set) Token: 0x060019F3 RID: 6643 RVA: 0x0000D38D File Offset: 0x0000B58D
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

	// Token: 0x17000C7F RID: 3199
	// (get) Token: 0x060019F4 RID: 6644 RVA: 0x0000D3A5 File Offset: 0x0000B5A5
	// (set) Token: 0x060019F5 RID: 6645 RVA: 0x0000D3AD File Offset: 0x0000B5AD
	public float StrengthMod { get; protected set; }

	// Token: 0x17000C80 RID: 3200
	// (get) Token: 0x060019F6 RID: 6646 RVA: 0x0000D3B6 File Offset: 0x0000B5B6
	// (set) Token: 0x060019F7 RID: 6647 RVA: 0x0000D3BE File Offset: 0x0000B5BE
	public float StrengthTemporaryMod { get; protected set; }

	// Token: 0x17000C81 RID: 3201
	// (get) Token: 0x060019F8 RID: 6648 RVA: 0x0000D3C7 File Offset: 0x0000B5C7
	// (set) Token: 0x060019F9 RID: 6649 RVA: 0x0000D3CF File Offset: 0x0000B5CF
	public int StrengthAdd { get; protected set; }

	// Token: 0x17000C82 RID: 3202
	// (get) Token: 0x060019FA RID: 6650 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
	// (set) Token: 0x060019FB RID: 6651 RVA: 0x0000D3E0 File Offset: 0x0000B5E0
	public int StrengthTemporaryAdd { get; protected set; }

	// Token: 0x17000C83 RID: 3203
	// (get) Token: 0x060019FC RID: 6652 RVA: 0x0000D3E9 File Offset: 0x0000B5E9
	public virtual float ActualStrength
	{
		get
		{
			return Mathf.Clamp((this.BaseStrength + (float)this.StrengthAdd + (float)this.StrengthTemporaryAdd) * (1f + this.StrengthMod + this.StrengthTemporaryMod), 0f, float.MaxValue);
		}
	}

	// Token: 0x17000C84 RID: 3204
	// (get) Token: 0x060019FD RID: 6653 RVA: 0x0000D424 File Offset: 0x0000B624
	// (set) Token: 0x060019FE RID: 6654 RVA: 0x0000D42C File Offset: 0x0000B62C
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

	// Token: 0x17000C85 RID: 3205
	// (get) Token: 0x060019FF RID: 6655 RVA: 0x0000D444 File Offset: 0x0000B644
	// (set) Token: 0x06001A00 RID: 6656 RVA: 0x0000D44C File Offset: 0x0000B64C
	public float MagicMod { get; protected set; }

	// Token: 0x17000C86 RID: 3206
	// (get) Token: 0x06001A01 RID: 6657 RVA: 0x0000D455 File Offset: 0x0000B655
	// (set) Token: 0x06001A02 RID: 6658 RVA: 0x0000D45D File Offset: 0x0000B65D
	public float MagicTemporaryMod { get; protected set; }

	// Token: 0x17000C87 RID: 3207
	// (get) Token: 0x06001A03 RID: 6659 RVA: 0x0000D466 File Offset: 0x0000B666
	// (set) Token: 0x06001A04 RID: 6660 RVA: 0x0000D46E File Offset: 0x0000B66E
	public int MagicAdd { get; protected set; }

	// Token: 0x17000C88 RID: 3208
	// (get) Token: 0x06001A05 RID: 6661 RVA: 0x0000D477 File Offset: 0x0000B677
	// (set) Token: 0x06001A06 RID: 6662 RVA: 0x0000D47F File Offset: 0x0000B67F
	public int MagicTemporaryAdd { get; protected set; }

	// Token: 0x17000C89 RID: 3209
	// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0000D488 File Offset: 0x0000B688
	public virtual float ActualMagic
	{
		get
		{
			return Mathf.Clamp((this.BaseMagic + (float)this.MagicAdd + (float)this.MagicTemporaryAdd) * (1f + this.MagicMod + this.MagicTemporaryMod), 0f, float.MaxValue);
		}
	}

	// Token: 0x17000C8A RID: 3210
	// (get) Token: 0x06001A08 RID: 6664 RVA: 0x0000D4C3 File Offset: 0x0000B6C3
	// (set) Token: 0x06001A09 RID: 6665 RVA: 0x0000D4CB File Offset: 0x0000B6CB
	public virtual int BaseMaxHealth { get; protected set; }

	// Token: 0x17000C8B RID: 3211
	// (get) Token: 0x06001A0A RID: 6666 RVA: 0x0000D4D4 File Offset: 0x0000B6D4
	// (set) Token: 0x06001A0B RID: 6667 RVA: 0x0000D4DC File Offset: 0x0000B6DC
	public int MaxHealthAdd { get; protected set; }

	// Token: 0x17000C8C RID: 3212
	// (get) Token: 0x06001A0C RID: 6668 RVA: 0x0000D4E5 File Offset: 0x0000B6E5
	// (set) Token: 0x06001A0D RID: 6669 RVA: 0x0000D4ED File Offset: 0x0000B6ED
	public int MaxHealthTemporaryAdd { get; protected set; }

	// Token: 0x17000C8D RID: 3213
	// (get) Token: 0x06001A0E RID: 6670 RVA: 0x0000D4F6 File Offset: 0x0000B6F6
	// (set) Token: 0x06001A0F RID: 6671 RVA: 0x0000D4FE File Offset: 0x0000B6FE
	public float MaxHealthMod { get; protected set; }

	// Token: 0x17000C8E RID: 3214
	// (get) Token: 0x06001A10 RID: 6672 RVA: 0x0000D507 File Offset: 0x0000B707
	// (set) Token: 0x06001A11 RID: 6673 RVA: 0x0000D50F File Offset: 0x0000B70F
	public float MaxHealthTemporaryMod { get; protected set; }

	// Token: 0x17000C8F RID: 3215
	// (get) Token: 0x06001A12 RID: 6674
	public abstract int ActualMaxHealth { get; }

	// Token: 0x17000C90 RID: 3216
	// (get) Token: 0x06001A13 RID: 6675 RVA: 0x0000D518 File Offset: 0x0000B718
	// (set) Token: 0x06001A14 RID: 6676 RVA: 0x0000D520 File Offset: 0x0000B720
	public virtual float CurrentHealth { get; protected set; }

	// Token: 0x06001A15 RID: 6677 RVA: 0x000908C0 File Offset: 0x0008EAC0
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

	// Token: 0x17000C91 RID: 3217
	// (get) Token: 0x06001A16 RID: 6678 RVA: 0x0000D529 File Offset: 0x0000B729
	public int CurrentHealthAsInt
	{
		get
		{
			return Mathf.CeilToInt(this.CurrentHealth);
		}
	}

	// Token: 0x17000C92 RID: 3218
	// (get) Token: 0x06001A17 RID: 6679 RVA: 0x0000D536 File Offset: 0x0000B736
	// (set) Token: 0x06001A18 RID: 6680 RVA: 0x0000D53E File Offset: 0x0000B73E
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

	// Token: 0x17000C93 RID: 3219
	// (get) Token: 0x06001A19 RID: 6681 RVA: 0x0000D55A File Offset: 0x0000B75A
	public Vector2 Velocity
	{
		get
		{
			return this.m_controllerCorgi.Velocity;
		}
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x0000D567 File Offset: 0x0000B767
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

	// Token: 0x06001A1B RID: 6683 RVA: 0x0000D5A7 File Offset: 0x0000B7A7
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

	// Token: 0x06001A1C RID: 6684 RVA: 0x0000D5D1 File Offset: 0x0000B7D1
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

	// Token: 0x17000C94 RID: 3220
	// (get) Token: 0x06001A1D RID: 6685 RVA: 0x0000D5FB File Offset: 0x0000B7FB
	public bool IsFalling
	{
		get
		{
			return this.m_controllerCorgi.State.IsFalling;
		}
	}

	// Token: 0x17000C95 RID: 3221
	// (get) Token: 0x06001A1E RID: 6686 RVA: 0x0000D60D File Offset: 0x0000B80D
	public bool IsGrounded
	{
		get
		{
			return this.m_controllerCorgi.State.IsGrounded && this.CurrentHealth > 0f;
		}
	}

	// Token: 0x17000C96 RID: 3222
	// (get) Token: 0x06001A1F RID: 6687 RVA: 0x0000D630 File Offset: 0x0000B830
	public bool IsFacingRight
	{
		get
		{
			return this.m_characterCorgi.IsFacingRight;
		}
	}

	// Token: 0x06001A20 RID: 6688 RVA: 0x0000D63D File Offset: 0x0000B83D
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

	// Token: 0x17000C97 RID: 3223
	// (get) Token: 0x06001A21 RID: 6689 RVA: 0x0000D66D File Offset: 0x0000B86D
	// (set) Token: 0x06001A22 RID: 6690 RVA: 0x0000D67F File Offset: 0x0000B87F
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

	// Token: 0x17000C98 RID: 3224
	// (get) Token: 0x06001A23 RID: 6691 RVA: 0x0000D692 File Offset: 0x0000B892
	public CharacterStates.MovementStates PreviousMovementState
	{
		get
		{
			return this.m_characterCorgi.MovementState.PreviousState;
		}
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x0000D6A4 File Offset: 0x0000B8A4
	[Obsolete("This is a SUPER dangerous call. Better to set the state to what you actually want")]
	public void RestorePreviousMovementState()
	{
		this.m_characterCorgi.MovementState.RestorePreviousState();
	}

	// Token: 0x17000C99 RID: 3225
	// (get) Token: 0x06001A25 RID: 6693 RVA: 0x0000D6B6 File Offset: 0x0000B8B6
	// (set) Token: 0x06001A26 RID: 6694 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
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

	// Token: 0x17000C9A RID: 3226
	// (get) Token: 0x06001A27 RID: 6695 RVA: 0x0000D6DB File Offset: 0x0000B8DB
	[Obsolete("This is a SUPER dangerous call. Better to set the state to what you actually want")]
	public CharacterStates.CharacterConditions PreviousConditionState
	{
		get
		{
			return this.m_characterCorgi.ConditionState.PreviousState;
		}
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x0000D6ED File Offset: 0x0000B8ED
	public void RestorePreviousCharacterCondition()
	{
		this.m_characterCorgi.ConditionState.RestorePreviousState();
	}

	// Token: 0x17000C9B RID: 3227
	// (get) Token: 0x06001A29 RID: 6697 RVA: 0x0000D6FF File Offset: 0x0000B8FF
	public IHitboxController HitboxController
	{
		get
		{
			return this.m_hitboxController;
		}
	}

	// Token: 0x17000C9C RID: 3228
	// (get) Token: 0x06001A2A RID: 6698 RVA: 0x0000D707 File Offset: 0x0000B907
	public SpawnPositionController SpawnPositionController
	{
		get
		{
			return this.m_spawnPositionController;
		}
	}

	// Token: 0x17000C9D RID: 3229
	// (get) Token: 0x06001A2B RID: 6699 RVA: 0x0000D70F File Offset: 0x0000B90F
	public BaseCharacterHitResponse CharacterHitResponse
	{
		get
		{
			return this.m_characterHitResponse;
		}
	}

	// Token: 0x17000C9E RID: 3230
	// (get) Token: 0x06001A2C RID: 6700 RVA: 0x0000D717 File Offset: 0x0000B917
	public CorgiController_RL ControllerCorgi
	{
		get
		{
			return this.m_controllerCorgi;
		}
	}

	// Token: 0x17000C9F RID: 3231
	// (get) Token: 0x06001A2D RID: 6701 RVA: 0x0000D71F File Offset: 0x0000B91F
	public Character CharacterCorgi
	{
		get
		{
			return this.m_characterCorgi;
		}
	}

	// Token: 0x17000CA0 RID: 3232
	// (get) Token: 0x06001A2E RID: 6702 RVA: 0x0000D727 File Offset: 0x0000B927
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17000CA1 RID: 3233
	// (get) Token: 0x06001A2F RID: 6703 RVA: 0x0000D72F File Offset: 0x0000B92F
	public StatusEffectController StatusEffectController
	{
		get
		{
			return this.m_statusEffectController;
		}
	}

	// Token: 0x17000CA2 RID: 3234
	// (get) Token: 0x06001A30 RID: 6704 RVA: 0x0000D737 File Offset: 0x0000B937
	public StatusBarController StatusBarController
	{
		get
		{
			return this.m_statusBarController;
		}
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x0000D73F File Offset: 0x0000B93F
	public void DisableGroundedState()
	{
		this.m_controllerCorgi.State.IsCollidingBelow = false;
		this.m_controllerCorgi.State.JustGotGrounded = false;
	}

	// Token: 0x17000CA3 RID: 3235
	// (get) Token: 0x06001A32 RID: 6706 RVA: 0x0000D763 File Offset: 0x0000B963
	// (set) Token: 0x06001A33 RID: 6707 RVA: 0x0000D76B File Offset: 0x0000B96B
	public bool IsDead { get; protected set; }

	// Token: 0x17000CA4 RID: 3236
	// (get) Token: 0x06001A34 RID: 6708 RVA: 0x0000D774 File Offset: 0x0000B974
	// (set) Token: 0x06001A35 RID: 6709 RVA: 0x0000D77C File Offset: 0x0000B97C
	public bool IsInitialized { get; protected set; }

	// Token: 0x06001A36 RID: 6710 RVA: 0x0009094C File Offset: 0x0008EB4C
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

	// Token: 0x06001A37 RID: 6711 RVA: 0x00090A30 File Offset: 0x0008EC30
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

	// Token: 0x06001A38 RID: 6712 RVA: 0x00090D50 File Offset: 0x0008EF50
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

	// Token: 0x06001A39 RID: 6713 RVA: 0x00090F84 File Offset: 0x0008F184
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

	// Token: 0x06001A3A RID: 6714 RVA: 0x0000D785 File Offset: 0x0000B985
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

	// Token: 0x06001A3B RID: 6715 RVA: 0x0009118C File Offset: 0x0008F38C
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

	// Token: 0x06001A3C RID: 6716 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnJustGrounded()
	{
	}

	// Token: 0x06001A3D RID: 6717
	public abstract float CalculateDamageTaken(IDamageObj damageObj, out CriticalStrikeType critType, out float damageBlocked, float damageOverride = -1f, bool trueDamage = false, bool pureCalculation = true);

	// Token: 0x06001A3E RID: 6718 RVA: 0x0000D794 File Offset: 0x0000B994
	public virtual void KillCharacter(GameObject killer, bool broadcastEvent)
	{
		this.IsDead = true;
		this.StatusEffectController.StopAllStatusEffects(false);
		this.ConditionState = CharacterStates.CharacterConditions.Dead;
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x0000D7B0 File Offset: 0x0000B9B0
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

	// Token: 0x06001A40 RID: 6720 RVA: 0x00091288 File Offset: 0x0008F488
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

	// Token: 0x06001A41 RID: 6721 RVA: 0x000912D8 File Offset: 0x0008F4D8
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

	// Token: 0x06001A42 RID: 6722 RVA: 0x0000D7E8 File Offset: 0x0000B9E8
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

	// Token: 0x06001A43 RID: 6723 RVA: 0x0000D828 File Offset: 0x0000BA28
	public virtual void ResetHealth()
	{
		this.SetHealth((float)this.ActualMaxHealth, false, true);
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x0000D839 File Offset: 0x0000BA39
	public void ResetIsDead()
	{
		this.IsDead = false;
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IEffectVelocity.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IPreOnDisable.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400185B RID: 6235
	private float m_baseScale;

	// Token: 0x0400185C RID: 6236
	private float m_baseStrength;

	// Token: 0x0400185D RID: 6237
	private float m_baseKnockbackDefense;

	// Token: 0x0400185E RID: 6238
	private float m_baseStunDefense;

	// Token: 0x0400185F RID: 6239
	private float m_baseMagic;

	// Token: 0x04001860 RID: 6240
	private float m_baseDamage;

	// Token: 0x04001861 RID: 6241
	protected bool m_disableFriction;

	// Token: 0x04001862 RID: 6242
	[SerializeField]
	[HideInInspector]
	private float m_fallMultiplierOverride = 1f;

	// Token: 0x04001863 RID: 6243
	[SerializeField]
	[HideInInspector]
	private float m_ascentMultiplierOverride = 1f;

	// Token: 0x04001864 RID: 6244
	[SerializeField]
	private GameObject m_visuals;

	// Token: 0x04001865 RID: 6245
	[SerializeField]
	private bool m_takesNoDamage;

	// Token: 0x04001866 RID: 6246
	private static MaterialPropertyBlock m_charMaterialPropertyBlock_STATIC;

	// Token: 0x04001867 RID: 6247
	protected CorgiController_RL m_controllerCorgi;

	// Token: 0x04001868 RID: 6248
	protected BaseCharacterHitResponse m_characterHitResponse;

	// Token: 0x04001869 RID: 6249
	protected Character m_characterCorgi;

	// Token: 0x0400186A RID: 6250
	protected SpawnPositionController m_spawnPositionController;

	// Token: 0x0400186B RID: 6251
	protected StatusEffectController m_statusEffectController;

	// Token: 0x0400186C RID: 6252
	protected StatusBarController m_statusBarController;

	// Token: 0x0400186D RID: 6253
	protected IHitboxController m_hitboxController;

	// Token: 0x0400186E RID: 6254
	protected Animator m_animator;

	// Token: 0x0400186F RID: 6255
	private Vector2 m_heading = new Vector2(1f, 0f);

	// Token: 0x04001870 RID: 6256
	private float m_orientation;

	// Token: 0x04001871 RID: 6257
	private bool m_knockedIntoAir;

	// Token: 0x04001872 RID: 6258
	private float m_knockedIntoAirStartTime;

	// Token: 0x04001873 RID: 6259
	private bool m_wasGroundedLastFixedUpdate;

	// Token: 0x04001874 RID: 6260
	private Vector2 m_internalKnockbackMod = Vector2.one;

	// Token: 0x04001875 RID: 6261
	private Vector2 m_externalKnockbackMod = Vector2.one;

	// Token: 0x04001876 RID: 6262
	protected HealthChangeEventArgs m_healthChangeEventArgs;

	// Token: 0x04001877 RID: 6263
	private bool m_useCachedVisualBounds;

	// Token: 0x04001878 RID: 6264
	private Bounds_RL m_cachedVisualBounds;

	// Token: 0x04001879 RID: 6265
	protected Relay<GameObject> m_onDeathEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x0400187E RID: 6270
	protected Relay<IPreOnDisable> m_onPreDisableRelay = new Relay<IPreOnDisable>();

	// Token: 0x0400187F RID: 6271
	private Relay<object, HealthChangeEventArgs> m_healthChangeRelay = new Relay<object, HealthChangeEventArgs>();

	// Token: 0x04001880 RID: 6272
	private Relay<object, HealthChangeEventArgs> m_maxHealthChangeRelay = new Relay<object, HealthChangeEventArgs>();

	// Token: 0x04001892 RID: 6290
	protected Vector2 m_speedHolder;
}
