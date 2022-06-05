using System;
using System.Collections.Generic;
using UnityEngine;

namespace RLWorldCreation
{
	// Token: 0x02000887 RID: 2183
	public class NoPotentialRoomsEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060047B8 RID: 18360 RVA: 0x001020B4 File Offset: 0x001002B4
		public NoPotentialRoomsEventArgs(GridPointManager originRoom, DoorLocation doorLocation, Dictionary<Vector2Int, List<DoorLocation>> roomSizesThatFit)
		{
			this.OriginRoom = originRoom;
			this.DoorLocation = doorLocation;
			this.RoomSizesThatFit = roomSizesThatFit;
		}

		// Token: 0x17001781 RID: 6017
		// (get) Token: 0x060047B9 RID: 18361 RVA: 0x001020D1 File Offset: 0x001002D1
		public GridPointManager OriginRoom { get; }

		// Token: 0x17001782 RID: 6018
		// (get) Token: 0x060047BA RID: 18362 RVA: 0x001020D9 File Offset: 0x001002D9
		public DoorLocation DoorLocation { get; }

		// Token: 0x17001783 RID: 6019
		// (get) Token: 0x060047BB RID: 18363 RVA: 0x001020E1 File Offset: 0x001002E1
		public Dictionary<Vector2Int, List<DoorLocation>> RoomSizesThatFit { get; }
	}
}
