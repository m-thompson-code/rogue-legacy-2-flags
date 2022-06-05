using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x0200089F RID: 2207
	public interface IItemEventTrackerState : IGameEventTrackerState
	{
		// Token: 0x1700179B RID: 6043
		// (get) Token: 0x0600481C RID: 18460
		List<ChestTrackerData> ChestsOpened { get; }

		// Token: 0x1700179C RID: 6044
		// (get) Token: 0x0600481D RID: 18461
		List<ItemTrackerData> ItemsCollected { get; }

		// Token: 0x0600481E RID: 18462
		void Initialise(List<ChestTrackerData> chestsOpened, List<ItemTrackerData> itemsCollected);
	}
}
