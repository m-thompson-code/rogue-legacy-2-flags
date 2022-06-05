using System;

namespace RLWorldCreation
{
	// Token: 0x02000DA6 RID: 3494
	public class MandatoryRoomBuildRequestEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060062B4 RID: 25268 RVA: 0x00036617 File Offset: 0x00034817
		public MandatoryRoomBuildRequestEventArgs(string roomID)
		{
			this.RoomID = roomID;
		}

		// Token: 0x17001FE2 RID: 8162
		// (get) Token: 0x060062B5 RID: 25269 RVA: 0x00036626 File Offset: 0x00034826
		public string RoomID { get; }
	}
}
