using System;

namespace RLWorldCreation
{
	// Token: 0x02000DA5 RID: 3493
	public class SpecialRoomBuildRequestEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060062B1 RID: 25265 RVA: 0x000365F1 File Offset: 0x000347F1
		public SpecialRoomBuildRequestEventArgs(SpecialRoomType specialRoomType, bool isSuccess)
		{
			this.SpecialRoomType = specialRoomType;
			this.IsSuccess = isSuccess;
		}

		// Token: 0x17001FE0 RID: 8160
		// (get) Token: 0x060062B2 RID: 25266 RVA: 0x00036607 File Offset: 0x00034807
		public SpecialRoomType SpecialRoomType { get; }

		// Token: 0x17001FE1 RID: 8161
		// (get) Token: 0x060062B3 RID: 25267 RVA: 0x0003660F File Offset: 0x0003480F
		public bool IsSuccess { get; }
	}
}
