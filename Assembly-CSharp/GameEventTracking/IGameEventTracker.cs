using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x02000DC8 RID: 3528
	public interface IGameEventTracker<T> where T : IGameEventTrackerState
	{
		// Token: 0x0600634F RID: 25423
		IEnumerable<IGameEventData> GetGameEvents();

		// Token: 0x06006350 RID: 25424
		void Reset();

		// Token: 0x06006351 RID: 25425
		void RestoreState(T state);

		// Token: 0x06006352 RID: 25426
		T SaveState();
	}
}
