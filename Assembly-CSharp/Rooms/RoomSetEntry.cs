using System;

namespace Rooms
{
	// Token: 0x02000E38 RID: 3640
	public struct RoomSetEntry
	{
		// Token: 0x170020E3 RID: 8419
		// (get) Token: 0x06006682 RID: 26242 RVA: 0x00038660 File Offset: 0x00036860
		// (set) Token: 0x06006683 RID: 26243 RVA: 0x00038668 File Offset: 0x00036868
		public bool IsMirrored { readonly get; private set; }

		// Token: 0x170020E4 RID: 8420
		// (get) Token: 0x06006684 RID: 26244 RVA: 0x00038671 File Offset: 0x00036871
		public readonly RoomMetaData RoomMetaData { get; }

		// Token: 0x170020E5 RID: 8421
		// (get) Token: 0x06006685 RID: 26245 RVA: 0x00038679 File Offset: 0x00036879
		// (set) Token: 0x06006686 RID: 26246 RVA: 0x00038681 File Offset: 0x00036881
		public int Weight { readonly get; private set; }

		// Token: 0x170020E6 RID: 8422
		// (get) Token: 0x06006687 RID: 26247 RVA: 0x0003868A File Offset: 0x0003688A
		// (set) Token: 0x06006688 RID: 26248 RVA: 0x00038692 File Offset: 0x00036892
		public int DoorMask { readonly get; private set; }

		// Token: 0x06006689 RID: 26249 RVA: 0x0017B244 File Offset: 0x00179444
		public RoomSetEntry(RoomMetaData roomMetaData, bool isMirrored = false)
		{
			this.RoomMetaData = roomMetaData;
			this.IsMirrored = isMirrored;
			this.Weight = -1;
			this.DoorMask = 0;
			foreach (DoorLocation doorLocation in roomMetaData.DoorLocations)
			{
				RoomSide roomSide = (!this.IsMirrored) ? doorLocation.RoomSide : RoomUtility.GetMirrorSide(doorLocation.RoomSide);
				int num = (!this.IsMirrored) ? doorLocation.DoorNumber : RoomUtility.GetMirrorDoorNumber(doorLocation, roomMetaData.Size);
				this.DoorMask |= 1 << (int)(roomSide * RoomSide.Right + num);
			}
		}

		// Token: 0x0600668A RID: 26250 RVA: 0x0003869B File Offset: 0x0003689B
		public RoomSetEntry(RoomSetEntry original, int weight)
		{
			this.IsMirrored = original.IsMirrored;
			this.RoomMetaData = original.RoomMetaData;
			this.Weight = weight;
			this.DoorMask = original.DoorMask;
		}

		// Token: 0x0600668B RID: 26251 RVA: 0x0017B2E0 File Offset: 0x001794E0
		public override string ToString()
		{
			return this.RoomMetaData.ID.ToString() + (this.IsMirrored ? "M" : "");
		}
	}
}
