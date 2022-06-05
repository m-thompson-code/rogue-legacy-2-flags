using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rooms;
using UnityEngine;

// Token: 0x0200065E RID: 1630
public class DebugCreateRooms : MonoBehaviour
{
	// Token: 0x06003B13 RID: 15123 RVA: 0x000CB436 File Offset: 0x000C9636
	public static void BuildRoom(BiomeType biome, Vector2Int coords, DoorLocation doorLocation, RoomSetEntry room)
	{
		DebugCreateRooms.m_buildRoomReports.Enqueue(new DebugCreateRooms.BuildRoomReport(biome, coords, doorLocation, room));
	}

	// Token: 0x06003B14 RID: 15124 RVA: 0x000CB44B File Offset: 0x000C964B
	public static void PotentialRooms(GridPointManager originRoom, DoorLocation doorLocation, RoomTypeEntry roomRequirements, HashSet<RoomSetEntry> potentialRooms)
	{
		DebugCreateRooms.m_buildRoomReports.Enqueue(new DebugCreateRooms.PotentialRoomsBuildReport(originRoom, doorLocation, roomRequirements, potentialRooms));
	}

	// Token: 0x06003B15 RID: 15125 RVA: 0x000CB460 File Offset: 0x000C9660
	public static void Reset()
	{
		DebugCreateRooms.m_buildRoomReports.Clear();
	}

	// Token: 0x06003B16 RID: 15126 RVA: 0x000CB46C File Offset: 0x000C966C
	internal static void MaxIterationCountReached()
	{
		Debugger.Break();
	}

	// Token: 0x04002CFF RID: 11519
	private static Queue<DebugCreateRooms.IBuildRoomReport> m_buildRoomReports = new Queue<DebugCreateRooms.IBuildRoomReport>();

	// Token: 0x02000DCC RID: 3532
	private interface IBuildRoomReport
	{
	}

	// Token: 0x02000DCD RID: 3533
	private class PotentialRoomsBuildReport : DebugCreateRooms.IBuildRoomReport
	{
		// Token: 0x060069E4 RID: 27108 RVA: 0x0018C958 File Offset: 0x0018AB58
		public PotentialRoomsBuildReport(GridPointManager originRoom, DoorLocation doorLocation, RoomTypeEntry roomRequirements, HashSet<RoomSetEntry> potentialRooms)
		{
			this.Biome = originRoom.Biome;
			this.OriginRoomNumber = originRoom.RoomNumber;
			this.OriginRoomCoords = originRoom.GridCoordinates;
			this.DoorLocation = doorLocation;
			this.RoomType = roomRequirements.RoomType;
			this.PotentialRooms = potentialRooms;
		}

		// Token: 0x170022B2 RID: 8882
		// (get) Token: 0x060069E5 RID: 27109 RVA: 0x0018C9AA File Offset: 0x0018ABAA
		public BiomeType Biome { get; }

		// Token: 0x170022B3 RID: 8883
		// (get) Token: 0x060069E6 RID: 27110 RVA: 0x0018C9B2 File Offset: 0x0018ABB2
		public int OriginRoomNumber { get; }

		// Token: 0x170022B4 RID: 8884
		// (get) Token: 0x060069E7 RID: 27111 RVA: 0x0018C9BA File Offset: 0x0018ABBA
		public Vector2Int OriginRoomCoords { get; }

		// Token: 0x170022B5 RID: 8885
		// (get) Token: 0x060069E8 RID: 27112 RVA: 0x0018C9C2 File Offset: 0x0018ABC2
		public DoorLocation DoorLocation { get; }

		// Token: 0x170022B6 RID: 8886
		// (get) Token: 0x060069E9 RID: 27113 RVA: 0x0018C9CA File Offset: 0x0018ABCA
		public RoomType RoomType { get; }

		// Token: 0x170022B7 RID: 8887
		// (get) Token: 0x060069EA RID: 27114 RVA: 0x0018C9D2 File Offset: 0x0018ABD2
		public HashSet<RoomSetEntry> PotentialRooms { get; }

		// Token: 0x170022B8 RID: 8888
		// (get) Token: 0x060069EB RID: 27115 RVA: 0x0018C9DA File Offset: 0x0018ABDA
		public int PotentialRoomCount
		{
			get
			{
				return this.PotentialRooms.Count;
			}
		}
	}

	// Token: 0x02000DCE RID: 3534
	private class BuildRoomReport : DebugCreateRooms.IBuildRoomReport
	{
		// Token: 0x060069EC RID: 27116 RVA: 0x0018C9E7 File Offset: 0x0018ABE7
		public BuildRoomReport(BiomeType biome, Vector2Int coords, DoorLocation doorLocation, RoomSetEntry room)
		{
			this.Biome = biome;
			this.Coords = coords;
			this.DoorLocation = doorLocation;
			this.RoomSize = room.RoomMetaData.Size;
		}

		// Token: 0x170022B9 RID: 8889
		// (get) Token: 0x060069ED RID: 27117 RVA: 0x0018CA16 File Offset: 0x0018AC16
		public BiomeType Biome { get; }

		// Token: 0x170022BA RID: 8890
		// (get) Token: 0x060069EE RID: 27118 RVA: 0x0018CA1E File Offset: 0x0018AC1E
		public Vector2Int Coords { get; }

		// Token: 0x170022BB RID: 8891
		// (get) Token: 0x060069EF RID: 27119 RVA: 0x0018CA26 File Offset: 0x0018AC26
		public DoorLocation DoorLocation { get; }

		// Token: 0x170022BC RID: 8892
		// (get) Token: 0x060069F0 RID: 27120 RVA: 0x0018CA2E File Offset: 0x0018AC2E
		public Vector2Int RoomSize { get; }
	}
}
