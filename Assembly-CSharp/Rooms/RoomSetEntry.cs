using System;

namespace Rooms
{
	// Token: 0x020008CE RID: 2254
	public struct RoomSetEntry
	{
		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x060049E7 RID: 18919 RVA: 0x0010A553 File Offset: 0x00108753
		// (set) Token: 0x060049E8 RID: 18920 RVA: 0x0010A55B File Offset: 0x0010875B
		public bool IsMirrored { readonly get; private set; }

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x060049E9 RID: 18921 RVA: 0x0010A564 File Offset: 0x00108764
		public readonly RoomMetaData RoomMetaData { get; }

		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x060049EA RID: 18922 RVA: 0x0010A56C File Offset: 0x0010876C
		// (set) Token: 0x060049EB RID: 18923 RVA: 0x0010A574 File Offset: 0x00108774
		public int Weight { readonly get; private set; }

		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x060049EC RID: 18924 RVA: 0x0010A57D File Offset: 0x0010877D
		// (set) Token: 0x060049ED RID: 18925 RVA: 0x0010A585 File Offset: 0x00108785
		public int DoorMask { readonly get; private set; }

		// Token: 0x060049EE RID: 18926 RVA: 0x0010A590 File Offset: 0x00108790
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

		// Token: 0x060049EF RID: 18927 RVA: 0x0010A62B File Offset: 0x0010882B
		public RoomSetEntry(RoomSetEntry original, int weight)
		{
			this.IsMirrored = original.IsMirrored;
			this.RoomMetaData = original.RoomMetaData;
			this.Weight = weight;
			this.DoorMask = original.DoorMask;
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x0010A65C File Offset: 0x0010885C
		public override string ToString()
		{
			return this.RoomMetaData.ID.ToString() + (this.IsMirrored ? "M" : "");
		}
	}
}
