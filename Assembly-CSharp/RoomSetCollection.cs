using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

// Token: 0x02000247 RID: 583
public class RoomSetCollection
{
	// Token: 0x17000B57 RID: 2903
	// (get) Token: 0x0600174E RID: 5966 RVA: 0x000488B4 File Offset: 0x00046AB4
	public BiomeType Biome { get; }

	// Token: 0x17000B58 RID: 2904
	// (get) Token: 0x0600174F RID: 5967 RVA: 0x000488BC File Offset: 0x00046ABC
	// (set) Token: 0x06001750 RID: 5968 RVA: 0x000488C4 File Offset: 0x00046AC4
	public MandatoryRoomEntry[] MandatoryRooms { get; set; }

	// Token: 0x17000B59 RID: 2905
	// (get) Token: 0x06001751 RID: 5969 RVA: 0x000488CD File Offset: 0x00046ACD
	// (set) Token: 0x06001752 RID: 5970 RVA: 0x000488EE File Offset: 0x00046AEE
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

	// Token: 0x17000B5A RID: 2906
	// (get) Token: 0x06001753 RID: 5971 RVA: 0x000488F7 File Offset: 0x00046AF7
	// (set) Token: 0x06001754 RID: 5972 RVA: 0x000488FF File Offset: 0x00046AFF
	public HashSet<RoomSetEntry> RoomSet { get; set; }

	// Token: 0x17000B5B RID: 2907
	// (get) Token: 0x06001755 RID: 5973 RVA: 0x00048908 File Offset: 0x00046B08
	// (set) Token: 0x06001756 RID: 5974 RVA: 0x00048910 File Offset: 0x00046B10
	public Dictionary<RoomType, HashSet<RoomSetEntry>> RoomTypeSets { get; set; }

	// Token: 0x17000B5C RID: 2908
	// (get) Token: 0x06001757 RID: 5975 RVA: 0x00048919 File Offset: 0x00046B19
	// (set) Token: 0x06001758 RID: 5976 RVA: 0x00048921 File Offset: 0x00046B21
	public RoomMetaData TransitionRoom { get; set; }

	// Token: 0x06001759 RID: 5977 RVA: 0x0004892C File Offset: 0x00046B2C
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

	// Token: 0x0600175A RID: 5978 RVA: 0x00048B70 File Offset: 0x00046D70
	public void SetMaxRoomSizeOverride(Vector2Int maxRoomSize)
	{
		this.m_maxRoomSizeOverride = maxRoomSize;
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x00048B7C File Offset: 0x00046D7C
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

	// Token: 0x0600175C RID: 5980 RVA: 0x00048C50 File Offset: 0x00046E50
	public int GetRNGWeightSum(RoomCriteriaLookupCacheKey criteria)
	{
		return RoomSetCollection.m_rngWeightSum[criteria];
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x00048C60 File Offset: 0x00046E60
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

	// Token: 0x0600175E RID: 5982 RVA: 0x00048E10 File Offset: 0x00047010
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

	// Token: 0x040016CA RID: 5834
	private static List<RoomSetEntry> m_roomList = new List<RoomSetEntry>(100);

	// Token: 0x040016CB RID: 5835
	private static Dictionary<RoomCriteriaLookupCacheKey, RoomSetEntry[]> m_roomCriteriaTable = null;

	// Token: 0x040016CC RID: 5836
	private static Dictionary<RoomCriteriaLookupCacheKey, int> m_rngWeightSum = null;

	// Token: 0x040016CD RID: 5837
	private Vector2Int m_maxRoomSizeOverride = Vector2Int.zero;

	// Token: 0x040016CE RID: 5838
	private Vector2Int m_maxRoomSize;
}
