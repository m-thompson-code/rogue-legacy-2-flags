using System;
using System.Collections;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200056D RID: 1389
public class Tunnel : MonoBehaviour, IRoomConsumer, IRootObj
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x060032CA RID: 13002 RVA: 0x000ABDF0 File Offset: 0x000A9FF0
	// (remove) Token: 0x060032CB RID: 13003 RVA: 0x000ABE28 File Offset: 0x000AA028
	public event EventHandler<EventArgs> DestroyedEvent;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x060032CC RID: 13004 RVA: 0x000ABE60 File Offset: 0x000AA060
	// (remove) Token: 0x060032CD RID: 13005 RVA: 0x000ABE98 File Offset: 0x000AA098
	public event EventHandler<EventArgs> PlayerEnteredEvent;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060032CE RID: 13006 RVA: 0x000ABED0 File Offset: 0x000AA0D0
	// (remove) Token: 0x060032CF RID: 13007 RVA: 0x000ABF08 File Offset: 0x000AA108
	public event EventHandler<EventArgs> PlayerExitedEvent;

	// Token: 0x17001265 RID: 4709
	// (get) Token: 0x060032D0 RID: 13008 RVA: 0x000ABF3D File Offset: 0x000AA13D
	public Interactable Interactable
	{
		get
		{
			return this.m_interactable;
		}
	}

	// Token: 0x17001266 RID: 4710
	// (get) Token: 0x060032D1 RID: 13009 RVA: 0x000ABF45 File Offset: 0x000AA145
	// (set) Token: 0x060032D2 RID: 13010 RVA: 0x000ABF4D File Offset: 0x000AA14D
	public bool IsInstantiated { get; private set; }

	// Token: 0x17001267 RID: 4711
	// (get) Token: 0x060032D3 RID: 13011 RVA: 0x000ABF56 File Offset: 0x000AA156
	protected bool IsCutsceneTeleport
	{
		get
		{
			return this.m_tempDestinationOverride;
		}
	}

	// Token: 0x17001268 RID: 4712
	// (get) Token: 0x060032D4 RID: 13012 RVA: 0x000ABF63 File Offset: 0x000AA163
	// (set) Token: 0x060032D5 RID: 13013 RVA: 0x000ABF7F File Offset: 0x000AA17F
	public Tunnel Destination
	{
		get
		{
			if (!this.m_tempDestinationOverride)
			{
				return this.m_destination;
			}
			return this.m_tempDestinationOverride;
		}
		set
		{
			this.m_destination = value;
		}
	}

	// Token: 0x17001269 RID: 4713
	// (get) Token: 0x060032D6 RID: 13014 RVA: 0x000ABF88 File Offset: 0x000AA188
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x1700126A RID: 4714
	// (get) Token: 0x060032D7 RID: 13015 RVA: 0x000ABF90 File Offset: 0x000AA190
	// (set) Token: 0x060032D8 RID: 13016 RVA: 0x000ABF98 File Offset: 0x000AA198
	public int Index { get; private set; }

	// Token: 0x1700126B RID: 4715
	// (get) Token: 0x060032D9 RID: 13017 RVA: 0x000ABFA1 File Offset: 0x000AA1A1
	// (set) Token: 0x060032DA RID: 13018 RVA: 0x000ABFA9 File Offset: 0x000AA1A9
	public bool LeadsToRoot { get; private set; }

	// Token: 0x1700126C RID: 4716
	// (get) Token: 0x060032DB RID: 13019 RVA: 0x000ABFB2 File Offset: 0x000AA1B2
	// (set) Token: 0x060032DC RID: 13020 RVA: 0x000ABFBA File Offset: 0x000AA1BA
	public bool IsLocked { get; protected set; }

	// Token: 0x1700126D RID: 4717
	// (get) Token: 0x060032DD RID: 13021 RVA: 0x000ABFC3 File Offset: 0x000AA1C3
	// (set) Token: 0x060032DE RID: 13022 RVA: 0x000ABFCB File Offset: 0x000AA1CB
	public TransitionID TransitionType { get; private set; }

	// Token: 0x1700126E RID: 4718
	// (get) Token: 0x060032DF RID: 13023 RVA: 0x000ABFD4 File Offset: 0x000AA1D4
	// (set) Token: 0x060032E0 RID: 13024 RVA: 0x000ABFDC File Offset: 0x000AA1DC
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
		private set
		{
			this.m_room = value;
		}
	}

	// Token: 0x1700126F RID: 4719
	// (get) Token: 0x060032E1 RID: 13025 RVA: 0x000ABFE5 File Offset: 0x000AA1E5
	// (set) Token: 0x060032E2 RID: 13026 RVA: 0x000ABFED File Offset: 0x000AA1ED
	public Tunnel Root { get; private set; }

	// Token: 0x17001270 RID: 4720
	// (get) Token: 0x060032E3 RID: 13027 RVA: 0x000ABFF6 File Offset: 0x000AA1F6
	// (set) Token: 0x060032E4 RID: 13028 RVA: 0x000ABFFE File Offset: 0x000AA1FE
	public Animator Animator { get; private set; }

	// Token: 0x060032E5 RID: 13029 RVA: 0x000AC007 File Offset: 0x000AA207
	protected virtual void Awake()
	{
		this.Animator = base.GetComponent<Animator>();
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x000AC015 File Offset: 0x000AA215
	private void OnDestroy()
	{
		if (this.DestroyedEvent != null)
		{
			this.DestroyedEvent(this, EventArgs.Empty);
		}
	}

	// Token: 0x060032E7 RID: 13031 RVA: 0x000AC030 File Offset: 0x000AA230
	private void OnDisable()
	{
		if (this.m_interactable)
		{
			this.m_interactable.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.OnPlayerInteractedWithTunnel));
		}
	}

	// Token: 0x060032E8 RID: 13032 RVA: 0x000AC05C File Offset: 0x000AA25C
	protected virtual void OnEnable()
	{
		if (this.m_interactable)
		{
			this.m_interactable.TriggerOnEnterEvent.AddListener(new UnityAction<GameObject>(this.OnPlayerInteractedWithTunnel));
		}
	}

	// Token: 0x060032E9 RID: 13033 RVA: 0x000AC088 File Offset: 0x000AA288
	public void Initialise()
	{
		this.IsInstantiated = true;
	}

	// Token: 0x060032EA RID: 13034 RVA: 0x000AC091 File Offset: 0x000AA291
	public virtual void ForceEnterTunnel(bool animate, Tunnel tempDestinationOverride = null)
	{
		this.m_tempDestinationOverride = tempDestinationOverride;
		RoomSaveController.DisableCutsceneSaving = tempDestinationOverride;
		if (animate)
		{
			this.OnPlayerInteractedWithTunnel(null);
			return;
		}
		this.EnterTunnel();
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x000AC0B8 File Offset: 0x000AA2B8
	protected virtual void OnPlayerInteractedWithTunnel(GameObject otherObj)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (!playerController.IsGrounded)
		{
			playerController.ControllerCorgi.enabled = false;
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.EnterTunnelCoroutine());
	}

	// Token: 0x060032EC RID: 13036 RVA: 0x000AC0F2 File Offset: 0x000AA2F2
	protected virtual IEnumerator EnterTunnelCoroutine()
	{
		if (this.IsCutsceneTeleport)
		{
			SceneLoader_RL.RunTransitionWithLogic(delegate()
			{
				this.EnterTunnel();
			}, TransitionID.QuickSwipe, false);
		}
		else
		{
			SceneLoader_RL.RunTransitionWithLogic(delegate()
			{
				this.EnterTunnel();
			}, this.TransitionType, false);
		}
		yield break;
	}

	// Token: 0x060032ED RID: 13037 RVA: 0x000AC104 File Offset: 0x000AA304
	public void SetRoot(Tunnel root)
	{
		if (root == null)
		{
			Debug.LogFormat("<color=red>| {0} | Given Tunnel <b>root</b> argument is null, but shouldn't be.</color>", new object[]
			{
				this
			});
			base.gameObject.SetActive(false);
			return;
		}
		this.Root = root;
		if (this.LeadsToRoot)
		{
			this.Destination = this.Root;
		}
	}

	// Token: 0x060032EE RID: 13038 RVA: 0x000AC156 File Offset: 0x000AA356
	public void SetIsDestinationRoot(bool leadsToRoot)
	{
		if (this.Root == this)
		{
			Debug.LogFormat("<color=red>| {0} | Given Tunnel is set to <b>Lead To Root</b>, but it <b>is</b> the Root. This should not happen.</color>", new object[]
			{
				this
			});
			base.gameObject.SetActive(false);
			return;
		}
		this.LeadsToRoot = leadsToRoot;
	}

	// Token: 0x060032EF RID: 13039 RVA: 0x000AC190 File Offset: 0x000AA390
	protected void EnterTunnel()
	{
		if (PlayerManager.GetPlayerController().CurrentHealth <= 0f && !ChallengeManager.IsInChallenge)
		{
			return;
		}
		if (this.Destination)
		{
			if (this.m_enterUnityEvent != null)
			{
				this.m_enterUnityEvent.Invoke();
			}
			this.Room.PlayerExit(null);
			LocalTeleporterController.StopTeleportPlayer();
			PlayerController playerController = PlayerManager.GetPlayerController();
			CutsceneManager.SetTraitsEnabled(true);
			playerController.StopActiveAbilities(true);
			playerController.StatusBarController.SetCanvasVisible(true);
			playerController.Visuals.SetActive(true);
			playerController.ControllerCorgi.enabled = true;
			playerController.HitboxController.DisableAllCollisions = false;
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayPlayerHUD, this, null);
			this.Destination.PlacePlayerHere();
			if (this.PlayerEnteredEvent != null)
			{
				this.PlayerEnteredEvent(this, EventArgs.Empty);
			}
		}
		else
		{
			Debug.LogFormat("<color=red>| {0} | Destination is null</color>", new object[]
			{
				this
			});
		}
		this.m_tempDestinationOverride = null;
		RoomSaveController.DisableCutsceneSaving = false;
	}

	// Token: 0x060032F0 RID: 13040 RVA: 0x000AC27C File Offset: 0x000AA47C
	public void PlacePlayerHere()
	{
		if (this.Room)
		{
			if (!this.Room.gameObject.activeInHierarchy)
			{
				this.Room.gameObject.SetActive(true);
			}
			Room room = this.Room as Room;
			if (room)
			{
				room.CalculateRoomLevel();
			}
			Vector3 a = this.Room.gameObject.transform.InverseTransformPoint(base.transform.position);
			PlayerManager.GetPlayerController().DisableEffectsOnEnterTunnel();
			this.Room.PlacePlayerInRoom(a + Tunnel.OFFSET);
			if (this.PlayerExitedEvent != null)
			{
				this.PlayerExitedEvent(this, EventArgs.Empty);
			}
			if (this.m_exitUnityEvent != null)
			{
				this.m_exitUnityEvent.Invoke();
				return;
			}
		}
		else if (this.Room == null)
		{
			Debug.LogFormat("<color=red>[{0}] Room is null</color>", new object[]
			{
				this
			});
		}
	}

	// Token: 0x060032F1 RID: 13041 RVA: 0x000AC366 File Offset: 0x000AA566
	public void SetDestination(Tunnel tunnel)
	{
		this.Destination = tunnel;
	}

	// Token: 0x060032F2 RID: 13042 RVA: 0x000AC36F File Offset: 0x000AA56F
	public void SetIndex(int index)
	{
		this.Index = index;
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x000AC378 File Offset: 0x000AA578
	public void SetTransitionType(TransitionID transitionType)
	{
		this.TransitionType = transitionType;
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x000AC381 File Offset: 0x000AA581
	public virtual void SetIsLocked(bool isLocked)
	{
		this.IsLocked = isLocked;
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(!isLocked);
		}
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x000AC3A6 File Offset: 0x000AA5A6
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x060032F6 RID: 13046 RVA: 0x000AC3AF File Offset: 0x000AA5AF
	public void SetIsVisible(bool isVisible)
	{
		if (this.m_visuals)
		{
			this.m_visuals.SetActive(isVisible);
		}
	}

	// Token: 0x060032F9 RID: 13049 RVA: 0x000AC3ED File Offset: 0x000AA5ED
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040027B3 RID: 10163
	private static Vector3 OFFSET = new Vector3(0f, 0.1f, 0f);

	// Token: 0x040027B4 RID: 10164
	[SerializeField]
	protected Interactable m_interactable;

	// Token: 0x040027B5 RID: 10165
	[SerializeField]
	private GameObject m_visuals;

	// Token: 0x040027B6 RID: 10166
	[SerializeField]
	private UnityEvent m_enterUnityEvent;

	// Token: 0x040027B7 RID: 10167
	[SerializeField]
	private UnityEvent m_exitUnityEvent;

	// Token: 0x040027B8 RID: 10168
	private BaseRoom m_room;

	// Token: 0x040027BD RID: 10173
	private Tunnel m_destination;

	// Token: 0x040027BE RID: 10174
	protected Tunnel m_tempDestinationOverride;
}
