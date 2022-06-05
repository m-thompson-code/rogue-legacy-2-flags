using System;
using System.Collections.Generic;
using GameEventTracking;

// Token: 0x020002DA RID: 730
[Serializable]
public class StageSaveData : IVersionUpdateable
{
	// Token: 0x17000CCE RID: 3278
	// (get) Token: 0x06001D10 RID: 7440 RVA: 0x0005F9D4 File Offset: 0x0005DBD4
	// (set) Token: 0x06001D11 RID: 7441 RVA: 0x0005F9DC File Offset: 0x0005DBDC
	public Dictionary<BiomeType, List<RoomSaveData>> RoomSaveDataDict
	{
		get
		{
			return this.m_roomSaveDataDict;
		}
		private set
		{
			this.m_roomSaveDataDict = value;
		}
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x0005F9E8 File Offset: 0x0005DBE8
	public void LinkTrackerData()
	{
		this.RoomTrackerDataList = GameEventTrackerManager.RoomEventTracker.RoomsEntered;
		this.EnemyTrackerDataList = GameEventTrackerManager.EnemyEventTracker.EnemiesKilled;
		this.ChestTrackerDataList = GameEventTrackerManager.ItemEventTracker.ChestsOpened;
		this.ItemTrackerDataList = GameEventTrackerManager.ItemEventTracker.ItemsCollected;
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x0005FA38 File Offset: 0x0005DC38
	public RoomSaveData GetRoomSaveData(BiomeType biomeType, int biomeControllerIndex)
	{
		Dictionary<int, RoomSaveData> roomDataLookupTable = this.GetRoomDataLookupTable(biomeType);
		if (roomDataLookupTable != null && roomDataLookupTable.ContainsKey(biomeControllerIndex))
		{
			return roomDataLookupTable[biomeControllerIndex];
		}
		return null;
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x0005FA64 File Offset: 0x0005DC64
	public Dictionary<int, RoomSaveData> GetRoomDataLookupTable(BiomeType biomeType)
	{
		biomeType = BiomeType_RL.GetGroupedBiomeType(biomeType);
		Dictionary<int, RoomSaveData> result;
		if (this.m_roomSaveDataLookupTable.TryGetValue(biomeType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x0005FA8C File Offset: 0x0005DC8C
	public List<RoomSaveData> GetRoomDataList(BiomeType biomeType)
	{
		biomeType = BiomeType_RL.GetGroupedBiomeType(biomeType);
		List<RoomSaveData> result;
		if (this.RoomSaveDataDict.TryGetValue(biomeType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x0005FAB4 File Offset: 0x0005DCB4
	public void CreateLevelEditorRoomSaveDataList(BaseRoom levelEditorRoom)
	{
		this.RoomSaveDataDict.Clear();
		BiomeType biomeType = levelEditorRoom.BiomeType;
		biomeType = BiomeType_RL.GetGroupedBiomeType(biomeType);
		List<RoomSaveData> list = new List<RoomSaveData>();
		foreach (BaseRoom baseRoom in OnPlayManager.RoomList)
		{
			RoomSaveData roomSaveData = StageSaveData.CreateRoomSaveData(biomeType, (baseRoom as Room).GridPointManager);
			roomSaveData.BiomeType = biomeType;
			list.Add(roomSaveData);
		}
		this.RoomSaveDataDict.Add(biomeType, list);
		this.CreateAllBiomeLookupTable();
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x0005FB54 File Offset: 0x0005DD54
	public void CreateEmptyRoomSaveDataList()
	{
		this.RoomSaveDataDict.Clear();
		foreach (KeyValuePair<BiomeType, BiomeController> keyValuePair in WorldBuilder.BiomeControllers)
		{
			BiomeController value = keyValuePair.Value;
			BiomeType groupedBiomeType = BiomeType_RL.GetGroupedBiomeType(value.Biome);
			List<GridPointManager> gridPointManagers = value.GridPointManager.GridPointManagers;
			gridPointManagers.AddRange(value.GridPointManager.TunnelDestinationGridPointManagers);
			List<RoomSaveData> list = new List<RoomSaveData>();
			for (int i = 0; i < gridPointManagers.Count; i++)
			{
				GridPointManager gridPointRoom = gridPointManagers[i];
				RoomSaveData item = StageSaveData.CreateRoomSaveData(groupedBiomeType, gridPointRoom);
				list.Add(item);
			}
			this.RoomSaveDataDict.Add(groupedBiomeType, list);
		}
		this.CreateAllBiomeLookupTable();
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x0005FC34 File Offset: 0x0005DE34
	private static RoomSaveData CreateRoomSaveData(BiomeType biomeType, GridPointManager gridPointRoom)
	{
		RoomSaveData roomSaveData = new RoomSaveData();
		roomSaveData.RoomName = gridPointRoom.RoomMetaData.name;
		roomSaveData.RoomNumber = gridPointRoom.RoomNumber;
		roomSaveData.BiomeType = biomeType;
		roomSaveData.RoomType = gridPointRoom.RoomType;
		roomSaveData.IsMirrored = gridPointRoom.IsRoomMirrored;
		roomSaveData.IsTunnelDestination = gridPointRoom.IsTunnelDestination;
		roomSaveData.BiomeControllerIndex = gridPointRoom.BiomeControllerIndex;
		roomSaveData.RoomSeed = gridPointRoom.RoomSeed;
		roomSaveData.RoomID = gridPointRoom.RoomMetaData.ID;
		roomSaveData.GridCoordinatesX = gridPointRoom.GridCoordinates.x;
		roomSaveData.GridCoordinatesY = gridPointRoom.GridCoordinates.y;
		roomSaveData.IsMerged = (gridPointRoom.MergeWithGridPointManagers.Count > 0);
		roomSaveData.MergedWithRoomNumbers = new int[gridPointRoom.MergeWithGridPointManagers.Count];
		for (int i = 0; i < gridPointRoom.MergeWithGridPointManagers.Count; i++)
		{
			roomSaveData.MergedWithRoomNumbers[i] = gridPointRoom.MergeWithGridPointManagers[i].RoomNumber;
		}
		return roomSaveData;
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x0005FD3A File Offset: 0x0005DF3A
	public void UpdateVersion()
	{
		if (this.REVISION_NUMBER != 14)
		{
			this.ForceResetWorld = true;
		}
		this.REVISION_NUMBER = 14;
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x0005FD58 File Offset: 0x0005DF58
	public void VerifyBiomeSaveData(BiomeType biome)
	{
		BiomeController biomeController = WorldBuilder.GetBiomeController(biome);
		if (biomeController != null)
		{
			int count = biomeController.GridPointManager.GridPointManagers.Count;
			int count2 = biomeController.GridPointManager.TunnelDestinationGridPointManagers.Count;
			int num = count + count2;
			if (this.m_roomSaveDataDict.ContainsKey(biome))
			{
				int count3 = this.m_roomSaveDataDict[biome].Count;
				if (num != count3)
				{
					for (int i = 0; i < biomeController.GridPointManager.TunnelDestinationGridPointManagers.Count; i++)
					{
						GridPointManager gridPointRoom = biomeController.GridPointManager.TunnelDestinationGridPointManagers[i];
						RoomSaveData item = StageSaveData.CreateRoomSaveData(biome, gridPointRoom);
						this.m_roomSaveDataDict[biome].Add(item);
					}
					this.CreateRoomSaveDataLookupTable(biome);
				}
			}
		}
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x0005FE18 File Offset: 0x0005E018
	public void CreateAllBiomeLookupTable()
	{
		foreach (KeyValuePair<BiomeType, List<RoomSaveData>> keyValuePair in this.RoomSaveDataDict)
		{
			this.CreateRoomSaveDataLookupTable(keyValuePair.Key);
		}
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x0005FE74 File Offset: 0x0005E074
	public void CreateRoomSaveDataLookupTable(BiomeType biome)
	{
		if (this.m_roomSaveDataLookupTable == null)
		{
			this.m_roomSaveDataLookupTable = new Dictionary<BiomeType, Dictionary<int, RoomSaveData>>();
		}
		List<RoomSaveData> list;
		if (this.RoomSaveDataDict.TryGetValue(biome, out list))
		{
			Dictionary<int, RoomSaveData> dictionary = null;
			if (this.m_roomSaveDataLookupTable.TryGetValue(biome, out dictionary))
			{
				dictionary.Clear();
			}
			else
			{
				dictionary = new Dictionary<int, RoomSaveData>();
				this.m_roomSaveDataLookupTable.Add(biome, dictionary);
			}
			foreach (RoomSaveData roomSaveData in list)
			{
				if (!dictionary.ContainsKey(roomSaveData.BiomeControllerIndex))
				{
					dictionary.Add(roomSaveData.BiomeControllerIndex, roomSaveData);
				}
			}
		}
	}

	// Token: 0x04001B05 RID: 6917
	public int REVISION_NUMBER = 14;

	// Token: 0x04001B06 RID: 6918
	public int FILE_NUMBER;

	// Token: 0x04001B07 RID: 6919
	public int BiomeCreationSeed = -1;

	// Token: 0x04001B08 RID: 6920
	public int MergeRoomSeed = -1;

	// Token: 0x04001B09 RID: 6921
	public int EnemySeed = -1;

	// Token: 0x04001B0A RID: 6922
	public int PropSeed = -1;

	// Token: 0x04001B0B RID: 6923
	private Dictionary<BiomeType, List<RoomSaveData>> m_roomSaveDataDict = new Dictionary<BiomeType, List<RoomSaveData>>();

	// Token: 0x04001B0C RID: 6924
	[NonSerialized]
	private Dictionary<BiomeType, Dictionary<int, RoomSaveData>> m_roomSaveDataLookupTable;

	// Token: 0x04001B0D RID: 6925
	public int TimesTrackerWasLoaded;

	// Token: 0x04001B0E RID: 6926
	public List<EnemyTrackerData> EnemyTrackerDataList;

	// Token: 0x04001B0F RID: 6927
	public List<ChestTrackerData> ChestTrackerDataList;

	// Token: 0x04001B10 RID: 6928
	public List<ItemTrackerData> ItemTrackerDataList;

	// Token: 0x04001B11 RID: 6929
	public List<RoomTrackerData> RoomTrackerDataList;

	// Token: 0x04001B12 RID: 6930
	public bool ForceResetWorld;
}
