using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x020008A5 RID: 2213
	[Serializable]
	public class EnemyEventTrackerState : IEnemyEventTrackerState, IGameEventTrackerState
	{
		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x0600483A RID: 18490 RVA: 0x00103D24 File Offset: 0x00101F24
		// (set) Token: 0x0600483B RID: 18491 RVA: 0x00103D2C File Offset: 0x00101F2C
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

		// Token: 0x0600483C RID: 18492 RVA: 0x00103D35 File Offset: 0x00101F35
		public EnemyEventTrackerState(List<EnemyTrackerData> enemiesKilled)
		{
			this.Initialise(enemiesKilled);
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x00103D44 File Offset: 0x00101F44
		public void Initialise(List<EnemyTrackerData> enemiesKilled)
		{
			this.EnemiesKilled = enemiesKilled;
		}

		// Token: 0x04003D04 RID: 15620
		private List<EnemyTrackerData> m_enemiesKilled;
	}
}
