using System;
using System.Collections.Generic;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020004B6 RID: 1206
public class RoomSaveController
{
	// Token: 0x1700112B RID: 4395
	// (get) Token: 0x06002CDD RID: 11485 RVA: 0x00097E2D File Offset: 0x0009602D
	public IRelayLink<bool> OnRoomDataSavedRelay
	{
		get
		{
			return this.m_onRoomDataSavedRelay.link;
		}
	}

	// Token: 0x1700112C RID: 4396
	// (get) Token: 0x06002CDE RID: 11486 RVA: 0x00097E3A File Offset: 0x0009603A
	public IRelayLink OnRoomDataLoadedRelay
	{
		get
		{
			return this.m_onRoomDataLoadedRelay.link;
		}
	}

	// Token: 0x1700112D RID: 4397
	// (get) Token: 0x06002CDF RID: 11487 RVA: 0x00097E47 File Offset: 0x00096047
	// (set) Token: 0x06002CE0 RID: 11488 RVA: 0x00097E4E File Offset: 0x0009604E
	public static bool DisableCutsceneSaving { get; set; }

	// Token: 0x1700112E RID: 4398
	// (get) Token: 0x06002CE1 RID: 11489 RVA: 0x00097E58 File Offset: 0x00096058
	public bool CanSaveRoom
	{
		get
		{
			return !RoomSaveController.DisableCutsceneSaving && this.Room.RoomType != RoomType.Boss && this.Room.SpecialRoomType != SpecialRoomType.Boss && this.Room.SpecialRoomType != SpecialRoomType.Heirloom && this.Room.SpecialRoomType != SpecialRoomType.Clown && !this.Room.DisableRoomSaving;
		}
	}

	// Token: 0x1700112F RID: 4399
	// (get) Token: 0x06002CE2 RID: 11490 RVA: 0x00097EB8 File Offset: 0x000960B8
	public BaseRoom Room { get; }

	// Token: 0x06002CE3 RID: 11491 RVA: 0x00097EC0 File Offset: 0x000960C0
	public RoomSaveController(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x06002CE4 RID: 11492 RVA: 0x00097EE8 File Offset: 0x000960E8
	public void OnPlayerEnter_LoadStageData()
	{
		this.InitializeBreakablePropsList();
		RoomSaveData roomSaveData = SaveManager.StageSaveData.GetRoomSaveData(this.Room.BiomeType, this.Room.BiomeControllerIndex);
		bool flag = false;
		if (roomSaveData != null && !roomSaveData.IsEmpty)
		{
			if (this.m_breakablePropsList != null && roomSaveData.BreakableStates != null && this.m_breakablePropsList.Count != roomSaveData.BreakableStates.Length)
			{
				flag = true;
			}
			else if (roomSaveData.ChestStates != null && this.Room.SpawnControllerManager.ChestSpawnControllers.Length != roomSaveData.ChestStates.Length)
			{
				flag = true;
			}
			else if (roomSaveData.EnemyStates != null && this.Room.SpawnControllerManager.EnemySpawnControllers.Length != roomSaveData.EnemyStates.Length)
			{
				flag = true;
			}
			if (!flag)
			{
				int num = 0;
				foreach (PropSpawnController propSpawnController in this.Room.SpawnControllerManager.PropSpawnControllers)
				{
					if (propSpawnController.CameraLayer == CameraLayer.Game && propSpawnController.BreakableDecoInstances != null)
					{
						num += propSpawnController.BreakableDecoInstances.Length;
					}
				}
				if (roomSaveData.DecoBreakableStates != null && num != roomSaveData.DecoBreakableStates.Length)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			string text = this.Room.gameObject.name;
			if (this.Room is Room)
			{
				text = (this.Room as Room).RoomID.ToString();
			}
			Debug.LogFormat("<color=red>WARNING: | Room ID - {0} | BiomeControllerIndex - {1} | Biome - {2} | Identified a save file mismatch with the room's actual spawners. Recreating Room Save Data</color>", new object[]
			{
				text,
				this.Room.BiomeControllerIndex,
				this.Room.BiomeType
			});
		}
		if (roomSaveData != null)
		{
			if (roomSaveData.IsEmpty || flag)
			{
				this.InitializeRoomSaveData(roomSaveData, flag);
				this.LoadRoomState(roomSaveData);
				return;
			}
			this.LoadRoomState(roomSaveData);
		}
	}

	// Token: 0x06002CE5 RID: 11493 RVA: 0x000980AC File Offset: 0x000962AC
	private void InitializeBreakablePropsList()
	{
		if (this.m_breakablePropsList == null)
		{
			this.m_breakablePropsList = new List<PropSpawnController>();
			foreach (PropSpawnController propSpawnController in this.Room.SpawnControllerManager.PropSpawnControllers)
			{
				if (!(propSpawnController.PropInstance == null) && propSpawnController.CameraLayer == CameraLayer.Game && propSpawnController.IsBreakable)
				{
					this.m_breakablePropsList.Add(propSpawnController);
				}
			}
		}
	}

	// Token: 0x06002CE6 RID: 11494 RVA: 0x0009811C File Offset: 0x0009631C
	private void InitializeRoomSaveData(RoomSaveData roomData, bool forceReinitialize)
	{
		if (forceReinitialize)
		{
			roomData.RoomVisited = false;
			roomData.IsRoomComplete = false;
		}
		if (roomData.BreakableStates == null || forceReinitialize)
		{
			roomData.BreakableStates = new RLBreakableSaveState[this.m_breakablePropsList.Count];
			for (int i = 0; i < this.m_breakablePropsList.Count; i++)
			{
				roomData.BreakableStates[i] = new RLBreakableSaveState();
			}
		}
		if (roomData.DecoBreakableStates == null || forceReinitialize)
		{
			int num = 0;
			foreach (PropSpawnController propSpawnController in this.Room.SpawnControllerManager.PropSpawnControllers)
			{
				if (propSpawnController.CameraLayer == CameraLayer.Game && propSpawnController.BreakableDecoInstances != null)
				{
					num += propSpawnController.BreakableDecoInstances.Length;
				}
			}
			roomData.DecoBreakableStates = new RLBreakableSaveState[num];
			for (int k = 0; k < num; k++)
			{
				roomData.DecoBreakableStates[k] = new RLBreakableSaveState();
			}
		}
		if (roomData.ChestStates == null || forceReinitialize)
		{
			int num2 = this.Room.SpawnControllerManager.ChestSpawnControllers.Length;
			roomData.ChestStates = new RLSaveState[num2];
			for (int l = 0; l < num2; l++)
			{
				roomData.ChestStates[l] = new RLSaveState();
			}
		}
		if (roomData.EnemyStates == null || forceReinitialize)
		{
			int num3 = this.Room.SpawnControllerManager.EnemySpawnControllers.Length;
			roomData.EnemyStates = new RLSaveState[num3];
			for (int m = 0; m < num3; m++)
			{
				roomData.EnemyStates[m] = new RLSaveState();
			}
		}
	}

	// Token: 0x06002CE7 RID: 11495 RVA: 0x00098290 File Offset: 0x00096490
	public void SaveRoomState(RoomSaveData roomData, bool exitingToMainMenu)
	{
		if (roomData == null)
		{
			return;
		}
		roomData.RoomVisited = true;
		for (int i = 0; i < this.m_breakablePropsList.Count; i++)
		{
			Prop propInstance = this.m_breakablePropsList[i].PropInstance;
			if (propInstance && propInstance.Breakable)
			{
				roomData.BreakableStates[i].IsStateActive = !propInstance.Breakable.IsBroken;
				roomData.BreakableStates[i].AttackerIsOnRight = propInstance.Breakable.AttackerIsOnRight;
			}
		}
		int num = 0;
		foreach (PropSpawnController propSpawnController in this.Room.SpawnControllerManager.PropSpawnControllers)
		{
			if (propSpawnController.CameraLayer == CameraLayer.Game && propSpawnController.BreakableDecoInstances != null)
			{
				foreach (Prop prop in propSpawnController.BreakableDecoInstances)
				{
					if (prop && prop.Breakable)
					{
						roomData.DecoBreakableStates[num].IsStateActive = !prop.Breakable.IsBroken;
						num++;
					}
				}
			}
		}
		ChestSpawnController[] chestSpawnControllers = this.Room.SpawnControllerManager.ChestSpawnControllers;
		for (int l = 0; l < chestSpawnControllers.Length; l++)
		{
			if (!chestSpawnControllers[l].ShouldSpawn || !chestSpawnControllers[l].ChestInstance)
			{
				roomData.ChestStates[l].IsSpawned = false;
				roomData.ChestStates[l].IsStateActive = false;
			}
			else
			{
				roomData.ChestStates[l].IsSpawned = true;
				roomData.ChestStates[l].IsStateActive = !chestSpawnControllers[l].ChestInstance.IsOpen;
			}
		}
		bool flag = false;
		if (this.Room.SpecialRoomType == SpecialRoomType.Fairy)
		{
			FairyRoomController component = this.Room.gameObject.GetComponent<FairyRoomController>();
			if (component && component.FairyRoomRuleEntries != null)
			{
				using (List<FairyRoomRuleEntry>.Enumerator enumerator = component.FairyRoomRuleEntries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.FairyRuleID == FairyRuleID.HiddenChest)
						{
							flag = true;
							break;
						}
					}
				}
			}
		}
		if (this.Room.SpecialRoomType != SpecialRoomType.Fairy || flag)
		{
			EnemySpawnController[] enemySpawnControllers = this.Room.SpawnControllerManager.EnemySpawnControllers;
			for (int m = 0; m < enemySpawnControllers.Length; m++)
			{
				roomData.EnemyStates[m].IsStateActive = !enemySpawnControllers[m].IsDead;
			}
		}
		this.m_onRoomDataSavedRelay.Dispatch(exitingToMainMenu);
	}

	// Token: 0x06002CE8 RID: 11496 RVA: 0x00098528 File Offset: 0x00096728
	public void LoadRoomState(RoomSaveData roomData)
	{
		if (roomData == null)
		{
			Debug.LogFormat("<color=red>| {0} | <b>LoadState</b> method was passed null Room Save Data</color>, this", Array.Empty<object>());
			return;
		}
		for (int i = 0; i < this.m_breakablePropsList.Count; i++)
		{
			Prop propInstance = this.m_breakablePropsList[i].PropInstance;
			if (propInstance != null)
			{
				propInstance.Breakable.AttackerIsOnRight = roomData.BreakableStates[i].AttackerIsOnRight;
				bool isBroken = !roomData.BreakableStates[i].IsStateActive;
				propInstance.Breakable.ForceBrokenState(isBroken);
			}
		}
		int num = 0;
		foreach (PropSpawnController propSpawnController in this.Room.SpawnControllerManager.PropSpawnControllers)
		{
			if (propSpawnController.CameraLayer == CameraLayer.Game && propSpawnController.BreakableDecoInstances != null)
			{
				Prop[] breakableDecoInstances = propSpawnController.BreakableDecoInstances;
				for (int k = 0; k < breakableDecoInstances.Length; k++)
				{
					breakableDecoInstances[k].Breakable.ForceBrokenState(!roomData.DecoBreakableStates[num].IsStateActive);
					num++;
				}
			}
		}
		ChestSpawnController[] chestSpawnControllers = this.Room.SpawnControllerManager.ChestSpawnControllers;
		for (int l = 0; l < chestSpawnControllers.Length; l++)
		{
			if (chestSpawnControllers[l].ShouldSpawn && !roomData.ChestStates[l].IsStateActive)
			{
				chestSpawnControllers[l].ChestInstance.ForceOpenChest();
			}
		}
		EnemySpawnController[] enemySpawnControllers = this.Room.SpawnControllerManager.EnemySpawnControllers;
		for (int m = 0; m < enemySpawnControllers.Length; m++)
		{
			if (!roomData.EnemyStates[m].IsStateActive)
			{
				enemySpawnControllers[m].ForceEnemyDead(true);
			}
			else
			{
				enemySpawnControllers[m].ResetIsDead();
			}
		}
		this.m_onRoomDataLoadedRelay.Dispatch();
	}

	// Token: 0x06002CE9 RID: 11497 RVA: 0x000986D4 File Offset: 0x000968D4
	public void OnPlayerExit_SaveStageData()
	{
		SaveFileSystem.SaveBatch saveBatch = SaveFileSystem.BeginSaveBatch(SaveManager.CurrentProfile);
		bool flag = SceneLoader_RL.CurrentScene == SceneLoadingUtility.GetSceneName(SceneID.World);
		if (this.CanSaveRoom)
		{
			RoomSaveData roomSaveData = SaveManager.StageSaveData.GetRoomSaveData(this.Room.BiomeType, this.Room.BiomeControllerIndex);
			this.SaveRoomState(roomSaveData, false);
			if (flag)
			{
				SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.Stage, SavingType.FileOnly, false, null);
			}
		}
		if (flag)
		{
			SaveManager.PlayerSaveData.SecondsPlayed += (uint)GameTimer.TotalSessionAccumulatedTime;
			GameTimer.ClearSessionAccumulatedTime();
			SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.Player, SavingType.FileOnly, false, null);
			SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.GameMode, SavingType.FileOnly, false, null);
		}
		saveBatch.End();
	}

	// Token: 0x04002418 RID: 9240
	private Relay<bool> m_onRoomDataSavedRelay = new Relay<bool>();

	// Token: 0x04002419 RID: 9241
	private Relay m_onRoomDataLoadedRelay = new Relay();

	// Token: 0x0400241B RID: 9243
	private List<PropSpawnController> m_breakablePropsList;
}
