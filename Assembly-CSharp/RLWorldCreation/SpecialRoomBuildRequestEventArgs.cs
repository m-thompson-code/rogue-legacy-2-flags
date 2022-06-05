using System;

namespace RLWorldCreation
{
	// Token: 0x02000885 RID: 2181
	public class SpecialRoomBuildRequestEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060047B3 RID: 18355 RVA: 0x00102077 File Offset: 0x00100277
		public SpecialRoomBuildRequestEventArgs(SpecialRoomType specialRoomType, bool isSuccess)
		{
			this.SpecialRoomType = specialRoomType;
			this.IsSuccess = isSuccess;
		}

		// Token: 0x1700177E RID: 6014
		// (get) Token: 0x060047B4 RID: 18356 RVA: 0x0010208D File Offset: 0x0010028D
		public SpecialRoomType SpecialRoomType { get; }

		// Token: 0x1700177F RID: 6015
		// (get) Token: 0x060047B5 RID: 18357 RVA: 0x00102095 File Offset: 0x00100295
		public bool IsSuccess { get; }
	}
}
