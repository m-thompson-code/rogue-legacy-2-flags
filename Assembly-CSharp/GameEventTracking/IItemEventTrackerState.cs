using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x02000DCC RID: 3532
	public interface IItemEventTrackerState : IGameEventTrackerState
	{
		// Token: 0x1700200D RID: 8205
		// (get) Token: 0x06006357 RID: 25431
		List<ChestTrackerData> ChestsOpened { get; }

		// Token: 0x1700200E RID: 8206
		// (get) Token: 0x06006358 RID: 25432
		List<ItemTrackerData> ItemsCollected { get; }

		// Token: 0x06006359 RID: 25433
		void Initialise(List<ChestTrackerData> chestsOpened, List<ItemTrackerData> itemsCollected);
	}
}
