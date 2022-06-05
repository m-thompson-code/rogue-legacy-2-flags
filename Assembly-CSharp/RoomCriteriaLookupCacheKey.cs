using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200065F RID: 1631
public class RoomCriteriaLookupCacheKey
{
	// Token: 0x170014B9 RID: 5305
	// (get) Token: 0x06003B19 RID: 15129 RVA: 0x000CB487 File Offset: 0x000C9687
	// (set) Token: 0x06003B1A RID: 15130 RVA: 0x000CB48F File Offset: 0x000C968F
	public BiomeType Biome { get; private set; }

	// Token: 0x170014BA RID: 5306
	// (get) Token: 0x06003B1B RID: 15131 RVA: 0x000CB498 File Offset: 0x000C9698
	// (set) Token: 0x06003B1C RID: 15132 RVA: 0x000CB4A0 File Offset: 0x000C96A0
	public RoomSide RoomSide { get; private set; }

	// Token: 0x170014BB RID: 5307
	// (get) Token: 0x06003B1D RID: 15133 RVA: 0x000CB4A9 File Offset: 0x000C96A9
	// (set) Token: 0x06003B1E RID: 15134 RVA: 0x000CB4B1 File Offset: 0x000C96B1
	public int[] RoomSizeToDoorLocationTable { get; set; }

	// Token: 0x170014BC RID: 5308
	// (get) Token: 0x06003B1F RID: 15135 RVA: 0x000CB4BA File Offset: 0x000C96BA
	// (set) Token: 0x06003B20 RID: 15136 RVA: 0x000CB4C2 File Offset: 0x000C96C2
	public RoomType RoomType { get; private set; }

	// Token: 0x170014BD RID: 5309
	// (get) Token: 0x06003B21 RID: 15137 RVA: 0x000CB4CB File Offset: 0x000C96CB
	// (set) Token: 0x06003B22 RID: 15138 RVA: 0x000CB4D3 File Offset: 0x000C96D3
	public SpecialRoomType SpecialRoomType { get; private set; }

	// Token: 0x170014BE RID: 5310
	// (get) Token: 0x06003B23 RID: 15139 RVA: 0x000CB4DC File Offset: 0x000C96DC
	// (set) Token: 0x06003B24 RID: 15140 RVA: 0x000CB4E4 File Offset: 0x000C96E4
	public bool MustBeEasy { get; private set; }

	// Token: 0x06003B25 RID: 15141 RVA: 0x000CB4ED File Offset: 0x000C96ED
	public RoomCriteriaLookupCacheKey(BiomeType biome, RoomSide side, RoomType roomType, Dictionary<Vector2Int, List<DoorLocation>> roomSizeToDoorLocations, SpecialRoomType specialRoomType, bool mustBeEasy)
	{
		this.Initialise(biome, side, roomType, roomSizeToDoorLocations, specialRoomType, mustBeEasy);
	}

	// Token: 0x06003B26 RID: 15142 RVA: 0x000CB504 File Offset: 0x000C9704
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

	// Token: 0x06003B27 RID: 15143 RVA: 0x000CB588 File Offset: 0x000C9788
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

	// Token: 0x06003B28 RID: 15144 RVA: 0x000CB614 File Offset: 0x000C9814
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

	// Token: 0x06003B29 RID: 15145 RVA: 0x000CB69C File Offset: 0x000C989C
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

	// Token: 0x02000DCF RID: 3535
	public class EqualityComparer : EqualityComparer<RoomCriteriaLookupCacheKey>
	{
		// Token: 0x060069F1 RID: 27121 RVA: 0x0018CA38 File Offset: 0x0018AC38
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

		// Token: 0x060069F2 RID: 27122 RVA: 0x0018CABC File Offset: 0x0018ACBC
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
