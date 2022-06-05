using System;
using System.Collections.Generic;
using UnityEngine;

namespace RLWorldCreation
{
	// Token: 0x02000DA7 RID: 3495
	public class NoPotentialRoomsEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060062B6 RID: 25270 RVA: 0x0003662E File Offset: 0x0003482E
		public NoPotentialRoomsEventArgs(GridPointManager originRoom, DoorLocation doorLocation, Dictionary<Vector2Int, List<DoorLocation>> roomSizesThatFit)
		{
			this.OriginRoom = originRoom;
			this.DoorLocation = doorLocation;
			this.RoomSizesThatFit = roomSizesThatFit;
		}

		// Token: 0x17001FE3 RID: 8163
		// (get) Token: 0x060062B7 RID: 25271 RVA: 0x0003664B File Offset: 0x0003484B
		public GridPointManager OriginRoom { get; }

		// Token: 0x17001FE4 RID: 8164
		// (get) Token: 0x060062B8 RID: 25272 RVA: 0x00036653 File Offset: 0x00034853
		public DoorLocation DoorLocation { get; }

		// Token: 0x17001FE5 RID: 8165
		// (get) Token: 0x060062B9 RID: 25273 RVA: 0x0003665B File Offset: 0x0003485B
		public Dictionary<Vector2Int, List<DoorLocation>> RoomSizesThatFit { get; }
	}
}
