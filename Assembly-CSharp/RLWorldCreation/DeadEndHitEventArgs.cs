using System;
using System.Collections.Generic;

namespace RLWorldCreation
{
	// Token: 0x02000DAA RID: 3498
	public class DeadEndHitEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060062C2 RID: 25282 RVA: 0x000366CF File Offset: 0x000348CF
		public DeadEndHitEventArgs(int roomNumber, int startLookingIndex, int stopLookingIndex, IEnumerable<GridPointManager> checkedRooms, GridPointManager chosenRoom)
		{
			this.RoomNumber = roomNumber;
			this.StartLookingIndex = startLookingIndex;
			this.StopLookingIndex = stopLookingIndex;
			this.CheckedRooms = checkedRooms;
			this.ChosenRoom = chosenRoom;
		}

		// Token: 0x17001FEC RID: 8172
		// (get) Token: 0x060062C3 RID: 25283 RVA: 0x000366FC File Offset: 0x000348FC
		public int RoomNumber { get; }

		// Token: 0x17001FED RID: 8173
		// (get) Token: 0x060062C4 RID: 25284 RVA: 0x00036704 File Offset: 0x00034904
		public int StartLookingIndex { get; }

		// Token: 0x17001FEE RID: 8174
		// (get) Token: 0x060062C5 RID: 25285 RVA: 0x0003670C File Offset: 0x0003490C
		public int StopLookingIndex { get; }

		// Token: 0x17001FEF RID: 8175
		// (get) Token: 0x060062C6 RID: 25286 RVA: 0x00036714 File Offset: 0x00034914
		public IEnumerable<GridPointManager> CheckedRooms { get; }

		// Token: 0x17001FF0 RID: 8176
		// (get) Token: 0x060062C7 RID: 25287 RVA: 0x0003671C File Offset: 0x0003491C
		public GridPointManager ChosenRoom { get; }
	}
}
