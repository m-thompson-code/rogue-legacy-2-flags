using System;
using System.Collections.Generic;

namespace RLWorldCreation
{
	// Token: 0x0200088A RID: 2186
	public class DeadEndHitEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060047C4 RID: 18372 RVA: 0x00102155 File Offset: 0x00100355
		public DeadEndHitEventArgs(int roomNumber, int startLookingIndex, int stopLookingIndex, IEnumerable<GridPointManager> checkedRooms, GridPointManager chosenRoom)
		{
			this.RoomNumber = roomNumber;
			this.StartLookingIndex = startLookingIndex;
			this.StopLookingIndex = stopLookingIndex;
			this.CheckedRooms = checkedRooms;
			this.ChosenRoom = chosenRoom;
		}

		// Token: 0x1700178A RID: 6026
		// (get) Token: 0x060047C5 RID: 18373 RVA: 0x00102182 File Offset: 0x00100382
		public int RoomNumber { get; }

		// Token: 0x1700178B RID: 6027
		// (get) Token: 0x060047C6 RID: 18374 RVA: 0x0010218A File Offset: 0x0010038A
		public int StartLookingIndex { get; }

		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x060047C7 RID: 18375 RVA: 0x00102192 File Offset: 0x00100392
		public int StopLookingIndex { get; }

		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x060047C8 RID: 18376 RVA: 0x0010219A File Offset: 0x0010039A
		public IEnumerable<GridPointManager> CheckedRooms { get; }

		// Token: 0x1700178E RID: 6030
		// (get) Token: 0x060047C9 RID: 18377 RVA: 0x001021A2 File Offset: 0x001003A2
		public GridPointManager ChosenRoom { get; }
	}
}
