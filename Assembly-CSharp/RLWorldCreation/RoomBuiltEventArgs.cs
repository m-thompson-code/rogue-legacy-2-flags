using System;

namespace RLWorldCreation
{
	// Token: 0x02000889 RID: 2185
	public class RoomBuiltEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060047C2 RID: 18370 RVA: 0x0010213E File Offset: 0x0010033E
		public RoomBuiltEventArgs(GridPointManager gridPointManager)
		{
			this.GridPointManager = gridPointManager;
		}

		// Token: 0x17001789 RID: 6025
		// (get) Token: 0x060047C3 RID: 18371 RVA: 0x0010214D File Offset: 0x0010034D
		public GridPointManager GridPointManager { get; }
	}
}
