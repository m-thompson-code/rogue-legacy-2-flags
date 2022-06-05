using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200075C RID: 1884
public abstract class BaseItemDrop : MonoBehaviour, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnStayHitResponse, IEffectTriggerEvent_OnTimeout, IEffectTriggerEvent_OnDeath, IGenericPoolObj, IEffectTriggerEvent_OnLanded
{
	// Token: 0x17001556 RID: 5462
	// (get) Token: 0x06003983 RID: 14723 RVA: 0x0001FA18 File Offset: 0x0001DC18
	// (set) Token: 0x06003984 RID: 14724 RVA: 0x0001FA20 File Offset: 0x0001DC20
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001557 RID: 5463
	// (get) Token: 0x06003985 RID: 14725 RVA: 0x0001FA29 File Offset: 0x0001DC29
	// (set) Token: 0x06003986 RID: 14726 RVA: 0x0001FA31 File Offset: 0x0001DC31
	public int ValueOverride { get; set; } = -1;

	// Token: 0x17001558 RID: 5464
	// (get) Token: 0x06003987 RID: 14727 RVA: 0x0001FA3A File Offset: 0x0001DC3A
	// (set) Token: 0x06003988 RID: 14728 RVA: 0x0001FA42 File Offset: 0x0001DC42
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17001559 RID: 5465
	// (get) Token: 0x06003989 RID: 14729 RVA: 0x0001FA4B File Offset: 0x0001DC4B
	// (set) Token: 0x0600398A RID: 14730 RVA: 0x0001FA53 File Offset: 0x0001DC53
	public BaseItemDrop ItemDropPrefab { get; set; }

	// Token: 0x1700155A RID: 5466
	// (get) Token: 0x0600398B RID: 14731 RVA: 0x0001FA5C File Offset: 0x0001DC5C
	// (set) Token: 0x0600398C RID: 14732 RVA: 0x0001FA64 File Offset: 0x0001DC64
	public bool Magnetized { get; private set; }

	// Token: 0x1700155B RID: 5467
	// (get) Token: 0x0600398D RID: 14733 RVA: 0x0001FA6D File Offset: 0x0001DC6D
	// (set) Token: 0x0600398E RID: 14734 RVA: 0x0001FA75 File Offset: 0x0001DC75
	public bool ForceMagnetizedAlways { get; private set; }

	// Token: 0x1700155C RID: 5468
	// (get) Token: 0x0600398F RID: 14735 RVA: 0x0001FA7E File Offset: 0x0001DC7E
	// (set) Token: 0x06003990 RID: 14736 RVA: 0x0001FA86 File Offset: 0x0001DC86
	public bool ForceMagnetized { get; private set; }

	// Token: 0x1700155D RID: 5469
	// (get) Token: 0x06003991 RID: 14737 RVA: 0x0001FA8F File Offset: 0x0001DC8F
	public IRelayLink<GameObject> OnDeathEffectTriggerRelay
	{
		get
		{
			return this.m_onDeathEffectTriggerRelay.link;
		}
	}

	// Token: 0x1700155E RID: 5470
	// (get) Token: 0x06003992 RID: 14738 RVA: 0x0001FA9C File Offset: 0x0001DC9C
	public IRelayLink<GameObject> OnTimeoutEffectTriggerRelay
	{
		get
		{
			return this.m_onTimeoutEffectTriggerRelay.link;
		}
	}

	// Token: 0x1700155F RID: 5471
	// (get) Token: 0x06003993 RID: 14739 RVA: 0x0001FAA9 File Offset: 0x0001DCA9
	public IRelayLink<CorgiController_RL> OnLandedEffectTriggerRelay
	{
		get
		{
			return this.m_corgiController.OnCorgiLandedEnterRelay;
		}
	}

	// Token: 0x17001560 RID: 5472
	// (get) Token: 0x06003994 RID: 14740
	public abstract ItemDropType ItemDropType { get; }

	// Token: 0x17001561 RID: 5473
	// (get) Token: 0x06003995 RID: 14741 RVA: 0x0001FAB6 File Offset: 0x0001DCB6
	public Vector2 Speed
	{
		get
		{
			return this.m_corgiController.Velocity;
		}
	}

	// Token: 0x17001562 RID: 5474
	// (get) Token: 0x06003996 RID: 14742 RVA: 0x0001FAC3 File Offset: 0x0001DCC3
	public CorgiController_RL CorgiController
	{
		get
		{
			return this.m_corgiController;
		}
	}

	// Token: 0x06003997 RID: 14743 RVA: 0x000EB2C0 File Offset: 0x000E94C0
	protected void Awake()
	{
		this.m_corgiController = base.GetComponent<CorgiController_RL>();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		if (BaseItemDrop.m_itemArgs_STATIC == null)
		{
			BaseItemDrop.m_itemArgs_STATIC = new ItemCollectedEventArgs(null);
		}
		this.IsAwakeCalled = true;
		this.m_onPlayerExitRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitRoom);
		this.ResetValues();
	}

	// Token: 0x06003998 RID: 14744 RVA: 0x000EB328 File Offset: 0x000E9528
	protected virtual void Update()
	{
		if (!this.m_hasTouchedGroundOnce && this.m_corgiController.State.IsGrounded)
		{
			this.m_hasTouchedGroundOnce = true;
		}
		this.UpdateMagnetism();
		if (!this.Magnetized && this.m_corgiController.State.IsGrounded && this.m_corgiController.Velocity.x != 0f)
		{
			this.m_corgiController.SetHorizontalForce(0f);
		}
		if (this.m_collectLockTimer > 0f)
		{
			this.m_collectLockTimer -= Time.deltaTime;
		}
		this.ConstrainItemDropToRoom();
	}

	// Token: 0x06003999 RID: 14745 RVA: 0x000EB3C4 File Offset: 0x000E95C4
	private void UpdateMagnetism()
	{
		bool flag = this.ItemDropType == ItemDropType.HealthDrop || this.ItemDropType == ItemDropType.PizzaDrop;
		if (flag && TraitManager.IsTraitActive(TraitType.NoMeat))
		{
			return;
		}
		if (!this.ForceMagnetizedAlways && (flag || this.ItemDropType == ItemDropType.ManaDrop || this.ItemDropType == ItemDropType.CandyDrop || this.ItemDropType == ItemDropType.CookieDrop || this.ItemDropType == ItemDropType.MilkManaDrop))
		{
			return;
		}
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (!this.ForceMagnetized && !this.ForceMagnetizedAlways && (this.m_hasTouchedGroundOnce || Time.time >= this.m_spawnTime + 1f))
			{
				float magnetDistance = RuneLogicHelper.GetMagnetDistance();
				if (CDGHelper.DistanceBetweenPts(base.transform.localPosition, playerController.Midpoint) <= magnetDistance)
				{
					if (!this.Magnetized)
					{
						this.m_magnetizeStartTime = Time.time;
					}
					this.Magnetized = true;
				}
				else
				{
					this.Magnetized = false;
				}
			}
			this.MagnetizeTowardsPosition(playerController.Midpoint);
		}
	}

	// Token: 0x0600399A RID: 14746 RVA: 0x000EB4C8 File Offset: 0x000E96C8
	private void MagnetizeTowardsPosition(Vector2 pos)
	{
		if (this.m_corgiController)
		{
			if (this.Magnetized || this.ForceMagnetized || this.ForceMagnetizedAlways)
			{
				if (this.m_corgiController.IsGravityActive)
				{
					this.m_corgiController.GravityActive(false);
				}
				if (!this.m_corgiController.DisablePlatformCollision)
				{
					this.m_corgiController.DisablePlatformCollision = true;
				}
				Vector2 vector = CDGHelper.VectorBetweenPts(base.transform.localPosition, pos);
				vector.Normalize();
				Vector2 vector2 = vector * Economy_EV.ITEM_DROP_ACCELERATION * Time.deltaTime;
				vector2.x = Mathf.Lerp(0f, vector.x * Economy_EV.ITEM_DROP_ACCELERATION.x, Time.time - this.m_magnetizeStartTime);
				vector2.y = Mathf.Lerp(0f, vector.y * Economy_EV.ITEM_DROP_ACCELERATION.y, Time.time - this.m_magnetizeStartTime);
				vector2.x = Mathf.Clamp(vector2.x, -Economy_EV.ITEM_DROP_MAX_MAGNET_SPEED.x, Economy_EV.ITEM_DROP_MAX_MAGNET_SPEED.x);
				vector2.y = Mathf.Clamp(vector2.y, -Economy_EV.ITEM_DROP_MAX_MAGNET_SPEED.y, Economy_EV.ITEM_DROP_MAX_MAGNET_SPEED.y);
				this.m_corgiController.SetForce(vector2);
				return;
			}
			if (!this.m_corgiController.IsGravityActive)
			{
				this.m_corgiController.GravityActive(true);
			}
			if (this.m_corgiController.DisablePlatformCollision)
			{
				this.m_corgiController.DisablePlatformCollision = false;
			}
		}
	}

	// Token: 0x0600399B RID: 14747 RVA: 0x0001C0A5 File Offset: 0x0001A2A5
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600399C RID: 14748 RVA: 0x000EB650 File Offset: 0x000E9850
	protected virtual void ConstrainItemDropToRoom()
	{
		Rect absBounds = this.CorgiController.AbsBounds;
		if (absBounds.width == 0f || absBounds.height == 0f)
		{
			Debug.Log("<color=yellow>Cannot constrain item to room.  Bounds width or height is 0.</color>");
			return;
		}
		BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
		if (currentPlayerRoom)
		{
			Rect boundsRect = currentPlayerRoom.BoundsRect;
			float num = 0f;
			float num2 = 0f;
			if (absBounds.xMin < boundsRect.xMin)
			{
				num = boundsRect.xMin - absBounds.xMin;
			}
			else if (absBounds.xMax > boundsRect.xMax)
			{
				num = boundsRect.xMax - absBounds.xMax;
			}
			if (absBounds.yMin < boundsRect.yMin)
			{
				this.ForceMagnetize(true);
				num2 = boundsRect.yMin - absBounds.yMin;
			}
			else if (absBounds.yMax > boundsRect.yMax)
			{
				num2 = boundsRect.yMax - absBounds.yMax;
			}
			if (num != 0f || num2 != 0f)
			{
				base.gameObject.transform.position += new Vector3(num, num2);
				this.UpdateBounds();
			}
		}
	}

	// Token: 0x0600399D RID: 14749 RVA: 0x0001FACB File Offset: 0x0001DCCB
	public void UpdateBounds()
	{
		if (this.CorgiController && this.CorgiController.IsInitialized)
		{
			this.CorgiController.SetRaysParameters();
		}
	}

	// Token: 0x0600399E RID: 14750 RVA: 0x000EB77C File Offset: 0x000E997C
	protected virtual void Collect(GameObject collector)
	{
		BaseItemDrop.m_itemArgs_STATIC.Initialize(this);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.ItemCollected, this, BaseItemDrop.m_itemArgs_STATIC);
		this.CollectedRelay.Dispatch();
		this.m_collected = true;
		this.m_onDeathEffectTriggerRelay.Dispatch(collector);
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600399F RID: 14751 RVA: 0x000EB7CC File Offset: 0x000E99CC
	public virtual void OnSpawnCollectCollisionCheck()
	{
		if (!PlayerManager.IsInstantiated)
		{
			return;
		}
		float y = PlayerManager.GetCurrentPlayerRoom().Bounds.max.y;
		if (base.transform.position.y > y)
		{
			base.transform.position = new Vector3(base.transform.position.x, y, base.transform.position.z);
		}
		int layerMask = this.CorgiController.PlatformMask & ~this.CorgiController.OneWayPlatformMask;
		Collider2D collider = this.m_hbController.GetCollider(HitboxType.Platform);
		if (Physics2D.OverlapBox(collider.bounds.center, collider.bounds.size, 0f, layerMask))
		{
			if (this.ItemDropType != ItemDropType.HealthDrop && this.ItemDropType != ItemDropType.ManaDrop && this.ItemDropType != ItemDropType.PizzaDrop && this.ItemDropType != ItemDropType.MushroomDrop && this.ItemDropType != ItemDropType.CandyDrop && this.ItemDropType != ItemDropType.CookieDrop && this.ItemDropType != ItemDropType.MilkManaDrop)
			{
				this.ForceMagnetize(false);
				return;
			}
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060039A0 RID: 14752 RVA: 0x0001FAF2 File Offset: 0x0001DCF2
	public void ForceMagnetize(bool forceMagnetizeAlways)
	{
		if (!this.ForceMagnetized && !this.ForceMagnetizedAlways)
		{
			if (forceMagnetizeAlways)
			{
				this.ForceMagnetizedAlways = true;
			}
			else
			{
				this.ForceMagnetized = true;
			}
			this.m_magnetizeStartTime = Time.time;
		}
	}

	// Token: 0x060039A1 RID: 14753 RVA: 0x0001FB25 File Offset: 0x0001DD25
	public void ForceMagnetizeOnGrounded()
	{
		base.StartCoroutine(this.ForceMagnetizeOnGroundCoroutine());
	}

	// Token: 0x060039A2 RID: 14754 RVA: 0x0001FB34 File Offset: 0x0001DD34
	private IEnumerator ForceMagnetizeOnGroundCoroutine()
	{
		while (!this.m_hasTouchedGroundOnce)
		{
			yield return null;
		}
		this.ForceMagnetize(false);
		yield break;
	}

	// Token: 0x060039A3 RID: 14755 RVA: 0x000EB900 File Offset: 0x000E9B00
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (this.m_collected)
		{
			return;
		}
		if (otherHBController.CollisionType == CollisionType.Hazard && otherHBController.RootGameObject.GetComponent<Cloud>())
		{
			this.ForceMagnetize(true);
			return;
		}
		if (this.m_collectLockTimer > 0f)
		{
			return;
		}
		if (otherHBController.RootGameObject.CompareTag("Player") || otherHBController.RootGameObject.CompareTag("Player_Dodging"))
		{
			this.Collect(otherHBController.RootGameObject);
		}
	}

	// Token: 0x060039A4 RID: 14756 RVA: 0x0001FB43 File Offset: 0x0001DD43
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnEnterHitResponse(otherHBController);
	}

	// Token: 0x060039A5 RID: 14757 RVA: 0x000EB978 File Offset: 0x000E9B78
	public void ResetValues()
	{
		this.m_collected = false;
		this.m_collectLockTimer = this.m_collectLockDuration;
		if (this.m_corgiController)
		{
			this.m_corgiController.GravityActive(true);
			this.m_corgiController.DisablePlatformCollision = false;
		}
		this.m_magnetizeStartTime = 0f;
		this.ForceMagnetized = false;
		this.ForceMagnetizedAlways = false;
		this.Magnetized = false;
		this.m_hasTouchedGroundOnce = false;
		this.ValueOverride = -1;
		base.StopAllCoroutines();
	}

	// Token: 0x060039A6 RID: 14758 RVA: 0x0001FB4C File Offset: 0x0001DD4C
	protected virtual void OnEnable()
	{
		this.m_spawnTime = Time.time;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x060039A7 RID: 14759 RVA: 0x0001FB66 File Offset: 0x0001DD66
	protected virtual void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x060039A9 RID: 14761 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002E01 RID: 11777
	private static ItemCollectedEventArgs m_itemArgs_STATIC;

	// Token: 0x04002E02 RID: 11778
	private const float MAGNETISM_DELAY_TIMER = 1f;

	// Token: 0x04002E03 RID: 11779
	[SerializeField]
	private float m_lifetime = 30f;

	// Token: 0x04002E04 RID: 11780
	protected float m_collectLockDuration = 0.1f;

	// Token: 0x04002E05 RID: 11781
	private float m_collectLockTimer;

	// Token: 0x04002E06 RID: 11782
	protected bool m_collected;

	// Token: 0x04002E07 RID: 11783
	protected CorgiController_RL m_corgiController;

	// Token: 0x04002E08 RID: 11784
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002E09 RID: 11785
	protected bool m_hasTouchedGroundOnce;

	// Token: 0x04002E0A RID: 11786
	private float m_spawnTime;

	// Token: 0x04002E0B RID: 11787
	private IHitboxController m_hbController;

	// Token: 0x04002E0C RID: 11788
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04002E14 RID: 11796
	private Relay<GameObject> m_onDeathEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x04002E15 RID: 11797
	private Relay<GameObject> m_onTimeoutEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x04002E16 RID: 11798
	public Relay CollectedRelay = new Relay();

	// Token: 0x04002E17 RID: 11799
	private float m_magnetizeStartTime;
}
