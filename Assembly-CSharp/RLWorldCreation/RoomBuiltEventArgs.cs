using System;

namespace RLWorldCreation
{
	// Token: 0x02000DA9 RID: 3497
	public class RoomBuiltEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060062C0 RID: 25280 RVA: 0x000366B8 File Offset: 0x000348B8
		public RoomBuiltEventArgs(GridPointManager gridPointManager)
		{
			this.GridPointManager = gridPointManager;
		}

		// Token: 0x17001FEB RID: 8171
		// (get) Token: 0x060062C1 RID: 25281 RVA: 0x000366C7 File Offset: 0x000348C7
		public GridPointManager GridPointManager { get; }
	}
}
