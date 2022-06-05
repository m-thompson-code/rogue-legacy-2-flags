using System;
using Rooms;

// Token: 0x02000B00 RID: 2816
public class RoomTypeEntry
{
	// Token: 0x06005474 RID: 21620 RVA: 0x0002DBF9 File Offset: 0x0002BDF9
	public RoomTypeEntry(RoomType roomType) : this(roomType, RoomSide.Any, null, false)
	{
	}

	// Token: 0x06005475 RID: 21621 RVA: 0x0002DC05 File Offset: 0x0002BE05
	public RoomTypeEntry(RoomType roomType, RoomSide side, RoomMetaData roomMetaData, bool mustBeEasy)
	{
		this.RoomType = roomType;
		this.Side = side;
		this.RoomMetaData = roomMetaData;
		this.MustBeEasy = mustBeEasy;
	}

	// Token: 0x06005476 RID: 21622 RVA: 0x0013FF94 File Offset: 0x0013E194
	public override string ToString()
	{
		string text = "None";
		if (this.RoomMetaData != null)
		{
			text = this.RoomMetaData.ID.ToString();
		}
		return string.Format("(Type = {0}. ID = {1}, Side = {2}, IsEasy = {3})", new object[]
		{
			this.RoomType,
			text,
			this.Side,
			this.MustBeEasy
		});
	}

	// Token: 0x04003EF3 RID: 16115
	public RoomType RoomType = RoomType.None;

	// Token: 0x04003EF4 RID: 16116
	public RoomMetaData RoomMetaData;

	// Token: 0x04003EF5 RID: 16117
	public RoomSide Side = RoomSide.Any;

	// Token: 0x04003EF6 RID: 16118
	public bool MustBeEasy;
}
