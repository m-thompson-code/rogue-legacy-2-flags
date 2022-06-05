using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

// Token: 0x02000652 RID: 1618
public class BiomeGridPointManager
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06003AA6 RID: 15014 RVA: 0x000C8D84 File Offset: 0x000C6F84
	// (remove) Token: 0x06003AA7 RID: 15015 RVA: 0x000C8DBC File Offset: 0x000C6FBC
	public event EventHandler<GridPointManager> RoomBuiltEvent;

	// Token: 0x170014A6 RID: 5286
	// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x000C8DF1 File Offset: 0x000C6FF1
	// (set) Token: 0x06003AA9 RID: 15017 RVA: 0x000C8DF9 File Offset: 0x000C6FF9
	public Bounds Border { get; set; }

	// Token: 0x170014A7 RID: 5287
	// (get) Token: 0x06003AAA RID: 15018 RVA: 0x000C8E02 File Offset: 0x000C7002
	// (set) Token: 0x06003AAB RID: 15019 RVA: 0x000C8E0A File Offset: 0x000C700A
	public Vector2Int Center { get; private set; }

	// Token: 0x170014A8 RID: 5288
	// (get) Token: 0x06003AAC RID: 15020 RVA: 0x000C8E13 File Offset: 0x000C7013
	// (set) Token: 0x06003AAD RID: 15021 RVA: 0x000C8E1B File Offset: 0x000C701B
	private Dictionary<RoomSide, GridPoint> ConnectionPoints
	{
		get
		{
			return this.m_connectionPoints;
		}
		set
		{
			this.m_connectionPoints = value;
		}
	}

	// Token: 0x170014A9 RID: 5289
	// (get) Token: 0x06003AAE RID: 15022 RVA: 0x000C8E24 File Offset: 0x000C7024
	// (set) Token: 0x06003AAF RID: 15023 RVA: 0x000C8E2C File Offset: 0x000C702C
	public Dictionary<RoomSide, int> Extents { get; private set; }

	// Token: 0x170014AA RID: 5290
	// (get) Token: 0x06003AB0 RID: 15024 RVA: 0x000C8E38 File Offset: 0x000C7038
	public List<GridPoint> GridPoints
	{
		get
		{
			List<GridPoint> list = new List<GridPoint>();
			if (this.GridPointManagers != null)
			{
				for (int i = 0; i < this.GridPointManagers.Count; i++)
				{
					list.AddRange(this.GridPointManagers[i].GridPointList);
				}
			}
			return list;
		}
	}

	// Token: 0x170014AB RID: 5291
	// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x000C8E81 File Offset: 0x000C7081
	// (set) Token: 0x06003AB2 RID: 15026 RVA: 0x000C8E89 File Offset: 0x000C7089
	public List<GridPointManager> GridPointManagers { get; private set; }

	// Token: 0x170014AC RID: 5292
	// (get) Token: 0x06003AB3 RID: 15027 RVA: 0x000C8E92 File Offset: 0x000C7092
	// (set) Token: 0x06003AB4 RID: 15028 RVA: 0x000C8E9A File Offset: 0x000C709A
	public int StandardRoomCount { get; private set; }

	// Token: 0x170014AD RID: 5293
	// (get) Token: 0x06003AB5 RID: 15029 RVA: 0x000C8EA3 File Offset: 0x000C70A3
	// (set) Token: 0x06003AB6 RID: 15030 RVA: 0x000C8EAB File Offset: 0x000C70AB
	public int SpecialRoomCount { get; private set; }

	// Token: 0x170014AE RID: 5294
	// (get) Token: 0x06003AB7 RID: 15031 RVA: 0x000C8EB4 File Offset: 0x000C70B4
	// (set) Token: 0x06003AB8 RID: 15032 RVA: 0x000C8EBC File Offset: 0x000C70BC
	public List<GridPointManager> TunnelDestinationGridPointManagers
	{
		get
		{
			return this.m_tunnelDestinationGridPointManagers;
		}
		private set
		{
			this.m_tunnelDestinationGridPointManagers = value;
		}
	}

	// Token: 0x170014AF RID: 5295
	// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x000C8EC5 File Offset: 0x000C70C5
	// (set) Token: 0x06003ABA RID: 15034 RVA: 0x000C8ECD File Offset: 0x000C70CD
	public int StandaloneRoomCount { get; private set; }

	// Token: 0x170014B0 RID: 5296
	// (get) Token: 0x06003ABB RID: 15035 RVA: 0x000C8ED6 File Offset: 0x000C70D6
	// (set) Token: 0x06003ABC RID: 15036 RVA: 0x000C8EDE File Offset: 0x000C70DE
	public int MergeRoomCount { get; private set; }

	// Token: 0x170014B1 RID: 5297
	// (get) Token: 0x06003ABD RID: 15037 RVA: 0x000C8EE7 File Offset: 0x000C70E7
	// (set) Token: 0x06003ABE RID: 15038 RVA: 0x000C8EEF File Offset: 0x000C70EF
	private Dictionary<RoomSide, List<DoorLocation>> ValidDoorLocations { get; set; }

	// Token: 0x170014B2 RID: 5298
	// (get) Token: 0x06003ABF RID: 15039 RVA: 0x000C8EF8 File Offset: 0x000C70F8
	// (set) Token: 0x06003AC0 RID: 15040 RVA: 0x000C8F00 File Offset: 0x000C7100
	public int TunnelDestinationCount { get; private set; }

	// Token: 0x06003AC1 RID: 15041 RVA: 0x000C8F0C File Offset: 0x000C710C
	public BiomeGridPointManager(BiomeType biome)
	{
		this.m_biome = biome;
	}

	// Token: 0x06003AC2 RID: 15042 RVA: 0x000C8F5C File Offset: 0x000C715C
	public GridPointManager AddRoomToGrid(int roomNumber, RoomMetaData roomMetaData, bool isRoomMirrored, RoomType roomType, Vector2Int roomCoordinates, BiomeType biome, bool incrementRoomCount = true)
	{
		if (this.GridPointManagers == null)
		{
			this.GridPointManagers = new List<GridPointManager>();
		}
		if (roomType == RoomType.Transition)
		{
			this.Center = roomCoordinates;
			this.SetBorder(roomCoordinates);
		}
		else if (incrementRoomCount)
		{
			if (roomType == RoomType.Standard)
			{
				int num = this.StandardRoomCount;
				this.StandardRoomCount = num + 1;
			}
			else
			{
				int num = this.SpecialRoomCount;
				this.SpecialRoomCount = num + 1;
			}
		}
		GridPointManager gridPointManager = new GridPointManager(roomNumber, roomCoordinates, biome, roomType, roomMetaData, isRoomMirrored, false);
		gridPointManager.SetBiomeControllerIndex(this.GridPointManagers.Count);
		this.GridPointManagers.Add(gridPointManager);
		this.UpdateExtents(gridPointManager);
		if (this.RoomBuiltEvent != null)
		{
			this.RoomBuiltEvent(this, gridPointManager);
		}
		return gridPointManager;
	}

	// Token: 0x06003AC3 RID: 15043 RVA: 0x000C9008 File Offset: 0x000C7208
	public void AddTunnelDestinationGridPointManager(GridPointManager gridPointManager)
	{
		if (this.TunnelDestinationGridPointManagers == null)
		{
			this.TunnelDestinationGridPointManagers = new List<GridPointManager>();
		}
		this.TunnelDestinationGridPointManagers.Add(gridPointManager);
		int tunnelDestinationCount = this.TunnelDestinationCount;
		this.TunnelDestinationCount = tunnelDestinationCount + 1;
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x000C9044 File Offset: 0x000C7244
	public bool GetAreCoordinatesWithinBorder(Vector2Int coordinates, bool isStandardRoom)
	{
		if (isStandardRoom && this.m_biome != BiomeType.Stone && this.m_biome != BiomeType.Tower && this.m_biome != BiomeType.TowerExterior)
		{
			return (float)coordinates.x < this.Border.max.x && (float)coordinates.x > this.Border.min.x && (float)coordinates.y < this.Border.max.y && (float)coordinates.y > this.Border.min.y;
		}
		return (float)coordinates.x < this.Border.max.x && (float)coordinates.y < this.Border.max.y && this.Border.Contains(coordinates);
	}

	// Token: 0x06003AC5 RID: 15045 RVA: 0x000C914E File Offset: 0x000C734E
	public GridPoint GetConnectionPoint(RoomSide side)
	{
		return this.ConnectionPoints[side];
	}

	// Token: 0x06003AC6 RID: 15046 RVA: 0x000C915C File Offset: 0x000C735C
	public List<DoorLocation> GetValidDoorLocations(RoomSide side)
	{
		return this.ValidDoorLocations[side];
	}

	// Token: 0x06003AC7 RID: 15047 RVA: 0x000C916C File Offset: 0x000C736C
	private void SetBorder(Vector2Int transitionRoomGridCoordinates)
	{
		Dictionary<RoomSide, int> biomeBorderOffsets = BiomeCreation_EV.GetBiomeBorderOffsets(this.m_biome);
		Bounds border = default(Bounds);
		RoomMetaData transitionRoom = RoomLibrary.GetSetCollection(this.m_biome).TransitionRoom;
		int num = transitionRoomGridCoordinates.x + biomeBorderOffsets[RoomSide.Left];
		int num2 = transitionRoomGridCoordinates.y + biomeBorderOffsets[RoomSide.Bottom];
		int num3 = transitionRoomGridCoordinates.x + transitionRoom.Size.x + biomeBorderOffsets[RoomSide.Right];
		int num4 = transitionRoomGridCoordinates.y + transitionRoom.Size.y + biomeBorderOffsets[RoomSide.Top];
		border.SetMinMax(new Vector2((float)num, (float)num2), new Vector2((float)num3, (float)num4));
		this.Border = border;
	}

	// Token: 0x06003AC8 RID: 15048 RVA: 0x000C922E File Offset: 0x000C742E
	public void SetStandaloneRoomCount(int count)
	{
		this.StandaloneRoomCount = count;
	}

	// Token: 0x06003AC9 RID: 15049 RVA: 0x000C9237 File Offset: 0x000C7437
	public void SetMergeRoomCount(int count)
	{
		this.MergeRoomCount = count;
	}

	// Token: 0x06003ACA RID: 15050 RVA: 0x000C9240 File Offset: 0x000C7440
	public void SetConnectionPoint(RoomSide side, GridPoint gridPoint, List<int> validTransitionRoomDoorNumbers)
	{
		this.ConnectionPoints[side] = gridPoint;
		if (this.ValidDoorLocations == null)
		{
			this.ValidDoorLocations = new Dictionary<RoomSide, List<DoorLocation>>();
		}
		this.ValidDoorLocations.Add(side, new List<DoorLocation>());
		for (int i = 0; i < validTransitionRoomDoorNumbers.Count; i++)
		{
			this.ValidDoorLocations[side].Add(new DoorLocation(RoomUtility.GetOppositeSide(side), validTransitionRoomDoorNumbers[i]));
		}
	}

	// Token: 0x06003ACB RID: 15051 RVA: 0x000C92B4 File Offset: 0x000C74B4
	private void UpdateExtents(GridPointManager gridPointManager)
	{
		if (this.Extents == null)
		{
			this.Extents = new Dictionary<RoomSide, int>();
		}
		if (this.Extents.ContainsKey(RoomSide.Left))
		{
			if (gridPointManager.GridCoordinates.x < this.Extents[RoomSide.Left])
			{
				this.Extents[RoomSide.Left] = gridPointManager.GridCoordinates.x;
			}
		}
		else
		{
			this.Extents.Add(RoomSide.Left, gridPointManager.GridCoordinates.x);
		}
		if (this.Extents.ContainsKey(RoomSide.Right))
		{
			if (gridPointManager.GridCoordinates.x + gridPointManager.Size.x > this.Extents[RoomSide.Right])
			{
				this.Extents[RoomSide.Right] = gridPointManager.GridCoordinates.x + gridPointManager.Size.x;
			}
		}
		else
		{
			this.Extents.Add(RoomSide.Right, gridPointManager.GridCoordinates.x + gridPointManager.Size.x);
		}
		if (this.Extents.ContainsKey(RoomSide.Bottom))
		{
			if (gridPointManager.GridCoordinates.y < this.Extents[RoomSide.Bottom])
			{
				this.Extents[RoomSide.Bottom] = gridPointManager.GridCoordinates.y;
			}
		}
		else
		{
			this.Extents.Add(RoomSide.Bottom, gridPointManager.GridCoordinates.y);
		}
		if (this.Extents.ContainsKey(RoomSide.Top))
		{
			if (gridPointManager.GridCoordinates.y + gridPointManager.Size.y > this.Extents[RoomSide.Top])
			{
				this.Extents[RoomSide.Top] = gridPointManager.GridCoordinates.y + gridPointManager.Size.y;
				return;
			}
		}
		else
		{
			this.Extents.Add(RoomSide.Top, gridPointManager.GridCoordinates.y + gridPointManager.Size.y);
		}
	}

	// Token: 0x06003ACC RID: 15052 RVA: 0x000C94B0 File Offset: 0x000C76B0
	public void RemoveTunnelDestinations()
	{
		for (int i = this.GridPointManagers.Count - 1; i >= 0; i--)
		{
			if (this.GridPointManagers[i].IsTunnelDestination)
			{
				this.GridPointManagers.RemoveAt(i);
			}
		}
		if (this.TunnelDestinationGridPointManagers != null)
		{
			this.TunnelDestinationGridPointManagers.Clear();
		}
		this.TunnelDestinationCount = 0;
	}

	// Token: 0x06003ACD RID: 15053 RVA: 0x000C9510 File Offset: 0x000C7710
	public GridPointManager GetGridPointManager(int biomeControllerIndex)
	{
		GridPointManager result = null;
		if (this.GridPointManagers != null)
		{
			for (int i = 0; i < this.GridPointManagers.Count; i++)
			{
				if (this.GridPointManagers[i].BiomeControllerIndex == biomeControllerIndex)
				{
					result = this.GridPointManagers[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x04002CE1 RID: 11489
	private BiomeType m_biome;

	// Token: 0x04002CE2 RID: 11490
	private List<GridPointManager> m_tunnelDestinationGridPointManagers = new List<GridPointManager>();

	// Token: 0x04002CE3 RID: 11491
	private Dictionary<RoomSide, GridPoint> m_connectionPoints = new Dictionary<RoomSide, GridPoint>
	{
		{
			RoomSide.Left,
			null
		},
		{
			RoomSide.Right,
			null
		},
		{
			RoomSide.Top,
			null
		},
		{
			RoomSide.Bottom,
			null
		}
	};
}
