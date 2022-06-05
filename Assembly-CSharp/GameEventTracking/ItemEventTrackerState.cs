using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x02000DE3 RID: 3555
	public class ItemEventTrackerState : IItemEventTrackerState, IGameEventTrackerState
	{
		// Token: 0x17002044 RID: 8260
		// (get) Token: 0x060063EB RID: 25579 RVA: 0x0003724D File Offset: 0x0003544D
		// (set) Token: 0x060063EC RID: 25580 RVA: 0x00037255 File Offset: 0x00035455
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

		// Token: 0x17002045 RID: 8261
		// (get) Token: 0x060063ED RID: 25581 RVA: 0x0003725E File Offset: 0x0003545E
		// (set) Token: 0x060063EE RID: 25582 RVA: 0x00037266 File Offset: 0x00035466
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

		// Token: 0x060063EF RID: 25583 RVA: 0x0003726F File Offset: 0x0003546F
		public ItemEventTrackerState(List<ChestTrackerData> chestsOpened, List<ItemTrackerData> goldCollected)
		{
			this.Initialise(chestsOpened, goldCollected);
		}

		// Token: 0x060063F0 RID: 25584 RVA: 0x0003727F File Offset: 0x0003547F
		public void Initialise(List<ChestTrackerData> chestsOpened, List<ItemTrackerData> goldCollected)
		{
			this.ChestsOpened = chestsOpened;
			this.ItemsCollected = goldCollected;
		}

		// Token: 0x0400517C RID: 20860
		private List<ChestTrackerData> m_chestsOpened;

		// Token: 0x0400517D RID: 20861
		private List<ItemTrackerData> m_goldCollected;
	}
}
