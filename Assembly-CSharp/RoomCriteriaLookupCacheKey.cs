using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AB8 RID: 2744
public class RoomCriteriaLookupCacheKey
{
	// Token: 0x17001C45 RID: 7237
	// (get) Token: 0x060052B1 RID: 21169 RVA: 0x0002D07B File Offset: 0x0002B27B
	// (set) Token: 0x060052B2 RID: 21170 RVA: 0x0002D083 File Offset: 0x0002B283
	public BiomeType Biome { get; private set; }

	// Token: 0x17001C46 RID: 7238
	// (get) Token: 0x060052B3 RID: 21171 RVA: 0x0002D08C File Offset: 0x0002B28C
	// (set) Token: 0x060052B4 RID: 21172 RVA: 0x0002D094 File Offset: 0x0002B294
	public RoomSide RoomSide { get; private set; }

	// Token: 0x17001C47 RID: 7239
	// (get) Token: 0x060052B5 RID: 21173 RVA: 0x0002D09D File Offset: 0x0002B29D
	// (set) Token: 0x060052B6 RID: 21174 RVA: 0x0002D0A5 File Offset: 0x0002B2A5
	public int[] RoomSizeToDoorLocationTable { get; set; }

	// Token: 0x17001C48 RID: 7240
	// (get) Token: 0x060052B7 RID: 21175 RVA: 0x0002D0AE File Offset: 0x0002B2AE
	// (set) Token: 0x060052B8 RID: 21176 RVA: 0x0002D0B6 File Offset: 0x0002B2B6
	public RoomType RoomType { get; private set; }

	// Token: 0x17001C49 RID: 7241
	// (get) Token: 0x060052B9 RID: 21177 RVA: 0x0002D0BF File Offset: 0x0002B2BF
	// (set) Token: 0x060052BA RID: 21178 RVA: 0x0002D0C7 File Offset: 0x0002B2C7
	public SpecialRoomType SpecialRoomType { get; private set; }

	// Token: 0x17001C4A RID: 7242
	// (get) Token: 0x060052BB RID: 21179 RVA: 0x0002D0D0 File Offset: 0x0002B2D0
	// (set) Token: 0x060052BC RID: 21180 RVA: 0x0002D0D8 File Offset: 0x0002B2D8
	public bool MustBeEasy { get; private set; }

	// Token: 0x060052BD RID: 21181 RVA: 0x0002D0E1 File Offset: 0x0002B2E1
	public RoomCriteriaLookupCacheKey(BiomeType biome, RoomSide side, RoomType roomType, Dictionary<Vector2Int, List<DoorLocation>> roomSizeToDoorLocations, SpecialRoomType specialRoomType, bool mustBeEasy)
	{
		this.Initialise(biome, side, roomType, roomSizeToDoorLocations, specialRoomType, mustBeEasy);
	}

	// Token: 0x060052BE RID: 21182 RVA: 0x00139E88 File Offset: 0x00138088
	public void Initialise(BiomeType biome, RoomSide side, RoomType roomType, Dictionary<Vector2Int, List<DoorLocation>> roomSizeToDoorLocations, SpecialRoomType specialRoomType, bool mustBeEasy)
	{
		this.Biome = biome;
		this.RoomSide = side;
		this.RoomType = roomType;
		this.SpecialRoomType = specialRoomType;
		this.MustBeEasy = mustBeEasy;
		this.RoomSizeToDoorLocationTable = new int[8];
		for (int i = 0; i < this.RoomSizeToDoorLocationTable.Length; i++)
		{
			int num = 0;
			Vector2Int key = new Vector2Int(i / 3 + 1, i % 3 + 1);
			List<DoorLocation> locations;
			if (roomSizeToDoorLocations.TryGetValue(key, out locations))
			{
				num = RoomUtility.GetDoorLocationMask(side, locations);
			}
			this.RoomSizeToDoorLocationTable[i] = num;
		}
	}

	// Token: 0x060052BF RID: 21183 RVA: 0x00139F0C File Offset: 0x0013810C
	public override bool Equals(object otherObject)
	{
		RoomCriteriaLookupCacheKey roomCriteriaLookupCacheKey = (RoomCriteriaLookupCacheKey)otherObject;
		bool result = false;
		if (this.Biome == roomCriteriaLookupCacheKey.Biome && this.RoomSide == roomCriteriaLookupCacheKey.RoomSide && this.RoomType == roomCriteriaLookupCacheKey.RoomType && this.SpecialRoomType == roomCriteriaLookupCacheKey.SpecialRoomType && this.MustBeEasy == roomCriteriaLookupCacheKey.MustBeEasy)
		{
			bool flag = true;
			for (int i = 0; i < this.RoomSizeToDoorLocationTable.Length; i++)
			{
				if (this.RoomSizeToDoorLocationTable[i] != roomCriteriaLookupCacheKey.RoomSizeToDoorLocationTable[i])
				{
					flag = false;
					break;
				}
			}
			result = flag;
		}
		return result;
	}

	// Token: 0x060052C0 RID: 21184 RVA: 0x00139F98 File Offset: 0x00138198
	public override int GetHashCode()
	{
		int num = 13;
		num = (num * 397 ^ (int)this.Biome);
		num = (num * 397 ^ (int)this.RoomSide);
		num = (num * 397 ^ (int)this.RoomType);
		num = (num * 397 ^ (int)this.SpecialRoomType);
		for (int i = 0; i < this.RoomSizeToDoorLocationTable.Length; i++)
		{
			num = (num * 397 ^ this.RoomSizeToDoorLocationTable[i]);
		}
		return num * 397 ^ (this.MustBeEasy ? 1 : 0);
	}

	// Token: 0x060052C1 RID: 21185 RVA: 0x0013A020 File Offset: 0x00138220
	public override string ToString()
	{
		return string.Format("RoomCriteriaLookupCacheKey - ({0}, {1}, {2}, {3}, {4}, {5})", new object[]
		{
			this.Biome,
			this.RoomSide,
			this.RoomSizeToDoorLocationTable.Length,
			this.RoomType,
			this.SpecialRoomType,
			this.MustBeEasy
		});
	}

	// Token: 0x02000AB9 RID: 2745
	public class EqualityComparer : EqualityComparer<RoomCriteriaLookupCacheKey>
	{
		// Token: 0x060052C2 RID: 21186 RVA: 0x0013A094 File Offset: 0x00138294
		public override bool Equals(RoomCriteriaLookupCacheKey x, RoomCriteriaLookupCacheKey y)
		{
			bool result = false;
			if (x.Biome == y.Biome && x.RoomSide == y.RoomSide && x.RoomType == y.RoomType && x.SpecialRoomType == y.SpecialRoomType && x.MustBeEasy == y.MustBeEasy)
			{
				bool flag = true;
				for (int i = 0; i < x.RoomSizeToDoorLocationTable.Length; i++)
				{
					if (x.RoomSizeToDoorLocationTable[i] != y.RoomSizeToDoorLocationTable[i])
					{
						flag = false;
						break;
					}
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x060052C3 RID: 21187 RVA: 0x0013A118 File Offset: 0x00138318
		public override int GetHashCode(RoomCriteriaLookupCacheKey key)
		{
			int num = 13;
			num = (num * 397 ^ (int)key.Biome);
			num = (num * 397 ^ (int)key.RoomType);
			num = (num * 397 ^ (int)key.SpecialRoomType);
			for (int i = 0; i < key.RoomSizeToDoorLocationTable.Length; i++)
			{
				num = (num * 397 ^ key.RoomSizeToDoorLocationTable[i]);
			}
			return num * 397 ^ (key.MustBeEasy ? 1 : 0);
		}
	}
}
