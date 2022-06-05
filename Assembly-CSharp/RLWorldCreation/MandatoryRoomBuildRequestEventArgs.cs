using System;

namespace RLWorldCreation
{
	// Token: 0x02000886 RID: 2182
	public class MandatoryRoomBuildRequestEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060047B6 RID: 18358 RVA: 0x0010209D File Offset: 0x0010029D
		public MandatoryRoomBuildRequestEventArgs(string roomID)
		{
			this.RoomID = roomID;
		}

		// Token: 0x17001780 RID: 6016
		// (get) Token: 0x060047B7 RID: 18359 RVA: 0x001020AC File Offset: 0x001002AC
		public string RoomID { get; }
	}
}
