using System;

// Token: 0x02000793 RID: 1939
public class DoorNumberChangeEventArgs : EventArgs
{
	// Token: 0x0600418B RID: 16779 RVA: 0x000E9A5A File Offset: 0x000E7C5A
	public DoorNumberChangeEventArgs(Door door, int originalNumber, int newNumber)
	{
		this.Door = door;
		this.OriginalNumber = originalNumber;
		this.NewNumber = newNumber;
	}

	// Token: 0x17001655 RID: 5717
	// (get) Token: 0x0600418C RID: 16780 RVA: 0x000E9A77 File Offset: 0x000E7C77
	// (set) Token: 0x0600418D RID: 16781 RVA: 0x000E9A7F File Offset: 0x000E7C7F
	public Door Door { get; private set; }

	// Token: 0x17001656 RID: 5718
	// (get) Token: 0x0600418E RID: 16782 RVA: 0x000E9A88 File Offset: 0x000E7C88
	// (set) Token: 0x0600418F RID: 16783 RVA: 0x000E9A90 File Offset: 0x000E7C90
	public int OriginalNumber { get; private set; }

	// Token: 0x17001657 RID: 5719
	// (get) Token: 0x06004190 RID: 16784 RVA: 0x000E9A99 File Offset: 0x000E7C99
	// (set) Token: 0x06004191 RID: 16785 RVA: 0x000E9AA1 File Offset: 0x000E7CA1
	public int NewNumber { get; private set; }
}
