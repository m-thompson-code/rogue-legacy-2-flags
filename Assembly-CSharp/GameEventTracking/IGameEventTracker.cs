using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x0200089B RID: 2203
	public interface IGameEventTracker<T> where T : IGameEventTrackerState
	{
		// Token: 0x06004814 RID: 18452
		IEnumerable<IGameEventData> GetGameEvents();

		// Token: 0x06004815 RID: 18453
		void Reset();

		// Token: 0x06004816 RID: 18454
		void RestoreState(T state);

		// Token: 0x06004817 RID: 18455
		T SaveState();
	}
}
