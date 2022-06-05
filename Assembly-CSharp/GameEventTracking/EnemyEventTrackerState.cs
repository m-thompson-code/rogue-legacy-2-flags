using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x02000DD6 RID: 3542
	[Serializable]
	public class EnemyEventTrackerState : IEnemyEventTrackerState, IGameEventTrackerState
	{
		// Token: 0x17002019 RID: 8217
		// (get) Token: 0x0600638B RID: 25483 RVA: 0x00036DD5 File Offset: 0x00034FD5
		// (set) Token: 0x0600638C RID: 25484 RVA: 0x00036DDD File Offset: 0x00034FDD
		public List<EnemyTrackerData> EnemiesKilled
		{
			get
			{
				return this.m_enemiesKilled;
			}
			private set
			{
				this.m_enemiesKilled = value;
			}
		}

		// Token: 0x0600638D RID: 25485 RVA: 0x00036DE6 File Offset: 0x00034FE6
		public EnemyEventTrackerState(List<EnemyTrackerData> enemiesKilled)
		{
			this.Initialise(enemiesKilled);
		}

		// Token: 0x0600638E RID: 25486 RVA: 0x00036DF5 File Offset: 0x00034FF5
		public void Initialise(List<EnemyTrackerData> enemiesKilled)
		{
			this.EnemiesKilled = enemiesKilled;
		}

		// Token: 0x04005144 RID: 20804
		private List<EnemyTrackerData> m_enemiesKilled;
	}
}
