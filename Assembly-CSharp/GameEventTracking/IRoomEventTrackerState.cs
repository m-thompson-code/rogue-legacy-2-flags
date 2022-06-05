using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x02000DCB RID: 3531
	public interface IRoomEventTrackerState : IGameEventTrackerState
	{
		// Token: 0x1700200C RID: 8204
		// (get) Token: 0x06006355 RID: 25429
		List<RoomTrackerData> RoomsEntered { get; }

		// Token: 0x06006356 RID: 25430
		void Initialise(List<RoomTrackerData> roomsEntered);
	}
}
