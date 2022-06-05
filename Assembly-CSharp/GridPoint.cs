using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

// Token: 0x02000AED RID: 2797
public class GridPoint
{
	// Token: 0x17001C7E RID: 7294
	// (get) Token: 0x060053CB RID: 21451 RVA: 0x0002D768 File Offset: 0x0002B968
	public BiomeType Biome
	{
		get
		{
			return this.Owner.Biome;
		}
	}

	// Token: 0x17001C7F RID: 7295
	// (get) Token: 0x060053CC RID: 21452 RVA: 0x0013D278 File Offset: 0x0013B478
	public Bounds Bounds
	{
		get
		{
			if (this.Owner != null && this.m_bounds == default(Bounds))
			{
				this.m_bounds = GridController.GetGridPointBounds(this.GridCoordinates);
			}
			return this.m_bounds;
		}
	}

	// Token: 0x17001C80 RID: 7296
	// (get) Token: 0x060053CD RID: 21453 RVA: 0x0002D775 File Offset: 0x0002B975
	// (set) Token: 0x060053CE RID: 21454 RVA: 0x0002D77D File Offset: 0x0002B97D
	public List<DoorLocation> Doors { get; private set; }

	// Token: 0x17001C81 RID: 7297
	// (get) Token: 0x060053CF RID: 21455 RVA: 0x0002D786 File Offset: 0x0002B986
	public Vector2Int GridCoordinates
	{
		get
		{
			return this.Owner.GridCoordinates + this.LocalCoords;
		}
	}

	// Token: 0x17001C82 RID: 7298
	// (get) Token: 0x060053D0 RID: 21456 RVA: 0x0002D79E File Offset: 0x0002B99E
	public bool IsMirrored
	{
		get
		{
			return this.Owner.IsRoomMirrored;
		}
	}

	// Token: 0x17001C83 RID: 7299
	// (get) Token: 0x060053D1 RID: 21457 RVA: 0x0002D7AB File Offset: 0x0002B9AB
	public Vector2Int LocalCoords { get; }

	// Token: 0x17001C84 RID: 7300
	// (get) Token: 0x060053D2 RID: 21458 RVA: 0x0002D7B3 File Offset: 0x0002B9B3
	public GridPointManager Owner { get; }

	// Token: 0x17001C85 RID: 7301
	// (get) Token: 0x060053D3 RID: 21459 RVA: 0x0002D7BB File Offset: 0x0002B9BB
	public RoomMetaData RoomMetaData
	{
		get
		{
			return this.Owner.RoomMetaData;
		}
	}

	// Token: 0x17001C86 RID: 7302
	// (get) Token: 0x060053D4 RID: 21460 RVA: 0x0002D7C8 File Offset: 0x0002B9C8
	public int RoomNumber
	{
		get
		{
			return this.Owner.RoomNumber;
		}
	}

	// Token: 0x17001C87 RID: 7303
	// (get) Token: 0x060053D5 RID: 21461 RVA: 0x0002D7D5 File Offset: 0x0002B9D5
	public RoomType RoomType
	{
		get
		{
			return this.Owner.RoomType;
		}
	}

	// Token: 0x060053D6 RID: 21462 RVA: 0x0002D7E2 File Offset: 0x0002B9E2
	public GridPoint(GridPointManager owner, Vector2Int localCoords, List<DoorLocation> doors)
	{
		this.Owner = owner;
		this.LocalCoords = localCoords;
		this.Doors = doors;
	}

	// Token: 0x060053D7 RID: 21463 RVA: 0x0013D2BC File Offset: 0x0013B4BC
	public DoorLocation GetDoorLocation(RoomSide side)
	{
		for (int i = 0; i < this.Doors.Count; i++)
		{
			if (this.Doors[i].RoomSide == side)
			{
				return this.Doors[i];
			}
		}
		return DoorLocation.Empty;
	}

	// Token: 0x060053D8 RID: 21464 RVA: 0x0013D308 File Offset: 0x0013B508
	public bool GetContainsDoorLocation(DoorLocation doorLocation)
	{
		for (int i = 0; i < this.Doors.Count; i++)
		{
			if (this.Doors[i] == doorLocation)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060053D9 RID: 21465 RVA: 0x0002D7FF File Offset: 0x0002B9FF
	public void SetDoorLocations(List<DoorLocation> doorLocations)
	{
		this.Doors = doorLocations;
	}

	// Token: 0x060053DA RID: 21466 RVA: 0x0013D344 File Offset: 0x0013B544
	public GridPoint GetConnectedGridPoint(RoomSide side)
	{
		if (this.GetDoorLocation(side) != DoorLocation.Empty)
		{
			GridPoint gridPointOnSide = this.GetGridPointOnSide(side);
			if (gridPointOnSide != null)
			{
				RoomSide oppositeSide = RoomUtility.GetOppositeSide(side);
				if (gridPointOnSide.GetDoorLocation(oppositeSide) != DoorLocation.Empty)
				{
					return gridPointOnSide;
				}
			}
		}
		return null;
	}

	// Token: 0x060053DB RID: 21467 RVA: 0x0013D38C File Offset: 0x0013B58C
	private GridPoint GetGridPointOnSide(RoomSide side)
	{
		Vector2Int zero = Vector2Int.zero;
		switch (side)
		{
		case RoomSide.Top:
			zero.y = 1;
			break;
		case RoomSide.Bottom:
			zero.y = -1;
			break;
		case RoomSide.Left:
			zero.x = -1;
			break;
		case RoomSide.Right:
			zero.x = 1;
			break;
		}
		return GridController.GetGridPoint(this.GridCoordinates + zero);
	}

	// Token: 0x04003EA7 RID: 16039
	private Bounds m_bounds;
}
