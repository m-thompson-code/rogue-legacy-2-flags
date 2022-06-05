using System;
using Rooms;

// Token: 0x02000680 RID: 1664
public class RoomTypeEntry
{
	// Token: 0x06003C15 RID: 15381 RVA: 0x000D00C6 File Offset: 0x000CE2C6
	public RoomTypeEntry(RoomType roomType) : this(roomType, RoomSide.Any, null, false)
	{
	}

	// Token: 0x06003C16 RID: 15382 RVA: 0x000D00D2 File Offset: 0x000CE2D2
	public RoomTypeEntry(RoomType roomType, RoomSide side, RoomMetaData roomMetaData, bool mustBeEasy)
	{
		this.RoomType = roomType;
		this.Side = side;
		this.RoomMetaData = roomMetaData;
		this.MustBeEasy = mustBeEasy;
	}

	// Token: 0x06003C17 RID: 15383 RVA: 0x000D010C File Offset: 0x000CE30C
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

	// Token: 0x04002D4A RID: 11594
	public RoomType RoomType = RoomType.None;

	// Token: 0x04002D4B RID: 11595
	public RoomMetaData RoomMetaData;

	// Token: 0x04002D4C RID: 11596
	public RoomSide Side = RoomSide.Any;

	// Token: 0x04002D4D RID: 11597
	public bool MustBeEasy;
}
