using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

// Token: 0x02000678 RID: 1656
public class GridPointManager
{
	// Token: 0x170014D0 RID: 5328
	// (get) Token: 0x06003BA4 RID: 15268 RVA: 0x000CD5AD File Offset: 0x000CB7AD
	// (set) Token: 0x06003BA5 RID: 15269 RVA: 0x000CD5B5 File Offset: 0x000CB7B5
	public int RoomSeed { get; private set; } = -1;

	// Token: 0x170014D1 RID: 5329
	// (get) Token: 0x06003BA6 RID: 15270 RVA: 0x000CD5BE File Offset: 0x000CB7BE
	// (set) Token: 0x06003BA7 RID: 15271 RVA: 0x000CD5C6 File Offset: 0x000CB7C6
	public int TunnelRoomSeed { get; private set; } = -1;

	// Token: 0x170014D2 RID: 5330
	// (get) Token: 0x06003BA8 RID: 15272 RVA: 0x000CD5CF File Offset: 0x000CB7CF
	// (set) Token: 0x06003BA9 RID: 15273 RVA: 0x000CD5D7 File Offset: 0x000CB7D7
	public int PropSeed { get; private set; } = -1;

	// Token: 0x170014D3 RID: 5331
	// (get) Token: 0x06003BAA RID: 15274 RVA: 0x000CD5E0 File Offset: 0x000CB7E0
	// (set) Token: 0x06003BAB RID: 15275 RVA: 0x000CD5E8 File Offset: 0x000CB7E8
	public int DecoSeed { get; private set; } = -1;

	// Token: 0x170014D4 RID: 5332
	// (get) Token: 0x06003BAC RID: 15276 RVA: 0x000CD5F1 File Offset: 0x000CB7F1
	// (set) Token: 0x06003BAD RID: 15277 RVA: 0x000CD5F9 File Offset: 0x000CB7F9
	public int EnemySeed { get; private set; } = -1;

	// Token: 0x170014D5 RID: 5333
	// (get) Token: 0x06003BAE RID: 15278 RVA: 0x000CD602 File Offset: 0x000CB802
	// (set) Token: 0x06003BAF RID: 15279 RVA: 0x000CD60A File Offset: 0x000CB80A
	public int ChestSeed { get; private set; } = -1;

	// Token: 0x170014D6 RID: 5334
	// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x000CD613 File Offset: 0x000CB813
	// (set) Token: 0x06003BB1 RID: 15281 RVA: 0x000CD61B File Offset: 0x000CB81B
	public int HazardSeed { get; private set; } = -1;

	// Token: 0x170014D7 RID: 5335
	// (get) Token: 0x06003BB2 RID: 15282 RVA: 0x000CD624 File Offset: 0x000CB824
	// (set) Token: 0x06003BB3 RID: 15283 RVA: 0x000CD62C File Offset: 0x000CB82C
	public int SpecialPropSeed { get; private set; } = -1;

	// Token: 0x170014D8 RID: 5336
	// (get) Token: 0x06003BB4 RID: 15284 RVA: 0x000CD635 File Offset: 0x000CB835
	public BiomeType Biome { get; }

	// Token: 0x170014D9 RID: 5337
	// (get) Token: 0x06003BB5 RID: 15285 RVA: 0x000CD63D File Offset: 0x000CB83D
	// (set) Token: 0x06003BB6 RID: 15286 RVA: 0x000CD645 File Offset: 0x000CB845
	public BiomeType AppearanceOverride
	{
		get
		{
			return this.m_appearanceOverride;
		}
		private set
		{
			this.m_appearanceOverride = value;
		}
	}

	// Token: 0x170014DA RID: 5338
	// (get) Token: 0x06003BB7 RID: 15287 RVA: 0x000CD64E File Offset: 0x000CB84E
	public Bounds Bounds { get; }

	// Token: 0x170014DB RID: 5339
	// (get) Token: 0x06003BB8 RID: 15288 RVA: 0x000CD656 File Offset: 0x000CB856
	// (set) Token: 0x06003BB9 RID: 15289 RVA: 0x000CD65E File Offset: 0x000CB85E
	public GridPointManagerContentEntry[] Content { get; set; }

	// Token: 0x170014DC RID: 5340
	// (get) Token: 0x06003BBA RID: 15290 RVA: 0x000CD668 File Offset: 0x000CB868
	public List<DoorLocation> DoorLocations
	{
		get
		{
			List<DoorLocation> list = new List<DoorLocation>();
			for (int i = 0; i < this.GridPoints.GetLength(0); i++)
			{
				for (int j = 0; j < this.GridPoints.GetLength(1); j++)
				{
					list.AddRange(this.GridPoints[i, j].Doors);
				}
			}
			return list;
		}
	}

	// Token: 0x170014DD RID: 5341
	// (get) Token: 0x06003BBB RID: 15291 RVA: 0x000CD6C2 File Offset: 0x000CB8C2
	public Vector2Int GridCoordinates { get; }

	// Token: 0x170014DE RID: 5342
	// (get) Token: 0x06003BBC RID: 15292 RVA: 0x000CD6CA File Offset: 0x000CB8CA
	// (set) Token: 0x06003BBD RID: 15293 RVA: 0x000CD6D2 File Offset: 0x000CB8D2
	public GridPoint[,] GridPoints { get; private set; }

	// Token: 0x170014DF RID: 5343
	// (get) Token: 0x06003BBE RID: 15294 RVA: 0x000CD6DB File Offset: 0x000CB8DB
	// (set) Token: 0x06003BBF RID: 15295 RVA: 0x000CD6E3 File Offset: 0x000CB8E3
	public List<GridPoint> GridPointList { get; private set; }

	// Token: 0x170014E0 RID: 5344
	// (get) Token: 0x06003BC0 RID: 15296 RVA: 0x000CD6EC File Offset: 0x000CB8EC
	// (set) Token: 0x06003BC1 RID: 15297 RVA: 0x000CD6F4 File Offset: 0x000CB8F4
	public bool IsRoomMirrored { get; private set; }

	// Token: 0x170014E1 RID: 5345
	// (get) Token: 0x06003BC2 RID: 15298 RVA: 0x000CD6FD File Offset: 0x000CB8FD
	public bool IsTunnelDestination { get; }

	// Token: 0x170014E2 RID: 5346
	// (get) Token: 0x06003BC3 RID: 15299 RVA: 0x000CD705 File Offset: 0x000CB905
	public Vector2Int MinExtents
	{
		get
		{
			return this.GridCoordinates;
		}
	}

	// Token: 0x170014E3 RID: 5347
	// (get) Token: 0x06003BC4 RID: 15300 RVA: 0x000CD70D File Offset: 0x000CB90D
	public Vector2Int MaxExtents
	{
		get
		{
			return this.GridCoordinates + this.Size;
		}
	}

	// Token: 0x170014E4 RID: 5348
	// (get) Token: 0x06003BC5 RID: 15301 RVA: 0x000CD720 File Offset: 0x000CB920
	// (set) Token: 0x06003BC6 RID: 15302 RVA: 0x000CD728 File Offset: 0x000CB928
	public RoomMetaData RoomMetaData { get; private set; }

	// Token: 0x170014E5 RID: 5349
	// (get) Token: 0x06003BC7 RID: 15303 RVA: 0x000CD731 File Offset: 0x000CB931
	public int RoomNumber { get; }

	// Token: 0x170014E6 RID: 5350
	// (get) Token: 0x06003BC8 RID: 15304 RVA: 0x000CD739 File Offset: 0x000CB939
	// (set) Token: 0x06003BC9 RID: 15305 RVA: 0x000CD741 File Offset: 0x000CB941
	public RoomType RoomType { get; set; }

	// Token: 0x170014E7 RID: 5351
	// (get) Token: 0x06003BCA RID: 15306 RVA: 0x000CD74A File Offset: 0x000CB94A
	public Vector2Int Size { get; }

	// Token: 0x170014E8 RID: 5352
	// (get) Token: 0x06003BCB RID: 15307 RVA: 0x000CD752 File Offset: 0x000CB952
	// (set) Token: 0x06003BCC RID: 15308 RVA: 0x000CD75A File Offset: 0x000CB95A
	public List<GridPointManager> MergeWithGridPointManagers
	{
		get
		{
			return this.m_mergeWithGridPointManagers;
		}
		private set
		{
			this.m_mergeWithGridPointManagers = value;
		}
	}

	// Token: 0x170014E9 RID: 5353
	// (get) Token: 0x06003BCD RID: 15309 RVA: 0x000CD763 File Offset: 0x000CB963
	// (set) Token: 0x06003BCE RID: 15310 RVA: 0x000CD76B File Offset: 0x000CB96B
	public int BiomeControllerIndex
	{
		get
		{
			return this.m_biomeControllerIndex;
		}
		private set
		{
			this.m_biomeControllerIndex = value;
		}
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x000CD774 File Offset: 0x000CB974
	public GridPointManager(int roomNumber, Vector2Int coordinates, BiomeType biome, RoomType roomType, RoomMetaData roomMetaData, bool isRoomMirrored, bool isTunnelDestination = false)
	{
		this.RoomNumber = roomNumber;
		this.GridCoordinates = coordinates;
		this.Biome = biome;
		this.RoomType = roomType;
		this.RoomMetaData = roomMetaData;
		this.IsRoomMirrored = isRoomMirrored;
		this.IsTunnelDestination = isTunnelDestination;
		if (roomMetaData != null)
		{
			if (roomMetaData.BiomeOverride == BiomeType.Lineage)
			{
				int num = Mathf.Min(SaveManager.LineageSaveData.LineageHeirList.Count, 20) - 4;
				num = Mathf.Max(0, Mathf.CeilToInt((float)num / 3f));
				int x = 2 + num;
				this.Size = new Vector2Int(x, 1);
			}
			else
			{
				this.Size = roomMetaData.Size;
			}
		}
		this.CreateGridPoints();
		if (this.GridPointList != null && this.GridPointList.Count > 0)
		{
			Bounds bounds = default(Bounds);
			bounds.center = this.GridPointList[0].Bounds.center;
			for (int i = 0; i < this.GridPointList.Count; i++)
			{
				bounds.Encapsulate(GridController.GetGridPointBounds(this.GridPointList[i].GridCoordinates));
			}
			this.Bounds = bounds;
		}
		this.AppearanceOverride = roomMetaData.BiomeOverride;
	}

	// Token: 0x06003BD0 RID: 15312 RVA: 0x000CD8F8 File Offset: 0x000CBAF8
	private void CreateGridPoints()
	{
		if (this.GridPointList == null)
		{
			this.GridPointList = new List<GridPoint>();
		}
		if (this.GridPoints == null)
		{
			this.GridPoints = new GridPoint[this.Size.x, this.Size.y];
		}
		for (int i = 0; i < this.Size.x; i++)
		{
			bool flag = i == 0;
			bool flag2 = i == this.Size.x - 1;
			for (int j = 0; j < this.Size.y; j++)
			{
				List<DoorLocation> list = new List<DoorLocation>();
				bool flag3 = j == this.Size.y - 1;
				bool flag4 = j == 0;
				int doorNumber = this.Size.y - 1 - j;
				if (flag && this.GetDoorLocation(RoomSide.Left, doorNumber) != DoorLocation.Empty)
				{
					list.Add(new DoorLocation(RoomSide.Left, doorNumber));
				}
				if (flag2 && this.GetDoorLocation(RoomSide.Right, doorNumber) != DoorLocation.Empty)
				{
					list.Add(new DoorLocation(RoomSide.Right, doorNumber));
				}
				doorNumber = i;
				if (flag3 && this.GetDoorLocation(RoomSide.Top, doorNumber) != DoorLocation.Empty)
				{
					list.Add(new DoorLocation(RoomSide.Top, doorNumber));
				}
				if (flag4 && this.GetDoorLocation(RoomSide.Bottom, doorNumber) != DoorLocation.Empty)
				{
					list.Add(new DoorLocation(RoomSide.Bottom, doorNumber));
				}
				this.CreateGridPoint(i, j, list);
			}
		}
	}

	// Token: 0x06003BD1 RID: 15313 RVA: 0x000CDA84 File Offset: 0x000CBC84
	private void CreateGridPoint(int xCoord, int yCoord, List<DoorLocation> doorLocations)
	{
		GridPoint gridPoint = this.GridPoints[xCoord, yCoord];
		if (gridPoint == null)
		{
			gridPoint = new GridPoint(this, new Vector2Int(xCoord, yCoord), doorLocations);
			this.GridPoints[xCoord, yCoord] = gridPoint;
			this.GridPointList.Add(gridPoint);
			if (GridController.IsInstantiated && !this.IsTunnelDestination)
			{
				GridController.AddRoomToGrid(gridPoint);
				return;
			}
		}
		else
		{
			gridPoint.SetDoorLocations(doorLocations);
		}
	}

	// Token: 0x06003BD2 RID: 15314 RVA: 0x000CDAE8 File Offset: 0x000CBCE8
	public void SetBiomeControllerIndex(int index)
	{
		this.BiomeControllerIndex = index;
	}

	// Token: 0x06003BD3 RID: 15315 RVA: 0x000CDAF1 File Offset: 0x000CBCF1
	public void SetAppearanceOverride(BiomeType biome)
	{
		this.AppearanceOverride = biome;
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x000CDAFC File Offset: 0x000CBCFC
	public List<DoorLocation> GetDoorLocationsOnSide(RoomSide side)
	{
		if (side == RoomSide.Any)
		{
			return this.DoorLocations;
		}
		List<DoorLocation> list = new List<DoorLocation>();
		foreach (DoorLocation item in this.DoorLocations)
		{
			if (item.RoomSide == side)
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x000CDB6C File Offset: 0x000CBD6C
	public void SetMergeWithRooms(List<GridPointManager> roomsToMerge)
	{
		this.MergeWithGridPointManagers = new List<GridPointManager>(roomsToMerge);
		if (this.MergeWithGridPointManagers.Contains(this))
		{
			this.MergeWithGridPointManagers.Remove(this);
		}
	}

	// Token: 0x06003BD6 RID: 15318 RVA: 0x000CDB98 File Offset: 0x000CBD98
	public GridPointManager GetConnectedRoom(DoorLocation doorLocation)
	{
		GridPoint gridPoint = GridController.GetGridPoint(this.GetDoorLeadsToGridCoordinates(doorLocation));
		GridPointManager result = null;
		if (gridPoint != null)
		{
			RoomSide oppositeSide = RoomUtility.GetOppositeSide(doorLocation.RoomSide);
			if (gridPoint.GetDoorLocation(oppositeSide) != DoorLocation.Empty)
			{
				result = gridPoint.Owner;
			}
		}
		return result;
	}

	// Token: 0x06003BD7 RID: 15319 RVA: 0x000CDBE0 File Offset: 0x000CBDE0
	public DoorLocation GetDoorLocation(RoomSide side, int doorNumber)
	{
		DoorLocation empty = DoorLocation.Empty;
		bool hasDoor;
		if (this.IsRoomMirrored)
		{
			hasDoor = this.RoomMetaData.GetHasDoor(new DoorLocation(side, doorNumber), true);
		}
		else
		{
			hasDoor = this.RoomMetaData.GetHasDoor(new DoorLocation(side, doorNumber), false);
		}
		if (hasDoor)
		{
			empty = new DoorLocation(side, doorNumber);
		}
		return empty;
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x000CDC34 File Offset: 0x000CBE34
	public Vector2Int GetDoorLeadsToGridCoordinates(DoorLocation location)
	{
		return GridController.GetDoorLeadsToGridCoordinates(this.GridCoordinates, this.Size, location);
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x000CDC48 File Offset: 0x000CBE48
	public void SetRoomMetaData(RoomMetaData roomMetaData, bool isRoomMirrored)
	{
		if (!roomMetaData.IsSpecialRoom && this.RoomType != RoomType.Standard && this.RoomType != RoomType.Trap)
		{
			this.RoomType = RoomType.Standard;
		}
		this.RoomMetaData = roomMetaData;
		this.IsRoomMirrored = isRoomMirrored;
		this.CreateGridPoints();
	}

	// Token: 0x06003BDA RID: 15322 RVA: 0x000CDC80 File Offset: 0x000CBE80
	public void SetRoomSeed(int seed)
	{
		this.RoomSeed = seed;
		RNGManager.SetSeed(RngID.Room_RNGSeed_Generator, this.RoomSeed);
		this.PropSeed = RNGManager.GetRandomNumber(RngID.Room_RNGSeed_Generator, "GridPointManager.SetRoomSeed()", 0, int.MaxValue);
		this.DecoSeed = RNGManager.GetRandomNumber(RngID.Room_RNGSeed_Generator, "GridPointManager.SetRoomSeed()", 0, int.MaxValue);
		this.EnemySeed = RNGManager.GetRandomNumber(RngID.Room_RNGSeed_Generator, "GridPointManager.SetRoomSeed()", 0, int.MaxValue);
		this.ChestSeed = RNGManager.GetRandomNumber(RngID.Room_RNGSeed_Generator, "GridPointManager.SetRoomSeed()", 0, int.MaxValue);
		this.HazardSeed = RNGManager.GetRandomNumber(RngID.Room_RNGSeed_Generator, "GridPointManager.SetRoomSeed()", 0, int.MaxValue);
		this.SpecialPropSeed = RNGManager.GetRandomNumber(RngID.Room_RNGSeed_Generator, "GridPointManager.SetRoomSeed()", 0, int.MaxValue);
		this.TunnelRoomSeed = RNGManager.GetRandomNumber(RngID.Room_RNGSeed_Generator, "GridPointManager.SetRoomSeed()", 0, int.MaxValue);
	}

	// Token: 0x06003BDB RID: 15323 RVA: 0x000CDD4C File Offset: 0x000CBF4C
	public GridPoint GetGridPoint(DoorLocation doorLocation)
	{
		for (int i = 0; i < this.GridPointList.Count; i++)
		{
			if (this.GridPointList[i].GetContainsDoorLocation(doorLocation))
			{
				return this.GridPointList[i];
			}
		}
		return null;
	}

	// Token: 0x04002D1F RID: 11551
	private List<GridPointManager> m_mergeWithGridPointManagers = new List<GridPointManager>();

	// Token: 0x04002D20 RID: 11552
	private int m_biomeControllerIndex = -1;

	// Token: 0x04002D21 RID: 11553
	private BiomeType m_appearanceOverride;
}
