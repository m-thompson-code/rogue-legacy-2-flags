using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000467 RID: 1127
public abstract class BaseItemDrop : MonoBehaviour, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnStayHitResponse, IEffectTriggerEvent_OnTimeout, IEffectTriggerEvent_OnDeath, IGenericPoolObj, IEffectTriggerEvent_OnLanded
{
	// Token: 0x17001023 RID: 4131
	// (get) Token: 0x0600297F RID: 10623 RVA: 0x00089317 File Offset: 0x00087517
	// (set) Token: 0x06002980 RID: 10624 RVA: 0x0008931F File Offset: 0x0008751F
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001024 RID: 4132
	// (get) Token: 0x06002981 RID: 10625 RVA: 0x00089328 File Offset: 0x00087528
	// (set) Token: 0x06002982 RID: 10626 RVA: 0x00089330 File Offset: 0x00087530
	public int ValueOverride { get; set; } = -1;

	// Token: 0x17001025 RID: 4133
	// (get) Token: 0x06002983 RID: 10627 RVA: 0x00089339 File Offset: 0x00087539
	// (set) Token: 0x06002984 RID: 10628 RVA: 0x00089341 File Offset: 0x00087541
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17001026 RID: 4134
	// (get) Token: 0x06002985 RID: 10629 RVA: 0x0008934A File Offset: 0x0008754A
	// (set) Token: 0x06002986 RID: 10630 RVA: 0x00089352 File Offset: 0x00087552
	public BaseItemDrop ItemDropPrefab { get; set; }

	// Token: 0x17001027 RID: 4135
	// (get) Token: 0x06002987 RID: 10631 RVA: 0x0008935B File Offset: 0x0008755B
	// (set) Token: 0x06002988 RID: 10632 RVA: 0x00089363 File Offset: 0x00087563
	public bool Magnetized { get; private set; }

	// Token: 0x17001028 RID: 4136
	// (get) Token: 0x06002989 RID: 10633 RVA: 0x0008936C File Offset: 0x0008756C
	// (set) Token: 0x0600298A RID: 10634 RVA: 0x00089374 File Offset: 0x00087574
	public bool ForceMagnetizedAlways { get; private set; }

	// Token: 0x17001029 RID: 4137
	// (get) Token: 0x0600298B RID: 10635 RVA: 0x0008937D File Offset: 0x0008757D
	// (set) Token: 0x0600298C RID: 10636 RVA: 0x00089385 File Offset: 0x00087585
	public bool ForceMagnetized { get; private set; }

	// Token: 0x1700102A RID: 4138
	// (get) Token: 0x0600298D RID: 10637 RVA: 0x0008938E File Offset: 0x0008758E
	public IRelayLink<GameObject> OnDeathEffectTriggerRelay
	{
		get
		{
			return this.m_onDeathEffectTriggerRelay.link;
		}
	}

	// Token: 0x1700102B RID: 4139
	// (get) Token: 0x0600298E RID: 10638 RVA: 0x0008939B File Offset: 0x0008759B
	public IRelayLink<GameObject> OnTimeoutEffectTriggerRelay
	{
		get
		{
			return this.m_onTimeoutEffectTriggerRelay.link;
		}
	}

	// Token: 0x1700102C RID: 4140
	// (get) Token: 0x0600298F RID: 10639 RVA: 0x000893A8 File Offset: 0x000875A8
	public IRelayLink<CorgiController_RL> OnLandedEffectTriggerRelay
	{
		get
		{
			return this.m_corgiController.OnCorgiLandedEnterRelay;
		}
	}

	// Token: 0x1700102D RID: 4141
	// (get) Token: 0x06002990 RID: 10640
	public abstract ItemDropType ItemDropType { get; }

	// Token: 0x1700102E RID: 4142
	// (get) Token: 0x06002991 RID: 10641 RVA: 0x000893B5 File Offset: 0x000875B5
	public Vector2 Speed
	{
		get
		{
			return this.m_corgiController.Velocity;
		}
	}

	// Token: 0x1700102F RID: 4143
	// (get) Token: 0x06002992 RID: 10642 RVA: 0x000893C2 File Offset: 0x000875C2
	public CorgiController_RL CorgiController
	{
		get
		{
			return this.m_corgiController;
		}
	}

	// Token: 0x06002993 RID: 10643 RVA: 0x000893CC File Offset: 0x000875CC
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

	// Token: 0x06002994 RID: 10644 RVA: 0x00089434 File Offset: 0x00087634
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

	// Token: 0x06002995 RID: 10645 RVA: 0x000894D0 File Offset: 0x000876D0
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

	// Token: 0x06002996 RID: 10646 RVA: 0x000895D4 File Offset: 0x000877D4
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

	// Token: 0x06002997 RID: 10647 RVA: 0x0008975B File Offset: 0x0008795B
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002998 RID: 10648 RVA: 0x0008976C File Offset: 0x0008796C
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

	// Token: 0x06002999 RID: 10649 RVA: 0x00089898 File Offset: 0x00087A98
	public void UpdateBounds()
	{
		if (this.CorgiController && this.CorgiController.IsInitialized)
		{
			this.CorgiController.SetRaysParameters();
		}
	}

	// Token: 0x0600299A RID: 10650 RVA: 0x000898C0 File Offset: 0x00087AC0
	protected virtual void Collect(GameObject collector)
	{
		BaseItemDrop.m_itemArgs_STATIC.Initialize(this);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.ItemCollected, this, BaseItemDrop.m_itemArgs_STATIC);
		this.CollectedRelay.Dispatch();
		this.m_collected = true;
		this.m_onDeathEffectTriggerRelay.Dispatch(collector);
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600299B RID: 10651 RVA: 0x00089910 File Offset: 0x00087B10
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

	// Token: 0x0600299C RID: 10652 RVA: 0x00089A43 File Offset: 0x00087C43
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

	// Token: 0x0600299D RID: 10653 RVA: 0x00089A76 File Offset: 0x00087C76
	public void ForceMagnetizeOnGrounded()
	{
		base.StartCoroutine(this.ForceMagnetizeOnGroundCoroutine());
	}

	// Token: 0x0600299E RID: 10654 RVA: 0x00089A85 File Offset: 0x00087C85
	private IEnumerator ForceMagnetizeOnGroundCoroutine()
	{
		while (!this.m_hasTouchedGroundOnce)
		{
			yield return null;
		}
		this.ForceMagnetize(false);
		yield break;
	}

	// Token: 0x0600299F RID: 10655 RVA: 0x00089A94 File Offset: 0x00087C94
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

	// Token: 0x060029A0 RID: 10656 RVA: 0x00089B0C File Offset: 0x00087D0C
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnEnterHitResponse(otherHBController);
	}

	// Token: 0x060029A1 RID: 10657 RVA: 0x00089B18 File Offset: 0x00087D18
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

	// Token: 0x060029A2 RID: 10658 RVA: 0x00089B91 File Offset: 0x00087D91
	protected virtual void OnEnable()
	{
		this.m_spawnTime = Time.time;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x060029A3 RID: 10659 RVA: 0x00089BAB File Offset: 0x00087DAB
	protected virtual void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x060029A5 RID: 10661 RVA: 0x00089C15 File Offset: 0x00087E15
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002228 RID: 8744
	private static ItemCollectedEventArgs m_itemArgs_STATIC;

	// Token: 0x04002229 RID: 8745
	private const float MAGNETISM_DELAY_TIMER = 1f;

	// Token: 0x0400222A RID: 8746
	[SerializeField]
	private float m_lifetime = 30f;

	// Token: 0x0400222B RID: 8747
	protected float m_collectLockDuration = 0.1f;

	// Token: 0x0400222C RID: 8748
	private float m_collectLockTimer;

	// Token: 0x0400222D RID: 8749
	protected bool m_collected;

	// Token: 0x0400222E RID: 8750
	protected CorgiController_RL m_corgiController;

	// Token: 0x0400222F RID: 8751
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002230 RID: 8752
	protected bool m_hasTouchedGroundOnce;

	// Token: 0x04002231 RID: 8753
	private float m_spawnTime;

	// Token: 0x04002232 RID: 8754
	private IHitboxController m_hbController;

	// Token: 0x04002233 RID: 8755
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

	// Token: 0x0400223B RID: 8763
	private Relay<GameObject> m_onDeathEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x0400223C RID: 8764
	private Relay<GameObject> m_onTimeoutEffectTriggerRelay = new Relay<GameObject>();

	// Token: 0x0400223D RID: 8765
	public Relay CollectedRelay = new Relay();

	// Token: 0x0400223E RID: 8766
	private float m_magnetizeStartTime;
}
