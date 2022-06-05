using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rooms;
using UnityEngine;

// Token: 0x02000AB4 RID: 2740
public class DebugCreateRooms : MonoBehaviour
{
	// Token: 0x0600529E RID: 21150 RVA: 0x0002CFA6 File Offset: 0x0002B1A6
	public static void BuildRoom(BiomeType biome, Vector2Int coords, DoorLocation doorLocation, RoomSetEntry room)
	{
		DebugCreateRooms.m_buildRoomReports.Enqueue(new DebugCreateRooms.BuildRoomReport(biome, coords, doorLocation, room));
	}

	// Token: 0x0600529F RID: 21151 RVA: 0x0002CFBB File Offset: 0x0002B1BB
	public static void PotentialRooms(GridPointManager originRoom, DoorLocation doorLocation, RoomTypeEntry roomRequirements, HashSet<RoomSetEntry> potentialRooms)
	{
		DebugCreateRooms.m_buildRoomReports.Enqueue(new DebugCreateRooms.PotentialRoomsBuildReport(originRoom, doorLocation, roomRequirements, potentialRooms));
	}

	// Token: 0x060052A0 RID: 21152 RVA: 0x0002CFD0 File Offset: 0x0002B1D0
	public static void Reset()
	{
		DebugCreateRooms.m_buildRoomReports.Clear();
	}

	// Token: 0x060052A1 RID: 21153 RVA: 0x0002CFDC File Offset: 0x0002B1DC
	internal static void MaxIterationCountReached()
	{
		Debugger.Break();
	}

	// Token: 0x04003E14 RID: 15892
	private static Queue<DebugCreateRooms.IBuildRoomReport> m_buildRoomReports = new Queue<DebugCreateRooms.IBuildRoomReport>();

	// Token: 0x02000AB5 RID: 2741
	private interface IBuildRoomReport
	{
	}

	// Token: 0x02000AB6 RID: 2742
	private class PotentialRoomsBuildReport : DebugCreateRooms.IBuildRoomReport
	{
		// Token: 0x060052A4 RID: 21156 RVA: 0x00139E34 File Offset: 0x00138034
		public PotentialRoomsBuildReport(GridPointManager originRoom, DoorLocation doorLocation, RoomTypeEntry roomRequirements, HashSet<RoomSetEntry> potentialRooms)
		{
			this.Biome = originRoom.Biome;
			this.OriginRoomNumber = originRoom.RoomNumber;
			this.OriginRoomCoords = originRoom.GridCoordinates;
			this.DoorLocation = doorLocation;
			this.RoomType = roomRequirements.RoomType;
			this.PotentialRooms = potentialRooms;
		}

		// Token: 0x17001C3A RID: 7226
		// (get) Token: 0x060052A5 RID: 21157 RVA: 0x0002CFEF File Offset: 0x0002B1EF
		public BiomeType Biome { get; }

		// Token: 0x17001C3B RID: 7227
		// (get) Token: 0x060052A6 RID: 21158 RVA: 0x0002CFF7 File Offset: 0x0002B1F7
		public int OriginRoomNumber { get; }

		// Token: 0x17001C3C RID: 7228
		// (get) Token: 0x060052A7 RID: 21159 RVA: 0x0002CFFF File Offset: 0x0002B1FF
		public Vector2Int OriginRoomCoords { get; }

		// Token: 0x17001C3D RID: 7229
		// (get) Token: 0x060052A8 RID: 21160 RVA: 0x0002D007 File Offset: 0x0002B207
		public DoorLocation DoorLocation { get; }

		// Token: 0x17001C3E RID: 7230
		// (get) Token: 0x060052A9 RID: 21161 RVA: 0x0002D00F File Offset: 0x0002B20F
		public RoomType RoomType { get; }

		// Token: 0x17001C3F RID: 7231
		// (get) Token: 0x060052AA RID: 21162 RVA: 0x0002D017 File Offset: 0x0002B217
		public HashSet<RoomSetEntry> PotentialRooms { get; }

		// Token: 0x17001C40 RID: 7232
		// (get) Token: 0x060052AB RID: 21163 RVA: 0x0002D01F File Offset: 0x0002B21F
		public int PotentialRoomCount
		{
			get
			{
				return this.PotentialRooms.Count;
			}
		}
	}

	// Token: 0x02000AB7 RID: 2743
	private class BuildRoomReport : DebugCreateRooms.IBuildRoomReport
	{
		// Token: 0x060052AC RID: 21164 RVA: 0x0002D02C File Offset: 0x0002B22C
		public BuildRoomReport(BiomeType biome, Vector2Int coords, DoorLocation doorLocation, RoomSetEntry room)
		{
			this.Biome = biome;
			this.Coords = coords;
			this.DoorLocation = doorLocation;
			this.RoomSize = room.RoomMetaData.Size;
		}

		// Token: 0x17001C41 RID: 7233
		// (get) Token: 0x060052AD RID: 21165 RVA: 0x0002D05B File Offset: 0x0002B25B
		public BiomeType Biome { get; }

		// Token: 0x17001C42 RID: 7234
		// (get) Token: 0x060052AE RID: 21166 RVA: 0x0002D063 File Offset: 0x0002B263
		public Vector2Int Coords { get; }

		// Token: 0x17001C43 RID: 7235
		// (get) Token: 0x060052AF RID: 21167 RVA: 0x0002D06B File Offset: 0x0002B26B
		public DoorLocation DoorLocation { get; }

		// Token: 0x17001C44 RID: 7236
		// (get) Token: 0x060052B0 RID: 21168 RVA: 0x0002D073 File Offset: 0x0002B273
		public Vector2Int RoomSize { get; }
	}
}
