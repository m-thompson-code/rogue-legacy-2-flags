using System;
using System.Collections;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200093E RID: 2366
public class Tunnel : MonoBehaviour, IRoomConsumer, IRootObj
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x060047A5 RID: 18341 RVA: 0x001166C4 File Offset: 0x001148C4
	// (remove) Token: 0x060047A6 RID: 18342 RVA: 0x001166FC File Offset: 0x001148FC
	public event EventHandler<EventArgs> DestroyedEvent;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x060047A7 RID: 18343 RVA: 0x00116734 File Offset: 0x00114934
	// (remove) Token: 0x060047A8 RID: 18344 RVA: 0x0011676C File Offset: 0x0011496C
	public event EventHandler<EventArgs> PlayerEnteredEvent;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060047A9 RID: 18345 RVA: 0x001167A4 File Offset: 0x001149A4
	// (remove) Token: 0x060047AA RID: 18346 RVA: 0x001167DC File Offset: 0x001149DC
	public event EventHandler<EventArgs> PlayerExitedEvent;

	// Token: 0x1700192C RID: 6444
	// (get) Token: 0x060047AB RID: 18347 RVA: 0x0002740B File Offset: 0x0002560B
	public Interactable Interactable
	{
		get
		{
			return this.m_interactable;
		}
	}

	// Token: 0x1700192D RID: 6445
	// (get) Token: 0x060047AC RID: 18348 RVA: 0x00027413 File Offset: 0x00025613
	// (set) Token: 0x060047AD RID: 18349 RVA: 0x0002741B File Offset: 0x0002561B
	public bool IsInstantiated { get; private set; }

	// Token: 0x1700192E RID: 6446
	// (get) Token: 0x060047AE RID: 18350 RVA: 0x00027424 File Offset: 0x00025624
	protected bool IsCutsceneTeleport
	{
		get
		{
			return this.m_tempDestinationOverride;
		}
	}

	// Token: 0x1700192F RID: 6447
	// (get) Token: 0x060047AF RID: 18351 RVA: 0x00027431 File Offset: 0x00025631
	// (set) Token: 0x060047B0 RID: 18352 RVA: 0x0002744D File Offset: 0x0002564D
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

	// Token: 0x17001930 RID: 6448
	// (get) Token: 0x060047B1 RID: 18353 RVA: 0x00003713 File Offset: 0x00001913
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x17001931 RID: 6449
	// (get) Token: 0x060047B2 RID: 18354 RVA: 0x00027456 File Offset: 0x00025656
	// (set) Token: 0x060047B3 RID: 18355 RVA: 0x0002745E File Offset: 0x0002565E
	public int Index { get; private set; }

	// Token: 0x17001932 RID: 6450
	// (get) Token: 0x060047B4 RID: 18356 RVA: 0x00027467 File Offset: 0x00025667
	// (set) Token: 0x060047B5 RID: 18357 RVA: 0x0002746F File Offset: 0x0002566F
	public bool LeadsToRoot { get; private set; }

	// Token: 0x17001933 RID: 6451
	// (get) Token: 0x060047B6 RID: 18358 RVA: 0x00027478 File Offset: 0x00025678
	// (set) Token: 0x060047B7 RID: 18359 RVA: 0x00027480 File Offset: 0x00025680
	public bool IsLocked { get; protected set; }

	// Token: 0x17001934 RID: 6452
	// (get) Token: 0x060047B8 RID: 18360 RVA: 0x00027489 File Offset: 0x00025689
	// (set) Token: 0x060047B9 RID: 18361 RVA: 0x00027491 File Offset: 0x00025691
	public TransitionID TransitionType { get; private set; }

	// Token: 0x17001935 RID: 6453
	// (get) Token: 0x060047BA RID: 18362 RVA: 0x0002749A File Offset: 0x0002569A
	// (set) Token: 0x060047BB RID: 18363 RVA: 0x000274A2 File Offset: 0x000256A2
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

	// Token: 0x17001936 RID: 6454
	// (get) Token: 0x060047BC RID: 18364 RVA: 0x000274AB File Offset: 0x000256AB
	// (set) Token: 0x060047BD RID: 18365 RVA: 0x000274B3 File Offset: 0x000256B3
	public Tunnel Root { get; private set; }

	// Token: 0x17001937 RID: 6455
	// (get) Token: 0x060047BE RID: 18366 RVA: 0x000274BC File Offset: 0x000256BC
	// (set) Token: 0x060047BF RID: 18367 RVA: 0x000274C4 File Offset: 0x000256C4
	public Animator Animator { get; private set; }

	// Token: 0x060047C0 RID: 18368 RVA: 0x000274CD File Offset: 0x000256CD
	protected virtual void Awake()
	{
		this.Animator = base.GetComponent<Animator>();
	}

	// Token: 0x060047C1 RID: 18369 RVA: 0x000274DB File Offset: 0x000256DB
	private void OnDestroy()
	{
		if (this.DestroyedEvent != null)
		{
			this.DestroyedEvent(this, EventArgs.Empty);
		}
	}

	// Token: 0x060047C2 RID: 18370 RVA: 0x000274F6 File Offset: 0x000256F6
	private void OnDisable()
	{
		if (this.m_interactable)
		{
			this.m_interactable.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.OnPlayerInteractedWithTunnel));
		}
	}

	// Token: 0x060047C3 RID: 18371 RVA: 0x00027522 File Offset: 0x00025722
	protected virtual void OnEnable()
	{
		if (this.m_interactable)
		{
			this.m_interactable.TriggerOnEnterEvent.AddListener(new UnityAction<GameObject>(this.OnPlayerInteractedWithTunnel));
		}
	}

	// Token: 0x060047C4 RID: 18372 RVA: 0x0002754E File Offset: 0x0002574E
	public void Initialise()
	{
		this.IsInstantiated = true;
	}

	// Token: 0x060047C5 RID: 18373 RVA: 0x00027557 File Offset: 0x00025757
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

	// Token: 0x060047C6 RID: 18374 RVA: 0x00116814 File Offset: 0x00114A14
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

	// Token: 0x060047C7 RID: 18375 RVA: 0x0002757C File Offset: 0x0002577C
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

	// Token: 0x060047C8 RID: 18376 RVA: 0x00116850 File Offset: 0x00114A50
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

	// Token: 0x060047C9 RID: 18377 RVA: 0x0002758B File Offset: 0x0002578B
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

	// Token: 0x060047CA RID: 18378 RVA: 0x001168A4 File Offset: 0x00114AA4
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

	// Token: 0x060047CB RID: 18379 RVA: 0x00116990 File Offset: 0x00114B90
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

	// Token: 0x060047CC RID: 18380 RVA: 0x000275C3 File Offset: 0x000257C3
	public void SetDestination(Tunnel tunnel)
	{
		this.Destination = tunnel;
	}

	// Token: 0x060047CD RID: 18381 RVA: 0x000275CC File Offset: 0x000257CC
	public void SetIndex(int index)
	{
		this.Index = index;
	}

	// Token: 0x060047CE RID: 18382 RVA: 0x000275D5 File Offset: 0x000257D5
	public void SetTransitionType(TransitionID transitionType)
	{
		this.TransitionType = transitionType;
	}

	// Token: 0x060047CF RID: 18383 RVA: 0x000275DE File Offset: 0x000257DE
	public virtual void SetIsLocked(bool isLocked)
	{
		this.IsLocked = isLocked;
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(!isLocked);
		}
	}

	// Token: 0x060047D0 RID: 18384 RVA: 0x00027603 File Offset: 0x00025803
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x060047D1 RID: 18385 RVA: 0x0002760C File Offset: 0x0002580C
	public void SetIsVisible(bool isVisible)
	{
		if (this.m_visuals)
		{
			this.m_visuals.SetActive(isVisible);
		}
	}

	// Token: 0x060047D4 RID: 18388 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040036E4 RID: 14052
	private static Vector3 OFFSET = new Vector3(0f, 0.1f, 0f);

	// Token: 0x040036E5 RID: 14053
	[SerializeField]
	protected Interactable m_interactable;

	// Token: 0x040036E6 RID: 14054
	[SerializeField]
	private GameObject m_visuals;

	// Token: 0x040036E7 RID: 14055
	[SerializeField]
	private UnityEvent m_enterUnityEvent;

	// Token: 0x040036E8 RID: 14056
	[SerializeField]
	private UnityEvent m_exitUnityEvent;

	// Token: 0x040036E9 RID: 14057
	private BaseRoom m_room;

	// Token: 0x040036EE RID: 14062
	private Tunnel m_destination;

	// Token: 0x040036EF RID: 14063
	protected Tunnel m_tempDestinationOverride;
}
