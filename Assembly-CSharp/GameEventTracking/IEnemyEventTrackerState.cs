using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x02000DCA RID: 3530
	public interface IEnemyEventTrackerState : IGameEventTrackerState
	{
		// Token: 0x1700200B RID: 8203
		// (get) Token: 0x06006353 RID: 25427
		List<EnemyTrackerData> EnemiesKilled { get; }

		// Token: 0x06006354 RID: 25428
		void Initialise(List<EnemyTrackerData> enemiesKilled);
	}
}
