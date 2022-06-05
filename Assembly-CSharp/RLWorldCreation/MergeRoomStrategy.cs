using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RLWorldCreation
{
	// Token: 0x02000DB0 RID: 3504
	public class MergeRoomStrategy
	{
		// Token: 0x060062E8 RID: 25320 RVA: 0x00036847 File Offset: 0x00034A47
		public virtual IEnumerator Run(CreateMergeRooms_BuildStage buildStage, BiomeController biomeController)
		{
			this.m_buildStage = buildStage;
			List<GridPointManager> potentialMergeTargets_V = this.GetPotentialMergeTargets_V2(biomeController, biomeController.Biome, new RoomType[]
			{
				RoomType.BossEntrance,
				RoomType.Fairy,
				RoomType.Transition,
				RoomType.Bonus
			});
			yield return this.MergeRooms_V2(biomeController, biomeController.Biome, potentialMergeTargets_V, true);
			yield break;
		}

		// Token: 0x060062E9 RID: 25321 RVA: 0x00170D28 File Offset: 0x0016EF28
		protected virtual List<GridPointManager> GetPotentialMergeTargets_V2(BiomeController biomeController, BiomeType biome, params RoomType[] exludeRoomTypes)
		{
			return (from room in biomeController.GridPointManager.GridPointManagers
			where room.RoomMetaData.CanMerge && room.Biome == biome && !exludeRoomTypes.Contains(room.RoomType)
			select room).ToList<GridPointManager>();
		}

		// Token: 0x060062EA RID: 25322 RVA: 0x00036864 File Offset: 0x00034A64
		protected IEnumerator MergeRooms_V2(BiomeController biomeController, BiomeType biome, List<GridPointManager> rooms, bool shuffle = true)
		{
			if (shuffle)
			{
				RoomUtility.ShuffleList<GridPointManager>(rooms);
			}
			while (rooms.Count > 1)
			{
				GridPointManager baseRoom = rooms.First<GridPointManager>();
				rooms.RemoveAt(0);
				RoomSide[] sidesToCheckForMerge_V = MergeRoomStrategy.GetSidesToCheckForMerge_V2(baseRoom);
				HashSet<GridPointManager> hashSet = new HashSet<GridPointManager>();
				foreach (RoomSide side in sidesToCheckForMerge_V)
				{
					List<GridPointManager> other = this.RecursiveMerge_V2(biomeController, rooms, baseRoom, null, side);
					hashSet.UnionWith(other);
				}
				if (hashSet.Count > 1)
				{
					foreach (GridPointManager gridPointManager in hashSet)
					{
						rooms.Remove(gridPointManager);
						gridPointManager.SetMergeWithRooms(hashSet.ToList<GridPointManager>());
					}
				}
			}
			MergeRoomStrategy.UpdateBiomeControllerIndices(biomeController);
			yield break;
		}

		// Token: 0x060062EB RID: 25323 RVA: 0x00170D6C File Offset: 0x0016EF6C
		protected static void UpdateBiomeControllerIndices(BiomeController biomeController)
		{
			List<GridPointManager> list = new List<GridPointManager>();
			List<GridPointManager> list2 = new List<GridPointManager>();
			for (int i = 0; i < biomeController.GridPointManager.GridPointManagers.Count; i++)
			{
				GridPointManager gridPointManager = biomeController.GridPointManager.GridPointManagers[i];
				if (gridPointManager.MergeWithGridPointManagers.Count == 0)
				{
					list.Add(gridPointManager);
				}
				else
				{
					list2.Add(gridPointManager);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].SetBiomeControllerIndex(j);
			}
			List<GridPointManager> list3 = new List<GridPointManager>();
			while (list2.Count > 0)
			{
				GridPointManager gridPointManager2 = list2[0];
				list2.RemoveAt(0);
				list3.Add(gridPointManager2);
				for (int k = 0; k < gridPointManager2.MergeWithGridPointManagers.Count; k++)
				{
					GridPointManager item = gridPointManager2.MergeWithGridPointManagers[k];
					list2.Remove(item);
				}
			}
			int num = list.Count;
			for (int l = 0; l < list3.Count; l++)
			{
				list3[l].SetBiomeControllerIndex(num);
				for (int m = 0; m < list3[l].MergeWithGridPointManagers.Count; m++)
				{
					list3[l].MergeWithGridPointManagers[m].SetBiomeControllerIndex(num);
				}
				num++;
			}
			biomeController.GridPointManager.SetStandaloneRoomCount(list.Count);
			biomeController.GridPointManager.SetMergeRoomCount(list3.Count);
		}

		// Token: 0x060062EC RID: 25324 RVA: 0x00170EE4 File Offset: 0x0016F0E4
		private List<GridPointManager> RecursiveMerge_V2(BiomeController biomeController, List<GridPointManager> rooms, GridPointManager baseRoom, GridPointManager roomToCheck, RoomSide side)
		{
			List<GridPointManager> list = new List<GridPointManager>();
			GridPointManager gridPointManager = baseRoom;
			if (roomToCheck != null)
			{
				gridPointManager = roomToCheck;
				list.Add(gridPointManager);
			}
			else
			{
				list.Add(baseRoom);
			}
			List<DoorLocation> doorLocationsOnSide = gridPointManager.GetDoorLocationsOnSide(side);
			if (doorLocationsOnSide.Count > 0)
			{
				List<GridPointManager> list2 = new List<GridPointManager>();
				foreach (DoorLocation doorLocation in doorLocationsOnSide)
				{
					GridPointManager connectedRoom = gridPointManager.GetConnectedRoom(doorLocation);
					if (connectedRoom != null && !list2.Contains(connectedRoom))
					{
						list2.Add(connectedRoom);
					}
				}
				if (list2.Count == 1)
				{
					GridPointManager gridPointManager2 = list2[0];
					if (MergeRoomStrategy.IsRoomInRoomPool_V2(rooms, gridPointManager2) && MergeRoomStrategy.IsRoomInBiome_V2(baseRoom.Biome, gridPointManager2) && this.AttemptMerge(biomeController, side, gridPointManager2) && MergeRoomStrategy.AreTargetOfMergeBoundsValid_V2(baseRoom, gridPointManager2, side) && MergeRoomStrategy.IsDoorLayoutOfConnectedRoomValid_V2(baseRoom, gridPointManager, gridPointManager2, side))
					{
						list.AddRange(this.RecursiveMerge_V2(biomeController, rooms, baseRoom, gridPointManager2, side));
					}
				}
			}
			return list;
		}

		// Token: 0x060062ED RID: 25325 RVA: 0x00170FEC File Offset: 0x0016F1EC
		private static bool AreTargetOfMergeBoundsEqual_V2(GridPointManager baseRoom, GridPointManager target, RoomSide side)
		{
			bool result = false;
			if (side == RoomSide.Left || side == RoomSide.Right)
			{
				if (baseRoom.Bounds.min.y == target.Bounds.min.y && baseRoom.Bounds.max.y == target.Bounds.max.y)
				{
					result = true;
				}
			}
			else if (baseRoom.Bounds.min.x == target.Bounds.min.x && baseRoom.Bounds.max.x == target.Bounds.max.x)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060062EE RID: 25326 RVA: 0x001710AC File Offset: 0x0016F2AC
		private static bool AreTargetOfMergeBoundsValid_V2(GridPointManager baseRoom, GridPointManager target, RoomSide side)
		{
			bool result;
			if (MergeRoomStrategy.AreTargetOfMergeBoundsEqual_V2(baseRoom, target, side))
			{
				result = true;
			}
			else if (side == RoomSide.Left || side == RoomSide.Right)
			{
				result = (target.Bounds.min.y >= baseRoom.Bounds.min.y && target.Bounds.max.y <= baseRoom.Bounds.max.y);
			}
			else
			{
				result = (target.Bounds.min.x >= baseRoom.Bounds.min.x && target.Bounds.max.x <= baseRoom.Bounds.max.x);
			}
			return result;
		}

		// Token: 0x060062EF RID: 25327 RVA: 0x00171188 File Offset: 0x0016F388
		protected virtual bool AttemptMerge(BiomeController biomeController, RoomSide side, GridPointManager connectedRoom)
		{
			int mergeOdds = this.GetMergeOdds(biomeController, side);
			return mergeOdds > 0 && RNGManager.GetRandomNumber(RngID.MergeRooms, "Attempt Merge?", 0, 99) <= mergeOdds;
		}

		// Token: 0x060062F0 RID: 25328 RVA: 0x001711BC File Offset: 0x0016F3BC
		private int GetMergeOdds(BiomeController biomeController, RoomSide side)
		{
			switch (side)
			{
			case RoomSide.Top:
				return biomeController.BiomeData.MergeTopOdds;
			case RoomSide.Bottom:
				return biomeController.BiomeData.MergeBottomOdds;
			case RoomSide.Left:
				return biomeController.BiomeData.MergeLeftOdds;
			case RoomSide.Right:
				return biomeController.BiomeData.MergeRightOdds;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x060062F1 RID: 25329 RVA: 0x00171218 File Offset: 0x0016F418
		private static RoomSide[] GetSidesToCheckForMerge_V2(GridPointManager baseRoom)
		{
			RoomSide[] array = (from door in baseRoom.DoorLocations
			select door.RoomSide).ToArray<RoomSide>();
			int num = 0;
			if (array.Length > 1)
			{
				num = RNGManager.GetRandomNumber(RngID.MergeRooms, string.Format("Get Random Side of Room {0} to attempt merge", baseRoom.RoomNumber), 0, array.Length);
			}
			RoomSide[] result = new RoomSide[]
			{
				RoomSide.Left,
				RoomSide.Right
			};
			if (array[num] == RoomSide.Top || array[num] == RoomSide.Bottom)
			{
				result = new RoomSide[]
				{
					RoomSide.Top,
					RoomSide.Bottom
				};
			}
			return result;
		}

		// Token: 0x060062F2 RID: 25330 RVA: 0x001712A4 File Offset: 0x0016F4A4
		private static bool IsDoorLayoutOfConnectedRoomValid_V2(GridPointManager baseRoom, GridPointManager currentRoom, GridPointManager target, RoomSide side)
		{
			List<RoomSide> invalidSides = new List<RoomSide>();
			if (side == RoomSide.Left || side == RoomSide.Right)
			{
				if (target.Bounds.min.y > baseRoom.Bounds.min.y)
				{
					invalidSides.Add(RoomSide.Bottom);
				}
				if (target.Bounds.max.y < baseRoom.Bounds.max.y)
				{
					invalidSides.Add(RoomSide.Top);
				}
			}
			else
			{
				if (target.Bounds.min.x > baseRoom.Bounds.min.x)
				{
					invalidSides.Add(RoomSide.Left);
				}
				if (target.Bounds.max.x < baseRoom.Bounds.max.x)
				{
					invalidSides.Add(RoomSide.Right);
				}
			}
			bool flag = true;
			if (invalidSides.Count > 0)
			{
				flag = !target.DoorLocations.Any((DoorLocation d) => invalidSides.Contains(d.RoomSide));
			}
			if (flag)
			{
				RoomSide oppositeSide = RoomUtility.GetOppositeSide(side);
				List<DoorLocation> doorLocationsOnSide = target.GetDoorLocationsOnSide(oppositeSide);
				for (int i = 0; i < doorLocationsOnSide.Count; i++)
				{
					GridPointManager connectedRoom = target.GetConnectedRoom(doorLocationsOnSide[i]);
					if (connectedRoom != null && connectedRoom != currentRoom)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x060062F3 RID: 25331 RVA: 0x00036889 File Offset: 0x00034A89
		private static bool IsRoomInBiome_V2(BiomeType biome, GridPointManager room)
		{
			return room.Biome == biome;
		}

		// Token: 0x060062F4 RID: 25332 RVA: 0x00036894 File Offset: 0x00034A94
		private static bool IsRoomInRoomPool_V2(List<GridPointManager> roomPool, GridPointManager room)
		{
			return roomPool.Contains(room);
		}

		// Token: 0x040050CC RID: 20684
		protected CreateMergeRooms_BuildStage m_buildStage;
	}
}
