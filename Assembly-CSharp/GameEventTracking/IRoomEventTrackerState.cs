using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x0200089E RID: 2206
	public interface IRoomEventTrackerState : IGameEventTrackerState
	{
		// Token: 0x1700179A RID: 6042
		// (get) Token: 0x0600481A RID: 18458
		List<RoomTrackerData> RoomsEntered { get; }

		// Token: 0x0600481B RID: 18459
		void Initialise(List<RoomTrackerData> roomsEntered);
	}
}
