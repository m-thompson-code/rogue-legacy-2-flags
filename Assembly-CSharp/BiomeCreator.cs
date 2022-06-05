using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RLWorldCreation;
using Rooms;
using UnityEngine;

// Token: 0x02000A7F RID: 2687
public class BiomeCreator : MonoBehaviour
{
	// Token: 0x17001C03 RID: 7171
	// (get) Token: 0x0600513D RID: 20797 RVA: 0x0002C5B2 File Offset: 0x0002A7B2
	// (set) Token: 0x0600513E RID: 20798 RVA: 0x0002C5BA File Offset: 0x0002A7BA
	public Queue<GridPointManager> BuildQueue
	{
		get
		{
			return this.m_buildQueue;
		}
		private set
		{
			this.m_buildQueue = value;
		}
	}

	// Token: 0x17001C04 RID: 7172
	// (get) Token: 0x0600513F RID: 20799 RVA: 0x0002C5C3 File Offset: 0x0002A7C3
	// (set) Token: 0x06005140 RID: 20800 RVA: 0x0002C5CB File Offset: 0x0002A7CB
	public GridController GridController
	{
		get
		{
			return this.m_gridController;
		}
		private set
		{
			this.m_gridController = value;
		}
	}

	// Token: 0x17001C05 RID: 7173
	// (get) Token: 0x06005141 RID: 20801 RVA: 0x0002C5D4 File Offset: 0x0002A7D4
	// (set) Token: 0x06005142 RID: 20802 RVA: 0x0002C5DC File Offset: 0x0002A7DC
	public bool IsComplete { get; private set; }

	// Token: 0x17001C06 RID: 7174
	// (get) Token: 0x06005143 RID: 20803 RVA: 0x0002C5E5 File Offset: 0x0002A7E5
	// (set) Token: 0x06005144 RID: 20804 RVA: 0x0002C5ED File Offset: 0x0002A7ED
	public bool IsPaused { get; set; }

	// Token: 0x17001C07 RID: 7175
	// (get) Token: 0x06005145 RID: 20805 RVA: 0x0002C5F6 File Offset: 0x0002A7F6
	// (set) Token: 0x06005146 RID: 20806 RVA: 0x0002C5FE File Offset: 0x0002A7FE
	public int RoomNumber
	{
		get
		{
			return this.m_roomNumber;
		}
		private set
		{
			this.m_roomNumber = value;
		}
	}

	// Token: 0x06005147 RID: 20807 RVA: 0x0002C607 File Offset: 0x0002A807
	public void SetState(BiomeController biomeController, BiomeBuildStateID stateID, string failureDescription = "")
	{
		if (this.State != stateID)
		{
			if (stateID == BiomeBuildStateID.Failed)
			{
				this.BiomeBuildFailureDescription = failureDescription;
			}
			if (stateID != BiomeBuildStateID.Running)
			{
				base.StopAllCoroutines();
			}
			this.State = stateID;
		}
	}

	// Token: 0x17001C08 RID: 7176
	// (get) Token: 0x06005148 RID: 20808 RVA: 0x0002C62E File Offset: 0x0002A82E
	// (set) Token: 0x06005149 RID: 20809 RVA: 0x0002C636 File Offset: 0x0002A836
	public int SpecialRoomPlacementInterval
	{
		get
		{
			return this.m_specialRoomPlacementInterval;
		}
		set
		{
			this.m_specialRoomPlacementInterval = value;
		}
	}

	// Token: 0x17001C09 RID: 7177
	// (get) Token: 0x0600514A RID: 20810 RVA: 0x0002C63F File Offset: 0x0002A83F
	// (set) Token: 0x0600514B RID: 20811 RVA: 0x0002C647 File Offset: 0x0002A847
	public List<RoomTypeEntry> SpecialRoomPool
	{
		get
		{
			return this.m_specialRoomPool;
		}
		set
		{
			this.m_specialRoomPool = value;
		}
	}

	// Token: 0x17001C0A RID: 7178
	// (get) Token: 0x0600514C RID: 20812 RVA: 0x0002C650 File Offset: 0x0002A850
	// (set) Token: 0x0600514D RID: 20813 RVA: 0x0002C658 File Offset: 0x0002A858
	public BiomeBuildStateID State
	{
		get
		{
			return this.m_state;
		}
		private set
		{
			this.m_state = value;
		}
	}

	// Token: 0x17001C0B RID: 7179
	// (get) Token: 0x0600514E RID: 20814 RVA: 0x0002C661 File Offset: 0x0002A861
	// (set) Token: 0x0600514F RID: 20815 RVA: 0x0002C669 File Offset: 0x0002A869
	public string BiomeBuildFailureDescription { get; private set; }

	// Token: 0x06005150 RID: 20816 RVA: 0x0002C672 File Offset: 0x0002A872
	private void BuildComplete(BiomeController biomeController)
	{
		this.SetState(biomeController, BiomeBuildStateID.Complete, "");
	}

	// Token: 0x06005151 RID: 20817 RVA: 0x00133A4C File Offset: 0x00131C4C
	public GridPointManager BuildRoom(BiomeController biomeController, RoomType roomType, RoomSetEntry roomSetEntry, Vector2Int buildCoordinates, DoorLocation doorLocation, BiomeType overrideBiome = BiomeType.None, bool incrementRoomCount = true)
	{
		BiomeType biome = biomeController.Biome;
		if (overrideBiome != BiomeType.None)
		{
			biome = overrideBiome;
		}
		else if (roomSetEntry.RoomMetaData.BiomeOverride != BiomeType.None)
		{
			biome = roomSetEntry.RoomMetaData.BiomeOverride;
		}
		this.m_checkedDoors.Clear();
		int roomNumber = this.RoomNumber;
		this.RoomNumber = roomNumber + 1;
		GridPointManager gridPointManager = biomeController.GridPointManager.AddRoomToGrid(this.RoomNumber, roomSetEntry.RoomMetaData, roomSetEntry.IsMirrored, roomType, GridController.GetRoomGridCoordinates(buildCoordinates, roomSetEntry.RoomMetaData.Size, doorLocation), biome, incrementRoomCount);
		this.BuildQueue.Enqueue(gridPointManager);
		return gridPointManager;
	}

	// Token: 0x06005152 RID: 20818 RVA: 0x0002C681 File Offset: 0x0002A881
	public IEnumerator CreateBiome(BiomeController biomeController)
	{
		if (RoomLibrary.IsLoaded)
		{
			this.Initialise(biomeController);
			yield return this.SetConnectionRoom(biomeController);
			if (this.State != BiomeBuildStateID.Failed)
			{
				this.CreateTransitionRoom(biomeController);
			}
			if (this.State != BiomeBuildStateID.Failed)
			{
				this.m_roomRequirementsController = new RoomRequirementsController(biomeController);
			}
			if (this.State != BiomeBuildStateID.Failed)
			{
				yield return this.CreateRooms(biomeController);
			}
			if (this.State != BiomeBuildStateID.Failed)
			{
				this.BuildComplete(biomeController);
			}
		}
		else
		{
			string text = string.Format("<color=red>| {0} | Cannot create {1} Biome because Room Library has not been loaded.</color>", this, biomeController.Biome);
			Debug.LogFormat(text, Array.Empty<object>());
			this.SetState(biomeController, BiomeBuildStateID.Failed, text);
		}
		yield break;
	}

	// Token: 0x06005153 RID: 20819 RVA: 0x0002C697 File Offset: 0x0002A897
	private IEnumerator CreateRooms(BiomeController biomeController)
	{
		this.m_createRoomsBuildRule = null;
		if (biomeController.Biome == BiomeType.Tower)
		{
			this.m_createRoomsBuildRule = new CreateRoomsBuildRule_Tower();
		}
		else if (biomeController.Biome == BiomeType.Cave)
		{
			this.m_createRoomsBuildRule = new CreateRoomsBuildRule_Cave();
		}
		else if (biomeController.Biome == BiomeType.Stone)
		{
			this.m_createRoomsBuildRule = new CreateRoomsBuildRule_Stone();
		}
		else if (biomeController.Biome == BiomeType.Forest)
		{
			this.m_createRoomsBuildRule = new CreateRoomsBuildRule_Forest();
		}
		else if (biomeController.Biome == BiomeType.Study)
		{
			this.m_createRoomsBuildRule = new CreateRoomsBuildRule_Study();
		}
		else
		{
			this.m_createRoomsBuildRule = new CreateRoomsBuildRule();
		}
		yield return this.m_createRoomsBuildRule.CreateRooms(this, biomeController);
		yield break;
	}

	// Token: 0x06005154 RID: 20820 RVA: 0x0002C6AD File Offset: 0x0002A8AD
	private IEnumerator SetConnectionRoom(BiomeController biomeController)
	{
		CheckConnectionRoomsBuildRule checkConnectionRoomsBuildRule = new CheckConnectionRoomsBuildRule_Default();
		yield return checkConnectionRoomsBuildRule.UpdateConnectionPoint(this, biomeController);
		yield break;
	}

	// Token: 0x06005155 RID: 20821 RVA: 0x00133AE8 File Offset: 0x00131CE8
	private void CreateTransitionRoom(BiomeController biomeController)
	{
		BiomeType connectsToBiome = BiomeUtility.GetConnectsToBiome(biomeController.Biome);
		if (connectsToBiome != BiomeType.None && WorldBuilder.GetBiomeController(connectsToBiome) != null)
		{
			RoomSide connectsToSide = BiomeUtility.GetConnectDirection(biomeController.Biome);
			GridPoint connectionPoint = WorldBuilder.GetBiomeController(connectsToBiome).GridPointManager.GetConnectionPoint(connectsToSide);
			DoorLocation location2 = DoorLocation.Empty;
			try
			{
				location2 = connectionPoint.Doors.Single((DoorLocation location) => location.RoomSide == connectsToSide);
				Vector2Int doorLeadsToGridCoordinates = connectionPoint.Owner.GetDoorLeadsToGridCoordinates(location2);
				RoomMetaData transitionRoom = RoomLibrary.GetSetCollection(biomeController.Biome).TransitionRoom;
				if (transitionRoom == null)
				{
					throw new InvalidProgramException(string.Format("| {0} | No Transition Room has been set for the {1} Biome in the Room Pool.", this, biomeController.Biome));
				}
				List<DoorLocation> validDoorLocations = WorldBuilder.GetBiomeController(connectsToBiome).GridPointManager.GetValidDoorLocations(connectsToSide);
				for (int i = validDoorLocations.Count - 1; i >= 0; i--)
				{
					if (!transitionRoom.GetHasDoor(validDoorLocations[i], false))
					{
						validDoorLocations.RemoveAt(i);
					}
				}
				int randomNumber = RNGManager.GetRandomNumber(RngID.BiomeCreation, "Get Random Transition Room Door Index", 0, validDoorLocations.Count);
				this.BuildRoom(biomeController, RoomType.Transition, new RoomSetEntry(transitionRoom, false), doorLeadsToGridCoordinates, validDoorLocations[randomNumber], BiomeType.None, true);
				return;
			}
			catch (Exception ex)
			{
				string text = ex.ToString();
				if (ex is InvalidOperationException)
				{
					text = string.Format("<color=red>| {0} | No available Door Location on {1} side of {2} Biome's {1} Connection Point, but there should be.</color>", this, connectsToSide, connectsToBiome);
					Debug.LogFormat(text, Array.Empty<object>());
				}
				else
				{
					text = string.Format("<color=red>| {0} | An Exception occurred while attempting to place <b>{1} Biome's</b> Transition Room.</color>", this, biomeController.Biome);
					Debug.LogFormat(text, Array.Empty<object>());
				}
				this.SetState(biomeController, BiomeBuildStateID.Failed, text);
				return;
			}
		}
		RoomMetaData transitionRoom2 = RoomLibrary.GetSetCollection(biomeController.Biome).TransitionRoom;
		Vector2Int zero = Vector2Int.zero;
		if (biomeController.Biome == BiomeType.Garden)
		{
			zero = new Vector2Int(-30, 0);
		}
		this.BuildRoom(biomeController, RoomType.Transition, new RoomSetEntry(transitionRoom2, false), zero, DoorLocation.Empty, BiomeType.None, true);
	}

	// Token: 0x06005156 RID: 20822 RVA: 0x00133CF4 File Offset: 0x00131EF4
	public void CreateBiomeFromSaveData(BiomeController biomeController, List<RoomSaveData> roomSaveDataCollection)
	{
		this.Initialise(biomeController);
		Dictionary<int, GridPointManager> dictionary = new Dictionary<int, GridPointManager>();
		for (int i = 0; i < roomSaveDataCollection.Count; i++)
		{
			RoomSaveData roomSaveData = roomSaveDataCollection[i];
			if (!roomSaveData.IsTunnelDestination)
			{
				Vector2Int roomCoordinates = new Vector2Int(roomSaveData.GridCoordinatesX, roomSaveData.GridCoordinatesY);
				RoomMetaData roomMetaDataFromID = RoomLibrary.GetRoomMetaDataFromID(roomSaveData.RoomID);
				BiomeType biome = roomSaveData.BiomeType;
				if (roomSaveData.BiomeType == BiomeType.Tower)
				{
					if (roomMetaDataFromID == RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.BossEntranceLeft_Tower) || roomMetaDataFromID == RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.BossEntranceCentre_Tower) || roomMetaDataFromID == RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.BossEntranceRight_Tower))
					{
						biome = BiomeType.TowerExterior;
					}
					else
					{
						foreach (RoomSetEntry roomSetEntry in RoomLibrary.GetSetCollection(BiomeType.TowerExterior).RoomSet)
						{
							if (roomSetEntry.RoomMetaData == roomMetaDataFromID)
							{
								biome = BiomeType.TowerExterior;
								break;
							}
						}
					}
				}
				GridPointManager gridPointManager = biomeController.GridPointManager.AddRoomToGrid(roomSaveData.RoomNumber, roomMetaDataFromID, roomSaveData.IsMirrored, roomSaveData.RoomType, roomCoordinates, biome, true);
				gridPointManager.SetBiomeControllerIndex(roomSaveData.BiomeControllerIndex);
				gridPointManager.SetRoomSeed(roomSaveData.RoomSeed);
				if (!roomSaveData.IsTunnelDestination)
				{
					if (!dictionary.ContainsKey(roomSaveData.RoomNumber))
					{
						dictionary.Add(roomSaveData.RoomNumber, gridPointManager);
					}
					else
					{
						Debug.LogFormat("Key {0} already exists", new object[]
						{
							roomSaveData.RoomNumber
						});
					}
				}
			}
		}
		int num = 0;
		int num2 = 0;
		List<int> list = new List<int>();
		for (int j = 0; j < roomSaveDataCollection.Count; j++)
		{
			if (!list.Contains(roomSaveDataCollection[j].BiomeControllerIndex))
			{
				if (roomSaveDataCollection[j].IsMerged)
				{
					num2++;
				}
				else if (!roomSaveDataCollection[j].IsTunnelDestination)
				{
					num++;
				}
				list.Add(roomSaveDataCollection[j].BiomeControllerIndex);
			}
		}
		biomeController.GridPointManager.SetStandaloneRoomCount(num);
		biomeController.GridPointManager.SetMergeRoomCount(num2);
		for (int k = 0; k < roomSaveDataCollection.Count; k++)
		{
			RoomSaveData roomSaveData2 = roomSaveDataCollection[k];
			if (roomSaveData2.IsMerged)
			{
				GridPointManager gridPointManager2 = dictionary[roomSaveData2.RoomNumber];
				int biomeControllerIndex = roomSaveData2.BiomeControllerIndex;
				List<GridPointManager> list2 = new List<GridPointManager>();
				for (int l = 0; l < roomSaveData2.MergedWithRoomNumbers.Length; l++)
				{
					int key = roomSaveData2.MergedWithRoomNumbers[l];
					GridPointManager gridPointManager3 = dictionary[key];
					list2.Add(gridPointManager3);
					gridPointManager3.SetBiomeControllerIndex(biomeControllerIndex);
				}
				gridPointManager2.SetMergeWithRooms(list2);
			}
		}
		this.SetState(biomeController, BiomeBuildStateID.Complete, "");
	}

	// Token: 0x06005157 RID: 20823 RVA: 0x00133FD8 File Offset: 0x001321D8
	private bool GetBuildRoomAtDoorLocation(BiomeController biomeController, GridPointManager gridPointManager, DoorLocation doorLocation)
	{
		int num = -1;
		switch (doorLocation.RoomSide)
		{
		case RoomSide.Top:
			num = biomeController.BiomeData.AttachTopOdds;
			break;
		case RoomSide.Bottom:
			num = biomeController.BiomeData.AttachBottomOdds;
			break;
		case RoomSide.Left:
			num = biomeController.BiomeData.AttachLeftOdds;
			break;
		case RoomSide.Right:
			num = biomeController.BiomeData.AttachRightOdds;
			break;
		}
		return num == 100 || (num != 0 && RNGManager.GetRandomNumber(RngID.BiomeCreation, string.Format("Should Room be built at Door Location ({0},{1}) in Room #{2} in {3} Biome?", new object[]
		{
			doorLocation.RoomSide,
			doorLocation.DoorNumber,
			gridPointManager.RoomNumber,
			gridPointManager.Biome
		}), 0, 100) < num);
	}

	// Token: 0x06005158 RID: 20824 RVA: 0x001340A0 File Offset: 0x001322A0
	public List<DoorLocation> GetBuildRoomAtDoorLocations(BiomeController biomeController, GridPointManager gridPointManager)
	{
		List<DoorLocation> list = new List<DoorLocation>();
		if (gridPointManager.RoomType == RoomType.Transition)
		{
			List<RoomSide> sides = new List<RoomSide>
			{
				RoomSide.Left,
				RoomSide.Right,
				RoomSide.Bottom,
				RoomSide.Top
			};
			if (BiomeUtility.GetConnectsToBiome(biomeController.Biome) != BiomeType.None)
			{
				sides.Remove(RoomUtility.GetOppositeSide(BiomeUtility.GetConnectDirection(gridPointManager.Biome)));
			}
			IEnumerable<DoorLocation> enumerable = (from location in BiomeCreator.GetDoorsInRoom(gridPointManager)
			where sides.Contains(location.RoomSide) && this.GetIsDoorLocationValid(biomeController, gridPointManager, location)
			select location).ToList<DoorLocation>();
			list = new List<DoorLocation>(enumerable);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (!this.GetBuildRoomAtDoorLocation(biomeController, gridPointManager, list[i]))
				{
					list.Remove(list[i]);
				}
			}
			if (list.Count < BiomeCreator.FORCE_BUILD_ROOM_IF_TRANSITION_ROOM_DOOR_COUNT_IS_LESS_THAN)
			{
				list = enumerable.ToList<DoorLocation>();
				RoomUtility.ShuffleList<DoorLocation>(list);
				list = list.GetRange(0, Mathf.Min(list.Count, BiomeCreator.FORCE_BUILD_ROOM_IF_TRANSITION_ROOM_DOOR_COUNT_IS_LESS_THAN));
			}
			else
			{
				RoomUtility.ShuffleList<DoorLocation>(list);
			}
		}
		else
		{
			list = (from location in BiomeCreator.GetDoorsInRoom(gridPointManager)
			where this.GetIsDoorLocationValid(biomeController, gridPointManager, location)
			select location).ToList<DoorLocation>();
			for (int j = list.Count - 1; j >= 0; j--)
			{
				if (!this.GetBuildRoomAtDoorLocation(biomeController, gridPointManager, list[j]))
				{
					list.Remove(list[j]);
				}
			}
			RoomUtility.ShuffleList<DoorLocation>(list);
		}
		return list;
	}

	// Token: 0x06005159 RID: 20825 RVA: 0x00134274 File Offset: 0x00132474
	public bool GetIsDoorLocationValid(BiomeController biomeController, GridPointManager gridPointManager, DoorLocation doorLocation)
	{
		Vector2Int doorLeadsToGridCoordinates = gridPointManager.GetDoorLeadsToGridCoordinates(doorLocation);
		return biomeController.GridPointManager.GetAreCoordinatesWithinBorder(doorLeadsToGridCoordinates, gridPointManager.RoomType == RoomType.Standard) && GridController.GetIsGridSpaceAvailable(doorLeadsToGridCoordinates);
	}

	// Token: 0x0600515A RID: 20826 RVA: 0x001342A8 File Offset: 0x001324A8
	public GridPointManager GetNextOriginRoom(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		if (biomeCreator.BuildQueue.Count > 0)
		{
			return biomeCreator.BuildQueue.Dequeue();
		}
		if (this.m_displayDeadEndWarnings)
		{
			Debug.LogFormat("<color=purple>|{0}| Dead End Hit at Room Number ({1}) in Biome ({2})</color>", new object[]
			{
				this,
				this.RoomNumber,
				biomeController.Biome
			});
		}
		int num = biomeController.GridPointManager.GridPointManagers.Count - 1;
		int num2 = num - (BiomeCreation_EV.GetNumberOfRoomsToLookBack(biomeController.Biome, biomeController.GridPointManager.GridPointManagers.Count, biomeController.TargetRoomCountsByRoomType.Sum((KeyValuePair<RoomType, int> entry) => entry.Value)) - 1);
		if (num2 == 0)
		{
			num2 = 1;
		}
		List<GridPointManager> list = new List<GridPointManager>();
		for (int i = num; i >= num2; i--)
		{
			GridPointManager gridPointManager = biomeController.GridPointManager.GridPointManagers[i];
			if (!this.m_checkedDoors.ContainsKey(gridPointManager))
			{
				this.m_checkedDoors.Add(gridPointManager, new List<DoorLocation>());
			}
			list.Add(gridPointManager);
			List<DoorLocation> doorsInRoom = BiomeCreator.GetDoorsInRoom(gridPointManager);
			RoomUtility.ShuffleList<DoorLocation>(doorsInRoom);
			foreach (DoorLocation doorLocation in doorsInRoom)
			{
				if (this.CheckDoorLocation_GetNextOriginRoom(biomeController, gridPointManager, doorLocation))
				{
					if (!this.m_checkedDoors[gridPointManager].Contains(doorLocation))
					{
						this.m_checkedDoors[gridPointManager].Add(doorLocation);
					}
					Vector2Int doorLeadsToGridCoordinates = gridPointManager.GetDoorLeadsToGridCoordinates(doorLocation);
					if (GridController.GetIsGridSpaceAvailable(doorLeadsToGridCoordinates))
					{
						bool flag = false;
						if (this.m_createRoomsBuildRule is CreateRoomsBuildRule_Tower)
						{
							if ((this.m_createRoomsBuildRule as CreateRoomsBuildRule_Tower).GetTier(doorLeadsToGridCoordinates) != TierID.None)
							{
								flag = true;
							}
						}
						else
						{
							flag = biomeController.GridPointManager.GetAreCoordinatesWithinBorder(doorLeadsToGridCoordinates, gridPointManager.RoomType == RoomType.Standard);
						}
						if (flag)
						{
							return gridPointManager;
						}
					}
				}
			}
		}
		return this.ReplaceOriginRoom(biomeCreator, biomeController);
	}

	// Token: 0x0600515B RID: 20827 RVA: 0x001344A8 File Offset: 0x001326A8
	private bool CheckDoorLocation_GetNextOriginRoom(BiomeController biomeController, GridPointManager originRoom, DoorLocation doorLocation)
	{
		bool result = true;
		if ((doorLocation.RoomSide == RoomSide.Top && biomeController.BiomeData.AttachTopOdds == 0) || (doorLocation.RoomSide == RoomSide.Bottom && biomeController.BiomeData.AttachBottomOdds == 0) || (doorLocation.RoomSide == RoomSide.Left && biomeController.BiomeData.AttachLeftOdds == 0) || (doorLocation.RoomSide == RoomSide.Right && biomeController.BiomeData.AttachRightOdds == 0))
		{
			result = false;
		}
		else
		{
			bool flag = true;
			for (int i = 0; i < biomeController.GridPointManager.GridPointManagers.Count; i++)
			{
				if (biomeController.GridPointManager.GridPointManagers[i].RoomType == RoomType.Standard)
				{
					flag = false;
					break;
				}
			}
			if (!flag && this.m_checkedDoors[originRoom].Contains(doorLocation))
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x0600515C RID: 20828 RVA: 0x0013456C File Offset: 0x0013276C
	private GridPointManager ReplaceOriginRoom(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		List<RoomSide> sidesToReplace = BiomeCreation_EV.GetSidesToReplace(biomeController.Biome);
		RoomSide oppositeSide = RoomUtility.GetOppositeSide(BiomeUtility.GetConnectDirection(biomeController.Biome));
		if (oppositeSide != RoomSide.None)
		{
			sidesToReplace.Remove(oppositeSide);
		}
		RoomUtility.ShuffleList<RoomSide>(sidesToReplace);
		foreach (RoomSide side in sidesToReplace)
		{
			IEnumerable<GridPoint> gridPointsWithSpaceOnSide = this.GetGridPointsWithSpaceOnSide(biomeController, side, biomeController.GridPointManager.GridPoints);
			IEnumerable<GridPoint> gridPointsOfRoomType = this.GetGridPointsOfRoomType(biomeController, RoomType.Transition);
			List<GridPoint> list = gridPointsWithSpaceOnSide.Except(gridPointsOfRoomType).ToList<GridPoint>();
			IEnumerable<GridPoint> gridPointsOfRoomType2 = this.GetGridPointsOfRoomType(biomeController, RoomType.Standard);
			list = gridPointsWithSpaceOnSide.Intersect(gridPointsOfRoomType2).ToList<GridPoint>();
			IEnumerable<GridPoint> gridPointsOfRoomType3 = this.GetGridPointsOfRoomType(biomeController, RoomType.Mandatory);
			list = list.Except(gridPointsOfRoomType3).ToList<GridPoint>();
			bool flag = false;
			while (!flag && list.Count<GridPoint>() > 0)
			{
				BiomeCreator.m_deadEndSeeds += string.Format("{0}:{1}, ", biomeController.Biome, RNGSeedManager.GetCurrentSeed(base.gameObject.scene.name));
				int randomNumber = RNGManager.GetRandomNumber(RngID.BiomeCreation, "Get Random Room To Replace", 0, list.Count<GridPoint>());
				GridPoint gridPoint = list.ElementAt(randomNumber);
				if (ReplaceRoomUtility.ReplaceRoomMetaData(biomeController, gridPoint, side))
				{
					return gridPoint.Owner;
				}
				list.RemoveAt(randomNumber);
			}
		}
		string format = "<color=red>|{0}| {1} Biome creation failed. There are Rooms remaining, but there is nowhere to put them. Room Number ({2} of {3})</color>";
		object[] array = new object[4];
		array[0] = this;
		array[1] = biomeController.Biome;
		array[2] = this.RoomNumber;
		array[3] = biomeController.TargetRoomCountsByRoomType.Sum((KeyValuePair<RoomType, int> entry) => entry.Value);
		string text = string.Format(format, array);
		Debug.LogFormat(text, Array.Empty<object>());
		this.SetState(biomeController, BiomeBuildStateID.Failed, text);
		return null;
	}

	// Token: 0x0600515D RID: 20829 RVA: 0x0002C6C3 File Offset: 0x0002A8C3
	private static List<DoorLocation> GetDoorsInRoom(GridPointManager gridPointManager)
	{
		return gridPointManager.DoorLocations;
	}

	// Token: 0x0600515E RID: 20830 RVA: 0x00134770 File Offset: 0x00132970
	public IEnumerable<GridPoint> GetGridPointsWithAvailableDoorsOnSide(BiomeController biomeController, RoomSide side)
	{
		Func<GridPoint, bool> predicate;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			predicate = ((GridPoint entry) => entry.Doors.Any((DoorLocation door) => door.RoomSide == RoomSide.Left && this.GetIsDoorLocationValid(biomeController, entry.Owner, door)));
			if (side == RoomSide.Right)
			{
				predicate = ((GridPoint entry) => entry.Doors.Any((DoorLocation door) => door.RoomSide == RoomSide.Right && this.GetIsDoorLocationValid(biomeController, entry.Owner, door)));
			}
		}
		else
		{
			predicate = ((GridPoint entry) => entry.Doors.Any((DoorLocation door) => door.RoomSide == RoomSide.Bottom && this.GetIsDoorLocationValid(biomeController, entry.Owner, door)));
			if (side == RoomSide.Top)
			{
				predicate = ((GridPoint entry) => entry.Doors.Any((DoorLocation door) => door.RoomSide == RoomSide.Top && this.GetIsDoorLocationValid(biomeController, entry.Owner, door)));
			}
		}
		return biomeController.GridPointManager.GridPoints.Where(predicate);
	}

	// Token: 0x0600515F RID: 20831 RVA: 0x001347F0 File Offset: 0x001329F0
	public IEnumerable<GridPoint> GetGridPointsAtExtent(BiomeController biomeController, RoomSide side, int extentOffset)
	{
		if (extentOffset < 0)
		{
			extentOffset = 0;
		}
		Func<GridPoint, bool> predicate;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			predicate = ((GridPoint entry) => entry.GridCoordinates.x <= biomeController.GridPointManager.Extents[RoomSide.Left] + extentOffset);
			if (side == RoomSide.Right)
			{
				predicate = ((GridPoint entry) => entry.GridCoordinates.x + 1 >= biomeController.GridPointManager.Extents[RoomSide.Right] - extentOffset);
			}
		}
		else
		{
			predicate = ((GridPoint entry) => entry.GridCoordinates.y <= biomeController.GridPointManager.Extents[RoomSide.Bottom] + extentOffset);
			if (side == RoomSide.Top)
			{
				predicate = ((GridPoint entry) => entry.GridCoordinates.y + 1 >= biomeController.GridPointManager.Extents[RoomSide.Top] - extentOffset);
			}
		}
		return biomeController.GridPointManager.GridPoints.Where(predicate);
	}

	// Token: 0x06005160 RID: 20832 RVA: 0x00134880 File Offset: 0x00132A80
	public IEnumerable<GridPoint> GetGridPointsOfRoomType(BiomeController biomeController, RoomType roomType)
	{
		return from gridPoint in biomeController.GridPointManager.GridPoints
		where gridPoint.Owner.RoomType == roomType
		select gridPoint;
	}

	// Token: 0x06005161 RID: 20833 RVA: 0x0002C6CB File Offset: 0x0002A8CB
	public IEnumerable<GridPoint> GetGridPointsWithSpaceOnSide(BiomeController biomeController, RoomSide side, IEnumerable<GridPoint> filteredGridPoints)
	{
		int maxExtents = GridController.Extents[side];
		foreach (GridPoint gridPoint in filteredGridPoints)
		{
			Vector2Int gridCoordsOnGridPointSide = GridController.GetGridCoordsOnGridPointSide(gridPoint, side);
			Vector2Int zero = Vector2Int.zero;
			if (side == RoomSide.Left)
			{
				zero = new Vector2Int(gridCoordsOnGridPointSide.x + 1 - maxExtents, 1);
			}
			else if (side == RoomSide.Right)
			{
				zero = new Vector2Int(maxExtents - gridCoordsOnGridPointSide.x, 1);
			}
			else if (side == RoomSide.Top)
			{
				zero = new Vector2Int(1, maxExtents - gridCoordsOnGridPointSide.y);
			}
			else if (side == RoomSide.Bottom)
			{
				zero = new Vector2Int(1, gridCoordsOnGridPointSide.y + 1 - maxExtents);
			}
			if (GridController.GetIsSpaceForRoomAtDoor(gridCoordsOnGridPointSide, zero, side, 0))
			{
				yield return gridPoint;
			}
		}
		IEnumerator<GridPoint> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x06005162 RID: 20834 RVA: 0x001348B8 File Offset: 0x00132AB8
	public List<GridPoint> GetGridPointsWhereTransitionRoomFits(BiomeController biomeController, IEnumerable<GridPoint> filteredGridPoints)
	{
		List<GridPoint> list = new List<GridPoint>();
		BiomeData data = BiomeDataLibrary.GetData(biomeController.Biome);
		RoomMetaData transitionRoom = RoomLibrary.GetSetCollection(biomeController.Biome).TransitionRoom;
		RoomSide connectToSide = data.ConnectDirection;
		IEnumerable<DoorLocation> enumerable = from door in transitionRoom.DoorLocations
		where door.RoomSide == connectToSide
		select door;
		if (biomeController.Biome != BiomeType.Stone)
		{
			int num = transitionRoom.Size.x;
			if (connectToSide == RoomSide.Left || connectToSide == RoomSide.Right)
			{
				num = transitionRoom.Size.y;
			}
			for (int i = 0; i < num; i++)
			{
				RoomSide oppositeSide = RoomUtility.GetOppositeSide(connectToSide);
				DoorLocation doorLocation = new DoorLocation(oppositeSide, i);
				if (transitionRoom.GetHasDoor(doorLocation, false))
				{
					foreach (GridPoint gridPoint in filteredGridPoints)
					{
						Vector2Int gridCoordsOnGridPointSide = GridController.GetGridCoordsOnGridPointSide(gridPoint, connectToSide);
						Vector2Int size = transitionRoom.Size;
						if (GridController.GetIsSpaceForRoomAtDoor(gridCoordsOnGridPointSide, size, connectToSide, i))
						{
							Vector2Int roomGridCoordinates = GridController.GetRoomGridCoordinates(gridCoordsOnGridPointSide, transitionRoom.Size, doorLocation);
							if (this.GetIsWayOutOfTransitionRoom(roomGridCoordinates, size, enumerable))
							{
								list.Add(gridPoint);
							}
						}
					}
				}
			}
		}
		else
		{
			int num2 = WorldBuilder.GetBiomeController(biomeController.BiomeData.ConnectsTo).GridPointManager.Extents[RoomSide.Right];
			int num3 = CreateRoomsBuildRule_Stone.BRIDGE_HEIGHT - transitionRoom.Size.y;
			RoomSide oppositeSide2 = RoomUtility.GetOppositeSide(connectToSide);
			foreach (DoorLocation doorLocation2 in transitionRoom.GetDoorsOnSide(oppositeSide2, false).ToList<DoorLocation>())
			{
				foreach (GridPoint gridPoint2 in filteredGridPoints)
				{
					Vector2Int gridCoordsOnGridPointSide2 = GridController.GetGridCoordsOnGridPointSide(gridPoint2, connectToSide);
					int x = num2 - 1 - gridPoint2.GridCoordinates.x;
					Vector2Int roomSize = new Vector2Int(x, CreateRoomsBuildRule_Stone.BRIDGE_HEIGHT);
					int atDoorNumber = doorLocation2.DoorNumber + num3;
					if (GridController.GetIsSpaceForRoomAtDoor(gridCoordsOnGridPointSide2, roomSize, connectToSide, atDoorNumber))
					{
						Vector2Int roomGridCoordinates2 = GridController.GetRoomGridCoordinates(gridCoordsOnGridPointSide2, transitionRoom.Size, new DoorLocation(RoomUtility.GetOppositeSide(connectToSide), 0));
						bool flag = false;
						foreach (DoorLocation doorLocation3 in enumerable)
						{
							int doorNumber = doorLocation3.DoorNumber + num3;
							DoorLocation location = new DoorLocation(connectToSide, doorNumber);
							if (GridController.GetIsGridSpaceAvailable(GridController.GetDoorLeadsToGridCoordinates(roomGridCoordinates2, roomSize, location)))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							list.Add(gridPoint2);
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06005163 RID: 20835 RVA: 0x00134C0C File Offset: 0x00132E0C
	private bool GetIsWayOutOfTransitionRoom(Vector2Int potentialGridCoordinates, Vector2Int transitionRoomSize, IEnumerable<DoorLocation> doorLocationsInTransitionRoom)
	{
		bool result = false;
		foreach (DoorLocation location in doorLocationsInTransitionRoom)
		{
			if (GridController.GetIsGridSpaceAvailable(GridController.GetDoorLeadsToGridCoordinates(potentialGridCoordinates, transitionRoomSize, location)))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005164 RID: 20836 RVA: 0x00134C64 File Offset: 0x00132E64
	public IEnumerable<GridPoint> GetGridPointsOnSideOfBiomeTransitionRoom(BiomeController biomeController, RoomSide side)
	{
		Vector2Int transitionRoomSize = RoomLibrary.GetSetCollection(biomeController.Biome).TransitionRoom.Size;
		Func<GridPoint, bool> predicate;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			predicate = ((GridPoint entry) => entry.GridCoordinates.x <= biomeController.GridPointManager.Center.x);
			if (side == RoomSide.Right)
			{
				predicate = ((GridPoint entry) => entry.GridCoordinates.x >= biomeController.GridPointManager.Center.x + transitionRoomSize.x - 1);
			}
		}
		else
		{
			predicate = ((GridPoint entry) => entry.GridCoordinates.y <= biomeController.GridPointManager.Center.y);
			if (side == RoomSide.Top)
			{
				predicate = ((GridPoint entry) => entry.GridCoordinates.y >= biomeController.GridPointManager.Center.y + transitionRoomSize.y - 1);
			}
		}
		return biomeController.GridPointManager.GridPoints.Where(predicate);
	}

	// Token: 0x06005165 RID: 20837 RVA: 0x0002C6E2 File Offset: 0x0002A8E2
	public IEnumerable<GridPoint> GetGridPointsWhereTransitionRoomFitsAtDoorNumber(Room transitionRoomPrefab, RoomSide attachmentSide, DoorLocation transitionRoomDoorLocation, IEnumerable<GridPoint> potentialGridPoints)
	{
		IEnumerable<DoorLocation> doorLocationsInTransitionRoom = from door in transitionRoomPrefab.Doors
		where door.Side != attachmentSide
		select new DoorLocation(door.Side, door.Number);
		foreach (GridPoint gridPoint in potentialGridPoints)
		{
			if (!(transitionRoomPrefab.GetDoor(transitionRoomDoorLocation.RoomSide, transitionRoomDoorLocation.DoorNumber) == null) && !(gridPoint.GetDoorLocation(attachmentSide) == DoorLocation.Empty))
			{
				Vector2Int leadsToCoords = gridPoint.Owner.GetDoorLeadsToGridCoordinates(gridPoint.GetDoorLocation(attachmentSide));
				if (GridController.GetIsSpaceForRoomAtDoor(leadsToCoords, transitionRoomPrefab.Size, attachmentSide, transitionRoomDoorLocation.DoorNumber) && doorLocationsInTransitionRoom.Any((DoorLocation door) => GridController.GetIsGridSpaceAvailable(GridController.GetDoorLeadsToGridCoordinates(leadsToCoords, transitionRoomPrefab.Size, door))))
				{
					yield return gridPoint;
				}
			}
		}
		IEnumerator<GridPoint> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x06005166 RID: 20838 RVA: 0x00134CFC File Offset: 0x00132EFC
	public IEnumerable<GridPoint> GetGridPointsWhereTransitionRoomCouldAttachOnSideAtDoorNumber(BiomeController biomeController, Room transitionRoomPrefab, RoomSide side, int doorNumber)
	{
		Func<GridPoint, bool> predicate;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			predicate = ((GridPoint entry) => GridController.GetIsSpaceForRoomAtDoor(entry.GridCoordinates + Vector2Int.left, new Vector2Int(entry.GridCoordinates.x - biomeController.GridPointManager.Extents[side], transitionRoomPrefab.Size.y), side, doorNumber));
			if (side == RoomSide.Right)
			{
				predicate = ((GridPoint entry) => GridController.GetIsSpaceForRoomAtDoor(entry.GridCoordinates + Vector2Int.right, new Vector2Int(biomeController.GridPointManager.Extents[side] - entry.GridCoordinates.x, transitionRoomPrefab.Size.y), side, doorNumber));
			}
		}
		else
		{
			predicate = ((GridPoint entry) => GridController.GetIsSpaceForRoomAtDoor(entry.GridCoordinates + Vector2Int.down, new Vector2Int(transitionRoomPrefab.Size.x, entry.GridCoordinates.y - biomeController.GridPointManager.Extents[side]), side, doorNumber));
			if (side == RoomSide.Top)
			{
				predicate = ((GridPoint entry) => GridController.GetIsSpaceForRoomAtDoor(entry.GridCoordinates + Vector2Int.up, new Vector2Int(transitionRoomPrefab.Size.x, biomeController.GridPointManager.Extents[side] - entry.GridCoordinates.y), side, doorNumber));
			}
		}
		List<GridPoint> list = biomeController.GridPointManager.GridPoints.Where(predicate).ToList<GridPoint>();
		IEnumerable<DoorLocation> source = from door in transitionRoomPrefab.Doors
		where door.Side != side
		select new DoorLocation(door.Side, door.Number);
		List<GridPoint> list2 = new List<GridPoint>();
		foreach (GridPoint gridPoint in list)
		{
			DoorLocation location = new DoorLocation(side, 0);
			Vector2Int leadsToCoords = gridPoint.Owner.GetDoorLeadsToGridCoordinates(location);
			if (source.Any((DoorLocation door) => GridController.GetIsGridSpaceAvailable(GridController.GetDoorLeadsToGridCoordinates(leadsToCoords, transitionRoomPrefab.Size, door))))
			{
				list2.Add(gridPoint);
			}
		}
		return list2;
	}

	// Token: 0x06005167 RID: 20839 RVA: 0x00134E7C File Offset: 0x0013307C
	public IEnumerable<GridPoint> GetPotentialConnectionPoints(BiomeController biomeController, RoomSide side)
	{
		Func<GridPoint, bool> predicate;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			predicate = ((GridPoint entry) => entry.GridCoordinates.x == biomeController.GridPointManager.Extents[RoomSide.Left]);
			if (side == RoomSide.Right)
			{
				predicate = ((GridPoint entry) => entry.GridCoordinates.x + entry.RoomMetaData.Size.x == biomeController.GridPointManager.Extents[RoomSide.Right]);
			}
		}
		else
		{
			predicate = ((GridPoint entry) => entry.GridCoordinates.y == biomeController.GridPointManager.Extents[RoomSide.Bottom]);
			if (side == RoomSide.Top)
			{
				predicate = ((GridPoint entry) => entry.GridCoordinates.y + entry.RoomMetaData.Size.y == biomeController.GridPointManager.Extents[RoomSide.Top]);
			}
		}
		return from entry in biomeController.GridPointManager.GridPoints.Where(predicate)
		where entry.RoomType != RoomType.Transition
		select entry;
	}

	// Token: 0x06005168 RID: 20840 RVA: 0x00134F18 File Offset: 0x00133118
	public DoorLocation GetRandomDoorLocation(List<DoorLocation> doorLocations, RoomSetEntry roomEntry, BiomeType biome, int roomNumber)
	{
		RoomSide roomSide = doorLocations.First<DoorLocation>().RoomSide;
		List<DoorLocation> list = new List<DoorLocation>();
		if (roomEntry.IsMirrored)
		{
			using (IEnumerator<DoorLocation> enumerator = roomEntry.RoomMetaData.GetDoorsOnSide(roomSide, true).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DoorLocation doorLocation = enumerator.Current;
					DoorLocation mirrorDoorLocation = RoomUtility.GetMirrorDoorLocation(roomEntry.RoomMetaData.Size, doorLocation);
					list.Add(mirrorDoorLocation);
				}
				goto IL_A7;
			}
		}
		foreach (DoorLocation item in roomEntry.RoomMetaData.GetDoorsOnSide(roomSide, false))
		{
			list.Add(item);
		}
		IL_A7:
		doorLocations = doorLocations.Intersect(list).ToList<DoorLocation>();
		if (doorLocations.Count > 0)
		{
			int randomNumber = RNGManager.GetRandomNumber(RngID.BiomeCreation, string.Format("Get Random Door Location to attach Room #{0} in {1} Biome", roomNumber, biome), 0, doorLocations.Count);
			return doorLocations[randomNumber];
		}
		string text = "";
		for (int i = 0; i < doorLocations.Count; i++)
		{
			text += string.Format("({0},{1})", doorLocations[i].RoomSide, doorLocations[i].DoorNumber);
			if (i != doorLocations.Count - 1)
			{
				text += ", ";
			}
		}
		throw new Exception(string.Format("|{0}| Given Room Entry ({1},{2}) does not contain any of the given Door Locations ({3}). Take note of Seed and let Paul and/or Kenny know.", new object[]
		{
			this,
			roomEntry.RoomMetaData.ID.SceneName,
			roomEntry.RoomMetaData.ID.Number,
			text
		}));
	}

	// Token: 0x06005169 RID: 20841 RVA: 0x001350F8 File Offset: 0x001332F8
	public RoomSetEntry GetRandomRoomPrefab(BiomeController biomeController, RoomSetEntry[] potentialRooms, int rngWeightSum)
	{
		bool flag = false;
		if (potentialRooms.Length <= 1)
		{
			return potentialRooms[0];
		}
		if (flag)
		{
			int randomNumber = RNGManager.GetRandomNumber(RngID.BiomeCreation, string.Format("Get Random Room Prefab to use as Room #{0} from set of {1} Potential Rooms in {2} Biome.", this.RoomNumber + 1, potentialRooms.Length, biomeController.Biome), 0, potentialRooms.Length);
			return potentialRooms.ElementAt(randomNumber);
		}
		bool flag2 = true;
		int randomNumber2 = RNGManager.GetRandomNumber(RngID.BiomeCreation, string.Format("Get Random Room Prefab to use as Room #{0} from set of {1} Potential Rooms in {2} Biome.", this.RoomNumber + 1, potentialRooms.Length, biomeController.Biome), 0, rngWeightSum);
		if (flag2)
		{
			int indexOfWeightedRoomEntry = this.GetIndexOfWeightedRoomEntry(randomNumber2, potentialRooms);
			if (indexOfWeightedRoomEntry >= 0 && indexOfWeightedRoomEntry < potentialRooms.Length)
			{
				return potentialRooms[indexOfWeightedRoomEntry];
			}
		}
		else
		{
			for (int i = 0; i < potentialRooms.Length; i++)
			{
				if (potentialRooms[i].Weight >= randomNumber2)
				{
					return potentialRooms[i];
				}
			}
		}
		return default(RoomSetEntry);
	}

	// Token: 0x0600516A RID: 20842 RVA: 0x001351E4 File Offset: 0x001333E4
	private int GetIndexOfWeightedRoomEntry(int randomNumber, RoomSetEntry[] rooms)
	{
		int num = 0;
		int num2 = rooms.Length - 1;
		while (num != num2)
		{
			int num3 = (num2 + num) / 2;
			if (rooms[num3].Weight < randomNumber)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		return num;
	}

	// Token: 0x0600516B RID: 20843 RVA: 0x00135220 File Offset: 0x00133420
	public Dictionary<Vector2Int, List<DoorLocation>> GetRoomSizesAndWhatDoorLocationsTheyFitAt(Bounds border, Vector2Int maxRoomSize, GridPointManager originRoom, DoorLocation doorLocation)
	{
		Dictionary<Vector2Int, List<DoorLocation>> dictionary = new Dictionary<Vector2Int, List<DoorLocation>>();
		Vector2Int doorLeadsToGridCoordinates = originRoom.GetDoorLeadsToGridCoordinates(doorLocation);
		for (int i = maxRoomSize.x; i > 0; i--)
		{
			for (int j = maxRoomSize.y; j > 0; j--)
			{
				Vector2Int vector2Int = new Vector2Int(i, j);
				List<DoorLocation> doorLocationsThatFitAtCoordinates = this.GridController.GetDoorLocationsThatFitAtCoordinates(doorLeadsToGridCoordinates, border, doorLocation.RoomSide, vector2Int);
				if (doorLocationsThatFitAtCoordinates.Count > 0)
				{
					dictionary.Add(vector2Int, doorLocationsThatFitAtCoordinates);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x0600516C RID: 20844 RVA: 0x0002C708 File Offset: 0x0002A908
	public void AddRoomTypeToBacklog(RoomTypeEntry entry)
	{
		this.m_roomRequirementsController.AddToBacklog(entry);
	}

	// Token: 0x0600516D RID: 20845 RVA: 0x0002C716 File Offset: 0x0002A916
	public RoomTypeEntry GetTargetRoomRequirements()
	{
		return this.m_roomRequirementsController.GetRequirements(this.RoomNumber);
	}

	// Token: 0x0600516E RID: 20846 RVA: 0x00135298 File Offset: 0x00133498
	private void Initialise(BiomeController biomeController)
	{
		this.SetState(biomeController, BiomeBuildStateID.Running, "");
		if (this.BuildQueue == null)
		{
			this.BuildQueue = new Queue<GridPointManager>();
		}
		else
		{
			this.BuildQueue.Clear();
		}
		if (this.GridController == null)
		{
			this.GridController = new GridController();
		}
		this.RoomNumber = 0;
		this.SetBiomeRoomCounts(biomeController);
	}

	// Token: 0x0600516F RID: 20847 RVA: 0x0002C729 File Offset: 0x0002A929
	public void Reset()
	{
		this.m_buildQueue = new Queue<GridPointManager>();
		this.m_roomNumber = 0;
		this.m_specialRoomPlacementInterval = 0;
		this.m_specialRoomPool = null;
		this.m_state = BiomeBuildStateID.None;
	}

	// Token: 0x06005170 RID: 20848 RVA: 0x001352F4 File Offset: 0x001334F4
	private void SetBiomeRoomCounts(BiomeController biomeController)
	{
		biomeController.TargetRoomCountsByRoomType = new Dictionary<RoomType, int>();
		float num = 1f + BurdenManager.GetBurdenStatGain(BurdenType.RoomCount);
		int minInclusive = Mathf.RoundToInt((float)biomeController.BiomeData.MinFairyRoomCount * num);
		int num2 = Mathf.RoundToInt((float)biomeController.BiomeData.MaxFairyRoomCount * num);
		int randomNumber = RNGManager.GetRandomNumber(RngID.BiomeCreation, string.Format("Get Fairy Room Count in {0} Biome", biomeController.Biome), minInclusive, num2 + 1);
		biomeController.TargetRoomCountsByRoomType[RoomType.Fairy] = randomNumber;
		int minInclusive2 = Mathf.RoundToInt((float)biomeController.BiomeData.MinRoomCount * num);
		int num3 = Mathf.RoundToInt((float)biomeController.BiomeData.MaxRoomCount * num);
		int randomNumber2 = RNGManager.GetRandomNumber(RngID.BiomeCreation, string.Format("Get Standard Room Count in {0} Biome", biomeController.Biome), minInclusive2, num3 + 1);
		biomeController.TargetRoomCountsByRoomType[RoomType.Standard] = randomNumber2;
		int minInclusive3 = Mathf.RoundToInt((float)biomeController.BiomeData.MinTrapRoomCount * num);
		int num4 = Mathf.RoundToInt((float)biomeController.BiomeData.MaxTrapRoomCount * num);
		int randomNumber3 = RNGManager.GetRandomNumber(RngID.BiomeCreation, string.Format("Get Trap Room Count in {0} Biome", biomeController.Biome), minInclusive3, num4 + 1);
		biomeController.TargetRoomCountsByRoomType[RoomType.Trap] = randomNumber3;
		int minInclusive4 = Mathf.RoundToInt((float)biomeController.BiomeData.MinBonusRoomCount * num);
		int num5 = Mathf.RoundToInt((float)biomeController.BiomeData.MaxBonusRoomCount * num);
		int randomNumber4 = RNGManager.GetRandomNumber(RngID.BiomeCreation, string.Format("Get Bonus Room Count in {0} Biome", biomeController.Biome), minInclusive4, num5 + 1);
		biomeController.TargetRoomCountsByRoomType[RoomType.Bonus] = randomNumber4;
		int value = RoomLibrary.GetSetCollection(biomeController.Biome).MandatoryRooms.Length;
		biomeController.TargetRoomCountsByRoomType[RoomType.Mandatory] = value;
	}

	// Token: 0x06005171 RID: 20849 RVA: 0x001354AC File Offset: 0x001336AC
	private void SetPremadeBiomeRoomCounts(BiomeController biomeController, List<Room> roomList)
	{
		biomeController.TargetRoomCountsByRoomType = new Dictionary<RoomType, int>();
		biomeController.TargetRoomCountsByRoomType[RoomType.Fairy] = 0;
		biomeController.TargetRoomCountsByRoomType[RoomType.Standard] = roomList.Count;
		biomeController.TargetRoomCountsByRoomType[RoomType.Trap] = 0;
		biomeController.TargetRoomCountsByRoomType[RoomType.Bonus] = 0;
		biomeController.TargetRoomCountsByRoomType[RoomType.Mandatory] = 0;
	}

	// Token: 0x06005172 RID: 20850 RVA: 0x0002C752 File Offset: 0x0002A952
	private IEnumerator BuildForcedRooms(List<Room> roomList)
	{
		yield break;
	}

	// Token: 0x04003D57 RID: 15703
	[SerializeField]
	private bool m_displayDeadEndWarnings;

	// Token: 0x04003D58 RID: 15704
	[SerializeField]
	private bool m_drawGridGizmos;

	// Token: 0x04003D59 RID: 15705
	private static int FORCE_BUILD_ROOM_IF_TRANSITION_ROOM_DOOR_COUNT_IS_LESS_THAN = 2;

	// Token: 0x04003D5A RID: 15706
	private Queue<GridPointManager> m_buildQueue = new Queue<GridPointManager>();

	// Token: 0x04003D5B RID: 15707
	private GridController m_gridController;

	// Token: 0x04003D5C RID: 15708
	private int m_roomNumber;

	// Token: 0x04003D5D RID: 15709
	private int m_specialRoomPlacementInterval;

	// Token: 0x04003D5E RID: 15710
	private List<RoomTypeEntry> m_specialRoomPool;

	// Token: 0x04003D5F RID: 15711
	private BiomeBuildStateID m_state;

	// Token: 0x04003D60 RID: 15712
	private static string m_deadEndSeeds = "";

	// Token: 0x04003D61 RID: 15713
	private RoomRequirementsController m_roomRequirementsController;

	// Token: 0x04003D62 RID: 15714
	private Dictionary<BiomeType, int> m_timesBiomeHasConnectedTooEarly = new Dictionary<BiomeType, int>();

	// Token: 0x04003D66 RID: 15718
	private CreateRoomsBuildRule m_createRoomsBuildRule;

	// Token: 0x04003D67 RID: 15719
	private Dictionary<GridPointManager, List<DoorLocation>> m_checkedDoors = new Dictionary<GridPointManager, List<DoorLocation>>();
}
