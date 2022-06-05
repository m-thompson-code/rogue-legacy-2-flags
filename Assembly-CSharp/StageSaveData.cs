using System;
using System.Collections.Generic;
using GameEventTracking;

// Token: 0x020004CF RID: 1231
[Serializable]
public class StageSaveData : IVersionUpdateable
{
	// Token: 0x17001057 RID: 4183
	// (get) Token: 0x060027DA RID: 10202 RVA: 0x00016676 File Offset: 0x00014876
	// (set) Token: 0x060027DB RID: 10203 RVA: 0x0001667E File Offset: 0x0001487E
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

	// Token: 0x060027DC RID: 10204 RVA: 0x000BBCBC File Offset: 0x000B9EBC
	public void LinkTrackerData()
	{
		this.RoomTrackerDataList = GameEventTrackerManager.RoomEventTracker.RoomsEntered;
		this.EnemyTrackerDataList = GameEventTrackerManager.EnemyEventTracker.EnemiesKilled;
		this.ChestTrackerDataList = GameEventTrackerManager.ItemEventTracker.ChestsOpened;
		this.ItemTrackerDataList = GameEventTrackerManager.ItemEventTracker.ItemsCollected;
	}

	// Token: 0x060027DD RID: 10205 RVA: 0x000BBD0C File Offset: 0x000B9F0C
	public RoomSaveData GetRoomSaveData(BiomeType biomeType, int biomeControllerIndex)
	{
		Dictionary<int, RoomSaveData> roomDataLookupTable = this.GetRoomDataLookupTable(biomeType);
		if (roomDataLookupTable != null && roomDataLookupTable.ContainsKey(biomeControllerIndex))
		{
			return roomDataLookupTable[biomeControllerIndex];
		}
		return null;
	}

	// Token: 0x060027DE RID: 10206 RVA: 0x000BBD38 File Offset: 0x000B9F38
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

	// Token: 0x060027DF RID: 10207 RVA: 0x000BBD60 File Offset: 0x000B9F60
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

	// Token: 0x060027E0 RID: 10208 RVA: 0x000BBD88 File Offset: 0x000B9F88
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

	// Token: 0x060027E1 RID: 10209 RVA: 0x000BBE28 File Offset: 0x000BA028
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

	// Token: 0x060027E2 RID: 10210 RVA: 0x000BBF08 File Offset: 0x000BA108
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

	// Token: 0x060027E3 RID: 10211 RVA: 0x00016687 File Offset: 0x00014887
	public void UpdateVersion()
	{
		if (this.REVISION_NUMBER != 14)
		{
			this.ForceResetWorld = true;
		}
		this.REVISION_NUMBER = 14;
	}

	// Token: 0x060027E4 RID: 10212 RVA: 0x000BC010 File Offset: 0x000BA210
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

	// Token: 0x060027E5 RID: 10213 RVA: 0x000BC0D0 File Offset: 0x000BA2D0
	public void CreateAllBiomeLookupTable()
	{
		foreach (KeyValuePair<BiomeType, List<RoomSaveData>> keyValuePair in this.RoomSaveDataDict)
		{
			this.CreateRoomSaveDataLookupTable(keyValuePair.Key);
		}
	}

	// Token: 0x060027E6 RID: 10214 RVA: 0x000BC12C File Offset: 0x000BA32C
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

	// Token: 0x0400230C RID: 8972
	public int REVISION_NUMBER = 14;

	// Token: 0x0400230D RID: 8973
	public int FILE_NUMBER;

	// Token: 0x0400230E RID: 8974
	public int BiomeCreationSeed = -1;

	// Token: 0x0400230F RID: 8975
	public int MergeRoomSeed = -1;

	// Token: 0x04002310 RID: 8976
	public int EnemySeed = -1;

	// Token: 0x04002311 RID: 8977
	public int PropSeed = -1;

	// Token: 0x04002312 RID: 8978
	private Dictionary<BiomeType, List<RoomSaveData>> m_roomSaveDataDict = new Dictionary<BiomeType, List<RoomSaveData>>();

	// Token: 0x04002313 RID: 8979
	[NonSerialized]
	private Dictionary<BiomeType, Dictionary<int, RoomSaveData>> m_roomSaveDataLookupTable;

	// Token: 0x04002314 RID: 8980
	public int TimesTrackerWasLoaded;

	// Token: 0x04002315 RID: 8981
	public List<EnemyTrackerData> EnemyTrackerDataList;

	// Token: 0x04002316 RID: 8982
	public List<ChestTrackerData> ChestTrackerDataList;

	// Token: 0x04002317 RID: 8983
	public List<ItemTrackerData> ItemTrackerDataList;

	// Token: 0x04002318 RID: 8984
	public List<RoomTrackerData> RoomTrackerDataList;

	// Token: 0x04002319 RID: 8985
	public bool ForceResetWorld;
}
