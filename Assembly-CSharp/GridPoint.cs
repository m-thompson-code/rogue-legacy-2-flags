using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

// Token: 0x02000676 RID: 1654
public class GridPoint
{
	// Token: 0x170014C2 RID: 5314
	// (get) Token: 0x06003B8E RID: 15246 RVA: 0x000CD35B File Offset: 0x000CB55B
	public BiomeType Biome
	{
		get
		{
			return this.Owner.Biome;
		}
	}

	// Token: 0x170014C3 RID: 5315
	// (get) Token: 0x06003B8F RID: 15247 RVA: 0x000CD368 File Offset: 0x000CB568
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

	// Token: 0x170014C4 RID: 5316
	// (get) Token: 0x06003B90 RID: 15248 RVA: 0x000CD3AA File Offset: 0x000CB5AA
	// (set) Token: 0x06003B91 RID: 15249 RVA: 0x000CD3B2 File Offset: 0x000CB5B2
	public List<DoorLocation> Doors { get; private set; }

	// Token: 0x170014C5 RID: 5317
	// (get) Token: 0x06003B92 RID: 15250 RVA: 0x000CD3BB File Offset: 0x000CB5BB
	public Vector2Int GridCoordinates
	{
		get
		{
			return this.Owner.GridCoordinates + this.LocalCoords;
		}
	}

	// Token: 0x170014C6 RID: 5318
	// (get) Token: 0x06003B93 RID: 15251 RVA: 0x000CD3D3 File Offset: 0x000CB5D3
	public bool IsMirrored
	{
		get
		{
			return this.Owner.IsRoomMirrored;
		}
	}

	// Token: 0x170014C7 RID: 5319
	// (get) Token: 0x06003B94 RID: 15252 RVA: 0x000CD3E0 File Offset: 0x000CB5E0
	public Vector2Int LocalCoords { get; }

	// Token: 0x170014C8 RID: 5320
	// (get) Token: 0x06003B95 RID: 15253 RVA: 0x000CD3E8 File Offset: 0x000CB5E8
	public GridPointManager Owner { get; }

	// Token: 0x170014C9 RID: 5321
	// (get) Token: 0x06003B96 RID: 15254 RVA: 0x000CD3F0 File Offset: 0x000CB5F0
	public RoomMetaData RoomMetaData
	{
		get
		{
			return this.Owner.RoomMetaData;
		}
	}

	// Token: 0x170014CA RID: 5322
	// (get) Token: 0x06003B97 RID: 15255 RVA: 0x000CD3FD File Offset: 0x000CB5FD
	public int RoomNumber
	{
		get
		{
			return this.Owner.RoomNumber;
		}
	}

	// Token: 0x170014CB RID: 5323
	// (get) Token: 0x06003B98 RID: 15256 RVA: 0x000CD40A File Offset: 0x000CB60A
	public RoomType RoomType
	{
		get
		{
			return this.Owner.RoomType;
		}
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x000CD417 File Offset: 0x000CB617
	public GridPoint(GridPointManager owner, Vector2Int localCoords, List<DoorLocation> doors)
	{
		this.Owner = owner;
		this.LocalCoords = localCoords;
		this.Doors = doors;
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x000CD434 File Offset: 0x000CB634
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

	// Token: 0x06003B9B RID: 15259 RVA: 0x000CD480 File Offset: 0x000CB680
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

	// Token: 0x06003B9C RID: 15260 RVA: 0x000CD4BA File Offset: 0x000CB6BA
	public void SetDoorLocations(List<DoorLocation> doorLocations)
	{
		this.Doors = doorLocations;
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x000CD4C4 File Offset: 0x000CB6C4
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

	// Token: 0x06003B9E RID: 15262 RVA: 0x000CD50C File Offset: 0x000CB70C
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

	// Token: 0x04002D17 RID: 11543
	private Bounds m_bounds;
}
