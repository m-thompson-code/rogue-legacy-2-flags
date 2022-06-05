using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x020008AF RID: 2223
	public class ItemEventTrackerState : IItemEventTrackerState, IGameEventTrackerState
	{
		// Token: 0x170017C8 RID: 6088
		// (get) Token: 0x06004881 RID: 18561 RVA: 0x001043E8 File Offset: 0x001025E8
		// (set) Token: 0x06004882 RID: 18562 RVA: 0x001043F0 File Offset: 0x001025F0
		public List<ChestTrackerData> ChestsOpened
		{
			get
			{
				return this.m_chestsOpened;
			}
			private set
			{
				this.m_chestsOpened = value;
			}
		}

		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x06004883 RID: 18563 RVA: 0x001043F9 File Offset: 0x001025F9
		// (set) Token: 0x06004884 RID: 18564 RVA: 0x00104401 File Offset: 0x00102601
		public List<ItemTrackerData> ItemsCollected
		{
			get
			{
				return this.m_goldCollected;
			}
			private set
			{
				this.m_goldCollected = value;
			}
		}

		// Token: 0x06004885 RID: 18565 RVA: 0x0010440A File Offset: 0x0010260A
		public ItemEventTrackerState(List<ChestTrackerData> chestsOpened, List<ItemTrackerData> goldCollected)
		{
			this.Initialise(chestsOpened, goldCollected);
		}

		// Token: 0x06004886 RID: 18566 RVA: 0x0010441A File Offset: 0x0010261A
		public void Initialise(List<ChestTrackerData> chestsOpened, List<ItemTrackerData> goldCollected)
		{
			this.ChestsOpened = chestsOpened;
			this.ItemsCollected = goldCollected;
		}

		// Token: 0x04003D2F RID: 15663
		private List<ChestTrackerData> m_chestsOpened;

		// Token: 0x04003D30 RID: 15664
		private List<ItemTrackerData> m_goldCollected;
	}
}
