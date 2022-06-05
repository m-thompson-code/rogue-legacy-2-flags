using System;
using System.Collections.Generic;
using System.Linq;
using RLAudio;
using UnityEngine;

// Token: 0x02000435 RID: 1077
public class FairyRoomController : BaseSpecialRoomController
{
	// Token: 0x17000F9C RID: 3996
	// (get) Token: 0x0600277A RID: 10106 RVA: 0x000832D4 File Offset: 0x000814D4
	// (set) Token: 0x0600277B RID: 10107 RVA: 0x000832DC File Offset: 0x000814DC
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

	// Token: 0x17000F9D RID: 3997
	// (get) Token: 0x0600277C RID: 10108 RVA: 0x000832F3 File Offset: 0x000814F3
	// (set) Token: 0x0600277D RID: 10109 RVA: 0x000832FB File Offset: 0x000814FB
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

	// Token: 0x17000F9E RID: 3998
	// (get) Token: 0x0600277E RID: 10110 RVA: 0x00083304 File Offset: 0x00081504
	// (set) Token: 0x0600277F RID: 10111 RVA: 0x0008330C File Offset: 0x0008150C
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

	// Token: 0x17000F9F RID: 3999
	// (get) Token: 0x06002780 RID: 10112 RVA: 0x00083315 File Offset: 0x00081515
	// (set) Token: 0x06002781 RID: 10113 RVA: 0x0008331D File Offset: 0x0008151D
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

	// Token: 0x17000FA0 RID: 4000
	// (get) Token: 0x06002782 RID: 10114 RVA: 0x00083326 File Offset: 0x00081526
	// (set) Token: 0x06002783 RID: 10115 RVA: 0x0008332E File Offset: 0x0008152E
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

	// Token: 0x17000FA1 RID: 4001
	// (get) Token: 0x06002784 RID: 10116 RVA: 0x00083337 File Offset: 0x00081537
	// (set) Token: 0x06002785 RID: 10117 RVA: 0x0008333F File Offset: 0x0008153F
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

	// Token: 0x17000FA2 RID: 4002
	// (get) Token: 0x06002786 RID: 10118 RVA: 0x00083348 File Offset: 0x00081548
	// (set) Token: 0x06002787 RID: 10119 RVA: 0x00083350 File Offset: 0x00081550
	public ChestObj Chest { get; private set; }

	// Token: 0x17000FA3 RID: 4003
	// (get) Token: 0x06002788 RID: 10120 RVA: 0x00083359 File Offset: 0x00081559
	public override SpecialRoomType SpecialRoomType
	{
		get
		{
			return SpecialRoomType.Fairy;
		}
	}

	// Token: 0x17000FA4 RID: 4004
	// (get) Token: 0x06002789 RID: 10121 RVA: 0x0008335D File Offset: 0x0008155D
	public bool IsHiddenFairyRoom
	{
		get
		{
			return this.m_isHiddenFairyRoom;
		}
	}

	// Token: 0x0600278A RID: 10122 RVA: 0x00083368 File Offset: 0x00081568
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

	// Token: 0x0600278B RID: 10123 RVA: 0x0008341C File Offset: 0x0008161C
	private void OnEnable()
	{
		this.SubscribeToFairyRuleStateChangeEvents(true);
	}

	// Token: 0x0600278C RID: 10124 RVA: 0x00083425 File Offset: 0x00081625
	private void OnDisable()
	{
		this.SubscribeToFairyRuleStateChangeEvents(false);
	}

	// Token: 0x0600278D RID: 10125 RVA: 0x00083430 File Offset: 0x00081630
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

	// Token: 0x0600278E RID: 10126 RVA: 0x00083580 File Offset: 0x00081780
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

	// Token: 0x0600278F RID: 10127 RVA: 0x000835E4 File Offset: 0x000817E4
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

	// Token: 0x06002790 RID: 10128 RVA: 0x000836BC File Offset: 0x000818BC
	private void OnPlayerOpenedChest()
	{
		this.SetAllFairyRulesPassed();
		this.RoomCompleted();
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x000836CC File Offset: 0x000818CC
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

	// Token: 0x06002792 RID: 10130 RVA: 0x0008376C File Offset: 0x0008196C
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

	// Token: 0x06002793 RID: 10131 RVA: 0x00083894 File Offset: 0x00081A94
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

	// Token: 0x06002794 RID: 10132 RVA: 0x00083940 File Offset: 0x00081B40
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

	// Token: 0x06002795 RID: 10133 RVA: 0x000839A8 File Offset: 0x00081BA8
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

	// Token: 0x06002796 RID: 10134 RVA: 0x00083A14 File Offset: 0x00081C14
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

	// Token: 0x06002797 RID: 10135 RVA: 0x00083B18 File Offset: 0x00081D18
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

	// Token: 0x06002798 RID: 10136 RVA: 0x00083BE4 File Offset: 0x00081DE4
	private void OnPlayerDeath(object sender, EventArgs args)
	{
		this.OnExitReactivateEnemies();
	}

	// Token: 0x06002799 RID: 10137 RVA: 0x00083BEC File Offset: 0x00081DEC
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

	// Token: 0x0600279A RID: 10138 RVA: 0x00083C98 File Offset: 0x00081E98
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

	// Token: 0x0600279B RID: 10139 RVA: 0x00083D10 File Offset: 0x00081F10
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

	// Token: 0x0600279C RID: 10140 RVA: 0x00083D78 File Offset: 0x00081F78
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

	// Token: 0x0600279D RID: 10141 RVA: 0x00083DD0 File Offset: 0x00081FD0
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

	// Token: 0x0600279E RID: 10142 RVA: 0x00083E29 File Offset: 0x00082029
	protected override void OnRoomDataSaved(bool exitingToMainMenu)
	{
		if (!this.m_isHiddenFairyRoom && this.State == FairyRoomState.Running && !exitingToMainMenu)
		{
			base.IsRoomComplete = true;
		}
		base.OnRoomDataSaved(exitingToMainMenu);
	}

	// Token: 0x0400210E RID: 8462
	public static EnemyType[] EnemyExceptionArray = new EnemyType[]
	{
		EnemyType.BouncySpike,
		EnemyType.Eggplant
	};

	// Token: 0x0400210F RID: 8463
	[SerializeField]
	private List<FairyRoomRuleEntry> m_fairyRoomRuleEntries;

	// Token: 0x04002110 RID: 8464
	[SerializeField]
	private GameObject m_fairyRuleLocation;

	// Token: 0x04002111 RID: 8465
	[SerializeField]
	private ChestSpawnController m_chestSpawnController;

	// Token: 0x04002112 RID: 8466
	[SerializeField]
	private PropSpawnController m_fairyRoomTriggerSpawner;

	// Token: 0x04002113 RID: 8467
	[SerializeField]
	private GameObject m_roomTriggerWall;

	// Token: 0x04002114 RID: 8468
	private FairyRoomState m_state;

	// Token: 0x04002115 RID: 8469
	private bool m_enemiesDeactivated;

	// Token: 0x04002116 RID: 8470
	private bool m_isHiddenFairyRoom;

	// Token: 0x04002117 RID: 8471
	private FairyRoomEnteredEventArgs m_fairyRoomEnteredEventArgs;

	// Token: 0x04002118 RID: 8472
	private static List<EnemyController> m_deactivatedEnemies_STATIC = new List<EnemyController>();

	// Token: 0x04002119 RID: 8473
	private Action m_onPlayerOpenedChest;

	// Token: 0x0400211A RID: 8474
	private Action<FairyRoomRuleStateChangeEventArgs> m_onFairyRoomRuleStateChange;

	// Token: 0x0400211B RID: 8475
	private Action<object, EventArgs> m_onPlayerDeath;
}
