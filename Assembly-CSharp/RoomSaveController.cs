using System;
using System.Collections.Generic;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020007CE RID: 1998
public class RoomSaveController
{
	// Token: 0x17001692 RID: 5778
	// (get) Token: 0x06003D89 RID: 15753 RVA: 0x00022147 File Offset: 0x00020347
	public IRelayLink<bool> OnRoomDataSavedRelay
	{
		get
		{
			return this.m_onRoomDataSavedRelay.link;
		}
	}

	// Token: 0x17001693 RID: 5779
	// (get) Token: 0x06003D8A RID: 15754 RVA: 0x00022154 File Offset: 0x00020354
	public IRelayLink OnRoomDataLoadedRelay
	{
		get
		{
			return this.m_onRoomDataLoadedRelay.link;
		}
	}

	// Token: 0x17001694 RID: 5780
	// (get) Token: 0x06003D8B RID: 15755 RVA: 0x00022161 File Offset: 0x00020361
	// (set) Token: 0x06003D8C RID: 15756 RVA: 0x00022168 File Offset: 0x00020368
	public static bool DisableCutsceneSaving { get; set; }

	// Token: 0x17001695 RID: 5781
	// (get) Token: 0x06003D8D RID: 15757 RVA: 0x000F8E40 File Offset: 0x000F7040
	public bool CanSaveRoom
	{
		get
		{
			return !RoomSaveController.DisableCutsceneSaving && this.Room.RoomType != RoomType.Boss && this.Room.SpecialRoomType != SpecialRoomType.Boss && this.Room.SpecialRoomType != SpecialRoomType.Heirloom && this.Room.SpecialRoomType != SpecialRoomType.Clown && !this.Room.DisableRoomSaving;
		}
	}

	// Token: 0x17001696 RID: 5782
	// (get) Token: 0x06003D8E RID: 15758 RVA: 0x00022170 File Offset: 0x00020370
	public BaseRoom Room { get; }

	// Token: 0x06003D8F RID: 15759 RVA: 0x00022178 File Offset: 0x00020378
	public RoomSaveController(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x06003D90 RID: 15760 RVA: 0x000F8EA0 File Offset: 0x000F70A0
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

	// Token: 0x06003D91 RID: 15761 RVA: 0x000F9064 File Offset: 0x000F7264
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

	// Token: 0x06003D92 RID: 15762 RVA: 0x000F90D4 File Offset: 0x000F72D4
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

	// Token: 0x06003D93 RID: 15763 RVA: 0x000F9248 File Offset: 0x000F7448
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

	// Token: 0x06003D94 RID: 15764 RVA: 0x000F94E0 File Offset: 0x000F76E0
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

	// Token: 0x06003D95 RID: 15765 RVA: 0x000F968C File Offset: 0x000F788C
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

	// Token: 0x0400307B RID: 12411
	private Relay<bool> m_onRoomDataSavedRelay = new Relay<bool>();

	// Token: 0x0400307C RID: 12412
	private Relay m_onRoomDataLoadedRelay = new Relay();

	// Token: 0x0400307E RID: 12414
	private List<PropSpawnController> m_breakablePropsList;
}
