using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

// Token: 0x02000A9A RID: 2714
public class BiomeGridPointManager
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x060051F1 RID: 20977 RVA: 0x001374D4 File Offset: 0x001356D4
	// (remove) Token: 0x060051F2 RID: 20978 RVA: 0x0013750C File Offset: 0x0013570C
	public event EventHandler<GridPointManager> RoomBuiltEvent;

	// Token: 0x17001C19 RID: 7193
	// (get) Token: 0x060051F3 RID: 20979 RVA: 0x0002CB1C File Offset: 0x0002AD1C
	// (set) Token: 0x060051F4 RID: 20980 RVA: 0x0002CB24 File Offset: 0x0002AD24
	public Bounds Border { get; set; }

	// Token: 0x17001C1A RID: 7194
	// (get) Token: 0x060051F5 RID: 20981 RVA: 0x0002CB2D File Offset: 0x0002AD2D
	// (set) Token: 0x060051F6 RID: 20982 RVA: 0x0002CB35 File Offset: 0x0002AD35
	public Vector2Int Center { get; private set; }

	// Token: 0x17001C1B RID: 7195
	// (get) Token: 0x060051F7 RID: 20983 RVA: 0x0002CB3E File Offset: 0x0002AD3E
	// (set) Token: 0x060051F8 RID: 20984 RVA: 0x0002CB46 File Offset: 0x0002AD46
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

	// Token: 0x17001C1C RID: 7196
	// (get) Token: 0x060051F9 RID: 20985 RVA: 0x0002CB4F File Offset: 0x0002AD4F
	// (set) Token: 0x060051FA RID: 20986 RVA: 0x0002CB57 File Offset: 0x0002AD57
	public Dictionary<RoomSide, int> Extents { get; private set; }

	// Token: 0x17001C1D RID: 7197
	// (get) Token: 0x060051FB RID: 20987 RVA: 0x00137544 File Offset: 0x00135744
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

	// Token: 0x17001C1E RID: 7198
	// (get) Token: 0x060051FC RID: 20988 RVA: 0x0002CB60 File Offset: 0x0002AD60
	// (set) Token: 0x060051FD RID: 20989 RVA: 0x0002CB68 File Offset: 0x0002AD68
	public List<GridPointManager> GridPointManagers { get; private set; }

	// Token: 0x17001C1F RID: 7199
	// (get) Token: 0x060051FE RID: 20990 RVA: 0x0002CB71 File Offset: 0x0002AD71
	// (set) Token: 0x060051FF RID: 20991 RVA: 0x0002CB79 File Offset: 0x0002AD79
	public int StandardRoomCount { get; private set; }

	// Token: 0x17001C20 RID: 7200
	// (get) Token: 0x06005200 RID: 20992 RVA: 0x0002CB82 File Offset: 0x0002AD82
	// (set) Token: 0x06005201 RID: 20993 RVA: 0x0002CB8A File Offset: 0x0002AD8A
	public int SpecialRoomCount { get; private set; }

	// Token: 0x17001C21 RID: 7201
	// (get) Token: 0x06005202 RID: 20994 RVA: 0x0002CB93 File Offset: 0x0002AD93
	// (set) Token: 0x06005203 RID: 20995 RVA: 0x0002CB9B File Offset: 0x0002AD9B
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

	// Token: 0x17001C22 RID: 7202
	// (get) Token: 0x06005204 RID: 20996 RVA: 0x0002CBA4 File Offset: 0x0002ADA4
	// (set) Token: 0x06005205 RID: 20997 RVA: 0x0002CBAC File Offset: 0x0002ADAC
	public int StandaloneRoomCount { get; private set; }

	// Token: 0x17001C23 RID: 7203
	// (get) Token: 0x06005206 RID: 20998 RVA: 0x0002CBB5 File Offset: 0x0002ADB5
	// (set) Token: 0x06005207 RID: 20999 RVA: 0x0002CBBD File Offset: 0x0002ADBD
	public int MergeRoomCount { get; private set; }

	// Token: 0x17001C24 RID: 7204
	// (get) Token: 0x06005208 RID: 21000 RVA: 0x0002CBC6 File Offset: 0x0002ADC6
	// (set) Token: 0x06005209 RID: 21001 RVA: 0x0002CBCE File Offset: 0x0002ADCE
	private Dictionary<RoomSide, List<DoorLocation>> ValidDoorLocations { get; set; }

	// Token: 0x17001C25 RID: 7205
	// (get) Token: 0x0600520A RID: 21002 RVA: 0x0002CBD7 File Offset: 0x0002ADD7
	// (set) Token: 0x0600520B RID: 21003 RVA: 0x0002CBDF File Offset: 0x0002ADDF
	public int TunnelDestinationCount { get; private set; }

	// Token: 0x0600520C RID: 21004 RVA: 0x00137590 File Offset: 0x00135790
	public BiomeGridPointManager(BiomeType biome)
	{
		this.m_biome = biome;
	}

	// Token: 0x0600520D RID: 21005 RVA: 0x001375E0 File Offset: 0x001357E0
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

	// Token: 0x0600520E RID: 21006 RVA: 0x0013768C File Offset: 0x0013588C
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

	// Token: 0x0600520F RID: 21007 RVA: 0x001376C8 File Offset: 0x001358C8
	public bool GetAreCoordinatesWithinBorder(Vector2Int coordinates, bool isStandardRoom)
	{
		if (isStandardRoom && this.m_biome != BiomeType.Stone && this.m_biome != BiomeType.Tower && this.m_biome != BiomeType.TowerExterior)
		{
			return (float)coordinates.x < this.Border.max.x && (float)coordinates.x > this.Border.min.x && (float)coordinates.y < this.Border.max.y && (float)coordinates.y > this.Border.min.y;
		}
		return (float)coordinates.x < this.Border.max.x && (float)coordinates.y < this.Border.max.y && this.Border.Contains(coordinates);
	}

	// Token: 0x06005210 RID: 21008 RVA: 0x0002CBE8 File Offset: 0x0002ADE8
	public GridPoint GetConnectionPoint(RoomSide side)
	{
		return this.ConnectionPoints[side];
	}

	// Token: 0x06005211 RID: 21009 RVA: 0x0002CBF6 File Offset: 0x0002ADF6
	public List<DoorLocation> GetValidDoorLocations(RoomSide side)
	{
		return this.ValidDoorLocations[side];
	}

	// Token: 0x06005212 RID: 21010 RVA: 0x001377D4 File Offset: 0x001359D4
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

	// Token: 0x06005213 RID: 21011 RVA: 0x0002CC04 File Offset: 0x0002AE04
	public void SetStandaloneRoomCount(int count)
	{
		this.StandaloneRoomCount = count;
	}

	// Token: 0x06005214 RID: 21012 RVA: 0x0002CC0D File Offset: 0x0002AE0D
	public void SetMergeRoomCount(int count)
	{
		this.MergeRoomCount = count;
	}

	// Token: 0x06005215 RID: 21013 RVA: 0x00137898 File Offset: 0x00135A98
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

	// Token: 0x06005216 RID: 21014 RVA: 0x0013790C File Offset: 0x00135B0C
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

	// Token: 0x06005217 RID: 21015 RVA: 0x00137B08 File Offset: 0x00135D08
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

	// Token: 0x06005218 RID: 21016 RVA: 0x00137B68 File Offset: 0x00135D68
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

	// Token: 0x04003DC5 RID: 15813
	private BiomeType m_biome;

	// Token: 0x04003DC6 RID: 15814
	private List<GridPointManager> m_tunnelDestinationGridPointManagers = new List<GridPointManager>();

	// Token: 0x04003DC7 RID: 15815
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
