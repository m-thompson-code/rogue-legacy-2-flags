using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x0200089D RID: 2205
	public interface IEnemyEventTrackerState : IGameEventTrackerState
	{
		// Token: 0x17001799 RID: 6041
		// (get) Token: 0x06004818 RID: 18456
		List<EnemyTrackerData> EnemiesKilled { get; }

		// Token: 0x06004819 RID: 18457
		void Initialise(List<EnemyTrackerData> enemiesKilled);
	}
}
