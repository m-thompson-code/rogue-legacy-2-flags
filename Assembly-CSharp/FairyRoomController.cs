using System;
using System.Collections.Generic;
using System.Linq;
using RLAudio;
using UnityEngine;

// Token: 0x020006FC RID: 1788
public class FairyRoomController : BaseSpecialRoomController
{
	// Token: 0x1700147D RID: 5245
	// (get) Token: 0x06003683 RID: 13955 RVA: 0x0001DFF1 File Offset: 0x0001C1F1
	// (set) Token: 0x06003684 RID: 13956 RVA: 0x0001DFF9 File Offset: 0x0001C1F9
	public FairyRoomState State
	{
		get
		{
			return this.m_state;
		}
		private set
		{
			this.m_state = value;
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.FairyRoomStateChange, this, this.m_fairyRoomEnteredEventArgs);
		}
	}

	// Token: 0x1700147E RID: 5246
	// (get) Token: 0x06003685 RID: 13957 RVA: 0x0001E010 File Offset: 0x0001C210
	// (set) Token: 0x06003686 RID: 13958 RVA: 0x0001E018 File Offset: 0x0001C218
	public List<FairyRoomRuleEntry> FairyRoomRuleEntries
	{
		get
		{
			return this.m_fairyRoomRuleEntries;
		}
		set
		{
			this.m_fairyRoomRuleEntries = value;
		}
	}

	// Token: 0x1700147F RID: 5247
	// (get) Token: 0x06003687 RID: 13959 RVA: 0x0001E021 File Offset: 0x0001C221
	// (set) Token: 0x06003688 RID: 13960 RVA: 0x0001E029 File Offset: 0x0001C229
	public GameObject FairyRuleLocation
	{
		get
		{
			return this.m_fairyRuleLocation;
		}
		set
		{
			this.m_fairyRuleLocation = value;
		}
	}

	// Token: 0x17001480 RID: 5248
	// (get) Token: 0x06003689 RID: 13961 RVA: 0x0001E032 File Offset: 0x0001C232
	// (set) Token: 0x0600368A RID: 13962 RVA: 0x0001E03A File Offset: 0x0001C23A
	public ChestSpawnController ChestSpawnController
	{
		get
		{
			return this.m_chestSpawnController;
		}
		set
		{
			this.m_chestSpawnController = value;
		}
	}

	// Token: 0x17001481 RID: 5249
	// (get) Token: 0x0600368B RID: 13963 RVA: 0x0001E043 File Offset: 0x0001C243
	// (set) Token: 0x0600368C RID: 13964 RVA: 0x0001E04B File Offset: 0x0001C24B
	public PropSpawnController FairyRoomTriggerSpawner
	{
		get
		{
			return this.m_fairyRoomTriggerSpawner;
		}
		set
		{
			this.m_fairyRoomTriggerSpawner = value;
		}
	}

	// Token: 0x17001482 RID: 5250
	// (get) Token: 0x0600368D RID: 13965 RVA: 0x0001E054 File Offset: 0x0001C254
	// (set) Token: 0x0600368E RID: 13966 RVA: 0x0001E05C File Offset: 0x0001C25C
	public GameObject RoomTriggerWall
	{
		get
		{
			return this.m_roomTriggerWall;
		}
		set
		{
			this.m_roomTriggerWall = value;
		}
	}

	// Token: 0x17001483 RID: 5251
	// (get) Token: 0x0600368F RID: 13967 RVA: 0x0001E065 File Offset: 0x0001C265
	// (set) Token: 0x06003690 RID: 13968 RVA: 0x0001E06D File Offset: 0x0001C26D
	public ChestObj Chest { get; private set; }

	// Token: 0x17001484 RID: 5252
	// (get) Token: 0x06003691 RID: 13969 RVA: 0x00006CB3 File Offset: 0x00004EB3
	public override SpecialRoomType SpecialRoomType
	{
		get
		{
			return SpecialRoomType.Fairy;
		}
	}

	// Token: 0x17001485 RID: 5253
	// (get) Token: 0x06003692 RID: 13970 RVA: 0x0001E076 File Offset: 0x0001C276
	public bool IsHiddenFairyRoom
	{
		get
		{
			return this.m_isHiddenFairyRoom;
		}
	}

	// Token: 0x06003693 RID: 13971 RVA: 0x000E3FF0 File Offset: 0x000E21F0
	protected override void Awake()
	{
		base.Awake();
		this.m_isHiddenFairyRoom = false;
		if (this.FairyRoomRuleEntries != null)
		{
			using (List<FairyRoomRuleEntry>.Enumerator enumerator = this.FairyRoomRuleEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.FairyRuleID == FairyRuleID.HiddenChest)
					{
						this.m_isHiddenFairyRoom = true;
						break;
					}
				}
			}
		}
		this.m_fairyRoomEnteredEventArgs = new FairyRoomEnteredEventArgs(this);
		this.m_onPlayerOpenedChest = new Action(this.OnPlayerOpenedChest);
		this.m_onFairyRoomRuleStateChange = new Action<FairyRoomRuleStateChangeEventArgs>(this.OnFairyRoomRuleStateChange);
		this.m_onPlayerDeath = new Action<object, EventArgs>(this.OnPlayerDeath);
	}

	// Token: 0x06003694 RID: 13972 RVA: 0x0001E07E File Offset: 0x0001C27E
	private void OnEnable()
	{
		this.SubscribeToFairyRuleStateChangeEvents(true);
	}

	// Token: 0x06003695 RID: 13973 RVA: 0x0001E087 File Offset: 0x0001C287
	private void OnDisable()
	{
		this.SubscribeToFairyRuleStateChangeEvents(false);
	}

	// Token: 0x06003696 RID: 13974 RVA: 0x000E40A4 File Offset: 0x000E22A4
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		FairyRoomAudioEventController.DisableAudio = true;
		if (!this.Chest)
		{
			this.Chest = this.m_chestSpawnController.ChestInstance;
			if (!this.Chest && base.Room)
			{
				Debug.LogFormat("<color=red>[{0}] No Chest was spawned in Room ({0}). Fairy Rooms require a Chest</color>", new object[]
				{
					base.Room.gameObject.name
				});
			}
		}
		this.Chest.OnOpenedRelay.AddListener(this.m_onPlayerOpenedChest, false);
		this.Chest.Interactable.SetIsInteractableActive(false);
		this.Chest.Interactable.ForceDisableInteractPrompt(false);
		this.Chest.SetOpacity(0f);
		if (this.m_roomTriggerWall)
		{
			this.m_roomTriggerWall.SetActive(true);
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerEnterFairyRoom, this, this.m_fairyRoomEnteredEventArgs);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		FairyRoomController.m_deactivatedEnemies_STATIC.Clear();
		if (!this.m_isHiddenFairyRoom)
		{
			this.DeactivateAllEnemies();
		}
		if (!base.IsRoomComplete)
		{
			if (!this.m_fairyRoomTriggerSpawner)
			{
				this.StartFairyRoom();
			}
		}
		else
		{
			this.FairyRoomTriggered();
			if (this.Chest.IsOpen)
			{
				this.SetAllFairyRulesPassed();
			}
			else
			{
				this.SetAllFairyRulesFailed();
			}
		}
		FairyRoomAudioEventController.DisableAudio = false;
	}

	// Token: 0x06003697 RID: 13975 RVA: 0x000E41F4 File Offset: 0x000E23F4
	private void FairyRoomTriggered()
	{
		if (this.m_roomTriggerWall)
		{
			this.m_roomTriggerWall.SetActive(false);
		}
		this.Chest.SetOpacity(1f);
		if (!this.Chest.IsOpen)
		{
			this.Chest.Interactable.SetIsInteractableActive(true);
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerFairyRoomTriggered, this, this.m_fairyRoomEnteredEventArgs);
	}

	// Token: 0x06003698 RID: 13976 RVA: 0x000E4258 File Offset: 0x000E2458
	public void StartFairyRoom()
	{
		if (this.State != FairyRoomState.Failed && this.State != FairyRoomState.Passed && this.FairyRoomRuleEntries != null && this.FairyRoomRuleEntries.Count > 0)
		{
			this.FairyRoomTriggered();
			this.ActivateAllEnemies();
			if (this.State == FairyRoomState.NotRunning)
			{
				this.State = FairyRoomState.Running;
				if (this.Chest)
				{
					if (this.FairyRoomRuleEntries.Any((FairyRoomRuleEntry entry) => entry.FairyRule.LockChestAtStart))
					{
						this.Chest.SetChestLockState(ChestLockState.Locked);
					}
					else
					{
						this.Chest.SetChestLockState(ChestLockState.Unlocked);
					}
				}
				this.RunAllFairyRules(true);
				return;
			}
			if (this.State == FairyRoomState.Running)
			{
				Debug.LogFormat("<color=red>[{0}] Fairy Room Controller's State was (Running) when Player entered Room. Valid States are (NotRunning, Passed and Failed)</color>", new object[]
				{
					this
				});
			}
		}
	}

	// Token: 0x06003699 RID: 13977 RVA: 0x0001E090 File Offset: 0x0001C290
	private void OnPlayerOpenedChest()
	{
		this.SetAllFairyRulesPassed();
		this.RoomCompleted();
	}

	// Token: 0x0600369A RID: 13978 RVA: 0x000E4330 File Offset: 0x000E2530
	protected override void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (this.Chest)
		{
			this.Chest.OnOpenedRelay.RemoveListener(this.m_onPlayerOpenedChest);
		}
		if (!this.m_isHiddenFairyRoom)
		{
			if (this.State == FairyRoomState.Running)
			{
				this.SetAllFairyRulesFailed();
			}
		}
		else if (this.FairyRoomRuleEntries != null && this.FairyRoomRuleEntries.Count > 0)
		{
			if (this.State != FairyRoomState.Failed && this.State != FairyRoomState.Passed)
			{
				this.State = FairyRoomState.NotRunning;
			}
			this.RunAllFairyRules(false);
			this.ResetAllFairyRules();
		}
		this.OnExitReactivateEnemies();
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerExitFairyRoom, this, this.m_fairyRoomEnteredEventArgs);
	}

	// Token: 0x0600369B RID: 13979 RVA: 0x000E43D0 File Offset: 0x000E25D0
	private void OnFairyRoomRuleStateChange(FairyRoomRuleStateChangeEventArgs eventArgs)
	{
		if (eventArgs.Rule.State == FairyRoomState.Passed)
		{
			bool flag = false;
			using (List<FairyRoomRuleEntry>.Enumerator enumerator = this.FairyRoomRuleEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.FairyRule.State == FairyRoomState.Running)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.PlayerPassed(true);
			}
			else
			{
				bool flag2 = false;
				foreach (FairyRoomRuleEntry fairyRoomRuleEntry in this.FairyRoomRuleEntries)
				{
					if (fairyRoomRuleEntry.FairyRule.State == FairyRoomState.Running && fairyRoomRuleEntry.FairyRule.LockChestAtStart)
					{
						flag2 = true;
						break;
					}
				}
				if (this.Chest)
				{
					if (flag2)
					{
						this.Chest.SetChestLockState(ChestLockState.Locked);
					}
					else
					{
						this.Chest.SetChestLockState(ChestLockState.Unlocked);
					}
				}
			}
		}
		else if (eventArgs.Rule.State == FairyRoomState.Failed)
		{
			this.PlayerFailed(true);
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.FairyRoomRuleStateChange, this, eventArgs);
	}

	// Token: 0x0600369C RID: 13980 RVA: 0x000E44F8 File Offset: 0x000E26F8
	private void SubscribeToFairyRuleStateChangeEvents(bool addListener)
	{
		foreach (FairyRoomRuleEntry fairyRoomRuleEntry in this.FairyRoomRuleEntries)
		{
			if (fairyRoomRuleEntry.FairyRule != null)
			{
				if (addListener)
				{
					fairyRoomRuleEntry.FairyRule.StateChangeRelay.AddListener(this.m_onFairyRoomRuleStateChange, false);
				}
				else
				{
					fairyRoomRuleEntry.FairyRule.StateChangeRelay.RemoveListener(this.m_onFairyRoomRuleStateChange);
				}
			}
		}
		if (addListener)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
			return;
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
	}

	// Token: 0x0600369D RID: 13981 RVA: 0x000E45A4 File Offset: 0x000E27A4
	private void SetAllFairyRulesFailed()
	{
		foreach (FairyRoomRuleEntry fairyRoomRuleEntry in this.FairyRoomRuleEntries)
		{
			if (fairyRoomRuleEntry.FairyRule != null)
			{
				fairyRoomRuleEntry.FairyRule.SetIsFailed();
			}
		}
	}

	// Token: 0x0600369E RID: 13982 RVA: 0x000E460C File Offset: 0x000E280C
	private void SetAllFairyRulesPassed()
	{
		foreach (FairyRoomRuleEntry fairyRoomRuleEntry in this.FairyRoomRuleEntries)
		{
			if (fairyRoomRuleEntry.FairyRule != null)
			{
				fairyRoomRuleEntry.FairyRule.SetIsPassed();
			}
		}
		this.SubscribeToFairyRuleStateChangeEvents(false);
	}

	// Token: 0x0600369F RID: 13983 RVA: 0x000E4678 File Offset: 0x000E2878
	private void DeactivateAllEnemies()
	{
		foreach (EnemySpawnController enemySpawnController in base.Room.SpawnControllerManager.EnemySpawnControllers)
		{
			if (enemySpawnController.EnemyInstance && !enemySpawnController.IsDead)
			{
				EnemyController enemyInstance = enemySpawnController.EnemyInstance;
				if (enemyInstance.IsCulled && enemyInstance.IsInitialized)
				{
					base.StartCoroutine(EnemyManager.SetEnemyCulled(enemyInstance, false, false));
				}
				enemyInstance.DisableCulling = true;
				enemyInstance.HitboxController.DisableAllCollisions = true;
				enemyInstance.Animator.enabled = false;
				enemyInstance.CharacterCorgi.enabled = false;
				enemyInstance.LogicController.LogicScript.enabled = false;
				enemyInstance.LogicController.StopAllLogic(true);
				enemyInstance.SetVelocity(0f, 0f, false);
				enemyInstance.LogicController.enabled = false;
				enemyInstance.Visuals.SetActive(false);
				enemyInstance.ActivatedByFairyRoomTrigger = true;
				FairyRoomController.m_deactivatedEnemies_STATIC.Add(enemyInstance);
			}
		}
		this.m_enemiesDeactivated = true;
	}

	// Token: 0x060036A0 RID: 13984 RVA: 0x000E477C File Offset: 0x000E297C
	private void OnExitReactivateEnemies()
	{
		foreach (EnemyController enemyController in FairyRoomController.m_deactivatedEnemies_STATIC)
		{
			enemyController.DisableCulling = false;
			enemyController.HitboxController.DisableAllCollisions = false;
			enemyController.Animator.enabled = true;
			enemyController.CharacterCorgi.enabled = true;
			enemyController.LogicController.LogicScript.enabled = true;
			enemyController.StatusEffectController.SetAllStatusEffectsHidden(false);
			enemyController.LogicController.enabled = true;
			enemyController.Visuals.SetActive(true);
			enemyController.ActivatedByFairyRoomTrigger = false;
			enemyController.gameObject.SetActive(false);
		}
		FairyRoomController.m_deactivatedEnemies_STATIC.Clear();
	}

	// Token: 0x060036A1 RID: 13985 RVA: 0x0001E09E File Offset: 0x0001C29E
	private void OnPlayerDeath(object sender, EventArgs args)
	{
		this.OnExitReactivateEnemies();
	}

	// Token: 0x060036A2 RID: 13986 RVA: 0x000E4848 File Offset: 0x000E2A48
	private void ActivateAllEnemies()
	{
		if (!this.m_enemiesDeactivated)
		{
			return;
		}
		foreach (EnemySpawnController enemySpawnController in base.Room.SpawnControllerManager.EnemySpawnControllers)
		{
			if (!enemySpawnController.IsNativeNull() && !enemySpawnController.IsDead)
			{
				EnemyController enemyInstance = enemySpawnController.EnemyInstance;
				enemyInstance.Animator.enabled = true;
				enemyInstance.CharacterCorgi.enabled = true;
				enemyInstance.LogicController.LogicScript.enabled = true;
				enemyInstance.LogicController.enabled = true;
				enemyInstance.ForceFaceTarget();
				base.StartCoroutine(EnemyManager.RunSummonAnimCoroutine(enemyInstance, 3f, true, 1f));
			}
		}
		this.m_enemiesDeactivated = false;
	}

	// Token: 0x060036A3 RID: 13987 RVA: 0x000E48F4 File Offset: 0x000E2AF4
	private void RunAllFairyRules(bool areRunning)
	{
		foreach (FairyRoomRuleEntry fairyRoomRuleEntry in this.FairyRoomRuleEntries)
		{
			if (fairyRoomRuleEntry.FairyRule != null)
			{
				if (areRunning)
				{
					fairyRoomRuleEntry.FairyRule.RunRule(this);
				}
				else
				{
					fairyRoomRuleEntry.FairyRule.StopRule();
				}
			}
		}
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x000E496C File Offset: 0x000E2B6C
	private void ResetAllFairyRules()
	{
		foreach (FairyRoomRuleEntry fairyRoomRuleEntry in this.FairyRoomRuleEntries)
		{
			if (fairyRoomRuleEntry.FairyRule != null)
			{
				fairyRoomRuleEntry.FairyRule.ResetRule();
			}
		}
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x000E49D4 File Offset: 0x000E2BD4
	private void PlayerPassed(bool setRoomComplete = true)
	{
		this.State = FairyRoomState.Passed;
		if (this.Chest && this.Chest.IsLocked && !this.Chest.IsOpen)
		{
			this.Chest.SetChestLockState(ChestLockState.Unlocked);
		}
		this.RunAllFairyRules(false);
		if (setRoomComplete)
		{
			this.RoomCompleted();
		}
	}

	// Token: 0x060036A6 RID: 13990 RVA: 0x000E4A2C File Offset: 0x000E2C2C
	private void PlayerFailed(bool setRoomComplete = true)
	{
		this.State = FairyRoomState.Failed;
		if (this.Chest && this.Chest.LockState != ChestLockState.Failed && !this.Chest.IsOpen)
		{
			this.Chest.SetChestLockState(ChestLockState.Failed);
		}
		this.RunAllFairyRules(false);
		if (setRoomComplete)
		{
			this.RoomCompleted();
		}
	}

	// Token: 0x060036A7 RID: 13991 RVA: 0x0001E0A6 File Offset: 0x0001C2A6
	protected override void OnRoomDataSaved(bool exitingToMainMenu)
	{
		if (!this.m_isHiddenFairyRoom && this.State == FairyRoomState.Running && !exitingToMainMenu)
		{
			base.IsRoomComplete = true;
		}
		base.OnRoomDataSaved(exitingToMainMenu);
	}

	// Token: 0x04002C39 RID: 11321
	public static EnemyType[] EnemyExceptionArray = new EnemyType[]
	{
		EnemyType.BouncySpike,
		EnemyType.Eggplant
	};

	// Token: 0x04002C3A RID: 11322
	[SerializeField]
	private List<FairyRoomRuleEntry> m_fairyRoomRuleEntries;

	// Token: 0x04002C3B RID: 11323
	[SerializeField]
	private GameObject m_fairyRuleLocation;

	// Token: 0x04002C3C RID: 11324
	[SerializeField]
	private ChestSpawnController m_chestSpawnController;

	// Token: 0x04002C3D RID: 11325
	[SerializeField]
	private PropSpawnController m_fairyRoomTriggerSpawner;

	// Token: 0x04002C3E RID: 11326
	[SerializeField]
	private GameObject m_roomTriggerWall;

	// Token: 0x04002C3F RID: 11327
	private FairyRoomState m_state;

	// Token: 0x04002C40 RID: 11328
	private bool m_enemiesDeactivated;

	// Token: 0x04002C41 RID: 11329
	private bool m_isHiddenFairyRoom;

	// Token: 0x04002C42 RID: 11330
	private FairyRoomEnteredEventArgs m_fairyRoomEnteredEventArgs;

	// Token: 0x04002C43 RID: 11331
	private static List<EnemyController> m_deactivatedEnemies_STATIC = new List<EnemyController>();

	// Token: 0x04002C44 RID: 11332
	private Action m_onPlayerOpenedChest;

	// Token: 0x04002C45 RID: 11333
	private Action<FairyRoomRuleStateChangeEventArgs> m_onFairyRoomRuleStateChange;

	// Token: 0x04002C46 RID: 11334
	private Action<object, EventArgs> m_onPlayerDeath;
}
