using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

// Token: 0x02000AEF RID: 2799
public class GridPointManager
{
	// Token: 0x17001C8C RID: 7308
	// (get) Token: 0x060053E1 RID: 21473 RVA: 0x0002D847 File Offset: 0x0002BA47
	// (set) Token: 0x060053E2 RID: 21474 RVA: 0x0002D84F File Offset: 0x0002BA4F
	public int RoomSeed { get; private set; } = -1;

	// Token: 0x17001C8D RID: 7309
	// (get) Token: 0x060053E3 RID: 21475 RVA: 0x0002D858 File Offset: 0x0002BA58
	// (set) Token: 0x060053E4 RID: 21476 RVA: 0x0002D860 File Offset: 0x0002BA60
	public int TunnelRoomSeed { get; private set; } = -1;

	// Token: 0x17001C8E RID: 7310
	// (get) Token: 0x060053E5 RID: 21477 RVA: 0x0002D869 File Offset: 0x0002BA69
	// (set) Token: 0x060053E6 RID: 21478 RVA: 0x0002D871 File Offset: 0x0002BA71
	public int PropSeed { get; private set; } = -1;

	// Token: 0x17001C8F RID: 7311
	// (get) Token: 0x060053E7 RID: 21479 RVA: 0x0002D87A File Offset: 0x0002BA7A
	// (set) Token: 0x060053E8 RID: 21480 RVA: 0x0002D882 File Offset: 0x0002BA82
	public int DecoSeed { get; private set; } = -1;

	// Token: 0x17001C90 RID: 7312
	// (get) Token: 0x060053E9 RID: 21481 RVA: 0x0002D88B File Offset: 0x0002BA8B
	// (set) Token: 0x060053EA RID: 21482 RVA: 0x0002D893 File Offset: 0x0002BA93
	public int EnemySeed { get; private set; } = -1;

	// Token: 0x17001C91 RID: 7313
	// (get) Token: 0x060053EB RID: 21483 RVA: 0x0002D89C File Offset: 0x0002BA9C
	// (set) Token: 0x060053EC RID: 21484 RVA: 0x0002D8A4 File Offset: 0x0002BAA4
	public int ChestSeed { get; private set; } = -1;

	// Token: 0x17001C92 RID: 7314
	// (get) Token: 0x060053ED RID: 21485 RVA: 0x0002D8AD File Offset: 0x0002BAAD
	// (set) Token: 0x060053EE RID: 21486 RVA: 0x0002D8B5 File Offset: 0x0002BAB5
	public int HazardSeed { get; private set; } = -1;

	// Token: 0x17001C93 RID: 7315
	// (get) Token: 0x060053EF RID: 21487 RVA: 0x0002D8BE File Offset: 0x0002BABE
	// (set) Token: 0x060053F0 RID: 21488 RVA: 0x0002D8C6 File Offset: 0x0002BAC6
	public int SpecialPropSeed { get; private set; } = -1;

	// Token: 0x17001C94 RID: 7316
	// (get) Token: 0x060053F1 RID: 21489 RVA: 0x0002D8CF File Offset: 0x0002BACF
	public BiomeType Biome { get; }

	// Token: 0x17001C95 RID: 7317
	// (get) Token: 0x060053F2 RID: 21490 RVA: 0x0002D8D7 File Offset: 0x0002BAD7
	// (set) Token: 0x060053F3 RID: 21491 RVA: 0x0002D8DF File Offset: 0x0002BADF
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

	// Token: 0x17001C96 RID: 7318
	// (get) Token: 0x060053F4 RID: 21492 RVA: 0x0002D8E8 File Offset: 0x0002BAE8
	public Bounds Bounds { get; }

	// Token: 0x17001C97 RID: 7319
	// (get) Token: 0x060053F5 RID: 21493 RVA: 0x0002D8F0 File Offset: 0x0002BAF0
	// (set) Token: 0x060053F6 RID: 21494 RVA: 0x0002D8F8 File Offset: 0x0002BAF8
	public GridPointManagerContentEntry[] Content { get; set; }

	// Token: 0x17001C98 RID: 7320
	// (get) Token: 0x060053F7 RID: 21495 RVA: 0x0013D3F0 File Offset: 0x0013B5F0
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

	// Token: 0x17001C99 RID: 7321
	// (get) Token: 0x060053F8 RID: 21496 RVA: 0x0002D901 File Offset: 0x0002BB01
	public Vector2Int GridCoordinates { get; }

	// Token: 0x17001C9A RID: 7322
	// (get) Token: 0x060053F9 RID: 21497 RVA: 0x0002D909 File Offset: 0x0002BB09
	// (set) Token: 0x060053FA RID: 21498 RVA: 0x0002D911 File Offset: 0x0002BB11
	public GridPoint[,] GridPoints { get; private set; }

	// Token: 0x17001C9B RID: 7323
	// (get) Token: 0x060053FB RID: 21499 RVA: 0x0002D91A File Offset: 0x0002BB1A
	// (set) Token: 0x060053FC RID: 21500 RVA: 0x0002D922 File Offset: 0x0002BB22
	public List<GridPoint> GridPointList { get; private set; }

	// Token: 0x17001C9C RID: 7324
	// (get) Token: 0x060053FD RID: 21501 RVA: 0x0002D92B File Offset: 0x0002BB2B
	// (set) Token: 0x060053FE RID: 21502 RVA: 0x0002D933 File Offset: 0x0002BB33
	public bool IsRoomMirrored { get; private set; }

	// Token: 0x17001C9D RID: 7325
	// (get) Token: 0x060053FF RID: 21503 RVA: 0x0002D93C File Offset: 0x0002BB3C
	public bool IsTunnelDestination { get; }

	// Token: 0x17001C9E RID: 7326
	// (get) Token: 0x06005400 RID: 21504 RVA: 0x0002D944 File Offset: 0x0002BB44
	public Vector2Int MinExtents
	{
		get
		{
			return this.GridCoordinates;
		}
	}

	// Token: 0x17001C9F RID: 7327
	// (get) Token: 0x06005401 RID: 21505 RVA: 0x0002D94C File Offset: 0x0002BB4C
	public Vector2Int MaxExtents
	{
		get
		{
			return this.GridCoordinates + this.Size;
		}
	}

	// Token: 0x17001CA0 RID: 7328
	// (get) Token: 0x06005402 RID: 21506 RVA: 0x0002D95F File Offset: 0x0002BB5F
	// (set) Token: 0x06005403 RID: 21507 RVA: 0x0002D967 File Offset: 0x0002BB67
	public RoomMetaData RoomMetaData { get; private set; }

	// Token: 0x17001CA1 RID: 7329
	// (get) Token: 0x06005404 RID: 21508 RVA: 0x0002D970 File Offset: 0x0002BB70
	public int RoomNumber { get; }

	// Token: 0x17001CA2 RID: 7330
	// (get) Token: 0x06005405 RID: 21509 RVA: 0x0002D978 File Offset: 0x0002BB78
	// (set) Token: 0x06005406 RID: 21510 RVA: 0x0002D980 File Offset: 0x0002BB80
	public RoomType RoomType { get; set; }

	// Token: 0x17001CA3 RID: 7331
	// (get) Token: 0x06005407 RID: 21511 RVA: 0x0002D989 File Offset: 0x0002BB89
	public Vector2Int Size { get; }

	// Token: 0x17001CA4 RID: 7332
	// (get) Token: 0x06005408 RID: 21512 RVA: 0x0002D991 File Offset: 0x0002BB91
	// (set) Token: 0x06005409 RID: 21513 RVA: 0x0002D999 File Offset: 0x0002BB99
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

	// Token: 0x17001CA5 RID: 7333
	// (get) Token: 0x0600540A RID: 21514 RVA: 0x0002D9A2 File Offset: 0x0002BBA2
	// (set) Token: 0x0600540B RID: 21515 RVA: 0x0002D9AA File Offset: 0x0002BBAA
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

	// Token: 0x0600540C RID: 21516 RVA: 0x0013D44C File Offset: 0x0013B64C
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

	// Token: 0x0600540D RID: 21517 RVA: 0x0013D5D0 File Offset: 0x0013B7D0
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

	// Token: 0x0600540E RID: 21518 RVA: 0x0013D75C File Offset: 0x0013B95C
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

	// Token: 0x0600540F RID: 21519 RVA: 0x0002D9B3 File Offset: 0x0002BBB3
	public void SetBiomeControllerIndex(int index)
	{
		this.BiomeControllerIndex = index;
	}

	// Token: 0x06005410 RID: 21520 RVA: 0x0002D9BC File Offset: 0x0002BBBC
	public void SetAppearanceOverride(BiomeType biome)
	{
		this.AppearanceOverride = biome;
	}

	// Token: 0x06005411 RID: 21521 RVA: 0x0013D7C0 File Offset: 0x0013B9C0
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

	// Token: 0x06005412 RID: 21522 RVA: 0x0002D9C5 File Offset: 0x0002BBC5
	public void SetMergeWithRooms(List<GridPointManager> roomsToMerge)
	{
		this.MergeWithGridPointManagers = new List<GridPointManager>(roomsToMerge);
		if (this.MergeWithGridPointManagers.Contains(this))
		{
			this.MergeWithGridPointManagers.Remove(this);
		}
	}

	// Token: 0x06005413 RID: 21523 RVA: 0x0013D830 File Offset: 0x0013BA30
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

	// Token: 0x06005414 RID: 21524 RVA: 0x0013D878 File Offset: 0x0013BA78
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

	// Token: 0x06005415 RID: 21525 RVA: 0x0002D9EE File Offset: 0x0002BBEE
	public Vector2Int GetDoorLeadsToGridCoordinates(DoorLocation location)
	{
		return GridController.GetDoorLeadsToGridCoordinates(this.GridCoordinates, this.Size, location);
	}

	// Token: 0x06005416 RID: 21526 RVA: 0x0002DA02 File Offset: 0x0002BC02
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

	// Token: 0x06005417 RID: 21527 RVA: 0x0013D8CC File Offset: 0x0013BACC
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

	// Token: 0x06005418 RID: 21528 RVA: 0x0013D998 File Offset: 0x0013BB98
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

	// Token: 0x04003EAF RID: 16047
	private List<GridPointManager> m_mergeWithGridPointManagers = new List<GridPointManager>();

	// Token: 0x04003EB0 RID: 16048
	private int m_biomeControllerIndex = -1;

	// Token: 0x04003EB1 RID: 16049
	private BiomeType m_appearanceOverride;
}
