using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

// Token: 0x02000404 RID: 1028
public class RoomSetCollection
{
	// Token: 0x17000E84 RID: 3716
	// (get) Token: 0x06002101 RID: 8449 RVA: 0x000118DF File Offset: 0x0000FADF
	public BiomeType Biome { get; }

	// Token: 0x17000E85 RID: 3717
	// (get) Token: 0x06002102 RID: 8450 RVA: 0x000118E7 File Offset: 0x0000FAE7
	// (set) Token: 0x06002103 RID: 8451 RVA: 0x000118EF File Offset: 0x0000FAEF
	public MandatoryRoomEntry[] MandatoryRooms { get; set; }

	// Token: 0x17000E86 RID: 3718
	// (get) Token: 0x06002104 RID: 8452 RVA: 0x000118F8 File Offset: 0x0000FAF8
	// (set) Token: 0x06002105 RID: 8453 RVA: 0x00011919 File Offset: 0x0000FB19
	public Vector2Int MaxRoomSize
	{
		get
		{
			if (this.m_maxRoomSizeOverride == Vector2Int.zero)
			{
				return this.m_maxRoomSize;
			}
			return this.m_maxRoomSizeOverride;
		}
		private set
		{
			this.m_maxRoomSize = value;
		}
	}

	// Token: 0x17000E87 RID: 3719
	// (get) Token: 0x06002106 RID: 8454 RVA: 0x00011922 File Offset: 0x0000FB22
	// (set) Token: 0x06002107 RID: 8455 RVA: 0x0001192A File Offset: 0x0000FB2A
	public HashSet<RoomSetEntry> RoomSet { get; set; }

	// Token: 0x17000E88 RID: 3720
	// (get) Token: 0x06002108 RID: 8456 RVA: 0x00011933 File Offset: 0x0000FB33
	// (set) Token: 0x06002109 RID: 8457 RVA: 0x0001193B File Offset: 0x0000FB3B
	public Dictionary<RoomType, HashSet<RoomSetEntry>> RoomTypeSets { get; set; }

	// Token: 0x17000E89 RID: 3721
	// (get) Token: 0x0600210A RID: 8458 RVA: 0x00011944 File Offset: 0x0000FB44
	// (set) Token: 0x0600210B RID: 8459 RVA: 0x0001194C File Offset: 0x0000FB4C
	public RoomMetaData TransitionRoom { get; set; }

	// Token: 0x0600210C RID: 8460 RVA: 0x000A636C File Offset: 0x000A456C
	public RoomSetCollection(BiomeType biome, RoomPool roomPool)
	{
		this.Biome = biome;
		this.RoomSet = new HashSet<RoomSetEntry>();
		this.RoomTypeSets = new Dictionary<RoomType, HashSet<RoomSetEntry>>();
		this.MandatoryRooms = roomPool.GetMandatoryRoomEntries(biome);
		this.TransitionRoom = roomPool.GetTransitionRoom(biome);
		foreach (RoomType roomType in RoomType_RL.RoomTypeArray)
		{
			this.RoomTypeSets.Add(roomType, new HashSet<RoomSetEntry>());
			CompiledScene_ScriptableObject[] compiledScenes = roomPool.GetCompiledScenes(biome, roomType);
			int j = 0;
			while (j < compiledScenes.Length)
			{
				CompiledScene_ScriptableObject compiledScene_ScriptableObject = compiledScenes[j];
				if (compiledScene_ScriptableObject.RoomMetaData != null && compiledScene_ScriptableObject.RoomMetaData.Count > 0)
				{
					try
					{
						this.AddRoomsAndMirrorToSet(this.RoomTypeSets[roomType], compiledScene_ScriptableObject.RoomMetaData, compiledScene_ScriptableObject.name);
						this.AddRoomsAndMirrorToSet(this.RoomSet, compiledScene_ScriptableObject.RoomMetaData, compiledScene_ScriptableObject.name);
						goto IL_11C;
					}
					catch (Exception ex)
					{
						if (ex is UnassignedReferenceException)
						{
							Debug.LogFormat("<color=red>| RoomLibrary | Room GameObject List Field has not been assigned to on Compiled Scene SO ({0}). This may mean the compilation of Level Editor Scene ({1}) failed. Try recompiling the Scene.</color>", new object[]
							{
								compiledScene_ScriptableObject.name,
								compiledScene_ScriptableObject.SceneID
							});
						}
						throw;
					}
					goto IL_102;
				}
				goto IL_102;
				IL_11C:
				j++;
				continue;
				IL_102:
				Debug.LogFormat("<color=red>| RoomLibrary | Compiled Scene SO (<b>{0}</b>)'s Room GameObject List is empty</color>", new object[]
				{
					compiledScene_ScriptableObject.name
				});
				goto IL_11C;
			}
		}
		foreach (RoomSetEntry roomSetEntry in this.RoomSet)
		{
			if (roomSetEntry.RoomMetaData.Size.x > this.MaxRoomSize.x)
			{
				this.MaxRoomSize = new Vector2Int(roomSetEntry.RoomMetaData.Size.x, this.MaxRoomSize.y);
			}
			if (roomSetEntry.RoomMetaData.Size.y > this.MaxRoomSize.y)
			{
				this.MaxRoomSize = new Vector2Int(this.MaxRoomSize.x, roomSetEntry.RoomMetaData.Size.y);
			}
		}
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x00011955 File Offset: 0x0000FB55
	public void SetMaxRoomSizeOverride(Vector2Int maxRoomSize)
	{
		this.m_maxRoomSizeOverride = maxRoomSize;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x000A65B0 File Offset: 0x000A47B0
	private void AddRoomsAndMirrorToSet(HashSet<RoomSetEntry> set, IEnumerable<RoomMetaData> roomMetaData, string compiledSceneName = "")
	{
		if (set != null && roomMetaData != null)
		{
			foreach (RoomMetaData roomMetaData2 in roomMetaData)
			{
				if (roomMetaData2 != null)
				{
					set.Add(new RoomSetEntry(roomMetaData2, false));
					if (roomMetaData2.CanFlip)
					{
						bool flag = true;
						if (flag)
						{
							roomMetaData2.Size == Vector2Int.one;
						}
						if (flag)
						{
							int x = roomMetaData2.Size.x;
						}
						if (flag)
						{
							set.Add(new RoomSetEntry(roomMetaData2, true));
						}
					}
				}
				else
				{
					Debug.LogFormat("<color=red>| RoomSetCollection | <b>{0}</b> Null room found in {1}.</color>", new object[]
					{
						this.Biome,
						compiledSceneName
					});
				}
			}
		}
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x0001195E File Offset: 0x0000FB5E
	public int GetRNGWeightSum(RoomCriteriaLookupCacheKey criteria)
	{
		return RoomSetCollection.m_rngWeightSum[criteria];
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x000A6684 File Offset: 0x000A4884
	public RoomSetEntry[] GetRoomsMatchingCriteria(RoomCriteriaLookupCacheKey criteria, List<RoomSetEntry> standardRoomPoolOverride)
	{
		if (RoomSetCollection.m_roomCriteriaTable == null)
		{
			RoomSetCollection.m_roomCriteriaTable = new Dictionary<RoomCriteriaLookupCacheKey, RoomSetEntry[]>(new RoomCriteriaLookupCacheKey.EqualityComparer());
		}
		if (RoomSetCollection.m_rngWeightSum == null)
		{
			RoomSetCollection.m_rngWeightSum = new Dictionary<RoomCriteriaLookupCacheKey, int>();
		}
		RoomSetEntry[] array;
		if (RoomSetCollection.m_roomCriteriaTable.TryGetValue(criteria, out array))
		{
			return array;
		}
		RoomSetCollection.m_roomList.Clear();
		HashSet<RoomSetEntry> hashSet;
		if (standardRoomPoolOverride == null)
		{
			hashSet = RoomLibrary.GetSetCollection(criteria.Biome).RoomTypeSets[criteria.RoomType];
		}
		else
		{
			hashSet = new HashSet<RoomSetEntry>(standardRoomPoolOverride);
		}
		foreach (RoomSetEntry original in hashSet)
		{
			Vector2Int size = original.RoomMetaData.Size;
			int num = (size.x - 1) * 3 + (size.y - 1);
			if (criteria.RoomSizeToDoorLocationTable[num] != 0 && (criteria.RoomType != RoomType.Bonus || (original.RoomMetaData.IsSpecialRoom && (original.RoomMetaData.SpecialRoomType == criteria.SpecialRoomType || original.RoomMetaData.SpecialRoomType == SpecialRoomType.IncludeInAll))) && (!criteria.MustBeEasy || original.RoomMetaData.IsEasy) && (original.DoorMask & criteria.RoomSizeToDoorLocationTable[num]) != 0)
			{
				int weight = RoomWeightManager.GetWeight(original.RoomMetaData.ID, original.IsMirrored);
				RoomSetEntry item = new RoomSetEntry(original, weight);
				RoomSetCollection.m_roomList.Add(item);
			}
		}
		array = RoomSetCollection.m_roomList.ToArray();
		RoomSetCollection.m_rngWeightSum.Add(criteria, RoomSetCollection.SetRoomWeights(array));
		RoomSetCollection.m_roomCriteriaTable.Add(criteria, array);
		return array;
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x000A6834 File Offset: 0x000A4A34
	private static int SetRoomWeights(RoomSetEntry[] rooms)
	{
		int num = 0;
		for (int i = 0; i < rooms.Length; i++)
		{
			int num2 = num + rooms[i].Weight;
			rooms[i] = new RoomSetEntry(rooms[i], num2);
			num = num2;
		}
		return num;
	}

	// Token: 0x04001DE2 RID: 7650
	private static List<RoomSetEntry> m_roomList = new List<RoomSetEntry>(100);

	// Token: 0x04001DE3 RID: 7651
	private static Dictionary<RoomCriteriaLookupCacheKey, RoomSetEntry[]> m_roomCriteriaTable = null;

	// Token: 0x04001DE4 RID: 7652
	private static Dictionary<RoomCriteriaLookupCacheKey, int> m_rngWeightSum = null;

	// Token: 0x04001DE5 RID: 7653
	private Vector2Int m_maxRoomSizeOverride = Vector2Int.zero;

	// Token: 0x04001DE6 RID: 7654
	private Vector2Int m_maxRoomSize;
}
